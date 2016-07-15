using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Models
{
  public class SponsorViewModel
  {
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
    [Required]
    public string ImageUrl { get; set; }
    [Required]
    public string Link { get; set; }
    [Required]
    public string SponsorLevel { get; set; }

    public bool Paid { get; set; }

  }
}
