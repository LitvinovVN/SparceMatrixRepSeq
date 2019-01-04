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
    /// с содержимым типа int
    /// </summary>
    public class FileBinaryInt : FileBinary
    {
        #region Конструкторы
        /// <summary>
        /// Конструктор
        /// </summary>
        public FileBinaryInt(string filePath) : base(filePath, 4)
        {

        }
        #endregion

        #region Чтение данных
        /// <summary>
        /// Возвращает элементы данных из файла с данными
        /// </summary>
        /// <param name="index">Индекс первого выбираемого элемента данных</param>
        /// <param name="numElements">Количество выбираемых элементов</param>
        /// <returns></returns>
        public new async Task<int[]> ReadAsync(int index, int numElements)
        {
            var bytes = await base.ReadAsync(index, numElements);
            int[] result = Converters.ConvertByteArrayToIntArray(bytes);
            return result;
        }

        /// <summary>
        /// Считывает один элемент данных из указанной позиции
        /// </summary>
        /// <param name="index">Позиция считываемого элемента</param>
        /// <returns></returns>
        public new async Task<int> ReadAsync(int index)
        {
            var bytes = await base.ReadAsync(index, 1);
            int result = Converters.ConvertByteArrayToInt(bytes);
            return result;
        }

        /// <summary>
        /// Считывает все данные из указанного файла
        /// </summary>        
        /// <returns></returns>
        public new async Task<int[]> ReadAsync()
        {
            var bytes = await base.ReadAsync();
            int[] result = Converters.ConvertByteArrayToIntArray(bytes);
            return result;
        }
        #endregion

        #region Добавление данных
        /// <summary>
        /// Записывает один элемент данных в конец файла
        /// </summary>
        /// <param name="element"></param>
        public async Task WriteDataAsync(int element)
        {
            byte[] bytes = Converters.ConvertIntToByteArray(element);
            await WriteDataAsync(bytes);
        }

        /// <summary>
        /// Записывает массив элементов данных в конец файла
        /// </summary>
        /// <param name="element"></param>
        public async Task WriteDataAsync(int[] elements)
        {
            byte[] bytes = Converters.ConvertIntArrayToByteArray(elements);
            await WriteDataAsync(bytes);
        }

        /// <summary>
        /// Вставляет один элемент данных в указанную позицию.
        /// При этом все элементы, начиная с указанной позиции,
        /// смещаются на 1
        /// </summary>
        /// <param name="insertingElement"></param>
        public async Task InsertDataAsync(int insertingElement, int index)
        {
            byte[] insertingElementBytes = Converters.ConvertIntToByteArray(insertingElement);
            await InsertDataAsync(insertingElementBytes, index);
        }

        /// <summary>
        /// Вставляет массив элементов данных в указанную позицию.
        /// При этом все элементы, начиная с указанной позиции,
        /// смещаются на число вставляемых элементов
        /// </summary>
        /// <param name="insertingElements"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task InsertDataAsync(int[] insertingElements, int index)
        {
            byte[] insertingElementsBytes = Converters.ConvertIntArrayToByteArray(insertingElements);
            await InsertDataAsync(insertingElementsBytes, index);
        }
        #endregion

        #region Редактирование данных
        /// <summary>
        /// Перезаписывает (заменяет) один элемент данных в указанной позиции
        /// </summary>
        /// <param name="element"></param>
        public async Task EditDataAsync(int element, int index)
        {
            byte[] bytes = Converters.ConvertIntToByteArray(element);
            await EditDataAsync(bytes, index);
        }

        /// <summary>
        /// Перезаписывает (заменяет) массив элементов данных,
        /// начиная с указанной позиции
        /// </summary>
        /// <param name="element"></param>
        public async Task EditDataAsync(int[] elements, int index)
        {
            byte[] bytes = Converters.ConvertIntArrayToByteArray(elements);
            await EditDataAsync(bytes, index);
        }
        #endregion

        #region Вывод содержимого файла
        /// <summary>
        /// Вывод на консоль содержимого файла
        /// </summary>
        public async Task PrintFileDataToConsoleAsync()
        {
            PrintFileStatisticsToConsole();
            Console.WriteLine($"--------------Start FileData---------------------");

            if (File.Exists(GetPathToFile))
            {
                if (GetNumElementsInFile > 0)
                {
                    Console.WriteLine($"Содержимое файла:");
                    for (int i = 0; i < GetNumElementsInFile; i++)
                    {
                        int value = await ReadAsync(i);
                        Console.WriteLine($"{i}: {value}");
                    }
                }
            }

            Console.WriteLine($"--------------End FileData---------------------");
        }
        #endregion
    }
}
