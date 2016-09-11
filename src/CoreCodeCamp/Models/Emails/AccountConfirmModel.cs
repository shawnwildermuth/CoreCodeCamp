using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Models.Emails
{
  public class AccountConfirmModel : EmailModel
  {
    public string Callback { get; internal set; }
  }
}
