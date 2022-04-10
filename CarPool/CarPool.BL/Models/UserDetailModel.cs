using System;
using AutoMapper;
using CarPool.DAL.Entities;

namespace CarPool.BL.Models
{
	public record UserDetailModel(
		string Email,
		string FirstName,
		string LastName,
		string PhoneNumber,
		DateOnly DateOfBirth
	) : ModelBase
	{
		public string Email { get; set; } = Email;
		public string FirstName { get; set; } = FirstName;
		public string LastName { get; set; } = LastName;
		public string PhoneNumber { get; set; } = PhoneNumber;
		public string? PhotoUrl { get; set; }
		public string? Info { get; set; }
		public List<CarListModel> Cars { get; set; } = new();
		public List<RideListModel> RidesAsPassenger { get; set; } = new();
		public List<RideListModel> RidesAsDriver { get; set; } = new();

		public class MapperProfile : Profile
		{
			public MapperProfile()
			{
				CreateMap<UserEntity, UserDetailModel>()
					.ForMember(dto => dto.RidesAsPassenger, opt => opt.MapFrom(x => x.RidesAsPassenger.Select(y => y.Ride).ToList()))
					.ReverseMap()
					.ForMember(entity => entity.RidesAsDriver, expression => expression.Ignore())
					.ForMember(entity => entity.Cars, expression => expression.Ignore());
			}
		}
	}
}
