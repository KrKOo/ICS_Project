using CarPool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarPool.Common.Tests.Seeds;

public static class UserRideSeeds
{
	public static readonly UserRideEntity EmptyUserRideEntity = new(
		Id: default,
		UserId: default,
		RideId: default
	)
	{
		User = default,
		Ride = default
	};

	public static readonly UserRideEntity UserRideEntity1 = new(
		Id: Guid.Parse(input: "0F8264D4-0EA1-435A-95EF-F0E5C376A6B2"),
		UserId: UserSeeds.UserEntity2.Id,
		RideId: RideSeeds.RideEntity1.Id
	)
	{
		User = UserSeeds.UserEntity2,
		Ride = RideSeeds.RideEntity1
	};

	public static readonly UserRideEntity UserRideEntity2 = new(
		Id: Guid.Parse(input: "A302CC21-F393-4029-820F-2245DCBCF97A"),
		UserId: UserSeeds.UserEntity2.Id,
		RideId: RideSeeds.RideEntity2.Id
	)
	{
		User = UserSeeds.UserEntity2,
		Ride = RideSeeds.RideEntity2
	};

	public static readonly UserRideEntity UserRideEntityUpdate = UserRideEntity1 with { Id = Guid.Parse("98243195-CDCA-476C-9204-D7C348E2FB1E"), User = null, Ride = null };
	public static readonly UserRideEntity UserRideEntityDelete = UserRideEntity1 with { Id = Guid.Parse("1924A46D-3F5E-4E1E-B8F5-A548EECBA03D"), User = null, Ride = null };

	public static void Seed(this ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<UserRideEntity>().HasData(
			UserRideEntity1 with { User = null, Ride = null },
			UserRideEntity2 with { User = null, Ride = null },
			UserRideEntityUpdate,
			UserRideEntityDelete
		);
	}

}