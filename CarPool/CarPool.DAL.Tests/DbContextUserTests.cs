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
		UserEntity user = new(
			Id: Guid.Parse(input: "210d0e64-c7a7-4227-84d4-85af8a59fee9"),
			Email: "user@email.com",
			FirstName: "Chris",
			LastName: "High",
			PhotoUrl: @"https://cdn.vectorstock.com/i/1000x1000/54/17/person-gray-photo-placeholder-man-vector-24005417.webp",
			PhoneNumber: "+420 987 987 987",
			DateOfBirth: new DateOnly(2000, 5, 4),
			Info: "Sample Info"
		);

		// Act
		CarPoolDbContextSUT.Users.Add(user);
		await CarPoolDbContextSUT.SaveChangesAsync();

		// Assert
		await using var dbx = await DbContextFactory.CreateDbContextAsync();
		var actualUser = await dbx.Users.SingleAsync(i => i.Id == user.Id);
		DeepAssert.Equal(user, actualUser);
	}

}