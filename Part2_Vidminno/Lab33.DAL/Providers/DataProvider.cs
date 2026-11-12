namespace Lab33.DAL.Providers;

/// <summary>Абстракція провайдера: відповідає за читання/запис колекції у файл певного формату.</summary>
public abstract class DataProvider<T> where T : class, new()
{
    public abstract string Extension { get; }
    public abstract List<T> Read(string path);
    public abstract void Write(string path, IEnumerable<T> items);
}
