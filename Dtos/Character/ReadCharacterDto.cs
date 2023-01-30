using System.Collections.Generic;
using rpg_trial.Dtos.Weapons;

public class ReadCharacterDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "Frodo";
    public int HitPoints { get; set; } = 100;
    public int Strength { get; set; } = 10;
    public int Defense { get; set; } = 10;
    public int Intelligence { get; set; } = 10;
    public RpgClass Class { get; set; } = RpgClass.Knight;

    public ReadWeaponDto? Weapon { get; set; }
    public List<ReadSkillDto>? Skills { get; set; }
}