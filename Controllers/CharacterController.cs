using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rpg_trial.Dtos.Character;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CharacterController : ControllerBase 
{
    private readonly ICharacterService _charService;

    public CharacterController(ICharacterService charService)
    {
        _charService = charService;
    }

    [HttpGet(Name="GetAllCharacters")]
    
    public async Task<ActionResult<ServiceResponse<List<ReadCharacterDto>>>> GetAll ()
    {
        return Ok(await  _charService.GetCharacters());
    }

    [HttpGet("{id}")]
    public  async Task<ActionResult<ServiceResponse<ReadCharacterDto>>> GetById(int id){
        return Ok(await _charService.GetCharacterById(id));
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<List<CreateCharacterDto>>>> Create([FromBody] CreateCharacterDto newCharacter){

        return Ok( await _charService.CreateCharacter(newCharacter));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceResponse<ReadCharacterDto>>> Update(int id, [FromBody] UpdateCharacterDto newChara)
    {
        return Ok(await _charService.UpdateCharacter(id, newChara));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ServiceResponse<ReadCharacterDto>>> Delete(int id)
        => Ok( await _charService.DeleteCharacter(id) );


    [HttpPost("Skill")]
    public async Task<ActionResult<ServiceResponse<ReadCharacterDto>>> AddCharacterSkill (CreateCharacterSkillDto characterSkill)
    {
        return Ok(await _charService.AddCharacterSkill(characterSkill));
    }
}