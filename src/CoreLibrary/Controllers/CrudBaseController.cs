using CoreLibrary.API.Domain.Entities.Base;
using CoreLibrary.Application.Interfaces;
using CoreLibrary.Application.Models;
using CoreLibrary.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreLibrary.Controllers;

[Authorize(Roles = "Admin")]
public class CrudBaseController<T> : ControllerBase, IApiController<T> where T : BaseEntity
{
    protected readonly IDbRepository _dbRepository;

    public CrudBaseController(IDbRepository repository)
    {
        _dbRepository = repository;
    }

    [HttpPost("GetAll")]
    public async Task<IActionResult> GetAll([FromBody] GetRequest<T>? request = null)
    {
        var result = await _dbRepository.GetAll(request);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _dbRepository.FirstOrDefaultAsync<T>(a => a.Id == id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(T model)
    {
        var result = await _dbRepository.AddSaveAsync(model);
        return Ok(new
        {
            result,
            model
        });
    }

    [HttpPut]
    public async Task<IActionResult> Put(T model)
    {
        var result = await _dbRepository.UpdateDetachSaveAsync(model, a => a.Id == model.Id);
        return Ok(new
        {
            result,
            model
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var model = await _dbRepository.FirstOrDefaultAsync<T>(a => a.Id == id);
        if (model == null)
            return NotFound();

        var result = await _dbRepository.DeleteSaveAsync(model);
        return Ok(new
        {
            result,
            model
        });
    }

}
