using AutoMapper;
using CarPool.DAL.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CarPool.BL.Models
{
	public record CarListModel(string LicensePlate, int NumberOfSeats) : ModelBase
	{
		public string LicensePlate { get; set; } = LicensePlate;
		public int NumberOfSeats { get; set; } = NumberOfSeats;
		public string? PhotoUrl { get; set; }

		public class MapperProfile : Profile
		{
			public MapperProfile()
			{
				CreateMap<CarEntity, CarListModel>();
			}
		}

		public static CarListModel Empty => new(string.Empty, default);
	}
}
