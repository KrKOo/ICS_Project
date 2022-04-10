using System;
using System.Threading.Tasks;
using CarPool.Common.Tests;
using CarPool.Common.Tests.Factories;
using CarPool.DAL.Entities;
using CarPool.DAL.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;
namespace CarPool.DAL.Tests;

public class DbContextUserTests : DbContextTestsBase
{
	public DbContextUserTests(ITestOutputHelper output) : base(output)
	{

	}

	[Fact]
	public async Task AddNew_User_Persisted()
	{
		// Arrange
        var entity = UserSeeds.EmptyUserEntity with{
			Id= Guid.Parse(input: "210d0e64-c7a7-4227-84d4-85af8a59fee9"),
			Email= "user@email.com",
			FirstName= "Chris",
			LastName= "High",
			PhotoUrl= @"https://cdn.vectorstock.com/i/1000x1000/54/17/person-gray-photo-placeholder-man-vector-24005417.webp",
			PhoneNumber= "+420 987 987 987",
			DateOfBirth= new DateOnly(2000, 5, 4),
			Info= "Sample Info"
		};

		// Act
		CarPoolDbContextSUT.Users.Add(entity);
		await CarPoolDbContextSUT.SaveChangesAsync();

		// Assert
		await using var dbx = await DbContextFactory.CreateDbContextAsync();
		var actualEntity = await dbx.Users.SingleAsync(i => i.Id == entity.Id);
		DeepAssert.Equal(entity, actualEntity);
	}

    [Fact]
    public async Task AddNew_UserWithCars_Persisted()
    {
        // Arrange
        var entity = UserSeeds.EmptyUserEntity with
        {
            Id= Guid.Parse(input: "210d0e64-c7a7-4227-84d4-85af8a59fee8"),
            Email= "user@email.com",
            FirstName= "Christopher",
            LastName= "Pratt",
            PhotoUrl= @"https://cdn.vectorstock.com/i/1000x1000/54/17/person-gray-photo-placeholder-man-vector-24005417.webp",
            PhoneNumber= "+420 987 987 000",
            DateOfBirth= new DateOnly(2000, 5, 4),
            Info= "Sample Info",
            Cars = new List<CarEntity>
            {
                CarSeeds.EmptyCarEntity with
                {
                    Manufacturer= "KIA",
                    Model= "Carens",
                    LicensePlate= "POTATO11",
                    DateOfRegistration= new DateOnly(2022, 7, 24),
                    PhotoUrl= "https://upload.wikimedia.org/wikipedia/commons/9/9c/2022_Kia_Carens_1.4_Luxury_Plus_%28India%29_front_view_01.jpg",
                    NumberOfSeats= 4
                }
            }
        };

        //Act
        CarPoolDbContextSUT.Users.Add(entity);
        await CarPoolDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Users
            .Include(i => i.Cars)
            .SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task AddNew_UserWithRideAsDriver_Persisted()
    {
        // Arrange
        var entity = UserSeeds.EmptyUserEntity with
        {
            Id= Guid.Parse(input: "210d0e64-c7a7-4227-84d4-85af8a59fee7"),
            Email= "user@email.com",
            FirstName= "Peon",
            LastName= "Warcraft",
            PhotoUrl= @"https://cdn.vectorstock.com/i/1000x1000/54/17/person-gray-photo-placeholder-man-vector-24005417.webp",
            PhoneNumber= "+420 987 567 000",
            DateOfBirth= new DateOnly(1999, 5, 4),
            Info= "Sample Info",
            RidesAsDriver = new List<RideEntity>
            {
                RSeeds.EmptyCarEntity with
                {
                    TimeOfStart = new DateTime(2022,7,5,12,12,0),
                    RideOrigin= "Bratislava",
                    RideDestination= "Praha",
                    Duration= new TimeSpan(3,20,0),
                    Info= "Some info"
                }
            }
        };

        //Act
        CarPoolDbContextSUT.Users.Add(entity);
        await CarPoolDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Users
            .Include(i => i.Rides)
            .SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task AddNew_UserWithRidesAsPassenger_Persisted()
    {
        // Arrange
        var entity = UserSeeds.EmptyUserEntity with 
        {
            Id= Guid.Parse(input: "210d0e64-c7a7-4227-84d4-85af8a59fee6"),
            Email= "user@email.com",
            FirstName= "Andrew",
            LastName= "Rider",
            PhotoUrl= @"https://cdn.vectorstock.com/i/1000x1000/54/17/person-gray-photo-placeholder-man-vector-24005417.webp",
            PhoneNumber= "+420 987 000 987",
            DateOfBirth= new DateOnly(2000, 5, 4),
            Info= "Sample Info",
            RideAsPassenger= new List<UserRideEntity>
            {
                UserRideSeeds.EmptyUserRideEntity with
                {
                    Ride = RideSeeds.EmptyRide with
                    {
                        TimeOfStart = new DateTime(2022,7,5,12,12,0),
                        RideOrigin= "Brno",
                        RideDestination= "Praha",
                        Duration= new TimeSpan(3,20,0),
                        Info= "Some info"
                    }
                }
            }
        };

        //Act
        CarPoolDbContextSUT.Users.Add(entity);
        await CarPoolDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Users
            .Include(i => i.Rides)
            .ThenInclude(i => i.Ride)
            .SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task AddNew_UserWithJustUserRides_Persisted()
    {
        // Arrange
        var entity = UserSeeds.EmptyUserEntity with 
        {
            Id= Guid.Parse(input: "210d0e64-c7a7-4227-84d4-85af8a59fee4"),
            Email= "user@email.com",
            FirstName= "Andrew",
            LastName= "Rider",
            PhotoUrl= @"https://cdn.vectorstock.com/i/1000x1000/54/17/person-gray-photo-placeholder-man-vector-24005417.webp",
            PhoneNumber= "+420 987 000 987",
            DateOfBirth= new DateOnly(2000, 5, 4),
            Info= "Sample Info",
            RideAsPassenger= new List<UserRideEntity>
            {
                UserRideSeeds.EmptyUserRideEntity with
                {
                    Ride = RideSeeds.EmptyRide with
                    {
                        RideId = RideSeeds.Ride1.Id
                    }
                }
            }
        };

            //Act
        CarPoolDbContextSUT.Users.Add(entity);
        await CarPoolDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Users
            .Include(i => i.Users)
            .SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

	[Fact]
    public async Task GetAll_Users_ContainsSeededUser()
    {
        //Act
        var entities = await CarPoolDbContextSUT.Users.ToArrayAsync();

        //Assert
        Assert.Contains(UserSeeds.User, entities);
    }

    [Fact]
    public async Task GetById_User_UserRetrieved()
    {
        //Act
        var entity = await CarPoolDbContextSUT.Users.SingleAsync(i=>i.Id == UserSeeds.User.Id);

        //Assert
        DeepAssert.Equal(UserSeeds.UserEntity, entity);
    }

    [Fact]
    public async Task GetById_IncludingCars_User()
    {
        //Act
        var entity = await CarPoolDbContextSUT.
            .Include(i=>i.Cars)
            .SingleAsync(i => i.Id == UserSeeds.UserEntity.Id);

        //Assert
        DeepAssert.Equal(UserSeeds.UserEntity, entity);
    }

    [Fact]
    public async Task GetById_IncludingRidesAsDriver_User()
    {
        //Act
        var entity = await CarPoolDbContextSUT.
            .Include(i=>i.RidesAsDriver)
            .SingleAsync(i => i.Id == UserSeeds.UserEntity.Id);

        //Assert
        DeepAssert.Equal(UserSeeds.UserEntity, entity);
    }

    [Fact]
    public async Task GetById_IncludingRidesAsPassenger_User()
    {
        //Act
        var entity = await CarPoolDbContextSUT.
            .Include(i=>i.RideAsPassenger)
            .ThenInclude(i => i.Ride)
            .SingleAsync(i => i.Id == UserSeeds.UserEntity.Id);

        //Assert
        DeepAssert.Equal(UserSeeds.UserEntity, entity);
    }

    [Fact]
    public async Task Update_User_Persisted()
    {
        //Arrange
        var baseEntity = UserSeeds.UserUpdate;
        var entity = baseEntity with
        {
            FirstName = baseEntity + "Updated",
        };

        //Act
        CarPoolDbContextSUT.Users.Update(entity);
        await CarPoolDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Users.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task Delete_User_UserDeleted()
    {
        //Arrange
        var entityBase = UserSeeds.UserDelete;

        //Act
        CarPoolDbContextSUT.Users.Remove(entityBase);
        await CarPoolDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await CarPoolDbContextSUT.Users.AnyAsync(i => i.Id == entityBase.Id));
    }

    [Fact]
    public async Task DeleteById_User_UserDeleted()
    {
        //Arrange
        var entityBase = UserSeeds.UserDelete;
        
        //Act
        CarPoolDbContextSUT.Remove(CarPoolDbContextSUT.Users.Single(i => i.Id == entityBase.Id));
        await CarPoolDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await CarPoolDbContextSUT.Users.AnyAsync(i => i.Id == entityBase.Id));
    }
}