
using CarPool.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarPool.DAL.Seeds;

public static class UserSeeds
{
	public static readonly UserEntity User1 = new(
		Id: Guid.Parse(input: "06a8a2cf-ea03-4095-a3e4-aa0291fe9c75"),
		Email: "user1@email.com",
		FirstName: "John",
		LastName: "Doe",
		PhotoUrl: @"https://cdn.vectorstock.com/i/1000x1000/54/17/person-gray-photo-placeholder-man-vector-24005417.webp",
		PhoneNumber: "+420 123 456 789",
		DateOfBirth: new DateOnly(1980, 9, 20),
		Info: "I like long rides."
	);
	public static readonly UserEntity User2 = new(
		Id: Guid.Parse(input: "df935095-8709-4040-a2bb-b6f97cb416dc"),
		Email: "user2@email.com",
		FirstName: "Peter",
		LastName: "Smith",
		PhotoUrl: @"https://cdn.vectorstock.com/i/1000x1000/54/17/person-gray-photo-placeholder-man-vector-24005417.webp",
		PhoneNumber: "+421 987 654 321",
		DateOfBirth: new DateOnly(1995, 6, 30),
		Info: "I like enjoy meeting new people."
	);

	public static void Seed(this ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<UserEntity>().HasData(
			User1,
			User2
		);
	}
}