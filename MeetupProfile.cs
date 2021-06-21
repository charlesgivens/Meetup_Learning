using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MeetupAPI.Entities;
using MeetupApi.Models;

namespace MeetupApi
{
  public class MeetupProfile : Profile
  {
    public MeetupProfile()
    {
      CreateMap<Meetup, MeetupDetailsDto>()
        .ForMember(m => m.City, map => map.MapFrom(meetup => meetup.Location.City))
        .ForMember(m => m.ZipCode, map => map.MapFrom(meetup => meetup.Location.PostCode))
        .ForMember(m => m.Street, map => map.MapFrom(meetup => meetup.Location.Street));

      CreateMap<MeetupDto, Meetup>();
      CreateMap<LectureDto, Lecture>()
        .ReverseMap();
    }
  }
}