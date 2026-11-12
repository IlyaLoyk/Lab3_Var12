using System.Text.Json;

namespace Lab33.DAL.Providers;

public class JsonProvider<T> : DataProvider<T> where T : class, new()
{
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

    public override string Extension => ".json";

    public override List<T> Read(string path)
    {
        using var stream = File.OpenRead(path);
        return JsonSerializer.Deserialize<List<T>>(stream, Options) ?? new List<T>();
    }

    public override void Write(string path, IEnumerable<T> items)
    {
        using var stream = File.Create(path);
        JsonSerializer.Serialize(stream, items.ToList(), Options);
    }
}
