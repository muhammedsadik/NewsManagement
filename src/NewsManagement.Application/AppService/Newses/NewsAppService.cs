using NewsManagement.Entities.Newses;
using NewsManagement.EntityDtos.NewsDtos;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NewsManagement.AppService.Newses
{
  public class NewsAppService : CrudAppService<News, NewsDto, int, GetListPagedAndSortedDto, CreateNewsDto, UpdateNewsDto>, INewsAppService
  {
    public NewsAppService(IRepository<News, int> repository) : base(repository)
    {
    }

    public Task DeleteHardAsync(int id)
    {
      throw new NotImplementedException();
    }
  }
}
