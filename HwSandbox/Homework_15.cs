using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwSandbox
{
    /*
     * Многопоточный проект
    Цель:
    Применение разных способов распараллеливания задач и оценка оптимального способа реализации.

    Описание/Пошаговая инструкция выполнения домашнего задания:
    Напишите вычисление суммы элементов массива интов:
    Обычное
    Параллельное (для реализации использовать Thread, например List)
    Параллельное с помощью LINQ
    Замерьте время выполнения для 100 000, 1 000 000 и 10 000 000

    Укажите в таблице результаты замеров, указав:

    Окружение (характеристики компьютера и ОС)
    Время выполнения последовательного вычисления
    Время выполнения параллельного вычисления
    Время выполнения LINQ
    Пришлите в чат с преподавателем помимо ссылки на репозиторий номера своих строк в таблице.

    Критерии оценки:
    5 баллов: Сделан пункт 1;
    2 балла: Сделан пункт 2;
    2 балла: Сделан пункт 3;
    1 балла: Добавлены замеры в таблицу;

    Минимальный проходной балл: 8.
    */
    internal class Homework_15
    {
        internal Homework_15()
        {
            
        }
        
        internal async void Go()
        {
            Console.WriteLine("Лекция 15. Внутрипроцессное взаимодействие:");
            Console.WriteLine("------------------");
            
            int processors = Environment.ProcessorCount;
            string osVersion = Environment.OSVersion.VersionString;
            
            // Замерьте время выполнения для 100 000,
            Console.WriteLine("Выполнение замеров для суммирования 100 000 элементов.");
            await Summ(100000);
            Console.WriteLine("------------------");
            // 1 000 000
            Console.WriteLine("Выполнение замеров для суммирования 1 000 000 элементов.");
            await Summ(1000000);
            Console.WriteLine("------------------");
            // и 10 000 000
            Console.WriteLine("Выполнение замеров для суммирования 10 000 000 элементов.");
            await Summ(10000000);
            Console.WriteLine("------------------");
            Console.WriteLine("Готово.");
        }
        //Напишите вычисление суммы элементов массива интов:
        private Task<TimeSpan[]> Summ(int quantity)
        {
            // планирую возвращать результат массивом из трёх замеров (sync, parallel_thread и parallel_linq)
            TimeSpan[] results = new TimeSpan[3];
            Random random = new Random();
            int[] nums = new int[quantity];
            for (int i = 0; i < nums.Length; i++)
            {
                nums[i] = random.Next();
            }
            // Обычное
            results[0] = Task.Run(() => SummSync(nums)).Result;
            // Параллельное (для реализации использовать Thread, например List)
            results[1] = Task.Run(() => SummParallel_withThreads(nums)).Result;
            // Параллельное с помощью LINQ
            results[2] =  Task.Run(() => SummParallel_withLinq(nums)).Result;
            return Task.FromResult(results);
        }
        
        private TimeSpan SummSync(int[] numbers)
        {
            Console.WriteLine("Start counting sync");
            long sum = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < numbers.Length; i++)
            {
                sum += numbers[i];
            }
            sw.Stop();
            Console.WriteLine("sync: " + sum);
            return sw.Elapsed;
        }
        
        private TimeSpan SummParallel_withThreads(int[] numbers)
        {
            Console.WriteLine("Start counting parallel with threads");
            long sum = 0;
            Stopwatch sw = new Stopwatch();
            int processors = Environment.ProcessorCount;
            Thread[] threads = new Thread[processors];
            
            int arrPartsCnt = numbers.Length / processors;
            
            sw.Start();
            for (int i = 0; i < processors; i++)
            {
                // делим массив по числу вычислительных ядер
                int startIndex = i * arrPartsCnt;
                int endIndex = i == processors - 1 ? numbers.Length - 1 : startIndex + arrPartsCnt - 1;
                long currSum = 0;
                Thread thread = new Thread(() =>
                {
                    for (int j = startIndex; j <= endIndex; j++)
                    {
                        currSum += numbers[j];
                    }
                    Interlocked.Add(ref sum, currSum);
                });
                threads[i] = thread;
                thread.Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            sw.Stop();
            Console.WriteLine("parallel with threads: " + sum);
            return sw.Elapsed;
        }
        
        private TimeSpan SummParallel_withLinq(int[] numbers)
        {
            Console.WriteLine("Start counting parallel with LINQ");
            long sum = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            sum = numbers.AsParallel().Select(x => (long)x).Sum();
            sw.Stop();
            Console.WriteLine("parallel with LINQ: " + sum);
            return sw.Elapsed;
        }
    }

}
