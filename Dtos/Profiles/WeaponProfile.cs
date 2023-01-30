
using AutoMapper;
using rpg_trial.Dtos.Weapons;

namespace rpg_trial.Dtos.Profiles
{
    public class WeaponProfile : Profile
    {
        public WeaponProfile()
        {
            CreateMap<Weapon, AddWeaponDto>().ReverseMap();
            CreateMap<Weapon, ReadWeaponDto>().ReverseMap();
        }
    }
}