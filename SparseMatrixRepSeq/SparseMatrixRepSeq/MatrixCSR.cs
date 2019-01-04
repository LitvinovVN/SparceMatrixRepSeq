using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SparseMatrixRepSeqNamespace
{
    /// <summary>
    /// Двумерная квадратная матрица в csr-формате
    /// </summary>
    [Serializable]
    public class MatrixCSR
    {
        /// <summary>
        /// Массив значений
        /// </summary>
        public double[] CsrValA { get; set; }

        /// <summary>
        /// Массив индексов первых ненулевых элементов строк + индекс следующего за последним элемента 
        /// </summary>
        public int[] CsrRowPtrA { get; set; }

        /// <summary>
        /// Массив индексов столбцов
        /// </summary>
        public int[] CsrColIndA { get; set; }

        /// <summary>
        /// Возвращает массив элементов, содержащихся в строке
        /// </summary>
        /// <param name="rowNumber">Номер строки</param>
        /// <returns></returns>
        public double[] GetRow(int rowNumber)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Число строк
        /// </summary>
        int _numRows;

        /// <summary>
        /// Число строк
        /// </summary>
        public int NumRows
        {
            get
            {
                return _numRows;
            }
            set
            {
                _numRows = value;
                CsrRowPtrA = new int[_numRows + 1];
            }
        }

        /// <summary>
        /// Число столбцов
        /// </summary>
        public int NumCols { get; set; }
                

        /// <summary>
        /// Индексатор
        /// </summary>
        /// <param name="i">Номер строки</param>
        /// <param name="j">Номер столбца</param>
        /// <returns></returns>
        public double this[int i, int j]
        {
            get
            {
                int rowFirstElementIndex = CsrRowPtrA[i];
                int rowNumElements = CsrRowPtrA[i + 1] - CsrRowPtrA[i];

                for (int rowElementsIndexer = rowFirstElementIndex; rowElementsIndexer < rowFirstElementIndex + rowNumElements; rowElementsIndexer++)
                {
                    if (CsrColIndA[rowElementsIndexer] == j)
                    {
                        return CsrValA[rowElementsIndexer];
                    }
                }

                return 0;
            }            
        }

        #region Конструкторы
        /// <summary>
        /// Конструктор (для сериализации)
        /// </summary>
        public MatrixCSR()
        {

        }
        #endregion

        
        #region Сериализация / десериализация
        /// <summary>
        /// Экспорт матрицы в файл XML
        /// </summary>
        /// <param name="fileName"></param>
        public void ExportToXml(string fileName)
        {
            // передаем в конструктор тип класса
            XmlSerializer formatter = new XmlSerializer(typeof(MatrixCSR));

            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                formatter.Serialize(fs, this);
            }
        }

        /// <summary>
        /// Импорт матрицы из файла XML
        /// </summary>
        /// <param name="fileName"></param>
        public static MatrixCSR ImportFromXml(string fileName)
        {
            // передаем в конструктор тип класса
            XmlSerializer formatter = new XmlSerializer(typeof(MatrixCSR));

            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                MatrixCSR data = (MatrixCSR)formatter.Deserialize(fs);
                return data;
            }
        }
        #endregion
    }
}
