using System.Text.Json;
using Lab33.DAL;

namespace Lab33.BLL;

/// Узагальнений сервіс: додавання, видалення, пошук. Не працює з файлами і консоллю напряму —
public abstract class EntityService<TModel, TEntity> : IEntityService<TModel>
    where TModel : Person
    where TEntity : class, new()
{
    private EntityContext<TEntity>? _context;

    protected abstract TModel ToModel(TEntity entity);
    protected abstract TEntity ToEntity(TModel model);

    public bool IsOpen => _context != null && _context.IsOpen;

    private EntityContext<TEntity> Context
    {
        get
        {
            if (_context == null || !_context.IsOpen)
                throw new StorageException("Спочатку створіть або відкрийте файл.");
            return _context;
        }
    }

    // ---------- робота з файлом ----------

    public void CreateFile(string path, StorageFormat format) => Guard(() =>
    {
        var context = NewContext(format);
        context.Create(path);
        _context = context;
    });

    public void OpenFile(string path, StorageFormat format) => Guard(() =>
    {
        var context = NewContext(format);
        context.Open(path);
        _context = context;
    });

    public void Save() => Guard(() => Context.Save());

    public void CloseFile()
    {
        _context?.Close();
        _context = null;
    }

    public void DeleteFile() => Guard(() =>
    {
        Context.DeleteFile();
        _context = null;
    });

    // ---------- операції над сутностями ----------

    public IReadOnlyList<TModel> GetAll() => Context.Items.Select(ToModel).ToList();

    public void Add(TModel model)
    {
        model.Validate();
        var context = Context;
        if (context.Items.Any(e => SameKey(ToModel(e).Key, model.Key)))
            throw new DuplicateEntityException($"Запис з ключем \"{model.Key}\" уже існує.");

        Guard(() =>
        {
            context.Add(ToEntity(model));
            context.Save();
        });
    }

    public TModel FindByKey(string key) => ToModel(FindEntity(key));

    public void RemoveByKey(string key)
    {
        var context = Context;
        var entity = FindEntity(key);
        Guard(() =>
        {
            context.Remove(entity);
            context.Save();
        });
    }

    public string Dive(string key) => FindByKey(key).Dive();

    // ---------- допоміжне ----------

    private TEntity FindEntity(string key) =>
        Context.Items.FirstOrDefault(e => SameKey(ToModel(e).Key, key))
        ?? throw new EntityNotFoundException($"Запис з ключем \"{key}\" не знайдено.");

    private static bool SameKey(string a, string b) =>
        string.Equals(a.Trim(), b.Trim(), StringComparison.OrdinalIgnoreCase);

    private static EntityContext<TEntity> NewContext(StorageFormat format) =>
        new(StorageFormats.CreateProvider<TEntity>(format));

    /// Перетворює технічні помилки файлової системи/форматів у власний StorageException
    private static void Guard(Action action)
    {
        try
        {
            action();
        }
        catch (Exception ex) when (IsStorageProblem(ex))
        {
            throw new StorageException($"Помилка роботи з файлом: {ex.Message}", ex);
        }
    }

    private static bool IsStorageProblem(Exception ex) =>
        ex is IOException or UnauthorizedAccessException or InvalidOperationException or JsonException
            or FormatException or InvalidCastException or OverflowException or NotSupportedException
            or ArgumentException;
}
