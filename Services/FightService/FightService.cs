using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using rpg_trial.Dtos.Fight;
using rpg_trial.Models;

namespace rpg_trial.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public FightService(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto fight)
        {
            var response = new ServiceResponse<FightResultDto>{
                Data = new FightResultDto()
            };

            try
            {
                var characters = await _context.Characters
                    .Include(x => x.Weapon)
                    .Include(x=>x.Skills)
                    .Where(c => fight.CharacterIds.Contains(c.Id))
                    .ToListAsync();

                bool defeated = false;

                while(!defeated)
                {
                    foreach (var attacker in characters)
                    {
                        var opponents = characters.Where(x => x.Id != attacker.Id).ToList();
                        var opponent = opponents[new Random().Next(opponents.Count)];

                        int damage = 0;
                        string attackUsed = string.Empty;
                        
                        bool useWeapon = new Random().Next(2) == 0;
                        if(useWeapon && attacker.Weapon is not null)
                        {
                            attackUsed = attacker.Weapon.Name;
                            damage = DoWeaponAttack(attacker, opponent);
                        }
                        else if(!useWeapon && attacker.Skills is not null )
                        {
                            var skill = attacker.Skills[new Random().Next(attacker.Skills.Count)];
                            attackUsed = skill.Name;
                            damage = DoSkillAttack(attacker, opponent, skill);
                        }
                        else
                        {
                            response.Data.Log
                            .Add($"{attacker.Name} wasn't able to attack!");
                            continue;
                        }

                        response.Data.Log
                        .Add($"{attacker.Name} attacks {opponent.Name} using {attackUsed} with {(damage >= 0 ? damage : 0)} damage");

                        if(opponent.HitPoints <= 0) 
                        {
                            defeated = true;
                            attacker.Victories ++;
                            opponent.Defeats ++;
                            response.Data.Log.Add($"{opponent.Name} was defeated");
                            response.Data.Log.Add($"{attacker.Name} wins with {attacker.HitPoints} remaining!");
                            break;
                        }

                    }
                }
                characters.ForEach(x => {
                    x.Fights ++;
                    x.HitPoints = 100;
                });

                await _context.SaveChangesAsync();

            }
            catch (System.Exception ex)
            {
                
                response.Sucess = false;
                response.Message = ex.Message;
            }

            return response;

        }

        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto skill)
        {
            var response = new ServiceResponse<AttackResultDto>();
           try
            {
                var attacker = await _context.Characters
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(x => x.Id == skill.AttackerId);

                var opponent = await _context.Characters
                    .FirstOrDefaultAsync(x => x.Id == skill.OpponentId);

                if (attacker is null || opponent is null || attacker.Skills is null)
                    throw new Exception("Something went wrong. . . ");

                var skill2Use = attacker.Skills.FirstOrDefault(s => s.Id == skill.SkillId);
                if (skill2Use is null)
                {
                    response.Sucess = false;
                    response.Message = $"{attacker.Name} doesn't have that skill . . .";
                    return response;
                }

                int damage = DoSkillAttack(attacker, opponent, skill2Use);

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

        private static int DoSkillAttack(Character attacker, Character opponent, Skill skill2Use)
        {
            int damage = skill2Use.Damage + (new Random().Next(attacker.Intelligence));
            damage -= new Random().Next(opponent.Defense);

            if (damage > 0)
                opponent.HitPoints -= damage;
            return damage;
        }

        public async  Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto weapon)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(x => x.Id == weapon.AttackerId);

                var opponent = await _context.Characters
                    .FirstOrDefaultAsync(x => x.Id == weapon.OpponentId);

                if (attacker is null || opponent is null || attacker.Weapon is null)
                    throw new Exception("Something went wrong. . . ");
                int damage = DoWeaponAttack(attacker, opponent);

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

        private static int DoWeaponAttack(Character attacker, Character opponent)
        {
            if(attacker.Weapon is null) throw new Exception("attacker has no weapon!");
            
            int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
            damage -= new Random().Next(opponent.Defense);

            if (damage > 0)
                opponent.HitPoints -= damage;
            return damage;
        }

        public async Task<ServiceResponse<List<HighScoreDto>>> GetHighScores()
        {
            var characters = await _context.Characters
                .Where(c => c.Fights > 0)
                .OrderByDescending(c => c.Victories)
                .ThenBy(c=> c.Defeats)
                .ToListAsync();

            var response = new ServiceResponse<List<HighScoreDto>>()
            {
                Data = characters.Select(c=> _mapper.Map<HighScoreDto>(c)).ToList()
            };

            return response;

        }
    }
}