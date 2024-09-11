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
  public class ListableContentAppService : NewsManagementAppService, IListableContentAppService
  {
    private readonly ListableContentManager _listableContentManager;

    public ListableContentAppService(ListableContentManager listableContentManager)
    {
      _listableContentManager = listableContentManager;
    }
    
    public async Task<ListableContentDto> GetByIdAsync(int id)
    {
      return await _listableContentManager.GetByIdAsync(id);
    }

  }
}
