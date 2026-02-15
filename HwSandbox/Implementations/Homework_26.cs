using HwSandbox.Abstractions;
using System.Diagnostics;
using System.Reflection;
using Newtonsoft.Json;

namespace HwSandbox.Implementations;

/*
   Рефлексия и её применение
   Цель:
   Написать свой класс-сериализатор данных любого типа в формат CSV, сравнение его быстродействия с типовыми механизмами серализации.
   Полезно для изучения возможностей Reflection, а может и для применения данного класса в будущем.
   
   Описание/Пошаговая инструкция выполнения домашнего задания:
   Основное задание:
   
   Написать сериализацию свойств или полей класса в строку
   Проверить на классе: class F { int i1, i2, i3, i4, i5; Get() => new F(){ i1 = 1, i2 = 2, i3 = 3, i4 = 4, i5 = 5 }; }
   Замерить время до и после вызова функции (для большей точности можно сериализацию сделать в цикле 100-100000 раз)
   Вывести в консоль полученную строку и разницу времен
   Отправить в чат полученное время с указанием среды разработки и количества итераций
   Замерить время еще раз и вывести в консоль сколько потребовалось времени на вывод текста в консоль
   Провести сериализацию с помощью каких-нибудь стандартных механизмов (например в JSON)
   И тоже посчитать время и прислать результат сравнения
   Написать десериализацию/загрузку данных из строки (ini/csv-файла) в экземпляр любого класса
   Замерить время на десериализацию
   Общий результат прислать в чат с преподавателем в системе в таком виде:
   Сериализуемый класс: class F { int i1, i2, i3, i4, i5;}
   код сериализации-десериализации: ...
   количество замеров: 1000 итераций
   мой рефлекшен:
   Время на сериализацию = 100 мс
   Время на десериализацию = 100 мс
   стандартный механизм (NewtonsoftJson):
   Время на сериализацию = 100 мс
   Время на десериализацию = 100 мс
   
   Критерии оценки:
   5 баллов: 1-5 пункты
   2 балла: 6-8 пункты
   2 балла: 9-11 пункты
   1 балл: CodeStyle, грамотная архитектура, всё замечания проверяющего исправлены
   
   Минимальное количество баллов для сдачи: 6
 */

public class Homework_26 : Homework
{
    public override void Go()
    {
        Console.WriteLine("Лекция 26. Рефлексия и её применение:");
        Console.WriteLine("------------------");
        const int iterations = 100000; 
        var testObject = F.Get();
        var csvSerializer = new CsvSerializer();
        
        Console.WriteLine($"Сериализуемый класс: class F {{ int i1, i2, i3, i4, i5; }}");
        Console.WriteLine($"Количество замеров: {iterations} итераций");
        Console.WriteLine(new string('-', 50));

        // замер времени сериализации классом-сериализатором
        var stopwatch = Stopwatch.StartNew();
        string serializedData = string.Empty;
        
        for (int i = 0; i < iterations; i++)
        {
            serializedData = csvSerializer.Serialize(testObject);
        }
        
        stopwatch.Stop();
        var mySerializationTime = stopwatch.ElapsedMilliseconds;
        Console.WriteLine($"Мой рефлекшен:");
        Console.WriteLine($"  Время на сериализацию = {mySerializationTime} мс");
        Console.WriteLine($"  Результат: {serializedData}");

        // замер времени на вывод в консоль
        stopwatch.Restart();
        Console.WriteLine($"  Вывод в консоль: {serializedData}");
        stopwatch.Stop();
        Console.WriteLine($"  Время на вывод в консоль = {stopwatch.ElapsedMilliseconds} мс");
        Console.WriteLine();

        // замер времени десериализации классом-сериализатором
        stopwatch.Restart();
        F deserializedObject = null;
        
        for (int i = 0; i < iterations; i++)
        {
            deserializedObject = csvSerializer.Deserialize<F>(serializedData);
        }
        
        stopwatch.Stop();
        var myDeserializationTime = stopwatch.ElapsedMilliseconds;
        
        Console.WriteLine($"  Время на десериализацию = {myDeserializationTime} мс");
        Console.WriteLine($"  Результат: i1={deserializedObject.i1}, i2={deserializedObject.i2}, i3={deserializedObject.i3}, i4={deserializedObject.i4}, i5={deserializedObject.i5}");
        Console.WriteLine();

        // замер времени JSON сериализации с Newtonsoft.Json
        stopwatch.Restart();
        string jsonData = string.Empty;
        
        for (int i = 0; i < iterations; i++)
        {
            jsonData = JsonConvert.SerializeObject(testObject);
        }
        
        stopwatch.Stop();
        var jsonSerializationTime = stopwatch.ElapsedMilliseconds;
        
        Console.WriteLine($"Стандартный механизм (NewtonsoftJson):");
        Console.WriteLine($"  Время на сериализацию = {jsonSerializationTime} мс");
        Console.WriteLine($"  Результат: {jsonData}");
        Console.WriteLine();

        // замер времени JSON десериализации с Newtonsoft.Json
        stopwatch.Restart();
        F jsonDeserializedObject = null;
        
        for (int i = 0; i < iterations; i++)
        {
            jsonDeserializedObject = JsonConvert.DeserializeObject<F>(jsonData);
        }
        
        stopwatch.Stop();
        var jsonDeserializationTime = stopwatch.ElapsedMilliseconds;
        
        Console.WriteLine($"  Время на десериализацию = {jsonDeserializationTime} мс");
        Console.WriteLine($"  Результат: i1={jsonDeserializedObject.i1}, i2={jsonDeserializedObject.i2}, i3={jsonDeserializedObject.i3}, i4={jsonDeserializedObject.i4}, i5={jsonDeserializedObject.i5}");
        Console.WriteLine(new string('-', 50));

        // сводка результатов
        Console.WriteLine("\nСВОДКА РЕЗУЛЬТАТОВ:");
        Console.WriteLine($"Среда разработки: {Environment.OSVersion}, .NET {Environment.Version}");
        Console.WriteLine($"Количество замеров: {iterations} итераций");
  
        Console.WriteLine("\nМой рефлекшен:");
        Console.WriteLine($"  Время на сериализацию = {mySerializationTime} мс");
        Console.WriteLine($"  Время на десериализацию = {myDeserializationTime} мс");
    
        Console.WriteLine("\nСтандартный механизм (NewtonsoftJson):");
        Console.WriteLine($"  Время на сериализацию = {jsonSerializationTime} мс");
        Console.WriteLine($"  Время на десериализацию = {jsonDeserializationTime} мс");
        
        // сравнение производительности
        Console.WriteLine($"\nСравнение производительности:");
        Console.WriteLine($"  Сериализация: рефлекшен {(mySerializationTime > jsonSerializationTime ? "медленнее" : "быстрее")} в {Math.Round((double)Math.Max(mySerializationTime, jsonSerializationTime) / Math.Min(mySerializationTime, jsonSerializationTime), 2)} раз");
        Console.WriteLine($"  Десериализация: рефлекшен {(myDeserializationTime > jsonDeserializationTime ? "медленнее" : "быстрее")} в {Math.Round((double)Math.Max(myDeserializationTime, jsonDeserializationTime) / Math.Min(myDeserializationTime, jsonDeserializationTime), 2)} раз");
        Console.WriteLine("\nГотово.");
    }

    public class F
    {
        public int i1 { get; set; }
        public int i2 { get; set; }
        public int i3 { get; set; }
        public int i4 { get; set; }
        public int i5 { get; set; }

        public static F Get() => new F() { i1 = 1, i2 = 2, i3 = 3, i4 = 4, i5 = 5 };
    }

    // класс-сериализатор CSV 
    public class CsvSerializer
    {
        public string Serialize<T>(T obj)
        {
            if (obj == null) return string.Empty;

            var type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            var values = properties.Select(p => p.GetValue(obj)?.ToString() ?? "");
            return string.Join(",", values);
        }

        public T Deserialize<T>(string csvData) where T : class, new()
        {
            if (string.IsNullOrEmpty(csvData)) return new T();

            var obj = new T();
            var type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            var values = csvData.Split(',');
            
            for (int i = 0; i < Math.Min(properties.Length, values.Length); i++)
            {
                var property = properties[i];
                if (property.CanWrite)
                {
                    try
                    {
                        var value = Convert.ChangeType(values[i], property.PropertyType);
                        property.SetValue(obj, value);
                    }
                    catch
                    {
                        Console.WriteLine($"Ошибка конвертации: {values[i]}");
                    }
                }
            }
            return obj;
        }
    }
}