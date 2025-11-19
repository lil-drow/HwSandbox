using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwSandbox
{
    // Лекция 10. Введение в параллелеризм:
    // Пункт 1: Прочитать 3 файла параллельно и вычислить количество пробелов в них (через Task).
    // Пункт 2: Написать функцию, принимающую в качестве аргумента путь к папке. Из этой папки параллельно прочитать все файлы и вычислить количество пробелов в них.
    //Замерьте время выполнения кода (класс Stopwatch).
    internal class Homework_10
    {
        internal Homework_10()
        {

        }
        internal async void Go()
        {
            Console.WriteLine("Лекция 10. Введение в параллелеризм:");
            Console.WriteLine("------------------");
            Console.WriteLine("Пункт 1:");
            ProcessNumberOfFiles();
            Console.WriteLine("------------------");
            Console.WriteLine("Пункт 2:");
            ProcessAllFilesInFolder();
            Console.WriteLine("------------------");
            Console.WriteLine("Готово.");
        }
        private void ProcessNumberOfFiles()
        {
            List<string> paths = new List<string>();
            int filesAmount = 0;
            while (filesAmount <= 0)
            {
                filesAmount = GetFilesAmount();
            }
            int iteration = 1;
            while (paths.Count < filesAmount)
            {
                string? path = GetFilePath(iteration);
                if (string.IsNullOrEmpty(path))
                {
                    Console.WriteLine($"Путь не был указан. Желаете создать тестовый файл (Да(Y)/Нет(N))?\nПо умолчанию будет выбран вариант \"Да\".");
                    string? answer = Console.ReadLine();
                    if (string.IsNullOrEmpty(answer) || answer.Contains("Да") || answer.Contains("Y") || answer.Contains("Д"))
                    {
                        path = CreateNewFile();
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (!File.Exists(path))
                {
                    Console.WriteLine($"Файл по выбранному пути не существует: {path}");
                    continue;
                }
                else
                {
                    //
                }
                Console.WriteLine($"Выбран путь: {path}");
                paths.Add(path);
                iteration++;
            }
            Console.WriteLine();
            Stopwatch sw_1 = Stopwatch.StartNew();
            string[] results = CountSpacesInFiles(paths).Result;
            sw_1.Stop();
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
            Console.WriteLine($"Общее время подсчёта {results.Length} файлов: {sw_1.Elapsed}");
        }
        private void ProcessAllFilesInFolder()
        {
            Console.WriteLine($"Введите путь к папке с текстовыми файлами.\nВнимание! Принимаются только файлы формата .txt!\nПо умолчанию будет выбран путь: {AppDomain.CurrentDomain.BaseDirectory}");
            string? path = Console.ReadLine();
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
            {
                path = AppDomain.CurrentDomain.BaseDirectory;
            }
            List<string> paths = new List<string>();
            foreach (string filePath in Directory.GetFiles(path))
            {
                if (filePath.Split('.').Last() == "txt")
                {
                    paths.Add(filePath);
                }
            }
            if (paths.Count == 0)
            {
                Console.WriteLine("Не обнаружено файлов для обработки.");
                return;
            }
            Stopwatch sw_2 = Stopwatch.StartNew();
            string[] results = CountSpacesInFiles(paths).Result;
            sw_2.Stop();
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
            Console.WriteLine($"Общее время подсчёта {results.Length} файлов: {sw_2.Elapsed}");
        }
        private string CreateNewFile()
        {
            Console.WriteLine($"Введите путь к папке, где будет создан файл.\nПо умолчанию будет выбран путь: {AppDomain.CurrentDomain.BaseDirectory}");
            string? path = Console.ReadLine();
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
            {
                path = AppDomain.CurrentDomain.BaseDirectory;
            }
            path += $"_{DateTime.Now.Ticks.ToString()}.txt";
            Random rnd = new Random();
            int messageIndex = rnd.Next(0, testMessages.Length - 1);
            File.WriteAllText(path, testMessages[messageIndex]);
            return path;
        }

        private int GetFilesAmount()
        {
            int amount = 0;
            Console.WriteLine("Сколько файлов хотите обработать (введите положительное целое число)?\nПо умолчанию будет выбрано число 3.");
            string? filesAmount = Console.ReadLine();
            if (int.TryParse(filesAmount, out amount))
            {
                return amount;
            }
            else
            {
                return 3;
            }
        }
        
        private string? GetFilePath(int fileNum)
        {
            Console.WriteLine($"Введите путь к файлу {fileNum} (абсолютный):");
            return Console.ReadLine();
        }

        // задача для подсчёта заданных символов
        private string CountSymbolsInFile(string path, char symbolToCount)
        {
            Stopwatch sw = Stopwatch.StartNew();
            int cnt = 0;
            using (StreamReader streamReader = new StreamReader(path))
            {
                int symbol;
                while ((symbol = streamReader.Read()) != -1)
                {
                    if (symbol == symbolToCount)
                    {
                        cnt++;
                    }
                }
            }
            sw.Stop();
            return $"{path}: {cnt}\nВремя выполнения подсчёта: {sw.Elapsed}";
        }
        private async Task<string[]> CountSpacesInFiles(List<string> paths)
        {
            List<Task<string>> tasks = new List<Task<string>>();
            foreach (var path in paths)
            {
                Task<string> t = Task.Run(() =>
                {
                    return CountSymbolsInFile(path, ' ');
                });
                tasks.Add(t);
            }

            string[] resultsArray = await Task.WhenAll(tasks);
            //foreach (var result in resultsArray)
            //{
            //    Console.WriteLine(result);
            //}
            return resultsArray;
        }
        string[] testMessages = {
            "Здесь мог бы быть любой текст. Поэтому я просто оставлю здесь это сообщение.",
            "Принятие проекта вызвало резонанс в медицинском сообществе. Ко второму чтению поправки доработали, но суть их осталась такой же. В третьем чтении документ приняли 11 ноября. Его поддержали фракции «Единая Россия» и ЛДПР. «Справедливая Россия» и КПРФ воздержались от поддержки, поскольку не увидели в поправках социальных гарантий для начинающих врачей.",
            "Для игры используется полная колода в 52 карты. Каждому игроку сдаётся по 13 карт. Иногда в вист играют двумя колодами, тогда каждый игрок получает по 26 карт. Колоду снимает противник — игрок, сидящий справа от сдающего. Раздавать карты сдающий начинает с игрока, сидящего слева. Карты раздаются по одной. Свою последнюю карту сдающий переворачивает и показывает всем игрокам — это и будет козырь.",
            "Чтобы стать хорошим игроком в вист, следует научиться запоминать ходы как противников, так и своего партнёра. Главное в висте — запомнить 26 карт своих и своего партнёра, порой карты приходится угадывать. Ходить следует строго по очереди. Иногда случается, что игрок, сидящий третьим от сделавшего первый ход, кладёт на стол свою карту раньше второго. В этом случае четвёртый играющий также имеет право сбросить карту раньше своего партнёра. Эту ошибку со стороны третьего игрока поправить уже нельзя."
        };
    }

}
