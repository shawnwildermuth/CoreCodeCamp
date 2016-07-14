using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodeCamp.Data.Entities;

namespace CoreCodeCamp.Data
{
  public interface ICodeCampRepository
  {
    IEnumerable<EventInfo> GetAllEventInfo();
    EventInfo GetEventInfo(string moniker);
    EventInfo GetCurrentEvent();
    IEnumerable<Sponsor> GetSponsors(string moniker);
    IEnumerable<CodeCampUser> GetUsers();

    Speaker GetSpeaker(string moniker, string name);

    IEnumerable<Talk> GetAllTalks();
    Talk GetTalk(int id);

    void AddOrUpdate(object entity);
    void Delete(object entity);
    Task<int> SaveChangesAsync();
  }
}