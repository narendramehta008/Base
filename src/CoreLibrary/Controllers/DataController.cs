using CoreLibrary.API.Application.Common.Handler;
using CoreLibrary.API.Domain.Interfaces.Repositories;
using CoreLibrary.API.Domain.Models;
using CoreLibrary.Domain.Interfaces;
using CoreLibrary.Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CoreLibrary.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DataController : ControllerBase
{

    private readonly IDbRepository _dbRepository;
    private readonly ILogger<DataController> _logger;

    public DataController(IDbRepository dbRepository, ILogger<DataController> logger)
    {
        _dbRepository = dbRepository;
        _logger = logger;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> SaveUrl(Url url)
    {
        _dbRepository.AddSave(url);
        return Ok();
    }
}
