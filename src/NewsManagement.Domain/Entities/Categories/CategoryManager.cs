using NewsManagement.Entities.Exceptions;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Domain.Repositories;
using NewsManagement.EntityDtos.CategoryDtos;
using Microsoft.AspNetCore.Authorization;
using NewsManagement.Permissions;
using Volo.Abp.Data;

namespace NewsManagement.Entities.Categories
{
  public class CategoryManager : DomainService
  {
    private readonly IObjectMapper _objectMapper;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IDataFilter<ISoftDelete> _softDeleteFilter;
    public CategoryManager(ICategoryRepository categoryRepository, IObjectMapper objectMapper, IDataFilter<ISoftDelete> softDeleteFilter)
    {
      _categoryRepository = categoryRepository;
      _softDeleteFilter = softDeleteFilter;
      _objectMapper = objectMapper;
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto createCategoryDto)
    {
      var category = _objectMapper.Map<CreateCategoryDto, Category>(createCategoryDto);

      if (!category.ParentCategoryId.HasValue)
        await CheckMainCategoryIsValidAsync(category);

      if (category.ParentCategoryId.HasValue)
        category = await CheckSubCategoryIsValidAsync(category);

      var createdCategory = await _categoryRepository.InsertAsync(category);

      var categoryDto = _objectMapper.Map<Category, CategoryDto>(createdCategory);

      return categoryDto;
    }

    public async Task CheckMainCategoryIsValidAsync(Category category)
    {
      var isExistCategory = await _categoryRepository.AnyAsync(
        c => c.CategoryName == category.CategoryName
        && c.ParentCategoryId == category.ParentCategoryId
        && c.Id != category.Id
      );
      if (isExistCategory)
        throw new AlreadyExistException(typeof(Category), category.CategoryName);
    }

    public async Task<Category> CheckSubCategoryIsValidAsync(Category category, int? id = null)
    {
      var isExistCategory = await _categoryRepository.AnyAsync(
        c => c.CategoryName == category.CategoryName
        && c.ParentCategoryId == category.ParentCategoryId
        && c.Id != id
      );
      if (isExistCategory)
        throw new AlreadyExistException(typeof(Category), category.CategoryName);

      var parentCategory = await _categoryRepository.GetAsync((int)category.ParentCategoryId);

      if (parentCategory.ParentCategoryId.HasValue)
        throw new BusinessException(NewsManagementDomainErrorCodes.OnlyOneSubCategory);

      if (parentCategory.IsActive == false)
        category.IsActive = parentCategory.IsActive;

      return category;
    }

    public async Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto)
    {
      if (!(await _categoryRepository.GetAsync(id)).ParentCategoryId.HasValue && updateCategoryDto.ParentCategoryId.HasValue)
        await ChangingParentIdOfMainCategory(id);

      var existingCategory = await _categoryRepository.GetAsync(id);

      var updatingCategory = _objectMapper.Map(updateCategoryDto, existingCategory);

      if (!updatingCategory.ParentCategoryId.HasValue)
        await CheckMainCategoryIsValidAsync(updatingCategory);

      if (updatingCategory.ParentCategoryId.HasValue)
        updatingCategory = await CheckSubCategoryIsValidAsync(updatingCategory, id);

      var category = await _categoryRepository.UpdateAsync(updatingCategory);

      if (!updatingCategory.ParentCategoryId.HasValue)
        if (updatingCategory.IsActive == false)
          await UpdateSubCategoryByMainIsActiveAsync(updatingCategory);

      var categoryDto = _objectMapper.Map<Category, CategoryDto>(category);
      return categoryDto;
    }

    private async Task ChangingParentIdOfMainCategory(int id)
    {
      if (await _categoryRepository.AnyAsync(c => c.ParentCategoryId == id))
        throw new BusinessException(NewsManagementDomainErrorCodes.MainCategoryWithSubCannotBeChanged);

    }

    private async Task UpdateSubCategoryByMainIsActiveAsync(Category category)
    {
      var subCategories = await _categoryRepository.GetListAsync(c => c.ParentCategoryId == category.Id);

      foreach (var subCategory in subCategories)
      {
        subCategory.IsActive = category.IsActive;
        await _categoryRepository.UpdateAsync(subCategory);
      }
    }

    public async Task<PagedResultDto<CategoryDto>> GetListAsync(GetListPagedAndSortedDto input)
    {
      var totalCount = input.Filter == null
        ? await _categoryRepository.CountAsync()
        : await _categoryRepository.CountAsync(c => c.CategoryName.Contains(input.Filter));

      if (totalCount == 0)
        throw new NotFoundException(typeof(Category), input.Filter ?? string.Empty);

      if (input.SkipCount >= totalCount)
        throw new BusinessException(NewsManagementDomainErrorCodes.FilterLimitsError);

      if (input.Sorting.IsNullOrWhiteSpace())
        input.Sorting = nameof(Category.CategoryName);

      var categoryList = await _categoryRepository.GetListAsync(input.SkipCount, input.MaxResultCount, input.Sorting, input.Filter);

      var categoryDtoList = _objectMapper.Map<List<Category>, List<CategoryDto>>(categoryList);

      return new PagedResultDto<CategoryDto>(totalCount, categoryDtoList);
    }

    public async Task<List<Category>> DeleteAsync(int id)
    {
      var category = await _categoryRepository.GetAsync(c => c.Id == id);

      var deletingList = new List<Category>();

      if (!category.ParentCategoryId.HasValue)
      {
        deletingList.AddRange(await _categoryRepository.GetListAsync(c => c.ParentCategoryId == id));
      }

      deletingList.Add(category);

      return deletingList;
    }

    public async Task DeleteHardAsync(int id)
    {
      var category = await _categoryRepository.GetAsync(id);

      if (!category.ParentCategoryId.HasValue)
      {
        using (_softDeleteFilter.Disable())
        {
          var deletingList = await _categoryRepository.GetListAsync(c => c.ParentCategoryId == id);
          deletingList.Add(category);

          await _categoryRepository.HardDeleteAsync(deletingList);
        }
      }

      await _categoryRepository.HardDeleteAsync(category);
    }

    public async Task<List<Category>> GetSubCategoriesById(int id)
    {
      var category = await _categoryRepository.GetAsync(id);

      if (category.IsActive == false)
        throw new NotFoundException(typeof(Category), id.ToString());

      List<Category> listCategory = new();

      if (!category.ParentCategoryId.HasValue)
      {
        listCategory.AddRange(await _categoryRepository.GetListAsync(c => c.ParentCategoryId == id && c.IsActive==true));
        listCategory.Add(category);
      }

      if (category.ParentCategoryId.HasValue)
      {
        listCategory.AddRange(await _categoryRepository.GetListAsync(c => c.ParentCategoryId == category.ParentCategoryId && c.IsActive == true));
        listCategory.Add(await _categoryRepository.GetAsync((int)category.ParentCategoryId));
      }

      return listCategory;
    }


  }
}