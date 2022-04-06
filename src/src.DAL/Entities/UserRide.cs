﻿namespace src.DAL.Entities;

public record UserRide(
    Guid Id,
    Guid UserId,
    Guid RideId
) : IEntity
{
    public UserEntity? User { get; init; }
    public RideEntity? Ride { get; init; }
}