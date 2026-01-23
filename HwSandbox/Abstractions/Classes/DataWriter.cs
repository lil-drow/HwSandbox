using HwSandbox.Abstractions.Interfaces;
namespace HwSandbox.Abstractions
{
    public abstract class DataWriter: IWritable
    {
        public string Name { get; }
        public string Path { get; }
        public string Type { get; }
        public string FullPath { get; }
        /// <summary>
        /// Добавление значений для последующей записи
        /// </summary>
        /// <param name="values"> массив добавляемых значений </param>
        public abstract void Add(string[] values);
        /// <summary>
        /// Запись значений, добавленных через Add. Возвращает численный код результата выполнения.
        /// </summary>
        public abstract int Write();
    }
}
