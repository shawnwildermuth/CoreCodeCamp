using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Models
{
  public class TokenModel
  {
    public string Token { get; internal set; }
    public DateTime Expiration { get; internal set; }
  }
}
