using System.Reflection;

namespace Atlas.Application.Tests.TestSupport;

internal static class EntityIdSetter
{
    public static T WithId<T>(this T entity, int id) where T : class
    {
        var property = typeof(T).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance)
            ?? throw new InvalidOperationException($"{typeof(T).Name} does not expose an Id property.");

        var setter = property.GetSetMethod(nonPublic: true)
            ?? throw new InvalidOperationException($"{typeof(T).Name}.Id has no accessible setter.");

        setter.Invoke(entity, [id]);
        return entity;
    }
}
