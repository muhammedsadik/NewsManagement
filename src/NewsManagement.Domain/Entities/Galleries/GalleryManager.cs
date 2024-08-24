using NewsManagement.Entities.Categories;
using NewsManagement.Entities.Cities;
using NewsManagement.Entities.Exceptions;
using NewsManagement.Entities.ListableContentRelations;
using NewsManagement.Entities.ListableContents;
using NewsManagement.Entities.Newses;
using NewsManagement.Entities.Tags;
using NewsManagement.Entities.Videos;
using NewsManagement.EntityConsts.ListableContentConsts;
using NewsManagement.EntityDtos.GalleryDtos;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;

namespace NewsManagement.Entities.Galleries
{
  public class GalleryManager : ListableContentBaseManager<Gallery, GalleryDto, GetListPagedAndSortedDto, CreateGalleryDto, UpdateGalleryDto>
  {
    private readonly IObjectMapper _objectMapper;
    private readonly ITagRepository _tagRepository;
    private readonly ICityRepository _cityRepository;
    private readonly INewsRepository _newsRepository;
    private readonly IVideoRepository _videoRepository;
    private readonly IGalleryRepository _galleryRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IListableContentGenericRepository<Gallery> _genericRepository;
    private readonly IRepository<ListableContentTag> _listableContentTagRepository;
    private readonly IRepository<ListableContentCity> _listableContentCityRepository;
    private readonly IRepository<ListableContentCategory> _listableContentCategoryRepository;
    private readonly IRepository<ListableContentRelation> _listableContentRelationRepository;

    public GalleryManager(
      IObjectMapper objectMapper,
      ITagRepository tagRepository,
      ICityRepository cityRepository,
      INewsRepository newsRepository,
      IVideoRepository videoRepository,
      IGalleryRepository galleryRepository,
      ICategoryRepository categoryRepository,
      IListableContentGenericRepository<Gallery> genericRepository,
      IRepository<ListableContentTag> listableContentTagRepository,
      IRepository<ListableContentCity> listableContentCityRepository,
      IRepository<ListableContentCategory> listableContentCategoryRepository,
      IRepository<ListableContentRelation> listableContentRelationRepository
      ) : base(objectMapper, tagRepository, cityRepository, newsRepository,
        videoRepository, galleryRepository, categoryRepository, genericRepository, listableContentTagRepository,
        listableContentCityRepository, listableContentCategoryRepository, listableContentRelationRepository
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
    }


    public async Task<GalleryDto> CreateAsync(CreateGalleryDto createGalleryDto)
    {      
      await CheckCreateInputBaseAsync(createGalleryDto);

      var creatingGallery = _objectMapper.Map<CreateGalleryDto, Gallery>(createGalleryDto);

      creatingGallery.PublishTime = DateTime.Now;
      creatingGallery.Status = StatusType.PendingReview;
      creatingGallery.listableContentType = ListableContentType.Gallery;

      //updateGalleryDto.GalleryImage  kontrolü
      // ❓ ImageId ye ait bir item varmı kontrolünü yap ve => 📩

      var gallery = await _genericRepository.InsertAsync(creatingGallery, autoSave: true);

      await CreateListableContentTagBaseAsync(createGalleryDto.TagIds, gallery.Id);

      await CreateListableContentCityBaseAsync(createGalleryDto.CityIds, gallery.Id);

      await CreateListableContentCategoryBaseAsync(createGalleryDto.ListableContentCategoryDtos, gallery.Id);

      if (createGalleryDto.RelatedListableContentIds != null)
        await CreateListableContentRelationBaseAsync(createGalleryDto.RelatedListableContentIds, gallery.Id);


      var galleryDto = _objectMapper.Map<Gallery, GalleryDto>(gallery);

      return galleryDto;
    }

    public async Task<GalleryDto> UpdateAsync(int id, UpdateGalleryDto updateGalleryDto)
    {
      var updatingGallery = await CheckUpdateInputBaseAsync(id, updateGalleryDto);

      updatingGallery.listableContentType = ListableContentType.Gallery;

      //updateGalleryDto.GalleryImage  kontrolü
      // ❓ ImageId ye ait bir item varmı kontrolünü yap ve => 📩

      var gallery = await _genericRepository.UpdateAsync(updatingGallery, autoSave: true);

      await ReCreateListableContentTagBaseAsync(updateGalleryDto.TagIds, id);

      await ReCreateListableContentCityBaseAsync(updateGalleryDto.CityIds, id);

      await ReCreateListableContentCategoryBaseAsync(updateGalleryDto.ListableContentCategoryDtos, id);

      if (updateGalleryDto.RelatedListableContentIds != null)
        await ReCreateListableContentRelationBaseAsync(updateGalleryDto.RelatedListableContentIds, id);

      var galleryDto = _objectMapper.Map<Gallery, GalleryDto>(gallery);

      return galleryDto;
    }

    public async Task<PagedResultDto<GalleryDto>> GetListAsync(GetListPagedAndSortedDto input)
    {
      return await GetListFilterBaseAsync(input);
    }

    public async Task DeleteAsync(int id)
    {
      await CheckDeleteInputBaseAsync(id);
    }

    public async Task DeleteHardAsync(int id)
    {
      await CheckDeleteHardInputBaseAsync(id);
    }


  }
}
