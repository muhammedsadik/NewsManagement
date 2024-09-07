using NewsManagement.AppService.ListableContents;
using NewsManagement.EntityConsts.ListableContentConsts;
using NewsManagement.EntityDtos.ListableContentDtos;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NewsManagement.ListableContent
{
  public class ListableContentAppService_Test : NewsManagementApplicationTestBase
  {
    private readonly ListableContentAppService _listableContentAppService;

    public ListableContentAppService_Test()
    {
      _listableContentAppService = GetRequiredService<ListableContentAppService>();
    }

    [Fact]
    public async Task GetByIdAsync_IdInValid_ReturnValueListableContentWithCrossDto()
    {
      var id = 1;

      var listableContent = await _listableContentAppService.GetByIdAsync(id);

      Assert.NotNull(listableContent);
      Assert.IsType<ListableContentWithCrossDto>(listableContent);
    }

     [Fact]
    public async Task GetByContentTypeAsync_TypeInValid_ReturnValueListableContentDto()
    {
      var type = ListableContentType.Gallery;

      var listableContent = await _listableContentAppService.GetByContentTypeAsync(type);

      Assert.NotNull(listableContent);
      Assert.IsType<List<ListableContentDto>>(listableContent);
    }

     [Fact]
    public async Task GetByIdWithRelationAsync_IdInValid_ReturnValueListableContentWithRelationDto()
    {
      var id = 6;

      var listableContent = await _listableContentAppService.GetByIdWithRelationAsync(id);

      Assert.NotNull(listableContent);
      Assert.Equal(5, listableContent.ListableContents.Count);
      Assert.IsType<ListableContentWithRelationDto>(listableContent);
    }

     [Fact]
    public async Task GetListAsync_InputInValid_ReturnValuePagedResultDto()
    {
      var listableContent = await _listableContentAppService.GetListAsync(new GetListPagedAndSortedDto() { Filter = "Haber 1"});

      Assert.NotNull(listableContent);
      Assert.Equal(3, listableContent.TotalCount);
      Assert.IsType<ListableContentWithRelationDto>(listableContent);
    }

  }
}
