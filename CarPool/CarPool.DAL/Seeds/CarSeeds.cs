using CarPool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarPool.DAL.Seeds;

public static class CarSeeds
{
	public static readonly CarEntity Car1 = new(
		Id: Guid.Parse(input: "16b8f2cf-ea03-4095-a3e4-aa0291fe9cd8"),
		Manufacturer: "Honda",
        Model: "Civic",
		LicensePlate: "5A6 0505",
		DateOfRegistration: new DateOnly(2021,5,5),
		PhotoUrl: @"https://autoartmodels.de/wp-content/uploads/2020/04/73268a-scaled.jpg",
		NumberOfSeats: 4,
		OwnerID: UserSeeds.User1.Id)
	{
        Owner = UserSeeds.User1
    };


	public static readonly CarEntity Car2 = new(
		Id: Guid.Parse(input: "4ebd0208-8328-5d69-8c44-ec50939c0967"),		  
		Manufacturer: "Alfa Romeo",
        Model: "4C Spider",
		LicensePlate: "5A6 0606",
		DateOfRegistration: new DateOnly(2021,6,6),
		PhotoUrl: @"https://autoartmodels.de/wp-content/uploads/2020/04/70143a-scaled.jpg",
		NumberOfSeats: 2,
		OwnerID: UserSeeds.User2.Id)
	{
        Owner = UserSeeds.User2
    };


	public static void Seed(this ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<CarEntity>().HasData(
			Car1 with { Owner = null, Rides = Array.Empty<RideEntity>()},
			Car2 with { Owner = null, Rides = Array.Empty<RideEntity>()}
		);
	}
}