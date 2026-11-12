using System.Xml.Serialization;

namespace Lab33.DAL.Providers;

public class XmlProvider<T> : DataProvider<T> where T : class, new()
{
    private static readonly XmlSerializer Serializer = new(typeof(List<T>));

    public override string Extension => ".xml";

    public override List<T> Read(string path)
    {
        using var stream = File.OpenRead(path);
        return (List<T>)(Serializer.Deserialize(stream) ?? new List<T>());
    }

    public override void Write(string path, IEnumerable<T> items)
    {
        using var stream = File.Create(path);
        Serializer.Serialize(stream, items.ToList());
    }
}
