using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities = CoreCodeCamp.Data.Entities;

namespace CoreCodeCamp.Data.Sessionize
{
  public partial class SessionizeResult 
  {
    public Entities.Speaker[] ConvertToSpeakers(IMapper mapper)
    {
      var results = mapper.Map<List<Entities.Speaker>>(this.Speakers);



      return results.ToArray();
      
    }
  }
}
