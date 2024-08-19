using NewsManagement.Entities.Categories;
using NewsManagement.Entities.Cities;
using NewsManagement.Entities.GenericRepository;
using NewsManagement.Entities.ListableContentRelations;
using NewsManagement.Entities.ListableContents;
using NewsManagement.Entities.Tags;
using NewsManagement.EntityConsts.ListableContentConsts;
using NewsManagement.EntityDtos.GalleryDtos;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;

namespace NewsManagement.Entities.Galleries
{
  public class GalleryManager : ListableContentBaseManager<Gallery, GalleryDto, GetListPagedAndSortedDto, CreateGalleryDto, UpdateGalleryDto>
  {
    private readonly IGalleryRepository _galleryRepository;
    private readonly IObjectMapper _objectMapper;
    private readonly ITagRepository _tagRepository;
    private readonly ICityRepository _cityRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IGenericRepository<Gallery> _genericRepository;
    private readonly IRepository<ListableContent, int> _listableContentRepository;
    private readonly IRepository<ListableContentTag> _listableContentTagRepository;
    private readonly IRepository<ListableContentCity> _listableContentCityRepository;
    private readonly IRepository<ListableContentCategory> _listableContentCategoryRepository;
    private readonly IRepository<ListableContentRelation> _listableContentRelationRepository;

    public GalleryManager(
      IObjectMapper objectMapper,
      ITagRepository tagRepository,
      IGalleryRepository galleryRepository,
      ICityRepository cityRepository,
      ICategoryRepository categoryRepository,
      IGenericRepository<Gallery> genericRepository,
      IRepository<ListableContent, int> listableContentRepository,
      IRepository<ListableContentTag> listableContentTagRepository,
      IRepository<ListableContentCity> listableContentCityRepository,
      IRepository<ListableContentCategory> listableContentCategoryRepository,
      IRepository<ListableContentRelation> listableContentRelationRepository
      )
      : base(objectMapper, tagRepository, cityRepository, categoryRepository, genericRepository,
          listableContentRepository, listableContentTagRepository, listableContentCityRepository, 
          listableContentCategoryRepository, listableContentRelationRepository)
    {
      _objectMapper = objectMapper;
      _galleryRepository = galleryRepository;
      _tagRepository = tagRepository;
      _cityRepository = cityRepository;
      _categoryRepository = categoryRepository;
      _genericRepository = genericRepository;
      _listableContentRepository = listableContentRepository;
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

      //if(updateGalleryDto.GalleryImage != null)
      // ❓ ImageId ye ait bir item varmı kontrolünü yap ve => 📩

      var gallery = await _galleryRepository.InsertAsync(creatingGallery);

      await CreateListableContentTagAsync(createGalleryDto.TagIds, gallery.Id);

      if (createGalleryDto.CityIds != null)
        await CreateListableContentCityAsync(createGalleryDto.CityIds, gallery.Id);

      await CreateListableContentCategoryAsync(createGalleryDto.ListableContentCategoryDtos, gallery.Id);

      if (createGalleryDto.RelatedListableContentIds != null)
        await CreateListableContentRelationAsync(createGalleryDto.RelatedListableContentIds, gallery.Id);

      var galleryDto = _objectMapper.Map<Gallery, GalleryDto>(gallery);

      return galleryDto;
    }

    public async Task<GalleryDto> UpdateAsync(int id, UpdateGalleryDto updateGalleryDto)
    {
      await CheckUpdateInputBaseAsync(id, updateGalleryDto);

      var updatingGallery = _objectMapper.Map<UpdateGalleryDto, Gallery>(updateGalleryDto);

      //if(updateGalleryDto.listableContentType != ListableContentType.Gallery)
      //burada listableContentType kontrolü yap listableContentType değişebilir ona göre yönlendirme yap
      //(burada UpdateGalleryDto dan geldiği için status değişemez olması gerekiyor ama ListableContent ten gelirse bunu ele almak gerekir.)

      //if(updateGalleryDto.GalleryImage != null)
      // ❓ ImageId ye ait bir item varmı kontrolünü yap ve => 📩

      var gallery = await _galleryRepository.InsertAsync(updatingGallery);

      await ReCreateListableContentTagAsync(updateGalleryDto.TagIds, gallery.Id);

      if (updateGalleryDto.CityIds != null)
        await ReCreateListableContentCityAsync(updateGalleryDto.CityIds, gallery.Id);

      await ReCreateListableContentCategoryAsync(updateGalleryDto.ListableContentCategoryDtos, gallery.Id);

      if (updateGalleryDto.RelatedListableContentIds != null)
        await ReCreateListableContentRelationAsync(updateGalleryDto.RelatedListableContentIds, gallery.Id);

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
