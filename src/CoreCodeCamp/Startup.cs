using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Models;
using CoreCodeCamp.Models.Admin;
using CoreCodeCamp.Services;
using Loggly;
using Loggly.Config;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace CoreCodeCamp
{
  public class Startup
  {
    IHostingEnvironment _env;
    IConfigurationRoot _config;

    public Startup(IHostingEnvironment env)
    {
      _env = env;

      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", false, true)
          .AddEnvironmentVariables();

      _config = builder.Build();

    }


    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection svcs)
    {
      svcs.AddSingleton(f => _config);

      if (_env.IsProduction())
      {
        svcs.AddScoped<IMailService, SendGridMailService>();
      }
      else
      {
        svcs.AddScoped<IMailService, DebugMailService>();
      }

      // Add framework services.
      svcs.AddDbContext<CodeCampContext>();
      svcs.AddScoped<ICodeCampRepository, CodeCampRepository>();
      svcs.AddTransient<CodeCampSeeder>();
      svcs.AddTransient<ViewRenderer>();

      // Configure Identity (Security)
      svcs.AddIdentity<CodeCampUser, IdentityRole>(config =>
      {
        config.Password.RequiredLength = 8;
        config.Password.RequireDigit = true;
        config.Password.RequireNonAlphanumeric = false;
        config.User.RequireUniqueEmail = true;
        config.User.RequireUniqueEmail = true;
        config.SignIn.RequireConfirmedEmail = true;
        config.Lockout.MaxFailedAccessAttempts = 10;
        config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
        {
          OnRedirectToLogin = async ctx =>
          {
            if (ctx.Request.Path.Value.Contains("/api/") &&
              ctx.Response.StatusCode == 200)
            {
              ctx.Response.StatusCode = 401;
            }
            else
            {
              ctx.Response.Redirect(ctx.RedirectUri);
            }
            await Task.Yield();
          }
        };
      })
          .AddEntityFrameworkStores<CodeCampContext>()
          .AddDefaultTokenProviders();

      svcs.AddMvc(opt =>
      {
        if (_env.IsProduction())
        {
          opt.Filters.Add(new RequireHttpsAttribute());
        }
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, 
      ILoggerFactory loggerFactory, 
      CodeCampSeeder seeder, 
      ICodeCampRepository repo,
      IApplicationLifetime appLifetime)
    {
      loggerFactory.AddConsole(_config.GetSection("Logging"));

      Mapper.Initialize(CreateMaps);

      if (_env.IsDevelopment() || _config["SiteSettings:ShowErrors"].ToLower() == "true")
      {
        loggerFactory.AddDebug(LogLevel.Information);
        app.UseDeveloperExceptionPage();
        app.UseDatabaseErrorPage();
        app.UseStatusCodePages();
      }

      if (_env.IsProduction())
      {
        app.UseStatusCodePagesWithRedirects("~/Error/{0}");
        app.UseExceptionHandler("/Error/Exception");

        SetupLoggerly(loggerFactory, appLifetime);

      }

      app.UseStaticFiles();

      app.UseIdentity();

      // Need seed data before we create the routes!
      seeder.SeedAsync().Wait();

      app.UseMvc(CreateRoutes);

    }

    private void SetupLoggerly(ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
    {
      var logConfig = LogglyConfig.Instance;

      // Setup Basics
      logConfig.CustomerToken = _config["Loggerly:Token"];
      logConfig.ApplicationName = _config["Loggerly:AppName"];

      // Setup Host
      logConfig.Transport.EndpointHostname = _config["Loggerly:EndpointHostname"];
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

    void CreateMaps(IMapperConfiguration config)
    {
      config.CreateMap<CodeCampUser, CodeCampUserViewModel>()
        .ReverseMap();

      config.CreateMap<SpeakerViewModel, Speaker>()
        .ForMember(m => m.Talks, opt => opt.Ignore());
      config.CreateMap<Speaker, SpeakerViewModel>()
        .ForMember(m => m.Talks, opt => opt.Ignore())
        .ForMember(m => m.SpeakerLink, opt => opt.MapFrom(s => s.Event == null ? "" : $"/{s.Event.Moniker}/Speakers/{s.Name.Replace(" ", "-")}"))
        .ForMember(m => m.Email, opt => opt.MapFrom(s => s.UserName));

      config.CreateMap<Talk, TalkViewModel>()
        .ForMember(m => m.Room, opt => opt.MapFrom(t => t.Room.Name))
        .ForMember(m => m.Time, opt => opt.MapFrom(t => t.TimeSlot.Time))
        .ForMember(m => m.Track, opt => opt.MapFrom(t => t.Track.Name));

      config.CreateMap<TalkViewModel, Talk>()
        .ForMember(m => m.Room, opt => opt.Ignore())
        .ForMember(m => m.TimeSlot, opt => opt.Ignore())
        .ForMember(m => m.Track, opt => opt.Ignore());

      config.CreateMap<Sponsor, SponsorViewModel>().ReverseMap();

      config.CreateMap<Talk, FavoriteTalkViewModel>()
        .ForMember(dest => dest.Room, opt => opt.MapFrom(s => s.Room.Name))
        .ForMember(dest => dest.Time, opt => opt.MapFrom(s => s.TimeSlot.Time))
        .ForMember(dest => dest.SpeakerName, opt => opt.MapFrom(s => s.Speaker.Name))
        .ForMember(dest => dest.Title, opt => opt.MapFrom(s => s.Title))
        .ForMember(dest => dest.Abstract, opt => opt.MapFrom(s => s.Abstract));

      config.CreateMap<EventInfo, EventInfoViewModel>().ReverseMap();
      config.CreateMap<EventLocation, EventLocationViewModel>().ReverseMap();
    }

    void CreateRoutes(IRouteBuilder routes)
    {
      routes.MapRoute(
        name: "Events",
        template: string.Concat("{moniker}/{controller=Root}/{action=Index}/{id?}")
        );

      routes.MapRoute(
        name: "Default",
        template: "{controller=Root}/{action=Index}/{id?}"
        );

    }
  }
}
