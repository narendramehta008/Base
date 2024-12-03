﻿using CoreLibrary.API.Domain.Entities.Base;
using CoreLibrary.API.Domain.Extensions;
using CoreLibrary.Application.Interfaces;
using CoreLibrary.Application.Models;
using CoreLibrary.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CoreLibrary.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class DataController : ControllerBase
{

    private readonly IDbRepository _dbRepository;
    private readonly ILogger<DataController> _logger;

    public DataController(IDbRepository dbRepository, ILogger<DataController> logger)
    {
        _dbRepository = dbRepository;
        _logger = logger;
    }

    [HttpPost("GetAll")]
    public async Task<IActionResult> GetAll([FromQuery] string type, [FromBody] GetRequest<object>? request = null)
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();
        var instance = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(a => a.BaseType?.Name == "BaseEntity" && a.Name == type);
        //var baseEntity = (BaseEntity)Activator.CreateInstance(instance);
        //baseEntity.Id = id;
        var result = await _dbRepository.GetAll(request);
        return Ok(result);
    }

    [HttpGet]
    public IActionResult Get([FromQuery] string type)
    {
        var result = _dbRepository.Find(GetEntityType(type));
        return new JsonResult(result);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id, [FromQuery] string type)
    {
        var result = _dbRepository.Find(GetEntityType(type), id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(object model, [FromQuery] string type)
    {
        var result = await _dbRepository.AddSaveAsync(model);
        return Ok(new
        {
            result,
            model
        });
    }

    [HttpPut]
    public async Task<IActionResult> Put(object model)
    {
        var data = (BaseEntity)model;
        var result = await _dbRepository.UpdateDetachSaveAsync(data, a => a.Id == data.Id);
        return Ok(new
        {
            result,
            model
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, [FromQuery] string type)
    {
        var model = _dbRepository.Find(GetEntityType(type), id);
        if (model == null)
            return NotFound();

        var result = await _dbRepository.DeleteSaveAsync(model);
        return Ok(new
        {
            result,
            model
        });
    }

    private Type GetEntityType(string type)
    {
        var types = Assembly.GetExecutingAssembly().GetTypes().Where(a => a.Name == type);
        return types.First(a => a.BaseType?.Name.ContainsAny(nameof(BaseEntityData), nameof(BaseParentEntity)) ?? false);
    }


}

[Route("api/[controller]")]
[ApiController]
public class UrlController : CrudBaseController<Url>
{

    public UrlController(IDbRepository dbRepository) : base(dbRepository)
    {
    }

}

[Route("api/[controller]")]
[ApiController]
public class QueryController : CrudBaseController<Query>
{

    public QueryController(IDbRepository dbRepository) : base(dbRepository)
    {
    }

}

[Route("api/[controller]")]
[ApiController]
public class SummaryController : CrudBaseController<Summary>
{

    public SummaryController(IDbRepository dbRepository) : base(dbRepository)
    {
    }

    [HttpGet("[action]")]
    public IActionResult GetParents()
    {
        var result = _dbRepository.Where<Summary>(a => a.ParentId == null);
        return Ok(result);
    }

    [HttpGet("[action]/{id}")]
    public IActionResult GetChilds(int id)
    {
        var result = _dbRepository.Where<Summary>(a => a.Id == id).Include(a => a.Summaries);
        return Ok(result);
    }
}
