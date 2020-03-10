using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreCodeCamp.Data
{
  public class CodeCampRepository : ICodeCampRepository
  {
    private CodeCampContext _ctx;
    private readonly IMapper _mapper;

    public CodeCampRepository(CodeCampContext ctx, IMapper mapper)
    {
      _ctx = ctx;
      _mapper = mapper;
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
        .ThenInclude(t => t.Track)
        .Include(s => s.Talks)
        .ThenInclude(t => t.TimeSlot)
        .Include(s => s.Talks)
        .ThenInclude(t => t.Room)
        .Where(s => s.UserName == userName && s.Event.Moniker == moniker)
        .FirstOrDefault();
    }

    public Speaker GetSpeaker(int id)
    {
      return _ctx.Speakers
        .Include(s => s.Talks)
        .ThenInclude(t => t.Track)
        .Include(s => s.Talks)
        .ThenInclude(t => t.TimeSlot)
        .Include(s => s.Talks)
        .ThenInclude(t => t.Room)
        .Where(s => s.Id == id)
        .FirstOrDefault();
    }

    public IEnumerable<Speaker> GetSpeakers(string moniker)
    {
      return _ctx.Speakers
        .Include(s => s.Talks)
        .ThenInclude(t => t.Track)
        .Include(s => s.Talks)
        .ThenInclude(t => t.TimeSlot)
        .Include(s => s.Talks)
        .ThenInclude(t => t.Room)
        .Where(s => s.Event.Moniker == moniker)
        .ToList();
    }

    public Sponsor GetSponsor(int id)
    {
      return _ctx.Sponsors.Where(s => s.Id == id).FirstOrDefault();
    }

    public IEnumerable<Sponsor> GetSponsors(string moniker)
    {
      var sponsorOrder = new List<string> {
        "Platinum",
        "Attendee Party",
        "Attendee Shirts",
        "Speaker Dinner",
        "Speaker Shirts",
        "Gold",
        "Silver",
        "Swag",
        "Other"
      };

      return _ctx.Sponsors
        .Where(e => e.Event.Moniker == moniker)
        .OrderBy(s => Guid.NewGuid())
        .ToList()
        .OrderBy(s => sponsorOrder.IndexOf(s.SponsorLevel))
        .ToList();
    }

    public IEnumerable<Talk> GetTalks(string moniker)
    {
      return _ctx.Talks
        .Include(t => t.Room)
        .Include(t => t.TimeSlot)
        .Include(t => t.Track)
        .Include(t => t.Speaker.Event)
        .Where(t => t.Speaker.Event.Moniker == moniker)
        .OrderBy(t => t.Title)
        .ToList();
    }

    public IEnumerable<Tuple<Talk, int>> GetTalkCounts(string moniker)
    {
      return _ctx.FavoriteTalks
        .Where(f => f.Talk.Speaker.Event.Moniker == moniker)
        .GroupBy(f => f.Talk)
        .Select(f => Tuple.Create(f.Key, f.Count()))
        .ToList();
    }

    public Talk GetTalk(int id)
    {
      return _ctx.Talks
        .Include(t => t.Room)
        .Include(t => t.TimeSlot)
        .Include(t => t.Track)
        .Include(t => t.Speaker.Event)
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
        .ThenInclude(t => t.Track)
        .Include(s => s.Talks)
        .ThenInclude(t => t.TimeSlot)
        .Include(s => s.Talks)
        .ThenInclude(t => t.Room)
        .Include(s => s.Event)
        .ThenInclude(e => e.Location)
        .Where(s => s.Name.ToLower() == transformName && s.Event.Moniker == moniker)
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
        .ThenInclude(f => f.Talk.TimeSlot)
        .Where(u => u.UserName == name)
        .FirstOrDefault();

      if (user == null) return new List<Talk>();

      return user.FavoriteTalks
        .Where(t => t.Talk.Speaker.Event.Moniker == moniker)
        .Select(t => t.Talk)
        .OrderBy(t => t.TimeSlot.Time)
        .ToList();
    }

    public IEnumerable<Room> GetRooms(string moniker)
    {
      return _ctx.Rooms.Where(r => r.Event.Moniker == moniker).OrderBy(r => r.Name).ToList();
    }

    public IEnumerable<Track> GetTracks(string moniker)
    {
      return _ctx.Tracks.Where(r => r.Event.Moniker == moniker).OrderBy(r => r.Name).ToList();
    }

    public IEnumerable<TimeSlot> GetTimeSlots(string moniker)
    {
      return _ctx.TimeSlots
        .Where(r => r.Event.Moniker == moniker)
        .OrderBy(r => r.Time)
        .ToList();
    }

    public Room GetRoom(string moniker, int id)
    {
      return _ctx.Rooms.Where(r => r.Id == id).FirstOrDefault();
    }

    public Track GetTrack(string moniker, int id)
    {
      return _ctx.Tracks.Where(r => r.Id == id).FirstOrDefault();
    }

    public TimeSlot GetTimeSlot(string moniker, int id)
    {
      return _ctx.TimeSlots.Where(r => r.Id == id).FirstOrDefault();
    }

    public List<IEnumerable<ScheduleModel>> GetTalksInSlots(string moniker)
    {
      var talks = GetTalks(moniker).ToList();

      var slots = from t in talks
                  where t.TimeSlot != null && t.Room != null && t.Approved
                  orderby t.TimeSlot.Time, t.Room.Name
                  group t by t.TimeSlot.Time into g
                  select new ScheduleModel()
                  {
                    Time = g.Key,
                    Talks = g.ToList()
                  };

      // split the slots into before/after lunch
      var results = new List<IEnumerable<ScheduleModel>>()
      {
        slots.Where(a => a.Time.TimeOfDay < TimeSpan.FromHours(12)).ToList(),
        slots.Where(a => a.Time.TimeOfDay > TimeSpan.FromHours(12)).ToList()
      };

      return results;
    }

    public Speaker MigrateSpeakerForCurrentUser(string moniker, CodeCampUser user)
    {
      var speaker = GetSpeakerForCurrentUser(moniker, user.UserName);
      if (speaker != null) return speaker; // Failsafe

      // Test for user name otherwise use slug for older speakers
      var slug = user.Name.Replace(" ", "-");

      var oldSpeaker = _ctx.Speakers
        .Where(s => s.UserName == user.UserName || s.Slug.ToLower() == slug.ToLower())
        .OrderByDescending(s => s.Event.EventDate)
        .FirstOrDefault();

      // Not an old speaker
      if (oldSpeaker == null) return null;

      speaker = new Speaker();
      _mapper.Map(oldSpeaker, speaker);

      var currentEvent = GetEventInfo(moniker);
      speaker.Event = currentEvent;
      speaker.UserName = user.UserName;

      AddOrUpdate(speaker);

      return speaker;

    }
  }
}
