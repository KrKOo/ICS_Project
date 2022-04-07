using CarPool.DAL;
using Microsoft.EntityFrameworkCore;

namespace CarPool.Common.Tests.Factories;

public class DbContextTestingInMemoryFactory : IDbContextFactory<CarPoolDbContext>
{
	private readonly string _databaseName;
	private readonly bool _seedTestingData;

	public DbContextTestingInMemoryFactory(string databaseName, bool seedTestingData = false)
	{
		_databaseName = databaseName;
		_seedTestingData = seedTestingData;
	}

	public CarPoolDbContext CreateDbContext()
	{
		DbContextOptionsBuilder<CarPoolDbContext> contextOptionsBuilder = new();
		contextOptionsBuilder.UseInMemoryDatabase(_databaseName);

		// contextOptionsBuilder.LogTo(System.Console.WriteLine); //Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
		// builder.EnableSensitiveDataLogging();

		return new CarPoolTestingDbContext(contextOptionsBuilder.Options, _seedTestingData);
	}
}