using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Authentication;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Models;
using CoreCodeCamp.Models.Admin;
using CoreCodeCamp.Services;
using FluentValidation.AspNetCore;
using Loggly;
using Loggly.Config;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using WilderMinds.AzureImageStorageService;

namespace CoreCodeCamp
{
  public class Startup
  {
    private const string IGNORE_STATUS_CODE_PAGES = "IgnoreStatusCodePages";
    IWebHostEnvironment _env;
    private readonly IConfiguration _config;

    public Startup(IWebHostEnvironment env, IConfiguration config)
    {
      _env = env;
      _config = config;
    }


    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection svcs)
    {
      if (_env.IsProduction())
      {
        svcs.AddScoped<IMailService, SendGridMailService>();
      }
      else
      {
        svcs.AddScoped<IMailService, DebugMailService>();
      }

      if (_env.IsProduction() || _env.IsStaging())
      {
        var svcConfig = _config.GetSection("BlobService");
        var acct = svcConfig["Account"];
        var key = svcConfig["Key"];
        var url = svcConfig["StorageUrl"];

        svcs.AddAzureImageStorageService(acct, key, url);
      }
      else
      {
        svcs.AddTransient<IAzureImageStorageService, DebugImageStorageService>();
      }


      // Add framework services.
      svcs.AddDbContext<CodeCampContext>();
      svcs.AddScoped<ICodeCampRepository, CodeCampRepository>();
      svcs.AddTransient<CodeCampSeeder>();
      svcs.AddTransient<ViewRenderer>();
      svcs.AddTransient<CoreCodeCampTokenFactory>();
      svcs.AddAutoMapper(Assembly.GetEntryAssembly());

      // Configure Identity (Security)
      svcs.AddAuthentication(o =>
      {
        o.DefaultScheme = IdentityConstants.ApplicationScheme;
        o.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
      })
        .AddCoreCodeCampBearerToken()
        .AddIdentityCookies(o => { });

      svcs.AddIdentityCore<CodeCampUser>(config =>
      {
        config.Stores.MaxLengthForKeys = 128;
        // If you change this, you need to change the regular expression in the Vue code too!
        config.Password.RequiredLength = 8;
        config.Password.RequireDigit = true;
        config.Password.RequireLowercase = true;
        config.Password.RequireUppercase = true;
        config.Password.RequireNonAlphanumeric = false;
        config.User.RequireUniqueEmail = true;
        config.SignIn.RequireConfirmedEmail = true;
        config.Lockout.MaxFailedAccessAttempts = 10;
      })
        .AddDefaultUI()
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<CodeCampContext>();

      svcs.AddControllersWithViews()
        .AddNewtonsoftJson()
        .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

      if (_env.IsDevelopment())
      {
        svcs.AddRazorPages()
          .AddRazorRuntimeCompilation();
      }
      else
      {
        svcs.AddRazorPages();
      }
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app,
      ILoggerFactory loggerFactory,
      IConfiguration config,
      IHostApplicationLifetime appLifetime,
      ILogger<Startup> logger)
    {
      var url = _config["SiteSettings:Redirect"];
      if (url is not null) {
        app.Run(ctx => {
          ctx.Response.Redirect(url, true);
          return Task.CompletedTask;
        });
      }

      if (_env.IsDevelopment() || config["SiteSettings:ShowErrors"].ToLower() == "true")
      {
        app.UseDeveloperExceptionPage();
        app.UseStatusCodePages();
      }

      if (_env.IsProduction())
      {
        app.UseStatusCodePages(new StatusCodePagesOptions()
        {
          HandleAsync = ctx =>
          {
            // Ignore if from Static Files
            if (ctx.HttpContext.Response.StatusCode != 404 &&
            ctx.HttpContext.Items.ContainsKey(IGNORE_STATUS_CODE_PAGES) &&
            ((bool)ctx.HttpContext.Items[IGNORE_STATUS_CODE_PAGES]))
            {
              logger.LogInformation($"Ignoring File Not Found from Static Files: {ctx.HttpContext.Request.Path}");
            }
            else
            {
              ctx.HttpContext.Response.Redirect("/error/404");
            }

            return ctx.Next.Invoke(ctx.HttpContext);
          }
        });

        app.UseExceptionHandler("/Error/Exception");
        app.UseHttpsRedirection();

        SetupLoggerly(loggerFactory, appLifetime, config);

      }

      app.UseStaticFiles(new StaticFileOptions()
      {
        OnPrepareResponse = ctx =>
        {
          if (ctx.Context.Response.StatusCode == 404)
          {
            // Mark this as ignore
            ctx.Context.Items.Add(IGNORE_STATUS_CODE_PAGES, true);
          }
        }
      });

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(cfg =>
      {
        cfg.MapControllers();
        CreateRoutes(cfg);
        cfg.MapRazorPages();
      });

    }

    private void SetupLoggerly(ILoggerFactory loggerFactory, IHostApplicationLifetime appLifetime, IConfiguration config)
    {
      var logConfig = LogglyConfig.Instance;

      // Setup Basics
      logConfig.CustomerToken = config["Loggerly:Token"];
      logConfig.ApplicationName = config["Loggerly:AppName"];

      // Setup Host
      logConfig.Transport.EndpointHostname = config["Loggerly:EndpointHostname"];
      logConfig.Transport.EndpointPort = 443;
      logConfig.Transport.LogTransport = LogTransport.Https;

      // Add Tag
      var ct = new ApplicationNameTag();
      ct.Formatter = "application-{0}";
      logConfig.TagConfig.Tags.Add(ct);

      // Setup Level to Log
      var logLevel = _env.IsProduction() ? LogEventLevel.Warning : LogEventLevel.Debug;

      // Setup Serilog
      Log.Logger = new LoggerConfiguration()
      .Enrich.FromLogContext()
      .WriteTo.Loggly(logLevel)
      .CreateLogger();

      // Add Serilog
      loggerFactory.AddSerilog();

      // Ensure that log is flushed
      appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
    }

    void CreateRoutes(IEndpointRouteBuilder bldr)
    {
      bldr.MapControllerRoute("Events",
        "{moniker}/{controller=Root}/{action=Index}/{id?}"
        );

      bldr.MapControllerRoute("Default",
        "{controller=Root}/{action=Index}/{id?}"
        );

    }
  }
}
