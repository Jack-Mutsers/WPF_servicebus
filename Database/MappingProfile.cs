using AutoMapper;
using Database.Entities.DataTransferObjects;
using Database.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Player, PlayerDto>();
            CreateMap<PlayerForCreationDto, Player>();
            CreateMap<PlayerForUpdateDto, Player>();

            CreateMap<Session, SessionDto>();
            CreateMap<SessionForCreationDto, Session>();
            CreateMap<SessionForUpdateDto, Session>();
        }
    }
}
