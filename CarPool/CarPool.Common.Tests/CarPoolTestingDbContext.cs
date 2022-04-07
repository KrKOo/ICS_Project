using CarPool.Common.Tests.Seeds;
using CarPool.DAL;
using Microsoft.EntityFrameworkCore;

namespace CarPool.Common.Tests
{
	public class CarPoolTestingDbContext : CarPoolDbContext
	{
		private readonly bool _seedTestingData;

		public CarPoolTestingDbContext(DbContextOptions contextOptions, bool seedTestingData = false)
			: base(contextOptions, seedDemoData: false)
		{
			_seedTestingData = seedTestingData;
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			if (_seedTestingData)
			{
				UserSeeds.Seed(modelBuilder);
				// TODO: Add other seeds
			}
		}
	}
}
