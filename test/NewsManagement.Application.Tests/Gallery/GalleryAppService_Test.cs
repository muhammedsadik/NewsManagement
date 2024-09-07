using NewsManagement.AppService.Galleries;
using NewsManagement.Entities.Exceptions;
using NewsManagement.EntityConsts.ListableContentConsts;
using NewsManagement.EntityDtos.GalleryDtos;
using NewsManagement.EntityDtos.ListableContentDtos;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectMapping;
using Xunit;
using static NewsManagement.Permissions.NewsManagementPermissions;

namespace NewsManagement.Gallery
{
  public class GalleryAppService_Test : NewsManagementApplicationTestBase
  {
    private readonly GalleryAppService _galleryAppService;
    private readonly IObjectMapper _objectMapper;
    private readonly Guid _filesImageId;
    private readonly Guid _uploadImageId;
    private readonly CreateGalleryDto _createGalleryDto;

    public GalleryAppService_Test()
    {
      _galleryAppService = GetRequiredService<GalleryAppService>();
      _objectMapper = GetRequiredService<IObjectMapper>();
      _filesImageId = NewsManagementTestConsts.FilesImageId;
      _uploadImageId = NewsManagementTestConsts.UploadImageId;

      _createGalleryDto = new CreateGalleryDto()
      {
        Title = "Gallery Haber 1",
        Spot = "string",
        ImageId = _filesImageId,
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
            ImageId = _filesImageId,
            NewsContent = "string",
            Order = 1
          }
        }
      };

    }

    [Fact]
    public async Task CreateAsync_GalleryNameInValid_AlreadyExistException()
    {
      await Assert.ThrowsAsync<AlreadyExistException>(async () =>
      {
        await _galleryAppService.CreateAsync(_createGalleryDto);
      });
    }

    [Fact]
    public async Task UpdateAsync_DateTimeInValid_BusinessException()
    {
      var updateGallery = _objectMapper.Map<CreateGalleryDto, UpdateGalleryDto>(_createGalleryDto);
      var id = 9;

      updateGallery.Status = StatusType.Scheduled;

      await Assert.ThrowsAsync<BusinessException>(async () =>
      {
        await _galleryAppService.UpdateAsync(id, updateGallery);
      });
    }

    [Fact]
    public async Task GetListAsync_NoFilter_ReturnGallertyList()
    {
      var galleryList = await _galleryAppService.GetListAsync(new GetListPagedAndSortedDto());

      Assert.NotNull(galleryList);
      Assert.Equal(3, galleryList.TotalCount);
    }

    [Fact]
    public async Task DeleteHardAsync_IdValid_EntityDeleting()
    {
      int id = 9;

      await _galleryAppService.DeleteHardAsync(id);

      await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
      {
        await _galleryAppService.GetAsync(id);
      });
    }


  }
}
