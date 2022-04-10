using System;
using System.Threading.Tasks;
using CarPool.BL.Models;
using CookBook.BL.Facades;
using CarPool.Common.Tests;
using CarPool.Common.Tests.Seeds;
using CarPool.Common.Tests.Factories;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace CarPool.BL.Tests
{

public class RideFacadeTests : CRUDFacadeTestsBase
    {
        private readonly RideFacade _facadeSUT;

        public RideFacadeTests(ITestOutputHelper output) : base(output)
        {
            _facadeSUT = new RideFacade(UnitOfWorkFactory, Mapper);
        }

        [Fact]
        public async Task Create_WithWithoutPassenger_DoesNotThrowAndEqualsCreated()
        {
            //Arrange
            var model = new RideDetailModel
            (
                TimeOfStart:  new DateTime(2022, 2, 2, 14, 00, 0),
                Duration: TimeSpan.FromHours(2),
                RideOrigin: "Breclav",
                RideDestination: "Bratislava"
            ); 

             //Act
             var returnedModel = await _facadeSUT.SaveAsync(model);

            //Assert
            FixIds(model, returnedModel);
            DeepAssert.Equal(model, returnedModel);
        }

        [Fact]
        public async Task Create_WithNonExistingPassenger_Throws()
        {
            //Arrange
            var model = new RideDetailModel
            (
                TimeOfStart:  new DateTime(2022, 2, 2, 22, 00, 0),
                Duration: TimeSpan.FromHours(2),
                RideOrigin: "Bratislava",
                RideDestination: "Breclav"
            )
            {
                Passengers = { new UserListModel() } //make it empty?
            };

            //Act & Assert
            try
            {
                await _facadeSUT.SaveAsync(model);
            }
            catch(DbUpdateException){}
        }

        [Fact]
        public async Task Create_WithPassenger_DoesNotThrowAndEqualsCreated()
        {
            //Arrange
            var model = new RideDetailModel
            (
                TimeOfStart:  new DateTime(2022, 2, 2, 22, 00, 0),
                Duration: TimeSpan.FromHours(2),
                RideOrigin: "Bratislava",
                RideDestination: "Breclav"
            )
            {
                Info = "Ked chces ist pridaj sa",
                Passengers = {
                    new UserListModel
                    (
                        Email: "mrkvicak@gmail.com",
                        FirstName: "Jozko",
                        LastName: "Mrkvicka"
                    )
                    {
                        PhotoUrl= @"https://cdn.vectorstock.com/i/1000x1000/54/17/person-gray-photo-placeholder-man-vector-24005445.webp",
                    }
                }
            };

            //Act
            var returnedModel = await _facadeSUT.SaveAsync(model);

            //Assert
            FixIds(model, returnedModel);
            DeepAssert.Equal(model,returnedModel);
        }

        [Fact]
        public async Task Create_WithExistingAndNotExistingPassenger_Throws()
        {
            //Arrange
            var model = new RideDetailModel
            (
                TimeOfStart:  new DateTime(2022, 2, 2, 22, 00, 0),
                Duration: TimeSpan.FromHours(2),
                RideOrigin: "Bratislava",
                RideDestination: "Breclav"
            )
            {
                Passengers = {
                    new UserListModel(),  //make it empty?
            
                    Mapper.Map<UserListModel>(UserSeeds.UserEntity)
                }
            };

            //Act & Assert
            try
            {
                await _facadeSUT.SaveAsync(model);
                Assert.True(false, "Assert Fail");
            }
            catch(DbUpdateException){}
            catch(ArgumentException){}
        }

        [Fact]
        public async Task GetById_FromSeeded_DoesNotThrowAndEqualsSeeded()
        {
            //Arrange
            var detailModel = Mapper.Map<RideDetailModel>(RideSeeds.RideEntity1);

            //Act
            var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);

            //Assert
            DeepAssert.Equal(detailModel, returnedModel);
        }

        [Fact]
        public async Task GetAll_FromSeeded_DoesNotThrowAndContainsSeeded()
        {
            //Arrange
            var listModel = Mapper.Map<RideListModel>(RideSeeds.RideEntity1);

            //Act
            var returnedModel = await _facadeSUT.GetAsync();

            //Assert
            Assert.Contains(listModel, returnedModel);
        }

        [Fact]
        public async Task Delete_FromSeeded_DoesNotThrow()
        {
            //Arrange
            var detailModel = Mapper.Map<RideDetailModel>(RideSeeds.RideEntity1);

            //Act & Assert
            await _facadeSUT.DeleteAsync(detailModel);
        }

        [Fact]
        public async Task Update_FromSeeded_DoesNotThrow()
        {
            //Arrange
            var detailModel = Mapper.Map<RideDetailModel>(RideSeeds.RideEntity1);
            detailModel.RideOrigin = "Update: Brno";

            //Act & Assert
            await _facadeSUT.SaveAsync(detailModel);
        }

        [Fact]
        public async Task Update_Name_FromSeeded_CheckUpdated()
        {
            //Arrange
            var detailModel = Mapper.Map<RideDetailModel>(RideSeeds.RideEntity1);
             detailModel.RideOrigin = "Update: Brno";

            //Act
            await _facadeSUT.SaveAsync(detailModel);

            //Assert
            var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);
            DeepAssert.Equal(detailModel, returnedModel);
        }

        [Fact]
        public async Task Update_RemovePassengers_FromSeeded_CheckUpdated()
        {
            //Arrange
            var detailModel = Mapper.Map<RideDetailModel>(RideSeeds.RideEntity1);
            detailModel.Passengers.Clear();

            //Act
            await _facadeSUT.SaveAsync(detailModel);

            //Assert
            var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);
            DeepAssert.Equal(detailModel, returnedModel);
        }
        
        [Fact]
        public async Task Update_RemoveOneOfPassengers_FromSeeded_CheckUpdated()
        {
            //Arrange
            var detailModel = Mapper.Map<RideDetailModel>(RideSeeds.RideEntity1);
            detailModel.Passengers.Remove(detailModel.Passengers.First());

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
            await _facadeSUT.DeleteAsync(RideSeeds.RideEntity.Id);
        }

        private static void FixIds(RideDetailModel expectedModel, RideDetailModel returnedModel)
        {
            returnedModel.Id = expectedModel.Id;

            foreach (var passengerModel in returnedModel.Passengers)
            {
                var passengerDetailModel = expectedModel.Passengers.FirstOrDefault
                    (i => 
                        i.Email == passengerModel.Email 
                        && i.FirstName == passengerModel.FirstName
                        && i.LastName == passengerModel.LastName
                        && i.PhotoUrl == passengerModel.PhotoUrl
                    );

                if (passengerDetailModel != null)
                {
                    passengerModel.Id = passengerDetailModel.Id;
                   // passengerModel.PassengerId = passengerDetailModel.passengerId; // ?id for UserListModel?
                }
            }
        }
    }











}