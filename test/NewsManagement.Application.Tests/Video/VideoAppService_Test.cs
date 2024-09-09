using NewsManagement.AppService.Galleries;
using NewsManagement.AppService.Videos;
using NewsManagement.Entities.Exceptions;
using NewsManagement.EntityConsts.ListableContentConsts;
using NewsManagement.EntityConsts.VideoConsts;
using NewsManagement.EntityDtos.GalleryDtos;
using NewsManagement.EntityDtos.ListableContentDtos;
using NewsManagement.EntityDtos.NewsDtos;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using NewsManagement.EntityDtos.VideoDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.FeatureManagement;
using Volo.Abp.ObjectMapping;
using Xunit;

namespace NewsManagement.Video
{
  public class VideoAppService_Test : NewsManagementApplicationTestBase
  {
    private readonly VideoAppService _videoAppService;
    private readonly IObjectMapper _objectMapper;
    private readonly Guid _filesImageId;
    private readonly Guid _uploadImageId;
    private readonly CreateVideoDto _createVideoDto;

    public VideoAppService_Test()
    {
      _videoAppService = GetRequiredService<VideoAppService>();
      _objectMapper = GetRequiredService<IObjectMapper>();
      _filesImageId = NewsManagementTestConsts.FilesImageId;
      _uploadImageId = NewsManagementTestConsts.UploadImageId;

      _createVideoDto = new CreateVideoDto()
      {
        Title = "Video Haber 1",
        Spot = "string",
        ImageId = _filesImageId,
        TagIds = new List<int>() { 1 },
        CityIds = new List<int>() { 1 },
        RelatedListableContentIds = new List<int>() { 1 },
        ListableContentCategoryDtos = new List<ListableContentCategoryDto>()
        {
          new()
          {
            CategoryId = 2, IsPrimary = true
          }
        },
        PublishTime = null,
        Status = StatusType.Draft,
        VideoType = VideoType.Video,
        VideoId = _uploadImageId,
      };
    }

    [Fact]
    public async Task CreateAsync_ParentCategoryInValid_BusinessException()
    {
      _createVideoDto.Title = "Video Haber 0";
      _createVideoDto.ListableContentCategoryDtos = new List<ListableContentCategoryDto>()
      {
        new()
        {
          CategoryId = 8, IsPrimary = true
        },
        new()
        {
          CategoryId = 9, IsPrimary = false
        }
      };

      var exception = await Assert.ThrowsAsync<BusinessException>(async () =>
      {
        await _videoAppService.CreateAsync(_createVideoDto);
      });

      Assert.Equal(NewsManagementDomainErrorCodes.WithoutParentCategory, exception.Code);
      Assert.Equal("8, 9", exception.Data["categoryId"]);
    }

    [Fact]
    public async Task UpdateAsync_VideoTypeUrlNull_BusinessException()
    {
      var updateVideo = _objectMapper.Map<CreateVideoDto, UpdateVideoDto>(_createVideoDto);
      updateVideo.Url = "www.google.com.tr";
      var id = 5;

      var exception = await Assert.ThrowsAsync<BusinessException>(async () =>
      {
        await _videoAppService.UpdateAsync(id, updateVideo);
      });

      Assert.Equal(NewsManagementDomainErrorCodes.UrlMustBeNullForVideoType, exception.Code);
    }

    [Fact]
    public async Task GetListAsync_FilterLimitsInValid_BusinessException()
    {
      var exception = await Assert.ThrowsAsync<BusinessException>(async () =>
      {
        await _videoAppService.GetListAsync(new GetListPagedAndSortedDto() { SkipCount = 10});
      });

      Assert.Equal(NewsManagementDomainErrorCodes.FilterLimitsError, exception.Code);
    }

    [Fact]
    public async Task DeleteAsync_IdInValid_EntityNotFoundException()
    {
      int id = 99;

      await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
      {
        await _videoAppService.DeleteAsync(id);
      });
    }


  }
}
