using System.Threading.Tasks;
using rpg_trial.Dtos.Weapons;

public interface IWeaponService
{
    Task<ServiceResponse<ReadCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
}