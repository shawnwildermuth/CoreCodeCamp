using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodeCamp.Data.Entities;

namespace CoreCodeCamp.Data
{
  public interface ICodeCampRepository
  {
    IEnumerable<CodeCampUser> GetUsers();
    IEnumerable<Talk> GetUserWithFavoriteTalksForEvent(string name, string moniker);

    IEnumerable<EventInfo> GetAllEventInfo();
    EventInfo GetEventInfo(string moniker);
    EventInfo GetCurrentEvent();

    IEnumerable<Sponsor> GetSponsors(string moniker);
    Sponsor GetSponsor(int id);

    IEnumerable<Speaker> GetSpeakers(string moniker);
    Speaker GetSpeakerForCurrentUser(string moniker, string name);
    Speaker GetSpeaker(int id);
    Speaker GetSpeakerByName(string moniker, string name);

    IEnumerable<Talk> GetTalks(string moniker);
    Talk GetTalk(int id);
    bool ToggleTalkForUser(string moniker, string userName, int talkId);

    void AddOrUpdate(object entity);
    void Delete(object entity);
    Task<int> SaveChangesAsync();
  }
}