using NewsManagement.AppService.Galleries;
using NewsManagement.Entities.Exceptions;
using NewsManagement.EntityConsts.ListableContentConsts;
using NewsManagement.EntityDtos.GalleryDtos;
using NewsManagement.EntityDtos.ListableContentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.ObjectMapping;
using Xunit;
using static NewsManagement.Permissions.NewsManagementPermissions;

namespace NewsManagement.Gallery
{
  public class GalleryAppService_Test : NewsManagementApplicationTestBase
  {
    private readonly GalleryAppService _galleryAppService;
    private readonly IObjectMapper _objectMapper;
    private readonly Guid filesImageId;
    private readonly Guid uploadImageId;

    public GalleryAppService_Test()
    {
      _galleryAppService = GetRequiredService<GalleryAppService>();
      _objectMapper = GetRequiredService<IObjectMapper>();
      filesImageId = Guid.Parse("17a4c001-a570-c250-60e0-18b9bf25b001");
      uploadImageId = Guid.Parse("27a4c002-a570-c250-60e0-18b9bf25b002");


    }


    [Fact]
    public async Task CreateAsync_GalleryNameInValid_AlreadyExistException()
    {
      var createGallery = new CreateGalleryDto()
      {
        Title = "Gallery Haber 1",
        Spot = "string",
        ImageId = filesImageId,
        TagIds = new List<int>() { 1 },
        CityIds = new List<int>() { 1 },
        RelatedListableContentIds = new List<int>() { 1 },
        ListableContentCategoryDtos = new List<ListableContentCategoryDto>() 
        {
          new()
          {
            CategoryId = 1, IsPrimary = true
          }
        },
        PublishTime = null,
        Status = StatusType.Draft,
        GalleryImages = new List<GalleryImageDto>()
        {
          new()
          {
            ImageId = filesImageId,
            NewsContent = "string",
            Order = 1
          }
        }

      };


      await Assert.ThrowsAsync<AlreadyExistException>(async () =>
      {
        await _galleryAppService.CreateAsync(createGallery);
      });
    }
  }
}
