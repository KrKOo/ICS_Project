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
	public class CarFacadeTests : CRUDFacadeTestsBase
	{
		private readonly CarFacade _carFacadeSUT;

		public CarFacadeTests(ITestOutputHelper output) : base(output)
		{
			_carFacadeSUT = new CarFacade(UnitOfWorkFactory, Mapper);
		}

		[Fact]
		public async Task Create_WithNonExistingItem_DoesNotThrow()
		{
			var model = new CarDetailModel
			(
				Manufacturer: "Volkswagen",
				Model: "Passat",
				LicensePlate: "3A7 8758",
				DateOfRegistration: new DateOnly(2021, 10, 25),
				PhotoUrl: "https://www.vectorstock.com/royalty-free-vector/retro-car-vector-6547718",
				NumberOfSeats: 4
			)
			{
				Owner = Mapper.Map<UserDetailModel>(UserSeeds.UserEntity)
			};

			var _ = await _carFacadeSUT.SaveAsync(model);
		}

		[Fact]
		public async Task GetAll_Single_SeededCarEntity()
		{
			var cars = await _carFacadeSUT.GetAsync();
			var car = cars.Single(i => i.Id == CarSeeds.CarEntity.Id);

			DeepAssert.Equal(Mapper.Map<CarListModel>(CarSeeds.CarEntity), car);
		}

		[Fact]
		public async Task GetById_SeededCarEntity()
		{
			var car = await _carFacadeSUT.GetAsync(CarSeeds.CarEntity.Id);

			DeepAssert.Equal(Mapper.Map<CarDetailModel>(CarSeeds.CarEntity), car);
		}

		[Fact]
		public async Task GetById_NonExistent()
		{
			var car = await _carFacadeSUT.GetAsync(CarSeeds.EmptyCarEntity.Id);

			Assert.Null(car);
		}

		[Fact]
		public async Task SeededCarEntity_DeleteById_Deleted()
		{
			await _carFacadeSUT.DeleteAsync(CarSeeds.CarEntity.Id);

			await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
			Assert.False(await dbxAssert.Cars.AnyAsync(i => i.Id == CarSeeds.CarEntity.Id));
		}

		[Fact]
		public async Task NewCar_InsertOrUpdate_CarAdded()
		{
			//Arrange
			var car = new CarDetailModel
			(
				Manufacturer: "Volkswagen",
				Model: "Golf",
				LicensePlate: "9A3 4635",
				DateOfRegistration: new DateOnly(2021, 11, 30),
				PhotoUrl: "https://www.vectorstock.com/royalty-free-vector/retro-car-vector-6547718",
				NumberOfSeats: 3
			)
			{
				Owner = Mapper.Map<UserDetailModel>(UserSeeds.UserEntity)
			};

			//Act
			car = await _carFacadeSUT.SaveAsync(car);

			//Assert
			await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
			var carFromDb = await dbxAssert.Cars.SingleAsync(i => i.Id == car.Id);
			DeepAssert.Equal(car, Mapper.Map<CarDetailModel>(carFromDb));
		}

		[Fact]
		public async Task SeededCarEntity_InsertOrUpdate_CarUpdated()
		{
			//Arrange
			var car = new CarDetailModel
			(
				Manufacturer: CarSeeds.CarEntity.Manufacturer,
				Model: CarSeeds.CarEntity.Model,
				LicensePlate: CarSeeds.CarEntity.LicensePlate,
				DateOfRegistration: CarSeeds.CarEntity.DateOfRegistration,
				PhotoUrl: CarSeeds.CarEntity.PhotoUrl,
				NumberOfSeats: CarSeeds.CarEntity.NumberOfSeats
			)
			{
				Id = CarSeeds.CarEntity.Id,
				Owner = Mapper.Map<UserDetailModel>(UserSeeds.UserEntity)
			};
			car.LicensePlate += "NEW LCNS";
			car.NumberOfSeats += 1;

			//Act
			await _carFacadeSUT.SaveAsync(car);

			//Assert
			await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
			var carFromDb = await dbxAssert.Cars.SingleAsync(i => i.Id == car.Id);
			DeepAssert.Equal(car, Mapper.Map<CarDetailModel>(carFromDb));
		}
	}
}