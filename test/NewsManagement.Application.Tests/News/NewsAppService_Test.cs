using NewsManagement.AppService.Newses;
using NewsManagement.Entities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NewsManagement.News
{
  public class NewsAppService_Test : NewsManagementApplicationTestBase
  {
    private readonly NewsAppService _newsAppService;

    public NewsAppService_Test()
    {
      _newsAppService = GetRequiredService<NewsAppService>();

    }

    [Fact]
    public async Task CreateAsync_MainCategoryNameInValid_AlreadyExistException()
    {
      // CreateCategoryDto category = new() { CategoryName = "Kültür", ColorCode = "#a6e79f", IsActive = true };

      await Assert.ThrowsAsync<AlreadyExistException>(async () =>
      {
        // await _categoryAppService.CreateAsync(category);
      });
    }
  }
}
