namespace CarPool.DAL.Entities;

public record UserRideEntity(
	Guid Id,
	Guid UserId,
	Guid RideId
) : IEntity
{
	public UserEntity? User { get; init; }
	public RideEntity? Ride { get; init; }
}