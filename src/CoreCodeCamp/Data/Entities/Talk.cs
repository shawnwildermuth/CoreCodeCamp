using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Data.Entities
{
    public class Talk
    {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Abstract { get; set; }
    public string Prerequisites { get; set; }
    public string PresentationUrl { get; set; }
    public string CodeUrl { get; set; }
    public string Audience { get; set; }
    public string SpeakerDeckUrl { get; set; }
    public string SpeakerRateUrl { get; set; }
    public int Level { get; set; }
    public bool Approved { get; set; }

    public Track Track { get; set; }
    public Room Room { get; set; }
    public TalkTime TalkTime { get; set; }

    public ICollection<Category> Categories { get; set; }

  }
}
