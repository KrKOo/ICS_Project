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
    public ICollection<RideEntity> Rides { get; init; } = new List<RideEntity>();

    public UserEntity? Owner { get; init; }
}