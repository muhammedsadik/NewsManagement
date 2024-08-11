using NewsManagement.EntityDtos.PagedAndSortedDtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace NewsManagement.EntityDtos.CategoryDtos
{
  public interface ICategoryAppService : ICrudAppService<CategoryDto, int, GetListPagedAndSortedDto, CreateCategoryDto, UpdateCategoryDto>
  {
    Task DeleteHardAsync(int id);
  }
}
