
using CarPool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarPool.Common.Tests.Seeds;

public static class CarSeeds
{
	public static readonly CarEntity EmptyCarEntity = new(
		Id: default,
		Manufacturer: default!,
		Model: default!,
		LicensePlate: default!,
		DateOfRegistration: default,
		PhotoUrl: default!,
		NumberOfSeats: default,
		OwnerID: default)
	{
		Owner = default
	};

	public static readonly CarEntity CarEntity = new(
		Id: Guid.Parse(input: "0d4fa150-ad80-4d46-a511-4c666166ec5e"),
		Manufacturer: "Skoda",
		Model: "Superb",
		LicensePlate: "1BY 1234",
		DateOfRegistration: new DateOnly(2022, 1, 1),
		PhotoUrl: "https://www.vectorstock.com/royalty-free-vector/retro-car-vector-6547718",
		NumberOfSeats: 4,
		OwnerID: UserSeeds.UserEntity.Id)
	{
		Owner = UserSeeds.UserEntity
	};


	public static readonly CarEntity CarEntityUpdate = CarEntity with { Id = Guid.Parse("A2E6849D-A158-4436-980C-7FC26B60C674"), Owner = null, Rides = Array.Empty<RideEntity>(), OwnerID = UserSeeds.UserCarsEntityUpdate.Id };
	public static readonly CarEntity CarEntityDelete = CarEntity with { Id = Guid.Parse("30872EFF-CED4-4F2B-89DB-0EE83A74D279"), Owner = null, Rides = Array.Empty<RideEntity>(), OwnerID = UserSeeds.UserCarsEntityDelete.Id };

	public static readonly CarEntity CarRideEntityUpdate = CarEntity with { Id = Guid.Parse("A2E6849D-A158-4436-980C-7FC26B60C675"), Owner = null, Rides = Array.Empty<RideEntity>(), OwnerID = UserSeeds.UserCarsEntityUpdate.Id };
	public static readonly CarEntity CarRideEntityDelete = CarEntity with { Id = Guid.Parse("30872EFF-CED4-4F2B-89DB-0EE83A74D271"), Owner = null, Rides = Array.Empty<RideEntity>(), OwnerID = UserSeeds.UserCarsEntityDelete.Id };


	static CarSeeds()
	{
		CarEntity.Rides.Add(RideSeeds.RideEntity1);
		CarEntity.Rides.Add(RideSeeds.RideEntity2);
	}

	public static void Seed(this ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<CarEntity>().HasData(
			CarEntity with { Owner = null, Rides = Array.Empty<RideEntity>() },
			CarEntityUpdate,
			CarEntityDelete,
			CarRideEntityUpdate,
			CarRideEntityDelete
		);
	}
}