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
        IPlayable Game = new GuessNumGame(new GuessNumGameConsoleConfiguration());
        bool letUsPlay = true;
        Game.Play();
        while (letUsPlay)
        {
            Console.WriteLine("\nСыграем снова?\nВведите yes (y) или да (д), чтобы продолжить)");
            string answer = Console.ReadLine().ToLower();
            if (answer is "y" or "д" or "yes" or "да")
            {
                Game.Play();
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

public interface IPlayable
{
    void Play();
}

public interface IConfigurable
{
    public int MinValue { get; } 
    public int MaxValue { get; }
    public int AttemptsNum { get; }
    void Configure();
}

public class GuessNumGameConsoleConfiguration : IConfigurable
{
    public int MinValue { get; private set; } 
    public int MaxValue { get; private set; }
    public int AttemptsNum { get; private set; }

    public void Configure()
    {
        Console.WriteLine("Для начала настроим игру.");
        Console.WriteLine("Определим количество попыток на отгадывание числа.");
        AttemptsNum = Extensions.GetPositiveNumValue();
        Console.WriteLine("\nОпределим нижнюю границу диапазона.");
        MinValue = Extensions.GetNumValue();
        Console.WriteLine("\nОпределим верхнюю границу диапазона.");
        while (MinValue >= MaxValue)
        {
            Console.WriteLine("\nЗначение верхней границы должно быть больше значения нижней.");
            MaxValue = Extensions.GetNumValue();
        }
    }
}

public class GuessNumGame : IPlayable
{
    IConfigurable _config;
    int _refNum;
    int _attemptsNum;
    public GuessNumGame(IConfigurable config)
    {
        _config = config;
        _config.Configure();
    }
    public void Play()
    {
        _attemptsNum = _config.AttemptsNum;
        Console.WriteLine("\nЗагадываю число...");
        _refNum = Extensions.GenerateRefNum(_config.MinValue, _config.MaxValue);
        Console.WriteLine("Готово!");
        bool userWon = false;
        while (!userWon && _attemptsNum > 0)
        {
            _attemptsNum--;
            Console.WriteLine("\nКакое число было загадано?");
            int guessedNum = Extensions.GetNumValue();
            userWon = Extensions.CompareNums(_refNum, guessedNum);
        }
        if (userWon)
        {
            Console.WriteLine($"Вы выиграли!\nБыло загадано число {_refNum}.");
        }
        else
        {
            Console.WriteLine("Число попыток исчерпано!");
        }
    }
}

public static class Extensions
{
    public static int GetNumValue()
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
    public static int GetPositiveNumValue()
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
    // загадываем число
    public static int GenerateRefNum(int min, int max)
    {
        Random rnd = new Random();
        return rnd.Next(min, max);
    }
    // обрабатываем предположение пользователя: сравниваем предполагаемое число с загаданным
    public static bool CompareNums(int refNum, int guessedNum)
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

