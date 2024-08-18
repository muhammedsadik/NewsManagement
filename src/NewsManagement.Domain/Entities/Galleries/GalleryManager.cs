using NewsManagement.Entities.Cities;
using NewsManagement.Entities.ListableContents;
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
