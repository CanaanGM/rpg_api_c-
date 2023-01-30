using AutoMapper;
using rpg_trial.Dtos.Fight;

public class CharacterProfile : Profile{
    public CharacterProfile()
    {
        CreateMap<Character, CreateCharacterDto>().ReverseMap();
        CreateMap<Character, ReadCharacterDto>().ReverseMap();
        CreateMap<Character, UpdateCharacterDto>().ReverseMap();
        CreateMap<Character, HighScoreDto>().ReverseMap();
    }
}