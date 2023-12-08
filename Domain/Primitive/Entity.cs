﻿namespace Domain.Primitive;

public abstract class Entity<TId>
    where TId : notnull
{
    public TId Id { get; protected set; }

    protected Entity(TId id)
    {
        Id = id;
    }
}