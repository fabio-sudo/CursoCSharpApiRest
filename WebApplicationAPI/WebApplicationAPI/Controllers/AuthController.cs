using Microsoft.AspNetCore.Mvc;
using WebApplicationAPI.Model;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;

    public AuthController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost]
    public IActionResult Autenticar([FromBody] LoginModel login)
    {
        if (login.Usuario == "admin" && login.Senha == "123")
        {
            var token = _tokenService.GerarToken(login.Usuario);
            return Ok(new { token });
        }

        return Unauthorized();
    }
}
