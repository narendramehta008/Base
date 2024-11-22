using CoreLibrary.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreLibrary.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthController(IAuthRepository authRepo, IConfiguration configuration) 
    : API.Controllers.AuthController(authRepo, configuration)
{
}
