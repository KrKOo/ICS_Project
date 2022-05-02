using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarPool.Common.Tests;
using CarPool.Common.Tests.Seeds;
using CarPool.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace CarPool.DAL.Tests
{

	public class DbContextRideTests : DbContextTestsBase
	{
		public DbContextRideTests(ITestOutputHelper output) : base(output)
		{

		}

		[Fact]
		public async Task AddNew_RideWithoutPassengers_Persisted()
		{
			// Arrange
			var ride = RideSeeds.EmptyRideEntity with
			{
				RideOrigin = "Brno",
				RideDestination = "Praha",
				DriverId = UserSeeds.UserEntity.Id,
				CarID = CarSeeds.CarEntity.Id
			};


			// Act
			CarPoolDbContextSUT.Rides.Add(ride);
			await CarPoolDbContextSUT.SaveChangesAsync();

			// Assert
			await using var dbx = await DbContextFactory.CreateDbContextAsync();
			var actualRide = await dbx.Rides.SingleAsync(i => i.Id == ride.Id);
			DeepAssert.Equal(ride, actualRide);
		}

		[Fact]
		public async Task AddNew_RideWithPassengers_Persisted()
		{
			//Arrange
			var ride = RideSeeds.EmptyRideEntity with
			{
				RideOrigin = "Praha",
				RideDestination = "Brno",
				DriverId = UserSeeds.UserEntity.Id,
				CarID = CarSeeds.CarEntity.Id,
				Passengers = new List<UserRideEntity> {
					UserRideSeeds.EmptyUserRideEntity with
					{
						User = UserSeeds.EmptyUserEntity with
						{
							Email = "mrkvicak@gmail.com",
							FirstName = "Jozko",
							LastName = "Mrkvicka",
							PhoneNumber = "+420420420420"
						}
					},
					UserRideSeeds.EmptyUserRideEntity with
					{
						User = UserSeeds.EmptyUserEntity with
						{
							Email = "mrkvicka@gmail.com",
							FirstName = "Cecilka",
							LastName = "Mrkvickova",
							PhoneNumber = "+420042042042"
						}
					}
				}
			};

			//Act
			CarPoolDbContextSUT.Rides.Add(ride);
			await CarPoolDbContextSUT.SaveChangesAsync();

			//Assert
			await using var dbx = await DbContextFactory.CreateDbContextAsync();
			var actualRide = await dbx.Rides
				.Include(i => i.Passengers)
				.ThenInclude(i => i.User)
				.SingleAsync(i => i.Id == ride.Id);
			DeepAssert.Equal(ride, actualRide);
		}



		[Fact]
		public async Task AddNew_RideWithJustPassengers_Persisted()
		{
			//Arrange
			var ride = RideSeeds.EmptyRideEntity with
			{
				RideOrigin = "Praha",
				RideDestination = "Brno",
				DriverId = UserSeeds.UserEntity.Id,
				CarID = CarSeeds.CarEntity.Id,
				Passengers = new List<UserRideEntity> {
					UserRideSeeds.EmptyUserRideEntity with
					{
						UserId = UserSeeds.UserEntity.Id
					},
					UserRideSeeds.EmptyUserRideEntity with
					{
						UserId = UserSeeds.UserEntity2.Id
					}
				}
			};

			//Act
			CarPoolDbContextSUT.Rides.Add(ride);
			await CarPoolDbContextSUT.SaveChangesAsync();

			//Assert
			await using var dbx = await DbContextFactory.CreateDbContextAsync();
			var actualRide = await dbx.Rides
				.Include(i => i.Passengers)
				.SingleAsync(i => i.Id == ride.Id);
			DeepAssert.Equal(ride, actualRide);
		}


		[Fact]
		public async Task GetById_Ride()
		{
			//Act
			var entity = await CarPoolDbContextSUT.Rides
				.SingleAsync(i => i.Id == RideSeeds.RideEntity1.Id);

			//Assert
			DeepAssert.Equal(RideSeeds.RideEntity1 with
			{
				Car = null,
				Driver = null,
				Passengers = Array.Empty<UserRideEntity>()
			}, entity);
		}

		[Fact]
		public async Task GetById_IncludingPassengers_Ride()
		{
			//Act
			var entity = await CarPoolDbContextSUT.Rides
				.Include(i => i.Passengers)
				.ThenInclude(i => i.User)
				.SingleAsync(i => i.Id == RideSeeds.RideEntity1.Id);

			//Assert
			DeepAssert.Equal(RideSeeds.RideEntity1, entity, new string[] { "RidesAsPassenger", "Driver", "Car" });
		}

		[Fact]
		public async Task Update_Ride_Persisted()
		{
			//Arrange
			var baseEntity = RideSeeds.RideEntityUpdate;
			var entity =
				baseEntity with
				{
					TimeOfStart = baseEntity.TimeOfStart,
					RideOrigin = baseEntity.RideOrigin + "Updated",
					RideDestination = baseEntity.RideDestination + "Updated",
					Duration = default,
					Info = baseEntity.Info + "Updated"
				};

			//Act
			CarPoolDbContextSUT.Rides.Update(entity);
			await CarPoolDbContextSUT.SaveChangesAsync();

			//Assert
			await using var dbx = await DbContextFactory.CreateDbContextAsync();
			var actualEntity = await dbx.Rides.SingleAsync(i => i.Id == entity.Id);
			DeepAssert.Equal(entity, actualEntity);
		}

		[Fact]
		public async Task Delete_Ride_Deleted()
		{
			//Arrange
			var baseEntity = RideSeeds.RideEntityDelete;

			//Act
			CarPoolDbContextSUT.Rides.Remove(baseEntity);
			await CarPoolDbContextSUT.SaveChangesAsync();

			//Assert
			Assert.False(await CarPoolDbContextSUT.Rides.AnyAsync(i => i.Id == baseEntity.Id));
		}

		[Fact]
		public async Task DeleteById_Ride_Deleted()
		{
			//Arrange
			var baseEntity = RideSeeds.RideEntityDelete;

			//Act
			CarPoolDbContextSUT.Remove(await CarPoolDbContextSUT.Rides.SingleAsync(i => i.Id == baseEntity.Id));
			await CarPoolDbContextSUT.SaveChangesAsync();

			//Assert
			Assert.False(await CarPoolDbContextSUT.Rides.AnyAsync(i => i.Id == baseEntity.Id));
		}


	}
}