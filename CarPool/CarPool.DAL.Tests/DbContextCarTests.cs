using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarPool.Common.Tests;
using CarPool.Common.Tests.Seeds;
using CarPool.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;
namespace CarPool.DAL.Tests;

public class DbContextCarTests : DbContextTestsBase
{
	public DbContextCarTests(ITestOutputHelper output) : base(output)
	{

	}

	[Fact]
	public async Task AddNew_Car_Persisted()
	{
		// Arrange
		var entity = CarSeeds.EmptyCarEntity with
		{
			Manufacturer = "Audi",
			Model = "AA",
			LicensePlate = "PLATE12",
			DateOfRegistration = new DateOnly(2020, 2, 2),
			PhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/db/Skoda_Fabia_II_front_20091209.jpg/1280px-Skoda_Fabia_II_front_20091209.jpg",
			NumberOfSeats = 3,
			OwnerID = UserSeeds.UserEntity.Id
		};

		// Act
		CarPoolDbContextSUT.Cars.Add(entity);
		await CarPoolDbContextSUT.SaveChangesAsync();

		// Assert
		await using var dbx = await DbContextFactory.CreateDbContextAsync();
		var actualEntity = await dbx.Cars.SingleAsync(i => i.Id == entity.Id);
		DeepAssert.Equal(entity, actualEntity);
	}

	[Fact]
	public async Task AddNew_CarWithRides_Persisted()
	{
		// Arrange
		var entity = CarSeeds.EmptyCarEntity with
		{
			Manufacturer = "VW",
			Model = "BB",
			LicensePlate = "PLATE13",
			DateOfRegistration = new DateOnly(2020, 2, 2),
			PhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/db/Skoda_Fabia_II_front_20091209.jpg/1280px-Skoda_Fabia_II_front_20091209.jpg",
			NumberOfSeats = 3,
			OwnerID = UserSeeds.UserEntity.Id,

			Rides = new List<RideEntity>
			{
				RideSeeds.EmptyRideEntity with
				{
					TimeOfStart = new DateTime(2022, 3, 2, 10, 10, 0),
					RideOrigin= "Brno",
					RideDestination= "Breclav",
					Duration= new TimeSpan(1, 10, 0),
					Info= "Some info",
					CarID = Guid.Parse(input: "210d0e64-c7a7-4227-84d4-85af8a59fec8"),
					DriverId = UserSeeds.UserEntity.Id
				}
			}
		};

		//Act
		CarPoolDbContextSUT.Cars.Add(entity);
		await CarPoolDbContextSUT.SaveChangesAsync();

		//Assert
		await using var dbx = await DbContextFactory.CreateDbContextAsync();
		var actualEntity = await dbx.Cars
			.Include(i => i.Rides)
			.SingleAsync(i => i.Id == entity.Id);
		DeepAssert.Equal(entity, actualEntity, "Passengers");
	}

	[Fact]
	public async Task GetById_Car_CarRetrieved()
	{
		//Act
		var entity = await CarPoolDbContextSUT.Cars
			.SingleAsync(i => i.Id == CarSeeds.CarEntity.Id);

		//Assert
		DeepAssert.Equal(CarSeeds.CarEntity with
		{
			Rides = Array.Empty<RideEntity>(),
			Owner = null
		}, entity);
	}

	[Fact]
	public async Task GetById_IncludingRides_Car()
	{
		//Act
		var entity = await CarPoolDbContextSUT.Cars
			.Include(i => i.Rides)
			.SingleAsync(i => i.Id == CarSeeds.CarEntity.Id);

		//Assert
		DeepAssert.Equal(CarSeeds.CarEntity, entity, "Passengers", "Driver", "Owner");
	}

	[Fact]
	public async Task Update_Car_Persisted()
	{
		//Arrange
		var baseEntity = CarSeeds.CarEntityUpdate;
		var entity =
			baseEntity with
			{
				LicensePlate = baseEntity.LicensePlate + "Updated",
			};

		//Act
		CarPoolDbContextSUT.Cars.Update(entity);
		await CarPoolDbContextSUT.SaveChangesAsync();

		//Assert
		await using var dbx = await DbContextFactory.CreateDbContextAsync();
		var actualEntity = await dbx.Cars.SingleAsync(i => i.Id == entity.Id);
		DeepAssert.Equal(entity, actualEntity);
	}

	[Fact]
	public async Task Delete_Car_CarDeleted()
	{
		//Arrange
		var entityBase = CarSeeds.CarEntityDelete;

		//Act
		CarPoolDbContextSUT.Cars.Remove(entityBase);
		await CarPoolDbContextSUT.SaveChangesAsync();

		//Assert
		Assert.False(await CarPoolDbContextSUT.Cars.AnyAsync(i => i.Id == entityBase.Id));
	}

	[Fact]
	public async Task DeleteById_Car_CarDeleted()
	{
		//Arrange
		var entityBase = CarSeeds.CarEntityDelete;

		//Act
		CarPoolDbContextSUT.Remove(await CarPoolDbContextSUT.Cars.SingleAsync(i => i.Id == entityBase.Id));
		await CarPoolDbContextSUT.SaveChangesAsync();

		//Assert
		Assert.False(await CarPoolDbContextSUT.Cars.AnyAsync(i => i.Id == entityBase.Id));
	}
}

