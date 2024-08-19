﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Uow;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using EasyAbp.FileManagement.EntityFrameworkCore;
using NewsManagement.Entities.ListableContents;
using NewsManagement.EntityRepositories.ListableContents;

namespace NewsManagement.EntityFrameworkCore;

[DependsOn(
    typeof(NewsManagementDomainModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCorePostgreSqlModule),
    typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(AbpTenantManagementEntityFrameworkCoreModule),
    typeof(FileManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule)
    )]
public class NewsManagementEntityFrameworkCoreModule : AbpModule
{
  public override void PreConfigureServices(ServiceConfigurationContext context)
  {
    // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

    NewsManagementEfCoreEntityExtensionMappings.Configure();
  }

  public override void ConfigureServices(ServiceConfigurationContext context)
  {
    context.Services.AddAbpDbContext<NewsManagementDbContext>(options =>
    {
      /* Remove "includeAllEntities: true" to create
       * default repositories only for aggregate roots */
      options.AddDefaultRepositories(includeAllEntities: true);
    });

    Configure<AbpDbContextOptions>(options =>
    {
      /* The main point to change your DBMS.
       * See also NewsManagementMigrationsDbContextFactory for EF Core tooling. */
      options.UseNpgsql();
    });

    context.Services.AddTransient(typeof(IListableContentGenericRepository<>), typeof(ListableContentGenericRepository<>));

  }
}
