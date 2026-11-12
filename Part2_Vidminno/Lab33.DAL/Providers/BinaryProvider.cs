namespace Lab33.DAL.Providers;

/// Бінарна серіалізація через BinaryWriter/BinaryReader; властивості обробляються рефлексією
public class BinaryProvider<T> : DataProvider<T> where T : class, new()
{
    public override string Extension => ".bin";

    public override void Write(string path, IEnumerable<T> items)
    {
        var list = items.ToList();
        using var writer = new BinaryWriter(File.Create(path));
        writer.Write(list.Count);
        foreach (var item in list)
            foreach (var property in PropertyMap<T>.All)
                WriteValue(writer, property.GetValue(item), property.PropertyType);
    }

    public override List<T> Read(string path)
    {
        using var reader = new BinaryReader(File.OpenRead(path));
        int count = reader.ReadInt32();
        var result = new List<T>(count);
        for (int i = 0; i < count; i++)
        {
            var item = new T();
            foreach (var property in PropertyMap<T>.All)
                property.SetValue(item, ReadValue(reader, property.PropertyType));
            result.Add(item);
        }
        return result;
    }

    private static void WriteValue(BinaryWriter writer, object? value, Type type)
    {
        switch (Type.GetTypeCode(type))
        {
            case TypeCode.Int32: writer.Write((int)value!); break;
            case TypeCode.Int64: writer.Write((long)value!); break;
            case TypeCode.Double: writer.Write((double)value!); break;
            case TypeCode.Decimal: writer.Write((decimal)value!); break;
            case TypeCode.Boolean: writer.Write((bool)value!); break;
            case TypeCode.String:
                var text = (string?)value;
                writer.Write(text != null);
                if (text != null) writer.Write(text);
                break;
            default:
                throw new NotSupportedException($"Тип {type.Name} не підтримується бінарним провайдером.");
        }
    }

    private static object? ReadValue(BinaryReader reader, Type type)
    {
        switch (Type.GetTypeCode(type))
        {
            case TypeCode.Int32: return reader.ReadInt32();
            case TypeCode.Int64: return reader.ReadInt64();
            case TypeCode.Double: return reader.ReadDouble();
            case TypeCode.Decimal: return reader.ReadDecimal();
            case TypeCode.Boolean: return reader.ReadBoolean();
            case TypeCode.String: return reader.ReadBoolean() ? reader.ReadString() : null;
            default:
                throw new NotSupportedException($"Тип {type.Name} не підтримується бінарним провайдером.");
        }
    }
}
