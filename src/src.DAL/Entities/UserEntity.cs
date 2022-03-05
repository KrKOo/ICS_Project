namespace src.DAL.Entities;

public class UserEntity : BaseEntity
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PhotoUrl { get; set; }
    public string PhoneNumber { get; set; }
    public DateOnly DateOfBirth { get; init; }
    public string? Info { get; set; }
    public ICollection<CarEntity> Cars { get; set; }
    public ICollection<RideEntity> RidesAsDriver { get; set; }
    public ICollection<UserRide> RidesAsPassenger { get; set; }
}