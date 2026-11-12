using System.Reflection;

namespace Lab33.DAL.Providers;

/// Спільний для бінарного та користувацького провайдерів список властивостей типу
internal static class PropertyMap<T>
{
    public static readonly PropertyInfo[] All = typeof(T)
        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Where(p => p.CanRead && p.CanWrite)
        .OrderBy(p => p.Name, StringComparer.Ordinal)
        .ToArray();
}
