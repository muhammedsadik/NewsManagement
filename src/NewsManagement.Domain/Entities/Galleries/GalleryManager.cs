using EasyAbp.FileManagement.Files;
using NewsManagement.Entities.Categories;
using NewsManagement.Entities.Cities;
using NewsManagement.Entities.Exceptions;
using NewsManagement.Entities.ListableContentRelations;
using NewsManagement.Entities.ListableContents;
using NewsManagement.Entities.Newses;
using NewsManagement.Entities.Tags;
using NewsManagement.Entities.Videos;
using NewsManagement.EntityConsts.ListableContentConsts;
using NewsManagement.EntityDtos.CategoryDtos;
using NewsManagement.EntityDtos.CityDtos;
using NewsManagement.EntityDtos.GalleryDtos;
using NewsManagement.EntityDtos.ListableContentDtos;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using NewsManagement.EntityDtos.TagDtos;
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
    private readonly IFileRepository _fileRepository;
    private readonly ICityRepository _cityRepository;
    private readonly INewsRepository _newsRepository;
    private readonly IVideoRepository _videoRepository;
    private readonly IGalleryRepository _galleryRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IRepository<GalleryImage> _galleryImageRepository;
    private readonly IListableContentGenericRepository<Gallery> _genericRepository;
    private readonly IListableContentTagRepository _listableContentTagRepository;
    private readonly IListableContentCityRepository _listableContentCityRepository;
    private readonly IListableContentCategoryRepository _listableContentCategoryRepository;
    private readonly IListableContentRelationRepository _listableContentRelationRepository;


    public GalleryManager(
      IObjectMapper objectMapper,
      ITagRepository tagRepository,
      IFileRepository fileRepository,
      ICityRepository cityRepository,
      INewsRepository newsRepository,
      IVideoRepository videoRepository,
      IGalleryRepository galleryRepository,
      ICategoryRepository categoryRepository,
      IRepository<GalleryImage> galleryImageRepository,
      IListableContentGenericRepository<Gallery> genericRepository,
      IListableContentTagRepository listableContentTagRepository,
      IListableContentCityRepository listableContentCityRepository,
      IListableContentCategoryRepository listableContentCategoryRepository,
      IListableContentRelationRepository listableContentRelationRepository
      ) : base(objectMapper, tagRepository, cityRepository, newsRepository,
        videoRepository, galleryRepository, categoryRepository, genericRepository, listableContentTagRepository,
        listableContentCityRepository, listableContentCategoryRepository, listableContentRelationRepository, fileRepository
          )
    {
      _objectMapper = objectMapper;
      _tagRepository = tagRepository;
      _fileRepository = fileRepository;
      _cityRepository = cityRepository;
      _newsRepository = newsRepository;
      _videoRepository = videoRepository;
      _galleryRepository = galleryRepository;
      _genericRepository = genericRepository;
      _categoryRepository = categoryRepository;
      _galleryImageRepository = galleryImageRepository;
      _listableContentTagRepository = listableContentTagRepository;
      _listableContentCityRepository = listableContentCityRepository;
      _listableContentCategoryRepository = listableContentCategoryRepository;
      _listableContentRelationRepository = listableContentRelationRepository;
    }


    public async Task<GalleryDto> CreateAsync(CreateGalleryDto createGalleryDto)
    {

      var creatingGallery = await CheckCreateInputBaseAsync(createGalleryDto);

      var orders = createGalleryDto.GalleryImages.Select(x => x.Order).ToList();
      CheckOrderInput(orders);

      foreach (var galleryImage in creatingGallery.GalleryImages)
      {
        var images = _fileRepository.GetAsync(galleryImage.ImageId).Result;
      }

      var gallery = await _genericRepository.InsertAsync(creatingGallery, autoSave: true);

      await CreateCrossEntity(createGalleryDto, gallery.Id);

      var galleryDto = _objectMapper.Map<Gallery, GalleryDto>(gallery);

      await GetCrossEntityAsync(galleryDto);

      return galleryDto;
    }

    public async Task<GalleryDto> UpdateAsync(int id, UpdateGalleryDto updateGalleryDto)
    {
      var updatingGallery = await CheckUpdateInputBaseAsync(id, updateGalleryDto);

      var orders = updateGalleryDto.GalleryImages.Select(x => x.Order).ToList();
      CheckOrderInput(orders);

      foreach (var galleryImage in updatingGallery.GalleryImages)
      {
        var images = _fileRepository.GetAsync(galleryImage.ImageId).Result;
      }

      await _galleryImageRepository.DeleteAsync(x => x.GalleryId == id);
      var gallery = await _genericRepository.UpdateAsync(updatingGallery, autoSave: true);


      await ReCreateCrossEntity(updateGalleryDto, gallery.Id);

      var galleryDto = _objectMapper.Map<Gallery, GalleryDto>(gallery);

      await GetCrossEntityAsync(galleryDto);
      
      return galleryDto;
    }

    public async Task<PagedResultDto<GalleryDto>> GetListAsync(GetListPagedAndSortedDto input)
    {
      var filteredList = await GetListFilterBaseAsync(input);

      foreach (var item in filteredList.Items.ToList())
      {
        var galleryImage = await _galleryImageRepository.GetListAsync(x => x.GalleryId == item.Id);
        item.GalleryImages  = _objectMapper.Map<List<GalleryImage>, List<GalleryImageDto>>(galleryImage);
      }

      return filteredList;
    }

    public async Task<GalleryDto> GetByIdAsync(int id)
    {
      var gallery = await GetByIdBaseAsync(id);

      var galleryImage = await _galleryImageRepository.GetListAsync(x => x.GalleryId == gallery.Id);
      gallery.GalleryImages = _objectMapper.Map<List<GalleryImage>, List<GalleryImageDto>>(galleryImage);

      return gallery;
    }

    public async Task DeleteAsync(int id)
    {
      await CheckDeleteInputBaseAsync(id);
    }

    public async Task DeleteHardAsync(int id)
    {
      await CheckDeleteHardInputBaseAsync(id);
    }

    public void CheckOrderInput(List<int> input)
    {
      input.Sort();

      for (int i = 0; i < input.Count; i++)
      {
        if (input[i] != i + 1)
        {
          throw new BusinessException(NewsManagementDomainErrorCodes.SortingError)
            .WithData("0", input[i].ToString());
        }
      }

    }

  }
}
