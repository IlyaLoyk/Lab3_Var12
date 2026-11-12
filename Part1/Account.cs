using System.Globalization;

namespace Part1;

/// Варіант 12: Рахунок
/// Публічний конструктор без параметрів і публічні властивості потрібні для XML/JSON-серіалізації.
public class Account : IEquatable<Account>
{
    public Account() { }

    public Account(int ownerCode, decimal sum)
    {
        OwnerCode = ownerCode;
        Sum = sum;
    }

    public int OwnerCode { get; set; }
    public decimal Sum { get; set; }

    /// Поповнення рахунку
    public void Deposit(decimal amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Сума поповнення має бути додатною.");
        Sum += amount;
    }

    /// Зняття готівки
    public void Withdraw(decimal amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Сума зняття має бути додатною.");
        if (amount > Sum) throw new InvalidOperationException("Недостатньо коштів на рахунку.");
        Sum -= amount;
    }

    /// Переведення рахунку на іншого власника
    public void ChangeOwner(int newOwnerCode) => OwnerCode = newOwnerCode;

    /// Перерахування суми за певним курсом
    public decimal ConvertSum(decimal rate)
    {
        if (rate <= 0) throw new ArgumentOutOfRangeException(nameof(rate), "Курс має бути додатним.");
        return Sum * rate;
    }

    /// Виведення інформації про рахунок
    public void PrintInfo() => Console.WriteLine(ToString());

    public override string ToString() =>
        $"Рахунок: власник {OwnerCode}, сума {Sum.ToString("0.00", CultureInfo.InvariantCulture)}";

    // ---- користувацька серіалізація ----
    public string ToCustomString() =>
        $"{OwnerCode}|{Sum.ToString(CultureInfo.InvariantCulture)}";

    public static Account FromCustomString(string line)
    {
        var parts = line.Split('|');
        if (parts.Length != 2) throw new FormatException($"Некоректний рядок: \"{line}\"");
        return new Account(
            int.Parse(parts[0], CultureInfo.InvariantCulture),
            decimal.Parse(parts[1], CultureInfo.InvariantCulture));
    }

    // ---- порівняння об'єктів ----
    public bool Equals(Account? other) =>
        other is not null && OwnerCode == other.OwnerCode && Sum == other.Sum;

    public override bool Equals(object? obj) => Equals(obj as Account);
    public override int GetHashCode() => HashCode.Combine(OwnerCode, Sum);
}
