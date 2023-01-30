using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rpg_trial.Dtos.Fight;

namespace rpg_trial.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;
        public FightService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto skill)
        {
            var response = new ServiceResponse<AttackResultDto>();
           try
            {
                var attacker = await _context.Characters
                    .Include(c=>c.Skills)
                    .FirstOrDefaultAsync(x=>x.Id == skill.AttackerId);

                var opponent = await _context.Characters
                    .FirstOrDefaultAsync(x =>x.Id == skill.OpponentId);

                if (attacker is null || opponent is null || attacker.Skills is null)
                    throw new Exception("Something went wrong. . . ");

                var skill2Use = attacker.Skills.FirstOrDefault(s => s.Id == skill.SkillId);
                if(skill2Use is null)
                {
                    response.Sucess = false;
                    response.Message = $"{attacker.Name} doesn't have that skill . . .";
                    return response;
                }


                int damage = skill2Use.Damage + (new Random().Next(attacker.Intelligence));
                damage -= new Random().Next(opponent.Defense);

                if (damage > 0)
                    opponent.HitPoints -= damage;

                if (opponent.HitPoints <= 0)
                    response.Message = $"{opponent.Name} has been defeated";

                await _context.SaveChangesAsync();

                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.HitPoints,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };
            }
            catch (System.Exception ex)
            {
                response.Sucess = false;
                response.Message = ex.Message;
            }
            return response; 
        }

        public async  Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto weapon)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters
                    .Include(c=>c.Weapon)
                    .FirstOrDefaultAsync(x=>x.Id == weapon.AttackerId);

                var opponent = await _context.Characters
                    .FirstOrDefaultAsync(x =>x.Id == weapon.OpponentId);

                if (attacker is null || opponent is null || attacker.Weapon is null)
                    throw new Exception("Something went wrong. . . ");

                int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
                damage -= new Random().Next(opponent.Defense);

                if (damage > 0)
                    opponent.HitPoints -= damage;

                if (opponent.HitPoints <= 0)
                    response.Message = $"{opponent.Name} has been defeated";

                await _context.SaveChangesAsync();

                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.HitPoints,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };
            }
            catch (System.Exception ex)
            {
                response.Sucess = false;
                response.Message = ex.Message;
            }
            return response; 
        }
    }
}