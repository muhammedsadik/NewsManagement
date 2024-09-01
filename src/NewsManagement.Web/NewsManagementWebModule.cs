using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewsManagement.EntityFrameworkCore;
using NewsManagement.Localization;
using NewsManagement.MultiTenancy;
using NewsManagement.Web.Menus;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity.Web;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Web;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement.Web;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.UI;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using EasyAbp.FileManagement.Web;
using EasyAbp.FileManagement.Options;
using EasyAbp.FileManagement;
using EasyAbp.FileManagement.Files;
using EasyAbp.FileManagement.Containers;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Minio;
using Volo.Abp.BackgroundJobs.Hangfire;
using Hangfire;
using Hangfire.PostgreSql;
using Volo.Abp.Hangfire;
using NewsManagement.Entities.BackgroundJobs;
using Volo.Abp.BackgroundJobs;

namespace NewsManagement.Web;

[DependsOn(
    typeof(NewsManagementHttpApiModule),
    typeof(NewsManagementApplicationModule),
    typeof(NewsManagementEntityFrameworkCoreModule),
    typeof(AbpAutofacModule),
    typeof(AbpIdentityWebModule),
    typeof(AbpSettingManagementWebModule),
    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpTenantManagementWebModule),
    typeof(FileManagementWebModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpBackgroundJobsHangfireModule),
    typeof(AbpHangfireModule),
    typeof(AbpBlobStoringMinioModule)
    )]
public class NewsManagementWebModule : AbpModule
{
  public override void PreConfigureServices(ServiceConfigurationContext context)
  {
    context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
    {
      options.AddAssemblyResource(
              typeof(NewsManagementResource),
              typeof(NewsManagementDomainModule).Assembly,
              typeof(NewsManagementDomainSharedModule).Assembly,
              typeof(NewsManagementApplicationModule).Assembly,
              typeof(NewsManagementApplicationContractsModule).Assembly,
              typeof(NewsManagementWebModule).Assembly
          );
    });

    PreConfigure<OpenIddictBuilder>(builder =>
    {
      builder.AddValidation(options =>
          {
            options.AddAudiences("NewsManagement");
            options.UseLocalServer();
            options.UseAspNetCore();
          });
    });
  }

  public override void ConfigureServices(ServiceConfigurationContext context)
  {
    var hostingEnvironment = context.Services.GetHostingEnvironment();
    var configuration = context.Services.GetConfiguration();

    ConfigureAuthentication(context);
    ConfigureUrls(configuration);
    ConfigureBundles();
    ConfigureAutoMapper();
    ConfigureVirtualFileSystem(hostingEnvironment);

    Configure<AbpBlobStoringOptions>(options =>
    {
      options.Containers.Configure<LocalFileSystemBlobContainer>(container =>
      {
        container.IsMultiTenant = true;
        container.UseMinio(minio =>
        {
          minio.EndPoint = "localhost:9000";
          minio.AccessKey = "h5lqrBpgTtFVNoWC12CK";
          minio.SecretKey = "Rw2FQ3g6hOK3fv8A4wNLi4nItp6ExhIU1WqsjdX6";
          minio.BucketName = "newsmanagement";
        });
      });
    });

    Configure<FileManagementOptions>(options =>
    {
      options.DefaultFileDownloadProviderType = typeof(LocalFileDownloadProvider);
      options.Containers.Configure<CommonFileContainer>(container =>
      {
        // private container never be used by non-owner users (except user who has the "File.Manage" permission).
        container.FileContainerType = FileContainerType.Public;
        container.AbpBlobContainerName = BlobContainerNameAttribute.GetContainerName<LocalFileSystemBlobContainer>();
        container.AbpBlobDirectorySeparator = "/";

        container.RetainUnusedBlobs = false;
        container.EnableAutoRename = true;

        container.MaxByteSizeForEachFile = 5 * 1024 * 1024;
        container.MaxByteSizeForEachUpload = 10 * 1024 * 1024;
        container.MaxFileQuantityForEachUpload = 2;

        container.AllowOnlyConfiguredFileExtensions = true;
        container.FileExtensionsConfiguration.Add(".jpg", true);
        container.FileExtensionsConfiguration.Add(".PNG", true);
        // container.FileExtensionsConfiguration.Add(".tar.gz", true);
        // container.FileExtensionsConfiguration.Add(".exe", false);

        container.GetDownloadInfoTimesLimitEachUserPerMinute = 10;
      });
    });

    ConfigureNavigationServices();
    ConfigureAutoApiControllers();
    ConfigureSwaggerServices(context.Services);
    ConfigureHangfire(context, configuration);
    context.Services.AddHangfireServer();
  }

  private void ConfigureHangfire(ServiceConfigurationContext context, IConfiguration configuration)
  {

    context.Services.AddHangfire(config =>
    {
      config.UsePostgreSqlStorage(configuration.GetConnectionString("Default"));
    });
  }

  private void ConfigureAuthentication(ServiceConfigurationContext context)
  {
    context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
  }

  private void ConfigureUrls(IConfiguration configuration)
  {
    Configure<AppUrlOptions>(options =>
    {
      options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
    });
  }

  private void ConfigureBundles()
  {
    Configure<AbpBundlingOptions>(options =>
    {
      options.StyleBundles.Configure(
              LeptonXLiteThemeBundles.Styles.Global,
              bundle =>
              {
                bundle.AddFiles("/global-styles.css");
              }
          );
    });
  }

  private void ConfigureAutoMapper()
  {
    Configure<AbpAutoMapperOptions>(options =>
    {
      options.AddMaps<NewsManagementWebModule>();
    });
  }

  private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
  {
    if (hostingEnvironment.IsDevelopment())
    {
      Configure<AbpVirtualFileSystemOptions>(options =>
      {
        options.FileSets.ReplaceEmbeddedByPhysical<NewsManagementDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}NewsManagement.Domain.Shared"));
        options.FileSets.ReplaceEmbeddedByPhysical<NewsManagementDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}NewsManagement.Domain"));
        options.FileSets.ReplaceEmbeddedByPhysical<NewsManagementApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}NewsManagement.Application.Contracts"));
        options.FileSets.ReplaceEmbeddedByPhysical<NewsManagementApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}NewsManagement.Application"));
        options.FileSets.ReplaceEmbeddedByPhysical<NewsManagementWebModule>(hostingEnvironment.ContentRootPath);
      });
    }
  }

  private void ConfigureNavigationServices()
  {
    Configure<AbpNavigationOptions>(options =>
    {
      options.MenuContributors.Add(new NewsManagementMenuContributor());
    });
  }

  private void ConfigureAutoApiControllers()
  {
    Configure<AbpAspNetCoreMvcOptions>(options =>
    {
      options.ConventionalControllers.Create(typeof(NewsManagementApplicationModule).Assembly);
    });
  }

  private void ConfigureSwaggerServices(IServiceCollection services)
  {
    services.AddAbpSwaggerGen(
        options =>
        {
          options.SwaggerDoc("v1", new OpenApiInfo { Title = "NewsManagement API", Version = "v1" });
          options.DocInclusionPredicate((docName, description) => true);
          options.CustomSchemaIds(type => type.FullName);
          options.HideAbpEndpoints();
        }
    );
  }

  public override void OnApplicationInitialization(ApplicationInitializationContext context)
  {
    var app = context.GetApplicationBuilder();
    var env = context.GetEnvironment();
    var backgroundJobManager = context.ServiceProvider.GetRequiredService<IBackgroundJobManager>();

    if (env.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
    }

    app.UseAbpRequestLocalization();

    if (!env.IsDevelopment())
    {
      app.UseErrorPage();
    }

    app.UseCorrelationId();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAbpOpenIddictValidation();

    if (MultiTenancyConsts.IsEnabled)
    {
      app.UseMultiTenancy();
    }

    app.UseUnitOfWork();
    app.UseAuthorization();
    app.UseSwagger();
    app.UseAbpSwaggerUI(options =>
    {
      options.SwaggerEndpoint("/swagger/v1/swagger.json", "NewsManagement API");
    });
    app.UseAuditing();
    app.UseAbpSerilogEnrichers();

    RecurringJob.AddOrUpdate<ChangingStatusTypeJob>(
        "ChangingStatusTypeJob",
        job => job.ExecuteAsync(0),
        Cron.MinuteInterval(1)
    );

    app.UseConfiguredEndpoints();
  }
}

