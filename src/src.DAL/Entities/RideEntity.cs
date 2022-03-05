namespace src.DAL.Entities;

public class RideEntity : BaseEntity
{
    public DateTime TimeOfStart { get; set; }
    public string RideOrigin { get; set; }
    public string RideDestination { get; set; }
    public TimeSpan Duration { get; set; }
    public string? Info { get; set; }
    public CarEntity Car { get; set; } 
    public Guid CarId { get; set; }
    public UserEntity Driver { get; set; } 
    public Guid DriverId { get; set; }
    public ICollection<UserRide> Passengers { get; set; }
}