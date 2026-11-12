using Lab33.DAL;

namespace Lab33.BLL;

public class StudentService : EntityService<Student, StudentEntity>, IStudentService
{
    public const string HomeCountry = "Україна";

    /// Поріг "відмінника" за 100-бальною шкалою
    public const double ExcellentGrade = 90;

    protected override Student ToModel(StudentEntity e) => new()
    {
        Surname = e.Surname,
        Course = e.Course,
        StudentCardNumber = e.StudentCardNumber,
        AverageGrade = e.AverageGrade,
        Country = e.Country,
        ForeignPassportNumber = e.ForeignPassportNumber
    };

    protected override StudentEntity ToEntity(Student m) => new()
    {
        Surname = m.Surname,
        Course = m.Course,
        StudentCardNumber = m.StudentCardNumber,
        AverageGrade = m.AverageGrade,
        Country = m.Country,
        ForeignPassportNumber = m.ForeignPassportNumber
    };

    public IReadOnlyList<Student> GetForeignExcellentFirstYear() =>
        GetAll()
            .Where(s => s.Course == 1 && s.AverageGrade >= ExcellentGrade && IsForeign(s.Country))
            .ToList();

    public int CountForeignExcellentFirstYear() => GetForeignExcellentFirstYear().Count;

    private static bool IsForeign(string country)
    {
        string c = country.Trim();
        return !c.Equals(HomeCountry, StringComparison.OrdinalIgnoreCase)
               && !c.Equals("Ukraine", StringComparison.OrdinalIgnoreCase);
    }
}

public class TailorService : EntityService<Tailor, TailorEntity>
{
    protected override Tailor ToModel(TailorEntity e) =>
        new() { Surname = e.Surname, ExperienceYears = e.ExperienceYears, Code = e.Code };

    protected override TailorEntity ToEntity(Tailor m) =>
        new() { Surname = m.Surname, ExperienceYears = m.ExperienceYears, Code = m.Code };
}

public class SingerService : EntityService<Singer, SingerEntity>
{
    protected override Singer ToModel(SingerEntity e) =>
        new() { Surname = e.Surname, Genre = e.Genre, Code = e.Code };

    protected override SingerEntity ToEntity(Singer m) =>
        new() { Surname = m.Surname, Genre = m.Genre, Code = m.Code };
}
