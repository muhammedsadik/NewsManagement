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
using Volo.Abp.FeatureManagement;
using Volo.Abp.Features;
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
    private readonly IRepository<GalleryImage> _galleryImageRepository;
    private readonly IRepository<NewsDetailImage> _newsDetailImageRepository;
    private readonly IRepository<ListableContentTag> _listableContentTagRepository;
    private readonly IRepository<ListableContentCity> _listableContentCityRepository;
    private readonly IRepository<ListableContentCategory> _listableContentCategoryRepository;
    private readonly IRepository<ListableContentRelation> _listableContentRelationRepository;
    private readonly IRepository<IdentityUser, Guid> _userRepository;
    private readonly IRepository<IdentityRole, Guid> _roleRepository;
    private readonly IdentityUserManager _identityUserManager;
    private readonly IdentityRoleManager _identityRoleManager;
    private readonly IPermissionManager _permissionManager;
    private readonly FeatureManager _featureManager;
    private readonly IFeatureChecker _featureChecker;
    private readonly ICurrentTenant _currentTenant;
    private readonly FileManager _fileManager;
    private readonly IFileRepository _fileRepository;
    private readonly IFileBlobNameGenerator _fileBlobNameGenerator;
    private readonly IFileContentHashProvider _fileContentHashProvider;
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
      IRepository<GalleryImage> galleryImageRepository,
      IRepository<NewsDetailImage> newsDetailImageRepository,
      IRepository<ListableContentTag> listableContentTagRepository,
      IRepository<ListableContentCity> listableContentCityRepository,
      IRepository<ListableContentCategory> listableContentCategoryRepository,
      IRepository<ListableContentRelation> listableContentRelationRepository,
      IRepository<IdentityUser, Guid> userRepository,
      IRepository<IdentityRole, Guid> roleRepository,
      IdentityRoleManager identityRoleManager,
      IdentityUserManager identityUserManager,
      IPermissionManager permissionManager,
      ICurrentTenant currentTenant,
      FeatureManager featureManager,
      IFeatureChecker featureChecker,
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
      _newsRepository = newsRepository;
      _videoRepository = videoRepository;
      _galleryRepository = galleryRepository;
      _categoryRepository = categoryRepository;
      _listableContentTagRepository = listableContentTagRepository;
      _listableContentCityRepository = listableContentCityRepository;
      _listableContentCategoryRepository = listableContentCategoryRepository;
      _listableContentRelationRepository = listableContentRelationRepository;
      _userRepository = userRepository;
      _roleRepository = roleRepository;
      _identityRoleManager = identityRoleManager;
      _identityUserManager = identityUserManager;
      _permissionManager = permissionManager;
      _featureManager = featureManager;
      _currentTenant = currentTenant;
      _fileManager = fileManager; 
      _featureChecker = featureChecker;
      _fileRepository = fileRepository;
      _fileContentHashProvider = fileContentHashProvider;
      _newsDetailImageRepository = newsDetailImageRepository;
      _galleryImageRepository = galleryImageRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
      Guid? tenantId = _currentTenant.Id;
      var filesImageId = NewsManagementConsts.FilesImageId;
      var uploadImageId = NewsManagementConsts.UploadImageId;

      using (_currentTenant.Change(tenantId))
      {
        await SeedRoleAsync(tenantId);
        await SeedUserAsync(tenantId);
        await SeedTagAsync(tenantId);
        await SeedCityAsync(tenantId);
        await SeedFeaturesAsync(tenantId);
        await SeedCategoryAsync(tenantId);
        await SeedFileAsync(tenantId, filesImageId, uploadImageId);
        await SeedNewsAsync(tenantId, filesImageId, uploadImageId);
        await SeedVideoAsync(tenantId, filesImageId, uploadImageId);
        await SeedGalleryAsync(tenantId, filesImageId, uploadImageId);
      }
    }

    #region Tenant

    private async Task SeedFeaturesAsync(Guid? tenantId)
    {
      //var featureValue = await _featureChecker.GetOrNullAsync("NewsApp.Video");
      //if (!string.IsNullOrEmpty(featureValue))
      //  return;
   

      //await _featureManager.SetAsync(
      //    "NewsApp.Video",
      //    "true"
      //);

    }

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
      if (await _tagRepository.CountAsync() > 0)
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
    private async Task SeedFileAsync(Guid? tenantId, Guid filesImageId, Guid uploadImageId)
    {
      if (await _fileRepository.CountAsync() > 0)
        return;

      var currentDirectory = Directory.GetCurrentDirectory();
      var projectRoot = Directory.GetParent(currentDirectory).Parent.Parent.Parent.Parent.CreateSubdirectory("src\\NewsManagement.Web").FullName;
      var containerName = "default";
      var typeProvider = new FileExtensionContentTypeProvider();

      #region Files

      var filesPath = Path.Combine(projectRoot, "wwwroot", "Files.png");
      var filesName = Path.GetFileName(filesPath);
      typeProvider.TryGetContentType(filesPath, out var filesMimeType);
      var byteSizeOfFiles = System.IO.File.ReadAllBytes(filesPath);
      var filesHashString = _fileContentHashProvider.GetHashString(byteSizeOfFiles);

      var filesConfiguration = _configurationProvider.Get(containerName);
      var filesBlobName = await _fileBlobNameGenerator.CreateAsync(FileType.RegularFile, filesName, null, filesMimeType, filesConfiguration.AbpBlobDirectorySeparator);

      var files = new EasyAbp.FileManagement.Files.File(
        id: uploadImageId,
        tenantId: tenantId,
        parent: null,
        fileContainerName: containerName,
        fileName: filesName,
        mimeType: filesMimeType,
        fileType: FileType.RegularFile,
        subFilesQuantity: 0,
        byteSize: byteSizeOfFiles.Length,
        hash: filesHashString,
        blobName: filesBlobName,
        ownerUserId: null
      );

      await _fileRepository.InsertAsync(files, autoSave: true);
      await _fileManager.TrySaveBlobAsync(files, byteSizeOfFiles);

      #endregion

      #region Upload

      var uploadPath = Path.Combine(projectRoot, "wwwroot", "Upload.png");
      var uploadName = Path.GetFileName(uploadPath);
      typeProvider.TryGetContentType(uploadPath, out var uploadMimeType);
      var byteSizeOfUpload = System.IO.File.ReadAllBytes(uploadPath);
      var uploadHashString = _fileContentHashProvider.GetHashString(byteSizeOfUpload);

      var uploadConfiguration = _configurationProvider.Get(containerName);
      var uploadBlobName = await _fileBlobNameGenerator.CreateAsync(FileType.RegularFile, uploadName, null, uploadMimeType, uploadConfiguration.AbpBlobDirectorySeparator);

      var upload = new EasyAbp.FileManagement.Files.File(
        id: filesImageId,
        tenantId: tenantId,
        parent: null,
        fileContainerName: containerName,
        fileName: uploadName,
        mimeType: uploadMimeType,
        fileType: FileType.RegularFile,
        subFilesQuantity: 0,
        byteSize: byteSizeOfUpload.Length,
        hash: uploadHashString,
        blobName: uploadBlobName,
        ownerUserId: null
      );

      await _fileRepository.InsertAsync(upload, autoSave: true);
      await _fileManager.TrySaveBlobAsync(upload, byteSizeOfUpload);

      #endregion

    }

    #endregion

    #region News

    private async Task SeedNewsAsync(Guid? tenantId, Guid filesImageId, Guid uploadImageId)
    {
      if (await _newsRepository.CountAsync() > 0)
        return;

      var tagSporId = (await _tagRepository.GetAsync(x => x.TagName == "Spor")).Id;
      var tagYasamId = (await _tagRepository.GetAsync(x => x.TagName == "Yaşam")).Id;
      var tagSanayiId = (await _tagRepository.GetAsync(x => x.TagName == "Sanayi")).Id;
      var tagTeknolojiId = (await _tagRepository.GetAsync(x => x.TagName == "Teknoloji")).Id;

      var cityKonyaId = (await _cityRepository.GetAsync(x => x.CityName == "Konya")).Id;
      var cityAnkaraId = (await _cityRepository.GetAsync(x => x.CityName == "Ankara")).Id;
      var cityMardinId = (await _cityRepository.GetAsync(x => x.CityName == "Mardin")).Id;
      var cityIstanbulId = (await _cityRepository.GetAsync(x => x.CityName == "İstanbul")).Id;
      var cityDiyarbakırId = (await _cityRepository.GetAsync(x => x.CityName == "Diyarbakır")).Id;

      var categorySaglıkId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Sağlık")).Id;
      var categoryKulturId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Kültür")).Id;
      var categoryEgitimId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Eğitim")).Id;
      var categoryEkonomiId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Ekonomi")).Id;
      var categorySiyasetId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Siyaset")).Id;
      var categoryYasamKulturuId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Yaşam")).Id;
      var categoryAsyaKulturId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Asya Kültürü")).Id;
      var categoryMakroekonomiId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Makroekonomi")).Id;
      var categoryMikroekonomiId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Mikroekonomi")).Id;

      #region News Haber 1

      await _newsRepository.InsertAsync(
        new News()
        {
          Title = "News Haber 1",
          Spot = "News haber 1 içeriği",
          ImageId = filesImageId,
          TenantId = tenantId,
          Status = StatusType.Draft,
          PublishTime = null,
          ListableContentType = ListableContentType.News
        },
        autoSave: true
      );

      var news1Id = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id;

      await _newsDetailImageRepository.InsertManyAsync(
        new List<NewsDetailImage>()
        {
          new()
          {
            NewsId = news1Id,
            DetailImageId =filesImageId,
            TenantId = tenantId
          },
          new()
          {
            NewsId = news1Id,
            DetailImageId =uploadImageId,
            TenantId = tenantId
          }
        }
      );

      await _listableContentTagRepository.InsertManyAsync(
        new List<ListableContentTag>()
        {
          new()
          {
            ListableContentId = news1Id,
            TagId = tagYasamId,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = news1Id,
            TagId = tagSanayiId,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = news1Id,
            TagId = tagSporId,
            TenantId =tenantId
          }
        },
        autoSave: true
      );

      await _listableContentCityRepository.InsertAsync(
        new ListableContentCity()
        {
          ListableContentId = news1Id,
          CityId = cityKonyaId,
          TenantId = tenantId
        }
        , autoSave: true
      );

      await _listableContentCategoryRepository.InsertManyAsync(
        new List<ListableContentCategory>()
        {
          new()
          {
            ListableContentId = news1Id,
            CategoryId = categoryEkonomiId,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = news1Id,
            CategoryId = categoryEgitimId,
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
          ImageId = uploadImageId,
          TenantId = tenantId,
          Status = StatusType.Published,
          PublishTime = DateTime.Now,
          ListableContentType = ListableContentType.News
        },
        autoSave: true
      );

      var news2Id = (await _newsRepository.GetAsync(x => x.Title == "News Haber 2")).Id;

      await _newsDetailImageRepository.InsertManyAsync(
        new List<NewsDetailImage>()
        {
          new()
          {
            NewsId = news2Id,
            DetailImageId = uploadImageId,
            TenantId = tenantId
          },
          new()
          {
            NewsId = news2Id,
            DetailImageId = filesImageId,
            TenantId = tenantId
          }
        }
      );

      await _listableContentTagRepository.InsertManyAsync(
        new List<ListableContentTag>()
        {
          new()
          {
            ListableContentId = news2Id,
            TagId = tagYasamId,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = news2Id,
            TagId = tagSporId,
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
            ListableContentId = news2Id,
            CityId = cityDiyarbakırId,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = news2Id,
            CityId = cityMardinId,
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
            ListableContentId = news2Id,
            CategoryId = categoryKulturId,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = news2Id,
            CategoryId = categoryAsyaKulturId,
            TenantId =tenantId
          }
        }
      , autoSave: true
      );

      await _listableContentRelationRepository.InsertAsync(
        new ListableContentRelation()
        {
          ListableContentId = news2Id,
          RelatedListableContentId = news1Id,
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
          ImageId = uploadImageId,
          TenantId = tenantId,
          Status = StatusType.Published,
          PublishTime = DateTime.Now,
          ListableContentType = ListableContentType.News
        },
        autoSave: true
      );

      var news3Id = (await _newsRepository.GetAsync(x => x.Title == "News Haber 3")).Id;

      await _newsDetailImageRepository.InsertManyAsync(
        new List<NewsDetailImage>()
        {
          new()
          {
            NewsId = news3Id,
            DetailImageId = filesImageId,
            TenantId = tenantId
          },
          new()
          {
            NewsId = news3Id,
            DetailImageId = uploadImageId,
            TenantId = tenantId
          }
        }
      );

      await _listableContentTagRepository.InsertManyAsync(
        new List<ListableContentTag>()
        {
          new()
          {
            ListableContentId = news3Id,
            TagId = tagSanayiId,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = news3Id,
            TagId = tagTeknolojiId,
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
            ListableContentId = news3Id,
            CityId = cityIstanbulId,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = news3Id,
            CityId = cityKonyaId,
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
            ListableContentId = news3Id,
            CategoryId = categoryKulturId,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = news3Id,
            CategoryId = categoryEgitimId,
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
            ListableContentId = news3Id,
            RelatedListableContentId = news1Id,
            TenantId = tenantId
          },
          new()
          {
            ListableContentId = news3Id,
            RelatedListableContentId = news2Id,
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
          ImageId = filesImageId,
          TenantId = tenantId,
          Status = StatusType.Published,
          PublishTime = DateTime.Now,
          ListableContentType = ListableContentType.News
        },
        autoSave: true
      );

      var news4Id = (await _newsRepository.GetAsync(x => x.Title == "News Haber 4")).Id;

      await _newsDetailImageRepository.InsertManyAsync(
        new List<NewsDetailImage>()
        {
          new()
          {
            NewsId = news4Id,
            DetailImageId = uploadImageId,
            TenantId = tenantId
          },
          new()
          {
            NewsId = news4Id,
            DetailImageId = filesImageId,
            TenantId = tenantId
          }
        }
      );

      await _listableContentTagRepository.InsertManyAsync(
        new List<ListableContentTag>()
        {
          new()
          {
            ListableContentId = news4Id,
            TagId = tagYasamId,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = news4Id,
            TagId = tagSanayiId,
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
            ListableContentId = news4Id,
            CityId = cityKonyaId,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = news4Id,
            CityId =cityMardinId,
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
            ListableContentId = news4Id,
            CategoryId = categorySaglıkId,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = news4Id,
            CategoryId = categoryEgitimId,
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
            ListableContentId = news4Id,
            RelatedListableContentId = news2Id,
            TenantId = tenantId
          },
          new()
          {
            ListableContentId = news4Id,
            RelatedListableContentId = news3Id,
            TenantId = tenantId
          },
          new()
          {
            ListableContentId = news4Id,
            RelatedListableContentId = news1Id,
            TenantId = tenantId
          }
        },
        autoSave: true
      );

      #endregion

    }

    #endregion

    #region Video

    private async Task SeedVideoAsync(Guid? tenantId, Guid filesImageId, Guid uploadImageId)
    {
      if (await _videoRepository.CountAsync() > 0)
        return;

      var tagSporId = (await _tagRepository.GetAsync(x => x.TagName == "Spor")).Id;
      var tagYasamId = (await _tagRepository.GetAsync(x => x.TagName == "Yaşam")).Id;
      var tagSanayiId = (await _tagRepository.GetAsync(x => x.TagName == "Sanayi")).Id;
      var tagTeknolojiId = (await _tagRepository.GetAsync(x => x.TagName == "Teknoloji")).Id;

      var cityKonyaId = (await _cityRepository.GetAsync(x => x.CityName == "Konya")).Id;
      var cityAnkaraId = (await _cityRepository.GetAsync(x => x.CityName == "Ankara")).Id;
      var cityMardinId = (await _cityRepository.GetAsync(x => x.CityName == "Mardin")).Id;
      var cityIstanbulId = (await _cityRepository.GetAsync(x => x.CityName == "İstanbul")).Id;
      var cityDiyarbakırId = (await _cityRepository.GetAsync(x => x.CityName == "Diyarbakır")).Id;

      var categorySaglıkId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Sağlık")).Id;
      var categoryKulturId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Kültür")).Id;
      var categoryEgitimId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Eğitim")).Id;
      var categoryEkonomiId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Ekonomi")).Id;
      var categorySiyasetId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Siyaset")).Id;
      var categoryYasamKulturuId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Yaşam")).Id;
      var categoryAsyaKulturId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Asya Kültürü")).Id;
      var categoryMakroekonomiId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Makroekonomi")).Id;
      var categoryMikroekonomiId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Mikroekonomi")).Id;

      var news1Id = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id;
      var news2Id = (await _newsRepository.GetAsync(x => x.Title == "News Haber 2")).Id;
      var news3Id = (await _newsRepository.GetAsync(x => x.Title == "News Haber 3")).Id;
      var news4Id = (await _newsRepository.GetAsync(x => x.Title == "News Haber 4")).Id;

      #region Video Haber 1 (Url)

      await _videoRepository.InsertAsync(
        new Video()
        {
          Title = "Video Haber 1",
          Spot = "Video haber 1 içeriği",
          ImageId = filesImageId,
          TenantId = tenantId,
          Status = StatusType.Published,
          PublishTime = DateTime.Now,
          ListableContentType = ListableContentType.Video,
          VideoType = VideoType.Link,
          Url = "www.dogruhaber.com.tr"
        },
        autoSave: true
      );

      var video1Id = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 1")).Id;

      await _listableContentTagRepository.InsertAsync(
        new ListableContentTag()
        {
          ListableContentId = video1Id,
          TagId = tagSporId,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _listableContentCityRepository.InsertManyAsync(
        new List<ListableContentCity>()
        {
          new()
          {
            ListableContentId = video1Id,
            CityId = cityIstanbulId,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = video1Id,
            CityId =cityMardinId,
            TenantId =tenantId
          }
        }
        , autoSave: true
      );

      await _listableContentCategoryRepository.InsertAsync(
        new ListableContentCategory()
        {
          ListableContentId = video1Id,
          CategoryId = categorySiyasetId,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _listableContentRelationRepository.InsertManyAsync(
        new List<ListableContentRelation>()
        {
          new()
          {
            ListableContentId = video1Id,
            RelatedListableContentId = news1Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = video1Id,
            RelatedListableContentId = news4Id,
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
          ImageId = filesImageId,
          TenantId = tenantId,
          Status = StatusType.Published,
          PublishTime = DateTime.Now,
          ListableContentType = ListableContentType.Video,
          VideoType = VideoType.Video,
          VideoId = uploadImageId
        },
        autoSave: true
      );

      var video2Id = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 2")).Id;

      await _listableContentTagRepository.InsertAsync(
        new ListableContentTag()
        {
          ListableContentId = video2Id,
          TagId = tagTeknolojiId,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _listableContentCityRepository.InsertManyAsync(
        new List<ListableContentCity>()
        {
          new()
          {
            ListableContentId = video2Id,
            CityId = cityIstanbulId,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = video2Id,
            CityId = cityKonyaId,
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
            ListableContentId = video2Id,
            CategoryId = categoryEkonomiId,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = video2Id,
            CategoryId = categoryMikroekonomiId,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = video2Id,
            CategoryId = categoryMakroekonomiId,
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
            ListableContentId = video2Id,
            RelatedListableContentId = news1Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = video2Id,
            RelatedListableContentId = news3Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = video2Id,
            RelatedListableContentId = news4Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = video2Id,
            RelatedListableContentId = news2Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = video2Id,
            RelatedListableContentId = video1Id,
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
          ImageId = filesImageId,
          TenantId = tenantId,
          Status = StatusType.Published,
          PublishTime = DateTime.Now,
          ListableContentType = ListableContentType.Video,
          VideoType = VideoType.Video,
          VideoId = uploadImageId
        },
        autoSave: true
      );

      var video3Id = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 3")).Id;

      await _listableContentTagRepository.InsertManyAsync(
        new List<ListableContentTag>()
        {
          new()
          {
            ListableContentId = video3Id,
            TagId = tagTeknolojiId,
            TenantId = tenantId
          },
          new()
          {
            ListableContentId = video3Id,
            TagId = tagYasamId,
            TenantId = tenantId
          },
          new()
          {
            ListableContentId = video3Id,
            TagId = tagSporId,
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
            ListableContentId = video3Id,
            CityId = cityIstanbulId,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = video3Id,
            CityId = cityAnkaraId,
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
            ListableContentId = video3Id,
            CategoryId = categorySaglıkId,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = video3Id,
            CategoryId = categorySiyasetId,
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
            ListableContentId = video3Id,
            RelatedListableContentId = news1Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = video3Id,
            RelatedListableContentId = news3Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = video3Id,
            RelatedListableContentId = news4Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = video3Id,
            RelatedListableContentId = video2Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = video3Id,
            RelatedListableContentId = video1Id,
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
          ImageId = null,
          TenantId = tenantId,
          Status = StatusType.Published,
          PublishTime = DateTime.Now,
          ListableContentType = ListableContentType.Video,
          VideoType = VideoType.Link,
          Url = "www.dogruhaber.com.tr"
        },
        autoSave: true
      );

      var video4Id = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 4")).Id;

      await _listableContentTagRepository.InsertAsync(
        new ListableContentTag()
        {
          ListableContentId = video4Id,
          TagId = tagYasamId,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _listableContentCityRepository.InsertManyAsync(
        new List<ListableContentCity>()
        {
          new()
          {
            ListableContentId = video4Id,
            CityId = cityDiyarbakırId,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = video4Id,
            CityId = cityAnkaraId,
            TenantId =tenantId
          }
        }
        , autoSave: true
      );

      await _listableContentCategoryRepository.InsertAsync(
        new ListableContentCategory()
        {
          ListableContentId = video4Id,
          CategoryId = categoryEgitimId,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _listableContentRelationRepository.InsertManyAsync(
        new List<ListableContentRelation>()
        {
          new()
          {
            ListableContentId = video4Id,
            RelatedListableContentId = news1Id,
            TenantId =tenantId
          },
          new()
          {
            ListableContentId = video4Id,
            RelatedListableContentId = video3Id,
            TenantId =tenantId
          }
        },
        autoSave: true
      );

      #endregion

    }

    #endregion

    #region Gallery
    private async Task SeedGalleryAsync(Guid? tenantId, Guid filesImageId, Guid uploadImageId)
    {
      if (await _galleryRepository.CountAsync() > 0)
        return;

      var tagSporId = (await _tagRepository.GetAsync(x => x.TagName == "Spor")).Id;
      var tagYasamId = (await _tagRepository.GetAsync(x => x.TagName == "Yaşam")).Id;
      var tagSanayiId = (await _tagRepository.GetAsync(x => x.TagName == "Sanayi")).Id;
      var tagTeknolojiId = (await _tagRepository.GetAsync(x => x.TagName == "Teknoloji")).Id;

      var cityKonyaId = (await _cityRepository.GetAsync(x => x.CityName == "Konya")).Id;
      var cityAnkaraId = (await _cityRepository.GetAsync(x => x.CityName == "Ankara")).Id;
      var cityMardinId = (await _cityRepository.GetAsync(x => x.CityName == "Mardin")).Id;
      var cityIstanbulId = (await _cityRepository.GetAsync(x => x.CityName == "İstanbul")).Id;
      var cityDiyarbakırId = (await _cityRepository.GetAsync(x => x.CityName == "Diyarbakır")).Id;

      var categorySaglıkId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Sağlık")).Id;
      var categoryKulturId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Kültür")).Id;
      var categoryEgitimId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Eğitim")).Id;
      var categoryEkonomiId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Ekonomi")).Id;
      var categorySiyasetId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Siyaset")).Id;
      var categoryYasamKulturuId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Yaşam")).Id;
      var categoryAsyaKulturId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Asya Kültürü")).Id;
      var categoryMakroekonomiId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Makroekonomi")).Id;
      var categoryMikroekonomiId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Mikroekonomi")).Id;

      var news1Id = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id;
      var news2Id = (await _newsRepository.GetAsync(x => x.Title == "News Haber 2")).Id;
      var news3Id = (await _newsRepository.GetAsync(x => x.Title == "News Haber 3")).Id;
      var news4Id = (await _newsRepository.GetAsync(x => x.Title == "News Haber 4")).Id;

      var video1Id = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 1")).Id;
      var video2Id = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 2")).Id;
      var video3Id = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 3")).Id;
      var video4Id = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 4")).Id;

      #region Gallery Haber 1

      await _galleryRepository.InsertAsync(
        new Gallery()
        {
          Title = "Gallery Haber 1",
          Spot = "Gallery haber 1 içeriği",
          ImageId = filesImageId,
          TenantId = tenantId,
          Status = StatusType.Published,
          PublishTime = DateTime.Now,
          ListableContentType = ListableContentType.Gallery,
        },
        autoSave: true
      );

      var gallery1Id = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 1")).Id;

      await _galleryImageRepository.InsertManyAsync(
        new List<GalleryImage>()
        {
          new()
          {
            GalleryId = gallery1Id,
            ImageId = filesImageId,
            TenantId = tenantId,
            NewsContent = "Gallery 1 imege 1",
            Order = 1,
          },
          new()
          {
            GalleryId = gallery1Id,
            ImageId = uploadImageId,
            TenantId = tenantId,
            NewsContent = "Gallery 1 imege 2",
            Order = 2,
          }
        }
      );

      await _listableContentTagRepository.InsertAsync(
        new ListableContentTag()
        {
          ListableContentId = gallery1Id,
          TagId = tagSporId,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _listableContentCityRepository.InsertManyAsync(
        new List<ListableContentCity>()
        {
          new()
          {
            ListableContentId = gallery1Id,
            CityId = cityDiyarbakırId,
            TenantId = tenantId
          },
          new()
          {
            ListableContentId = gallery1Id,
            CityId =cityAnkaraId,
          TenantId = tenantId
          }
        }
        , autoSave: true
      );

      await _listableContentCategoryRepository.InsertAsync(
        new ListableContentCategory()
        {
          ListableContentId = gallery1Id,
          CategoryId = categoryKulturId,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _listableContentRelationRepository.InsertManyAsync(
        new List<ListableContentRelation>()
        {
          new()
          {
            ListableContentId = gallery1Id,
            RelatedListableContentId = news4Id,
            TenantId = tenantId
          },
          new()
          {
            ListableContentId = gallery1Id,
            RelatedListableContentId = video2Id,
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
          ImageId = uploadImageId,
          TenantId = tenantId,
          Status = StatusType.Published,
          PublishTime = DateTime.Now,
          ListableContentType = ListableContentType.Gallery
        },
        autoSave: true
      );

      var gallery2Id = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 2")).Id;

      await _galleryImageRepository.InsertManyAsync(
        new List<GalleryImage>()
        {
          new()
          {
            GalleryId = gallery2Id,
            ImageId = uploadImageId,
            TenantId = tenantId,
            NewsContent = "Gallery 2 imege 1",
            Order = 1,
          },
          new()
          {
            GalleryId =gallery2Id,
            ImageId = filesImageId,
            TenantId = tenantId,
            NewsContent = "Gallery 2 imege 2",
            Order = 2,
          }
        }
      );

      await _listableContentTagRepository.InsertAsync(
        new ListableContentTag()
        {
          ListableContentId = gallery2Id,
          TagId = tagTeknolojiId,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _listableContentCityRepository.InsertManyAsync(
        new List<ListableContentCity>()
        {
          new()
          {
            ListableContentId = gallery2Id,
            CityId = cityDiyarbakırId,
            TenantId = tenantId
          },
          new()
          {
            ListableContentId = gallery2Id,
            CityId = cityMardinId,
            TenantId = tenantId
          }
        }
        , autoSave: true
      );

      await _listableContentCategoryRepository.InsertAsync(
        new ListableContentCategory()
        {
          ListableContentId = gallery2Id,
          CategoryId = categoryKulturId,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _listableContentRelationRepository.InsertManyAsync(
        new List<ListableContentRelation>()
        {
          new()
          {
            ListableContentId = gallery2Id,
            RelatedListableContentId = video1Id,
            TenantId = tenantId
          },
          new()
          {
            ListableContentId = gallery2Id,
            RelatedListableContentId = news2Id,
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
          ImageId = uploadImageId,
          TenantId = tenantId,
          Status = StatusType.Published,
          PublishTime = DateTime.Now,
          ListableContentType = ListableContentType.Gallery
        },
        autoSave: true
      );

      var gallery3Id = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 3")).Id;

      await _galleryImageRepository.InsertManyAsync(
        new List<GalleryImage>()
        {
          new()
          {
            GalleryId = gallery3Id,
            ImageId = uploadImageId,
            TenantId = tenantId,
            NewsContent = "Gallery 3 imege 1",
            Order = 1,
          },
          new()
          {
            GalleryId = gallery3Id,
            ImageId = filesImageId,
            TenantId = tenantId,
            NewsContent = "Gallery 3 imege 2",
            Order = 2,
          }
        }
      );

      await _listableContentTagRepository.InsertAsync(
        new ListableContentTag()
        {
          ListableContentId = gallery3Id,
          TagId = tagYasamId,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _listableContentCityRepository.InsertManyAsync(
        new List<ListableContentCity>()
        {
          new()
          {
            ListableContentId = gallery3Id,
            CityId = cityAnkaraId,
            TenantId = tenantId
          },
          new()
          {
            ListableContentId = gallery3Id,
            CityId = cityIstanbulId,
            TenantId = tenantId
          }
        }
        , autoSave: true
      );

      await _listableContentCategoryRepository.InsertAsync(
        new ListableContentCategory()
        {
          ListableContentId = gallery3Id,
          CategoryId = categoryEkonomiId,
          TenantId = tenantId
        },
        autoSave: true
      );

      await _listableContentRelationRepository.InsertManyAsync(
        new List<ListableContentRelation>()
        {
          new()
          {
            ListableContentId = gallery3Id,
            RelatedListableContentId = gallery1Id,
            TenantId = tenantId
          },
          new()
          {
            ListableContentId = gallery3Id,
            RelatedListableContentId = gallery2Id,
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
