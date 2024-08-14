using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using EasyAbp.FileManagement.EntityFrameworkCore;
using NewsManagement.Entities.Tags;
using Volo.Abp.EntityFrameworkCore.Modeling;
using NewsManagement.Entities.Cities;
using NewsManagement.Entities.Categories;
using NewsManagement.Entities.Videos;
using NewsManagement.Entities.Galleries;
using NewsManagement.Entities.Newses;
using NewsManagement.Entities.ListableContents;

namespace NewsManagement.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class NewsManagementDbContext :
    AbpDbContext<NewsManagementDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{


  public DbSet<ListableContent> ListableContents { get; set; }
  public DbSet<Gallery> Galleries { get; set; }
  public DbSet<Video> Videos { get; set; }
  public DbSet<News> Newses { get; set; }
  public DbSet<Tag> Tags { get; set; }
  public DbSet<City> Cities { get; set; }
  public DbSet<Category> Categories { get; set; }
  public DbSet<ListableContentTag> ListableContentTags { get; set; }
  public DbSet<ListableContentCity> ListableContentCities { get; set; }
  public DbSet<ListableContentCategory> ListableContentCategories { get; set; }


  #region Entities from the modules

  //Identity
  public DbSet<IdentityUser> Users { get; set; }
  public DbSet<IdentityRole> Roles { get; set; }
  public DbSet<IdentityClaimType> ClaimTypes { get; set; }
  public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
  public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
  public DbSet<IdentityLinkUser> LinkUsers { get; set; }
  public DbSet<IdentityUserDelegation> UserDelegations { get; set; }

  // Tenant Management
  public DbSet<Tenant> Tenants { get; set; }
  public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

  #endregion

  public NewsManagementDbContext(DbContextOptions<NewsManagementDbContext> options)
      : base(options)
  {

  }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    builder.ConfigurePermissionManagement();
    builder.ConfigureSettingManagement();
    builder.ConfigureBackgroundJobs();
    builder.ConfigureAuditLogging();
    builder.ConfigureIdentity();
    builder.ConfigureOpenIddict();
    builder.ConfigureFeatureManagement();
    builder.ConfigureTenantManagement();
    builder.ConfigureFileManagement();

    #region Gallery, Video, News
    //builder.Entity<Gallery>(b =>
    //{

    //  b.ToTable(NewsManagementConsts.DbTablePrefix + "Galleries", NewsManagementConsts.DbSchema);
    //  b.ConfigureByConvention();
    //});
    
    //builder.Entity<Video>(b =>
    //{

    //  b.ToTable(NewsManagementConsts.DbTablePrefix + "Videos", NewsManagementConsts.DbSchema);
    //  b.ConfigureByConvention();
    //});
    
    //builder.Entity<News>(b =>
    //{

    //  b.ToTable(NewsManagementConsts.DbTablePrefix + "Newses", NewsManagementConsts.DbSchema);
    //  b.ConfigureByConvention();
    //});
    #endregion
    
    #region ListableContent(Tag, City, Category)
    builder.Entity<ListableContentTag>(b =>
    {
      b.HasKey(x => new { x.ListableContentId, x.TagId });
      b.HasOne(x => x.ListableContent).WithMany(x => x.ListableContentTags).HasForeignKey(x => x.ListableContentId);
      b.HasOne(x => x.Tag).WithMany(x => x.ListableContentTags).HasForeignKey(x => x.TagId);

      b.ToTable(NewsManagementConsts.DbTablePrefix + "ListableContentTags", NewsManagementConsts.DbSchema);
      b.ConfigureByConvention();
    });
    
    builder.Entity<ListableContentCity>(b =>
    {
      b.HasKey(x => new { x.ListableContentId, x.CityId });
      b.HasOne(x => x.ListableContent).WithMany(x => x.ListableContentCities).HasForeignKey(x => x.ListableContentId);
      b.HasOne(x => x.City).WithMany(x => x.ListableContentCities).HasForeignKey(x => x.CityId);

      b.ToTable(NewsManagementConsts.DbTablePrefix + "ListableContentCities", NewsManagementConsts.DbSchema);
      b.ConfigureByConvention();
    });
        
    builder.Entity<ListableContentCategory>(b =>
    {
      b.HasKey(x => new { x.ListableContentId, x.CategoryId });
      b.HasOne(x => x.ListableContent).WithMany(x => x.ListableContentCategories).HasForeignKey(x => x.ListableContentId);
      b.HasOne(x => x.Category).WithMany(x => x.ListableContentCategories).HasForeignKey(x => x.CategoryId);

      b.ToTable(NewsManagementConsts.DbTablePrefix + "ListableContentCategories", NewsManagementConsts.DbSchema);
      b.ConfigureByConvention();
    });
    #endregion 

    #region Tag, City, Category
    builder.Entity<Tag>(b =>
    {
      b.HasMany(x => x.ListableContentTags).WithOne().HasForeignKey(x => x.TagId);

      b.ToTable(NewsManagementConsts.DbTablePrefix + "Tags", NewsManagementConsts.DbSchema);
      b.ConfigureByConvention();
    });

    builder.Entity<City>(b =>
    {
      b.HasMany(x => x.ListableContentCities).WithOne().HasForeignKey(x => x.CityId);

      b.ToTable(NewsManagementConsts.DbTablePrefix + "Cities", NewsManagementConsts.DbSchema);
      b.ConfigureByConvention();
    });
        
    builder.Entity<Category>(b =>
    {
      b.HasMany(x => x.ListableContentCategories).WithOne().HasForeignKey(x => x.CategoryId);

      b.ToTable(NewsManagementConsts.DbTablePrefix + "Categories", NewsManagementConsts.DbSchema);
      b.ConfigureByConvention();
    });
    #endregion


  }
}
