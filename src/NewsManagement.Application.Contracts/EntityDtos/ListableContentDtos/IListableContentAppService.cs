using NewsManagement.EntityConsts.ListableContentConsts;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NewsManagement.EntityDtos.ListableContentDtos
{
  public interface IListableContentAppService : IApplicationService
  {

    Task<ListableContentWithCrossDto> GetByIdAsync(int id);
    Task<ListableContentWithRelationDto> GetByIdWithRelationAsync(int id);
    Task<List<ListableContentDto>> GetByContentTypeAsync(ListableContentType type);
    Task<PagedResultDto<ListableContentDto>> GetListAsync(GetListPagedAndSortedDto getListPagedAndSortedDto);


  }
}
