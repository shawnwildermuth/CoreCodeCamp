using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Data.Entities
{
  public class TimeSlot
  {
    public int Id { get; set; }
    public DateTime Time { get; set; }

    public EventInfo Event { get; set; }

  }
}
