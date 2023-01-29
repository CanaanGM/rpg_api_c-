using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private IAuthRepo _authRepo;

    public AuthController(IAuthRepo authRepo)
    {
        _authRepo = authRepo;
    }


    [HttpPost("Register")]
    public async Task<ActionResult<ServiceResponse<int>>> Register ([FromBody] UserRegisterDto user)
    {
        var response = await _authRepo.Register(
            new User {Username = user.Username }, user.Password
        );

        return response.Sucess ? Ok() : BadRequest(response.Message);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<ServiceResponse<string>>> Login ([FromBody] UserLoginDto user)
    {
        var response = await _authRepo.Login(
            user.Username, user.Password
        );

        return response.Sucess ? Ok(response) : BadRequest(response.Message);
    }
}