using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarPool.Common.Tests;
using CarPool.Common.Tests.Factories;
using CarPool.Common.Tests.Seeds;
using CarPool.DAL.Entities;
using CarPool.DAL.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;


namespace CarPool.DAL.Tests
{
	public class DbContextUserRideTests : DbContextTestsBase
	{
		public DbContextUserRideTests(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public async Task GetAll_UserRides_ForRide()
		{
			//Act
			var userRides = await CarPoolDbContextSUT.UserRideEntities
				.Where(i => i.RideId == RideSeeds.RideEntity1.Id) // ?
				.ToArrayAsync();

			//Assert
			Assert.Contains(UserRideSeeds.UserRideEntity1 with { Ride = null, User = null }, userRides);
			Assert.DoesNotContain(UserRideSeeds.UserRideEntity2 with { Ride = null, User = null }, userRides);
		}

		[Fact]
		public async Task Update_Passengers_Persisted()
		{
			//Arrange
			var baseEntity = UserRideSeeds.UserRideEntityUpdate;
			var entity =
				baseEntity with
				{
					UserId = UserSeeds.UserEntity.Id,
					RideId = RideSeeds.RideEntity1.Id
				};

			//Act
			CarPoolDbContextSUT.UserRideEntities.Update(entity);
			await CarPoolDbContextSUT.SaveChangesAsync();

			//Assert
			await using var dbx = await DbContextFactory.CreateDbContextAsync();
			var actualEntity = await dbx.UserRideEntities.SingleAsync(i => i.Id == entity.Id);
			Assert.Equal(entity, actualEntity);
		}

		[Fact]
		public async Task Delete_Passengers_Deleted()
		{
			//Arrange
			var baseEntity = UserRideSeeds.UserRideEntityDelete;

			//Act
			CarPoolDbContextSUT.UserRideEntities.Remove(baseEntity);
			await CarPoolDbContextSUT.SaveChangesAsync();

			//Assert
			Assert.False(await CarPoolDbContextSUT.UserRideEntities.AnyAsync(i => i.Id == baseEntity.Id));
		}

		[Fact]
		public async Task DeleteById_Passengers_Deleted()
		{
			//Arrange
			var baseEntity = UserRideSeeds.UserRideEntityDelete;

			//Act
			CarPoolDbContextSUT.Remove(
				CarPoolDbContextSUT.UserRideEntities.Single(i => i.Id == baseEntity.Id));
			await CarPoolDbContextSUT.SaveChangesAsync();

			//Assert
			Assert.False(await CarPoolDbContextSUT.UserRideEntities.AnyAsync(i => i.Id == baseEntity.Id));

		}

	}
}
