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
        .FirstOrDefault();
    }

    public IEnumerable<Sponsor> GetSponsors(string moniker)
    {
      return _ctx.Sponsors
        .Where(e => e.Event.Moniker == moniker)
        .OrderBy(s => Guid.NewGuid())
        .ToList();
    }

    public IEnumerable<CodeCampUser> GetUsers()
    {
      return _ctx.Users
        .OrderBy(u => u.UserName)
        .ToList();
    }
  }
}
