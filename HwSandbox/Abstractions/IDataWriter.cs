namespace HwSandbox.Abstractions
{
    public interface IDataWriter
    {
        public string Name { get; }
        public string Path { get; }
        public string Type { get; }
        public string FullPath { get; }
        /// <summary>
        /// Добавление значений для последующей записи
        /// </summary>
        /// <param name="values"> массив добавляемых значений </param>
        public void Add(string[] values);
        /// <summary>
        /// Запись значений, добавленных через Add. Возвращает численный код результата выполнения.
        /// </summary>
        public int Write();
    }
}
