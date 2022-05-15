using AutoMapper;
using CarPool.BL.Models;
using CarPool.DAL.Entities;
using CarPool.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CarPool.BL.Facades;

public class RideFacade : CRUDFacade<RideEntity, RideListModel, RideDetailModel>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWorkFactory _unitOfWorkFactory;

	public RideFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
	{
		_unitOfWorkFactory = unitOfWorkFactory;
		_mapper = mapper;
	}

	public async Task<IEnumerable<RideListModel>> GetRideByFilterAsync(string Origin = "", string Destination = "", DateOnly Date = default)
	{
		await using var uow = _unitOfWorkFactory.Create();

		var query = uow
			.GetRepository<RideEntity>()
			.Get()
			.Where(e => e.RideOrigin.Contains(Origin))
			.Where(e => e.RideDestination.Contains(Destination));

        if (Date != default)
        {
			query = query.Where(e => e.TimeOfStart.Date == Date.ToDateTime(default));
		}
			

		return await _mapper.ProjectTo<RideListModel>(query).ToArrayAsync().ConfigureAwait(false);
	}

    public Task GetRideByOriginAndDestinationAsync(string v1, string v2)
    {
        throw new NotImplementedException();
    }
}