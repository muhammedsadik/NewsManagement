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
using EasyAbp.FileManagement.Files;
using NewsManagement.Entities.Newses;
using NewsManagement.Entities.Videos;
using NewsManagement.Entities.Galleries;
using NewsManagement.EntityDtos.GalleryDtos;
using Microsoft.AspNetCore.Http.HttpResults;
using NewsManagement.EntityDtos.CategoryDtos;
using NewsManagement.EntityDtos.CityDtos;
using NewsManagement.EntityDtos.TagDtos;


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
    private readonly IListableContentTagRepository _listableContentTagRepository;
    private readonly IListableContentCityRepository _listableContentCityRepository;
    private readonly IListableContentCategoryRepository _listableContentCategoryRepository;
    private readonly IListableContentRelationRepository _listableContentRelationRepository;
    private readonly IFileRepository _fileRepository;

    protected ListableContentBaseManager(
      IObjectMapper objectMapper,
      ITagRepository tagRepository,
      ICityRepository cityRepository,
      INewsRepository newsRepository,
      IVideoRepository videoRepository,
      IGalleryRepository galleryRepository,
      ICategoryRepository categoryRepository,
      IListableContentGenericRepository<TEntity> genericRepository,
      IListableContentTagRepository listableContentTagRepository,
      IListableContentCityRepository listableContentCityRepository,
      IListableContentCategoryRepository listableContentCategoryRepository,
      IListableContentRelationRepository listableContentRelationRepository,
      IFileRepository fileRepository
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
      _fileRepository = fileRepository;
    }

    protected async Task<TEntity> CheckCreateInputBaseAsync(TEntityCreateDto createDto)
    {
      var isExist = await _genericRepository.AnyAsync(x => x.Title == createDto.Title);
      if (isExist)
        throw new AlreadyExistException(typeof(TEntity), createDto.Title);

      if (createDto.Status == StatusType.Deleted)
        throw new BusinessException(NewsManagementDomainErrorCodes.OnCreationStatusCannotBeDelete);

      if (createDto.ImageId != null)
        await _fileRepository.GetAsync((Guid)createDto.ImageId);

      var entity = _objectMapper.Map<TEntityCreateDto, TEntity>(createDto);

      string entityTypeName = typeof(TEntity).Name;
      entity.ListableContentType = (ListableContentType)Enum.Parse(typeof(ListableContentType), entityTypeName);

      await CheckTagByIdBaseAsync(createDto.TagIds);
      await CheckCityByIdBaseAsync(createDto.CityIds);

      if (createDto.RelatedListableContentIds != null)
        await CheckListableContentByIdBaseAsync(createDto.RelatedListableContentIds);

      await CheckListableContentCategoryBaseAsync(createDto.ListableContentCategoryDtos, entity.ListableContentType);
      CheckStatusAndDateTimeBaseAsync(createDto.Status, createDto.PublishTime);

      return entity;
    }

    protected async Task<TEntity> CheckUpdateInputBaseAsync(int id, TEntityUpdateDto updateDto)
    {
      var isExistId = await _genericRepository.AnyAsync(x => x.Id == id);
      if (!isExistId)
        throw new NotFoundException(typeof(TEntity), id.ToString());

      var isExist = await _genericRepository.AnyAsync(x => x.Title == updateDto.Title && x.Id != id);
      if (isExist)
        throw new AlreadyExistException(typeof(TEntity), updateDto.Title);

      var entity = _objectMapper.Map(updateDto, await _genericRepository.GetAsync(id));

      await CheckTagByIdBaseAsync(updateDto.TagIds);
      await CheckCityByIdBaseAsync(updateDto.CityIds);

      if (updateDto.RelatedListableContentIds != null)
      {
        var listableContentSelfReference = updateDto.RelatedListableContentIds.Any(x => x == id);
        if (listableContentSelfReference)
          throw new BusinessException(NewsManagementDomainErrorCodes.CannotAddItself);

        await CheckListableContentByIdBaseAsync(updateDto.RelatedListableContentIds);
      }

      await CheckListableContentCategoryBaseAsync(updateDto.ListableContentCategoryDtos, entity.ListableContentType);
      CheckStatusAndDateTimeBaseAsync(updateDto.Status, updateDto.PublishTime);

      if (entity.Status == StatusType.Deleted)// Bunun kontrollerini yap
        entity.IsDeleted = true;

      return entity;
    }

    protected async Task<PagedResultDto<TEntityDto>> GetListFilterBaseAsync(TPagedDto input)
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

      foreach (var entityDto in entityDtoList)
      {
        await GetCrossEntityAsync(entityDto);
      }

      return new PagedResultDto<TEntityDto>(totalCount, entityDtoList);
    }

    protected async Task CheckDeleteInputBaseAsync(int id)
    {
      var isExist = await _genericRepository.AnyAsync(t => t.Id == id);
      if (!isExist)
        throw new EntityNotFoundException(typeof(TEntity), id);
    }

    protected async Task CheckDeleteHardInputBaseAsync(int id)
    {
      var entity = await _genericRepository.GetAsync(id);

      await _genericRepository.HardDeleteAsync(entity);
    }

    protected async Task<TEntityDto> GetByIdBaseAsync(int id)
    {
      var entity = await _genericRepository.GetAsync(id);

      entity.ViewsCount += 1;

      await _genericRepository.UpdateAsync(entity);

      var entityDto = _objectMapper.Map<TEntity, TEntityDto>(entity);

      await GetCrossEntityAsync(entityDto);

      return entityDto;
    }

    #region Helper Method

    protected async Task CheckTagByIdBaseAsync(List<int> tagIds)
    {
      CheckDuplicateInputsBase(nameof(tagIds), tagIds);

      foreach (var tagId in tagIds)
      {
        var existTag = await _tagRepository.AnyAsync(t => t.Id == tagId);
        if (!existTag)
          throw new NotFoundException(typeof(Tag), tagId.ToString());
      }
    }

    protected async Task CheckCityByIdBaseAsync(List<int> cityIds)
    {
      CheckDuplicateInputsBase(nameof(cityIds), cityIds);

      foreach (var cityId in cityIds)
      {
        var existCity = await _cityRepository.AnyAsync(c => c.Id == cityId);
        if (!existCity)
          throw new NotFoundException(typeof(City), cityId.ToString());
      }
    }

    protected void CheckDuplicateInputsBase(string inputName, List<int> inputId)
    {
      var duplicates = inputId.GroupBy(x => x)
        .Where(u => u.Count() > 1).Select(u => u.Key).ToList();

      if (duplicates.Count > 0)
      {
        var duplicateUnits = string.Join(", ", duplicates);
        throw new BusinessException(NewsManagementDomainErrorCodes.RepeatedDataError)
          .WithData("0", inputName).WithData("1", string.Join(", ", duplicateUnits));
      }
    }

    protected void CheckStatusAndDateTimeBaseAsync(StatusType type, DateTime? dateTime)
    {
      if (type == StatusType.Draft && dateTime.HasValue)//eğer üzerinde çalışılıyor ise tarih olamaz
        throw new BusinessException(NewsManagementDomainErrorCodes.DraftStatusCannotHaveaPublishingTime);

      if (type == StatusType.Archived && dateTime.HasValue)//eğer Arşivlenmiş eski haberler ise tarih olamaz.
        throw new BusinessException(NewsManagementDomainErrorCodes.ArchivedStatusCannotHaveaPublishingTime);

      if (type == StatusType.Deleted && dateTime.HasValue)//eğer Silinmiş ise tarih olamaz
        throw new BusinessException(NewsManagementDomainErrorCodes.DeletedStatusCannotHaveaPublishingTime);

      if (type == StatusType.Published && !dateTime.HasValue)//eğer yayında ise tarih olmalı
        throw new BusinessException(NewsManagementDomainErrorCodes.SelectedStatusMustHaveaPublishingTime);

      //yayında ise tarih şimdi olmalı. sn göz ardı edildi
      if (type == StatusType.Published && dateTime.Value.ToString("yyyyMMddHHmm") != DateTime.Now.ToString("yyyyMMddHHmm"))
        throw new BusinessException(NewsManagementDomainErrorCodes.PublishedStatusDatetimeMustBeNow);

      if (type == StatusType.Scheduled && !dateTime.HasValue)//eğer planlanmış ise tarih olmalı
        throw new BusinessException(NewsManagementDomainErrorCodes.SelectedStatusMustHaveaPublishingTime);

      if (type == StatusType.Scheduled && dateTime.Value <= DateTime.Now)//eğer planlanmış ise tarih geri olamaz
        throw new BusinessException(NewsManagementDomainErrorCodes.ScheduledStatusDatetimeMustBeInTheFuture);

    }

    protected async Task CheckListableContentByIdBaseAsync(List<int> RelatedListableContentIds)
    {
      CheckDuplicateInputsBase(nameof(RelatedListableContentIds), RelatedListableContentIds);

      foreach (var ListableContentId in RelatedListableContentIds)
      {
        var gallery = await _galleryRepository.AnyAsync(x => x.Id == ListableContentId);
        var Video = await _videoRepository.AnyAsync(x => x.Id == ListableContentId);
        var News = await _newsRepository.AnyAsync(x => x.Id == ListableContentId);

        if (!gallery && !Video && !News)
          throw new NotFoundException(typeof(ListableContent), ListableContentId.ToString());
      }
    }

    protected async Task CheckListableContentCategoryBaseAsync(List<ListableContentCategoryDto> listableContentCategoryDto, ListableContentType type)
    {
      var categoryIds = listableContentCategoryDto.Select(x => x.CategoryId).ToList();

      CheckDuplicateInputsBase(nameof(categoryIds), categoryIds);

      var categories = await _categoryRepository.GetListAsync(c => categoryIds.Contains(c.Id));

      var isExistCategoyIds = categoryIds.Except(categories.Select(c => c.Id)).ToList();
      if (isExistCategoyIds.Any())
        throw new NotFoundException(typeof(ListableContentCategory), string.Join(", ", isExistCategoyIds));

      var parentCategoryIds = categories.Where(c => c.ParentCategoryId == null).Select(x => x.Id).ToList();

      var missingCategoryIds = categories.Where(c => c.ParentCategoryId != null && !parentCategoryIds.Contains(c.ParentCategoryId.Value)).Select(x => x.Id).ToList();

      var categoriesWithNoParent = categories.Where(c => c.ParentCategoryId != null && !parentCategoryIds.Contains(c.ParentCategoryId.Value))
    .Select(c => c.Id)
    .ToList();

      if (categories.Any(x => x.listableContentType != type))
        throw new BusinessException(NewsManagementDomainErrorCodes.MustHaveTheSameContentType);

      if (listableContentCategoryDto.Count(x => x.IsPrimary) != 1)
        throw new BusinessException(NewsManagementDomainErrorCodes.OnlyOneCategoryIsActiveStatusTrue)
          .WithData("0", listableContentCategoryDto.Count(x => x.IsPrimary));

      if (missingCategoryIds.Any())
        throw new BusinessException(NewsManagementDomainErrorCodes.WithoutParentCategory)
          .WithData("categoryId", string.Join(", ", missingCategoryIds));

    }

    #endregion

    #region CreateListableContent Cross entity

    protected async Task CreateCrossEntity(TEntityCreateDto createDto, int listableContentId)
    {
      await CreateListableContentTagBaseAsync(createDto.TagIds, listableContentId);
      await CreateListableContentCityBaseAsync(createDto.CityIds, listableContentId);
      await CreateListableContentCategoryBaseAsync(createDto.ListableContentCategoryDtos, listableContentId);

      if (createDto.RelatedListableContentIds != null)
        await CreateListableContentRelationBaseAsync(createDto.RelatedListableContentIds, listableContentId);
    }

    protected async Task ReCreateCrossEntity(TEntityUpdateDto updateDto, int listableContentId)
    {
      await DeleteAllCrossEntitiesByListableContentId(listableContentId);

      await CreateListableContentTagBaseAsync(updateDto.TagIds, listableContentId);
      await CreateListableContentCityBaseAsync(updateDto.CityIds, listableContentId);
      await CreateListableContentCategoryBaseAsync(updateDto.ListableContentCategoryDtos, listableContentId);

      if (updateDto.RelatedListableContentIds != null)
        await CreateListableContentRelationBaseAsync(updateDto.RelatedListableContentIds, listableContentId);
    }

    protected async Task GetCrossEntityAsync(TEntityDto entityDto)
    {
      var tags = await _listableContentTagRepository.GetCrossListAsync(entityDto.Id);
      var cities = await _listableContentCityRepository.GetCrossListAsync(entityDto.Id);
      var relations = await _listableContentRelationRepository.GetCrossListAsync(entityDto.Id);
      var categiries = await _listableContentCategoryRepository.GetCrossListAsync(entityDto.Id);

      var returnTagDto = _objectMapper.Map<List<ListableContentTag>, List<ReturnTagDto>>(tags);
      var returnCityDto = _objectMapper.Map<List<ListableContentCity>, List<ReturnCityDto>>(cities);
      var returnCategoryDto = _objectMapper.Map<List<ListableContentCategory>, List<ReturnCategoryDto>>(categiries);
      var returnRelationDto = _objectMapper.Map<List<ListableContentRelation>, List<ReturnListableContentRelationDto>>(relations);

      entityDto.Tags = returnTagDto;
      entityDto.Cities = returnCityDto;
      entityDto.Categories = returnCategoryDto;
      entityDto.ListableContentRelations = returnRelationDto;
    }

    protected async Task DeleteAllCrossEntitiesByListableContentId(int listableContentId)
    {
      await _listableContentCategoryRepository.DeleteAsync(x => x.ListableContentId == listableContentId);
      await _listableContentCityRepository.DeleteAsync(x => x.ListableContentId == listableContentId);
      await _listableContentTagRepository.DeleteAsync(x => x.ListableContentId == listableContentId);
      await _listableContentRelationRepository.DeleteAsync(x => x.ListableContentId == listableContentId);
    }

    protected async Task CreateListableContentTagBaseAsync(List<int> tagIds, int listableContentId)
    {
      List<ListableContentTag> listableContentTags = new();
      foreach (var tagId in tagIds)
      {
        var tag = new ListableContentTag { TagId = tagId, ListableContentId = listableContentId };
        listableContentTags.Add(tag);
      }

      await _listableContentTagRepository.InsertManyAsync(listableContentTags, autoSave: true);
    }

    protected async Task CreateListableContentCityBaseAsync(List<int> cityIds, int listableContentId)
    {
      List<ListableContentCity> listableContentCitis = new();
      foreach (var cityId in cityIds)
      {
        var city = new ListableContentCity { CityId = cityId, ListableContentId = listableContentId };
        listableContentCitis.Add(city);
      }

      await _listableContentCityRepository.InsertManyAsync(listableContentCitis, autoSave: true);
    }

    protected async Task CreateListableContentRelationBaseAsync(List<int> RelatedListableContentIds, int listableContentId)
    {
      List<ListableContentRelation> listableContentRelations = new();
      foreach (var RelatedId in RelatedListableContentIds)
      {
        var listableContentRelation = new ListableContentRelation
        {
          ListableContentId = listableContentId,
          RelatedListableContentId = RelatedId
        };

        listableContentRelations.Add(listableContentRelation);
      }

      await _listableContentRelationRepository.InsertManyAsync(listableContentRelations, autoSave: true);
    }

    protected async Task CreateListableContentCategoryBaseAsync(List<ListableContentCategoryDto> listableContentCategoryDto, int listableContentId)
    {
      List<ListableContentCategory> listableContentCategories = new();
      foreach (var item in listableContentCategoryDto)
      {
        var category = new ListableContentCategory { ListableContentId = listableContentId, CategoryId = item.CategoryId, IsPrimary = item.IsPrimary };
        listableContentCategories.Add(category);
      }

      await _listableContentCategoryRepository.InsertManyAsync(listableContentCategories, autoSave: true);
    }

    protected async Task ReCreateListableContentRelationBaseAsync(List<int> RelatedListableContentIds, int listableContentId)
    {
      var isExist = await _listableContentRelationRepository.GetListAsync(x => x.ListableContentId == listableContentId);

      if (isExist.Count() != 0)
        await _listableContentRelationRepository.DeleteManyAsync(isExist, autoSave: true);


      await CreateListableContentRelationBaseAsync(RelatedListableContentIds, listableContentId);
    }


    #endregion

  }
}