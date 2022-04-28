namespace CarPool.DAL.Entities;

public record CarEntity(
	Guid Id,
	string Manufacturer,
	string Model,
	string LicensePlate,
	DateOnly DateOfRegistration,
	string PhotoUrl,
	int NumberOfSeats,
	Guid OwnerID
) : IEntity
{
	//Automapper requires parameter less constructor for collection synchronization for now
#nullable disable
	public CarEntity() : this(default, default, default, default, default, default, default, default) { }
#nullable enable

	public ICollection<RideEntity> Rides { get; init; } = new List<RideEntity>();

	public UserEntity? Owner { get; init; }
}