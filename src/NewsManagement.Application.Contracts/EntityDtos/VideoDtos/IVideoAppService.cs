using NewsManagement.EntityDtos.PagedAndSortedDtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace NewsManagement.EntityDtos.VideoDtos
{
  public interface IVideoAppService : ICrudAppService<VideoDto, int, GetListPagedAndSortedDto, CreateVideoDto, UpdateVideoDto>
  {
    Task DeleteHardAsync(int id);
  }
}
