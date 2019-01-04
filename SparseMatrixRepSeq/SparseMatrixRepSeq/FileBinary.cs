using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SparseMatrixRepSeqNamespace
{
    /// <summary>
    /// Абстрактный класс для работы с бинарным файлом
    /// </summary>
    public abstract class FileBinary : IDisposable
    {
        #region Закрытые поля
        /// <summary>
        /// Файловый поток для доступа к файлу с данными
        /// </summary>
        protected FileStream _fileStream;
        #endregion

        #region Открытые свойства
        /// <summary>
        /// Возвращает путь к файлу
        /// </summary>
        public string GetPathToFile { get; }

        /// <summary>
        /// Размер в байтах одного элемента данных
        /// (тип double - 8 байт (64 бит), int - 4 байта (32 бита))
        /// </summary>
        public int GetSizeOfDataElement { get; }

        /// <summary>
        /// Возвращает количество элементов в файле
        /// </summary>
        public long GetNumElementsInFile
        {
            get
            {
                return _fileStream.Length / GetSizeOfDataElement;
            }
        }
        #endregion

        #region Конструкторы
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <param name="sizeOfDataElement">Размер одного элемента данных в байтах</param>
        public FileBinary(string filePath, int sizeOfDataElement)
        {
            GetPathToFile = filePath;
            GetSizeOfDataElement = sizeOfDataElement;
            try
            {
                _fileStream = new FileStream(GetPathToFile,
                              FileMode.OpenOrCreate,
                              FileAccess.ReadWrite,
                              FileShare.Read);
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Исключение при открытии файла: {exc.Message}");
            }
        }
        #endregion

        #region Освобождение ресурсов
        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        public void Dispose()
        {
            _fileStream.Close();
        }
        #endregion

        #region Чтение данных
        /// <summary>
        /// Возвращает элементы данных из файла с данными
        /// </summary>
        /// <param name="index">Индекс первого выбираемого элемента данных</param>
        /// <param name="numElements">Количество выбираемых элементов</param>
        /// <returns></returns>
        public async Task<byte[]> ReadAsync(int index, int numElements)
        {
            if ((index + numElements) > GetNumElementsInFile)
            {
                throw new Exception("Исключение! Попытка чтения по несуществующему индексу!");
            }

            int offset = index * GetSizeOfDataElement;
            long p = _fileStream.Seek(offset, SeekOrigin.Begin);

            byte[] bytes = new byte[GetSizeOfDataElement * numElements];
            try
            {
                int res = await _fileStream.ReadAsync(bytes, 0, bytes.Length);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Exception");
            }
            
            return bytes;
        }

        /// <summary>
        /// Считывает один элемент данных из указанной позиции
        /// </summary>
        /// <param name="index">Позиция считываемого элемента</param>
        /// <returns></returns>
        public async Task<byte[]> ReadAsync(int index)
        {
            byte[] result = await ReadAsync(index, 1);
            return result;
        }

        /// <summary>
        /// Считывает все данные из файла
        /// </summary>
        /// <param name="index">Позиция считываемого элемента</param>
        /// <returns></returns>
        public async Task<byte[]> ReadAsync()
        {            
            byte[] result = await ReadAsync(0, (int)GetNumElementsInFile);
            return result;
        }
        #endregion

        #region Добавление данных
        /// <summary>
        /// Записывает один данные в конец файла
        /// </summary>
        /// <param name="bytes"></param>
        public async Task WriteDataAsync(byte[] bytes)
        {
            _fileStream.Seek(0, SeekOrigin.End);            
            await _fileStream.WriteAsync(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Вставляет массив элементов данных в указанную позицию.
        /// При этом все элементы, начиная с указанной позиции,
        /// смещаются на число вставляемых элементов
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task InsertDataAsync(byte[] insertingElements, int index)
        {
            // 1. Выделяем память для хранения вставляемых данных
            //           V = 2              Length = 6
            // | 0 | 1 | 2 | 3 | 4 | 5 |
            // | 0 | 1 | 2 | 3 | 4 | 5 | 6 |
            //         | Length - V + 1 = 6 - 2 + 1 = 5
            byte[] bytes = new byte[(GetNumElementsInFile - index + insertingElements.Length / GetSizeOfDataElement) * GetSizeOfDataElement];

            // 2. В первых байтах размещаем добавляемые элементы
            byte[] insertingElementBytes = insertingElements;
            for (int i = 0; i < insertingElements.Length; i++)
            {
                bytes[i] = insertingElementBytes[i];
            }

            // 3. В остальные элементы добавляем считанные из файла данные, начиная с индекса вставки            
            int offset = index * GetSizeOfDataElement;
            try
            {
                long p = _fileStream.Seek(offset, SeekOrigin.Begin);
                int res = await _fileStream.ReadAsync(bytes,
                    insertingElements.Length,
                    bytes.Length - insertingElements.Length);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Exception. _fileStreamData.ReadAsync");
            }

            // 4. Записываем сформированный массив байт в файл, начиная с указанного индекса            
            try
            {
                long p = _fileStream.Seek(offset, SeekOrigin.Begin);
                await _fileStream.WriteAsync(bytes, 0, bytes.Length);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Exception. _fileStreamData.WriteAsync");
            }
        }
        #endregion

        #region Редактирование данных
        /// <summary>
        /// Перезаписывает (заменяет) элементы данных в указанной позиции
        /// </summary>
        /// <param name="bytes"></param>
        public async Task EditDataAsync(byte[] bytes, int index)
        {
            _fileStream.Seek(index * GetSizeOfDataElement, SeekOrigin.Begin);            
            await _fileStream.WriteAsync(bytes, 0, bytes.Length);
        }
        #endregion

        #region Удаление данных
        /// <summary>
        /// Очищает файл с данными
        /// (стирает имеющийся файл и создаёт новый)
        /// </summary>
        /// <returns></returns>
        public void ClearFile()
        {
            _fileStream.SetLength(0);
        }

        /// <summary>
        /// Удаляет элемент по указанному индексу
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task RemoveAsync(int index)
        {
            await RemoveAsync(index, 1);
        }

        /// <summary>
        /// Удаляет элементы данных,
        /// начиная с указанного индекса
        /// </summary>        
        /// <param name="index"></param>
        /// <param name="numRemovingElements"></param>        
        /// <returns></returns>
        public async Task RemoveAsync(int index, int numRemovingElements)
        {
            // 0. Проверка выхода за границы файла
            if ((index + numRemovingElements) > GetNumElementsInFile)
            {
                throw new Exception("Выход за границы файла");
            }

            // 1. Выделяем память для хранения оставляемых данных
            //           V = 2              Length = 6
            // | 0 | 1 | 2 | 3 | 4 | 5 |
            // | 0 | 1 | 3 | 4 | 5 |
            //         | Length - V = 6 - 2 - 1 = 3
            byte[] bytes = new byte[(GetNumElementsInFile - index - numRemovingElements) * GetSizeOfDataElement];


            // 2. Считываем из файла данные, которые остаются, начиная с индекса вставки + кол-во удаляемых элементов            
            try
            {
                int offset = (index + numRemovingElements) * GetSizeOfDataElement;
                long p = _fileStream.Seek(offset, SeekOrigin.Begin);
                int res = await _fileStream.ReadAsync(bytes, 0, bytes.Length);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Exception. _fileStream.ReadAsync");
            }

            // 3. Записываем сформированный массив байт в файл, начиная с указанного индекса            
            try
            {
                int offset = index * GetSizeOfDataElement;
                long p = _fileStream.Seek(offset, SeekOrigin.Begin);
                await _fileStream.WriteAsync(bytes, 0, bytes.Length);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Exception. _fileStream.WriteAsync");
            }

            // 4. Удаляем оставшиеся данные в конце файла
            _fileStream.SetLength(_fileStream.Length - numRemovingElements * GetSizeOfDataElement);
        }
        #endregion

        #region Вывод содержимого файла
        /// <summary>
        /// Вывод на консоль содержимого файла
        /// </summary>
        public void PrintFileStatisticsToConsole()
        {
            Console.WriteLine($"----------------Start FileStatistics-------------------");
            Console.WriteLine($"Имя файла: {GetPathToFile}");

            Console.WriteLine($"Существование файла: {File.Exists(GetPathToFile)}");

            if (File.Exists(GetPathToFile))
            {
                Console.WriteLine($"Размер файла: {_fileStream.Length} байт");
                Console.WriteLine($"Количество элементов данных: {GetNumElementsInFile}");                               
            }

            Console.WriteLine($"-------------End FileStatistics----------------------");
        }
        #endregion
    }
}
