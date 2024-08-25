using NewsManagement.Entities.Categories;
using NewsManagement.Entities.Cities;
using NewsManagement.Entities.Tags;
using NewsManagement.EntityConsts.RoleConsts;
using NewsManagement.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

namespace NewsManagement
{
  public class NewsManagementDataSeederContributor : IDataSeedContributor, ITransientDependency
  {
    private readonly IGuidGenerator _guidGenerator;
    private readonly IRepository<Tag, int> _tagRepository;
    private readonly IRepository<City, int> _cityRepository;
    private readonly IRepository<Category, int> _categoryRepository;
    private readonly IRepository<IdentityUser, Guid> _userRepository;
    private readonly IRepository<IdentityRole, Guid> _roleRepository;
    private readonly IdentityUserManager _identityUserManager;
    private readonly IdentityRoleManager _identityRoleManager;
    private readonly IPermissionManager _permissionManager;

    public NewsManagementDataSeederContributor(
      IGuidGenerator guidGenerator,
      IRepository<Tag, int> tagRepository,
      IRepository<City, int> cityRepository,
      IRepository<Category, int> categoryRepository,
      IRepository<IdentityUser, Guid> userRepository,
      IRepository<IdentityRole, Guid> roleRepository,
      IdentityRoleManager identityRoleManager,
      IdentityUserManager identityUserManager,
      IPermissionManager permissionManager
      )
    {
      _guidGenerator = guidGenerator;
      _tagRepository = tagRepository;
      _cityRepository = cityRepository;
      _categoryRepository = categoryRepository;
      _userRepository = userRepository;
      _roleRepository = roleRepository;
      _identityRoleManager = identityRoleManager;
      _identityUserManager = identityUserManager;
      _permissionManager = permissionManager;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
      await SeedRoleAsync();
      await SeedTagAsync();
      await SeedCityAsync();
      await SeedCategoryAsync();
    }


    #region Role

    private async Task SeedRoleAsync()
    {
      if (!await _identityRoleManager.RoleExistsAsync(RoleConst.Editor))
      {
        await _identityRoleManager.CreateAsync(
          new IdentityRole
          (
            _guidGenerator.Create(),
            RoleConst.Editor
          )
        );

        await _permissionManager.SetForRoleAsync(RoleConst.Editor, NewsManagementPermissions.Tags.Default, true);
        await _permissionManager.SetForRoleAsync(RoleConst.Editor, NewsManagementPermissions.Tags.Create, true);
        await _permissionManager.SetForRoleAsync(RoleConst.Editor, NewsManagementPermissions.Tags.Edit, true);
        await _permissionManager.SetForRoleAsync(RoleConst.Editor, NewsManagementPermissions.Tags.Delete, true);
        
        await _permissionManager.SetForRoleAsync(RoleConst.Editor, NewsManagementPermissions.Cities.Default, true);
        await _permissionManager.SetForRoleAsync(RoleConst.Editor, NewsManagementPermissions.Cities.Create, true);
        await _permissionManager.SetForRoleAsync(RoleConst.Editor, NewsManagementPermissions.Cities.Edit, true);
        await _permissionManager.SetForRoleAsync(RoleConst.Editor, NewsManagementPermissions.Cities.Delete, true);
        
        await _permissionManager.SetForRoleAsync(RoleConst.Editor, NewsManagementPermissions.Categories.Default, true);
        await _permissionManager.SetForRoleAsync(RoleConst.Editor, NewsManagementPermissions.Categories.Create, true);
        await _permissionManager.SetForRoleAsync(RoleConst.Editor, NewsManagementPermissions.Categories.Edit, true);
        await _permissionManager.SetForRoleAsync(RoleConst.Editor, NewsManagementPermissions.Categories.Delete, true);
        
        await _permissionManager.SetForRoleAsync(RoleConst.Editor, NewsManagementPermissions.Videos.Default, true);
        await _permissionManager.SetForRoleAsync(RoleConst.Editor, NewsManagementPermissions.Videos.Create, true);
        await _permissionManager.SetForRoleAsync(RoleConst.Editor, NewsManagementPermissions.Videos.Edit, true);
        await _permissionManager.SetForRoleAsync(RoleConst.Editor, NewsManagementPermissions.Videos.Delete, true);
        
        await _permissionManager.SetForRoleAsync(RoleConst.Editor, NewsManagementPermissions.Galleries.Default, true);
        await _permissionManager.SetForRoleAsync(RoleConst.Editor, NewsManagementPermissions.Galleries.Create, true);
        await _permissionManager.SetForRoleAsync(RoleConst.Editor, NewsManagementPermissions.Galleries.Edit, true);
        await _permissionManager.SetForRoleAsync(RoleConst.Editor, NewsManagementPermissions.Galleries.Delete, true);
        
        await _permissionManager.SetForRoleAsync(RoleConst.Editor, NewsManagementPermissions.Newses.Default, true);
        await _permissionManager.SetForRoleAsync(RoleConst.Editor, NewsManagementPermissions.Newses.Create, true);
        await _permissionManager.SetForRoleAsync(RoleConst.Editor, NewsManagementPermissions.Newses.Edit, true);
        await _permissionManager.SetForRoleAsync(RoleConst.Editor, NewsManagementPermissions.Newses.Delete, true);

      }

      if (!await _identityRoleManager.RoleExistsAsync(RoleConst.Reporter))
      {
        await _identityRoleManager.CreateAsync(
          new IdentityRole
          (
            _guidGenerator.Create(),
            RoleConst.Reporter
          )
        );

        await _permissionManager.SetForRoleAsync(RoleConst.Reporter, NewsManagementPermissions.Videos.Default, true);
        await _permissionManager.SetForRoleAsync(RoleConst.Reporter, NewsManagementPermissions.Videos.Create, true);
                                                           
        await _permissionManager.SetForRoleAsync(RoleConst.Reporter, NewsManagementPermissions.Galleries.Default, true);
        await _permissionManager.SetForRoleAsync(RoleConst.Reporter, NewsManagementPermissions.Galleries.Create, true);
                                                          
        await _permissionManager.SetForRoleAsync(RoleConst.Reporter, NewsManagementPermissions.Newses.Default, true);
        await _permissionManager.SetForRoleAsync(RoleConst.Reporter, NewsManagementPermissions.Newses.Create, true);

      }









    }

    #endregion

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
          IsActive = true,
          ParentCategoryId = null
        },
        autoSave: true
      );

      await _categoryRepository.InsertAsync(
        new Category()
        {
          CategoryName = "Ekonomi",
          ColorCode = "#148f77",
          IsActive = true,
          ParentCategoryId = null
        },
        autoSave: true
      );

      await _categoryRepository.InsertAsync(
        new Category()
        {
          CategoryName = "Siyaset",
          ColorCode = "#ec7063",
          IsActive = true,
          ParentCategoryId = null
        },
        autoSave: true
      );

      await _categoryRepository.InsertAsync(
        new Category()
        {
          CategoryName = "Asya Kültürü",
          ColorCode = "#ec70ff",
          IsActive = true,
          ParentCategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Kültür")).Id
        },
        autoSave: true
      );

      await _categoryRepository.InsertAsync(
        new Category()
        {
          CategoryName = "Yaşam",
          ColorCode = "#8c7063",
          IsActive = true,
          ParentCategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Kültür")).Id
        },
        autoSave: true
      );

      await _categoryRepository.InsertAsync(
        new Category()
        {
          CategoryName = "Makroekonomi",
          ColorCode = "#7c0e63",
          IsActive = true,
          ParentCategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Ekonomi")).Id
        },
        autoSave: true
      );

    }
    #endregion





  }
}
