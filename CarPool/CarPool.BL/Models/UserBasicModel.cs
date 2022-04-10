using System;
using AutoMapper;
using CarPool.DAL.Entities;

namespace CarPool.BL.Models
{
	public record UserBasicModel(
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

		public class MapperProfile : Profile
		{
			public MapperProfile()
			{
				CreateMap<UserEntity, UserBasicModel>()
					.ReverseMap();
			}
		}
	}
}
