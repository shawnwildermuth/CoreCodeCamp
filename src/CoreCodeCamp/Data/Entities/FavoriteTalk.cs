using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Data.Entities
{
  public class FavoriteTalk
  {
    public int Id { get; set; }
    public CodeCampUser User { get; set; }
    public Talk Talk { get; set; }
  }
}
