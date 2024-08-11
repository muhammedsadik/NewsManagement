using NewsManagement.EntityDtos.PagedAndSortedDtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace NewsManagement.EntityDtos.CityDtos
{
  public interface ICityAppService : ICrudAppService<CityDto, int, GetListPagedAndSortedDto, CreateCityDto, UpdateCityDto>
  {
    Task DeleteHardAsync(int id);
  }
}
