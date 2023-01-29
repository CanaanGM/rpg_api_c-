using AutoMapper;

public class UserProfile : Profile{

    public UserProfile()
    {
        CreateMap<User, UserRegisterDto>().ReverseMap();
    }
}