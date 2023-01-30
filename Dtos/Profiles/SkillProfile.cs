using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using rpg_trial.Models;

namespace rpg_trial.Dtos.Profiles
{
    public class SkillProfile : Profile
    {
        public SkillProfile()
        {
            CreateMap<Skill, ReadSkillDto>().ReverseMap();
            
        }
    }
}