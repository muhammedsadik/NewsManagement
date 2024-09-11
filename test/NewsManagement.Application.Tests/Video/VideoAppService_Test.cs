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
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.FeatureManagement;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.TenantManagement;
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
    private readonly IDataFilter<IMultiTenant> _dataFilter;
    private readonly ICurrentTenant _currentTenant;
    private readonly ITenantRepository _tenantRepository;
    private readonly Guid? _tenantId;

    public VideoAppService_Test()
    {
      _videoAppService = GetRequiredService<VideoAppService>();
      _objectMapper = GetRequiredService<IObjectMapper>();
      _filesImageId = NewsManagementConsts.YoungTenanFilesImageId;
      _uploadImageId = NewsManagementConsts.YoungTenanFilesImageId;
      _dataFilter = GetRequiredService<IDataFilter<IMultiTenant>>();
      _currentTenant = GetRequiredService<CurrentTenant>();
      _tenantRepository = GetRequiredService<ITenantRepository>();

      _tenantId = _tenantRepository.FindByName(NewsManagementConsts.YoungTenanName).Id;

      _createVideoDto = new CreateVideoDto()
      {

        Title = "Video Haber 1",
        Spot = "string",
        ImageId = _filesImageId,
        TagIds = new List<int>() { 5 },
        CityIds = new List<int>() { 8 },
        RelatedListableContentIds = new List<int>() { 12 },
        ListableContentCategoryDtos = new List<ListableContentCategoryDto>()
        {
          new()
          {
            CategoryId = 11, IsPrimary = true
          }
        },
        PublishTime = null,
        Status = StatusType.Draft,
        VideoType = VideoType.Video,
        VideoId = _filesImageId,

      };
    }

    [Fact]
    public async Task CreateAsync_ParentCategoryInValid_BusinessException()
    {
      using (_currentTenant.Change(_tenantId))
      {
        _createVideoDto.Title = "Video Haber 0";
        _createVideoDto.ListableContentCategoryDtos = new List<ListableContentCategoryDto>()
        {
          new()
          {
            CategoryId = 17, IsPrimary = true
          },
          new()
          {
            CategoryId = 18, IsPrimary = false
          }
        };

        var exception = await Assert.ThrowsAsync<BusinessException>(async () =>
        {
          await _videoAppService.CreateAsync(_createVideoDto);
        });

        Assert.Equal(NewsManagementDomainErrorCodes.WithoutParentCategory, exception.Code);
        Assert.Equal("17, 18", exception.Data["categoryId"]);
      }
    }

    [Fact]
    public async Task UpdateAsync_VideoTypeUrlNull_BusinessException()
    {
      using (_currentTenant.Change(_tenantId))
      {
        var updateVideo = _objectMapper.Map<CreateVideoDto, UpdateVideoDto>(_createVideoDto);
        updateVideo.Url = "www.google.com.tr";
        var id = 16;

        var exception = await Assert.ThrowsAsync<BusinessException>(async () =>
        {
          await _videoAppService.UpdateAsync(id, updateVideo);
        });

        Assert.Equal(NewsManagementDomainErrorCodes.UrlMustBeNullForVideoType, exception.Code);
      }
    }

    [Fact]
    public async Task GetListAsync_FilterLimitsInValid_BusinessException()
    {
      using (_currentTenant.Change(_tenantId))
      {
        var exception = await Assert.ThrowsAsync<BusinessException>(async () =>
        {
          await _videoAppService.GetListAsync(new GetListPagedAndSortedDto() { SkipCount = 5 });
        });

        Assert.Equal(NewsManagementDomainErrorCodes.FilterLimitsError, exception.Code);
      }
    }

    [Fact]
    public async Task DeleteAsync_IdInValid_EntityNotFoundException()
    {
      int id = 99;
      using (_currentTenant.Change(_tenantId))
      {
        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
          await _videoAppService.DeleteAsync(id);
        });
      }
    }

  }
}
