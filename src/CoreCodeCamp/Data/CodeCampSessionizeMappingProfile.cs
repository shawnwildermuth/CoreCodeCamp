using AutoMapper;
using CoreCodeCamp.Data.Entities;
using Sessionize = CoreCodeCamp.Data.Sessionize;
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
      CreateMap<Sessionize.Speaker, Speaker>()
        .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
        .ForMember(d => d.ImageUrl, opt => opt.MapFrom(s => s.ProfilePicture))
        .ForMember(d => d.Title, opt => opt.MapFrom(s => s.TagLine))
        .ForMember(d => d.Name, opt => opt.MapFrom(s => s.FullName))
        //.ForMember(d => d.Blog, opt => opt.MapFrom(s => s.Links.Where(l => l.LinkType == "Blog").FirstOrDefault()))
        .ForAllOtherMembers(opt => opt.Ignore());
    }
  }
}
