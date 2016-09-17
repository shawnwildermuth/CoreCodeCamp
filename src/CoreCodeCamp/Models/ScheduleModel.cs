using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Data.Entities;

namespace CoreCodeCamp.Models
{
  public class ScheduleModel
  {
    public List<Talk> Talks { get; set; }
    public DateTime Time { get; set; }
  }
}
