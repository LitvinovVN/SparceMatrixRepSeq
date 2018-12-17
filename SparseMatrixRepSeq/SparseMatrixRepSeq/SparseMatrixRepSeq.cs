using System;
using System.IO;
using System.Threading.Tasks;
using Utilities;

namespace SparseMatrixRepSeqNamespace
{
    /// <summary>
    /// Разрежённая матрица с большим количеством повторяющихся элементов
    /// </summary>
    public class SparseMatrixRepSeq : IDisposable
    {
        #region Константы
        /// <summary>
        /// Размер в байтах одного элемента данных
        /// (тип double - 8 байт (64 бит))
        /// </summary>
        const int SIZE_OF_DATA_ELEMENT = 8;

        /// <summary>
        /// Размер в байтах одного элемента индекса
        /// (тип int - 4 байта (32 бита))
        /// </summary>
        const int SIZE_OF_INDEX_ELEMENT = 4;
        #endregion

        #region Сведения о файлах
        /// <summary>
        /// Путь к директории с файлами
        /// </summary>
        readonly string _path="";

        /// <summary>
        /// Имя файла без расширения
        /// </summary>
        readonly string _fileName;

        /// <summary>
        /// Расширение файла, содержащего ненулевые
        /// элементы матрицы за исключением
        /// повторяющихся последовательностей
        /// </summary>
        readonly string _fileExtensionData = "smrsdat";

        /// <summary>
        /// Расширение файла, содержащего индексы
        /// первых ненулевых элементов каждой строки
        /// за исключением повторяющихся последовательностей
        /// </summary>
        readonly string _fileExtensionRowIndex = "smrsrindx";
        
        /// <summary>
        /// Возвращает путь к файлу с данными
        /// </summary>
        public string GetPathToDataFile {
            get
            {
                return Path.Combine(_path, _fileName + "." + _fileExtensionData);
            }
        }

        /// <summary>
        /// Возвращает путь к файлу с индексами начала строк
        /// </summary>
        public string GetPathToRowIndexFile
        {
            get
            {
                return Path.Combine(_path, _fileName + "." + _fileExtensionRowIndex);
            }
        }
        

        /// <summary>
        /// Возвращает количество элементов в файле с данными
        /// </summary>
        public long GetNumElementsInDataFile
        {
            get
            {                
                return _fileStreamData.Length / SIZE_OF_DATA_ELEMENT;
            }
        }        
        #endregion

        

        #region Файловые потоки
        /// <summary>
        /// Файловый поток для доступа к файлу с данными
        /// </summary>
        private FileStream _fileStreamData;

        /// <summary>
        /// Файловый поток для доступа к файлу с индексами первых элементов строк
        /// </summary>
        private FileStream _fileStreamRowIndex;
        #endregion

        

        #region Конструкторы
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="fileName">Имя файла без расширения</param>
        public SparseMatrixRepSeq(string fileName)
        {
            _fileName = fileName;

            FileSreamsInit();            
        }
        #endregion
               

        /// <summary>
        /// Инициализация файловых потоков
        /// </summary>
        private void FileSreamsInit()
        {
            _fileStreamData = new FileStream(GetPathToDataFile,
                              FileMode.OpenOrCreate,
                              FileAccess.ReadWrite,
                              FileShare.Read);

            _fileStreamRowIndex = new FileStream(GetPathToRowIndexFile,
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
            _fileStreamData.Close();
            File.Delete(GetPathToDataFile);
            _fileStreamData = new FileStream(GetPathToDataFile,
                              FileMode.OpenOrCreate,
                              FileAccess.ReadWrite,
                              FileShare.Read);
        }

        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        public void Dispose()
        {
            _fileStreamData.Close();
            _fileStreamRowIndex.Close();
        }

        /// <summary>
        /// Считывает индекс первого ненулевого элемента
        /// из указанной строки за исключением повторяющихся последовательностей
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public async Task<int> ReadRowIndexAsync(int rowIndex)
        {
            int offset = rowIndex * SIZE_OF_INDEX_ELEMENT;
            long p = _fileStreamData.Seek(offset, SeekOrigin.Begin);

            byte[] bytes = new byte[SIZE_OF_INDEX_ELEMENT];
            try
            {
                int res = await _fileStreamData.ReadAsync(bytes, 0, SIZE_OF_INDEX_ELEMENT);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Exception");
            }
            int result = Converters.ConvertByteArrayToInt(bytes);
            return result;
        }        

        /// <summary>
        /// Возвращает количество элементов в указанной строке
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public async Task<int> ReadRowElementsCountAsync(int rowIndex)
        {
            int offset = rowIndex * SIZE_OF_INDEX_ELEMENT;
            long p = _fileStreamRowIndex.Seek(offset, SeekOrigin.Begin);

            byte[] bytes = new byte[SIZE_OF_INDEX_ELEMENT*2];
            try
            {
                int res = await _fileStreamRowIndex.ReadAsync(bytes, 0, bytes.Length);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Exception");
            }
            int curRowIndex  = Converters.ConvertByteArrayToInt(new byte[] { bytes[0], bytes[1], bytes[2], bytes[3] });
            int nextRowIndex = Converters.ConvertByteArrayToInt(new byte[] { bytes[4], bytes[5], bytes[6], bytes[7] });
            int numRowElements = nextRowIndex - curRowIndex;
            return numRowElements;
        }

        /// <summary>
        /// Возвращает количество элементов в указанной последовательности строк
        /// </summary>
        /// <param name="rowStartIndex">Индекс первой строки</param>
        /// <param name="numRows">Количество строк</param>
        /// <returns></returns>
        public async Task<int[]> ReadRowsElementsCountAsync(int rowStartIndex, int numRows)
        {
            int offset = rowStartIndex * SIZE_OF_INDEX_ELEMENT;
            long p = _fileStreamRowIndex.Seek(offset, SeekOrigin.Begin);

            byte[] bytes = new byte[SIZE_OF_INDEX_ELEMENT * (numRows+1)];
            try
            {
                int res = await _fileStreamRowIndex.ReadAsync(bytes, 0, bytes.Length);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Exception");
            }

            int[] numRowsElements = new int[numRows];
            for(int i = 0; i < numRows; i++)
            {
                int rows_offset = i * SIZE_OF_INDEX_ELEMENT;
                int curRowIndex  = Converters.ConvertByteArrayToInt(new byte[] { bytes[rows_offset+0], bytes[rows_offset+1], bytes[rows_offset+2], bytes[rows_offset+3] });
                int nextRowIndex = Converters.ConvertByteArrayToInt(new byte[] { bytes[rows_offset+4], bytes[rows_offset+5], bytes[rows_offset+6], bytes[rows_offset+7] });
                numRowsElements[i] = nextRowIndex - curRowIndex;
            }
            
            return numRowsElements;
        }

        /// <summary>
        /// Записывает индекс первого ненулевого элемента указанной строки
        /// </summary>
        /// <param name="rowIndex">Номер строки</param>
        /// <param name="dataIndex">Индекс элемента в файле с данными</param>
        /// <returns></returns>
        public async Task WriteRowIndexAsync(int rowIndex, int dataIndex)
        {
            long offset = rowIndex * sizeof(int);
            _fileStreamRowIndex.Seek(offset, SeekOrigin.Begin);
            byte[] bytes = Converters.ConvertIntToByteArray(dataIndex);
            await _fileStreamRowIndex.WriteAsync(bytes, 0, bytes.Length);
        }
                
        /// <summary>
        /// Записывает один элемент данных в конец файла с данными
        /// </summary>
        /// <param name="element"></param>
        public async Task WriteDataAsync(double element)
        {
            _fileStreamData.Seek(0, SeekOrigin.End);
            byte[] bytes = Converters.ConvertDoubleToByteArray(element);
            await _fileStreamData.WriteAsync(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Перезаписывает (заменяет) один элемент данных в указанной позиции
        /// </summary>
        /// <param name="element"></param>
        public async Task RewriteDataAsync(double element, int index)
        {
            _fileStreamData.Seek(index * SIZE_OF_DATA_ELEMENT, SeekOrigin.Begin);
            byte[] bytes = Converters.ConvertDoubleToByteArray(element);
            await _fileStreamData.WriteAsync(bytes, 0, bytes.Length);
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
            byte[] bytes = new byte[(GetNumElementsInDataFile - index + insertingElements.Length) * SIZE_OF_DATA_ELEMENT];

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
                long p = _fileStreamData.Seek(offset, SeekOrigin.Begin);
                int res = await _fileStreamData.ReadAsync(bytes,
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
                long p = _fileStreamData.Seek(offset, SeekOrigin.Begin);
                await _fileStreamData.WriteAsync(bytes, 0, bytes.Length);
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
            long p = _fileStreamData.Seek(offset, SeekOrigin.Begin);

            byte[] bytes = new byte[SIZE_OF_DATA_ELEMENT * numElements];
            try
            {
                int res = await _fileStreamData.ReadAsync(bytes, 0, bytes.Length);
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
            long p = _fileStreamData.Seek(offset, SeekOrigin.Begin);

            byte[] bytes = new byte[SIZE_OF_DATA_ELEMENT];
            try
            {
                int res = await _fileStreamData.ReadAsync(bytes, 0, SIZE_OF_DATA_ELEMENT);
            }
            catch(Exception exc)
            {
                Console.WriteLine("Exception");
            }
            double result = Converters.ConvertByteArrayToDouble(bytes);
            return result;
        }           

        
    }
}
