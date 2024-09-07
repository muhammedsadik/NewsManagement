using NewsManagement.Entities.ListableContentRelations;
using NewsManagement.EntityConsts.ListableContentConsts;
using NewsManagement.EntityDtos.ListableContentDtos;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;
using NewsManagement.Entities.Cities;
using NewsManagement.Entities.Exceptions;
using NewsManagement.EntityDtos.CityDtos;
using Volo.Abp;
using NewsManagement.Entities.Galleries;

namespace NewsManagement.Entities.ListableContents
{
  public class ListableContentManager : DomainService
  {
    private readonly IObjectMapper _objectMapper;
    private readonly IRepository<GalleryImage> _galleryImageRepository;
    private readonly IListableContentRepository _listableContentRepository;
    private readonly IRepository<ListableContentTag> _listableContentTagRepository;
    private readonly IRepository<ListableContentCity> _listableContentCityRepository;
    private readonly IRepository<ListableContentCategory> _listableContentCategoryRepository;
    private readonly IRepository<ListableContentRelation> _listableContentRelationRepository;

    public ListableContentManager(
      IObjectMapper objectMapper,
      IRepository<GalleryImage> galleryImageRepository,
      IListableContentRepository listableContentRepository,
      IRepository<ListableContentTag> listableContentTagRepository,
      IRepository<ListableContentCity> listableContentCityRepository,
      IRepository<ListableContentCategory> listableContentCategoryRepository,
      IRepository<ListableContentRelation> listableContentRelationRepository
      )
    {
      _objectMapper = objectMapper;
      _galleryImageRepository = galleryImageRepository;
      _listableContentRepository = listableContentRepository;
      _listableContentTagRepository = listableContentTagRepository;
      _listableContentCityRepository = listableContentCityRepository;
      _listableContentCategoryRepository = listableContentCategoryRepository;
      _listableContentRelationRepository = listableContentRelationRepository;
    }

    public async Task<ListableContentWithCrossDto> GetByIdAsync(int id)
    {
      var listableContent = await _listableContentRepository.GetAsync(id);
      var item = _objectMapper.Map<ListableContent, ListableContentWithCrossDto>(listableContent);

      var listableContentType = listableContent.GetType().Name;
      if (listableContentType == "Gallery")
      {


        var galleryImage = await _galleryImageRepository.GetListAsync(x => x.GalleryId == id);
        //gallery.GalleryImages = galleryImage;

  



        var queryableGalleryImage = await _galleryImageRepository.GetQueryableAsync();
        var tagIddds = queryableGalleryImage.Where(x => x.GalleryId == id).Select(x => x.ImageId).ToList();
      }

      var queryableTag = await _listableContentTagRepository.GetQueryableAsync();
      var tagIds = queryableTag.Where(x => x.ListableContentId == id).Select(x => x.TagId).ToList();
      
      var queryableCity = await _listableContentCityRepository.GetQueryableAsync();
      var cityIds = queryableCity.Where(x => x.ListableContentId == id).Select(x => x.CityId).ToList();


      var queryableCategory = await _listableContentCategoryRepository.GetQueryableAsync();
      var categoryIds = queryableCategory.Where(x => x.ListableContentId == id).Select(x => x.CategoryId).ToList();

      var queryableRelation = await _listableContentRelationRepository.GetQueryableAsync();
      var relatedListableContentIds = queryableRelation.Where(x => x.ListableContentId == id).Select(x => x.RelatedListableContentId).ToList();
    
      
      item.CityIds = cityIds;
      item.CategoryIds = categoryIds;
      item.TagIds = tagIds;
      item.RelatedListableContentIds = relatedListableContentIds;

      return item;
    }

    public async Task<List<ListableContentDto>> GetByContentTypeAsync(ListableContentType type)
    {
      var ListableContents = await _listableContentRepository.GetListAsync(x => x.ListableContentType == type);

      var listableContentDtos = _objectMapper.Map<List<ListableContent>, List<ListableContentDto>>(ListableContents);

      return listableContentDtos;
    }

    public async Task<ListableContentWithRelationDto> GetByIdWithRelationAsync(int id)
    {
      var listableContent = await _listableContentRepository.GetAsync(id);

      var queryableRelation = await _listableContentRelationRepository.GetQueryableAsync();
      var relationContentIds = queryableRelation.Where(x => x.ListableContentId == id).Select(x => x.RelatedListableContentId).ToList();

      var listableContents = await _listableContentRepository.GetListAsync(x => relationContentIds.Contains(x.Id));

      var items = _objectMapper.Map<ListableContent, ListableContentWithRelationDto>(listableContent);
      var listableContentDtos = _objectMapper.Map<List<ListableContent>, List<ListableContentDto>>(listableContents);

      items.ListableContents = listableContentDtos;

      return items;
    }

    public async Task<PagedResultDto<ListableContentDto>> GetListAsync(GetListPagedAndSortedDto input)
    {

      var totalCount = input.Filter == null
       ? await _listableContentRepository.CountAsync()
       : await _listableContentRepository.CountAsync(c => c.Title.Contains(input.Filter));

      if (totalCount == 0)
        throw new NotFoundException(typeof(ListableContent), input.Filter ?? string.Empty);

      if (input.SkipCount >= totalCount)
        throw new BusinessException(NewsManagementDomainErrorCodes.FilterLimitsError);

      if (input.Sorting.IsNullOrWhiteSpace())
        input.Sorting = nameof(ListableContent.Title);

      var listableContents = await _listableContentRepository.GetListAsync(input.SkipCount, input.MaxResultCount, input.Sorting, input.Filter);

      var listableContentDtos = _objectMapper.Map<List<ListableContent>, List<ListableContentDto>>(listableContents);

      return new PagedResultDto<ListableContentDto>(totalCount, listableContentDtos);


    }

    private async Task

  }
}
