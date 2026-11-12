using Lab33.BLL;

namespace Lab33.PL;

/// Власні класи сутностей рівня представлення; перетворюються на BLL-моделі перед передачею в сервіс
internal interface IEntityView
{
    string Describe();
}

internal class StudentView : IEntityView
{
    public string Surname { get; set; } = "";
    public int Course { get; set; }
    public string CardNumber { get; set; } = "";
    public double AverageGrade { get; set; }
    public string Country { get; set; } = "";
    public string PassportNumber { get; set; } = "";

    public static StudentView FromModel(Student s) => new()
    {
        Surname = s.Surname,
        Course = s.Course,
        CardNumber = s.StudentCardNumber,
        AverageGrade = s.AverageGrade,
        Country = s.Country,
        PassportNumber = s.ForeignPassportNumber
    };

    public Student ToModel() => new()
    {
        Surname = Surname,
        Course = Course,
        StudentCardNumber = CardNumber,
        AverageGrade = AverageGrade,
        Country = Country,
        ForeignPassportNumber = PassportNumber
    };

    public string Describe() =>
        $"{Surname,-15} | курс {Course} | квиток {CardNumber} | бал {AverageGrade:0.##} | {Country} | паспорт {PassportNumber}";
}

internal class TailorView : IEntityView
{
    public string Surname { get; set; } = "";
    public int ExperienceYears { get; set; }
    public string Code { get; set; } = "";

    public static TailorView FromModel(Tailor t) =>
        new() { Surname = t.Surname, ExperienceYears = t.ExperienceYears, Code = t.Code };

    public Tailor ToModel() =>
        new() { Surname = Surname, ExperienceYears = ExperienceYears, Code = Code };

    public string Describe() => $"{Surname,-15} | стаж {ExperienceYears} р. | код {Code}";
}

internal class SingerView : IEntityView
{
    public string Surname { get; set; } = "";
    public string Genre { get; set; } = "";
    public string Code { get; set; } = "";

    public static SingerView FromModel(Singer s) =>
        new() { Surname = s.Surname, Genre = s.Genre, Code = s.Code };

    public Singer ToModel() =>
        new() { Surname = Surname, Genre = Genre, Code = Code };

    public string Describe() => $"{Surname,-15} | жанр {Genre} | код {Code}";
}
