using NewsManagement.EntityDtos.PagedAndSortedDtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace NewsManagement.EntityDtos.NewsDtos
{
  public interface INewsAppService : ICrudAppService<NewsDto, int , GetListPagedAndSortedDto, CreateNewsDto, UpdateNewsDto>
  {
    Task DeleteHardAsync(int id);
  }
}
