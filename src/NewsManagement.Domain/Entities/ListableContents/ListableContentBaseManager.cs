using NewsManagement.Entities.Categories;
using NewsManagement.Entities.Cities;
using NewsManagement.Entities.Exceptions;
using NewsManagement.Entities.ListableContentRelations;
using NewsManagement.Entities.Tags;
using NewsManagement.EntityConsts.ListableContentConsts;
using NewsManagement.EntityDtos.ListableContentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;
using static NewsManagement.Permissions.NewsManagementPermissions;

namespace NewsManagement.Entities.ListableContents
{
  public abstract class ListableContentBaseManager<TEntity, TEntityDto, TPagedDto, TEntityCreateDto, TEntityUpdateDto> : DomainService
    where TEntity : ListableContent
    where TEntityDto : ListableContentDto
    where TEntityCreateDto : CreateListableContentDto
    where TEntityUpdateDto : UpdateListableContentDto
  {
    private readonly IObjectMapper _objectMapper;
    private readonly IRepository<TEntity, int> _repository;
    private readonly ITagRepository _tagRepository;
    private readonly ICityRepository _cityRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IRepository<ListableContent, int> _listableContentRepository;

    protected ListableContentBaseManager(
      IRepository<TEntity, int> repository,
      IObjectMapper objectMapper,
      ITagRepository tagRepository,
      ICityRepository cityRepository,
      IRepository<ListableContent, int> listableContentRepository,
      ICategoryRepository categoryRepository
    )
    {
      _repository = repository;
      _objectMapper = objectMapper;
      _tagRepository = tagRepository;
      _cityRepository = cityRepository;
      _listableContentRepository = listableContentRepository;
      _categoryRepository = categoryRepository;
    }




    public async Task CreateBaseAsync(TEntityCreateDto createDto)
    {
      var isExist = await _repository.AnyAsync(x => x.Title == createDto.Title);
      if (isExist)
        throw new AlreadyExistException(typeof(TEntityDto), createDto.Title);
      
      await CheckTagByIdAsync(createDto.TagIds);

      if (createDto.CityIds != null)
        await CheckCityByIdAsync(createDto.CityIds);

      if (createDto.RelatedListableContentIds != null)
        await CheckListableContentByIdAsync(createDto.RelatedListableContentIds);

      await CheckListableContentCategoryAsync(createDto.ListableContentCategoryDtos);

    }

    #region Helper Method

    public async Task CheckTagByIdAsync(int[] tagIds)
    {
      CheckDuplicateInputs(nameof(tagIds), tagIds);

      foreach (var tagId in tagIds)
      {
        var existTag = await _tagRepository.AnyAsync(t => t.Id == tagId);
        if (!existTag)
          throw new NotFoundException(typeof(Tag), tagId.ToString());
      }
    }

    public async Task CheckCityByIdAsync(int[] cityIds)
    {
      CheckDuplicateInputs(nameof(cityIds), cityIds);

      foreach (var cityId in cityIds)
      {
        var existCity = await _cityRepository.AnyAsync(c => c.Id == cityId);
        if (!existCity)
          throw new NotFoundException(typeof(Tag), cityId.ToString());
      }
    }

    public void CheckDuplicateInputs(string inputName, int[] inputId)
    {
      var duplicates = inputId.GroupBy(x => x)
        .Where(u => u.Count() > 1).Select(u => u.Key).ToList();

      if (duplicates.Count > 0)
      {
        var duplicateUnits = string.Join(", ", duplicates);
        throw new BusinessException(NewsManagementDomainErrorCodes.RepeatedDataError)// 📩
          .WithData("index", inputName)
          .WithData("repeat", duplicateUnits);
      }
    }

    public async Task CheckListableContentByIdAsync(int[] RelatedListableContentIds)
    {
      CheckDuplicateInputs(nameof(RelatedListableContentIds), RelatedListableContentIds);

      foreach (var ListableContentId in RelatedListableContentIds)
      {
        var existListableContent = await _listableContentRepository.AnyAsync(l => l.Id == ListableContentId);
        if (!existListableContent)
          throw new NotFoundException(typeof(ListableContent), ListableContentId.ToString());
      }
    }

    public async Task CheckStatusAndDateTimeAsync(StatusType type, DateTime? dateTime)
    {
      if (type == StatusType.Draft && !dateTime.HasValue)//eğer üzerinde çalışılıyor ise tarih olamaz
        throw new BusinessException(NewsManagementDomainErrorCodes.NotInVideoEnumType);// 📩

      if (type == StatusType.PendingReview && !dateTime.HasValue)//eğer onay bekliyor ise tarih olamaz
        throw new BusinessException(NewsManagementDomainErrorCodes.NotInVideoEnumType);// 📩

      if (type == StatusType.Archived && !dateTime.HasValue)//eğer Arşivlenmiş eski haberler ise tarih olamaz.
        throw new BusinessException(NewsManagementDomainErrorCodes.NotInVideoEnumType);// 📩

      if (type == StatusType.Rejected && !dateTime.HasValue)//eğer Reddedilmiş ise tarih olamaz.
        throw new BusinessException(NewsManagementDomainErrorCodes.NotInVideoEnumType);// 📩

      if (type == StatusType.Deleted && !dateTime.HasValue)//eğer Silinmiş ise tarih olamaz
        throw new BusinessException(NewsManagementDomainErrorCodes.NotInVideoEnumType);// 📩

      if (!dateTime.HasValue)
        dateTime = DateTime.Now;

      if (type == StatusType.Published && dateTime.Value > DateTime.Now)//eğer yayında ise tarih ileri olamaz.⚠yayına alınan içerik için zamanı yönet⚠ 
        throw new BusinessException(NewsManagementDomainErrorCodes.IfStatusPublishedDatetimeMustNowOrNull);// 📩

      if (type == StatusType.Scheduled && dateTime.Value <= DateTime.Now)//eğer planlanmış ise tarih geri olamaz
        throw new BusinessException(NewsManagementDomainErrorCodes.NotInVideoEnumType);// 📩

      if (type == StatusType.Scheduled && dateTime.Value > DateTime.Now)//eğer planlanmış ise tarihe göre işleme alınacak
      {
        //Burada background Job kullanılacak
      }
    }

    public async Task CheckListableContentCategoryAsync(List<ListableContentCategoryDto> listableContentCategoryDto)
    {
      var categoryIds = listableContentCategoryDto.Select(x => x.CategoryId).ToArray();

      CheckDuplicateInputs(nameof(categoryIds), categoryIds);

      foreach (var categoryId in categoryIds)
      {
        var existTag = await _categoryRepository.AnyAsync(t => t.Id == categoryId);
        if (!existTag)
          throw new NotFoundException(typeof(ListableContentCategory), categoryId.ToString());// 📢 📩
      }

      if(listableContentCategoryDto.Count(x => x.IsPrimary) != 1)
        throw new BusinessException();// validation da bu durum için error message yazdık  📢 📩
    }

    #endregion




    public async Task<TEntityDto> UpdateBaseAsync(int id, TEntityUpdateDto updateDto)
    {
      var existingContent = await _repository.GetAsync(id);

      var isExisting = await _repository.AnyAsync(x => x.Title == updateDto.Title && x.Id != id);
      if (isExisting)
        throw new AlreadyExistException(typeof(TEntity), updateDto.Title);

      _objectMapper.Map(updateDto, existingContent);

      var updatedContent = await _repository.UpdateAsync(existingContent);

      var updatedDto = _objectMapper.Map<TEntity, TEntityDto>(updatedContent);

      return updatedDto;
    }

    public async Task<PagedResultDto<TEntityDto>> GetListBaseAsync(TPagedDto input)
    {
      return new PagedResultDto<TEntityDto>();
    }

    public async Task DeleteBaseAsync(int id)
    {
      var isExist = await _repository.AnyAsync(t => t.Id == id);
      if (!isExist)
        throw new EntityNotFoundException(typeof(TEntity), id);
    }

    public async Task DeleteHardBaseAsync(int id)
    {

    }



  }
}

