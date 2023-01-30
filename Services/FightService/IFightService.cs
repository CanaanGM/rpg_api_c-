using System.Collections.Generic;
using System.Threading.Tasks;
using rpg_trial.Dtos.Fight;

public interface IFightService
{
    Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto weapon);
    Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto weapon);
    Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto fight);  
    Task<ServiceResponse<List<HighScoreDto>>> GetHighScores();  


}