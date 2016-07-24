using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Data.Entities;

namespace CoreCodeCamp.Models
{
  public class TalkViewModel
  {
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }
    [Required]
    public string Abstract { get; set; }
    public string Prerequisites { get; set; }
    [Required]
    public string Audience { get; set; }
    [Required]
    public string Category { get; set; }
    [Required]
    public string Level { get; set; }
    public bool Approved { get; set; }

    public string Room { get; set; }
    public DateTime Time { get; set; }
    public string Track { get; set; }

    public string PresentationUrl { get; set; }
    public string CodeUrl { get; set; }
    public string SpeakerDeckUrl { get; set; }
    public string SpeakerRateUrl { get; set; }

    public bool Favorite { get; set; }
    public SpeakerViewModel Speaker { get; set; }
  }
}
