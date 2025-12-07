using AutoMapper;
using BirdAPI_lab4.DTOs;
using BirdAPI_lab4.Models;

namespace BirdAPI_lab4.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Bird, BirdDto>();
            CreateMap<CreateBirdDto, Bird>();
            CreateMap<UpdateBirdDto, Bird>();

            CreateMap<Egg, EggDto>();
            CreateMap<CreateEggDto, Egg>();
            CreateMap<UpdateEggDto, Egg>();
        }
    }
}