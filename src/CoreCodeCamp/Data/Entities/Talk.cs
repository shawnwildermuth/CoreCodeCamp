using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Data.Entities
{
    public class Talk
    {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Abstract { get; set; }
    public string Level { get; set; }
    public bool Approved { get; set; }
    public int Votes { get; set; }

    public Track Track { get; set; }
    public Room Room { get; set; }
    public TalkTime TalkTime { get; set; }

    public ICollection<Category> Categories { get; set; }

  }
}
