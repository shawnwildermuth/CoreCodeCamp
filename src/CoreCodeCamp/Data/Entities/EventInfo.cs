using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Data.Entities
{
  public class EventInfo
  {
    public int Id { get; set; }
    public string Moniker { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsDefault { get; set; }
    public DateTime EventDate { get; set; }
    public bool IsPublic { get; set; }
    public short EventLength { get; set; }
    public string ContactEmail { get; set; }
    public string FacebookLink { get; set; }
    public string TwitterLink { get; set; }
    public string InstagramLink { get; set; }
    public DateTime CallForSpeakersOpened { get; set; }
    public DateTime CallForSpeakersClosed { get; set; }
    public string RegistrationLink { get; set; }

    public EventLocation Location { get; set; }
  }
}
