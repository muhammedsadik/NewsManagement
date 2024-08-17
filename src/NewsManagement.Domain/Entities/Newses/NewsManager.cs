using NewsManagement.EntityDtos.NewsDtos;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;

namespace NewsManagement.Entities.Newses
{
  public class NewsManager : DomainService
  {
    private readonly INewsRepository _newsRepository;
    private readonly IObjectMapper _objectMapper;

    public NewsManager(IObjectMapper objectMapper, INewsRepository newsRepository)
    {
      _objectMapper = objectMapper;
      _newsRepository = newsRepository;
    }

    //public async Task<NewsDto> CreateAsync(CreateNewsDto createNewsDto)
    //{

    //}

    //public async Task<NewsDto> UpdateAsync(int id, UpdateNewsDto updateNewsDto)
    //{

    //}

    //public async Task<PagedResultDto<NewsDto>> GetListAsync(GetListPagedAndSortedDto input)
    //{

    //}

    public async Task DeleteAsync(int id)
    {

    }

    public async Task DeleteHardAsync(int id)
    {

    }



  }
}
