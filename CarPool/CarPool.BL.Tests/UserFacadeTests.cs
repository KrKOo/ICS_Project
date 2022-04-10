using System;
using System.Linq;
using System.Threading.Tasks;
using CarPool.BL.Facades;
using CarPool.BL.Models;
using CarPool.Common.Enums;
using CarPool.Common.Tests;
using CarPool.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace CarPool.BL.Tests
{
    public class UserFacadeTests : CRUDFacadeTestsBase
    {
        private readonly UserFacade _facadeSUT;

        [Fact]
        public async Task Create_WithoutCarRides_DoesNotThrowAndEqualsCreated()
        {
            //Arrange
            var model = new UserDetailModel
            (
                Email: "some_email@gmail.com",
                FirstName: "Some",
                LastName: "Email",
                PhoneNumber: "+420 578 765 898",
                DateOfBirth: new DateOnly(1986, 5, 28)
            );

            //Act
            var returnedModel = await _facadeSUT.SaveAsync(model);

            //Assert
            FixIds(model, returnedModel);
            DeepAssert.Equal(model, returnedModel);
        }

    }

    [Fact]
    public async Task GetById_FromSeeded_DoesNotThrowAndEqualsSeeded()
    {
        //Arrange
        var detailModel = Mapper.Map<UserDetailModel>(UserSeeds.UserEntity);

        //Act
        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);

        //Assert
        DeepAssert.Equal(detailModel, returnedModel);
    }

    [Fact]
    public async Task GetAll_FromSeeded_DoesNotThrowAndContainsSeeded()
    {
        //Arrange
        var listModel = Mapper.Map<UserListModel>(UserSeeds.UserEntity);

        //Act
        var returnedModel = await _facadeSUT.GetAsync();

        //Assert
        Assert.Contains(listModel, returnedModel);
    }

    [Fact]
    public async Task Update_FromSeeded_DoesNotThrow()
    {
        //Arrange
        var detailModel = Mapper.Map<UserDetailModel>(UserSeeds.UserEntity);
        detailModel.LastName = "Oneil";

        //Act & Assert
        await _facadeSUT.SaveAsync(detailModel);
    }

    [Fact]
    public async Task Delete_FromSeeded_DoesNotThrow()
    {
        //Arrange
        var detailModel = Mapper.Map<UserDetailModel>(UserSeeds.UserEntity1);

        //Act & Assert
        await _facadeSUT.DeleteAsync(detailModel);
    }

    [Fact]
    public async Task Update_Name_FromSeeded_CheckUpdated()
    {
        //Arrange
        var detailModel = Mapper.Map<UserDetailModel>(UserSeeds.UserEntity);
        detailModel.LastName = "Gogol";

        //Act
        await _facadeSUT.SaveAsync(detailModel);

        //Assert
        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equal(detailModel, returnedModel);
    }

    [Fact]
    public async Task Update_RemoveCar_FromSeeded_CheckUpdated()
    {
        //Arrange
        var detailModel = Mapper.Map<UserDetailModel>(UserSeeds.UserEntity);
        detailModel.Cars.Clear();

        //Act
        await _facadeSUT.SaveAsync(detailModel);

        //Assert
        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equal(detailModel, returnedModel);
    }

    [Fact]
    public async Task Update_RemoveRidesAsDriver_FromSeeded_CheckUpdated()
    {
        //Arrange
        var detailModel = Mapper.Map<UserDetailModel>(UserSeeds.UserEntity);
        detailModel.RidesAsDriver.Clear();

        //Act
        await _facadeSUT.SaveAsync(detailModel);

        //Assert
        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equal(detailModel, returnedModel);
    }

    [Fact]
    public async Task Update_RemoveRidesAsPassenger_FromSeeded_CheckUpdated()
    {
        //Arrange
        var detailModel = Mapper.Map<UserDetailModel>(UserSeeds.UserEntity);
        detailModel.RidesAsPassenger.Clear();

        //Act
        await _facadeSUT.SaveAsync(detailModel);

        //Assert
        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equal(detailModel, returnedModel);
    }

    [Fact]
    public async Task Update_RemoveOneOfCars_FromSeeded_CheckUpdated()
    {
        //Arrange
        var detailModel = Mapper.Map<UserDetailModel>(UserSeeds.UserEntity);
        detailModel.Cars.Remove(detailModel.Cars.First());

        //Act
        await _facadeSUT.SaveAsync(detailModel);

        //Assert
        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equal(detailModel, returnedModel);
    }

    [Fact]
    public async Task Update_RemoveOneOfRidesAsDriver_FromSeeded_CheckUpdated()
    {
        //Arrange
        var detailModel = Mapper.Map<UserDetailModel>(UserSeeds.UserEntity);
        detailModel.RidesAsDriver.Remove(detailModel.RidesAsDriver.First());

        //Act
        await _facadeSUT.SaveAsync(detailModel);

        //Assert
        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equal(detailModel, returnedModel);
    }

    [Fact]
    public async Task Update_RemoveOneOfRidesAsPassenger_FromSeeded_CheckUpdated()
    {
        //Arrange
        var detailModel = Mapper.Map<UserDetailModel>(UserSeeds.UserEntity2);
        detailModel.RidesAsPassenger.Remove(detailModel.RidesAsPassenger.First());

        //Act
        await _facadeSUT.SaveAsync(detailModel);

        //Assert
        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equal(detailModel, returnedModel);
    }

    [Fact]
    public async Task DeleteById_FromSeeded_DoesNotThrow()
    {
        //Arrange & Act & Assert
        await _facadeSUT.DeleteAsync(UserSeeds.UserEntity.Id);
    }


    private static void FixIds(UserDetailModel expectedModel, UserDetailModel returnedModel)
    {
        returnedModel.Id = expectedModel.Id;

        foreach (var CarModel in returnedModel.Cars)
        {
            var CarListModel = expectedModel.Cars.FirstOrDefault(i =>
                i.LicensePlate == CarModel.LicensePlate
                && i.ImageUrl == CarModel.ImageUrl);

            if (CarListModel != null)
            {
                CarModel.Id = CarListModel.Id;
            }
        }

        foreach (var RideAsDriverModel in returnedModel.RideAsDriver)
        {
            var RideAsDriverBasicModel = expectedModel.RideAsDriver.FirstOrDefault(i =>
                i.TimeOfStart == RideAsDriverDetailModel.TimeOfStart
                && i.Duration == RideAsDriverDetailModel.Duration
                && i.RideOrigin == RideAsDriverDetailModel.RideOrigin
                && i.RideDestination == RideAsDriverDetailModel.RideDestination);

            if (RideAsDriverDetailModel != null)
            {
                RideAsDriverModel.Id = RideAsDriverBasicModel.Id;
            }
        }

        foreach (var RideAsPassenger in returnedModel.RideAsPassenger)
        {
            var RideAsPassengerBasicModel = expectedModel.RideAsPassenger.FirstOrDefault(i =>
                i.TimeOfStart == RideAsPassengerBasicModel.TimeOfStart
                && i.Duration == RideAsPassengerBasicModel.Duration
                && i.RideOrigin == RideAsPassengerBasicModel.RideOrigin
                && i.RideDestination == RideAsPassengerBasicModel.RideDestination);

            if (RideAsPassengerBasicModel != null)
            {
                RideAsPassenger.Id = RideAsPassengerBasicModel.Id;
            }
        }
    }
}