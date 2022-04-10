using System;
using AutoMapper;
using CarPool.DAL.Entities;

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
		public UserListModel Driver { get; set; }
		public CarListModel Car { get; set; }
		public List<UserListModel> Passengers { get; init; } = new();

		public class MapperProfile : Profile
		{
			public MapperProfile()
			{
				CreateMap<RideEntity, RideDetailModel>()
					.ForMember(dto => dto.Passengers, opt => opt.MapFrom(x => x.Passengers.Select(y => y.User).ToList()))
					.ReverseMap();

			}
		}

		public static RideDetailModel Empty => new(default, default, string.Empty, string.Empty);
	}
}
