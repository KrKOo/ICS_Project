using System;
using System.Threading.Tasks;
using CarPool.Common.Tests.Seeds;
using CarPool.Common.Tests;
using CarPool.Common.Tests.Factories;
using CarPool.DAL.Entities;
using CarPool.DAL.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace CarPool.DAL.Tests;



public class DbContextRideTests : DbContextTestsBase
{
	public DbContextRideTests(ITestOutputHelper output) : base(output)
	{

	}

	[Fact]
	public async Task AddNew_RideWithoutPassengers_Persisted()
	{
		// Arrange
		var ride = RideSeeds.EmptyRide with {
			RideOrigin = "Brno",
			RideDestination = "Praha"
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
		var ride = RideSeeds.EmptyRide with
		{
			RideOrigin = "Praha",
			RideDestination = "Brno",
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










}