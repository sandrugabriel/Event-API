using EventAPI.Models;
using EventAPI.Dto;
using AutoMapper;

namespace EventAPI.Mappings
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles()
        {


            CreateMap<CreateRequest, Event>();
        }

    }
}
