using Fantasy.Backend.UnitsOfWork.Interfaces;
using Fantasy.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fantasy.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : GenericController<Team>
    {
        public TeamsController(IGenericUnitOfWork<Team> unitOfWork) : base(unitOfWork)
        {
        }
    }
}