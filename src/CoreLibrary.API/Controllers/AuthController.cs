using CoreLibrary.API.Domain.Entities;
using CoreLibrary.API.Domain.Interfaces.Repositories;
using CoreLibrary.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CoreLibrary.API.Controllers;

public abstract class AuthController : ControllerBase
{
    protected readonly IAuthRepository<DbContext> _repo;
    protected readonly IConfiguration _configuration;

    public AuthController(IAuthRepository<DbContext> authRepo, IConfiguration configuration)
    {
        _repo = authRepo;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
    {
        userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
        if (await _repo.UserExists(userForRegisterDto.Username))
            return BadRequest("username already exists");

        var userToCreate = new User
        {
            Username = userForRegisterDto.Username
        };
        var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);
        return StatusCode(201);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
    {
        var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);
        if (userFromRepo == null)
            return Unauthorized();

        var claims = new[]
        {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name,userForLoginDto.Username),
                new Claim(ClaimTypes.Role,userFromRepo.Role.Code),
        };
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSetting:Token").Value!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = creds
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new JsonResult(new
        {
            token = tokenHandler.WriteToken(token),
            expires = new DateTimeOffset(tokenDescriptor.Expires.Value).ToUnixTimeMilliseconds(),
            userData = userFromRepo
        });
    }
}
