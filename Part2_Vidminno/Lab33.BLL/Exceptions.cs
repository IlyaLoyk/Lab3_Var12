namespace Lab33.BLL;

/// Базовий виняток предметної області. Генерується в BLL, а обробляється на рівні представлення
public class EntityServiceException : Exception
{
    public EntityServiceException(string message) : base(message) { }
    public EntityServiceException(string message, Exception inner) : base(message, inner) { }
}

/// Некоректні дані сутності
public class InvalidEntityDataException : EntityServiceException
{
    public InvalidEntityDataException(string message) : base(message) { }
}

/// Запис з таким ключем уже існує
public class DuplicateEntityException : EntityServiceException
{
    public DuplicateEntityException(string message) : base(message) { }
}

/// Запис з таким ключем не знайдено
public class EntityNotFoundException : EntityServiceException
{
    public EntityNotFoundException(string message) : base(message) { }
}

/// Помилка роботи з файлом
public class StorageException : EntityServiceException
{
    public StorageException(string message) : base(message) { }
    public StorageException(string message, Exception inner) : base(message, inner) { }
}
