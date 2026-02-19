using HwSandbox.Abstractions;

namespace HwSandbox.Implementations;
/*
   Делегаты и события
   Цель:
   В этом задании требуется реализовать механизмы делегатов и событий для получения практического навыка их применения
   
   Описание/Пошаговая инструкция выполнения домашнего задания:
   Написать обобщённую функцию расширения, находящую и возвращающую максимальный элемент коллекции.
   Функция должна принимать на вход делегат, преобразующий входной тип в число для возможности поиска максимального значения.
   public static T GetMax(this IEnumerable collection, Func<T, float> convertToNumber) where T : class;
   Написать класс, обходящий каталог файлов и выдающий событие при нахождении каждого файла;
   Оформить событие и его аргументы с использованием .NET соглашений:
   public event EventHandler FileFound;
   FileArgs – будет содержать имя файла и наследоваться от EventArgs
   Добавить возможность отмены дальнейшего поиска из обработчика;
   Вывести в консоль сообщения, возникающие при срабатывании событий и результат поиска максимального элемента.
   
   Критерии оценки:
   4 балла: Пункт 1
   2 балла: Пункты 2-3
   2 балла: Пункт 4
   2 балла: Пункт 5
   
   Минимальный проходной балл: 6
 */
public class Homework_30: Homework
{
    // Планирую рассмотреть работу с делегатами и событиями на примере юридической практики.
    public override void Go()
    {
        Console.WriteLine("Лекция 30. Делегаты и события:");
        Console.WriteLine("------------------");
        // будем искать самые дорогие судебные дела
        TestGetMax();
        Console.WriteLine("------------------");
        // и просматривать папки с документами
        TestFileSearch();
        Console.WriteLine("------------------");
    }

    private void TestGetMax()
    {
        Console.WriteLine("1. Поиск максимального элемента:\n");
        
        var cases = new List<Case>
        {
            new("А07-2048/25", 1_500_000m, 0.85f),
            new("02-512/24", 750_000m, 0.65f),
            new("01-128/23", 2_500_000m, 0.92f),
            null,
            new("А56-032/22", 8_900_000m, 0.45f)
        };

        var maxClaim = cases.GetMax(c => (float)c.ClaimAmount);
        Console.WriteLine($"Макс. сумма иска: {maxClaim?.CaseNumber} - {maxClaim?.ClaimAmount:C}\n");
        
        var maxWin = cases.GetMax(c => c.WinProbability);
        Console.WriteLine($"Макс. вероятность: {maxWin?.CaseNumber} - {maxWin?.WinProbability:P1}\n");
    }

    private void TestFileSearch()
    {
        Console.WriteLine("2. Поиск файлов с событиями:\n");
        
        string tempDir = Path.Combine(Path.GetTempPath(), $"Legal_{DateTime.Now:yyyyMMdd_HHmmss}");
        Directory.CreateDirectory(tempDir);
        
        string[] files = {
            "Исковое_заявление.docx",
            "Решение_суда.pdf",
            "Апелляционная_жалоба.docx",
            "Договор.pdf",
            "Исполнительный_лист.pdf"
        };
        
        foreach (string file in files)
            File.Create(Path.Combine(tempDir, file)).Dispose();
        
        Console.WriteLine($"Создано {files.Length} файлов\n");
        
        var searcher = new FileSearcher();
        searcher.FileFound += (sender, e) =>
        {
            Console.WriteLine($"  Найден: {e.FileName}");
            if (e.FileName.Contains("Апелляционная"))
            {
                Console.WriteLine("...Останавливаем поиск.");
                ((FileSearcher)sender).CancelSearch();
            }
        };
        
        searcher.Search(tempDir);
        Directory.Delete(tempDir, true);
        Console.WriteLine("\nДиректория удалена.");
    }
}

public static class Extensions
{
    public static T GetMax<T>(this IEnumerable<T> collection, Func<T, float> converter) where T : class
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        if (converter == null) throw new ArgumentNullException(nameof(converter));
        
        T maxItem = null;
        float maxValue = float.MinValue;
        bool hasItems = false;
        
        foreach (var item in collection)
        {
            if (item == null) continue;
            
            float value = converter(item);
            if (!hasItems || value > maxValue)
            {
                maxValue = value;
                maxItem = item;
                hasItems = true;
            }
        }
        
        if (!hasItems) throw new InvalidOperationException("Нет элементов для сравнения");
        return maxItem;
    }
}

public class Case
{
    /// <summary>
    /// Номер дела
    /// </summary>
    public string CaseNumber { get; }
    /// <summary>
    /// Сумма иска (в рублях)
    /// </summary>
    public decimal ClaimAmount { get; }
    /// <summary>
    /// Вероятность выигрыша (от 0 до 1)
    /// </summary>
    public float WinProbability { get; }
    
    public Case(string number, decimal amount, float probability)
    {
        CaseNumber = number;
        ClaimAmount = amount;
        WinProbability = probability;
    }
}

public class FileArgs : EventArgs
{
    public string FileName { get; }
    public FileArgs(string fileName) => FileName = fileName;
}

public class FileSearcher
{
    public event EventHandler<FileArgs> FileFound;
    private bool _cancel;
    
    public void Search(string path)
    {
        if (!Directory.Exists(path)) return;
        
        _cancel = false;
        foreach (string file in Directory.GetFiles(path))
        {
            if (_cancel) break;
            OnFileFound(new FileArgs(Path.GetFileName(file)));
        }
    }
    
    public void CancelSearch() => _cancel = true;
    protected virtual void OnFileFound(FileArgs e) => FileFound?.Invoke(this, e);
}