using AutoMapper;
using CarPool.BL.Models;
using CarPool.DAL.Entities;
using CarPool.DAL.UnitOfWork;

namespace CarPool.BL.Facades;

public class RideFacade : CRUDFacade<IngredientEntity, IngredientListModel, IngredientDetailModel>
{
	public RideFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
	{
	}
}