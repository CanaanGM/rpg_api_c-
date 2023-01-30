using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using rpg_trial.Dtos.Weapons;

public class WeaponService : IWeaponService
{
    private DataContext _context;
    private IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

    public WeaponService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse<ReadCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
    {
        var response = new ServiceResponse<ReadCharacterDto>();
        try
        {
            var chara = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId
                && c.User!.Id == int.Parse(_httpContextAccessor.HttpContext!.User
                    .FindFirstValue(ClaimTypes.NameIdentifier)!));

            if (chara is null)
            {
                response.Sucess = false;
                response.Message = "Character not found . . .";
                return response;
            }

            var weapon = _mapper.Map<Weapon>(newWeapon);
            weapon.CharacterId = chara.Id;
            weapon.Character = chara;

            _context.Weapons.Add(weapon);
            await _context.SaveChangesAsync();
            response.Data = _mapper.Map<ReadCharacterDto>(chara);

        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
            response.Sucess = false;
        }

        return response;
    }
}