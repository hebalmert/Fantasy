using Fantasy.Backend.Data;
using Fantasy.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fantasy.Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly DataContext _context;

    public CountriesController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Country>>> GetAsync()
    {
        var countries = await _context.Countries.ToListAsync();

        return Ok(countries);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Country>> GetAsync(int id)
    {
        var countries = await _context.Countries.FirstOrDefaultAsync(x => x.CountryId == id);
        if (countries is null)
        {
            return BadRequest("Registro No Encontrado");
        }
        return Ok(countries);
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] Country country)
    {
        _context.Add(country);
        await _context.SaveChangesAsync();

        return Ok(country);
    }

    [HttpPut]
    public async Task<ActionResult<Country>> PutAsync([FromBody] Country country)
    {
        var upDate = await _context.Countries.FirstOrDefaultAsync(x => x.CountryId == country.CountryId);
        if (upDate == null)
        {
            return BadRequest("El Pais no puede estar Vacio");
        }
        upDate.Name = country.Name;
        _context.Update(upDate);
        await _context.SaveChangesAsync();

        return Ok(country);
    }

    [HttpDelete("{Id:int}")]
    public async Task<ActionResult> DeleteAsync(int Id)
    {
        var countries = await _context.Countries.FirstOrDefaultAsync(x => x.CountryId == Id);
        if (countries is null)
        {
            return BadRequest("Registro No Encontrado");
        }

        _context.Remove(countries);
        await _context.SaveChangesAsync();

        return Ok();
    }
}