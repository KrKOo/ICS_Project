using AutoMapper;
using CarPool.DAL.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CarPool.BL.Models
{
	public record CarListModel(string LicensePlate) : ModelBase
	{
		public string LicensePlate { get; set; } = LicensePlate;
		public string? PhotoUrl { get; set; }

		public class MapperProfile : Profile
		{
			public MapperProfile()
			{
				CreateMap<CarEntity, CarListModel>();
			}
		}
	}
}
