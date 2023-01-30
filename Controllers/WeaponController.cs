using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rpg_trial.Dtos.Weapons;

namespace rpg_trial.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeaponController : ControllerBase
    {
        private readonly IWeaponService _weaponService;
        public WeaponController(IWeaponService weaponService)
        {
            _weaponService = weaponService;
        }


        [HttpPost]
        public async Task<ActionResult<ServiceResponse<ReadCharacterDto>>> CreateWeapon([FromBody] AddWeaponDto weapon)
        {
            return Ok(await _weaponService.AddWeapon(weapon));
        }
    }
}