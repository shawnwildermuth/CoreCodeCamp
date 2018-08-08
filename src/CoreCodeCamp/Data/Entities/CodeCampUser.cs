using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CoreCodeCamp.Data.Entities
{
  public class CodeCampUser : IdentityUser
  {
    public string Name { get; set; }
    public ICollection<FavoriteTalk> FavoriteTalks { get; set; }
  }
}
