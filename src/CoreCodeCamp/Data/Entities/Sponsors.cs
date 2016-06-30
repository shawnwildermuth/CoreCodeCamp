using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Data.Entities
{
  public class Sponsor
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public string Link { get; set; }
    public string SponsorLevel { get; set; }
    public bool Paid { get; set; }
  }
}
