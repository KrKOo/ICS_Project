using CarPool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarPool.DAL.Seeds;

public static class RideSeeds
{
	public static readonly RideEntity Ride1 = new(
		Id: Guid.Parse(input: "0c3693ae-70bf-48a1-bfc4-7aa9bc42bbc4"),
		TimeOfStart: new DateTime(2022, 5, 20, 15, 30, 0),
		RideOrigin: "Brno",
		RideDestination: "Budapest",
		Duration: new TimeSpan(3, 45, 00),
		Info: "Hello, planning trip with my girlfriend but I'd like to find somebody to share a ride and split expenses for it. If you want to go the same direction, feel free to join us.",
		CarID: CarSeeds.Car1.Id,
		DriverId: UserSeeds.User1.Id)
	{
		Driver = UserSeeds.User1,
		Car = CarSeeds.Car1
	};

	public static readonly RideEntity Ride2 = new(
        Id: Guid.Parse(input: "80512dfd-597a-4ba4-9dc8-2f29543678b8"),
		TimeOfStart: new DateTime(2022, 5, 22, 18, 00, 0),
		RideOrigin: "Brno",
		RideDestination: "Praha",
		Duration: new TimeSpan(2, 30, 00),
		Info: "Going to Prague on business, and I'd be more than happy anybody to join me and accompany me since I don't like driving alone.",
		CarID: CarSeeds.Car2.Id,
		DriverId: UserSeeds.User2.Id)
	{
		Driver = UserSeeds.User2,
		Car = CarSeeds.Car2
	};

    static RideSeeds()
    {
        Ride1.Passengers.Add(UserRideSeeds.UserRide1);
        Ride2.Passengers.Add(UserRideSeeds.UserRide2);
    }

	public static void Seed(this ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<RideEntity>().HasData(
			Ride1 with { Passengers = Array.Empty<UserRideEntity>(), Driver = null, Car = null},
			Ride2 with { Passengers = Array.Empty<UserRideEntity>(), Driver = null, Car = null}
		);
	}
}