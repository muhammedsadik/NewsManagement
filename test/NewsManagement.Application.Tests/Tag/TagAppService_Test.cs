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

namespace NewsManagement.Tag
{
  public class TagAppService_Test : NewsManagementApplicationTestBase
  {
    private readonly TagAppService _tagAppService;

    public TagAppService_Test()
    {
      _tagAppService = GetRequiredService<TagAppService>();

    }

    [Fact]
    public async Task CreateAsync_TagNameInValid_AlreadyExistException()
    {
      CreateTagDto tag = new() { TagName = "Yaşam" };

      await Assert.ThrowsAsync<AlreadyExistException>(async () =>
      {
        await _tagAppService.CreateAsync(tag);
      });

    }

    [Fact]
    public async Task UpdateAsync_ReturnValue_TagDto()
    {
      int tagId = 2;
      UpdateTagDto tag = new() { TagName = "Teknoloji" };

      var result = await _tagAppService.UpdateAsync(tagId, tag);

      Assert.NotNull(result);
      Assert.IsType<TagDto>(result);
    }


    [Fact]
    public async Task GetListAsync_RetrunValue_FilterData()
    {
      var tagList = await _tagAppService.GetListAsync(new GetListPagedAndSortedDto() { Filter = "Sp" });

      tagList.Items.ShouldContain(t => t.TagName == "Spor");
    }

    [Fact]
    public async Task DeleteAsynce_IdInValid_EntityNotFoundException()
    {
      int tagId = 30;

      await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
      {
        await _tagAppService.DeleteAsync(tagId);
      });
    }



    [Fact]
    public async Task DeleteHardAsynce_IdInValid_EntityNotFoundException()
    {
      int tagId = 30;

      await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
      {
        await _tagAppService.DeleteHardAsync(tagId);
      });
    }
  }
}
