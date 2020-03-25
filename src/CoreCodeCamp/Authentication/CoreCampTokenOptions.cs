using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Authentication
{
  public class CoreCodeCampTokenOptions
  {
    public string Issuer { get; set; } = "http://localhost:5000";
    public string Audience { get; set; } = "users";
    public string SigningKey { get; set; }
    public int ExpirationLength { get; set; } = 30;
  }
}
