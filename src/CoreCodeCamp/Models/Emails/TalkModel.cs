using CoreCodeCamp.Data.Entities;

namespace CoreCodeCamp.Models.Emails
{
  public class TalkModel : EmailModel
  {
    public Talk Talk { get; set; }
    public string SpeakerUrl { get; set; }
  }
}