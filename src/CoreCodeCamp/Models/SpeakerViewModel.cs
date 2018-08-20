using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Models
{
  public class SpeakerViewModel
  {

    [Required]
    [StringLength(100, MinimumLength = 5)]
    [Display(Name = "Name", Description = "Your Full Name.")]
    public string Name { get; set; }

    [StringLength(255)]
    [Display(Name = "Company Website", Description = "Your company's website.")]
    public string Website { get; set; }

    [StringLength(255)]
    [RegularExpression(@"^@?(\w){1,15}$", ErrorMessage = "Must be a valid twitter handle without the @ sign.")]
    [Display(Name = "Twitter Handle", Description = "Your twitter name (without the '@' symbol).")]
    public string Twitter { get; set; }

    [StringLength(255)]
    [Display(Name = "Your Blog (if any)", Description = "A web address.")]
    public string Blog { get; set; }

    [Required]
    [StringLength(2048, MinimumLength = 100)]
    [Display(Name = "Your Biography", Description = "How you got started!")]
    public string Bio { get; set; }

    [Required]
    public string ImageUrl { get; set; }

    [StringLength(255)]
    [Display(Name = "Title", Description = "Your professional title.")]
    public string Title { get; set; }

    [StringLength(255)]
    [Display(Name = "Company", Description = "Where do you work?")]
    public string CompanyName { get; set; }

    [StringLength(255)]
    [Display(Name = "Company Website", Description = "Company URL")]
    public string CompanyUrl { get; set; }

    [Required]
    [Phone]
    [Display(Name = "Phone", Description = "Your phone number in case we need to get a hold of you.")]
    public string PhoneNumber { get; set; }

    [Required]
    [Display(Name = "T-Shirt Size", Description = "For an event shirt!")]
    public string TShirtSize { get; set; }

    public int Id { get; set; }
    public string Email { get; set; }
    public string SpeakerLink { get; set; }

    public ICollection<TalkViewModel> Talks { get; set; }

  }
}
