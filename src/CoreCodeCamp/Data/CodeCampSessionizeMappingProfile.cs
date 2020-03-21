using AutoMapper;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Data.Sessionize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Data
{
  public class CodeCampSessionizeMappingProfile : Profile
  {
    public CodeCampSessionizeMappingProfile()
    {
      CreateMap<SessionizeSpeaker, Speaker>();
    }
  }
}
