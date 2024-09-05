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
using Volo.Abp.PermissionManagement;

namespace NewsManagement
{
  public class NewsManagementTestDataSeedContributor : IDataSeedContributor, ITransientDependency
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

    public NewsManagementTestDataSeedContributor(
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
      IPermissionManager permissionManager
      )
    {
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
    }

    public async Task SeedAsync(DataSeedContext context)
    {
      await SeedRoleAsync();
      await SeedUserAsync();
      await SeedTagAsync();
      await SeedCityAsync();
      await SeedCategoryAsync();
      //await SeedNewsAsync();
      //await SeedVideoAsync();
      //await SeedGalleryAsync();
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

    #region User

    private async Task SeedUserAsync()
    {
      if (!await _userRepository.AnyAsync(x => x.UserName == "Ahmet"))
      {
        var userAhmet = new IdentityUser
          (
            _guidGenerator.Create(),
            "Ahmet",
            "ahmet@gmail.com"
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
            "halil@gmail.com"
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
            "murat@gmail.com"
          );

        await _identityUserManager.CreateAsync(userMurat, "1q2w3E*");
        await _identityUserManager.SetRolesAsync(userMurat, new List<string> { RoleConst.Reporter });
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

      await _tagRepository.InsertAsync(
        new Tag()
        {
          TagName = "Sanayi"
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
          CityCode = 34,
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

      await _cityRepository.InsertAsync(
        new City()
        {
          CityName = "Konya",
          CityCode = 42
        },
        autoSave: true
      );

      await _cityRepository.InsertAsync(
        new City()
        {
          CityName = "Mardin",
          CityCode = 47
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
          ParentCategoryId = null,
          listableContentType = ListableContentType.Gallery
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
          listableContentType = ListableContentType.Video
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
          listableContentType = ListableContentType.News
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
          listableContentType = ListableContentType.News
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
          listableContentType = ListableContentType.News
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
          listableContentType = ListableContentType.Gallery
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
          listableContentType = ListableContentType.Gallery
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
          listableContentType = ListableContentType.Video
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
          listableContentType = ListableContentType.Video
        },
        autoSave: true
      );

    }
    #endregion
/*
    #region News

    private async Task SeedNewsAsync()
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
            TagId = (await _tagRepository.GetAsync(x => x.TagName == "Yaşam")).Id
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id,
            TagId = (await _tagRepository.GetAsync(x => x.TagName == "Sanayi")).Id
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id,
            TagId = (await _tagRepository.GetAsync(x => x.TagName == "Spor")).Id
          }
        },
        autoSave: true
      );

      await _listableContentCityRepository.InsertAsync(
        new ListableContentCity()
        {
          ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id,
          CityId = (await _cityRepository.GetAsync(x => x.CityName == "Konya")).Id
        }
        , autoSave: true
      );

      await _listableContentCategoryRepository.InsertManyAsync(
        new List<ListableContentCategory>()
        {
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id,
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Ekonomi")).Id
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id,
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Eğitim")).Id
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
            TagId = (await _tagRepository.GetAsync(x => x.TagName == "Yaşam")).Id
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 2")).Id,
            TagId = (await _tagRepository.GetAsync(x => x.TagName == "Spor")).Id
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
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Diyarbakır")).Id
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 2")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Mardin")).Id
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
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Kültür")).Id
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 2")).Id,
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Asya Kültürü")).Id
          }
        }
      , autoSave: true
      );

      await _listableContentRelationRepository.InsertAsync(
        new ListableContentRelation()
        {
          ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 2")).Id,
          RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id
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
            TagId = (await _tagRepository.GetAsync(x => x.TagName == "Sanayi")).Id
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 3")).Id,
            TagId = (await _tagRepository.GetAsync(x => x.TagName == "Teknoloji")).Id
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
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "İstanbul")).Id
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 3")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Konya")).Id
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
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Kültür")).Id
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 3")).Id,
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Eğitim")).Id
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
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 3")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 2")).Id
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
            TagId = (await _tagRepository.GetAsync(x => x.TagName == "Yaşam")).Id
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 4")).Id,
            TagId = (await _tagRepository.GetAsync(x => x.TagName == "Sanayi")).Id
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
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Konya")).Id
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 4")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Mardin")).Id
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
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Sağlık")).Id
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 4")).Id,
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Ekonomi")).Id
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
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 2")).Id
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 4")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 3")).Id
          },
          new()
          {
            ListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 4")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id
          }
        },
        autoSave: true
      );

      #endregion

    }

    #endregion

    #region Video

    private async Task SeedVideoAsync()
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
          TagId = (await _tagRepository.GetAsync(x => x.TagName == "Spor")).Id
        },
        autoSave: true
      );

      await _listableContentCityRepository.InsertManyAsync(
        new List<ListableContentCity>()
        {
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 1")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "İstanbul")).Id
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 1")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Mardin")).Id
          }
        }
        , autoSave: true
      );

      await _listableContentCategoryRepository.InsertAsync(
        new ListableContentCategory()
        {
          ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 1")).Id,
          CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Siyaset")).Id
        },
        autoSave: true
      );

      await _listableContentRelationRepository.InsertManyAsync(
        new List<ListableContentRelation>()
        {
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 1")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 1")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 4")).Id
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
          TagId = (await _tagRepository.GetAsync(x => x.TagName == "Teknoloji")).Id
        },
        autoSave: true
      );

      await _listableContentCityRepository.InsertManyAsync(
        new List<ListableContentCity>()
        {
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 2")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "İstanbul")).Id
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 2")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Konya")).Id
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
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Ekonomi")).Id
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 2")).Id,
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Mikroekonomi")).Id
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 2")).Id,
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Makroekonomi")).Id
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
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 2")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 3")).Id
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 2")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 4")).Id
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 2")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 2")).Id
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 2")).Id,
            RelatedListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 1")).Id
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
            TagId = (await _tagRepository.GetAsync(x => x.TagName == "Teknoloji")).Id
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 3")).Id,
            TagId = (await _tagRepository.GetAsync(x => x.TagName == "Yaşam")).Id
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 3")).Id,
            TagId = (await _tagRepository.GetAsync(x => x.TagName == "Sanayi")).Id
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
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "İstanbul")).Id
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 3")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Ankara")).Id
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
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Sağlık")).Id
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 3")).Id,
            CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Siyaset")).Id
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
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 3")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 3")).Id
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 3")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 4")).Id
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 3")).Id,
            RelatedListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 2")).Id
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 3")).Id,
            RelatedListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 1")).Id
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
          TagId = (await _tagRepository.GetAsync(x => x.TagName == "Yaşam")).Id
        },
        autoSave: true
      );

      await _listableContentCityRepository.InsertManyAsync(
        new List<ListableContentCity>()
        {
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 4")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Diyarbakır")).Id
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 4")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Ankara")).Id
          }
        }
        , autoSave: true
      );

      await _listableContentCategoryRepository.InsertAsync(
        new ListableContentCategory()
        {
          ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 4")).Id,
          CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Eğitim")).Id
        },
        autoSave: true
      );

      await _listableContentRelationRepository.InsertManyAsync(
        new List<ListableContentRelation>()
        {
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 4")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id
          },
          new()
          {
            ListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 1")).Id,
            RelatedListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 3")).Id
          }
        },
        autoSave: true
      );

      #endregion

    }

    #endregion

    #region Gallery
    private async Task SeedGalleryAsync()
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
          TagId = (await _tagRepository.GetAsync(x => x.TagName == "Spor")).Id
        },
        autoSave: true
      );

      await _listableContentCityRepository.InsertManyAsync(
        new List<ListableContentCity>()
        {
          new()
          {
            ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 1")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Diyarbakır")).Id
          },
          new()
          {
            ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 1")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Ankara")).Id
          }
        }
        , autoSave: true
      );

      await _listableContentCategoryRepository.InsertAsync(
        new ListableContentCategory()
        {
          ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 1")).Id,
          CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Kültür")).Id
        },
        autoSave: true
      );

      await _listableContentRelationRepository.InsertManyAsync(
        new List<ListableContentRelation>()
        {
          new()
          {
            ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 1")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 4")).Id
          },
          new()
          {
            ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 1")).Id,
            RelatedListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 2")).Id
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
          TagId = (await _tagRepository.GetAsync(x => x.TagName == "Teknoloji")).Id
        },
        autoSave: true
      );

      await _listableContentCityRepository.InsertManyAsync(
        new List<ListableContentCity>()
        {
          new()
          {
            ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 2")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Diyarbakır")).Id
          },
          new()
          {
            ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 2")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Mardin")).Id
          }
        }
        , autoSave: true
      );

      await _listableContentCategoryRepository.InsertAsync(
        new ListableContentCategory()
        {
          ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 2")).Id,
          CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Kültür")).Id
        },
        autoSave: true
      );

      await _listableContentRelationRepository.InsertManyAsync(
        new List<ListableContentRelation>()
        {
          new()
          {
            ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 2")).Id,
            RelatedListableContentId = (await _videoRepository.GetAsync(x => x.Title == "Video Haber 1")).Id
          },
          new()
          {
            ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 2")).Id,
            RelatedListableContentId = (await _newsRepository.GetAsync(x => x.Title == "News Haber 1")).Id
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
          TagId = (await _tagRepository.GetAsync(x => x.TagName == "Yaşam")).Id
        },
        autoSave: true
      );

      await _listableContentCityRepository.InsertManyAsync(
        new List<ListableContentCity>()
        {
          new()
          {
            ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 3")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "Ankara")).Id
          },
          new()
          {
            ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 3")).Id,
            CityId = (await _cityRepository.GetAsync(x => x.CityName == "İstanbul")).Id
          }
        }
        , autoSave: true
      );

      await _listableContentCategoryRepository.InsertAsync(
        new ListableContentCategory()
        {
          ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 3")).Id,
          CategoryId = (await _categoryRepository.GetAsync(c => c.CategoryName == "Kültür")).Id
        },
        autoSave: true
      );

      await _listableContentRelationRepository.InsertManyAsync(
        new List<ListableContentRelation>()
        {
          new()
          {
            ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 3")).Id,
            RelatedListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 1")).Id
          },
          new()
          {
            ListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 3")).Id,
            RelatedListableContentId = (await _galleryRepository.GetAsync(x => x.Title == "Gallery Haber 2")).Id
          }
        },
        autoSave: true
      );

      #endregion

    }

    #endregion
*/
  }
}
