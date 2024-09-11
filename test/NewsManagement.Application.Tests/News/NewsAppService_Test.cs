using NewsManagement.AppService.Galleries;
using NewsManagement.AppService.Newses;
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
using Volo.Abp.Domain.Entities;
using Volo.Abp;
using Xunit;
using Volo.Abp.ObjectMapping;
using NewsManagement.EntityDtos.NewsDtos;
using NewsManagement.Entities.Newses;
using NewsManagement.EntityDtos.CategoryDtos;
using NewsManagement.AppService.Categories;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;

namespace NewsManagement.News
{
  public class NewsAppService_Test : NewsManagementApplicationTestBase
  {
    private readonly NewsAppService _newsAppService;
    private readonly IObjectMapper _objectMapper;
    private readonly Guid _filesImageId;
    private readonly Guid _uploadImageId;
    private readonly CreateNewsDto _createNewsDto;
    private readonly ICurrentTenant _currentTenant;
    private readonly ITenantRepository _tenantRepository;
    private readonly Guid? _tenantId;

    public NewsAppService_Test()
    {
      _newsAppService = GetRequiredService<NewsAppService>();
      _objectMapper = GetRequiredService<IObjectMapper>();
      _filesImageId = NewsManagementConsts.ChildTenanFilesImageId;
      _uploadImageId = NewsManagementConsts.ChildTenanUploadImageId;
      _currentTenant = GetRequiredService<CurrentTenant>();
      _tenantRepository = GetRequiredService<ITenantRepository>();

      _tenantId = _tenantRepository.FindByName(NewsManagementConsts.ChildTenanName).Id;

      _createNewsDto = new CreateNewsDto()
      {
        Title = "News Haber 1",
        Spot = "string",
        ImageId = _filesImageId,
        TagIds = new List<int>() { 1 },
        CityIds = new List<int>() { 1 },
        RelatedListableContentIds = new List<int>() { 1 },
        ListableContentCategoryDtos = new List<ListableContentCategoryDto>()
        {
          new()
          {
            CategoryId = 3, IsPrimary = true
          },
          new()
          {
            CategoryId = 4, IsPrimary = false
          }
        },
        PublishTime = null,
        Status = StatusType.Draft,
        DetailImageIds = new List<NewsDetailImageDto>()
        {
          new()
          {
            DetailImageId = _uploadImageId
          }
        }
      };

    }

    [Fact]
    public async Task CreateAsync_CheckTagDuplicateInput_BusinessException()
    {
      using (_currentTenant.Change(_tenantId))
      {
        _createNewsDto.Title = "News Haber 0";
        _createNewsDto.TagIds = new List<int>() { 1, 1 };

        var exception = await Assert.ThrowsAsync<BusinessException>(async () =>
        {
          await _newsAppService.CreateAsync(_createNewsDto);
        });

        Assert.Equal(NewsManagementDomainErrorCodes.RepeatedDataError, exception.Code);
        Assert.Equal("tagIds", exception.Data["0"]);
        Assert.Equal("1", exception.Data["1"]);
      }
    }

    [Fact]
    public async Task UpdateAsync_ReturnValue_NewsDto()
    {
      using (_currentTenant.Change(_tenantId))
      {
        var updateNews = _objectMapper.Map<CreateNewsDto, UpdateNewsDto>(_createNewsDto);
        updateNews.Title = "News Haber 0";
        var id = 3;

        var result = await _newsAppService.UpdateAsync(id, updateNews);

        Assert.NotNull(result);
        Assert.IsType<NewsDto>(result);
      }
    }

    [Fact]
    public async Task GetListAsync_FilterValid_NewsDto()
    {
      using (_currentTenant.Change(_tenantId))
      {
        var categoryList = await _newsAppService.GetListAsync(new GetListPagedAndSortedDto() { Filter = "Haber 1" });

        categoryList.Items.ShouldContain(x => x.Title == "News Haber 1");
      }
    }

    [Fact]
    public async Task GetAsync_IdValid_ViewsCountIncrease()
    {
      using (_currentTenant.Change(_tenantId))
      {
        int id = 1;

        var viewsCount1 = (await _newsAppService.GetAsync(id)).ViewsCount;
        var viewsCount2 = (await _newsAppService.GetAsync(id)).ViewsCount;

        Assert.Equal(viewsCount1 + 1, viewsCount2);
      }
    }

  }
}
