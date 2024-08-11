using NewsManagement.Entities.Tags;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace NewsManagement;

public class NewsManagementTestDataSeedContributor : IDataSeedContributor, ITransientDependency
{
  private readonly IRepository<Tag, int> _tagRepository;

  public NewsManagementTestDataSeedContributor(IRepository<Tag, int> tagRepository)
  {
    _tagRepository = tagRepository;
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


}
