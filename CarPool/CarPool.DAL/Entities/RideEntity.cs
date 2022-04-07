namespace CarPool.DAL.Entities;

public record RideEntity(
    Guid Id,
    DateTime TimeOfStart,
    string RideOrigin,
    string RideDestination,
    TimeSpan Duration,
    string? Info,
    Guid CarID,
    Guid DriverId
) : IEntity
{
    public ICollection<UserRide> Passengers { get; init; } = new List<UserRide>();
    public UserEntity? Driver { get; init; }
    public CarEntity? Car { get; init; }
}