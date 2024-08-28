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

namespace NewsManagement.Entities.ListableContents
{
  public class ListableContentManager : DomainService
  {
    private readonly IObjectMapper _objectMapper;
    private readonly IListableContentRepository _listableContentRepository;
    private readonly IRepository<ListableContentTag> _listableContentTagRepository;
    private readonly IRepository<ListableContentCity> _listableContentCityRepository;
    private readonly IRepository<ListableContentCategory> _listableContentCategoryRepository;
    private readonly IRepository<ListableContentRelation> _listableContentRelationRepository;

    public ListableContentManager(
      IObjectMapper objectMapper,
      IListableContentRepository listableContentRepository,
      IRepository<ListableContentTag> listableContentTagRepository,
      IRepository<ListableContentCity> listableContentCityRepository,
      IRepository<ListableContentCategory> listableContentCategoryRepository,
      IRepository<ListableContentRelation> listableContentRelationRepository
      )
    {
      _objectMapper = objectMapper;
      _listableContentRepository = listableContentRepository;
      _listableContentTagRepository = listableContentTagRepository;
      _listableContentCityRepository = listableContentCityRepository;
      _listableContentCategoryRepository = listableContentCategoryRepository;
      _listableContentRelationRepository = listableContentRelationRepository;
    }

    public async Task<ListableContentWithCrossDto> GetByIdAsync(int id)
    {
      var ListableContent = await _listableContentRepository.GetAsync(id);

      var queryableTag = await _listableContentTagRepository.GetQueryableAsync();
      var tagIds = queryableTag.Where(x => x.ListableContentId == id).Select(x => x.TagId).ToList();
      
      var queryableCity = await _listableContentCityRepository.GetQueryableAsync();
      var cityIds = queryableCity.Where(x => x.ListableContentId == id).Select(x => x.CityId).ToList();


      var queryableCategory = await _listableContentCategoryRepository.GetQueryableAsync();
      var categoryIds = queryableCategory.Where(x => x.ListableContentId == id).Select(x => x.CategoryId).ToList();

      var queryableRelation = await _listableContentRelationRepository.GetQueryableAsync();
      var relatedListableContentIds = queryableRelation.Where(x => x.ListableContentId == id).Select(x => x.RelatedListableContentId).ToList();

      var item = _objectMapper.Map<ListableContent, ListableContentWithCrossDto>(ListableContent);
      
      item.CityIds = cityIds;
      item.CategoryIds = categoryIds;
      item.TagIds = tagIds;
      item.RelatedListableContentIds = relatedListableContentIds;

      return item;
    }

    public async Task<List<ListableContentDto>> GetByContentTypeAsync(ListableContentType type)
    {
      var ListableContents = await _listableContentRepository.GetListAsync(x => x.listableContentType == type);

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

    public Task<PagedResultDto<ListableContentDto>> GetListAsync(GetListPagedAndSortedDto getListPagedAndSortedDto)
    {
      throw new NotImplementedException();
    }



  }
}
