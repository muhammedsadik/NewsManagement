using NewsManagement.AppService.Videos;
using NewsManagement.Entities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NewsManagement.Video
{
  public class VideoAppService_Test : NewsManagementApplicationTestBase
  {
    private readonly VideoAppService _videoAppService;

    public VideoAppService_Test()
    {
      _videoAppService = GetRequiredService<VideoAppService>();

    }

    [Fact]
    public async Task CreateAsync_MainCategoryNameInValid_AlreadyExistException()
    {
      // CreateCategoryDto category = new() { CategoryName = "Kültür", ColorCode = "#a6e79f", IsActive = true };

      await Assert.ThrowsAsync<AlreadyExistException>(async () =>
      {
        //  await _categoryAppService.CreateAsync(category);
      });
    }

  }
}
