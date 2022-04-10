using CarPool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarPool.Common.Tests.Seeds;

public static class RideSeeds
{
    public static readonly RideEntity EmptyRide = new(
        Id: default,
        TimeOfStart: default,
        RideOrigin: default!,
        RideDestination: default!,
        Duration: default,
        Info: default,
        CarID: default,
        DriverId: default)
    {
        Driver = default,
        Car = default
    };

    public static readonly RideEntity RideEntity1 = new(
        Id: Guid.Parse(input: "E23ABB6D-3AFF-4FA6-95E2-B429BD599D08"),
        TimeOfStart: new DateTime(2022, 4, 2, 12, 30, 0),
        RideOrigin: "Ostrava",
        RideDestination: "Brno",
        Duration: new TimeSpan(1, 45, 00),
        Info: "Du dom, gdo chce svezu vas.",
        CarID: CarSeeds.CarEntity.Id,
        DriverId: CarSeeds.CarEntity.OwnerID
    )
    {
        Driver = UserSeeds.UserEntity,
        Car = CarSeeds.CarEntity
    };

    public static readonly RideEntity RideEntity2 = new(
        Id: Guid.Parse(input: "E1E35534-158A-4E7D-B084-77A70A50F65C"),
        TimeOfStart: new DateTime(2022, 3, 5, 14, 00, 0),
        RideOrigin: "Ostrava",
        RideDestination: "Praha",
        Duration: new TimeSpan(2, 55, 00),
        Info: "Miluji Rock.",
        CarID: CarSeeds.CarEntity.Id,
        DriverId: CarSeeds.CarEntity.OwnerID
    )
    {
        Driver = UserSeeds.UserEntity,
        Car = CarSeeds.CarEntity
    };

    public static readonly RideEntity RideEntityUpdate = RideEntity1 with { Id = Guid.Parse("6435BFB8-E391-48A0-9473-8A97BB558D82"), Passengers = Array.Empty<UserRideEntity>(), Driver = null, Car = null};
    public static readonly RideEntity RideEntityDelete = RideEntity1 with { Id = Guid.Parse("45D3C767-40D4-435B-B06F-89B850ECDC8D"), Passengers = Array.Empty<UserRideEntity>(), Driver = null, Car = null};

    public static readonly RideEntity RideEntityPassengersUpdate = RideEntity1 with { Id = Guid.Parse("B2EA4CD5-699D-4F83-B27F-226F27477008"), Passengers = Array.Empty<UserRideEntity>(), Driver = null, Car = null};
    public static readonly RideEntity RideEntityPassengersDelete = RideEntity1 with { Id = Guid.Parse("6F514278-91FD-4D3F-825B-DA335E14E9B8"), Passengers = Array.Empty<UserRideEntity>(), Driver = null, Car = null};

    static RideSeeds()
    {
        RideEntity1.Passengers.Add(UserRideSeeds.UserRideEntity1);
        RideEntity2.Passengers.Add(UserRideSeeds.UserRideEntity2);
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RideEntity>().HasData(
            RideEntity1 with { Passengers = Array.Empty<UserRideEntity>(), Driver = null, Car = null },
            RideEntity2 with { Passengers = Array.Empty<UserRideEntity>(), Driver = null, Car = null },
            RideEntityUpdate,
            RideEntityDelete,
            RideEntityPassengersUpdate,
            RideEntityPassengersDelete
        );
    }
}