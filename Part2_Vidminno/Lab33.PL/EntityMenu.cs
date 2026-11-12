using Lab33.BLL;

namespace Lab33.PL;

/// Узагальнене підменю для однієї сутності: файл (створити/відкрити/зберегти/закрити/видалити),
/// перегляд, додавання, пошук, видалення, "пірнати" та додаткові пункти.
/// Усі винятки BLL перехоплюються тут, на рівні представлення.
internal class EntityMenu<TView, TModel>
    where TView : IEntityView
    where TModel : Person
{
    private readonly string _title;
    private readonly string _keyPrompt;
    private readonly IEntityService<TModel> _service;
    private readonly Func<TView> _read;
    private readonly Func<TModel, TView> _toView;
    private readonly Func<TView, TModel> _toModel;
    private readonly List<(string Title, Action Action)> _extra;

    public EntityMenu(
        string title,
        string keyPrompt,
        IEntityService<TModel> service,
        Func<TView> read,
        Func<TModel, TView> toView,
        Func<TView, TModel> toModel,
        List<(string Title, Action Action)>? extra = null)
    {
        _title = title;
        _keyPrompt = keyPrompt;
        _service = service;
        _read = read;
        _toView = toView;
        _toModel = toModel;
        _extra = extra ?? new List<(string Title, Action Action)>();
    }

    public void Run()
    {
        var items = new List<(string Title, Action Action)>
        {
            ("Створити файл", CreateFile),
            ("Відкрити файл", OpenFile),
            ("Зберегти файл", () => _service.Save()),
            ("Закрити файл", () => _service.CloseFile()),
            ("Видалити файл", () => _service.DeleteFile()),
            ("Показати всі записи", ShowAll),
            ("Додати запис", Add),
            ("Знайти запис", Find),
            ("Видалити запис", Remove),
            ("Пірнати", Dive),
        };
        items.AddRange(_extra);

        while (true)
        {
            Console.WriteLine($"\n=== {_title} === (файл: {(_service.IsOpen ? "відкрито" : "не відкрито")})");
            for (int i = 0; i < items.Count; i++)
                Console.WriteLine($"{i + 1}. {items[i].Title}");
            Console.WriteLine("0. Назад");

            int choice = ConsoleInput.ReadInt("Ваш вибір: ", 0, items.Count);
            if (choice == 0) return;

            try
            {
                items[choice - 1].Action();
            }
            catch (EntityServiceException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }

    // Назва та розташування файлу передаються з PL на нижчі рівні.
    private (string FilePath, StorageFormat Format)? AskFile()
    {
        var format = ConsoleInput.ReadFormat();
        string name = ConsoleInput.ReadString("Ім'я файлу (можна зі шляхом): ").Trim();
        if (name.Length == 0)
        {
            Console.WriteLine("Ім'я файлу не вказано.");
            return null;
        }
        if (!Path.HasExtension(name)) name += StorageFormats.DefaultExtension(format);
        return (name, format);
    }

    private void CreateFile()
    {
        var file = AskFile();
        if (file == null) return;
        _service.CreateFile(file.Value.FilePath, file.Value.Format);
        Console.WriteLine("Файл створено і відкрито.");
    }

    private void OpenFile()
    {
        var file = AskFile();
        if (file == null) return;
        _service.OpenFile(file.Value.FilePath, file.Value.Format);
        Console.WriteLine($"Файл відкрито, записів: {_service.GetAll().Count}.");
    }

    private void ShowAll()
    {
        var all = _service.GetAll();
        if (all.Count == 0)
        {
            Console.WriteLine("Записів немає.");
            return;
        }
        foreach (var model in all)
            Console.WriteLine(_toView(model).Describe());
    }

    private void Add()
    {
        var view = _read();
        _service.Add(_toModel(view));
        Console.WriteLine("Запис додано.");
    }

    private void Find()
    {
        string key = ConsoleInput.ReadString(_keyPrompt);
        Console.WriteLine(_toView(_service.FindByKey(key)).Describe());
    }

    private void Remove()
    {
        string key = ConsoleInput.ReadString(_keyPrompt);
        _service.RemoveByKey(key);
        Console.WriteLine("Запис видалено.");
    }

    private void Dive()
    {
        string key = ConsoleInput.ReadString(_keyPrompt);
        Console.WriteLine(_service.Dive(key));
    }
}
