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

    public void AddOrUpdate(object entity)
    {
      var state = _ctx.Entry(entity).State;

      switch (state)
      {
        case EntityState.Detached:
          _ctx.Add(entity);
          break;
        case EntityState.Modified:
          _ctx.Update(entity);
          break;
        case EntityState.Added:
        case EntityState.Deleted:
        case EntityState.Unchanged:
          // do nothing
          break;
      }
    }

    public void Delete(object entity)
    {
      _ctx.Remove(entity);
    }

    public IEnumerable<EventInfo> GetAllEventInfo()
    {
      return _ctx.CodeCampEvents
        .Include(e => e.Location)
        .OrderByDescending(e => e.EventDate)
        .ToList();
    }

    public IEnumerable<Talk> GetTalks(string moniker)
    {
      return _ctx.Talks
        .Include(t => t.Speaker)
        .Include(t => t.TalkTime)
        .Where(t => t.Speaker.Event.Moniker == moniker)
        .OrderBy(t => t.Title)
        .ToList();
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

    public Speaker GetSpeakerForCurrentUser(string moniker, string userName)
    {
      return _ctx.Speakers
        .Include(s => s.Talks)
        .Where(s => s.UserName == userName && s.Event.Moniker == moniker)
        .FirstOrDefault();
    }

    public Speaker GetSpeaker(int id)
    {
      return _ctx.Speakers
        .Include(s => s.Talks)
        .ThenInclude(s => s.Room)
        .Include(s => s.Talks)
        .ThenInclude(s => s.TalkTime)
        .Where(s => s.Id == id)
        .FirstOrDefault();
    }

    public IEnumerable<Speaker> GetSpeakers(string moniker)
    {
      return _ctx.Speakers.Include(s => s.Talks).Where(s => s.Event.Moniker == moniker).ToList();
    }

    public Sponsor GetSponsor(int id)
    {
      return _ctx.Sponsors.Where(s => s.Id == id).FirstOrDefault();
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

    public Talk GetTalk(int id)
    {
      return _ctx.Talks
        .Include(t => t.Room)
        .Include(t => t.TalkTime)
        .Where(t => t.Id == id)
        .FirstOrDefault();
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

    public bool ToggleTalkForUser(string moniker, string userName, int talkId)
    {
      var user = _ctx.Users
        .Include(u => u.FavoriteTalks)
        .ThenInclude(f => f.Talk.Speaker.Event)
        .Where(u => u.UserName == userName)
        .First();

      var fav = user.FavoriteTalks.Where(t => t.Talk.Id == talkId && t.Talk.Speaker.Event.Moniker == moniker).FirstOrDefault();
      if (fav == null)
      {
        fav = new FavoriteTalk()
        {
          User = user,
          Talk = _ctx.Talks.Where(t => t.Id == talkId).First()
        };
        user.FavoriteTalks.Add(fav);
        return true;
      }
      else
      {
        user.FavoriteTalks.Remove(fav);
        return false;
      }
    }

    public Speaker GetSpeakerByName(string moniker, string name)
    {
      var transformName = name.Replace("-", " ").ToLower();
      return _ctx.Speakers
        .Include(s => s.Talks)
        .ThenInclude(s => s.Room)
        .Where(s => s.Name.ToLower() == transformName)
        .FirstOrDefault();
    }

    public IEnumerable<Talk> GetUserWithFavoriteTalksForEvent(string name, string moniker)
    {
      var user = _ctx.Users
        .Include(u => u.FavoriteTalks)
        .ThenInclude(f => f.Talk.Speaker)
        .ThenInclude(s => s.Event)
        .Include(u => u.FavoriteTalks)
        .ThenInclude(f => f.Talk.Room)
        .Include(u => u.FavoriteTalks)
        .ThenInclude(f => f.Talk.TalkTime)
        .Where(u => u.UserName == name)
        .First();

      if (user == null) return new List<Talk>();

      return user.FavoriteTalks
        .Where(t => t.Talk.Speaker.Event.Moniker == moniker)
        .Select(t => t.Talk)
        .ToList();
    }
  }
}
