using System;
using AutoMapper;
using CarPool.DAL.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CarPool.BL.Models
{
	public record RideDetailModel(
		DateTime TimeOfStart,
		TimeSpan Duration,
		string RideOrigin,
		string RideDestination
	) : ModelBase
	{
		public DateTime TimeOfStart { get; set; } = TimeOfStart;
		public TimeSpan Duration { get; set; } = Duration;
		public string RideOrigin { get; set; } = RideOrigin;
		public string RideDestination { get; set; } = RideDestination;
		public string? Info { get; set; }
		public UserListModel? Driver { get; set; }
		public CarListModel? Car { get; set; }
		public List<UserListModel> Passengers { get; init; } = new();

		public class MapperProfile : Profile
		{
			public MapperProfile()
			{
				CreateMap<RideEntity, RideDetailModel>()
				.ForMember(dest => dest.Passengers, opt => opt.MapFrom(x => x.Passengers.Select(s => s.User)));

				CreateMap<RideDetailModel, RideEntity>()
				.ForMember(entity => entity.CarID, expression => expression.MapFrom(s => s.Car!.Id))
				.ForMember(entity => entity.DriverId, expression => expression.MapFrom(s => s.Driver!.Id))
				.ForMember(entity => entity.Car, expression => expression.Ignore())
				.ForMember(entity => entity.Driver, expression => expression.Ignore())
				.ForMember(entity => entity.Passengers, expression => expression.MapFrom(ride => ride.Passengers
					.Select(c => new UserRideEntity { UserId = c.Id, RideId = ride.Id })));
			}
		}
		public static RideDetailModel Empty => new(default, default, string.Empty, string.Empty);
	}
}
