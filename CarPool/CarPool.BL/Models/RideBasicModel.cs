using System;
using AutoMapper;
using CarPool.DAL.Entities;

namespace CarPool.BL.Models
{
	public record RideBasicModel(
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
				CreateMap<RideEntity, RideBasicModel>()
					.ReverseMap();
			}
		}
	}
}
