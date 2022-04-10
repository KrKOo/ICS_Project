
using CarPool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarPool.Common.Tests.Seeds;

public static class UserSeeds
{
	public static readonly UserEntity EmptyUserEntity = new(
		Id: default,
		Email: default!,
		FirstName: default!,
		LastName: default!,
		PhotoUrl: default,
		PhoneNumber: default!,
		DateOfBirth: default,
		Info: default);

	public static readonly UserEntity UserEntity = new(
		Id: Guid.Parse(input: "06a8a2cf-ea03-4095-a3e4-aa0291fe9c75"),
		Email: "user1@email.com",
		FirstName: "John",
		LastName: "Doe",
		PhotoUrl: @"https://cdn.vectorstock.com/i/1000x1000/54/17/person-gray-photo-placeholder-man-vector-24005417.webp",
		PhoneNumber: "+420 123 456 789",
		DateOfBirth: new DateOnly(1980, 9, 20),
		Info: "I like long rides.");

	public static readonly UserEntity UserEntity2 = new(
		Id: Guid.Parse(input: "06a8a2cf-ea03-4095-a3e4-aa0291fe9c76"),
		Email: "user2@email.com",
		FirstName: "Thomas",
		LastName: "Eben",
		PhotoUrl: @"https://cdn.vectorstock.com/i/1000x1000/54/17/person-gray-photo-placeholder-man-vector-24005445.webp",
		PhoneNumber: "+420 725 496 785",
		DateOfBirth: new DateOnly(1995, 12, 4),
		Info: "Safety is my priority, so I am riding slow as snail.");

	//To ensure that no tests reuse these clones for non-idempotent operations
	public static readonly UserEntity UserEntityWithNothing = UserEntity with { Id = Guid.Parse("98B7F7B6-0F51-43B3-B8C0-B5FCFFF6DC2E"), Cars = Array.Empty<CarEntity>(), RidesAsDriver = Array.Empty<RideEntity>(), RidesAsPassenger = Array.Empty<UserRideEntity>() };
	public static readonly UserEntity UserEntityUpdate = UserEntity with { Id = Guid.Parse("0953F3CE-7B1A-48C1-9796-D2BAC7F67868"), Cars = Array.Empty<CarEntity>(), RidesAsDriver = Array.Empty<RideEntity>(), RidesAsPassenger = Array.Empty<UserRideEntity>() };
	public static readonly UserEntity UserEntityDelete = UserEntity with { Id = Guid.Parse("5DCA4CEA-B8A8-4C86-A0B3-FFB78FBA1A09"), Cars = Array.Empty<CarEntity>(), RidesAsDriver = Array.Empty<RideEntity>(), RidesAsPassenger = Array.Empty<UserRideEntity>() };

	public static readonly UserEntity UserCarsEntityUpdate = UserEntity with { Id = Guid.Parse("4FD824C0-A7D1-48BA-8E7C-4F136CF8BF31"), Cars = Array.Empty<CarEntity>(), RidesAsDriver = Array.Empty<RideEntity>(), RidesAsPassenger = Array.Empty<UserRideEntity>() };
	public static readonly UserEntity UserCarsEntityDelete = UserEntity with { Id = Guid.Parse("F78ED923-E094-4016-9045-3F5BB7F2EB88"), Cars = Array.Empty<CarEntity>(), RidesAsDriver = Array.Empty<RideEntity>(), RidesAsPassenger = Array.Empty<UserRideEntity>() };

	public static readonly UserEntity UserRidesAsDriverEntityUpdate = UserEntity with { Id = Guid.Parse("E0927FE8-99C8-4A88-9460-9B4300FD619A"), Cars = Array.Empty<CarEntity>(), RidesAsDriver = Array.Empty<RideEntity>(), RidesAsPassenger = Array.Empty<UserRideEntity>() };
	public static readonly UserEntity UserRidesAsDriverEntityDelete = UserEntity with { Id = Guid.Parse("D66A43B8-CBE1-4497-8551-29B89B5AD371"), Cars = Array.Empty<CarEntity>(), RidesAsDriver = Array.Empty<RideEntity>(), RidesAsPassenger = Array.Empty<UserRideEntity>() };

	public static readonly UserEntity UserRidesAsPassengerEntityUpdate = UserEntity2 with { Id = Guid.Parse("3E201B8E-7399-437F-A379-727BBCFB5E91"), Cars = Array.Empty<CarEntity>(), RidesAsDriver = Array.Empty<RideEntity>(), RidesAsPassenger = Array.Empty<UserRideEntity>() };
	public static readonly UserEntity UserRidesAsPassengerEntityDelete = UserEntity2 with { Id = Guid.Parse("AD62ED05-7859-474A-9FC9-079A936E1BE5"), Cars = Array.Empty<CarEntity>(), RidesAsDriver = Array.Empty<RideEntity>(), RidesAsPassenger = Array.Empty<UserRideEntity>() };

	static UserSeeds()
	{
		UserEntity.Cars.Add(CarSeeds.CarEntity);

		UserEntity.RidesAsDriver.Add(RideSeeds.RideEntity1);
		UserEntity2.RidesAsPassenger.Add(UserRideSeeds.UserRideEntity1);

		UserEntity.RidesAsDriver.Add(RideSeeds.RideEntity2);
		UserEntity2.RidesAsPassenger.Add(UserRideSeeds.UserRideEntity2);
	}

	public static void Seed(this ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<UserEntity>().HasData(
			UserEntity with { Cars = Array.Empty<CarEntity>(), RidesAsDriver = Array.Empty<RideEntity>(), RidesAsPassenger = Array.Empty<UserRideEntity>() },
			UserEntity2 with { Cars = Array.Empty<CarEntity>(), RidesAsDriver = Array.Empty<RideEntity>(), RidesAsPassenger = Array.Empty<UserRideEntity>() },
			UserEntityWithNothing,
			UserEntityUpdate,
			UserEntityDelete,
			UserCarsEntityUpdate,
			UserCarsEntityDelete,
			UserRidesAsDriverEntityUpdate,
			UserRidesAsDriverEntityDelete,
			UserRidesAsPassengerEntityUpdate,
			UserRidesAsPassengerEntityDelete

		);
	}
}