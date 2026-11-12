# Лабораторна робота 3.3, варіант 12 («відмінно»)

| Папка | Що це |
|---|---|
| `Part1/` | Частина 1: клас Account, 4 об'єкти, бінарна / користувацька / XML / JSON серіалізація, масив vs List |
| `Part2_Vidminno/` | Частина 2: App -> PL (Menu) -> BLL (EntityService) -> DAL (EntityContext + DataProvider-и) |

Запуск (потрібен .NET 8 SDK; інша версія - змінити `TargetFramework` у `.csproj`):

    cd Part1 && dotnet run
    cd Part2_Vidminno/Lab33.App && dotnet run

Припущення: «іноземець» = країна не «Україна»/«Ukraine»; «відмінник» = середній бал >= 90 (константи в `StudentService`).
Додаткові сутності Tailor, Singer; вміння «Пірнати» = інтерфейс `IDiver`.
