using NewsManagement.Entities.Videos;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using NewsManagement.EntityDtos.VideoDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NewsManagement.AppService.Videos
{
  public class VideoAppService : CrudAppService<Video, VideoDto, int, GetListPagedAndSortedDto, CreateVideoDto, UpdateVideoDto>, IVideoAppService
  {
    public VideoAppService(IRepository<Video, int> repository) : base(repository)
    {
    }

    public Task DeleteHardAsync(int id)
    {
      throw new NotImplementedException();
    }
  }
}
