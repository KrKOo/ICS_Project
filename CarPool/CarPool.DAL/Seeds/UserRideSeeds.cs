using CarPool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarPool.DAL.Seeds;

public static class UserRideSeeds
{
	public static readonly UserRideEntity UserRide1 = new(
        Id: Guid.Parse(input: "798d4338-6051-4c32-b6a5-9022bd885877"),
		UserId: UserSeeds.User2.Id,
		RideId: RideSeeds.Ride1.Id)
	{
		User = UserSeeds.User2,
		Ride = RideSeeds.Ride1
	};

	public static readonly UserRideEntity UserRide2 = new(
	    Id: Guid.Parse(input: "f59c0b78-7319-4722-b75a-80fa0f3e7326"),
		UserId: UserSeeds.User1.Id,
		RideId: RideSeeds.Ride2.Id)
	{
		User = UserSeeds.User1,
		Ride = RideSeeds.Ride2
	};


	public static void Seed(this ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<UserRideEntity>().HasData(
			UserRide1 with { User = null, Ride = null},
			UserRide2 with { User = null, Ride = null}
		);
	}
}