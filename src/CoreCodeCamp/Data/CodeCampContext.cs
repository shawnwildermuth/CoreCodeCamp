using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CoreCodeCamp.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using CoreCodeCamp.Data.Entities;
using System.IO;

namespace CoreCodeCamp.Data
{
  public class CodeCampContext : IdentityDbContext<CodeCampUser>
  {
    private IHostingEnvironment _hosting;

    public CodeCampContext(IHostingEnvironment hosting, DbContextOptions<CodeCampContext> options)
        : base(options)
    {
      _hosting = hosting;
    }

    public DbSet<Talk> Talks { get; set; }
    public DbSet<Track> Tracks { get; set; }
    public DbSet<Speaker> Speakers { get; set; }
    public DbSet<TalkTime> TalkTimes { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Category> Categories { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
      // Customize the ASP.NET Identity model and override the defaults if needed.
      // For example, you can rename the ASP.NET Identity table names and more.
      // Add your customizations after calling base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      base.OnConfiguring(optionsBuilder);

      Directory.CreateDirectory($@"{_hosting.ContentRootPath}\Database\");

      var connection = new SqliteConnectionStringBuilder
      {
        DataSource = $@"{_hosting.ContentRootPath}\Database\CodeCamp.db"
      };

      optionsBuilder.UseSqlite(connection.ToString());
    }
  }
}
