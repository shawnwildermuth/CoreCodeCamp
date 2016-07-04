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
    
    public EventLocation Location { get; set; }
  }
}
