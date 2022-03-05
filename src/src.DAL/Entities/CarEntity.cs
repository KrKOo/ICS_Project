namespace src.DAL.Entities;

public class CarEntity : BaseEntity
{
    public string Manufacturer { get; init; }
    public string Model { get; init; }
    public string LicencePlate { get; set; }
    public DateOnly DateOfRegistration { get; init; }
    public string PhotoUrl { get; set; }
    public int NumberOfSeats { get; set; }
    public UserEntity Owner { get; set; }
    public Guid OwnerID { get; set; }
    public ICollection<RideEntity> Rides { get; set; }
}