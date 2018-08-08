using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Models;
using CoreCodeCamp.Models.Admin;

namespace CoreCodeCamp.Data
{
  public class CodeCampMappingProfile : Profile
  {
    public CodeCampMappingProfile()
    {
      CreateMap<CodeCampUser, CodeCampUserViewModel>()
        .ReverseMap();

      CreateMap<Speaker, Speaker>()
        .ForMember(m => m.Id, opt => opt.Ignore())
        .ForMember(m => m.Talks, opt => opt.Ignore())
        .ForMember(m => m.Event, opt => opt.Ignore())
        .ForMember(m => m.UserName, opt => opt.Ignore());

      CreateMap<SpeakerViewModel, Speaker>()
        .ForMember(m => m.Talks, opt => opt.Ignore());
      CreateMap<Speaker, SpeakerViewModel>()
        .ForMember(m => m.Talks, opt => opt.Ignore())
        .ForMember(m => m.SpeakerLink, opt => opt.MapFrom(s => s.Event == null ? "" : $"/{s.Event.Moniker}/Speakers/{s.Name.Replace(" ", "-")}"))
        .ForMember(m => m.Email, opt => opt.MapFrom(s => s.UserName));

      CreateMap<Talk, TalkViewModel>()
        .ForMember(m => m.Room, opt => opt.MapFrom(t => t.Room.Name))
        .ForMember(m => m.Time, opt => opt.MapFrom(t => t.TimeSlot.Time))
        .ForMember(m => m.Track, opt => opt.MapFrom(t => t.Track.Name));

      CreateMap<TalkViewModel, Talk>()
        .ForMember(m => m.Room, opt => opt.Ignore())
        .ForMember(m => m.TimeSlot, opt => opt.Ignore())
        .ForMember(m => m.Track, opt => opt.Ignore());

      CreateMap<Sponsor, SponsorViewModel>().ReverseMap();

      CreateMap<Talk, FavoriteTalkViewModel>()
        .ForMember(dest => dest.Room, opt => opt.MapFrom(s => s.Room.Name))
        .ForMember(dest => dest.Time, opt => opt.MapFrom(s => s.TimeSlot.Time))
        .ForMember(dest => dest.SpeakerName, opt => opt.MapFrom(s => s.Speaker.Name))
        .ForMember(dest => dest.Title, opt => opt.MapFrom(s => s.Title))
        .ForMember(dest => dest.Abstract, opt => opt.MapFrom(s => s.Abstract));

      CreateMap<EventInfo, EventInfoViewModel>().ReverseMap();
      CreateMap<EventLocation, EventLocationViewModel>().ReverseMap();
    }

  }
}
