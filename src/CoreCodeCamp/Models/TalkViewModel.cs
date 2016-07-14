using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Models
{
  public class TalkViewModel
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Abstract { get; set; }
    public string Prerequisites { get; set; }
    public string Audience { get; set; }
    public string Category { get; set; }
    public string Level { get; set; }
    public bool Approved { get; set; }

    public string PresentationUrl { get; set; }
    public string CodeUrl { get; set; }
    public string SpeakerDeckUrl { get; set; }
    public string SpeakerRateUrl { get; set; }
  }
}
