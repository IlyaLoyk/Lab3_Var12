using System.Text.Json;
using System.Xml.Serialization;

namespace Part1;

/// Чотири способи серіалізації колекції рахунків
public static class Serializers
{
    // 1. Бінарна
    public static void SaveBinary(string path, IEnumerable<Account> items)
    {
        var list = items.ToList();
        using var writer = new BinaryWriter(File.Create(path));
        writer.Write(list.Count);
        foreach (var a in list)
        {
            writer.Write(a.OwnerCode);
            writer.Write(a.Sum);
        }
    }

    public static List<Account> LoadBinary(string path)
    {
        using var reader = new BinaryReader(File.OpenRead(path));
        int count = reader.ReadInt32();
        var result = new List<Account>(count);
        for (int i = 0; i < count; i++)
            result.Add(new Account(reader.ReadInt32(), reader.ReadDecimal()));
        return result;
    }

    // 2. Користувацька
    public static void SaveCustom(string path, IEnumerable<Account> items) =>
        File.WriteAllLines(path, items.Select(a => a.ToCustomString()));

    public static List<Account> LoadCustom(string path) =>
        File.ReadAllLines(path)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(Account.FromCustomString)
            .ToList();

    // 3. XML
    public static void SaveXml(string path, IEnumerable<Account> items)
    {
        using var stream = File.Create(path);
        new XmlSerializer(typeof(List<Account>)).Serialize(stream, items.ToList());
    }

    public static List<Account> LoadXml(string path)
    {
        using var stream = File.OpenRead(path);
        return (List<Account>)(new XmlSerializer(typeof(List<Account>)).Deserialize(stream) ?? new List<Account>());
    }

    // 4. JSON
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public static void SaveJson(string path, IEnumerable<Account> items)
    {
        using var stream = File.Create(path);
        JsonSerializer.Serialize(stream, items.ToList(), JsonOptions);
    }

    public static List<Account> LoadJson(string path)
    {
        using var stream = File.OpenRead(path);
        return JsonSerializer.Deserialize<List<Account>>(stream, JsonOptions) ?? new List<Account>();
    }
}
