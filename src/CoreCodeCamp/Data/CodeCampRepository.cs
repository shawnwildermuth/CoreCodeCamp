using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreCodeCamp.Data
{
  public class CodeCampRepository : ICodeCampRepository
  {
    private CodeCampContext _ctx;

    public CodeCampRepository(CodeCampContext ctx)
    {
      _ctx = ctx;
    }

    public void AddOrUpdateSpeaker(Speaker speaker)
    {
      if (_ctx.Entry(speaker).State == EntityState.Detached)
      {
        _ctx.Add(speaker);
      }
      else
      {
        _ctx.Update(speaker);
      }
    }

    public IEnumerable<EventInfo> GetAllEventInfo()
    {
      return _ctx.CodeCampEvents
        .Include(e => e.Location)
        .OrderByDescending(e => e.EventDate)
        .ToList();
    }

    public IEnumerable<Talk> GetAllTalks()
    {
      return _ctx.Talks.ToList();
    }

    public EventInfo GetCurrentEvent()
    {
      return _ctx.CodeCampEvents
        .Include(e => e.Location)
        .OrderByDescending(e => e.EventDate)
        .FirstOrDefault();
    }

    public EventInfo GetEventInfo(string moniker)
    {
      return _ctx.CodeCampEvents
        .Include(e => e.Location)
        .Where(e => e.Moniker == moniker)
        .FirstOrDefault();
    }

    public Speaker GetSpeaker(string userName)
    {
      return _ctx.Speakers
        .Include(s => s.Talks)
        .Where(s => s.UserName == userName)
        .FirstOrDefault();
    }

    public IEnumerable<Sponsor> GetSponsors(string moniker)
    {
      var sponsorOrder = new List<string> { "Platinum", "Lunch", "T-Shirt", "Gold", "Silver", "Other" };

      return _ctx.Sponsors
        .Where(e => e.Event.Moniker == moniker)
        .OrderBy(s => Guid.NewGuid())
        .ToList()
        .OrderBy(s => sponsorOrder.IndexOf(s.SponsorLevel))
        .ToList();
    }

    public IEnumerable<CodeCampUser> GetUsers()
    {
      return _ctx.Users
        .OrderBy(u => u.UserName)
        .ToList();
    }

    public Task<int> SaveChangesAsync()
    {
      return _ctx.SaveChangesAsync();
    }
  }
}
