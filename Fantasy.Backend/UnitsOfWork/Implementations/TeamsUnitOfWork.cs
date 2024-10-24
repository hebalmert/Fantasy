using Fantasy.Backend.Repositories.Interfaces;
using Fantasy.Backend.UnitsOfWork.Interfaces;
using Fantasy.Shared.DTOs;
using Fantasy.Shared.Entities;
using Fantasy.Shared.Responses;

namespace Fantasy.Backend.UnitsOfWork.Implementations;

public class TeamsUnitOfWork : GenericUnitOfWork<Team>, ITeamUnitOfWork
{
    private readonly ITeamRepository _teamRepository;

    public TeamsUnitOfWork(IGenericRepository<Team> repository, ITeamRepository teamRepository) : base(repository)
    {
        _teamRepository = teamRepository;
    }

    public override async Task<ActionResponse<IEnumerable<Team>>> GetAsync() => await _teamRepository.GetAsync();

    public override async Task<ActionResponse<Team>> GetAsync(int id) => await _teamRepository.GetAsync(id);

    public async Task<ActionResponse<Team>> AddAsync(TeamDTO teamDTO) => await _teamRepository.AddAsync(teamDTO);

    public async Task<IEnumerable<Team>> GetComboAsync(int countryId) => await _teamRepository.GetComboAsync(countryId);

    public async Task<ActionResponse<Team>> UpdateAsync(TeamDTO teamDTO) => await _teamRepository.UpdateAsync(teamDTO);
}