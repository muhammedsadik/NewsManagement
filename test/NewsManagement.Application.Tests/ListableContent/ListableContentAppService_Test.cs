using NewsManagement.AppService.ListableContents;
using NewsManagement.EntityConsts.ListableContentConsts;
using NewsManagement.EntityDtos.ListableContentDtos;
using NewsManagement.EntityDtos.NewsDtos;
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
    public async Task GetByIdAsync_ReturnValue_NewsDto()
    {
      var id = 1;

      var listableContent = await _listableContentAppService.GetByIdAsync(id);

      Assert.NotNull(listableContent);
      Assert.IsType<NewsDto>(listableContent);
    }

  }
}
