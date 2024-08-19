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


      //Insert yapmadan önce duruma göre time ve status düzenle
      var gallery = await _galleryRepository.InsertAsync(updatingGallery);


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
