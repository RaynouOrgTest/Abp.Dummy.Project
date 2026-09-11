using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using OpenIddict.Validation.AspNetCore;
using OpenIddict.Server.AspNetCore;
using Abp.Dummy.Project.EntityFrameworkCore;
using Abp.Dummy.Project.MultiTenancy;
using Abp.Dummy.Project.HealthChecks;
using Microsoft.OpenApi;
using Volo.Abp;
using Volo.Abp.Studio;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Microsoft.AspNetCore.Hosting;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Identity;
using Volo.Abp.OpenIddict;
using Volo.Abp.Swashbuckle;
using Volo.Abp.Studio.Client.AspNetCore;
using Volo.Abp.Security.Claims;
using Volo.Abp.Security.Encryption;

namespace Abp.Dummy.Project;

[DependsOn(
    typeof(ProjectHttpApiModule),
    typeof(AbpStudioClientAspNetCoreModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(ProjectApplicationModule),
    typeof(ProjectEntityFrameworkCoreModule),
    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAspNetCoreSerilogModule)
    )]
public class ProjectHttpApiHostModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("Project");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });

        if (!hostingEnvironment.IsDevelopment())
        {
            PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
            {
                options.AddDevelopmentEncryptionAndSigningCertificate = false;
            });

            PreConfigure<OpenIddictServerBuilder>(serverBuilder =>
            {
                serverBuilder.AddProductionEncryptionAndSigningCertificate("openiddict.pfx", RequireConfig(configuration, "AuthServer:CertificatePassPhrase"));
                serverBuilder.SetIssuer(new Uri(RequireConfig(configuration, "AuthServer:Authority")));
            });
        }
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (!configuration.GetValue<bool>("App:DisablePII"))
        {
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.LogCompleteSecurityArtifact = true;
        }

        var requireHttpsMetadata = hostingEnvironment.IsDevelopment()
            ? configuration.GetValue<bool>("AuthServer:RequireHttpsMetadata")
            : bool.Parse(RequireConfig(configuration, "AuthServer:RequireHttpsMetadata"));

        if (!requireHttpsMetadata)
        {
            Configure<OpenIddictServerAspNetCoreOptions>(options =>
            {
                options.DisableTransportSecurityRequirement = true;
            });

            Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
                options.KnownIPNetworks.Clear();
                options.KnownProxies.Clear();
            });
        }

        if (hostingEnvironment.IsDevelopment())
        {
            context.Services.AddRazorPages()
                .AddRazorRuntimeCompilation();
        }

        ConfigureStudio(hostingEnvironment);
        ConfigureAuthentication(context);
        ConfigureUrls(configuration, hostingEnvironment);
        ConfigureStringEncryption(configuration, hostingEnvironment);
        ConfigureBundles(hostingEnvironment);
        ConfigureConventionalControllers();
        ConfigureHealthChecks(context);
        ConfigureSwagger(context, configuration);
        ConfigureVirtualFileSystem(context);
        ConfigureCors(context, configuration, hostingEnvironment);
    }

    private void ConfigureStudio(IHostEnvironment hostingEnvironment)
    {
        if (hostingEnvironment.IsProduction())
        {
            Configure<AbpStudioClientOptions>(options =>
            {
                options.IsLinkEnabled = false;
            });
        }
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });
    }

    private void ConfigureUrls(IConfiguration configuration, IHostEnvironment hostingEnvironment)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = RequireConfigOutsideDevelopment(configuration, hostingEnvironment, "App:SelfUrl");
            options.Applications["Angular"].RootUrl = RequireConfigOutsideDevelopment(configuration, hostingEnvironment, "App:AngularUrl");
            options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',') ?? Array.Empty<string>());
        });
    }

    private void ConfigureBundles(IHostEnvironment hostingEnvironment)
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

            options.ScriptBundles.Configure(
                LeptonXLiteThemeBundles.Scripts.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-scripts.js");
                    if (hostingEnvironment.IsDevelopment())
                    {
                        bundle.AddFiles("/dev-login-helper.js");
                    }
                }
            );
        });
    }


    private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<ProjectDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Abp.Dummy.Project.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<ProjectDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Abp.Dummy.Project.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<ProjectApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Abp.Dummy.Project.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<ProjectApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Abp.Dummy.Project.Application"));
            });
        }
    }

    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(ProjectApplicationModule).Assembly);
        });
    }

    private void ConfigureStringEncryption(IConfiguration configuration, IHostEnvironment hostingEnvironment)
    {
        var passPhrase = RequireConfigOutsideDevelopment(configuration, hostingEnvironment, "StringEncryption:DefaultPassPhrase");
        if (!string.IsNullOrWhiteSpace(passPhrase))
        {
            Configure<AbpStringEncryptionOptions>(options =>
            {
                options.DefaultPassPhrase = passPhrase;
            });
        }
    }

    private static void ConfigureSwagger(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAbpSwaggerGenWithOidc(
            RequireConfig(configuration, "AuthServer:Authority"),
            ["Project"],
            [AbpSwaggerOidcFlows.AuthorizationCode],
            null,
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Project API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration, IHostEnvironment hostingEnvironment)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(
                        RequireConfigOutsideDevelopment(configuration, hostingEnvironment, "App:CorsOrigins")?
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.Trim().RemovePostFix("/"))
                            .ToArray() ?? Array.Empty<string>()
                    )
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    private void ConfigureHealthChecks(ServiceConfigurationContext context)
    {
        context.Services.AddProjectHealthChecks();
    }

    private static string RequireConfig(IConfiguration configuration, string key)
    {
        var value = configuration[key];
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new AbpException(
                $"Missing required configuration '{key}'.");
        }

        return value;
    }

    private static string? RequireConfigOutsideDevelopment(IConfiguration configuration, IHostEnvironment hostingEnvironment, string key)
    {
        var value = configuration[key];
        if (!hostingEnvironment.IsDevelopment() && string.IsNullOrWhiteSpace(value))
        {
            throw new AbpException(
                $"Missing required configuration '{key}'.");
        }

        return value;
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        app.UseForwardedHeaders();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseRouting();
        app.MapAbpStaticAssets();
        app.UseAbpStudioLink();
        app.UseAbpSecurityHeaders();
        app.UseCors();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseDynamicClaims();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Project API");

            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
        });
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
