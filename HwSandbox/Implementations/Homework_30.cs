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
        
    }

    private void TestFileSearch()
    {
        Console.WriteLine("Поиск файлов с событиями:\n");
        
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
        };
        
        searcher.Search(tempDir);
        Directory.Delete(tempDir, true);
        Console.WriteLine("\nДиректория удалена.");
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
        foreach (string file in Directory.GetFiles(path))
        {
            OnFileFound(new FileArgs(Path.GetFileName(file)));
        }
    }
    protected virtual void OnFileFound(FileArgs e) => FileFound?.Invoke(this, e);
}