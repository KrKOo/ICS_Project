namespace CarPool.DAL;

using CarPool.DAL.Entities;
using CarPool.DAL.Seeds;
using Microsoft.EntityFrameworkCore;

public class CarPoolDbContext : DbContext
{
	private readonly bool _seedDemoData;

	public CarPoolDbContext(DbContextOptions contextOptions, bool seedDemoData = false)
		: base(contextOptions)
	{
		_seedDemoData = seedDemoData;
	}

	public DbSet<CarEntity> Cars => Set<CarEntity>();
	public DbSet<UserEntity> Users => Set<UserEntity>();
	public DbSet<RideEntity> Rides => Set<RideEntity>();
	public DbSet<UserRideEntity> UserRideEntities => Set<UserRideEntity>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// modelBuilder.Entity<CarEntity>()
		// 	.HasMany(i => i.Ingredients)
		// 	.WithOne(i => i.Recipe)
		// 	.OnDelete(DeleteBehavior.Cascade);

		// modelBuilder.Entity<IngredientEntity>()
		// 	.HasMany<IngredientAmountEntity>()
		// 	.WithOne(i => i.Ingredient)
		// 	.OnDelete(DeleteBehavior.Restrict);

		if (_seedDemoData)
		{
			UserSeeds.Seed(modelBuilder);
		}
	}
}