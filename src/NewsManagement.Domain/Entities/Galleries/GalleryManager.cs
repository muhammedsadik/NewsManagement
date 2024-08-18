using NewsManagement.Entities.Cities;
using NewsManagement.Entities.ListableContents;
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
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;

namespace NewsManagement.Entities.Galleries
{
  public class GalleryManager : ListableContentBaseManager<Gallery, GalleryDto, GetListPagedAndSortedDto, CreateGalleryDto, UpdateGalleryDto>
  {
    private readonly IGalleryRepository _galleryRepository;
    private readonly IObjectMapper _objectMapper;

    public GalleryManager(IObjectMapper objectMapper, IGalleryRepository galleryRepository) : base(galleryRepository, objectMapper)
    {
      _objectMapper = objectMapper;
      _galleryRepository = galleryRepository;
    }


    public async Task<GalleryDto> CreateAsync(CreateGalleryDto createGalleryDto)
    {
      await CheckCreateInputBaseAsync(createGalleryDto);

      var creatingGallery = _objectMapper.Map<CreateGalleryDto, Gallery>(createGalleryDto);

      creatingGallery.PublishTime = DateTime.Now;
      creatingGallery.Status = StatusType.PendingReview;
      creatingGallery.listableContentType = ListableContentType.Gallery;

      var gallery = await _galleryRepository.InsertAsync(creatingGallery);

      await CreateListableContentTagAsync(createGalleryDto.TagIds, gallery.Id);

      if(createGalleryDto.CityIds != null)
      await CreateListableContentCityAsync(createGalleryDto.CityIds, gallery.Id);

      await CreateListableContentCategoryAsync(createGalleryDto.ListableContentCategoryDtos, gallery.Id);

      if (createGalleryDto.RelatedListableContentIds != null)
        await CreateListableContentRelationAsync(createGalleryDto.RelatedListableContentIds, gallery.Id);

      var galleryDto = _objectMapper.Map<Gallery, GalleryDto>(gallery);

      return galleryDto;
    }

    public async Task<GalleryDto> UpdateAsync(int id, UpdateGalleryDto updateGalleryDto)
    {

    }

    public async Task<PagedResultDto<GalleryDto>> GetListAsync(GetListPagedAndSortedDto input)
    {

    }

    public async Task DeleteAsync(int id)
    {

    }

    public async Task DeleteHardAsync(int id)
    {

    }


  }
}
