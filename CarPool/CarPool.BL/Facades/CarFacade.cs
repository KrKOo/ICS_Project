using AutoMapper;
using CarPool.BL.Models;
using CarPool.DAL.Entities;
using CarPool.DAL.UnitOfWork;

namespace CarPool.BL.Facades;

public class CarFacade : CRUDFacade<IngredientEntity, IngredientListModel, IngredientDetailModel>
{
	public CarFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
	{
	}
}