using AutoMapper;
using CarPool.DAL.Entities;

namespace CarPool.BL.Models
{
	public record CarDetailModel(
		string Manufacturer,
		string Model,
		string LicensePlate,
		DateOnly DateOfRegistration,
		string PhotoUrl,
		int NumberOfSeats
	) : ModelBase
	{
		public string Manufacturer { get; set; } = Manufacturer;
		public string Model { get; set; } = Model;
		public string LicensePlate { get; set; } = LicensePlate;
		public DateOnly DateOfRegistration { get; set; } = DateOfRegistration;
		public string PhotoUrl { get; set; } = PhotoUrl;
		public int NumberOfSeats { get; set; } = NumberOfSeats;
		public UserListModel? Owner { get; set; }

		public class MapperProfile : Profile
		{
			public MapperProfile()
			{
				CreateMap<CarEntity, CarDetailModel>()
					.ReverseMap()
					.ForMember(entity => entity.Owner, expression => expression.Ignore());
			}
		}

		public static CarDetailModel Empty => new(string.Empty, string.Empty, string.Empty, default, string.Empty, default);

	}
}
