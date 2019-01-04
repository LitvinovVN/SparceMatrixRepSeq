using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ManagedCuda.CudaSolve;
using ManagedCuda.CudaSparse;
using ManagedCuda;
using ManagedCuda.BasicTypes;
using System.Diagnostics;

namespace SparseMatrixRepSeqNamespace
{
    /// <summary>
    /// Система линейных алгебраических уравнений
    /// </summary>
    public class SLAE : IDisposable
    {
        #region Закрытые поля
        /// <summary>
        /// Таймер
        /// </summary>
        static Stopwatch stopWatch = new Stopwatch();

        /// <summary>
        /// Путь к папке с проектом
        /// </summary>
        readonly string _pathToProjectDir = "";

        /// <summary>
        /// Имя файла, содержащего правую часть СЛАУ
        /// </summary>
        readonly string _fileRightSideName = "slae.rside";

        /// <summary>
        /// Имя файла, содержащего решение СЛАУ
        /// </summary>
        readonly string _fileResultName = "slae.result";

        /// <summary>
        /// Матрица
        /// </summary>
        SparseMatrixRepSeq _sparseMatrixRepSeq;

        /// <summary>
        /// Файл, содержащий правую часть СЛАУ
        /// </summary>
        FileBinaryDouble _fileBinaryRightSide;

        /// <summary>
        /// Файл, содержащий решение СЛАУ
        /// </summary>
        FileBinaryDouble _fileBinaryResult;
        #endregion

        #region Открытые свойства
        /// <summary>
        /// Возвращает путь к файлу,
        /// содержащему правую часть СЛАУ
        /// </summary>
        public string GetPathToRightSideFile
        {
            get
            {
                return Path.Combine(_pathToProjectDir, _fileRightSideName);
            }
        }

        /// <summary>
        /// Возвращает путь к файлу,
        /// содержащему правую часть СЛАУ
        /// </summary>
        public string GetPathToResultFile
        {
            get
            {
                return Path.Combine(_pathToProjectDir, _fileResultName);
            }
        }

        /// <summary>
        /// Матрица
        /// </summary>
        public SparseMatrixRepSeq SparseMatrixRepSeq
        {
            get
            {
                return _sparseMatrixRepSeq;
            }            
        }
        #endregion

        #region Конструкторы
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="pathToProjectDir">Путь к папке с проектом</param>
        public SLAE(string pathToProjectDir)
        {
            if (!Directory.Exists(pathToProjectDir))
            {
                Directory.CreateDirectory(pathToProjectDir);
            }
            _pathToProjectDir = pathToProjectDir;

            _fileBinaryRightSide = new FileBinaryDouble(GetPathToRightSideFile);
            _fileBinaryResult = new FileBinaryDouble(GetPathToResultFile);

            _sparseMatrixRepSeq = new SparseMatrixRepSeq(pathToProjectDir);
        }        
        #endregion

        #region Очистка данных и освобождение ресурсов
        /// <summary>
        /// Очищает файлы с данными
        /// </summary>
        /// <returns></returns>
        public async Task ClearAsync()
        {
            _fileBinaryRightSide.ClearFile();
            _fileBinaryResult.ClearFile();
            await _sparseMatrixRepSeq.ClearAsync();            
        }

        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        public void Dispose()
        {
            _fileBinaryRightSide.Dispose();
            _fileBinaryResult.Dispose();
            _sparseMatrixRepSeq.Dispose();
        }
        #endregion

        #region Вывод сведений в консоль
        /// <summary>
        /// Вывод в консоль содержимого файла
        /// с правыми частями СЛАУ
        /// </summary>
        /// <returns></returns>
        public async Task PrintRightSide()
        {
            double[] rSide = await GetRightSide();

            Console.WriteLine();
            Console.WriteLine("Правая часть СЛАУ:");
            for (int i = 0; i < rSide.Length; i++)
            {
                Console.WriteLine($"{i}:\t{rSide[i]}");
            }
            Console.WriteLine("----------------");
        }

        /// <summary>
        /// Вывод в консоль содержимого файла
        /// с результатами решения СЛАУ
        /// </summary>
        /// <returns></returns>
        public async Task PrintResult()
        {
            double[] result = await GetResult();

            Console.WriteLine();
            Console.WriteLine("Результаты решения СЛАУ:");
            if (result.Length == 0)
            {
                Console.WriteLine("Результаты отсутствуют!");
            }
            for (int i = 0; i < result.Length; i++)
            {
                Console.WriteLine($"{i}:\t{result[i]}");
            }
            Console.WriteLine("----------------");
        }
        #endregion

        /// <summary>
        /// Устанавливает значения правой стороны матрицы
        /// </summary>
        /// <param name="rightSideData"></param>
        /// <returns></returns>
        public async Task SetRightSideAsync(double[] rightSideData)
        {
            int numRows = await _sparseMatrixRepSeq.GetNumRowsAsync();
            if(rightSideData.Length != numRows)
            {
                throw new Exception($"Ошибка! Несоответствие числа строк матрицы ({numRows}) и правых значений СЛАУ ({rightSideData.Length})");
            }

            await _fileBinaryRightSide.WriteDataAsync(rightSideData);
        }


        /// <summary>
        /// Возвращает массив правых частей СЛАУ
        /// </summary>
        /// <returns></returns>
        private async Task<double[]> GetRightSide()
        {
            double[] rSide = await _fileBinaryRightSide.ReadAsync();
            return rSide;
        }

        /// <summary>
        /// Возвращает массив коэффициентов решения СЛАУ
        /// </summary>
        /// <returns></returns>
        private async Task<double[]> GetResult()
        {
            double[] result = await _fileBinaryResult.ReadAsync();
            return result;
        }

        /// <summary>
        /// Решение СЛАУ с помощью CUDA
        /// </summary>
        /// <returns>Время решения СЛАУ, мс</returns>
        public async Task<long> SolveCuda()
        {
            CudaSolveSparse sp = new CudaSolveSparse(); //Создаем решатель из библиотеки ManagedCuda
            CudaSparseMatrixDescriptor matrixDescriptor = new CudaSparseMatrixDescriptor(); // Создается дескриптор матрицы
            double tolerance = 0.0000001; //Точность расчета. Значение взято для иллюстрации

            //// Информация о разреженной матрице в CSR формате
            var matrixCsr = await SparseMatrixRepSeq.ConvertMatrixToCsrFormat();            

            if(matrixCsr.NumCols!=matrixCsr.NumRows)
            {
                throw new Exception($"Ошибка! Число строк ({matrixCsr.NumRows}) не соответствует числу столбцов ({matrixCsr.NumCols}).");
            }



            //int numRows = 3;
            //var nonZeroValues = new double[] { 1.2, 3.3, 4, 5, 6, 7 };
            //var rowPointers   = new int[]    { 0,        2,    4,           6 };
            //var columnIndices = new int[]    { 0,   1,   1, 2, 2, 3 };
            //var rightSide = new double[] { 1, 2, 3 };
            //var x = new double[rowPointers.Length-1];
            //sp.CsrlsvluHost(numRows, nonZeroValues.Length, matrixDescriptor, nonZeroValues,
            //    rowPointers, columnIndices, rightSide,
            //    tolerance, 0, x); //Решение СЛАУ методом LU факторизации

            double[] rightSide = await GetRightSide();
            double[] solutionVector = new double[matrixCsr.NumRows];

            Console.Write($"{DateTime.Now.ToShortTimeString()}: Решение СЛАУ...");
            stopWatch.Restart();
            sp.CsrlsvluHost(matrixCsr.NumRows,
                matrixCsr.CsrValA.Length,
                matrixDescriptor,
                matrixCsr.CsrValA,
                matrixCsr.CsrRowPtrA,
                matrixCsr.CsrColIndA,
                rightSide,
                tolerance,
                0,
                solutionVector); //Решение СЛАУ методом LU факторизации

            stopWatch.Stop();
            Console.WriteLine("OK");

            Console.Write($"{DateTime.Now.ToShortTimeString()}: Запись решения СЛАУ в файл...");
            await _fileBinaryResult.WriteDataAsync(solutionVector);
            Console.WriteLine("OK");

            return stopWatch.ElapsedMilliseconds;
        }

        
    }
}
