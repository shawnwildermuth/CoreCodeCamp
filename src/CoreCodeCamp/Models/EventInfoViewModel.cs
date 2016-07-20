using System;

namespace CoreCodeCamp.Models
{
  public class EventInfoViewModel
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime EventDate { get; set; }
    public short EventLength { get; set; }
    public string ContactEmail { get; set; }
    public string FacebookLink { get; set; }
    public string TwitterLink { get; set; }
    public string InstagramLink { get; set; }

    public EventLocationViewModel Location { get; set; }
    public string Moniker { get; internal set; }
  }
}