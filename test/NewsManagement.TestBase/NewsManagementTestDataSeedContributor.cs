using NewsManagement.Entities.Categories;
using NewsManagement.Entities.Cities;
using NewsManagement.Entities.Tags;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace NewsManagement;

public class NewsManagementTestDataSeedContributor : IDataSeedContributor, ITransientDependency
{
  private readonly IRepository<Tag, int> _tagRepository;
  private readonly IRepository<City, int> _cityRepository;
  private readonly IRepository<Category, int> _categoryRepository;
  public NewsManagementTestDataSeedContributor(
    IRepository<Tag, int> tagRepository,
    IRepository<City, int> cityRepository,
    IRepository<Category, int> categoryRepository
  )
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
      }
    );

    await _cityRepository.InsertAsync(
      new City()
      {
        CityName = "Ankara",
        CityCode = 06
      }
    );

    await _cityRepository.InsertAsync(
      new City()
      {
        CityName = "Diyarbakır",
        CityCode = 21
      }
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

    await _categoryRepository.InsertAsync(
        new Category()
        {
          CategoryName = "Asya Kültürü",
          ColorCode = "#ec70ff",
          IsActive = true,
          ParentCategoryId = 1
        },
        autoSave: true
      );

    await _categoryRepository.InsertAsync(
      new Category()
      {
        CategoryName = "Yaşam",
        ColorCode = "#8c7063",
        IsActive = true,
        ParentCategoryId = 1
      },
      autoSave: true
    );

    await _categoryRepository.InsertAsync(
      new Category()
      {
        CategoryName = "Makroekonomi",
        ColorCode = "#7c0e63",
        IsActive = true,
        ParentCategoryId = 2
      },
      autoSave: true
    );

  }
  #endregion

}
