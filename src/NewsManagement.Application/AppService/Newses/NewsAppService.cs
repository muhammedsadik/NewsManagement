using NewsManagement.Entities.Newses;
using NewsManagement.EntityDtos.NewsDtos;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using NewsManagement.EntityDtos.VideoDtos;
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
  public class NewsAppService : CrudAppService<News, NewsDto, int, GetListPagedAndSortedDto, CreateNewsDto, UpdateNewsDto>, INewsAppService
  {
    private readonly NewsManager _newsManager;

    public NewsAppService(IRepository<News, int> repository, NewsManager newsManager) : base(repository)
    {
      _newsManager = newsManager;
    }

    public override async Task<NewsDto> CreateAsync(CreateNewsDto createNewsDto)
    {
      return await base.CreateAsync(createNewsDto);
    }

    public async override Task<NewsDto> UpdateAsync(int id, UpdateNewsDto updateNewsDto)
    {
      return await base.UpdateAsync(id, updateNewsDto);
    }

    public async override Task<PagedResultDto<NewsDto>> GetListAsync(GetListPagedAndSortedDto input)
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
