using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Data.Entities;

namespace CoreCodeCamp.Models.Emails
{
  public class SpeakerModel : EmailModel
  {
    public Speaker Speaker { get; internal set; }
    public string SpeakerUrl { get; internal set; }
    public EventInfo Event { get; internal set; }
  }
}
