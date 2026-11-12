using Lab33.DAL.Providers;

namespace Lab33.BLL;

/// Тип серіалізації, який користувач обирає на рівні PL
public enum StorageFormat
{
    Binary,
    Xml,
    Json,
    Custom
}

public static class StorageFormats
{
    public static string DefaultExtension(StorageFormat format) => format switch
    {
        StorageFormat.Binary => ".bin",
        StorageFormat.Xml => ".xml",
        StorageFormat.Json => ".json",
        StorageFormat.Custom => ".txt",
        _ => throw new ArgumentOutOfRangeException(nameof(format))
    };

    /// Фабрика: за обраним форматом повертає відповідний DataProvider
    internal static DataProvider<T> CreateProvider<T>(StorageFormat format) where T : class, new() => format switch
    {
        StorageFormat.Binary => new BinaryProvider<T>(),
        StorageFormat.Xml => new XmlProvider<T>(),
        StorageFormat.Json => new JsonProvider<T>(),
        StorageFormat.Custom => new CustomProvider<T>(),
        _ => throw new ArgumentOutOfRangeException(nameof(format))
    };
}
