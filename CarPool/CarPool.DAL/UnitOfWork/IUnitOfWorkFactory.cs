namespace CarPool.DAL.UnitOfWork;

public interface IUnitOfWorkFactory
{
	IUnitOfWork Create();
}