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
        
        #endregion

        #region Сведения о файлах
        /// <summary>
        /// Путь к папке с проектом
        /// </summary>
        readonly string _pathToProjectDir="";
        
        /// <summary>
        /// Имя файла, содержащего ненулевые
        /// элементы матрицы за исключением
        /// повторяющихся последовательностей
        /// </summary>
        readonly string _fileData = "smrs.dat";

        /// <summary>
        /// Имя файла, содержащего индексы
        /// первых ненулевых элементов каждой строки матрицы
        /// за исключением повторяющихся последовательностей
        /// </summary>
        readonly string _fileRowIndex = "smrs.rindx";
        
        /// <summary>
        /// Возвращает путь к файлу с данными
        /// </summary>
        public string GetPathToDataFile {
            get
            {
                return Path.Combine(_pathToProjectDir, _fileData);
            }
        }

        /// <summary>
        /// Возвращает путь к файлу с индексами начала строк
        /// </summary>
        public string GetPathToRowIndexFile
        {
            get
            {
                return Path.Combine(_pathToProjectDir, _fileRowIndex);
            }
        }
                
        #endregion

        

        #region Файловые потоки
        /// <summary>
        /// Файловый поток для доступа к файлу с данными
        /// </summary>
        private FileBinaryDouble _fileBinaryData;

        /// <summary>
        /// Файловый поток для доступа к файлу с индексами первых элементов строк
        /// </summary>
        private FileBinaryInt _fileBinaryRowIndex;
        #endregion



        #region Конструкторы
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="pathToProjectDir">Путь к папке с проектом</param>
        public SparseMatrixRepSeq(string pathToProjectDir)
        {
            _pathToProjectDir = pathToProjectDir;

            FileSreamsInit();            
        }
        #endregion
               

        /// <summary>
        /// Инициализация файловых потоков
        /// </summary>
        private void FileSreamsInit()
        {
            _fileBinaryData     = new FileBinaryDouble(GetPathToDataFile);
            _fileBinaryRowIndex = new FileBinaryInt(GetPathToRowIndexFile);
        }

        /// <summary>
        /// Очищает файлы с данными
        /// </summary>
        /// <returns></returns>
        public void Clear()
        {
            _fileBinaryData.ClearFile();
            _fileBinaryRowIndex.ClearFile();
        }                

        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        public void Dispose()
        {
            _fileBinaryData.Dispose();
            _fileBinaryRowIndex.Dispose();
        }
        
    }
}
