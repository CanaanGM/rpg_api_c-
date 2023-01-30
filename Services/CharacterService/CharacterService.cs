using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class CharacterService : ICharacterService
{
    public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    private int GetUserId () => int.Parse(_httpContextAccessor.HttpContext!.User
        .FindFirstValue(ClaimTypes.NameIdentifier)!);
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public async Task<ServiceResponse<List<ReadCharacterDto>>> CreateCharacter(CreateCharacterDto newCharacter)
    {   
        var serviceResponse = new ServiceResponse<List<ReadCharacterDto>>();
        var newChara = _mapper.Map<Character>(newCharacter);
        newChara.User = await _context.Users.FirstOrDefaultAsync(x => x.Id == GetUserId());


        _context.Characters.Add(newChara);
        if(SaveChanges())
            serviceResponse.Data = _mapper.Map<List<ReadCharacterDto>>( await _context.Characters.Where(u=> u.User!.Id == GetUserId()).ToListAsync());
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
        var chara = await _context.Characters
            .FirstOrDefaultAsync(x=>x.Id == id && x.User!.Id == GetUserId() );
        if (chara != null)
            serviceResponse.Data =_mapper.Map<ReadCharacterDto>(chara) ;
        else{
            serviceResponse.Data = null;
            serviceResponse.Sucess = false;
            serviceResponse.Message = "character not found . . .";
        }

        
        return serviceResponse ;
    }

    public async Task<ServiceResponse<List<ReadCharacterDto>>> GetCharacters()
    {
        var userId = GetUserId();
        var serviceResponse = new ServiceResponse<List<ReadCharacterDto>>();
        serviceResponse.Data = _mapper.Map<List<ReadCharacterDto>>(await _context.Characters.Where(u=> u.User!.Id == userId).ToListAsync());
        return serviceResponse;
    }

    public async Task<ServiceResponse<ReadCharacterDto>> UpdateCharacter(int id, UpdateCharacterDto newCharacter)
    {
        var serviceResponse = new ServiceResponse<ReadCharacterDto>();
        var chara = await _context.Characters
            .Include(c=>c.User)
            .FirstOrDefaultAsync(x=>x.Id == id  && x.User!.Id == GetUserId());
        
        if(chara != null || chara!.User!.Id == GetUserId())
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

        var chara2Delete = await  _context.Characters.FirstOrDefaultAsync(x=>x.Id == id  && x.User!.Id == GetUserId());

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