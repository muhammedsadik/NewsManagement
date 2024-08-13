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

namespace NewsManagement.Entities.Categories
{
  public class CategoryManager : DomainService
  {
    private readonly IObjectMapper _objectMapper;
    private readonly ICategoryRepository _categoryRepository;

    public CategoryManager(ICategoryRepository categoryRepository, IObjectMapper objectMapper)
    {
      _categoryRepository = categoryRepository;
      _objectMapper = objectMapper;
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto createCategoryDto)
    {
      if (!createCategoryDto.ParentCategoryId.HasValue)
        await CheckMainCategoryIsValidAsync(createCategoryDto.CategoryName);
      
      if (createCategoryDto.ParentCategoryId.HasValue)
        await CheckSubCategoryIsValidAsync(createCategoryDto.CategoryName, (int)createCategoryDto.ParentCategoryId);

      var createCategory = _objectMapper.Map<CreateCategoryDto, Category>(createCategoryDto);

      var category = await _categoryRepository.InsertAsync(createCategory);
      var categoryDto = _objectMapper.Map<Category, CategoryDto>(category);
      return categoryDto;
    }

    public async Task CheckMainCategoryIsValidAsync(string categoryName)
    {
      var isExistCategory = await _categoryRepository.AnyAsync(c => c.CategoryName == categoryName);
      if (isExistCategory)
        throw new AlreadyExistException(typeof(Category), categoryName);
    }

    public async Task CheckSubCategoryIsValidAsync(string categoryName, int parentCategoryId, int? id = null)
    {
      var isExistCategory = await _categoryRepository.AnyAsync(
        c => c.CategoryName == categoryName 
        && c.ParentCategoryId == parentCategoryId
        && c.Id != id
      );
      if (isExistCategory)
        throw new AlreadyExistException(typeof(Category), categoryName);

      var category = await _categoryRepository.GetAsync(parentCategoryId);

      if (category.ParentCategoryId.HasValue)
        throw new BusinessException(NewsManagementDomainErrorCodes.JustOneSubCategory);
    }

    public async Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto)
    {
      var existingCategory = await _categoryRepository.GetAsync(id);

      if (!updateCategoryDto.ParentCategoryId.HasValue)
        await CheckMainCategoryIsValidAsync(updateCategoryDto.CategoryName);

      if (updateCategoryDto.ParentCategoryId.HasValue)
        await CheckSubCategoryIsValidAsync(updateCategoryDto.CategoryName, (int)updateCategoryDto.ParentCategoryId, id);

      _objectMapper.Map(updateCategoryDto, existingCategory);

      var category = await _categoryRepository.UpdateAsync(existingCategory);

      var categoryDto = _objectMapper.Map<Category, CategoryDto>(category);
      return categoryDto;
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

    public async Task DeleteAsync(int id)
    {
      var isCategoryExist = await _categoryRepository.AnyAsync(c => c.Id == id);
      if (!isCategoryExist)
        throw new EntityNotFoundException(typeof(Category), id);
    }

    public async Task DeleteHardAsync(int id)
    {
      var category = await _categoryRepository.GetAsync(id);

      await _categoryRepository.HardDeleteAsync(category);
    }



  }
}
