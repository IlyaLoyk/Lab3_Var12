using Lab33.DAL.Providers;

namespace Lab33.DAL;

/// Контекст "бази даних" у файлі: створення/відкриття, оновлення, закриття та видалення файлу.
/// Формат файлу визначає провайдер, який передається ззовні.
public class EntityContext<T> where T : class, new()
{
    private readonly DataProvider<T> _provider;
    private List<T> _items = new();

    public EntityContext(DataProvider<T> provider) => _provider = provider;

    public string? FilePath { get; private set; }
    public bool IsOpen => FilePath != null;
    public IReadOnlyList<T> Items => _items;

    public void Create(string path)
    {
        if (File.Exists(path)) throw new IOException($"Файл \"{path}\" уже існує.");
        _provider.Write(path, Array.Empty<T>());
        _items = new List<T>();
        FilePath = path;
    }

    public void Open(string path)
    {
        if (!File.Exists(path)) throw new FileNotFoundException("Файл не знайдено.", path);
        _items = _provider.Read(path);
        FilePath = path;
    }

    public void Add(T item) => _items.Add(item);

    public bool Remove(T item) => _items.Remove(item);

    /// Оновлення файлу: записує поточний стан колекції
    public void Save()
    {
        if (FilePath == null) throw new InvalidOperationException("Файл не відкрито.");
        _provider.Write(FilePath, _items);
    }

    public void Close()
    {
        _items = new List<T>();
        FilePath = null;
    }

    public void DeleteFile()
    {
        if (FilePath == null) throw new InvalidOperationException("Файл не відкрито.");
        string path = FilePath;
        Close();
        File.Delete(path);
    }
}
