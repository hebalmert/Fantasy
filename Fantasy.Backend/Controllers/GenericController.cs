using Fantasy.Backend.UnitsOfWork.Interfaces;
using Fantasy.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fantasy.Backend.Controllers;

public class GenericController<T> : Controller where T : class
{
    private readonly IGenericUnitOfWork<T> _unitOfWork;

    public GenericController(IGenericUnitOfWork<T> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public virtual async Task<IActionResult> GetAsync()
    {
        var action = await _unitOfWork.GetAsync();
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest();
    }

    [HttpGet("{id:int}")]
    public virtual async Task<IActionResult> GetAsync(int id)
    {
        var action = await _unitOfWork.GetAsync(id);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return NotFound();
    }

    [HttpPost]
    public virtual async Task<IActionResult> PostAsync(T model)
    {
        var action = await _unitOfWork.AddAsync(model);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    [HttpPut]
    public virtual async Task<IActionResult> PutAsync(T model)
    {
        var action = await _unitOfWork.UpdateAsync(model);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    [HttpDelete("{Id:int}")]
    public virtual async Task<IActionResult> DeleteAsync(int Id)
    {
        var action = await _unitOfWork.DeleteAsync(Id);
        if (action.WasSuccess)
        {
            return NoContent();
        }
        return BadRequest(action.Message);
    }
}