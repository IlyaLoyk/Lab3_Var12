namespace Lab33.BLL;

/// Додаткове вміння з варіанта 12: пірнати
public interface IDiver
{
    string Dive();
}

/// Власні класи сутностей рівня BLL (не залежать від DAL-сутностей)
public abstract class Person : IDiver
{
    public string Surname { get; set; } = "";

    /// Унікальний ключ для пошуку та видалення
    public abstract string Key { get; }

    public abstract string Describe();
    public abstract void Validate();

    public virtual string Dive() => $"{Surname} пірнає у воду.";

    protected static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidEntityDataException(message);
    }
}

public class Student : Person
{
    public int Course { get; set; }
    public string StudentCardNumber { get; set; } = "";
    public double AverageGrade { get; set; }
    public string Country { get; set; } = "";
    public string ForeignPassportNumber { get; set; } = "";

    public override string Key => StudentCardNumber;

    public override string Describe() =>
        $"Студент {Surname}, курс {Course}, квиток {StudentCardNumber}, середній бал {AverageGrade:0.##}, " +
        $"країна {Country}, закордонний паспорт {ForeignPassportNumber}";

    public override void Validate()
    {
        Require(!string.IsNullOrWhiteSpace(Surname), "Прізвище не може бути порожнім.");
        Require(Course >= 1 && Course <= 6, "Курс має бути від 1 до 6.");
        Require(!string.IsNullOrWhiteSpace(StudentCardNumber), "Номер студентського квитка не може бути порожнім.");
        Require(AverageGrade >= 0 && AverageGrade <= 100, "Середній бал має бути від 0 до 100.");
        Require(!string.IsNullOrWhiteSpace(Country), "Країна не може бути порожньою.");
        Require(!string.IsNullOrWhiteSpace(ForeignPassportNumber), "Номер закордонного паспорту не може бути порожнім.");
    }
}

public class Tailor : Person
{
    public int ExperienceYears { get; set; }
    public string Code { get; set; } = "";

    public override string Key => Code;

    public override string Describe() => $"Кравець {Surname}, стаж {ExperienceYears} р., код {Code}";

    public override string Dive() => $"{Surname} пірнає, не випускаючи голку з рук.";

    public override void Validate()
    {
        Require(!string.IsNullOrWhiteSpace(Surname), "Прізвище не може бути порожнім.");
        Require(ExperienceYears >= 0 && ExperienceYears <= 80, "Стаж має бути від 0 до 80 років.");
        Require(!string.IsNullOrWhiteSpace(Code), "Код кравця не може бути порожнім.");
    }
}

public class Singer : Person
{
    public string Genre { get; set; } = "";
    public string Code { get; set; } = "";

    public override string Key => Code;

    public override string Describe() => $"Співак {Surname}, жанр {Genre}, код {Code}";

    public override string Dive() => $"{Surname} пірнає і навіть під водою співає.";

    public override void Validate()
    {
        Require(!string.IsNullOrWhiteSpace(Surname), "Прізвище не може бути порожнім.");
        Require(!string.IsNullOrWhiteSpace(Genre), "Жанр не може бути порожнім.");
        Require(!string.IsNullOrWhiteSpace(Code), "Код співака не може бути порожнім.");
    }
}
