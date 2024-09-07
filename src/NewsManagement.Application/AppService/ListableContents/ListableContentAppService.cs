using NewsManagement.Entities.ListableContents;
using NewsManagement.EntityConsts.ListableContentConsts;
using NewsManagement.EntityDtos.ListableContentDtos;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Features;

namespace NewsManagement.AppService.ListableContents
{
  //[RequiresFeature("NewsApp.ListableContent")]
  public class ListableContentAppService : NewsManagementAppService, IListableContentAppService
  {
    private readonly ListableContentManager _listableContentManager;

    public ListableContentAppService(ListableContentManager listableContentManager)
    {
      _listableContentManager = listableContentManager;
    }

    
    public async Task<ListableContentWithCrossDto> GetByIdAsync(int id)
    {
      return await _listableContentManager.GetByIdAsync(id);
    }

    public async Task<List<ListableContentDto>> GetByContentTypeAsync(ListableContentType type)
    {
      return await _listableContentManager.GetByContentTypeAsync(type);
    }

    public async Task<ListableContentWithRelationDto> GetByIdWithRelationAsync(int id)
    {
      return await _listableContentManager.GetByIdWithRelationAsync(id);
    }

    public async Task<PagedResultDto<ListableContentDto>> GetListAsync(GetListPagedAndSortedDto getListPagedAndSortedDto)
    {
      return await _listableContentManager.GetListAsync(getListPagedAndSortedDto);
    }
  }
}
