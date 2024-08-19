using Microsoft.AspNetCore.Authorization;
using NewsManagement.Entities.Galleries;
using NewsManagement.Entities.Newses;
using NewsManagement.EntityDtos.GalleryDtos;
using NewsManagement.EntityDtos.NewsDtos;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using NewsManagement.EntityDtos.VideoDtos;
using NewsManagement.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NewsManagement.AppService.Newses
{
  [Authorize(NewsManagementPermissions.Newses.Default)]
  public class NewsAppService : CrudAppService<News, NewsDto, int, GetListPagedAndSortedDto, CreateNewsDto, UpdateNewsDto>, INewsAppService
  {
    private readonly NewsManager _newsManager;

    public NewsAppService(IRepository<News, int> repository, NewsManager newsManager) : base(repository)
    {
      _newsManager = newsManager;
    }

    [Authorize(NewsManagementPermissions.Newses.Create)]
    public override async Task<NewsDto> CreateAsync(CreateNewsDto createNewsDto)
    {
      return await _newsManager.CreateAsync(createNewsDto);
    }

    [Authorize(NewsManagementPermissions.Newses.Edit)]
    public async override Task<NewsDto> UpdateAsync(int id, UpdateNewsDto updateNewsDto)
    {
      return await _newsManager.UpdateAsync(id, updateNewsDto);
    }

    public async override Task<PagedResultDto<NewsDto>> GetListAsync(GetListPagedAndSortedDto input)
    {
      return await _newsManager.GetListAsync(input);
    }

    [Authorize(NewsManagementPermissions.Newses.Delete)]
    public override async Task DeleteAsync(int id)
    {
      await _newsManager.DeleteAsync(id);

      await base.DeleteAsync(id);
    }

    [Authorize(NewsManagementPermissions.Newses.Delete)]
    public async Task DeleteHardAsync(int id)
    {
      await _newsManager.DeleteHardAsync(id);
    }
  }
}
