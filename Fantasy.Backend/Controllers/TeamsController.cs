using Fantasy.Backend.UnitsOfWork.Interfaces;
using Fantasy.Shared.DTOs;
using Fantasy.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Fantasy.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : GenericController<Team>
    {
        private readonly ITeamUnitOfWork _teamUnitOfWork;

        public TeamsController(IGenericUnitOfWork<Team> unitOfWork, ITeamUnitOfWork teamUnitOfWork) : base(unitOfWork)
        {
            _teamUnitOfWork = teamUnitOfWork;
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            var response = await _teamUnitOfWork.GetAsync(); ;
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("{id:int}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var response = await _teamUnitOfWork.GetAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }

        [HttpGet("combo/{countryId:int}")]
        public async Task<IActionResult> GetCombo(int countryId)
        {
            return Ok(await _teamUnitOfWork.GetComboAsync(countryId));
        }

        [HttpPost("full")]
        public async Task<IActionResult> PostAsync(TeamDTO teamDTO)
        {
            var action = await _teamUnitOfWork.AddAsync(teamDTO);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            };
            return BadRequest(action.Message);
        }

        [HttpPut("full")]
        public async Task<IActionResult> PutAsync(TeamDTO teamDTO)
        {
            var action = await _teamUnitOfWork.UpdateAsync(teamDTO);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            };
            return BadRequest(action.Message);
        }
    }
}