namespace src.DAL.Entities;

public class UserRide 
{
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
    public Guid RideId { get; set; }
    public RideEntity Ride { get; set; }
}