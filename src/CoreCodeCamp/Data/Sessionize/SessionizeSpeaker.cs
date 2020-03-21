using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Data.Sessionize
{
  public class SessionizeSession
  {
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public bool IsServiceSession { get; set; }
    public bool IsPlenumSession { get; set; }
    public List<SessionizeSpeaker> Speakers { get; set; }
    public List<SessionizeCategory> CategoryItems { get; set; }
    public List<SessionizeQuestion> QuestionAnswers { get; set; }
    public int RoomId { get; set; }
  }

  public class SessionizeSpeaker
  {
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Bio { get; set; }
    public string TagLine { get; set; }
    public string ProfilePicture { get; set; }
    public bool IsTopSpeaker { get; set; }
    public List<string> Links { get; set; }
    public List<int> Sessions { get; set; }
    public string FullName { get; set; }
    public List<SessionizeCategory> categoryItems { get; set; }
    public List<string> questionAnswers { get; set; }
  }

  public class SessionizeQuestion
  {
    public int Id { get; set; }
    public string Question { get; set; }
    public string QuestionType { get; set; }
    public int Sort { get; set; }
  }

  public class SessionizeItem
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Sort { get; set; }
  }

  public class SessionizeCategory
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public List<SessionizeItem> Items { get; set; }
    public int Sort { get; set; }
    public string Type { get; set; }
  }

  public class SessionizeRoom
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Sort { get; set; }
  }

  public class SessionizeResults
  {
    public List<SessionizeSession> Sessions { get; set; }
    public List<SessionizeSpeaker> Speakers { get; set; }
    public List<SessionizeQuestion> Questions { get; set; }
    public List<SessionizeCategory> Categories { get; set; }
    public List<SessionizeRoom> Rooms { get; set; }
  }
}
