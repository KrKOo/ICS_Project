using System;
using System.Linq;
using System.Threading.Tasks;
using CarPool.BL.Facades;
using CarPool.BL.Models;
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

		public UserFacadeTests(ITestOutputHelper output) : base(output)
		{
			_facadeSUT = new UserFacade(UnitOfWorkFactory, Mapper);
		}


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
			var detailModel = Mapper.Map<UserDetailModel>(UserSeeds.UserEntity);

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
					&& i.PhotoUrl == CarModel.PhotoUrl);

				if (CarListModel != null)
				{
					CarModel.Id = CarListModel.Id;
				}
			}

			foreach (var RidesAsDriverModel in returnedModel.RidesAsDriver)
			{
				var RidesAsDriverBasicModel = expectedModel.RidesAsDriver.FirstOrDefault(i =>
					i.TimeOfStart == RidesAsDriverModel.TimeOfStart
					&& i.Duration == RidesAsDriverModel.Duration
					&& i.RideOrigin == RidesAsDriverModel.RideOrigin
					&& i.RideDestination == RidesAsDriverModel.RideDestination);

				if (RidesAsDriverBasicModel != null)
				{
					RidesAsDriverModel.Id = RidesAsDriverBasicModel.Id;
				}
			}

			foreach (var RidesAsPassenger in returnedModel.RidesAsPassenger)
			{
				var RidesAsPassengerBasicModel = expectedModel.RidesAsPassenger.FirstOrDefault(i =>
					i.TimeOfStart == RidesAsPassenger.TimeOfStart
					&& i.Duration == RidesAsPassenger.Duration
					&& i.RideOrigin == RidesAsPassenger.RideOrigin
					&& i.RideDestination == RidesAsPassenger.RideDestination);

				if (RidesAsPassengerBasicModel != null)
				{
					RidesAsPassenger.Id = RidesAsPassengerBasicModel.Id;
				}
			}
		}
	}
}