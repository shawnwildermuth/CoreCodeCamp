using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Models;
using CoreCodeCamp.Services;
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

    public async Task<IEnumerable<EventInfo>> GetAllEventInfoAsync()
    {
      return await _ctx.CodeCampEvents
        .Include(e => e.Location)
        .OrderByDescending(e => e.EventDate)
        .ToListAsync();
    }

    public async Task<EventInfo> GetCurrentEventAsync()
    {
      return await _ctx.CodeCampEvents
        .Include(e => e.Location)
        .OrderByDescending(e => e.EventDate)
        .FirstOrDefaultAsync();
    }

    public async Task<EventInfo> GetEventInfoAsync(string moniker)
    {
      return await _ctx.CodeCampEvents
        .Include(e => e.Location)
        .Where(e => e.Moniker == moniker)
        .FirstOrDefaultAsync();
    }

    public async Task<Speaker> GetSpeakerForCurrentUserAsync(string moniker, string userName)
    {
      return await _ctx.Speakers
        .Include(s => s.Talks)
        .ThenInclude(t => t.Track)
        .Include(s => s.Talks)
        .ThenInclude(t => t.TimeSlot)
        .Include(s => s.Talks)
        .ThenInclude(t => t.Room)
        .Where(s => s.UserName == userName && s.Event.Moniker == moniker)
        .FirstOrDefaultAsync();
    }

    public async Task<Speaker> GetSpeakerAsync(int id)
    {
      return await _ctx.Speakers
        .Include(s => s.Talks)
        .ThenInclude(t => t.Track)
        .Include(s => s.Talks)
        .ThenInclude(t => t.TimeSlot)
        .Include(s => s.Talks)
        .ThenInclude(t => t.Room)
        .Where(s => s.Id == id)
        .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Speaker>> GetSpeakersAsync(string moniker)
    {
      return await _ctx.Speakers
        .Include(s => s.Talks)
        .ThenInclude(t => t.Track)
        .Include(s => s.Talks)
        .ThenInclude(t => t.TimeSlot)
        .Include(s => s.Talks)
        .ThenInclude(t => t.Room)
        .Where(s => s.Event.Moniker == moniker)
        .ToListAsync();
    }

    public async Task<Sponsor> GetSponsorAsync(int id)
    {
      return await _ctx.Sponsors
        .Where(s => s.Id == id)
        .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Sponsor>> GetSponsorsAsync(string moniker)
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

      return (await _ctx.Sponsors
        .Where(e => e.Event.Moniker == moniker)
        .OrderBy(s => Guid.NewGuid())
        .ToListAsync())
        .OrderBy(s => sponsorOrder.IndexOf(s.SponsorLevel))
        .ToList();
    }

    public async Task<IEnumerable<Talk>> GetTalksAsync(string moniker)
    {
      return await _ctx.Talks
        .Include(t => t.Room)
        .Include(t => t.TimeSlot)
        .Include(t => t.Track)
        .Include(t => t.Speaker.Event)
        .Where(t => t.Speaker.Event.Moniker == moniker)
        .OrderBy(t => t.Title)
        .ToListAsync();
    }

    public async Task<IEnumerable<Tuple<Talk, int>>> GetTalkCountsAsync(string moniker)
    {
      return (await _ctx.FavoriteTalks
        .Include(c => c.Talk.Speaker.Event)
        .Where(f => f.Talk.Speaker.Event.Moniker == moniker)
        .ToListAsync())
        .GroupBy(f => f.Talk)
        .Select(f => Tuple.Create(f.Key, f.Count()))
        .ToList();
    }

    public async Task<Talk> GetTalkAsync(int id)
    {
      return await _ctx.Talks
        .Include(t => t.Room)
        .Include(t => t.TimeSlot)
        .Include(t => t.Track)
        .Include(t => t.Speaker.Event)
        .Where(t => t.Id == id)
        .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<CodeCampUser>> GetUsersAsync()
    {
      return await _ctx.Users
        .OrderBy(u => u.UserName)
        .ToListAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
      return await _ctx.SaveChangesAsync();
    }

    public async Task<bool> ToggleTalkForUserAsync(string moniker, string userName, int talkId)
    {
      var user = await _ctx.Users
        .Include(u => u.FavoriteTalks)
        .ThenInclude(f => f.Talk.Speaker.Event)
        .Where(u => u.UserName == userName)
        .FirstAsync();

      var fav = user.FavoriteTalks.Where(t => t.Talk.Id == talkId && t.Talk.Speaker.Event.Moniker == moniker).FirstOrDefault();
      if (fav == null)
      {
        fav = new FavoriteTalk()
        {
          User = user,
          Talk = await _ctx.Talks.Where(t => t.Id == talkId).FirstAsync()
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

    public async Task<Speaker> GetSpeakerByNameAsync(string moniker, string name)
    {
      var transformName = name.Replace("-", " ").ToLower();
      return await _ctx.Speakers
        .Include(s => s.Talks)
        .ThenInclude(t => t.Track)
        .Include(s => s.Talks)
        .ThenInclude(t => t.TimeSlot)
        .Include(s => s.Talks)
        .ThenInclude(t => t.Room)
        .Include(s => s.Event)
        .ThenInclude(e => e.Location)
        .Where(s => s.Name.ToLower() == transformName && s.Event.Moniker == moniker)
        .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Talk>> GetUserWithFavoriteTalksForEventAsync(string name, string moniker)
    {
      var user = await _ctx.Users
        .Include(u => u.FavoriteTalks)
        .ThenInclude(f => f.Talk.Speaker)
        .ThenInclude(s => s.Event)
        .Include(u => u.FavoriteTalks)
        .ThenInclude(f => f.Talk.Room)
        .Include(u => u.FavoriteTalks)
        .ThenInclude(f => f.Talk.TimeSlot)
        .Where(u => u.UserName == name)
        .FirstOrDefaultAsync();

      if (user == null) return new List<Talk>();

      return user.FavoriteTalks
        .Where(t => t.Talk.Speaker.Event.Moniker == moniker)
        .Select(t => t.Talk)
        .OrderBy(t => t.TimeSlot.Time)
        .ToList();
    }

    public async Task<IEnumerable<Room>> GetRoomsAsync(string moniker)
    {
      return await _ctx.Rooms
        .Where(r => r.Event.Moniker == moniker)
        .OrderBy(r => r.Name)
        .ToListAsync();
    }

    public async Task<IEnumerable<Track>> GetTracksAsync(string moniker)
    {
      return await _ctx.Tracks
        .Where(r => r.Event.Moniker == moniker)
        .OrderBy(r => r.Name)
        .ToListAsync();
    }

    public async Task<IEnumerable<TimeSlot>> GetTimeSlotsAsync(string moniker)
    {
      return await _ctx.TimeSlots
        .Where(r => r.Event.Moniker == moniker)
        .OrderBy(r => r.Time)
        .ToListAsync();
    }

    public async Task<Room> GetRoomAsync(string moniker, int id)
    {
      return await _ctx.Rooms
        .Where(r => r.Id == id)
        .FirstOrDefaultAsync();
    }

    public async Task<Track> GetTrackAsync(string moniker, int id)
    {
      return await _ctx.Tracks
        .Where(r => r.Id == id)
        .FirstOrDefaultAsync();
    }

    public async Task<TimeSlot> GetTimeSlotAsync(string moniker, int id)
    {
      return await _ctx.TimeSlots
        .Where(r => r.Id == id)
        .FirstOrDefaultAsync();
    }

    public async Task<List<IEnumerable<ScheduleModel>>> GetTalksInSlotsAsync(string moniker)
    {
      var talks = (await GetTalksAsync(moniker)).ToList();

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

    public async Task<Speaker> MigrateSpeakerForCurrentUserAsync(string moniker, CodeCampUser user)
    {
      var speaker = await GetSpeakerForCurrentUserAsync(moniker, user.UserName);
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

      var currentEvent = await GetEventInfoAsync(moniker);
      speaker.Event = currentEvent;
      speaker.UserName = user.UserName;

      AddOrUpdate(speaker);

      return speaker;

    }
  }
}
