using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodeCamp.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CoreCodeCamp.Data.Entities
{
  // Add profile data for application users by adding properties to the ApplicationUser class
  public class CodeCampUser : IdentityUser
  {
    public ICollection<Talk> Talks { get; set; }
  }
}
