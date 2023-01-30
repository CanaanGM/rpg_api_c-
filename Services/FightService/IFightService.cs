using System.Threading.Tasks;
using rpg_trial.Dtos.Fight;

public interface IFightService
{
    Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto weapon);
    Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto weapon);
}