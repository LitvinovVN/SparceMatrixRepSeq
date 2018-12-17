using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace SparseMatrixRepSeqNamespace
{
    /// <summary>
    /// Двоичный файл с произвольным доступом
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class FileBinaryWithRandomAccessDouble : IDisposable
    {
        /// <summary>
        /// Файловый поток для доступа к файлу с данными
        /// </summary>
        private FileStream _fileStream;

        /// <summary>
        /// Размер в байтах одного элемента данных
        /// (тип double - 8 байт (64 бит))
        /// </summary>
        const int SIZE_OF_DATA_ELEMENT = 8;

        /// <summary>
        /// Возвращает путь к файлу
        /// </summary>
        public string GetPathToFile { get; }

        /// <summary>
        /// Возвращает количество элементов в файле
        /// </summary>
        public long GetNumElementsInFile
        {
            get
            {
                return _fileStream.Length / SIZE_OF_DATA_ELEMENT;
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public FileBinaryWithRandomAccessDouble(string filePath)
        {
            GetPathToFile = filePath;
            _fileStream = new FileStream(GetPathToFile,
                              FileMode.OpenOrCreate,
                              FileAccess.ReadWrite,
                              FileShare.Read);
        }

        /// <summary>
        /// Очищает файл с данными
        /// (стирает имеющийся файл и создаёт новый)
        /// </summary>
        /// <returns></returns>
        public void ClearDataFile()
        {
            _fileStream.Close();
            File.Delete(GetPathToFile);
            _fileStream = new FileStream(GetPathToFile,
                              FileMode.OpenOrCreate,
                              FileAccess.ReadWrite,
                              FileShare.Read);
        }

        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        public void Dispose()
        {
            _fileStream.Close();
        }


        /// <summary>
        /// Записывает один элемент данных в конец файла с данными
        /// </summary>
        /// <param name="element"></param>
        public async Task WriteDataAsync(double element)
        {
            _fileStream.Seek(0, SeekOrigin.End);
            byte[] bytes = Converters.ConvertDoubleToByteArray(element);
            await _fileStream.WriteAsync(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Перезаписывает (заменяет) один элемент данных в указанной позиции
        /// </summary>
        /// <param name="element"></param>
        public async Task RewriteDataAsync(double element, int index)
        {
            _fileStream.Seek(index * SIZE_OF_DATA_ELEMENT, SeekOrigin.Begin);
            byte[] bytes = Converters.ConvertDoubleToByteArray(element);
            await _fileStream.WriteAsync(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Вставляет один элемент данных в указанную позицию.
        /// При этом все элементы, начиная с указанной позиции,
        /// смещаются на 1
        /// </summary>
        /// <param name="insertingElement"></param>
        public async Task InsertDataAsync(double insertingElement, int index)
        {
            await InsertDataAsync(new double[] { insertingElement }, index);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="insertingElements"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task InsertDataAsync(double[] insertingElements, int index)
        {
            // 1. Выделяем память для хранения вставляемых данных
            //           V = 2              Length = 6
            // | 0 | 1 | 2 | 3 | 4 | 5 |
            // | 0 | 1 | 2 | 3 | 4 | 5 | 6 |
            //         | Length - V + 1 = 6 - 2 + 1 = 5
            byte[] bytes = new byte[(GetNumElementsInFile - index + insertingElements.Length) * SIZE_OF_DATA_ELEMENT];

            // 2. В первых байтах размещаем добавляемые элементы
            byte[] insertingElementBytes = Converters.ConvertDoubleArrayToByteArray(insertingElements);
            for (int i = 0; i < SIZE_OF_DATA_ELEMENT * insertingElements.Length; i++)
            {
                bytes[i] = insertingElementBytes[i];
            }

            // 3. В остальные элементы добавляем считанные из файла данные, начиная с индекса вставки            
            int offset = index * SIZE_OF_DATA_ELEMENT;
            try
            {
                long p = _fileStream.Seek(offset, SeekOrigin.Begin);
                int res = await _fileStream.ReadAsync(bytes,
                    SIZE_OF_DATA_ELEMENT * insertingElements.Length,
                    bytes.Length - SIZE_OF_DATA_ELEMENT * insertingElements.Length);
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


        /// <summary>
        /// Возвращает элементы данных из файла с данными
        /// </summary>
        /// <param name="index">Индекс первого выбираемого элемента данных</param>
        /// <param name="numElements">Количество выбираемых элементов</param>
        /// <returns></returns>
        private async Task<double[]> GetElementsFromDataFileAsync(int index, int numElements)
        {
            int offset = index * SIZE_OF_DATA_ELEMENT;
            long p = _fileStream.Seek(offset, SeekOrigin.Begin);

            byte[] bytes = new byte[SIZE_OF_DATA_ELEMENT * numElements];
            try
            {
                int res = await _fileStream.ReadAsync(bytes, 0, bytes.Length);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Exception");
            }
            double[] result = Converters.ConvertByteArrayToDoubleArray(bytes);
            return result;
        }

        /// <summary>
        /// Считывает один элемент данных из указанной позиции
        /// </summary>
        /// <param name="index">Позиция считываемого элемента</param>
        /// <returns></returns>
        public async Task<double> ReadDataAsync(int index)
        {
            int offset = index * SIZE_OF_DATA_ELEMENT;
            long p = _fileStream.Seek(offset, SeekOrigin.Begin);

            byte[] bytes = new byte[SIZE_OF_DATA_ELEMENT];
            try
            {
                int res = await _fileStream.ReadAsync(bytes, 0, SIZE_OF_DATA_ELEMENT);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Exception");
            }
            double result = Converters.ConvertByteArrayToDouble(bytes);
            return result;
        }
    }
}
