using NewsManagement.AppService.Cities;
using NewsManagement.Entities.Cities;
using NewsManagement.Entities.Exceptions;
using NewsManagement.EntityDtos.CityDtos;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace NewsManagement.City
{
  public class CityAppService_Test : NewsManagementApplicationTestBase
  {
    private readonly CityAppService _cityAppService;
    private readonly IDataFilter<IMultiTenant> _dataFilter;
    public CityAppService_Test()
    {
      _cityAppService = GetRequiredService<CityAppService>();
      _dataFilter = GetRequiredService<IDataFilter<IMultiTenant>>();
    }


    [Fact]
    public async Task CreateAsync_ReturnValue_CityDto()
    {
      using (_dataFilter.Disable())
      {
        CreateCityDto city = new() { CityName = "Adana", CityCode = 01 };

        var result = await _cityAppService.CreateAsync(city);

        Assert.NotNull(result);
        Assert.IsType<CityDto>(result);
      }
    }

    [Fact]
    public async Task UpdateAsync_CityNameInValid_AlreadyExistException()
    {
      using (_dataFilter.Disable())
      {
        int cityId = 1;
        UpdateCityDto city = new() { CityName = "Ankara", CityCode = 01 };

        await Assert.ThrowsAsync<AlreadyExistException>(async () =>
        {
          await _cityAppService.UpdateAsync(cityId, city);
        });
      }
    }

    [Fact]
    public async Task GetListAsync_SkipCountBiggerThanTotalCount_BusinessException()
    {
      using (_dataFilter.Disable())
      {
        var skipCount = 15;

        await Assert.ThrowsAsync<BusinessException>(async () =>
        {
          await _cityAppService.GetListAsync(new GetListPagedAndSortedDto() { SkipCount = skipCount });
        });
      }
    }

    [Fact]
    public async Task DeleteAsync_IdInValid_EntityNotFoundException()
    {
      using (_dataFilter.Disable())
      {
        int cityId = 30;

        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
          await _cityAppService.DeleteAsync(cityId);
        });
      }
    }

    [Fact]
    public async Task DeleteHardAsync_IdInValid_EntityNotFoundException()
    {
      using (_dataFilter.Disable())
      {
        int cityId = 30;

        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
          await _cityAppService.DeleteHardAsync(cityId);
        });
      }
    }


  }
}
