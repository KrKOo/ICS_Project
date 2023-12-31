namespace CarPool.DAL;

using CarPool.DAL.Entities;
using CarPool.DAL.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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

		modelBuilder.Entity<CarEntity>(builder =>
		{
			builder.Property(x => x.DateOfRegistration).HasConversion<DateOnlyConverter>();
		});
		modelBuilder.Entity<UserEntity>(builder =>
		{
			builder.Property(x => x.DateOfBirth).HasConversion<DateOnlyConverter>();
			builder.HasMany(e => e.Cars)
				.WithOne(e => e.Owner)
				.OnDelete(DeleteBehavior.ClientCascade);
		});

		modelBuilder.Entity<UserRideEntity>(builder =>
		{
			builder.HasOne(e => e.User)
				.WithMany(e => e.RidesAsPassenger)
				.OnDelete(DeleteBehavior.ClientCascade);
		});

		if (_seedDemoData)
		{
			UserSeeds.Seed(modelBuilder);	
			CarSeeds.Seed(modelBuilder);
			RideSeeds.Seed(modelBuilder);
			UserRideSeeds.Seed(modelBuilder);
		}
	}
}

/// <summary>
/// Converts <see cref="DateOnly" /> to <see cref="DateTime"/> and vice versa.
/// </summary>
public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
	/// <summary>
	/// Creates a new instance of this converter.
	/// </summary>
	public DateOnlyConverter() : base(
			d => d.ToDateTime(TimeOnly.MinValue),
			d => DateOnly.FromDateTime(d))
	{ }
}