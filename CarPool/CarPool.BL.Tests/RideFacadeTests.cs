using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using CarPool.BL.Facades;
using CarPool.BL.Models;
using CarPool.Common.Tests;
using CarPool.Common.Tests.Factories;
using CarPool.Common.Tests.Seeds;
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
		public async Task Create_WithoutPassenger_DoesNotThrowAndEqualsCreated()
		{
			//Arrange
			var model = new RideDetailModel
			(
				TimeOfStart: new DateTime(2022, 2, 2, 14, 00, 0),
				Duration: TimeSpan.FromHours(2),
				RideOrigin: "Breclav",
				RideDestination: "Bratislava"
			)
			{
				Info = "asdfasdf",
				Driver = Mapper.Map<UserListModel>(UserSeeds.UserEntity),
				Car = Mapper.Map<CarListModel>(CarSeeds.CarEntity)
			};

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
				TimeOfStart: new DateTime(2022, 2, 2, 22, 00, 0),
				Duration: TimeSpan.FromHours(2),
				RideOrigin: "Bratislava",
				RideDestination: "Breclav"
			)
			{
				Passengers = {new UserListModel(
					default,
					default,
					default
				)}
			};

			//Act & Assert
			try
			{
				await _facadeSUT.SaveAsync(model);
			}
			catch (DbUpdateException) { }
		}

		[Fact]
		public async Task Create_WithPassenger_DoesNotThrowAndEqualsCreated()
		{
			//Arrange
			var model = new RideDetailModel
			(
				TimeOfStart: new DateTime(2022, 2, 2, 22, 00, 0),
				Duration: TimeSpan.FromHours(2),
				RideOrigin: "Bratislava",
				RideDestination: "Breclav"
			)
			{
				Info = "Ked chces ist pridaj sa",
				Passengers = {
					Mapper.Map<UserListModel>(UserSeeds.UserEntity)
				},
				Driver = Mapper.Map<UserListModel>(UserSeeds.UserEntity),
				Car = Mapper.Map<CarListModel>(CarSeeds.CarEntity)
			};

			//Act
			var returnedModel = await _facadeSUT.SaveAsync(model);

			//Assert

			//Assert
			await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
			var rideFromDb = await dbxAssert.Rides.Include(i => i.Passengers)
			.ThenInclude(i => i.User).SingleAsync(i => i.Id == returnedModel.Id);
			var rideDetail = Mapper.Map<RideDetailModel>(rideFromDb);


			FixIds(model, rideDetail);
			DeepAssert.Equal(model, rideDetail, "Driver", "Car");
		}

		[Fact]
		public async Task Create_WithExistingAndNotExistingPassenger_Throws()
		{
			//Arrange
			var model = new RideDetailModel
			(
				TimeOfStart: new DateTime(2022, 2, 2, 22, 00, 0),
				Duration: TimeSpan.FromHours(2),
				RideOrigin: "Bratislava",
				RideDestination: "Breclav"
			)
			{
				Passengers = {
					Mapper.Map<UserListModel>(UserSeeds.UserEntity2 with {
						Id = Guid.Parse("91aeb75b-c918-4121-b748-d06e592cb983")
					})
				}
			};

			//Act & Assert
			await Assert.ThrowsAnyAsync<Exception>(async () =>
			{
				var returnedModel = await _facadeSUT.SaveAsync(model);
			});
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
			await _facadeSUT.DeleteAsync(RideSeeds.RideEntity1.Id);
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
				}
			}
		}
	}











}