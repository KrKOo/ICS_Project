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

	public async Task<IEnumerable<RideListModel>> GetRideByOriginAndDestinationAsync(string Origin = "", string Destination = "")
	{
		await using var uow = _unitOfWorkFactory.Create();

		var query = uow
			.GetRepository<RideEntity>()
			.Get()
			.Where(e => e.RideOrigin.Contains(Origin))
			.Where(e => e.RideDestination.Contains(Destination));

		return await _mapper.ProjectTo<RideListModel>(query).ToArrayAsync().ConfigureAwait(false);
	}
}