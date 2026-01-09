using HwSandbox.Implementations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
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
            

            TableWriter tableWriter = new TableWriter();
            tableWriter.Add(["Количество вычислительных ядер: ", processors.ToString()]);
            tableWriter.Add(["Версия ОС: ", osVersion]);

            // Замерьте время выполнения для 100 000,
            tableWriter.Add(["Число интов", "Синхронно", "Параллельно (Threads)", "Параллельно (LINQ)"]);
            bool random = true;

            tableWriter.Add(await GetSummResults(100000, random));

            // 1 000 000
            tableWriter.Add(await GetSummResults(1000000, random));

            // и 10 000 000
            tableWriter.Add(await GetSummResults(10000000, random));


            // дополнительный тест с другой выборкой (что, если взять не рандомные числа, а последовательные)
            random = false;

            tableWriter.Add(await GetSummResults(100000, random));

            // 1 000 000
            tableWriter.Add(await GetSummResults(1000000, random));

            // и 10 000 000
            tableWriter.Add(await GetSummResults(10000000, random));


            tableWriter.Write();
            Console.WriteLine("Готово.");
        }
        private async Task<string[]> GetSummResults(int quantity, bool random)
        {
            Console.WriteLine($"Выполнение замеров для суммирования {quantity} элементов.");
            string[] valuesArray = new string[4];
            int cellCounter = 1;
            valuesArray[0] = quantity.ToString();

            foreach (var item in await Summ(quantity, random))
            {
                valuesArray[cellCounter++] = item.ToString();
            }
            Console.WriteLine("------------------");
            return valuesArray;
        }
        //Напишите вычисление суммы элементов массива интов:
        private Task<TimeSpan[]> Summ(int quantity, bool random)
        {
            // планирую возвращать результат массивом из трёх замеров (sync, parallel_thread и parallel_linq)
            TimeSpan[] results = new TimeSpan[3];
            int[] nums = random ? GetNumsArray_randomly(quantity) : GetNumsArray_sequentially(quantity); 
            // Обычное
            results[0] = SummSync(nums);
            // Параллельное (для реализации использовать Thread, например List)
            results[1] = SummParallel_withThreads(nums);
            // Параллельное с помощью LINQ
            results[2] = SummParallel_withLinq(nums);
            return Task.FromResult(results);
        }
        private int[] GetNumsArray_randomly(int quantity)
        {
            Random random = new Random();
            int[] nums = new int[quantity];
            for (int i = 0; i < nums.Length; i++)
            {
                nums[i] = random.Next();
            }
            return nums;
        }
        private int[] GetNumsArray_sequentially(int quantity)
        {
            IEnumerable<int> nums = Enumerable.Range(1, quantity);
            return nums.ToArray();
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
            
            // 
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
