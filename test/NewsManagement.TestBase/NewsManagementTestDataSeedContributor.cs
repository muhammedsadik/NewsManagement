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
  public NewsManagementTestDataSeedContributor(IRepository<Tag, int> tagRepository, IRepository<City, int> cityRepository)
  {
    _tagRepository = tagRepository;
    _cityRepository = cityRepository;
  }

  public async Task SeedAsync(DataSeedContext context)
  {
    await SeedTagAsync();
  
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

}
