using System.Globalization;

namespace Lab33.DAL.Providers;

/// <summary>
/// Користувацька серіалізація: власний текстовий формат.
/// Один об'єкт = один рядок "Властивість=значення;Властивість=значення", значення кодуються через Uri.EscapeDataString.
/// </summary>
public class CustomProvider<T> : DataProvider<T> where T : class, new()
{
    public override string Extension => ".txt";

    public override void Write(string path, IEnumerable<T> items) =>
        File.WriteAllLines(path, items.Select(Serialize));

    public override List<T> Read(string path) =>
        File.ReadAllLines(path)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(Deserialize)
            .ToList();

    private static string Serialize(T item)
    {
        var parts = new List<string>();
        foreach (var property in PropertyMap<T>.All)
        {
            var value = property.GetValue(item);
            if (value == null) continue;
            string text = Convert.ToString(value, CultureInfo.InvariantCulture) ?? "";
            parts.Add($"{property.Name}={Uri.EscapeDataString(text)}");
        }
        return string.Join(";", parts);
    }

    private static T Deserialize(string line)
    {
        var item = new T();
        foreach (var part in line.Split(';', StringSplitOptions.RemoveEmptyEntries))
        {
            int eq = part.IndexOf('=');
            if (eq < 0) throw new FormatException($"Некоректний рядок: \"{line}\"");

            string name = part.Substring(0, eq);
            var property = PropertyMap<T>.All.FirstOrDefault(p => p.Name == name)
                           ?? throw new FormatException($"Невідоме поле \"{name}\" у рядку: \"{line}\"");

            string raw = Uri.UnescapeDataString(part.Substring(eq + 1));
            property.SetValue(item, Convert.ChangeType(raw, property.PropertyType, CultureInfo.InvariantCulture));
        }
        return item;
    }
}
