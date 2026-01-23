using HwSandbox.Abstractions;

namespace HwSandbox.Implementations
{
    internal class TableWriter: DataWriter
    {
        public string Name { get; private set; }
        public string Path { get; private set; }
        public string Type { get; private set; }
        public string FullPath { get; private set; }
        List<string[]>_valuesToWrite = new List<string[]>();
        
        public TableWriter(string tableName = "", string tablePath = "")
        {
             Name = tableName;
             Path = tablePath;
             Type = "csv";
        }
        public override void Add(string[] valuesString)
        {
            _valuesToWrite.Add(valuesString);
        }

        public override int Write()
        {
            if (string.IsNullOrEmpty(Name))
            {
                Name = $"{DateTime.Now.Ticks}";
            }
            if (string.IsNullOrEmpty(Path))
            {
                Path = AppDomain.CurrentDomain.BaseDirectory;
            }
            FullPath = $"{Path}{Name}.{Type}";
            try
            {
                using (StreamWriter logFile = new StreamWriter(FullPath, true, System.Text.Encoding.UTF8))
                {
                    for (int i = 0; i < _valuesToWrite.Count; i++)
                    {
                        for (int j = 0; j < _valuesToWrite[i].Length; j++)
                        {
                            logFile.Write(_valuesToWrite[i][j] + ";");
                        }
                        logFile.WriteLine();
                    }
                    logFile.Flush();
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Ошибка доступа: Нет прав на запись в заданную папку!\n"+ ex.Message);
                return -2;
            }
            catch (IOException ex)
            {
                Console.WriteLine("Ошибка ввода-вывода (доступ к файлу запрещён)\n" + ex.Message);
                return -3;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка:\n" + ex.Message);
                return -1;
            }
            return 0;
        }
    }
}
