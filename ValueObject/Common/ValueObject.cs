using System.Reflection;

namespace MrX.Web.ValueObject.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#region Base ValueObject
public abstract class ValueObject<T>
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj) =>
        obj is ValueObject<T> other &&
        GetType() == obj.GetType() &&
        GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var obj in GetEqualityComponents())
            hash.Add(obj);
        return hash.ToHashCode();
    }

    public static bool operator ==(ValueObject<T>? left, ValueObject<T>? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(ValueObject<T>? left, ValueObject<T>? right) =>
        !(left == right);
}
#endregion

#region NonNullValueObject
public abstract class NonNullValueObject<T> : ValueObject<T> { }
#endregion

#region NullValueObject
public abstract class NullValueObject<T> : ValueObject<T> where T : NullValueObject<T>
{
    public bool IsNull { get; protected set; } = false;

    public static T CreateEmpty()
    {
        var ctor = typeof(T).GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            Type.EmptyTypes,
            null
        ) ?? throw new InvalidOperationException($"{typeof(T).Name} must have a private parameterless constructor.");

        var instance = (T)ctor.Invoke(null);
        instance.IsNull = true;
        return instance;
    }
}
#endregion
