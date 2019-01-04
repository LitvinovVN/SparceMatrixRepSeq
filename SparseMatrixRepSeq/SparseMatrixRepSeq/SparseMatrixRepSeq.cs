using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace SparseMatrixRepSeqNamespace
{
    /// <summary>
    /// Разрежённая матрица с большим количеством повторяющихся элементов
    /// </summary>
    public class SparseMatrixRepSeq : IDisposable
    {
        #region Сведения о файлах
        #region Имена файлов
        /// <summary>
        /// Путь к папке с проектом
        /// </summary>
        readonly string _pathToProjectDir = "";

        /// <summary>
        /// Имя файла, содержащего общие
        /// сведения о матрице
        /// (число строк, число столбцов и пр.)
        /// </summary>
        readonly string _fileDescriptionName = "smrs.descr";

        /// <summary>
        /// Имя файла, содержащего ненулевые
        /// элементы матрицы за исключением
        /// повторяющихся последовательностей
        /// </summary>
        readonly string _fileDataName = "smrs.dat";

        /// <summary>
        /// Имя файла, содержащего индексы
        /// первых ненулевых элементов каждой строки матрицы
        /// за исключением повторяющихся последовательностей
        /// </summary>
        readonly string _fileRowIndexName = "smrs.rindx";

        /// <summary>
        /// Имя файла, содержащего индексы столбцов
        /// ненулевых элементов каждой строки матрицы
        /// за исключением повторяющихся последовательностей
        /// </summary>
        readonly string _fileColIndexName = "smrs.cindx";

        /// <summary>
        /// Имя файла, содержащего элементы повторяющихся
        /// последовательностей
        /// </summary>
        readonly string _fileSeqDataName = "smrs.seqdat";

        /// <summary>
        /// Имя файла, содержащего индексы первых элементов
        /// повторяющихся последовательностей
        /// </summary>
        readonly string _fileSeqIndexName = "smrs.seqindx";

        /// <summary>
        /// Имя файла, содержащего координаты первых элементов
        /// повторяющихся последовательностей
        /// </summary>
        readonly string _fileSeqCoordName = "smrs.seqcoord";

        /// <summary>
        /// Имя файла, содержащего координаты первых элементов
        /// повторяющихся последовательностей,
        /// расположенных в виде ленты
        /// </summary>
        readonly string _fileBandCoordName = "smrs.bandcoord";
        #endregion
        #region Пути к файлам
        /// <summary>
        /// Возвращает путь к файлу с общим описанием матрицы
        /// </summary>
        public string GetPathToDescriptionFile
        {
            get
            {
                return Path.Combine(_pathToProjectDir, _fileDescriptionName);
            }
        }

        /// <summary>
        /// Возвращает путь к файлу с данными
        /// </summary>
        public string GetPathToDataFile {
            get
            {
                return Path.Combine(_pathToProjectDir, _fileDataName);
            }
        }

        /// <summary>
        /// Возвращает путь к файлу с индексами начала строк
        /// </summary>
        public string GetPathToRowIndexFile
        {
            get
            {
                return Path.Combine(_pathToProjectDir, _fileRowIndexName);
            }
        }

        /// <summary>
        /// Возвращает путь к файлу с индексами столбцов
        /// </summary>
        public string GetPathToColIndexFile
        {
            get
            {
                return Path.Combine(_pathToProjectDir, _fileColIndexName);
            }
        }                
        
        /// <summary>
        /// Возвращает путь к файлу, содержащему элементы повторяющихся
        /// последовательностей
        /// </summary>
        public string GetPathToSeqDataFile
        {
            get
            {
                return Path.Combine(_pathToProjectDir, _fileSeqDataName);
            }
        }

        /// <summary>
        /// Возвращает путь к файлу,
        /// содержащему индексы первых элементов
        /// повторяющихся последовательностей
        /// </summary>
        public string GetPathToSeqIndexFile
        {
            get
            {
                return Path.Combine(_pathToProjectDir, _fileSeqIndexName);
            }
        }                

        /// <summary>
        /// Возвращает путь к файлу,
        /// содержащему координаты первых элементов
        /// повторяющихся последовательностей
        /// </summary>
        public string GetPathToSeqCoordFile
        {
            get
            {
                return Path.Combine(_pathToProjectDir, _fileSeqCoordName);
            }
        }

        /// <summary>
        /// Возвращает путь к файлу, содержащему
        /// координаты первых элементов
        /// повторяющихся последовательностей,
        /// расположенных в виде ленты
        /// </summary>
        public string GetPathToBandCoordFile
        {
            get
            {
                return Path.Combine(_pathToProjectDir, _fileBandCoordName);
            }
        }
        #endregion
        #endregion        

        #region Файловые потоки
        /// <summary>
        /// Файловый поток для доступа к файлу с данными
        /// </summary>
        private FileBinaryInt _fileBinaryDescription;

        /// <summary>
        /// Файловый поток для доступа к файлу с данными
        /// </summary>
        private FileBinaryDouble _fileBinaryData;

        /// <summary>
        /// Файловый поток для доступа к файлу с индексами первых элементов строк
        /// </summary>
        private FileBinaryInt _fileBinaryRowIndex;

        /// <summary>
        /// Файловый поток для доступа к файлу с индексами столбцов элементов
        /// </summary>
        private FileBinaryInt _fileBinaryColIndex;

        /// <summary>
        /// Файловый поток для доступа к файлу,
        /// содержащему элементы повторяющихся
        /// последовательностей
        /// </summary>
        private FileBinaryDouble _fileBinarySeqData;
        
        /// <summary>
        /// Файловый поток для доступа к файлу,
        /// содержащему индексы первых элементов
        /// повторяющихся последовательностей
        /// </summary>
        private FileBinaryInt _fileBinarySeqIndex;

        /// <summary>
        /// Файловый поток для доступа к файлу,
        /// содержащему координаты первых элементов
        /// повторяющихся последовательностей
        /// </summary>
        private FileBinaryInt _fileBinarySeqCoord;
              

        /// <summary>
        /// Файловый поток для доступа к файлу,
        /// содержащему координаты первых элементов
        /// повторяющихся последовательностей,
        /// расположенных в виде ленты
        /// </summary>
        private FileBinaryInt _fileBinaryBandCoord;        
        #endregion



        #region Конструкторы
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="pathToProjectDir">Путь к папке с проектом</param>
        public SparseMatrixRepSeq(string pathToProjectDir)
        {
            if(!Directory.Exists(pathToProjectDir))
            {
                Directory.CreateDirectory(pathToProjectDir);
            }
            _pathToProjectDir = pathToProjectDir;

            _fileBinaryDescription = new FileBinaryInt(GetPathToDescriptionFile);
            _fileBinaryData        = new FileBinaryDouble(GetPathToDataFile);
            _fileBinaryRowIndex    = new FileBinaryInt(GetPathToRowIndexFile);
            _fileBinaryColIndex    = new FileBinaryInt(GetPathToColIndexFile);
            _fileBinarySeqData           = new FileBinaryDouble(GetPathToSeqDataFile);
            _fileBinarySeqIndex    = new FileBinaryInt(GetPathToSeqIndexFile);
            _fileBinarySeqCoord    = new FileBinaryInt(GetPathToSeqCoordFile);
            _fileBinaryBandCoord   = new FileBinaryInt(GetPathToBandCoordFile);
        }
        #endregion



        #region Очистка данных и освобождение ресурсов
        /// <summary>
        /// Очищает файлы с данными
        /// </summary>
        /// <returns></returns>
        public async Task ClearAsync()
        {
            _fileBinaryDescription.ClearFile();
            _fileBinaryData.ClearFile();
            _fileBinaryRowIndex.ClearFile();
            _fileBinaryColIndex.ClearFile();
            _fileBinarySeqData.ClearFile();
            _fileBinarySeqIndex.ClearFile();
            _fileBinarySeqCoord.ClearFile();
            _fileBinaryBandCoord.ClearFile();

            await SetNumRowsAsync(0);
            await SetNumColsAsync(0);
        }                

        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        public void Dispose()
        {
            _fileBinaryDescription.Dispose();
            _fileBinaryData.Dispose();
            _fileBinaryRowIndex.Dispose();
            _fileBinaryColIndex.Dispose();
            _fileBinarySeqData.Dispose();
            _fileBinarySeqIndex.Dispose();
            _fileBinarySeqCoord.Dispose();
            _fileBinaryBandCoord.Dispose();
        }
        #endregion

        #region Работа со строками
        /// <summary>
        /// Возвращает количество строк матрицы
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetNumRowsAsync()
        {
            int numRows = await _fileBinaryDescription.ReadAsync(0);
            return numRows;
        }

        /// <summary>
        /// Устанавливает количество строк матрицы
        /// </summary>
        /// <returns></returns>
        public async Task SetNumRowsAsync(int numRows)
        {
            await _fileBinaryDescription.EditDataAsync(numRows, 0);            
        }

        /// <summary>
        /// Увеличение числа строк на указанную величину
        /// </summary>
        /// <param name="numRowsPlus"></param>
        /// <returns></returns>
        public async Task IncreaseNumRowsAsync(int numRowsPlus)
        {
            int numRows = await GetNumRowsAsync();
            int newNumRows = numRows + numRowsPlus;
            await SetNumRowsAsync(newNumRows);
        }

        /// <summary>
        /// Добавление строки в конец матрицы
        /// </summary>
        /// <param name="rowData"></param>
        public async Task AddRowAsync(double[] rowData)
        {
            await AddRowAsync(rowData, 0);            
        }
               
        /// <summary>
        /// Добавление строки в конец матрицы с добавлением данных начиная с указанного индекса столбца
        /// </summary>
        /// <param name="rowData">Массив данных</param>
        /// /// <param name="colIndexStart">Индекс столбца начала вставки</param>
        public async Task AddRowAsync(double[] rowData, int colIndexStart)
        {
            await IncreaseNumRowsAsync(1);

            int numCols = await GetNumColsAsync();
            if (numCols < colIndexStart + rowData.Length)
            {
                await SetNumColsAsync(colIndexStart + rowData.Length);
            }


            long numElementsInRowIndexFile = _fileBinaryRowIndex.GetNumElementsInFile;

            int numAddedElements = 0;
            for (int i = colIndexStart; i < colIndexStart+rowData.Length; i++)
            {
                if (Math.Abs(rowData[i - colIndexStart]) > double.Epsilon)
                {
                    await _fileBinaryData.WriteDataAsync(rowData[i - colIndexStart]);
                    await _fileBinaryColIndex.WriteDataAsync(i);
                    numAddedElements++;
                }
            }

            if (numElementsInRowIndexFile == 0)
            {
                await _fileBinaryRowIndex.WriteDataAsync(0);
                await _fileBinaryRowIndex.WriteDataAsync(numAddedElements);
            }
            else
            {
                int lastElement = await _fileBinaryRowIndex.ReadAsync((int)numElementsInRowIndexFile - 1);
                await _fileBinaryRowIndex.WriteDataAsync(lastElement + numAddedElements);
            }
        }
                
        #endregion

        #region Работа со столбцами
        /// <summary>
        /// Возвращает количество столбцов матрицы
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetNumColsAsync()
        {
            int numCols = await _fileBinaryDescription.ReadAsync(1);
            return numCols;
        }

        /// <summary>
        /// Устанавливает количество столбцов матрицы
        /// </summary>
        /// <returns></returns>
        public async Task SetNumColsAsync(int numCols)
        {
            await _fileBinaryDescription.EditDataAsync(numCols, 1);
        }

        /// <summary>
        /// Увеличение числа столбцов на указанную величину
        /// </summary>
        /// <param name="numColsPlus"></param>
        /// <returns></returns>
        public async Task IncreaseNumColsAsync(int numColsPlus)
        {
            int numCols = await GetNumColsAsync();
            int newNumCols = numCols + numColsPlus;
            await SetNumRowsAsync(newNumCols);
        }

        #endregion

        #region Вывод сведений в консоль
        /// <summary>
        /// Вывод в консоль путей к файлам с данными
        /// </summary>
        /// <returns></returns>
        public void PrintMatrixFileNames()
        {            
            Console.WriteLine(GetPathToDescriptionFile);
            Console.WriteLine(GetPathToDataFile);
            Console.WriteLine(GetPathToRowIndexFile);
            Console.WriteLine(GetPathToColIndexFile);
            Console.WriteLine(GetPathToSeqDataFile);
            Console.WriteLine(GetPathToSeqIndexFile);
            Console.WriteLine(GetPathToSeqCoordFile);
            Console.WriteLine(GetPathToBandCoordFile);
        }

        /// <summary>
        /// Вывод в консоль описания матрицы
        /// </summary>
        /// <returns></returns>
        public async Task PrintMatrixDescription()
        {
            Console.WriteLine($"Количество строк матрицы: { await GetNumRowsAsync() }");
            Console.WriteLine($"Количество столбцов матрицы: { await GetNumColsAsync() }");
        }

        /// <summary>
        /// Вывод матрицы в консоль
        /// </summary>
        /// <returns></returns>
        public async Task PrintMatrix()
        {
            int numRows = await GetNumRowsAsync();
            int numCols = await GetNumColsAsync();

            for (int row = 0; row < numRows; row++)
            {
                double[] rowData = await GetRowAsync(row);
                for (int col = 0; col < numCols; col++)
                {
                    Console.Write($"{rowData[col]} \t");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Вывод матрицы в консоль на основе
        /// ненулевых отдельных элементов
        /// </summary>
        /// <returns></returns>
        public async Task PrintRowsFromNnzSeparateElements()
        {
            int numRows = await GetNumRowsAsync();
            int numCols = await GetNumColsAsync();

            for(int row = 0; row < numRows; row++)
            {
                double[] rowData = await GetRowFromSeparatedNnzElementsAsync(row);
                for(int col = 0; col < numCols; col++)
                {
                    Console.Write($"{rowData[col]} \t");
                }                
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Вывод в консоль содержимого файла с данными последовательностей
        /// </summary>
        /// <returns></returns>
        public async Task PrintMatrixSequenceDataFile()
        {
            Console.WriteLine("Содержимое файла с данными последовательностей");
            double[] data = await _fileBinarySeqData.ReadAsync();
            for (int i = 0; i < data.Length; i++)
            {
                Console.WriteLine($"{i}:\t{data[i]}");
            }            
        }

        /// <summary>
        /// Вывод в консоль содержимого файла с индексами последовательностей
        /// </summary>
        /// <returns></returns>
        public async Task PrintMatrixSequenceIndexFile()
        {
            Console.WriteLine("Содержимое файла с индексами последовательностей");
            int[] indexes = await _fileBinarySeqIndex.ReadAsync();
            for (int i = 0; i < indexes.Length; i++)
            {
                Console.WriteLine($"{i}:\t{indexes[i]}");
            }
        }

        /// <summary>
        /// Вывод в консоль содержимого файла с координатами последовательностей
        /// </summary>
        /// <returns></returns>
        public async Task PrintMatrixSequenceCoordinatesFile()
        {
            Console.WriteLine("Содержимое файла с координатами последовательностей");
            Console.WriteLine($"index:\tseqIndex\trowIndex\tcolIndex");
            int[] coordinates = await _fileBinarySeqCoord.ReadAsync();
            for (int i = 0; i < coordinates.Length; i+=3)
            {
                Console.WriteLine($"{i/3}:\t{coordinates[i]}\t{coordinates[i+1]}\t{coordinates[i+2]}");
            }
        }

        /// <summary>
        /// Вывод в консоль содержимого файла с координатами ленточных последовательностей
        /// </summary>
        /// <returns></returns>
        public async Task PrintMatrixSequenceBandCoordinatesFile()
        {
            Console.WriteLine("Содержимое файла с координатами ленточных последовательностей");
            Console.WriteLine($"index:\tseqIndex\trowIndex\tcolIndex\tnumRows");
            int[] coordinates = await _fileBinaryBandCoord.ReadAsync();
            for (int i = 0; i < coordinates.Length; i += 4)
            {
                Console.WriteLine($"{i / 3}:\t{coordinates[i]}\t{coordinates[i + 1]}\t{coordinates[i + 2]}\t{coordinates[i + 3]}");
            }
        }

        /// <summary>
        /// Выводит в консоль содержимое строки с указанным индексом
        /// на основе данных о последовательностях
        /// </summary>
        /// <param name="rowIndex">Индекс строки</param>
        /// <returns></returns>
        public async Task PrintRowFromSequence(int rowIndex)
        {
            Console.WriteLine($"Строка {rowIndex}:");
            
            double[] rowDataFromSeq = await GetRowFromSequenceAsync(rowIndex);
            for (int i = 0; i < rowDataFromSeq.Length; i++)
            {
                Console.Write($"{rowDataFromSeq[i]}\t");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Выводит в консоль содержимое строки с указанным индексом
        /// на основе данных о ленточных последовательностях
        /// </summary>
        /// <param name="rowIndex">Индекс строки</param>
        /// <returns></returns>
        public async Task PrintRowFromSequenceBand(int rowIndex)
        {
            Console.WriteLine($"Строка {rowIndex}:");

            double[] rowDataFromSeqBand = await GetRowFromSequenceBandAsync(rowIndex);
            for (int i = 0; i < rowDataFromSeqBand.Length; i++)
            {
                Console.Write($"{rowDataFromSeqBand[i]}\t");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Выводит в консоль матрицу
        /// на основе данных о последовательностях
        /// </summary>
        /// <returns></returns>
        public async Task PrintRowsFromSequences()
        {
            double[,] matrixFromSeq = await GetRowsFromSequenceAsync();

            for (int i = 0; i <= matrixFromSeq.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= matrixFromSeq.GetUpperBound(1); j++)
                {
                    Console.Write($"{matrixFromSeq[i,j]}\t");
                }
                Console.WriteLine();
            }                        
        }

        /// <summary>
        /// Выводит в консоль матрицу
        /// на основе данных о ленточных последовательностях
        /// </summary>
        /// <returns></returns>
        public async Task PrintRowsFromSequencesBand()
        {
            double[,] matrixFromSeq = await GetRowsFromSequenceBandAsync();

            for (int i = 0; i <= matrixFromSeq.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= matrixFromSeq.GetUpperBound(1); j++)
                {
                    Console.Write($"{matrixFromSeq[i, j]}\t");
                }
                Console.WriteLine();
            }
        }

        #endregion

        /// <summary>
        /// Возвращает элемент матрицы,
        /// расположенный в указанной позиции
        /// </summary>
        /// <param name="row">Номер строки (начиная с 0)</param>
        /// <param name="col">Номер столбца (начиная с 0)</param>
        /// <returns></returns>
        public async Task<double> GetElementAsync(int row, int col)
        {
            var rowData = await GetRowFromSeparatedNnzElementsAsync(row);

            if(col < 0 || col > rowData.Length-1)
            {
                throw new Exception($"Выход за пределы массива! Попытка запроса столбца с индесом {col}.");
            }

            double element = rowData[col];
            return element;
        }

        /// <summary>
        /// Возвращает строку матрицы в виде массива
        /// на основе данных об отдельных ненулевых элементах,
        /// последовательностях и лентах
        /// </summary>
        /// <param name="row">Номер строки (начиная с 0)</param>
        /// <returns>массив double[]</returns>
        public async Task<double[]> GetRowAsync(int row)
        {
            if (row < 0)
            {
                throw new Exception($"Выход за пределы массива! Попытка запроса строки с индексом {row}. Индекс строки не может быть меньше нуля!");
            }

            double[] nnz  = await GetRowFromSeparatedNnzElementsAsync(row);
            double[] seq  = await GetRowFromSequenceAsync(row);
            double[] band = await GetRowFromSequenceBandAsync(row);

            double[] resultRow = new double[nnz.Count()];

            for (int i = 0; i < resultRow.Length; i++)
            {
                int numNnzItems = 0;
                if (nnz[i] > double.Epsilon) numNnzItems++;
                if (seq[i] > double.Epsilon) numNnzItems++;
                if (band[i] > double.Epsilon) numNnzItems++;

                if(numNnzItems>1)
                {
                    throw new Exception("Пересечение данных! Несколько элементов данных расположены в одной ячейке.");
                }

                resultRow[i] = nnz[i] + seq[i] + band[i];
            }

            return resultRow;
        }

        /// <summary>
        /// Возвращает строку матрицы в виде массива
        /// на основе данных об отдельных ненулевых элементах
        /// </summary>
        /// <param name="row">Номер строки (начиная с 0)</param>
        /// <returns>массив double[]</returns>
        public async Task<double[]> GetRowFromSeparatedNnzElementsAsync(int row)
        {
            if(row < 0)
            {
                throw new Exception($"Выход за пределы массива! Попытка запроса строки с индексом {row}. Индекс строки не может быть меньше нуля!");
            }

            int numCols = await GetNumColsAsync();
            double[] rowData = new double[numCols];

            // Определяем наличие неповторяющихся ненулевых элементов
            int numElementsInFile = (int)_fileBinaryRowIndex.GetNumElementsInFile;
            int maxRowIndex = numElementsInFile - 2;
            if(maxRowIndex < row)
            {
                return rowData;
            }

            // Считываем неповторяющиеся ненулевые значения
            int indexStart = await _fileBinaryRowIndex.ReadAsync(row);
            int indexStop  = await _fileBinaryRowIndex.ReadAsync(row+1);

            for (int i = indexStart; i < indexStop; i++)
            {
                int indexCol = await _fileBinaryColIndex.ReadAsync(i);
                double cellValue = await _fileBinaryData.ReadAsync(i);
                rowData[indexCol] = cellValue;
            }

            return rowData;
        }

        /// <summary>
        /// Возвращает содержимое строки с указанным индексом
        /// на основе данных о последовательностях
        /// </summary>
        /// <param name="rowIndex">Индекс строки</param>
        /// <returns></returns>
        public async Task<double[]> GetRowFromSequenceAsync(int rowIndex)
        {            
            int numColumns = await GetNumColsAsync();
            double[] rowDataFromSeq = new double[numColumns];

            int[] seqCoordinates = await _fileBinarySeqCoord.ReadAsync();
            for (int i = 0; i < seqCoordinates.Length; i+=3)
            {
                if(seqCoordinates[i+1] == rowIndex)
                {
                    int seqIndex    = seqCoordinates[i];
                    int seqRowIndex = seqCoordinates[i + 1];
                    int seqColIndex = seqCoordinates[i + 2];

                    double[] seqData = await GetSequenceAsync(seqIndex);
                    for (int j = 0; j < seqData.Length; j++)
                    {
                        rowDataFromSeq[seqColIndex + j] = seqData[j];
                    }
                }
            }            

            return rowDataFromSeq;
        }

        /// <summary>
        /// Возвращает содержимое строки с указанным индексом
        /// на основе данных о ленточных последовательностях
        /// </summary>
        /// <param name="rowIndex">Индекс строки</param>
        /// <returns></returns>
        public async Task<double[]> GetRowFromSequenceBandAsync(int rowIndex)
        {
            int numColumns = await GetNumColsAsync();
            double[] rowDataFromSeq = new double[numColumns];

            int[] seqBandCoordinates = await _fileBinaryBandCoord.ReadAsync();
            for (int i = 0; i < seqBandCoordinates.Length; i += 4)
            {
                int seqIndex = seqBandCoordinates[i];
                int seqRowStartIndex = seqBandCoordinates[i + 1];
                int seqColStartIndex = seqBandCoordinates[i + 2];
                int numRows = seqBandCoordinates[i + 3];

                // Проверка попадания строки в ленту
                if (rowIndex >= seqRowStartIndex && rowIndex < seqRowStartIndex + numRows)
                {
                    int offset = rowIndex - seqRowStartIndex;
                    double[] seqData = await GetSequenceAsync(seqIndex);
                    for (int j = 0; j < seqData.Length; j++)
                    {
                        rowDataFromSeq[seqColStartIndex + offset + j] = seqData[j];
                    }
                }
            }

            return rowDataFromSeq;
        }


        #region Работа с последовательностями
        /// <summary>
        /// Возвращает количество последовательностей
        /// </summary>
        /// <returns></returns>
        public int GetNumSequences()
        {
            int numSequences = (int)_fileBinarySeqIndex.GetNumElementsInFile;
            if(numSequences > 0)
            {
                numSequences--;
            }
            return numSequences;
        }

        /// <summary>
        /// Возвращает количество координат последовательностей
        /// </summary>
        /// <returns></returns>
        public int GetNumSequenceCoordinates()
        {
            int numSequenceCoordinates = (int)_fileBinarySeqCoord.GetNumElementsInFile/3;
            
            return numSequenceCoordinates;
        }


        /// <summary>
        /// Добавление последовательности
        /// </summary>
        /// <param name="sequenceData"></param>
        /// <returns>Индекс добавленной последовательности</returns>
        public async Task<int> AddSequence(double[] sequenceData)
        {
            // Проверка наличия последовательности
            // (для исключения дублирования)
            // --- добавить ---
                        

            int numSequences = GetNumSequences();            
            if(numSequences == 0)
            {
                await _fileBinarySeqIndex.WriteDataAsync(0);
                await _fileBinarySeqIndex.WriteDataAsync(sequenceData.Length);
            }
            else
            {
                int numElementsInDataSeqFile = (int)_fileBinarySeqData.GetNumElementsInFile;
                await _fileBinarySeqIndex.WriteDataAsync(numElementsInDataSeqFile + sequenceData.Length);
            }
            int addedSeqIndex = numSequences;

            // Записываем данные последовательности
            await _fileBinarySeqData.WriteDataAsync(sequenceData);

            return addedSeqIndex;
        }

        /// <summary>
        /// Добавление координаты размещения первого элемента последовательности
        /// </summary>
        /// <param name="sequenceIndex">Индекс последовательности</param>
        /// <param name="rowIndex">Индекс строки размещения последовательности</param>
        /// <param name="colIndex">Индекс столбца, в котором находится первый элемент последовательности</param>
        /// <returns></returns>
        public async Task<int> AddSequenceCoord(int sequenceIndex, int rowIndex, int colIndex)
        {
            // Проверка наличия координат последовательности
            // (для исключения дублирования)
            // --- добавить ---


            int numSequenceCoordinates = GetNumSequenceCoordinates();
            
            int addedSequenceCoordinates = numSequenceCoordinates;

            // Записываем данные о координатах последовательности
            int[] sequenceCoordData = new int[] { sequenceIndex, rowIndex, colIndex };
            await _fileBinarySeqCoord.WriteDataAsync(sequenceCoordData);

            return addedSequenceCoordinates;
        }

        /// <summary>
        /// Возвращает последовательность с указанным индексом
        /// </summary>
        /// <param name="seqIndex"></param>
        /// <returns></returns>
        public async Task<double[]> GetSequenceAsync(int seqIndex)
        {
            int startIndex, numElements;
            (startIndex, numElements) = await GetSequenceIndexesAsync(seqIndex);
            double[] sequence = await _fileBinarySeqData.ReadAsync(startIndex, numElements);
            return sequence;
        }

        /// <summary>
        /// Возвращает индекс первого элемента последовательности и количество элементов
        /// </summary>
        /// <param name="seqIndex"></param>
        /// <returns></returns>
        public async Task<(int startIndex, int numElements)> GetSequenceIndexesAsync(int seqIndex)
        {
            int[] seqIndexes = await _fileBinarySeqIndex.ReadAsync(seqIndex,2);
            int startIndex  = seqIndexes[0];
            int numElements = seqIndexes[1] - seqIndexes[0];
            return (startIndex, numElements);
        }

        /// <summary>
        /// Добавляет в матрицу последовательность
        /// и координаты её использования
        /// </summary>
        /// <param name="sequence">Последовательность</param>
        /// <param name="rows">Массив индексов строк
        /// использования последовательности</param>
        /// <param name="columns">Массив индексов
        /// столбцов использования последовательности</param>
        /// <returns>Кортеж из индекса добавленой последовательности
        /// и массива индексов координат её использования</returns>
        public async Task<(int, int[])> AddRowsSequenceAsync(double[] sequence,
            int[] rows,
            int[] columns)
        {
            // Проверка равенства массивов строк и столбцов
            if(rows.Length!=columns.Length)
            {
                throw new Exception("Неверный формат входных данных!" +
                    " Число элементов в массивах строк и столбцов" +
                    " должно быть одинаковым.");
            }

            // Расширяем матрицу при необходимости
            int maxColumnIndex = columns.Max();
            int needColumns = maxColumnIndex + sequence.Length;
            int numColumns = await GetNumColsAsync();

            if (needColumns > numColumns)
            {
                await SetNumColsAsync(needColumns);
            }
            
            
            int needRows = rows.Max() + 1;
            int numRows = await GetNumRowsAsync();

            if(needRows > numRows)
            {
                await SetNumRowsAsync(needRows);
            }

            int seqIndex = await AddSequence(sequence);

            int[] addedSequenceIndexes = new int[rows.Length];
            for (int i = 0; i < rows.Length; i++)
            {
                addedSequenceIndexes[i] = await AddSequenceCoord(seqIndex, rows[i], columns[i]);
            }

            return (seqIndex, addedSequenceIndexes);
        }

        /// <summary>
        /// Возвращает двумерный массив,
        /// сформированный на основе данных о последовательностях
        /// </summary>
        /// <returns></returns>
        public async Task<double[,]> GetRowsFromSequenceAsync()
        {
            int numRows = await GetNumRowsAsync();
            int numCols = await GetNumColsAsync();
            double[,] matrix = new double[numRows, numCols];
            for (int i = 0; i < numRows; i++)
            {
                double[] rowDataFromSeq = await GetRowFromSequenceAsync(i);
                for (int j = 0; j < numCols; j++)
                {
                    matrix[i, j] = rowDataFromSeq[j];
                }
            }
            return matrix;
        }
        #endregion

        #region Ленточные последовательности
        /// <summary>
        /// Возвращает двумерный массив,
        /// сформированный на основе данных
        /// о ленточных последовательностях
        /// </summary>
        /// <returns></returns>
        public async Task<double[,]> GetRowsFromSequenceBandAsync()
        {
            int numRows = await GetNumRowsAsync();
            int numCols = await GetNumColsAsync();
            double[,] matrix = new double[numRows, numCols];
            for (int i = 0; i < numRows; i++)
            {
                double[] rowDataFromSeq = await GetRowFromSequenceBandAsync(i);
                for (int j = 0; j < numCols; j++)
                {
                    matrix[i, j] = rowDataFromSeq[j];
                }
            }
            return matrix;
        }

        /// <summary>
        /// Возвращает количество координат ленточных последовательностей
        /// </summary>
        /// <returns></returns>
        public int GetNumSequenceBandCoordinates()
        {
            int numSequenceBandCoordinates = (int)_fileBinaryBandCoord.GetNumElementsInFile / 4;

            return numSequenceBandCoordinates;
        }

        /// <summary>
        /// Добавление координат размещения первого элемента
        /// ленточной последовательности и количества строк ленты
        /// </summary>
        /// <param name="sequenceIndex">Индекс последовательности</param>
        /// <param name="rowIndexStart">Индекс строки
        /// размещения последовательности</param>
        /// <param name="colIndexStart">Индекс столбца,
        /// в котором находится первый элемент последовательности</param>
        /// <param name = "numRows">Количество строк ленты</param>
        /// <returns>Индекс добавленной записи о ленте</returns>
        public async Task<int> AddSequenceBandCoord(int sequenceIndex,
            int rowIndexStart, int colIndexStart, int numRows)
        {
            // Проверка наличия координат последовательности
            // (для исключения дублирования)
            // --- добавить ---


            int numSequenceBandCoordinates = GetNumSequenceBandCoordinates();

            int addedSequenceBandCoordinates = numSequenceBandCoordinates;

            // Записываем данные о координатах последовательности
            int[] sequenceBandCoordData = new int[] { sequenceIndex, rowIndexStart, colIndexStart, numRows };
            await _fileBinaryBandCoord.WriteDataAsync(sequenceBandCoordData);

            return addedSequenceBandCoordinates;
        }

        /// <summary>
        /// Добавляет в матрицу ленточную последовательность
        /// </summary>
        /// <param name="sequence">Последовательность</param>
        /// <param name="startRow">Индекс строки
        /// первого элемента первой строки
        /// ленточной последовательности</param>
        /// <param name="startColumn">Индекс столбца
        /// первого элемента первой строки
        /// ленточной последовательности</param>
        /// <param name="numRowsOfSequenceBand">Количество строк
        /// ленточной последовательности</param>
        /// <returns>Кортеж из индекса добавленой последовательности
        /// и индекса её использования</returns>
        public async Task<(int, int)> AddRowsSequenceBandAsync(double[] sequence, int startRow, int startColumn, int numRowsOfSequenceBand)
        {
            // Расширяем матрицу при необходимости            
            int needColumns = startColumn + numRowsOfSequenceBand - 1 + sequence.Length;
            int numColumns = await GetNumColsAsync();
            if (needColumns > numColumns)
            {
                await SetNumColsAsync(needColumns);
            }
            
            int needRows = startRow + numRowsOfSequenceBand;
            int numRows = await GetNumRowsAsync();
            if (needRows > numRows)
            {
                await SetNumRowsAsync(needRows);
            }

            int seqIndex = await AddSequence(sequence);
            int addedSequenceIndex = await AddSequenceBandCoord(seqIndex, startRow, startColumn, numRowsOfSequenceBand);
            
            return (seqIndex, addedSequenceIndex);
        }


        #endregion

        #region Преобразование матрицы в другие форматы
        /// <summary>
        /// Возвращает матрицу в формате CSR
        /// </summary>
        /// <returns></returns>
        public async Task<MatrixCSR> ConvertMatrixToCsrFormat()
        {
            MatrixCSR matrixCsr = new MatrixCSR();
            int numRows = await GetNumRowsAsync();
            matrixCsr.NumRows = numRows;
            int numCols = await GetNumColsAsync();
            matrixCsr.NumCols = numCols;

            List<double> listCsrValA = new List<double>();
            List<int> listCsrColIndA = new List<int>();

            int nnzElCounter = 0;
            for (int i = 0; i < matrixCsr.NumRows; i++)
            {
                matrixCsr.CsrRowPtrA[i] = nnzElCounter;
                double[] rowData = await GetRowAsync(i);
                for (int colIndex = 0; colIndex < rowData.Length; colIndex++)
                {
                    if(rowData[colIndex] > double.Epsilon || rowData[colIndex] < -double.Epsilon)
                    {
                        listCsrValA.Add(rowData[colIndex]);
                        listCsrColIndA.Add(colIndex);
                        nnzElCounter++;
                    }
                }
            }
            matrixCsr.CsrRowPtrA[numRows] = nnzElCounter;

            matrixCsr.CsrValA = listCsrValA.ToArray();
            matrixCsr.CsrColIndA = listCsrColIndA.ToArray();

            return matrixCsr;
        }
        #endregion
    }
}
