using EasyAbp.FileManagement.Files;
using EasyAbp.FileManagement.Options.Containers;
using Microsoft.AspNetCore.StaticFiles;
using NewsManagement.Entities.Categories;
using NewsManagement.Entities.Cities;
using NewsManagement.Entities.Galleries;
using NewsManagement.Entities.ListableContentRelations;
using NewsManagement.Entities.Newses;
using NewsManagement.Entities.Tags;
using NewsManagement.Entities.Videos;
using NewsManagement.EntityConsts.ListableContentConsts;
using NewsManagement.EntityConsts.RoleConsts;
using NewsManagement.EntityConsts.VideoConsts;
using NewsManagement.Permissions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TenantManagement;
using static NewsManagement.Permissions.NewsManagementPermissions;
using static System.Net.Mime.MediaTypeNames;
using static Volo.Abp.TenantManagement.TenantManagementPermissions;

namespace NewsManagement
{
  public class NewsManagementDataSeederContributor : IDataSeedContributor, ITransientDependency
  {
    private readonly IGuidGenerator _guidGenerator;
    private readonly IRepository<Tag, int> _tagRepository;
    private readonly IRepository<City, int> _cityRepository;
    private readonly IRepository<Category, int> _categoryRepository;
    private readonly IRepository<News, int> _newsRepository;
    private readonly IRepository<Video, int> _videoRepository;
    private readonly IRepository<Gallery, int> _galleryRepository;
    private readonly IRepository<ListableContentTag> _listableContentTagRepository;
    private readonly IRepository<ListableContentCity> _listableContentCityRepository;
    private readonly IRepository<ListableContentCategory> _listableContentCategoryRepository;
    private readonly IRepository<ListableContentRelation> _listableContentRelationRepository;
    private readonly IRepository<IdentityUser, Guid> _userRepository;
    private readonly IRepository<IdentityRole, Guid> _roleRepository;
    private readonly IdentityUserManager _identityUserManager;
    private readonly IdentityRoleManager _identityRoleManager;
    private readonly IPermissionManager _permissionManager;
    private readonly ITenantManager _tenantManager;
    private readonly ITenantRepository _tenantRepository;
    private readonly ICurrentTenant _currentTenant;
    private readonly FileManager _fileManager;
    private readonly IFileContentHashProvider _fileContentHashProvider;
    private readonly IFileRepository _fileRepository;
    private readonly IFileBlobNameGenerator _fileBlobNameGenerator;
    private readonly IFileContainerConfigurationProvider _configurationProvider;

    public NewsManagementDataSeederContributor(
      IFileBlobNameGenerator fileBlobNameGenerator,
      IFileContainerConfigurationProvider configurationProvider,
      IGuidGenerator guidGenerator,
      IRepository<Tag, int> tagRepository,
      IRepository<City, int> cityRepository,
      IRepository<Category, int> categoryRepository,
      IRepository<News, int> newsRepository,
      IRepository<Video, int> videoRepository,
      IRepository<Gallery, int> galleryRepository,
      IRepository<ListableContentTag> listableContentTagRepository,
      IRepository<ListableContentCity> listableContentCityRepository,
      IRepository<ListableContentCategory> listableContentCategoryRepository,
      IRepository<ListableContentRelation> listableContentRelationRepository,
      IRepository<IdentityUser, Guid> userRepository,
      IRepository<IdentityRole, Guid> roleRepository,
      IdentityRoleManager identityRoleManager,
      IdentityUserManager identityUserManager,
      IPermissionManager permissionManager,
      ITenantManager tenantManager,
      ITenantRepository tenantRepository,
      ICurrentTenant currentTenant,
      FileManager fileManager,
      IFileContentHashProvider fileContentHashProvider,
      IFileRepository fileRepository
      )
    {
      _fileBlobNameGenerator = fileBlobNameGenerator;
      _configurationProvider = configurationProvider;
      _guidGenerator = guidGenerator;
      _tagRepository = tagRepository;
      _cityRepository = cityRepository;
      _categoryRepository = categoryRepository;
      _newsRepository = newsRepository;
      _videoRepository = videoRepository;
      _galleryRepository = galleryRepository;
      _listableContentTagRepository = listableContentTagRepository;
      _listableContentCityRepository = listableContentCityRepository;
      _listableContentCategoryRepository = listableContentCategoryRepository;
      _listableContentRelationRepository = listableContentRelationRepository;
      _userRepository = userRepository;
      _roleRepository = roleRepository;
      _identityRoleManager = identityRoleManager;
      _identityUserManager = identityUserManager;
      _permissionManager = permissionManager;
      _tenantManager = tenantManager;
      _tenantRepository = tenantRepository;
      _currentTenant = currentTenant;
      _fileManager = fileManager;
      _fileContentHashProvider = fileContentHashProvider;
      _fileRepository = fileRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
      Guid? tenantId = _currentTenant.Id; //.HasValue ? _currentTenant.Id.Value : (Guid?)null;  //tenant eklerken çalışmaz sa (//) <= kaldır
      using (_currentTenant.Change(tenantId))
      {
        await SeedRoleAsync(tenantId);
        await SeedUserAsync(tenantId);
        await SeedTagAsync(tenantId);
        await SeedCityAsync(tenantId);
        await SeedCategoryAsync(tenantId);
        //await SeedFileAsync(tenantId);
        await SeedNewsAsync(tenantId);
        await SeedVideoAsync(tenantId);
        await SeedGalleryAsync(tenantId);
      }
    }

    #region Tenant

    //private async Task SeedTenantAsync()
    //{
    //  var isStaffTenantExist = await _tenantRepository.FindByNameAsync("Staff");
    //  if (isStaffTenantExist != null)
    //    return;

    //  await _tenantManager.CreateAsync("Staff");


    //}

    #endregion

    #region Role

    private async Task SeedRoleAsync(Guid? tenantId)
    {

      if (!await _identityRoleManager.RoleExistsAsync(RoleConst.Editor))
      {
        await _identityRoleManager.CreateAsync(
          new IdentityRole
          (
            _guidGenerator.Create(),
            RoleConst.Editor,
            tenantId: tenantId
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
            RoleConst.Reporter,
            tenantId: tenantId
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

    #region User

    private async Task SeedUserAsync(Guid? tenantId)
    {
      if (!await _userRepository.AnyAsync(x => x.UserName == "Ahmet"))
      {
        var userAhmet = new IdentityUser
          (
            _guidGenerator.Create(),
            "Ahmet",
            "ahmet@gmail.com",
            tenantId
          );

        await _identityUserManager.CreateAsync(userAhmet, "1q2w3E*");
        await _identityUserManager.SetRolesAsync(
          userAhmet, new List<string> { RoleConst.Editor }
        );
      }

      if (!await _userRepository.AnyAsync(x => x.UserName == "Halil"))
      {
        var userHalil = new IdentityUser
          (
            _guidGenerator.Create(),
            "Halil",
            "halil@gmail.com",
            tenantId
          );

        await _identityUserManager.CreateAsync(userHalil, "1q2w3E*");
        await _identityUserManager.SetRolesAsync(userHalil, new List<string> { RoleConst.Reporter });
      }

      if (!await _userRepository.AnyAsync(x => x.UserName == "Murat"))
      {
        var userMurat = new IdentityUser
          (
            _guidGenerator.Create(),
            "Murat",
            "murat@gmail.com",
            tenantId
          );

        await _identityUserManager.CreateAsync(userMurat, "1q2w3E*");
        await _identityUserManager.SetRolesAsync(userMurat, new List<string> { RoleConst.Reporter });
      }

    }

    #endregion

    #region Tag
    private async Task SeedTagAsync(Guid? tenantId)
    {
      if (tenantId.HasValue || await _tagRepository.CountAsync() > 0)
        return;

      await _tagRepository.InsertAsync(
        new Tag()
        {
          TagName = "Yaşam",
          TenantId = tenantId
        },
        autoSave: true
      );

      await _tagRepository.InsertAsync(
        new Tag()
        {
          TagName = "Teknoloji",
          TenantId = tenantId
        },
        autoSave: true
      );

      await _tagRepository.InsertAsync(
        new Tag()
        {
          TagName = "Spor",
          TenantId = tenantId
        },
        autoSave: true
      );

      await _tagRepository.InsertAsync(
        new Tag()
        {
          TagName = "Sanayi",
          TenantId = tenantId
        },
        autoSave: true
      );

    }
    #endregion

    #region City
    private async Task SeedCityAsync(Guid? tenantId)
    {
      if (await _cityRepository.CountAsync() > 0)
        return;

      await _cityRepository.InsertAsync(
        new City()
        {
          TenantId = tenantId,
          CityName = "İstanbul",
          CityCode = 34,
        },
        autoSave: true
      );

      await _cityRepository.InsertAsync(
        new City()
        {
          TenantId = tenantId,
          CityName = "Ankara",
          CityCode = 06
        },
        autoSave: true
      );

      await _cityRepository.InsertAsync(
        new City()
        {
          TenantId = tenantId,
          CityName = "Diyarbakır",
          CityCode = 21
        },
        autoSave: true
      );

      await _cityRepository.InsertAsync(
        new City()
        {
          TenantId = tenantId,
          CityName = "Konya",
          CityCode = 42
        },
        autoSave: true
      );

      await _cityRepository.InsertAsync(
        new City()
        {
          TenantId = tenantId,
          CityName = "Mardin",
          CityCode = 47
        },
        autoSave: true
      );

    }
    #endregion

    #region Category
    private async Task SeedCategoryAsync(Guid? tenantId)
    {
      if (await _categoryRepository.CountAsync() > 0)
        return;

      await _categoryRepository.InsertAsync(
        new Category()
        {
          CategoryName = "Kültür",
          ColorCode = "#f9e79f",
          IsActive = true,
          ParentCategoryId = null,
          listableContentType = ListableContentType.Gallery,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _categoryRepository.InsertAsync(
        new Category()
        {
          CategoryName = "Ekonomi",
          ColorCode = "#148f77",
          IsActive = true,
          ParentCategoryId = null,
          listableContentType = ListableContentType.Video,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _categoryRepository.InsertAsync(
        new Category()
        {
          CategoryName = "Siyaset",
          ColorCode = "#ec7063",
          IsActive = true,
          ParentCategoryId = null,
          listableContentType = ListableContentType.News,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _categoryRepository.InsertAsync(
        new Category()
        {
          CategoryName = "Sağlık",
          ColorCode = "#ec7063",
          IsActive = true,
          ParentCategoryId = null,
          listableContentType = ListableContentType.News,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _categoryRepository.InsertAsync(
        new Category()
        {
          CategoryName = "Eğitim",
          ColorCode = "#ec7063",
          IsActive = true,
          ParentCategoryId = null,
          listableContentType = ListableContentType.News,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _categoryRepository.InsertAsync(
        new Category()
        {
          CategoryName = "Asya Kültürü",
          ColorCode = "#ec70ff",
          IsActive = true,
          ParentCategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Kültür")).Id,
          listableContentType = ListableContentType.Gallery,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _categoryRepository.InsertAsync(
        new Category()
        {
          CategoryName = "Yaşam",
          ColorCode = "#8c7063",
          IsActive = true,
          ParentCategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Kültür")).Id,
          listableContentType = ListableContentType.Gallery,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _categoryRepository.InsertAsync(
        new Category()
        {
          CategoryName = "Makroekonomi",
          ColorCode = "#7c0e63",
          IsActive = true,
          ParentCategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Ekonomi")).Id,
          listableContentType = ListableContentType.Video,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _categoryRepository.InsertAsync(
        new Category()
        {
          CategoryName = "Mikroekonomi",
          ColorCode = "#7a0e65",
          IsActive = true,
          ParentCategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Ekonomi")).Id,
          listableContentType = ListableContentType.Video,
          TenantId = tenantId
        },
        autoSave: true
      );

    }
    #endregion

    #region File
    private async Task SeedFileAsync(Guid? tenantId)
    {

      var guid = Guid.Parse("77a4c000-a570-c250-60e0-18b9bf25b000");
      var rootPath = @"C:\Users\LENOVO\Desktop\NewsManagement\src\NewsManagement.Web\wwwroot\Files.png";
      var fileName = "Files.png";
      var fileContainerName = "default";
      var provider = new FileExtensionContentTypeProvider();
      provider.TryGetContentType(rootPath, out var mimeType);

      var byteSizeOfFiles = System.IO.File.ReadAllBytes(rootPath);



      var hashString = _fileContentHashProvider.GetHashString(byteSizeOfFiles); // FileManager  6 

      //var configuration = _configurationProvider.Get(fileContainerName);
      //var blobName = _fileBlobNameGenerator.CreateAsync(FileType.RegularFile, fileName, null, mimeType, configuration.AbpBlobDirectorySeparator);

      var newFile = await _fileManager.CreateAsync(
        fileContainerName, null, fileName.Trim() , mimeType, FileType.RegularFile, null, byteSizeOfFiles);

      //var file = new EasyAbp.FileManagement.Files.File(
      //  id: guid,
      //  tenantId: tenantId,
      //  parent: null,
      //  fileContainerName: fileContainerName,
      //  fileName: fileName,
      //  mimeType: mimeType,
      //  fileType: FileType.RegularFile,
      //  subFilesQuantity: 0,
      //  byteSize: byteSizeOfFiles.Length,
      //  hash: hashString,
      //  blobName: "local/",
      //  ownerUserId: null
      //);

      //var blobContainer = _fileManager.GetBlobContainer(file);
      //await blobContainer.SaveAsync(fileName, memoryStream, false, default);

      await _fileRepository.InsertAsync(newFile, autoSave: true);
      await _fileManager.TrySaveBlobAsync(newFile, byteSizeOfFiles);   // FileManager  11
    }

    #endregion

    #region News

    private async Task SeedNewsAsync(Guid? tenantId)
    {
      if (await _newsRepository.CountAsync() > 0)
        return;

      #region News Haber 1

      await _newsRepository.InsertAsync(
        new News()
        {
          Title = "News Haber 1",
          Spot = "News haber 1 içeriği",
          ImageId = _guidGenerator.Create(), // File dan gelen Id
          TenantId = tenantId,
          Status = StatusType.Draft,
          PublishTime = null,
          ListableContentType = ListableContentType.News,
          DetailImageId = new List<Guid>
          {
            _guidGenerator.Create(), _guidGenerator.Create() // File dan gelen Ids
          }
        },
        autoSave: true
      );

      await _listableContentTagRepository.InsertManyAsync(
        new List<ListableContentTag>()
        {
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id,
            TagId = (await _tagRepository.GetAsync(x => x.TagName == "Yaşam")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id,
            TagId = (await _tagRepository.GetAsync(x => x.TagName == "Sanayi")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id,
            TagId = (await _tagRepository.GetAsync(x => x.TagName == "Spor")).Id,
            TenantId =tenantId
          }
        },
        autoSave: true
      );

      await _listableContentCityRepository.InsertAsync(
        new ListableContentCity()
        {
          ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id,
          CityId = (await _cityRepository.GetAsync(x => x.CityName == "Konya")).Id,
          TenantId = tenantId
        }
        , autoSave: true
      );

      await _listableContentCategoryRepository.InsertManyAsync(
        new List<ListableContentCategory>()
        {
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id,
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Ekonomi")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id,
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Eğitim")).Id,
            TenantId =tenantId
          }
        }
      , autoSave: true
      );

      #endregion

      #region News Haber 2

      await _newsRepository.InsertAsync(
        new News()
        {
          Title = "News Haber 2",
          Spot = "News haber 2 içeriği",
          ImageId = _guidGenerator.Create(), // File dan gelen Id
          TenantId = tenantId,
          Status = StatusType.Published,
          PublishTime = DateTime.Now,
          ListableContentType = ListableContentType.News,
          DetailImageId = new List<Guid>
          {
            _guidGenerator.Create(), _guidGenerator.Create() // File dan gelen Ids
          }
        },
        autoSave: true
      );

      await _listableContentTagRepository.InsertManyAsync(
        new List<ListableContentTag>()
        {
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 2")).Id,
            TagId = (await _tagRepository.GetAsync(x => x.TagName == "Yaşam")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 2")).Id,
            TagId = (await _tagRepository.GetAsync(x => x.TagName == "Spor")).Id,
            TenantId =tenantId
          }
        },
        autoSave: true
      );

      await _listableContentCityRepository.InsertManyAsync(
        new List<ListableContentCity>()
        {
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 2")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Diyarbakır")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 2")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Mardin")).Id,
            TenantId =tenantId
          }
        }
        , autoSave: true
      );

      await _listableContentCategoryRepository.InsertManyAsync(
        new List<ListableContentCategory>()
        {
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 2")).Id,
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Kültür")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 2")).Id,
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Asya Kültürü")).Id,
            TenantId =tenantId
          }
        }
      , autoSave: true
      );

      await _listableContentRelationRepository.InsertAsync(
        new ListableContentRelation()
        {
          ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 2")).Id,
          RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id,
          TenantId = tenantId
        },
        autoSave: true
      );

      #endregion

      #region News Haber 3

      await _newsRepository.InsertAsync(
        new News()
        {
          Title = "News Haber 3",
          Spot = "News Haber 3 içeriği",
          ImageId = _guidGenerator.Create(), // File dan gelen Id
          TenantId = tenantId,
          Status = StatusType.Published,
          PublishTime = DateTime.Now,
          ListableContentType = ListableContentType.News,
          DetailImageId = new List<Guid>
          {
            _guidGenerator.Create(), _guidGenerator.Create() // File dan gelen Ids
          }
        },
        autoSave: true
      );

      await _listableContentTagRepository.InsertManyAsync(
        new List<ListableContentTag>()
        {
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 3")).Id,
            TagId = (await _tagRepository.GetAsync(x => x.TagName == "Sanayi")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 3")).Id,
            TagId = (await _tagRepository.GetAsync(x => x.TagName == "Teknoloji")).Id,
            TenantId =tenantId
          }
        },
        autoSave: true
      );

      await _listableContentCityRepository.InsertManyAsync(
        new List<ListableContentCity>()
        {
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 3")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "İstanbul")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 3")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Konya")).Id,
            TenantId =tenantId
          }
        }
        , autoSave: true
      );

      await _listableContentCategoryRepository.InsertManyAsync(
        new List<ListableContentCategory>()
        {
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 3")).Id,
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Kültür")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 3")).Id,
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Eğitim")).Id,
            TenantId =tenantId
          }
        }
      , autoSave: true
      );

      await _listableContentRelationRepository.InsertManyAsync(
        new List<ListableContentRelation>()
        {
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 3")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id,
            TenantId = tenantId
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 3")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 2")).Id,
            TenantId = tenantId
          }
        },
        autoSave: true
      );

      #endregion

      #region News Haber 4

      await _newsRepository.InsertAsync(
        new News()
        {
          Title = "News Haber 4",
          Spot = "News Haber 4 içeriği",
          ImageId = _guidGenerator.Create(), // File dan gelen Id
          TenantId = tenantId,
          Status = StatusType.Published,
          PublishTime = DateTime.Now,
          ListableContentType = ListableContentType.News,
          DetailImageId = new List<Guid>
          {
            _guidGenerator.Create(), _guidGenerator.Create() // File dan gelen Ids
          }
        },
        autoSave: true
      );

      await _listableContentTagRepository.InsertManyAsync(
        new List<ListableContentTag>()
        {
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 4")).Id,
            TagId = (await _tagRepository.GetAsync(x => x.TagName == "Yaşam")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 4")).Id,
            TagId = (await _tagRepository.GetAsync(x => x.TagName == "Sanayi")).Id,
            TenantId =tenantId
          }
        },
        autoSave: true
      );

      await _listableContentCityRepository.InsertManyAsync(
        new List<ListableContentCity>()
        {
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 4")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Konya")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 4")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Mardin")).Id,
            TenantId =tenantId
          }
        }
        , autoSave: true
      );

      await _listableContentCategoryRepository.InsertManyAsync(
        new List<ListableContentCategory>()
        {
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 4")).Id,
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Sağlık")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 4")).Id,
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Ekonomi")).Id,
            TenantId =tenantId
          }
        }
      , autoSave: true
      );

      await _listableContentRelationRepository.InsertManyAsync(
        new List<ListableContentRelation>()
        {
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 4")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 2")).Id,
            TenantId = tenantId
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 4")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 3")).Id,
            TenantId = tenantId
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 4")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id,
            TenantId = tenantId
          }
        },
        autoSave: true
      );

      #endregion

    }

    #endregion

    #region Video

    private async Task SeedVideoAsync(Guid? tenantId)
    {
      if (await _videoRepository.CountAsync() > 0)
        return;

      #region Video Haber 1 (Url)

      await _videoRepository.InsertAsync(
        new Video()
        {
          Title = "Video Haber 1",
          Spot = "Video haber 1 içeriği",
          ImageId = _guidGenerator.Create(), // File dan gelen Id
          TenantId = tenantId,
          Status = StatusType.Published,
          PublishTime = DateTime.Now,
          ListableContentType = ListableContentType.Video,
          VideoType = VideoType.Link,
          Url = null // Url Verilecek
        },
        autoSave: true
      );

      await _listableContentTagRepository.InsertAsync(
        new ListableContentTag()
        {
          ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 1")).Id,
          TagId = (await _tagRepository.GetAsync(x => x.TagName == "Spor")).Id,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _listableContentCityRepository.InsertManyAsync(
        new List<ListableContentCity>()
        {
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 1")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "İstanbul")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 1")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Mardin")).Id,
            TenantId =tenantId
          }
        }
        , autoSave: true
      );

      await _listableContentCategoryRepository.InsertAsync(
        new ListableContentCategory()
        {
          ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 1")).Id,
          CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Siyaset")).Id,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _listableContentRelationRepository.InsertManyAsync(
        new List<ListableContentRelation>()
        {
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 1")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 1")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 4")).Id,
            TenantId =tenantId
          }
        },
        autoSave: true
      );

      #endregion

      #region Video Haber 2 (VideoId)

      await _videoRepository.InsertAsync(
        new Video()
        {
          Title = "Video Haber 2",
          Spot = "Video haber 2 içeriği",
          ImageId = _guidGenerator.Create(), // File dan gelen Id
          TenantId = tenantId,
          Status = StatusType.Published,
          PublishTime = DateTime.Now,
          ListableContentType = ListableContentType.Video,
          VideoType = VideoType.Video,
          VideoId = null // VideoId Verilecek
        },
        autoSave: true
      );

      await _listableContentTagRepository.InsertAsync(
        new ListableContentTag()
        {
          ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 2")).Id,
          TagId = (await _tagRepository.GetAsync(x => x.TagName == "Teknoloji")).Id,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _listableContentCityRepository.InsertManyAsync(
        new List<ListableContentCity>()
        {
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 2")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "İstanbul")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 2")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Konya")).Id,
            TenantId =tenantId
          }
        }
        , autoSave: true
      );

      await _listableContentCategoryRepository.InsertManyAsync(
        new List<ListableContentCategory>()
        {
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 2")).Id,
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Ekonomi")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 2")).Id,
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Mikroekonomi")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 2")).Id,
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Makroekonomi")).Id,
            TenantId =tenantId
          }
        },
        autoSave: true
      );

      await _listableContentRelationRepository.InsertManyAsync(
        new List<ListableContentRelation>()
        {
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 2")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 2")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 3")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 2")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 4")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 2")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 2")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 2")).Id,
            RelatedListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 1")).Id,
            TenantId =tenantId
          }
        },
        autoSave: true
      );

      #endregion

      #region Video Haber 3 (VideoId)

      await _videoRepository.InsertAsync(
        new Video()
        {
          Title = "Video Haber 3",
          Spot = "Video haber 3 içeriği",
          ImageId = _guidGenerator.Create(), // File dan gelen Id
          TenantId = tenantId,
          Status = StatusType.Published,
          PublishTime = DateTime.Now,
          ListableContentType = ListableContentType.Video,
          VideoType = VideoType.Video,
          VideoId = null // VideoId Verilecek
        },
        autoSave: true
      );

      await _listableContentTagRepository.InsertManyAsync(
        new List<ListableContentTag>()
        {
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 3")).Id,
            TagId = (await _tagRepository.GetAsync(x => x.TagName == "Teknoloji")).Id,
            TenantId = tenantId
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 3")).Id,
            TagId = (await _tagRepository.GetAsync(x => x.TagName == "Yaşam")).Id,
            TenantId = tenantId
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 3")).Id,
            TagId = (await _tagRepository.GetAsync(x => x.TagName == "Sanayi")).Id,
            TenantId = tenantId
          },
        },
        autoSave: true
      );

      await _listableContentCityRepository.InsertManyAsync(
        new List<ListableContentCity>()
        {
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 3")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "İstanbul")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 3")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Ankara")).Id,
            TenantId =tenantId
          }
        }
        , autoSave: true
      );

      await _listableContentCategoryRepository.InsertManyAsync(
        new List<ListableContentCategory>()
        {
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 3")).Id,
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Sağlık")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 3")).Id,
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Siyaset")).Id,
            TenantId =tenantId
          }
        },
        autoSave: true
      );

      await _listableContentRelationRepository.InsertManyAsync(
        new List<ListableContentRelation>()
        {
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 3")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 3")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 3")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 3")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 4")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 3")).Id,
            RelatedListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 2")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 3")).Id,
            RelatedListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 1")).Id,
            TenantId =tenantId
          }
        },
        autoSave: true
      );

      #endregion

      #region Video Haber 4 (Url)

      await _videoRepository.InsertAsync(
        new Video()
        {
          Title = "Video Haber 4",
          Spot = "Video haber 4 içeriği",
          ImageId = _guidGenerator.Create(), // File dan gelen Id
          TenantId = tenantId,
          Status = StatusType.Published,
          PublishTime = DateTime.Now,
          ListableContentType = ListableContentType.Video,
          VideoType = VideoType.Link,
          Url = null // Url Verilecek
        },
        autoSave: true
      );

      await _listableContentTagRepository.InsertAsync(
        new ListableContentTag()
        {
          ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 4")).Id,
          TagId = (await _tagRepository.GetAsync(x => x.TagName == "Yaşam")).Id,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _listableContentCityRepository.InsertManyAsync(
        new List<ListableContentCity>()
        {
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 4")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Diyarbakır")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 4")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Ankara")).Id,
            TenantId =tenantId
          }
        }
        , autoSave: true
      );

      await _listableContentCategoryRepository.InsertAsync(
        new ListableContentCategory()
        {
          ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 4")).Id,
          CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Eğitim")).Id,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _listableContentRelationRepository.InsertManyAsync(
        new List<ListableContentRelation>()
        {
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 4")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 1")).Id,
            RelatedListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 3")).Id,
            TenantId =tenantId
          }
        },
        autoSave: true
      );

      #endregion

    }

    #endregion

    #region Gallery
    private async Task SeedGalleryAsync(Guid? tenantId)
    {
      if (await _galleryRepository.CountAsync() > 0)
        return;

      #region Gallery Haber 1 

      await _galleryRepository.InsertAsync(
        new Gallery()
        {
          Title = "Gallery Haber 1",
          Spot = "Gallery haber 1 içeriği",
          ImageId = _guidGenerator.Create(), // File dan gelen Id
          TenantId = tenantId,
          Status = StatusType.Published,
          PublishTime = DateTime.Now,
          ListableContentType = ListableContentType.Gallery,
          GalleryImages = new List<GalleryImage>()
          {
            new(){Order = 1, ImageId = _guidGenerator.Create(), NewsContent = "Image 1 Content 1"},  // File dan gelen Id
            new(){Order = 2, ImageId = _guidGenerator.Create(), NewsContent = "Image 2 Content 1"}   // File dan gelen Id
          }
        },
        autoSave: true
      );

      await _listableContentTagRepository.InsertAsync(
        new ListableContentTag()
        {
          ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 1")).Id,
          TagId = (await _tagRepository.GetAsync(x => x.TagName == "Spor")).Id,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _listableContentCityRepository.InsertManyAsync(
        new List<ListableContentCity>()
        {
          new()
          {
            ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 1")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Diyarbakır")).Id,
          TenantId = tenantId
          },
          new()
          {
            ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 1")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Ankara")).Id,
          TenantId = tenantId
          }
        }
        , autoSave: true
      );

      await _listableContentCategoryRepository.InsertAsync(
        new ListableContentCategory()
        {
          ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 1")).Id,
          CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Kültür")).Id,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _listableContentRelationRepository.InsertManyAsync(
        new List<ListableContentRelation>()
        {
          new()
          {
            ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 1")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 4")).Id,
            TenantId = tenantId
          },
          new()
          {
            ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 1")).Id,
            RelatedListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 2")).Id,
            TenantId = tenantId
          }
        },
        autoSave: true
      );

      #endregion

      #region Gallery Haber 2 

      await _galleryRepository.InsertAsync(
        new Gallery()
        {
          Title = "Gallery Haber 2",
          Spot = "Gallery haber 2 içeriği",
          ImageId = _guidGenerator.Create(), // File dan gelen Id
          TenantId = tenantId,
          Status = StatusType.Published,
          PublishTime = DateTime.Now,
          ListableContentType = ListableContentType.Gallery,
          GalleryImages = new List<GalleryImage>()
          {
            new(){Order = 1, ImageId = _guidGenerator.Create(), NewsContent = "Image 1 Content 2"},  // File dan gelen Id
            new(){Order = 2, ImageId = _guidGenerator.Create(), NewsContent = "Image 2 Content 2"}   // File dan gelen Id
          }
        },
        autoSave: true
      );

      await _listableContentTagRepository.InsertAsync(
        new ListableContentTag()
        {
          ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 2")).Id,
          TagId = (await _tagRepository.GetAsync(x => x.TagName == "Teknoloji")).Id,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _listableContentCityRepository.InsertManyAsync(
        new List<ListableContentCity>()
        {
          new()
          {
            ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 2")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Diyarbakır")).Id,
            TenantId = tenantId
          },
          new()
          {
            ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 2")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Mardin")).Id,
            TenantId = tenantId
          }
        }
        , autoSave: true
      );

      await _listableContentCategoryRepository.InsertAsync(
        new ListableContentCategory()
        {
          ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 2")).Id,
          CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Kültür")).Id,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _listableContentRelationRepository.InsertManyAsync(
        new List<ListableContentRelation>()
        {
          new()
          {
            ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 2")).Id,
            RelatedListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 1")).Id,
            TenantId = tenantId
          },
          new()
          {
            ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 2")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id,
            TenantId = tenantId
          }
        },
        autoSave: true
      );

      #endregion

      #region Gallery Haber 3 

      await _galleryRepository.InsertAsync(
        new Gallery()
        {
          Title = "Gallery Haber 3",
          Spot = "Gallery haber 3 içeriği",
          ImageId = _guidGenerator.Create(), // File dan gelen Id
          TenantId = tenantId,
          Status = StatusType.Published,
          PublishTime = DateTime.Now,
          ListableContentType = ListableContentType.Gallery,
          GalleryImages = new List<GalleryImage>()
          {
            new(){Order = 1, ImageId = _guidGenerator.Create(), NewsContent = "Image 1 Content 3"},  // File dan gelen Id
          }
        },
        autoSave: true
      );

      await _listableContentTagRepository.InsertAsync(
        new ListableContentTag()
        {
          ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 3")).Id,
          TagId = (await _tagRepository.GetAsync(x => x.TagName == "Yaşam")).Id,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _listableContentCityRepository.InsertManyAsync(
        new List<ListableContentCity>()
        {
          new()
          {
            ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 3")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Ankara")).Id,
            TenantId = tenantId
          },
          new()
          {
            ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 3")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "İstanbul")).Id,
            TenantId = tenantId
          }
        }
        , autoSave: true
      );

      await _listableContentCategoryRepository.InsertAsync(
        new ListableContentCategory()
        {
          ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 3")).Id,
          CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Kültür")).Id,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _listableContentRelationRepository.InsertManyAsync(
        new List<ListableContentRelation>()
        {
          new()
          {
            ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 3")).Id,
            RelatedListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 1")).Id,
            TenantId = tenantId
          },
          new()
          {
            ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 3")).Id,
            RelatedListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 2")).Id,
            TenantId = tenantId
          }
        },
        autoSave: true
      );

      #endregion

    }

    #endregion

  }
}
