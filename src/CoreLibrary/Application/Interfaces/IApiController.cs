using CoreLibrary.API.Domain.Entities.Base;
using CoreLibrary.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreLibrary.Application.Interfaces;

public interface IApiController<T> where T : BaseEntity
{
    public Task<IActionResult> GetAll(GetRequest<T>? request);
    public Task<IActionResult> Get(int id);
    public Task<IActionResult> Post(T entity);
    public Task<IActionResult> Put(T entity);
    public Task<IActionResult> Delete(int id);
}