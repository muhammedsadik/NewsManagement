using Microsoft.AspNetCore.Authorization;
using NewsManagement.Entities.Cities;
using NewsManagement.Entities.Galleries;
using NewsManagement.EntityDtos.CityDtos;
using NewsManagement.EntityDtos.GalleryDtos;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using NewsManagement.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NewsManagement.AppService.Galleries
{
  public class GalleryAppService : CrudAppService<Gallery, GalleryDto, int, GetListPagedAndSortedDto, CreateGalleryDto, UpdateGalleryDto>, IGalleryAppService
  {
    private readonly GalleryManager _galleryManager;

    public GalleryAppService(IRepository<Gallery, int> repository, GalleryManager galleryManager) : base(repository)
    {
      _galleryManager = galleryManager;
    }

    public override async Task<GalleryDto> CreateAsync(CreateGalleryDto createGalleryDto)
    {
      return await _galleryManager.CreateAsync(createGalleryDto);
    }

    public async override Task<GalleryDto> UpdateAsync(int id, UpdateGalleryDto updateGalleryDto)
    {
      return await _galleryManager.UpdateAsync(id, updateGalleryDto);
    }

    public async override Task<PagedResultDto<GalleryDto>> GetListAsync(GetListPagedAndSortedDto input)
    {
      throw new NotImplementedException();
    }

    public override async Task DeleteAsync(int id)
    {

      await base.DeleteAsync(id);
    }

    public Task DeleteHardAsync(int id)
    {
      throw new NotImplementedException();
    }
  }
}
