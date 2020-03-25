using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Models;

namespace CoreCodeCamp.Data
{
  public interface ICodeCampRepository
  {
    Task<IEnumerable<CodeCampUser>> GetUsersAsync();
    Task<IEnumerable<Talk>> GetUserWithFavoriteTalksForEventAsync(string name, string moniker);

    Task<IEnumerable<EventInfo>> GetAllEventInfoAsync();
    Task<EventInfo> GetEventInfoAsync(string moniker);
    Task<EventInfo> GetCurrentEventAsync();

    Task<IEnumerable<Sponsor>> GetSponsorsAsync(string moniker);
    Task<Sponsor> GetSponsorAsync(int id);

    Task<IEnumerable<Speaker>> GetSpeakersAsync(string moniker);
    Task<Speaker> GetSpeakerForCurrentUserAsync(string moniker, string name);
    Task<Speaker> MigrateSpeakerForCurrentUserAsync(string moniker, CodeCampUser user);
    Task<Speaker> GetSpeakerAsync(int id);
    Task<Speaker> GetSpeakerByNameAsync(string moniker, string name);

    Task<IEnumerable<Room>> GetRoomsAsync(string moniker);
    Task<Room> GetRoomAsync(string moniker, int id);

    Task<IEnumerable<Tuple<Talk, int>>> GetTalkCountsAsync(string moniker);

    Task<IEnumerable<Track>> GetTracksAsync(string moniker);
    Task<Track> GetTrackAsync(string moniker, int id);

    Task<IEnumerable<TimeSlot>> GetTimeSlotsAsync(string moniker);
    Task<TimeSlot> GetTimeSlotAsync(string moniker, int id);

    Task<IEnumerable<Talk>> GetTalksAsync(string moniker);
    Task<Talk> GetTalkAsync(int id);
    Task<bool> ToggleTalkForUserAsync(string moniker, string userName, int talkId);
    Task<List<IEnumerable<ScheduleModel>>> GetTalksInSlotsAsync(string moniker);

    void AddOrUpdate(object entity);
    void Delete(object entity);
    Task<int> SaveChangesAsync();
  }
}