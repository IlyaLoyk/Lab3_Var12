namespace Lab33.BLL;

/// Контракт сервісу, яким користується рівень представлення (PL знає лише про BLL)
public interface IEntityService<TModel> where TModel : Person
{
    bool IsOpen { get; }

    void CreateFile(string path, StorageFormat format);
    void OpenFile(string path, StorageFormat format);
    void Save();
    void CloseFile();
    void DeleteFile();

    IReadOnlyList<TModel> GetAll();
    void Add(TModel model);
    TModel FindByKey(string key);
    void RemoveByKey(string key);
    string Dive(string key);
}

public interface IStudentService : IEntityService<Student>
{
    /// Іноземні студенти-відмінники 1-го курсу
    IReadOnlyList<Student> GetForeignExcellentFirstYear();

    int CountForeignExcellentFirstYear();
}
