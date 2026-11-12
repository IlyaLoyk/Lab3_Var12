namespace Lab33.DAL;

/// Сутності рівня DAL: прості контейнери даних

public class StudentEntity
{
    public string Surname { get; set; } = "";
    public int Course { get; set; }
    public string StudentCardNumber { get; set; } = "";
    public double AverageGrade { get; set; }
    public string Country { get; set; } = "";
    public string ForeignPassportNumber { get; set; } = "";
}

public class TailorEntity
{
    public string Surname { get; set; } = "";
    public int ExperienceYears { get; set; }
    public string Code { get; set; } = "";
}

public class SingerEntity
{
    public string Surname { get; set; } = "";
    public string Genre { get; set; } = "";
    public string Code { get; set; } = "";
}
