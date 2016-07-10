using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodeCamp.Data.Entities;

namespace CoreCodeCamp.Data
{
  public interface ICodeCampRepository
  {
    IEnumerable<Talk> GetAllTalks();

    IEnumerable<EventInfo> GetAllEventInfo();
    EventInfo GetEventInfo(string moniker);
    EventInfo GetCurrentEvent();
    IEnumerable<Sponsor> GetSponsors(string moniker);
    IEnumerable<CodeCampUser> GetUsers();
    Speaker GetSpeaker(string name);
    void AddOrUpdateSpeaker(Speaker speaker);
    Task<int> SaveChangesAsync();
  }
}