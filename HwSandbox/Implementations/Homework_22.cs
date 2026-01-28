using HwSandbox.Abstractions;

namespace HwSandbox.Implementations;
/*
   Демонстрация SOLID принципов
   Цель:
   Практическое применение SOLID принципов.
   
   Описание/Пошаговая инструкция выполнения домашнего задания:
   На примере реализации игры «Угадай число» продемонстрировать практическое применение SOLID принципов.
   Программа рандомно генерирует число, пользователь должен угадать это число. 
   При каждом вводе числа программа пишет больше или меньше отгадываемого. 
   Кол-во попыток отгадывания и диапазон чисел должен задаваться из настроек.
   В отчёте написать, что именно сделано по каждому принципу.
   Приложить ссылку на проект и написать, сколько времени ушло на выполнение задачи.
   
   Критерии оценки:
   2 балла: Принцип единственной ответственности;
   1 балла: Принцип инверсии зависимостей;
   2 балла: Принцип разделения интерфейса;
   2 балла: Принцип открытости/закрытости;
   2 балла: Принцип подстановки Барбары Лисков;
   1 балл: CodeStyle, грамотная архитектура, всё замечания проверяющего исправлены.
   
   Минимально необходимый балл: 6.
 */
public class Homework_22: Homework
{
    public Homework_22()
    {
        
    }
    public override void Go()
    {
        Console.WriteLine("Лекция 22. Принципы SOLID:");
        Console.WriteLine("------------------");
        
        // Создаём и настраиваем конфигурацию игры
        IConfigurable config = new GuessNumGameConsoleConfiguration();
        config.Configure();
        
        // Создаём игру, передавая все зависимости через конструктор (Принцип инверсии зависимостей)
        IPlayable game = new GuessNumGame(config, new RandomNumGenerator(), new NumComparer(), new NumGetter());
        bool letUsPlay = true;
        game.Play();
        while (letUsPlay)
        {
            Console.WriteLine("\nСыграем снова?\nВведите yes (y) или да (д), чтобы продолжить.");
            string answer = Console.ReadLine()?.ToLower();
            if (answer is "y" or "д" or "yes" or "да")
            {
                game.Play();
            }
            else
            {
                letUsPlay = false;
                Console.WriteLine("\nКонец игры.");
            }
        }
        Console.WriteLine("------------------");
    }
}

// Принцип разделения интерфейса:
// Функционал разбит на отдельные интерфейсы
public interface IPlayable
{
    void Play();
}

public interface IConfigurable
{
    int MinValue { get; }
    int MaxValue { get; }
    int AttemptsNum { get; }
    void Configure();
}

public interface INumGeneratable
{
    int Generate(int min, int max);
}

public interface INumComparable
{
    bool Compare(int refNum, int guessedNum);
}

public interface INumGettable
{
    int GetNumValue();
    int GetPositiveNumValue();
}

// Принцип инверсии зависимостей:
// Класс зависит от абстракций, а не от конкретных реализаций
public class GuessNumGame : IPlayable
{
    private readonly IConfigurable _config;
    private readonly INumGeneratable _generator;
    private readonly INumComparable _comparer;
    private readonly INumGettable _gettable;

    // Все зависимости передаются через конструктор
    public GuessNumGame(
        IConfigurable config,
        INumGeneratable generator,
        INumComparable comparer,
        INumGettable gettable)
    {
        _config = config;
        _generator = generator;
        _comparer = comparer;
        _gettable = gettable;
    }

    public void Play()
    {
        int attemptsNum = _config.AttemptsNum;
        Console.WriteLine("\nЗагадываю число...");
        int refNum = _generator.Generate(_config.MinValue, _config.MaxValue);
        Console.WriteLine("Готово!");
        bool userWon = false;
        while (!userWon && attemptsNum > 0)
        {
            attemptsNum--;
            Console.WriteLine("\nКакое число было загадано?");
            int guessedNum = _gettable.GetNumValue();
            userWon = _comparer.Compare(refNum, guessedNum);
        }
        if (userWon)
        {
            Console.WriteLine($"Вы выиграли!\nБыло загадано число {refNum}.");
        }
        else
        {
            Console.WriteLine("Число попыток исчерпано!");
        }
    }
}

// Принцип единственной ответственности:
// Классы отвечают только за одну часть функционала:
// за настройку параметров игры
public class GuessNumGameConsoleConfiguration : IConfigurable
{
    public int MinValue { get; private set; } 
    public int MaxValue { get; private set; }
    public int AttemptsNum { get; private set; }
    
    private readonly INumGettable _gettable;

    public GuessNumGameConsoleConfiguration()
    {
        _gettable = new NumGetter();
    }

    public void Configure()
    {
        Console.WriteLine("Для начала настроим игру.");
        Console.WriteLine("Определим количество попыток на отгадывание числа.");
        AttemptsNum = _gettable.GetPositiveNumValue();
        Console.WriteLine("\nОпределим нижнюю границу диапазона.");
        MinValue = _gettable.GetNumValue();
        Console.WriteLine("\nОпределим верхнюю границу диапазона.");
        while (MinValue >= MaxValue)
        {
            Console.WriteLine("\nЗначение верхней границы должно быть больше значения нижней.");
            MaxValue = _gettable.GetNumValue();
        }
    }
}

// за получение данных из консоли
public class NumGetter : INumGettable
{
    public int GetNumValue()
    {
        string input = string.Empty;
        int result = 0;
        while (!int.TryParse(input, out result))
        {
            Console.WriteLine("Введите целочисленное значение.");
            input = Console.ReadLine();
        }
        return result;
    }

    public int GetPositiveNumValue()
    {
        string input = string.Empty;
        int result = 0;
        while (!int.TryParse(input, out result) || result <= 0)
        {
            Console.WriteLine("Введите целочисленное значение больше нуля.");
            input = Console.ReadLine();
        }
        return result;
    }
}

// за генерацию случайного числа в заданном диапазоне
public class RandomNumGenerator : INumGeneratable
{
    private readonly Random _rnd = new Random();

    public int Generate(int min, int max)
    {
        return _rnd.Next(min, max);
    }
}

// за сравнение предполагаемого числа с загаданным
public class NumComparer : INumComparable
{
    public bool Compare(int refNum, int guessedNum)
    {
        if (guessedNum == refNum)
        {
            return true;
        }
        if (guessedNum < refNum)
        {
            Console.WriteLine("\nВаше число меньше загаданного.");
        }
        else
        {
            Console.WriteLine("\nВаше число больше загаданного.");
        }
        return false;
    }
}

// Принцип подстановки Лисков:
// если заменить настройку игры из консоли конфигурацией с предустановленными значениями, работа игры не изменится.
// Принцип открытости/закрытости:
// класс остаётся открытым для расширения, но закрытым для модификации заданных условий.
public class GuessNumGamePresetConfiguration : IConfigurable
{
    public int MinValue { get; }
    public int MaxValue { get; }
    public int AttemptsNum { get; }

    public GuessNumGamePresetConfiguration(int minValue, int maxValue, int attemptsNum)
    {
        MinValue = minValue;
        MaxValue = maxValue;
        AttemptsNum = attemptsNum;
    }
    
    public void Configure()
    {
        Console.WriteLine($"Заданы настройки: диапазон {MinValue}-{MaxValue}, попытки: {AttemptsNum}");
    }
}