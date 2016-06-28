using System.Collections.Generic;
using CoreCodeCamp.Data.Entities;

namespace CoreCodeCamp.Data
{
  public interface ICodeCampRepository
  {
    IEnumerable<Talk> GetAllTalks();
  }
}