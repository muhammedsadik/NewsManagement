using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp;
using NewsManagement.Entities.Tags;
using NewsManagement.AppService.Tags;
using Shouldly;
using Xunit;
using NewsManagement.EntityDtos.TagDtos;
using NewsManagement.Entities.Exceptions;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using static NewsManagement.Permissions.NewsManagementPermissions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace NewsManagement.Tag
{
  public class TagAppService_Test : NewsManagementApplicationTestBase
  {
    private readonly TagAppService _tagAppService;
    private readonly IDataFilter<IMultiTenant> _dataFilter;

    public TagAppService_Test()
    {
      _tagAppService = GetRequiredService<TagAppService>();
      _dataFilter = GetRequiredService<IDataFilter<IMultiTenant>>();

    }

    [Fact]
    public async Task CreateAsync_TagNameInValid_AlreadyExistException()
    {
      using (_dataFilter.Disable())
      {
        CreateTagDto tag = new() { TagName = "Yaşam" };

        await Assert.ThrowsAsync<AlreadyExistException>(async () =>
        {
          await _tagAppService.CreateAsync(tag);
        });
      }
    }

    [Fact]
    public async Task UpdateAsync_ReturnValue_TagDto()
    {
      using (_dataFilter.Disable())
      {
        int tagId = 2;
        UpdateTagDto tag = new() { TagName = "Deniz" };

        var result = await _tagAppService.UpdateAsync(tagId, tag);

        Assert.NotNull(result);
        Assert.IsType<TagDto>(result);
      }
    }


    [Fact]
    public async Task GetListAsync_RetrunValue_FilterData()
    {
      using (_dataFilter.Disable())
      {
        var tagList = await _tagAppService.GetListAsync(new GetListPagedAndSortedDto() { Filter = "Sp" });

        tagList.Items.ShouldContain(t => t.TagName == "Spor");
      }
    }

    [Fact]
    public async Task DeleteAsync_IdInValid_EntityNotFoundException()
    {
      using (_dataFilter.Disable())
      {
        int tagId = 30;

        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
          await _tagAppService.DeleteAsync(tagId);
        });
      }
    }

    [Fact]
    public async Task DeleteHardAsync_IdInValid_EntityNotFoundException()
    {
      using (_dataFilter.Disable())
      {
        int tagId = 30;

        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
          await _tagAppService.DeleteHardAsync(tagId);
        });
      }
    }
  }
}
