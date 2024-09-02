using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
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
  [Authorize(NewsManagementPermissions.Galleries.Default)]
  public class GalleryAppService : CrudAppService<Gallery, GalleryDto, int, GetListPagedAndSortedDto, CreateGalleryDto, UpdateGalleryDto>, IGalleryAppService
  {
    private readonly GalleryManager _galleryManager;

    public GalleryAppService(IRepository<Gallery, int> repository, GalleryManager galleryManager) : base(repository)
    {
      _galleryManager = galleryManager;
    }

    [Authorize(NewsManagementPermissions.Galleries.Create)]
    public override async Task<GalleryDto> CreateAsync(CreateGalleryDto createGalleryDto)
    {
      return await _galleryManager.CreateAsync(createGalleryDto);
    }

    [Authorize(NewsManagementPermissions.Galleries.Edit)]
    public async override Task<GalleryDto> UpdateAsync(int id, UpdateGalleryDto updateGalleryDto)
    {
      return await _galleryManager.UpdateAsync(id, updateGalleryDto);
    }

    protected override async Task<Gallery> GetEntityByIdAsync(int id)
    {


      return await base.GetEntityByIdAsync(id);
    }

    public async override Task<PagedResultDto<GalleryDto>> GetListAsync(GetListPagedAndSortedDto input)
    {
      return await _galleryManager.GetListAsync(input);
    }

    [Authorize(NewsManagementPermissions.Galleries.Delete)]
    public override async Task DeleteAsync(int id)
    {
      await _galleryManager.DeleteAsync(id);

      await base.DeleteAsync(id);
    }

    [Authorize(NewsManagementPermissions.Galleries.Delete)]
    public async Task DeleteHardAsync(int id)
    {
       await _galleryManager.DeleteHardAsync(id);
    }
  }
}
