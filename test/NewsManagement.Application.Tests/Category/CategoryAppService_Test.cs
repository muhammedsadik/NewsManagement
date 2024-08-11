using NewsManagement.AppService.Categories;
using NewsManagement.Entities.Exceptions;
using NewsManagement.EntityDtos.CategoryDtos;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Xunit;

namespace NewsManagement.Category
{
  public class CategoryAppService_Test : NewsManagementApplicationTestBase
  {
    private readonly CategoryAppService _categoryAppService;

    public CategoryAppService_Test()
    {
      _categoryAppService = GetRequiredService<CategoryAppService>();

    }

    [Fact]
    public async Task CreateAsync_TagNameInValid_AlreadyExistException()
    {
      CreateCategoryDto category = new() { CategoryName = "Sanat", ColorCode = "#a6e79f", IsActive = true };

      await Assert.ThrowsAsync<AlreadyExistException>(async () =>
      {
        await _categoryAppService.CreateAsync(category);
      });

    }

    [Fact]
    public async Task UpdateAsync_ReturnValue_TagDto()
    {
      int categoryId = 2;
      UpdateCategoryDto category = new() { CategoryName = "Eğitim", ColorCode = "#ff179a", IsActive = true };

      var result = await _categoryAppService.UpdateAsync(categoryId, category);

      Assert.NotNull(result);
      Assert.IsType<CategoryDto>(result);
    }

    [Fact]
    public async Task GetListAsync_RetrunValue_FilterData()
    {
      var tagList = await _categoryAppService.GetListAsync(new GetListPagedAndSortedDto() { Filter = "Tek" });

      tagList.Items.ShouldContain(c => c.CategoryName == "Teknoloji");
    }

    [Fact]
    public async Task DeleteAsynce_IdInValid_EntityNotFoundException()
    {
      int categoryId = 30;

      await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
      {
        await _categoryAppService.DeleteAsync(categoryId);
      });
    }

    [Fact]
    public async Task DeleteHardAsynce_IdInValid_EntityNotFoundException()
    {
      int categoryId = 30;

      await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
      {
        await _categoryAppService.DeleteHardAsync(categoryId);
      });
    }
  }
}
