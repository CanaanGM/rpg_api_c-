using AutoMapper;

public class CharacterProfile : Profile{
    public CharacterProfile()
    {
        CreateMap<Character, CreateCharacterDto>().ReverseMap();
        CreateMap<Character, ReadCharacterDto>().ReverseMap();
        CreateMap<Character, UpdateCharacterDto>().ReverseMap();
    }
}