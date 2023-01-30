using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class CharacterService : ICharacterService
{
    public CharacterService(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }
     private static List<Character> characters =
     new List<Character>()
    {
        new Character(){
            Id=1,
            Class=RpgClass.Knight, 
            Defense=30,
            HitPoints=500,
            Intelligence=13,
            Strength=21,
            Name="Sam"
        },
        new Character(){
            Id=2,
            Class=RpgClass.Mage, 
            Defense=30,
            HitPoints=500,
            Intelligence=23,
            Strength=21,
            Name="Gandalf"
        },
        new Character(){
            Id=3,
            Class=RpgClass.Cleric, 
            Defense=30,
            HitPoints=500,
            Intelligence=23,
            Strength=21,
            Name="Arwin"
        }
        };
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public async Task<ServiceResponse<List<ReadCharacterDto>>> CreateCharacter(CreateCharacterDto newCharacter)
    {   
        var serviceResponse = new ServiceResponse<List<ReadCharacterDto>>();
        var newChara = _mapper.Map<Character>(newCharacter);

        _context.Characters.Add(newChara);
        if(SaveChanges())
            serviceResponse.Data = _mapper.Map<List<ReadCharacterDto>>( _context.Characters.ToList());
        else{
            serviceResponse.Data = null;
            serviceResponse.Sucess = false;
            serviceResponse.Message = "couldn't create character . . .";
        }


        return serviceResponse;
    }

    public async Task<ServiceResponse<ReadCharacterDto>> GetCharacterById(int id)
    {
        var serviceResponse = new ServiceResponse<ReadCharacterDto>();
        var chara = await _context.Characters.FirstOrDefaultAsync(x=>x.Id == id);
        if (chara != null)
            serviceResponse.Data =_mapper.Map<ReadCharacterDto>(chara) ;
        else{
            serviceResponse.Data = null;
            serviceResponse.Sucess = false;
            serviceResponse.Message = "character not found . . .";
        }

        
        return serviceResponse ;
    }

    public async Task<ServiceResponse<List<ReadCharacterDto>>> GetCharacters(int userId)
    {
        var serviceResponse = new ServiceResponse<List<ReadCharacterDto>>();
        serviceResponse.Data = _mapper.Map<List<ReadCharacterDto>>(await _context.Characters.Where(u=> u!.Id == userId).ToListAsync());
        return serviceResponse;
    }

    public async Task<ServiceResponse<ReadCharacterDto>> UpdateCharacter(int id, UpdateCharacterDto newCharacter)
    {
        var serviceResponse = new ServiceResponse<ReadCharacterDto>();
        var chara = await _context.Characters.FirstOrDefaultAsync(x=>x.Id == id);
        
        if(chara != null)
        {
            _mapper.Map(newCharacter, chara);
            SaveChanges();
            serviceResponse.Data = _mapper.Map<ReadCharacterDto>(chara);
        }
         else{
            serviceResponse.Data = null;
            serviceResponse.Sucess = false;
            serviceResponse.Message = "character not found . . .";
        }

        return serviceResponse;
    }


    public async Task<ServiceResponse<ReadCharacterDto>> DeleteCharacter(int id){
        var serviceResponse = new ServiceResponse<ReadCharacterDto>();

        var chara2Delete = await  _context.Characters.FirstOrDefaultAsync(x=>x.Id == id);

        if( chara2Delete != null){

            _context.Characters.Remove(chara2Delete);
            SaveChanges();
        }
        else{
            serviceResponse.Data = null;
            serviceResponse.Sucess = false;
            serviceResponse.Message = $"couldn't remove character with '{id}' . . .";
        }

        return serviceResponse;
    }

 public bool SaveChanges() => (_context.SaveChanges() >= 0);


}