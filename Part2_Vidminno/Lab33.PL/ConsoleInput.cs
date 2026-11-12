using System.Globalization;
using Lab33.BLL;

namespace Lab33.PL;

/// Допоміжні методи введення з консолі (перевіряють лише формат, змістовну валідацію робить BLL)
internal static class ConsoleInput
{
    public static string ReadString(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine() ?? "";
    }

    public static int ReadInt(string prompt, int min = int.MinValue, int max = int.MaxValue)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max)
                return value;
            Console.WriteLine("Некоректне значення, спробуйте ще раз.");
        }
    }

    public static double ReadDouble(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string text = (Console.ReadLine() ?? "").Replace(',', '.');
            if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                return value;
            Console.WriteLine("Некоректне число, спробуйте ще раз.");
        }
    }

    public static StorageFormat ReadFormat()
    {
        Console.WriteLine("Формат файлу: 1 - бінарний, 2 - XML, 3 - JSON, 4 - користувацький");
        return ReadInt("Формат: ", 1, 4) switch
        {
            1 => StorageFormat.Binary,
            2 => StorageFormat.Xml,
            3 => StorageFormat.Json,
            _ => StorageFormat.Custom
        };
    }
}
