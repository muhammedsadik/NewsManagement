using NewsManagement.EntityDtos.PagedAndSortedDtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace NewsManagement.EntityDtos.TagDtos
{
  public interface ITagAppService : ICrudAppService<TagDto, int, GetListPagedAndSortedDto, CreateTagDto, UpdateTagDto>
  {
    Task DeleteHardAsync(int id);
  }
}
