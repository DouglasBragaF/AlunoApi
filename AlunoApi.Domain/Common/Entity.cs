using System;

public abstract class Entity : IEquatable<Entity>
{
    public Guid Id { get; protected set; } = Guid.CreateVersion7(); // sequencial, bom para índices

    public bool Equals(Entity? other) =>
        other is not null && GetType() == other.GetType() && Id == other.Id;

    public override bool Equals(object? obj) => Equals(obj as Entity);
    public override int GetHashCode() => HashCode.Combine(GetType(), Id);

    public static bool operator ==(Entity? a, Entity? b) => a is null ? b is null : a.Equals(b);
    public static bool operator !=(Entity? a, Entity? b) => !(a == b);
}
