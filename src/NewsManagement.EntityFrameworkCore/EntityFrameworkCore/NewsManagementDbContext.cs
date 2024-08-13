﻿using Microsoft.EntityFrameworkCore;
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

namespace NewsManagement.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class NewsManagementDbContext :
    AbpDbContext<NewsManagementDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
  /* Add DbSet properties for your Aggregate Roots / Entities here. */
  public DbSet<Tag> Tags { get; set; }
  public DbSet<City> Cities { get; set; }
  public DbSet<Category> Categories { get; set; }

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


    builder.Entity<Tag>(b =>
    {
      b.ToTable(NewsManagementConsts.DbTablePrefix + "Tag", NewsManagementConsts.DbSchema);
      b.ConfigureByConvention();


    });

    builder.Entity<City>(b =>
    {
      b.ToTable(NewsManagementConsts.DbTablePrefix + "City", NewsManagementConsts.DbSchema);
      b.ConfigureByConvention();


    });
        
    builder.Entity<Category>(b =>
    {
      b.ToTable(NewsManagementConsts.DbTablePrefix + "Category", NewsManagementConsts.DbSchema);
      b.ConfigureByConvention();

    });


  }
}
