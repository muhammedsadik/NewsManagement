using NewsManagement.AppService.Galleries;
using NewsManagement.Entities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NewsManagement.Gallery
{
  public class GalleryAppService_Test : NewsManagementApplicationTestBase
  {
    private readonly GalleryAppService _galleryppService;

    public GalleryAppService_Test()
    {
      _galleryppService = GetRequiredService<GalleryAppService>();

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
