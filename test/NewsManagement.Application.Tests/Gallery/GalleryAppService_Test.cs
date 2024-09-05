using NewsManagement.AppService.Galleries;
using NewsManagement.Entities.Exceptions;
using NewsManagement.EntityDtos.GalleryDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.ObjectMapping;
using Xunit;

namespace NewsManagement.Gallery
{
  public class GalleryAppService_Test : NewsManagementApplicationTestBase
  {
    private readonly GalleryAppService _galleryAppService;
    private readonly IObjectMapper _objectMapper;

    public GalleryAppService_Test()
    {
      _galleryAppService = GetRequiredService<GalleryAppService>();
      _objectMapper = GetRequiredService<IObjectMapper>();
    }


    [Fact]
    public async Task CreateAsync_GalleryNameInValid_AlreadyExistException()
    {
      var gallery = new CreateGalleryDto();

      _objectMapper.Map(await _galleryAppService.GetAsync(9), gallery);
      

      await Assert.ThrowsAsync<AlreadyExistException>(async () =>
      {
         await _galleryAppService.CreateAsync(gallery);
      });
    }
  }
}
