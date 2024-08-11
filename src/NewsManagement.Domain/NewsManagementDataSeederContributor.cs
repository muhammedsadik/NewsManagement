using NewsManagement.Entities.Categories;
using NewsManagement.Entities.Cities;
using NewsManagement.Entities.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace NewsManagement
{
  public class NewsManagementDataSeederContributor : IDataSeedContributor, ITransientDependency
  {
    private readonly IRepository<Tag, int> _tagRepository;
    private readonly IRepository<City, int> _cityRepository;
    private readonly IRepository<Category, int> _categoryRepository;

    public NewsManagementDataSeederContributor(
      IRepository<Tag, int> tagRepository,
      IRepository<City, int> cityRepository,
      IRepository<Category, int> categoryRepository)
    {
      _tagRepository = tagRepository;
      _cityRepository = cityRepository;
      _categoryRepository = categoryRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
      await SeedTagAsync();
      await SeedCityAsync();
      await SeedCategoryAsync();
    }

    #region Tag
    private async Task SeedTagAsync()
    {
      if (await _tagRepository.CountAsync() > 0)
        return;

      await _tagRepository.InsertAsync(
        new Tag()
        {
          TagName = "Yaşam"
        },
        autoSave: true
      );

      await _tagRepository.InsertAsync(
        new Tag()
        {
          TagName = "Teknoloji"
        },
        autoSave: true
      );

      await _tagRepository.InsertAsync(
        new Tag()
        {
          TagName = "Spor"
        },
        autoSave: true
      );

    }
    #endregion

    #region City
    private async Task SeedCityAsync()
    {
      if (await _cityRepository.CountAsync() > 0)
        return;

      await _cityRepository.InsertAsync(
        new City()
        {
          CityName = "İstanbul",
          CityCode = 34
        },
        autoSave: true
      );

      await _cityRepository.InsertAsync(
        new City()
        {
          CityName = "Ankara",
          CityCode = 06
        },
        autoSave: true
      );

      await _cityRepository.InsertAsync(
        new City()
        {
          CityName = "Diyarbakır",
          CityCode = 21
        },
        autoSave: true
      );

    }
    #endregion

    #region Category
    private async Task SeedCategoryAsync()
    {
      if (await _categoryRepository.CountAsync() > 0)
        return;

      await _categoryRepository.InsertAsync(
        new Category()
        {
          CategoryName = "Kültür",
          ColorCode = "#f9e79f",
          IsActive = true          
        },
        autoSave: true
      );
      
      await _categoryRepository.InsertAsync(
        new Category()
        {
          CategoryName = "Ekonomi",
          ColorCode = "#148f77",
          IsActive = true          
        },
        autoSave: true
      );
      
      await _categoryRepository.InsertAsync(
        new Category()
        {
          CategoryName = "Siyaset",
          ColorCode = "#ec7063",
          IsActive = true          
        },
        autoSave: true
      );


    }
    #endregion

  }
}
