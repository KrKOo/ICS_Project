namespace CarPool.DAL.Entities;

public record UserEntity(
	Guid Id,
	string Email,
	string FirstName,
	string LastName,
	string? PhotoUrl,
	string PhoneNumber,
	DateOnly DateOfBirth,
	string? Info
) : IEntity
{
	//Automapper requires parameter less constructor for collection synchronization for now
#nullable disable
	public UserEntity() : this(default, default, default, default, default, default, default, default) { }
#nullable enable
	public ICollection<CarEntity> Cars { get; init; } = new List<CarEntity>();
	public ICollection<RideEntity> RidesAsDriver { get; init; } = new List<RideEntity>();
	public ICollection<UserRideEntity> RidesAsPassenger { get; init; } = new List<UserRideEntity>();
}