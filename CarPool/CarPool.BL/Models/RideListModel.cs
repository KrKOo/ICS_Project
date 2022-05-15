using System;
using AutoMapper;
using CarPool.DAL.Entities;

namespace CarPool.BL.Models
{
	public record RideListModel(
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

		public class MapperProfile : Profile
		{
			public MapperProfile()
			{
				CreateMap<RideEntity, RideListModel>();

				CreateMap<UserRideEntity, RideListModel>()
				.ConstructUsing(source => new RideListModel(default, default, "", ""))
				.ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Ride!.Id))
				.ForMember(dest => dest.TimeOfStart, opt => opt.MapFrom(s => s.Ride!.TimeOfStart))
				.ForMember(dest => dest.Duration, opt => opt.MapFrom(s => s.Ride!.Duration))
				.ForMember(dest => dest.RideOrigin, opt => opt.MapFrom(s => s.Ride!.RideOrigin))
				.ForMember(dest => dest.RideDestination, opt => opt.MapFrom(s => s.Ride!.RideDestination))
				.ForMember(dest => dest.Info, opt => opt.MapFrom(s => s.Ride!.Info));
			}
		}
		public static RideListModel Empty => new(default, default, string.Empty, string.Empty);
	}
}
