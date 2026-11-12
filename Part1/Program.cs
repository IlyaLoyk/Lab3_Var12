using System.Text;
using System.Xml.Serialization;
using Part1;

Console.OutputEncoding = Encoding.UTF8;

// ---------- Методи класу ----------
Console.WriteLine("=== Методи класу Account ===");
var demo = new Account(2001, 100m);
demo.Deposit(250m);
Console.WriteLine($"Після поповнення на 250: {demo}");
demo.Withdraw(100m);
Console.WriteLine($"Після зняття 100:        {demo}");
demo.ChangeOwner(2002);
Console.WriteLine($"Після переведення на власника 2002: {demo}");
Console.WriteLine($"Сума за курсом 0.025: {demo.ConvertSum(0.025m)}");
demo.PrintInfo();

// ---------- Пункти 1-2: масив з 4 об'єктів ----------
Account[] accounts =
{
    new(1001, 500m),
    new(1002, 1250.75m),
    new(1003, 0m),
    new(1004, 99999.99m),
};

Directory.CreateDirectory("data");

var formats = new[]
{
    new Format("Бінарна",       "data/accounts.bin",        Serializers.SaveBinary, Serializers.LoadBinary),
    new Format("Користувацька", "data/accounts.custom.txt", Serializers.SaveCustom, Serializers.LoadCustom),
    new Format("XML",           "data/accounts.xml",        Serializers.SaveXml,    Serializers.LoadXml),
    new Format("JSON",          "data/accounts.json",       Serializers.SaveJson,   Serializers.LoadJson),
};

foreach (var f in formats)
{
    Console.WriteLine($"\n=== {f.Name} серіалізація ===");

    // Пункт 2: серіалізація масиву у файл
    f.Save(f.File, accounts);

    // Пункт 3: новий масив, відновлений з файлу
    Account[] restored = f.Load(f.File).ToArray();
    Console.WriteLine($"Масив відновлено: {restored.Length} об'єкти(ів), збігається з оригіналом: {accounts.SequenceEqual(restored)}");
    foreach (var a in restored) Console.WriteLine("  " + a);

    // Пункт 4: колекція List<Account> і порівняння з масивом
    var list = new List<Account>(accounts);
    string listFile = f.File + ".list";
    f.Save(listFile, list);
    var restoredList = f.Load(listFile);
    bool sameFiles = File.ReadAllBytes(f.File).SequenceEqual(File.ReadAllBytes(listFile));
    Console.WriteLine($"List відновлено: {restoredList.Count}, збігається з масивом: {accounts.SequenceEqual(restoredList)}, файли однакові: {sameFiles}");
}

// Порівняння "рідного" XML-представлення масиву і колекції
static string ToXml<T>(T obj)
{
    var writer = new StringWriter();
    new XmlSerializer(typeof(T)).Serialize(writer, obj);
    return writer.ToString();
}

string xmlArray = ToXml(accounts);
string xmlList = ToXml(new List<Account>(accounts));
Console.WriteLine($"\nXML масиву Account[] і List<Account> однакові: {xmlArray == xmlList}");

record Format(string Name, string File, Action<string, IEnumerable<Account>> Save, Func<string, List<Account>> Load);
