using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Data.Entities;

namespace CoreCodeCamp.Data
{
  public class CodeCampRepository : ICodeCampRepository
  {
    private CodeCampContext _ctx;

    public CodeCampRepository(CodeCampContext ctx)
    {
      _ctx = ctx;
    }

    public IEnumerable<Talk> GetAllTalks()
    {
      return _ctx.Talks.ToList();
    }

  }
}
