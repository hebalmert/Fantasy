using Fantasy.Backend.UnitsOfWork.Interfaces;
using Fantasy.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Fantasy.Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountriesController : GenericController<Country>
{
    public CountriesController(IGenericUnitOfWork<Country> unitOfWork) : base(unitOfWork)
    {
    }
}