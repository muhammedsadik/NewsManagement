using NewsManagement.Entities.Categories;
using NewsManagement.Entities.Cities;
using NewsManagement.Entities.Exceptions;
using NewsManagement.Entities.ListableContentRelations;
using NewsManagement.Entities.Tags;
using NewsManagement.EntityConsts.ListableContentConsts;
using NewsManagement.EntityDtos.ListableContentDtos;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
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
using System.Linq.Dynamic.Core;
using static System.Runtime.InteropServices.JavaScript.JSType;
using EasyAbp.FileManagement.Files;
using NewsManagement.Entities.Newses;
using NewsManagement.Entities.Videos;
using NewsManagement.Entities.Galleries;

namespace NewsManagement.Entities.ListableContents
{
  public abstract class ListableContentBaseManager<TEntity, TEntityDto, TPagedDto, TEntityCreateDto, TEntityUpdateDto> : DomainService
    where TEntity : ListableContent, new()
    where TEntityDto : ListableContentDto
    where TEntityCreateDto : CreateListableContentDto
    where TEntityUpdateDto : UpdateListableContentDto
    where TPagedDto : GetListPagedAndSortedDto
  {
    private readonly IObjectMapper _objectMapper;
    private readonly ITagRepository _tagRepository;
    private readonly ICityRepository _cityRepository; 
    private readonly INewsRepository _newsRepository;
    private readonly IVideoRepository _videoRepository;
    private readonly IGalleryRepository _galleryRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IListableContentGenericRepository<TEntity> _genericRepository;
    private readonly IRepository<ListableContentTag> _listableContentTagRepository;
    private readonly IRepository<ListableContentCity> _listableContentCityRepository;
    private readonly IRepository<ListableContentCategory> _listableContentCategoryRepository;
    private readonly IRepository<ListableContentRelation> _listableContentRelationRepository;
    //private readonly FileAppService _fileAppService;

    protected ListableContentBaseManager(
      IObjectMapper objectMapper,
      ITagRepository tagRepository,
      ICityRepository cityRepository,
      INewsRepository newsRepository,
      IVideoRepository videoRepository,
      IGalleryRepository galleryRepository,
      ICategoryRepository categoryRepository,
      IListableContentGenericRepository<TEntity> genericRepository,
      IRepository<ListableContentTag> listableContentTagRepository,
      IRepository<ListableContentCity> listableContentCityRepository,
      IRepository<ListableContentCategory> listableContentCategoryRepository,
      IRepository<ListableContentRelation> listableContentRelationRepository
    //FileAppService fileAppService
    )
    {
      _objectMapper = objectMapper;
      _tagRepository = tagRepository;
      _cityRepository = cityRepository;
      _newsRepository = newsRepository;
      _videoRepository = videoRepository;
      _galleryRepository = galleryRepository;
      _genericRepository = genericRepository;
      _categoryRepository = categoryRepository;
      _listableContentTagRepository = listableContentTagRepository;
      _listableContentCityRepository = listableContentCityRepository;
      _listableContentCategoryRepository = listableContentCategoryRepository;
      _listableContentRelationRepository = listableContentRelationRepository;
      //_fileAppService = fileAppService;
    }

    public async Task CheckCreateInputBaseAsync(TEntityCreateDto createDto)
    {
      var isExist = await _genericRepository.AnyAsync(x => x.Title == createDto.Title);
      if (isExist)
        throw new AlreadyExistException(typeof(TEntity), createDto.Title);

      //var isExistImageId = await _fileAppService.GetAsync(createDto.ImageId);
      //if (isExistImageId)
      //  throw new AlreadyExistException(typeof(TEntityDto), createDto.Title);


      await CheckTagByIdBaseAsync(createDto.TagIds);

      await CheckCityByIdBaseAsync(createDto.CityIds);

      await CheckListableContentCategoryBaseAsync(createDto.ListableContentCategoryDtos);

      if (createDto.RelatedListableContentIds != null)
        await CheckListableContentByIdBaseAsync(createDto.RelatedListableContentIds);

    }

    public async Task CheckUpdateInputBaseAsync(int id, TEntityUpdateDto updateDto)
    {
      var isExist = await _genericRepository.AnyAsync(x => x.Title == updateDto.Title && x.Id != id);
      if (isExist)
        throw new AlreadyExistException(typeof(TEntity), updateDto.Title);

      await CheckTagByIdBaseAsync(updateDto.TagIds);

      await CheckCityByIdBaseAsync(updateDto.CityIds);

      await CheckListableContentCategoryBaseAsync(updateDto.ListableContentCategoryDtos);

      await CheckStatusAndDateTimeBaseAsync(updateDto.Status, updateDto.PublishTime);

      if (updateDto.RelatedListableContentIds != null)
        await CheckListableContentByIdBaseAsync(updateDto.RelatedListableContentIds);

    }

    public async Task<PagedResultDto<TEntityDto>> GetListFilterBaseAsync(TPagedDto input)
    {
      var totalCount = input.Filter == null
         ? await _genericRepository.CountAsync()
         : await _genericRepository.CountAsync(c => c.Title.Contains(input.Filter));

      if (totalCount == 0)
        throw new NotFoundException(typeof(TEntity), input.Filter ?? string.Empty);

      if (input.SkipCount >= totalCount)
        throw new BusinessException(NewsManagementDomainErrorCodes.FilterLimitsError);

      if (input.Sorting.IsNullOrWhiteSpace())
        input.Sorting = nameof(ListableContent.Title);

      var entityList = await _genericRepository.GetListAsync(input.SkipCount, input.MaxResultCount, input.Sorting, input.Filter);

      var entityDtoList = _objectMapper.Map<List<TEntity>, List<TEntityDto>>(entityList);

      return new PagedResultDto<TEntityDto>(totalCount, entityDtoList);
    }

    public async Task CheckDeleteInputBaseAsync(int id)
    {
      var isExist = await _genericRepository.AnyAsync(t => t.Id == id);
      if (!isExist)
        throw new EntityNotFoundException(typeof(TEntity), id);
    }

    public async Task CheckDeleteHardInputBaseAsync(int id)
    {
      var entity = await _genericRepository.GetAsync(id);

      await _genericRepository.HardDeleteAsync(entity);
    }

    #region Helper Method

    public async Task CheckTagByIdBaseAsync(List<int> tagIds)
    {
      CheckDuplicateInputsBase(nameof(tagIds), tagIds);

      foreach (var tagId in tagIds)
      {
        var existTag = await _tagRepository.AnyAsync(t => t.Id == tagId);
        if (!existTag)
          throw new NotFoundException(typeof(Tag), tagId.ToString());
      }
    }

    public async Task CheckCityByIdBaseAsync(List<int> cityIds)
    {
      CheckDuplicateInputsBase(nameof(cityIds), cityIds);

      foreach (var cityId in cityIds)
      {
        var existCity = await _cityRepository.AnyAsync(c => c.Id == cityId);
        if (!existCity)
          throw new NotFoundException(typeof(City), cityId.ToString());
      }
    }

    public void CheckDuplicateInputsBase(string inputName, List<int> inputId)
    {
      var duplicates = inputId.GroupBy(x => x)
        .Where(u => u.Count() > 1).Select(u => u.Key).ToList();

      if (duplicates.Count > 0)
      {
        var duplicateUnits = string.Join(", ", duplicates);
        throw new BusinessException(NewsManagementDomainErrorCodes.RepeatedDataError)// 📩 çalışmasını test etmek için vlidation kapat
          .WithData("index", inputName)
          .WithData("repeat", duplicateUnits);
      }
    }

    public async Task CheckListableContentByIdBaseAsync(List<int> RelatedListableContentIds)
    {
      CheckDuplicateInputsBase(nameof(RelatedListableContentIds), RelatedListableContentIds);

      foreach (var ListableContentId in RelatedListableContentIds)
      {
        var galleryId = await _galleryRepository.AnyAsync(x => x.Id == ListableContentId);
        var VideoId = await _videoRepository.AnyAsync(x => x.Id == ListableContentId);
        var NewsId = await _newsRepository.AnyAsync(x => x.Id == ListableContentId);

        if (galleryId || VideoId || NewsId)
          throw new NotFoundException(typeof(ListableContent), ListableContentId.ToString());
      }
    }

    public async Task CheckStatusAndDateTimeBaseAsync(StatusType type, DateTime? dateTime)
    {
      if (type == StatusType.Draft && dateTime.HasValue)//eğer üzerinde çalışılıyor ise tarih olamaz
        throw new BusinessException(NewsManagementDomainErrorCodes.DraftStatusCannotHaveaPublishingTime);

      if (type == StatusType.PendingReview && dateTime.HasValue)//eğer onay bekliyor ise tarih olamaz
        throw new BusinessException(NewsManagementDomainErrorCodes.PendingReviewStatusCannotHaveaPublishingTime);

      if (type == StatusType.Archived && dateTime.HasValue)//eğer Arşivlenmiş eski haberler ise tarih olamaz.
        throw new BusinessException(NewsManagementDomainErrorCodes.ArchivedStatusCannotHaveaPublishingTime);

      if (type == StatusType.Rejected && dateTime.HasValue)//eğer Reddedilmiş ise tarih olamaz.
        throw new BusinessException(NewsManagementDomainErrorCodes.RejectedStatusCannotHaveaPublishingTime);

      if (type == StatusType.Deleted && dateTime.HasValue)//eğer Silinmiş ise tarih olamaz
        throw new BusinessException(NewsManagementDomainErrorCodes.DeletedStatusCannotHaveaPublishingTime);

      if (type == StatusType.Published && !dateTime.HasValue)//eğer yayında ise tarih olmalı
        throw new BusinessException(NewsManagementDomainErrorCodes.PublishedStatusMustHaveaPublishingTime);

      if (!dateTime.HasValue) // veri tabanına birşeyler kaydetmek gerekir.
        dateTime = DateTime.Now;

      if (type == StatusType.Published && dateTime.Value < DateTime.Now.AddHours(-1))//eğer yayında ise tarih en fazla şimdi den 1 saat önce olabilir ⚠ 
        throw new BusinessException(NewsManagementDomainErrorCodes.PublishedStatusDatetimeTimeoutError);

      if (type == StatusType.Published && dateTime.Value > DateTime.Now)//eğer yayında ise tarih ileri olamaz.⚠yayına alınan içerik için zamanı yönet⚠ 
        throw new BusinessException(NewsManagementDomainErrorCodes.PublishedStatusDatetimeMustNowOrNull);

      if (type == StatusType.Scheduled && dateTime.Value <= DateTime.Now)//eğer planlanmış ise tarih geri olamaz
        throw new BusinessException(NewsManagementDomainErrorCodes.ScheduledStatusDatetimeMustBeInTheFuture);

      if (type == StatusType.Scheduled && dateTime.Value > DateTime.Now)//eğer planlanmış ise tarihe göre işleme alınacak
      {
        //Burada background Job kullanılacak
      }
    }

    public async Task CheckListableContentCategoryBaseAsync(List<ListableContentCategoryDto> listableContentCategoryDto)
    {
      var categoryIds = listableContentCategoryDto.Select(x => x.CategoryId).ToList();

      CheckDuplicateInputsBase(nameof(categoryIds), categoryIds);

      foreach (var categoryId in categoryIds)
      {
        var existCategory = await _categoryRepository.AnyAsync(t => t.Id == categoryId);
        if (!existCategory)
          throw new NotFoundException(typeof(ListableContentCategory), categoryId.ToString());
      }

      if (listableContentCategoryDto.Count(x => x.IsPrimary) != 1)
        throw new BusinessException(NewsManagementDomainErrorCodes.OnlyOneCategoryIsActiveStatusTrue)
          .WithData("0", listableContentCategoryDto.Count(x => x.IsPrimary));
    }
    
    #endregion

    #region CreateListableContentSubs

    public async Task CreateListableContentTagBaseAsync(List<int> tagIds, int listableContentId)
    {
      foreach (var tagId in tagIds)
      {
        await _listableContentTagRepository.InsertAsync(new() { ListableContentId = listableContentId, TagId = tagId }, autoSave: true);
      }
    }

    public async Task CreateListableContentCityBaseAsync(List<int> cityIds, int listableContentId)
    {
      foreach (var cityId in cityIds)
      {
        await _listableContentCityRepository.InsertAsync(new() { ListableContentId = listableContentId, CityId = cityId }, autoSave: true);
      }
    }

    public async Task CreateListableContentCategoryBaseAsync(List<ListableContentCategoryDto> listableContentCategoryDto, int listableContentId)
    {
      foreach (var item in listableContentCategoryDto)
      {
        await _listableContentCategoryRepository.InsertAsync(new()
        { ListableContentId = listableContentId, CategoryId = item.CategoryId, IsPrimary = item.IsPrimary }, autoSave: true);
      }
    }

    public async Task CreateListableContentRelationBaseAsync(List<int> RelatedListableContentIds, int listableContentId)
    {
      foreach (var RelatedId in RelatedListableContentIds)
      {
        await _listableContentRelationRepository.InsertAsync(new()
        {
          ListableContentId = listableContentId,
          RelatedListableContentId = RelatedId
        }, autoSave: true);
      }
    }

    public async Task ReCreateListableContentTagBaseAsync(List<int> tagIds, int listableContentId)
    {
      var isExist = await _listableContentTagRepository.GetListAsync(x => x.ListableContentId == listableContentId);

      if (isExist.Count() != 0)
      {
        await _listableContentTagRepository.DeleteManyAsync(isExist, autoSave: true);
      }

      await CreateListableContentTagBaseAsync(tagIds, listableContentId);
    }

    public async Task ReCreateListableContentCityBaseAsync(List<int> cityIds, int listableContentId)
    {
      var isExist = await _listableContentCityRepository.GetListAsync(x => x.ListableContentId == listableContentId);

      if (isExist.Count() != 0)
      {
        await _listableContentCityRepository.DeleteManyAsync(isExist, autoSave: true);
      }

      await CreateListableContentTagBaseAsync(cityIds, listableContentId);
    }

    public async Task ReCreateListableContentCategoryBaseAsync(List<ListableContentCategoryDto> listableContentCategoryDto, int listableContentId)
    {
      var isExist = await _listableContentCategoryRepository.GetListAsync(x => x.ListableContentId == listableContentId);

      if (isExist.Count() != 0)
      {
        await _listableContentCategoryRepository.DeleteManyAsync(isExist, autoSave: true);
      }

      await CreateListableContentCategoryBaseAsync(listableContentCategoryDto, listableContentId);
    }

    public async Task ReCreateListableContentRelationBaseAsync(List<int> RelatedListableContentIds, int listableContentId)
    {
      var isExist = await _listableContentRelationRepository.GetListAsync(x => x.ListableContentId == listableContentId);

      if (isExist.Count() != 0)
      {
        await _listableContentRelationRepository.DeleteManyAsync(isExist, autoSave: true);
      }

      await CreateListableContentRelationBaseAsync(RelatedListableContentIds, listableContentId);
    }

    #endregion




  }
}

