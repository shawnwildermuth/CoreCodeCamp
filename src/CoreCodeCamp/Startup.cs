using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Models;
using CoreCodeCamp.Models.Admin;
using CoreCodeCamp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddSingleton(f => _config);

      if (_env.IsProduction())
      {
        services.AddScoped<IMailService, SendGridMailService>();
      }
      else
      {
        services.AddScoped<IMailService, DebugMailService>();
      }

      // Add framework services.
      services.AddDbContext<CodeCampContext>();
      services.AddScoped<ICodeCampRepository, CodeCampRepository>();
      services.AddTransient<CodeCampSeeder>();

      // Configure Identity (Security)
      services.AddIdentity<CodeCampUser, IdentityRole>(config =>
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

      services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, CodeCampSeeder seeder, ICodeCampRepository repo)
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
      else
      {
        app.UseStatusCodePagesWithRedirects("~/Error/{0}");
        app.UseExceptionHandler("/Error/Exception");
      }

      app.UseStaticFiles();

      app.UseIdentity();

      // Need seed data before we create the routes!
      seeder.SeedAsync().Wait();

      app.UseMvc(CreateRoutes);

    }

    void CreateMaps(IMapperConfiguration config)
    {
      config.CreateMap<CodeCampUser, CodeCampUserViewModel>()
        .ReverseMap();

      config.CreateMap<SpeakerViewModel, Speaker>()
        .ForMember(m => m.Talks, opt => opt.Ignore());
      config.CreateMap<Speaker, SpeakerViewModel>()
        .ForMember(m => m.Talks, opt => opt.Ignore());

      config.CreateMap<Talk, TalkViewModel>();
      config.CreateMap<TalkViewModel, Talk>();

      config.CreateMap<Sponsor, SponsorViewModel>().ReverseMap();

      config.CreateMap<Talk, FavoriteTalkViewModel>()
        .ForMember(dest => dest.Room, opt => opt.MapFrom(s => s.Room.Name))
        .ForMember(dest => dest.Time, opt => opt.MapFrom(s => s.TalkTime.Name))
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
