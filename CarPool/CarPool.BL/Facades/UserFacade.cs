using AutoMapper;
using CarPool.BL.Models;
using CarPool.DAL.Entities;
using CarPool.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CarPool.BL.Facades;

public class UserFacade : CRUDFacade<UserEntity, UserListModel, UserDetailModel>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWorkFactory _unitOfWorkFactory;

	public UserFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
	{
		_unitOfWorkFactory = unitOfWorkFactory;
		_mapper = mapper;
	}

	public async Task<UserDetailModel?> GetUserByEmailAsync(string Email = "")
	{
		await using var uow = _unitOfWorkFactory.Create();

		var query = uow
			.GetRepository<UserEntity>()
			.Get()
			.Where(e => e.Email.Equals(Email));

		return await _mapper.ProjectTo<UserDetailModel>(query).FirstOrDefaultAsync().ConfigureAwait(false);
	}
}