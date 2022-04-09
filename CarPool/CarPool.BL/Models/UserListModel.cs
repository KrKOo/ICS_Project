using System;
using AutoMapper;
using CarPool.DAL.Entities;

namespace CarPool.BL.Models
{
	public record UserListModel(
		string Email,
		string FirstName,
		string LastName
	) : ModelBase
	{
		public string Email { get; set; } = Email;
		public string FirstName { get; set; } = FirstName;
		public string LastName { get; set; } = LastName;
		public string? PhotoUrl { get; set; }

		public class MapperProfile : Profile
		{
			public MapperProfile()
			{
				CreateMap<UserEntity, UserListModel>();
			}
		}
	}
}
