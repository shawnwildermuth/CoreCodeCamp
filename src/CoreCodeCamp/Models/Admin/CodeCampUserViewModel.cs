using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Models.Admin
{
  public class CodeCampUserViewModel
  {
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public bool IsAdmin { get; set; }
  }
}
