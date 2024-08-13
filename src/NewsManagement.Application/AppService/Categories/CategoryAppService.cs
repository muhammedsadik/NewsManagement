using NewsManagement.Entities.Categories;
using NewsManagement.EntityDtos.CategoryDtos;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NewsManagement.Permissions;
using Volo.Abp.Application.Dtos;

namespace NewsManagement.AppService.Categories
{

  [Authorize(NewsManagementPermissions.Categories.Default)]
  public class CategoryAppService : CrudAppService<Category, CategoryDto, int, GetListPagedAndSortedDto, CreateCategoryDto, UpdateCategoryDto>, ICategoryAppService
  {
    private readonly CategoryManager _categoryManager;

    public CategoryAppService(IRepository<Category, int> repository, CategoryManager categoryManager) : base(repository)
    {
      _categoryManager = categoryManager;
    }


    [Authorize(NewsManagementPermissions.Categories.Create)]
    public override async Task<CategoryDto> CreateAsync(CreateCategoryDto createCategoryDto)
    {
      return await _categoryManager.CreateAsync(createCategoryDto);
    }

    [Authorize(NewsManagementPermissions.Categories.Edit)]
    public async override Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto)
    {
      return await _categoryManager.UpdateAsync(id, updateCategoryDto);
    }

    public async override Task<PagedResultDto<CategoryDto>> GetListAsync(GetListPagedAndSortedDto input)
    {
      return await _categoryManager.GetListAsync(input);
    }

    [Authorize(NewsManagementPermissions.Categories.Delete)]
    public override async Task DeleteAsync(int id)
    {
      var category = await _categoryManager.DeleteAsync(id);

      foreach (var item in category)
        await base.DeleteAsync(item.Id);
    }

    [Authorize(NewsManagementPermissions.Categories.Delete)]
    public async Task DeleteHardAsync(int id)
    {
      await _categoryManager.DeleteHardAsync(id);
    }

    public async Task<List<Category>> GetSubCategoriesById(int id)
    {
      return await _categoryManager.GetSubCategoriesById(id);
    }
  }
}
