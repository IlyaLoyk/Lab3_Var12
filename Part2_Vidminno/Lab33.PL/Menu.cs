using System.Text;
using Lab33.BLL;

namespace Lab33.PL;

/// Рівень представлення: єдина точка входу MainMenu(), яку викликає Main()
public static class Menu
{
    public static void MainMenu()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        var studentService = new StudentService();

        var students = new EntityMenu<StudentView, Student>(
            "Студенти", "Номер студентського квитка: ", studentService,
            ReadStudent, StudentView.FromModel, v => v.ToModel(),
            new List<(string Title, Action Action)>
            {
                ("Іноземні відмінники 1-го курсу (кількість і дані з файлу)", () => ShowForeignExcellent(studentService))
            });

        var tailors = new EntityMenu<TailorView, Tailor>(
            "Кравці", "Код кравця: ", new TailorService(),
            ReadTailor, TailorView.FromModel, v => v.ToModel());

        var singers = new EntityMenu<SingerView, Singer>(
            "Співаки", "Код співака: ", new SingerService(),
            ReadSinger, SingerView.FromModel, v => v.ToModel());

        while (true)
        {
            Console.WriteLine("\n=== ГОЛОВНЕ МЕНЮ ===");
            Console.WriteLine("1. Студенти");
            Console.WriteLine("2. Кравці");
            Console.WriteLine("3. Співаки");
            Console.WriteLine("0. Вихід");

            switch (ConsoleInput.ReadInt("Ваш вибір: ", 0, 3))
            {
                case 1: students.Run(); break;
                case 2: tailors.Run(); break;
                case 3: singers.Run(); break;
                default: return;
            }
        }
    }

    private static StudentView ReadStudent() => new()
    {
        Surname = ConsoleInput.ReadString("Прізвище: "),
        Course = ConsoleInput.ReadInt("Курс: "),
        CardNumber = ConsoleInput.ReadString("Номер студентського квитка: "),
        AverageGrade = ConsoleInput.ReadDouble("Середній бал (0-100): "),
        Country = ConsoleInput.ReadString("Країна: "),
        PassportNumber = ConsoleInput.ReadString("Номер закордонного паспорту: ")
    };

    private static TailorView ReadTailor() => new()
    {
        Surname = ConsoleInput.ReadString("Прізвище: "),
        ExperienceYears = ConsoleInput.ReadInt("Стаж (років): "),
        Code = ConsoleInput.ReadString("Код кравця: ")
    };

    private static SingerView ReadSinger() => new()
    {
        Surname = ConsoleInput.ReadString("Прізвище: "),
        Genre = ConsoleInput.ReadString("Жанр: "),
        Code = ConsoleInput.ReadString("Код співака: ")
    };

    private static void ShowForeignExcellent(IStudentService service)
    {
        var list = service.GetForeignExcellentFirstYear();
        Console.WriteLine($"Іноземних студентів-відмінників 1-го курсу: {list.Count}");
        foreach (var student in list)
            Console.WriteLine(StudentView.FromModel(student).Describe());
    }
}
