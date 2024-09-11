using NewsManagement.AppService.Categories;
using NewsManagement.Entities.Categories;
using NewsManagement.Entities.Exceptions;
using NewsManagement.EntityConsts.ListableContentConsts;
using NewsManagement.EntityDtos.CategoryDtos;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace NewsManagement.Category
{
  public class CategoryAppService_Test : NewsManagementApplicationTestBase
  {
    private readonly CategoryAppService _categoryAppService;
    private readonly IDataFilter<IMultiTenant> _dataFilter;

    public CategoryAppService_Test()
    {
      _categoryAppService = GetRequiredService<CategoryAppService>();
      _dataFilter = GetRequiredService<IDataFilter<IMultiTenant>>();
    }

    [Fact]
    public async Task CreateAsync_MainCategoryNameInValid_AlreadyExistException()
    {
      using (_dataFilter.Disable())
      {
        CreateCategoryDto category = new()
        {
          CategoryName = "Kültür",
          ColorCode = "#a6e79f",
          IsActive = true,
          listableContentType = ListableContentType.Gallery
        };

        await Assert.ThrowsAsync<AlreadyExistException>(async () =>
        {
          await _categoryAppService.CreateAsync(category);
        });
      }
    }

    [Fact]
    public async Task CreateAsync_SubCategoryNameInValid_AlreadyExistException()
    {
      using (_dataFilter.Disable())
      {
        CreateCategoryDto category = new()
        {
          CategoryName = "Yaşam",
          ColorCode = "#a6e79f",
          IsActive = true,
          ParentCategoryId = 1,
          listableContentType = ListableContentType.Gallery
        };

        await Assert.ThrowsAsync<AlreadyExistException>(async () =>
        {
          await _categoryAppService.CreateAsync(category);
        });
      }
    }

    [Fact]
    public async Task CreateAsync_SubCategory_BusinessException()
    {
      using (_dataFilter.Disable())
      {
        CreateCategoryDto category = new()
        {
          CategoryName = "Spor",
          ColorCode = "#f6e79f",
          IsActive = true,
          ParentCategoryId = 6,
          listableContentType = ListableContentType.Gallery
        };

        await Assert.ThrowsAsync<BusinessException>(async () =>
        {
          await _categoryAppService.CreateAsync(category);
        });
      }
    }

    [Fact]
    public async Task UpdateAsync_ReturnValue_CategoryDto()
    {
      using (_dataFilter.Disable())
      {
        int categoryId = 2;
        UpdateCategoryDto category = new()
        {
          CategoryName = "İç Ekonomi",
          ColorCode = "#ff179a",
          IsActive = true,
          ParentCategoryId = null,
          listableContentType = ListableContentType.Video
        };

        var result = await _categoryAppService.UpdateAsync(categoryId, category);

        Assert.NotNull(result);
        Assert.IsType<CategoryDto>(result);
      }
    }

    [Fact]
    public async Task GetListAsync_RetrunValue_FilterData()
    {
      using (_dataFilter.Disable())
      {
        var categoryList = await _categoryAppService.GetListAsync(new GetListPagedAndSortedDto() { Filter = "Siya" });

        categoryList.Items.ShouldContain(c => c.CategoryName == "Siyaset");
      }
    }

    [Fact]
    public async Task DeleteAsync_IdInValid_EntityNotFoundException()
    {
      using (_dataFilter.Disable())
      {
        int categoryId = 0;

        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
          await _categoryAppService.DeleteAsync(categoryId);
        });
      }
    }

    [Fact]
    public async Task DeleteHardAsync_IdInValid_EntityNotFoundException()
    {
      using (_dataFilter.Disable())
      {
        int categoryId = 0;

        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
          await _categoryAppService.DeleteHardAsync(categoryId);
        });
      }
    }

    [Fact]
    public async Task GetSubCategoriesById_IdValid_ReturnEntity()
    {
      using (_dataFilter.Disable())
      {
        var id = 6;
        var categories = await _categoryAppService.GetSubCategoriesById(id);

        Assert.NotNull(categories);
        Assert.Equal(3, categories.Count);
      }
    }


  }
}
