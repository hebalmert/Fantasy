using Fantasy.Backend.UnitsOfWork.Interfaces;
using Fantasy.Shared.DTOs;
using Fantasy.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fantasy.Backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class TeamsController : GenericController<Team>
    {
        private readonly ITeamsUnitOfWork _teamUnitOfWork;

        public TeamsController(IGenericUnitOfWork<Team> unitOfWork, ITeamsUnitOfWork teamUnitOfWork) : base(unitOfWork)
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

        [HttpGet("paginated")]
        public override async Task<IActionResult> GetAsync(PaginationDTO pagination)
        {
            var response = await _teamUnitOfWork.GetAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalRecordsPaginated")]
        public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _teamUnitOfWork.GetTotalRecordsAsync(pagination);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
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