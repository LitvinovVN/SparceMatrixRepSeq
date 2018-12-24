using SparseMatrixRepSeqNamespace;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SparseMatrixRepSeqConsoleApp
{
    class Program
    {
        protected static void consoleCancelEvent(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("\nThe read operation has been interrupted.");
            Console.WriteLine("  Key pressed: {0}", args.SpecialKey);
            Console.WriteLine("  Cancel property: {0}", args.Cancel);

            // Set the Cancel property to true to prevent the process from terminating.
            Console.WriteLine("Setting the Cancel property to true...");
            args.Cancel = true;

            // Announce the new value of the Cancel property.
            Console.WriteLine("  Cancel property: {0}", args.Cancel);
            Console.WriteLine("The read operation will resume...\n");
        }


        /// <summary>
        /// Отрисовка меню
        /// </summary>
        private static void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("Выберите режим работы. Для выхода нажмите 'q'.");
            Console.WriteLine("Для прерывания процесса вычислений и возврата в меню нажмите CTRL+C.\n");

            Console.WriteLine("1. Создание пустой матрицы");
            Console.WriteLine("2. Создание пустой матрицы и запись 10 данных");
            Console.WriteLine("3. Считывание 10 данных");
            Console.WriteLine("4. Запись и считывание 10 индексов первых ненулевых элементов строк");
            Console.WriteLine("5. Опеределение количества элементов в указанной строке массива");
            Console.WriteLine("6. Перезапись элементов данных в указанных позициях");
            Console.WriteLine("7. Вставка элемента данных в указанную позицию");
            Console.WriteLine("8. Вставка набора данных в указанную позицию");
            Console.WriteLine("9. Удаление элемента с указанным индексом из файла с данными");
            Console.WriteLine("10. Тестирование класса FileBinaryDouble");
        }
        
        static void Main(string[] args)
        {
            //ConsoleKeyInfo cki;            

            // Установка обработчика событий для обработки событий нажатия клавиш
            Console.CancelKeyPress += new ConsoleCancelEventHandler(consoleCancelEvent);

            while (true)
            {
                PrintMenu();

                // Считываем символ с клавиатуры
                //cki = Console.ReadKey(true);
                // Завершаем работу приложения, если пользователь нажал кнопку 'X'
                //if (cki.Key == ConsoleKey.X) break;

                // Announce the name of the key that was pressed .
                //Console.WriteLine($"  Key pressed: {cki.Key}\n");

                string s=Console.ReadLine();
                Console.WriteLine($"  Пользователь написал: {s}\n");

                Console.Clear();
                switch (s)
                {
                    case "q":
                    case "quit":
                    case "exit":
                        return;
                    case "1":
                        CreateSparceMatrixRepSeqWith0Elements();
                        Console.ReadKey(false);
                        break;
                    case "2":
                        CreateSparceMatrixRepSeqWithWriting10ElementsAsync();
                        Console.ReadKey(false);
                        break;
                    case "3":
                        CreateSparceMatrixRepSeqWithReading10ElementsAsync();
                        Console.ReadKey(false);
                        break;
                    case "4":
                        CreateSparceMatrixRepSeqWithWritingAndReadingIndexesOfRowsFirstElementsAsync();
                        Console.ReadKey(false);
                        break;
                    case "5":
                        GetNumElementsInRowAsync();
                        Console.ReadKey(false);
                        break;
                    case "6":
                        RewriteElementsInDataByIndexAsync();
                        Console.ReadKey(false);
                        break;
                    case "7":
                        InsertElementInDataFileByIndexAsync();
                        Console.ReadKey(false);
                        break;
                    case "8":
                        InsertElementsInDataFileByIndexAsync();
                        Console.ReadKey(false);
                        break;
                    case "9":
                        RemoveElementInDataFileByIndexAsync();
                        Console.ReadKey(false);
                        break;
                    case "10":
                        TestingOfClassFileBinaryDouble();
                        Console.ReadKey(false);
                        break;
                    default:
                        break;
                }
                
            }
        }

        /// <summary>
        /// 10. Тестирование класса FileBinaryDouble
        /// </summary>
        private static async Task TestingOfClassFileBinaryDouble()
        {
            string fileName = "10.dat";            
            using (FileBinaryDouble file = new FileBinaryDouble(fileName))
            {               

                if(file.GetNumElementsInFile > 0)
                {
                    await file.PrintFileDataToConsoleAsync();
                    Console.WriteLine($"Очистка содержимого файла...");
                    file.ClearFile();
                    Console.WriteLine($"Размер файла после очистки: {(new FileInfo(fileName)).Length} байт");
                }

                #region 1. Запись числа в конец файла
                Console.WriteLine($"\n1. Запись числа в конец файла");
                await file.PrintFileDataToConsoleAsync();
                Console.WriteLine($"Запись в конец файла числа 543.987654321...");
                await file.WriteDataAsync(543.987654321);                
                Console.WriteLine($"Запись в конец файла числа 111.1111...");
                await file.WriteDataAsync(111.1111);                
                await file.PrintFileDataToConsoleAsync();
                #endregion

                #region 2. Запись массива данных в конец файла
                Console.WriteLine($"\n2. Запись массива данных в конец файла");                
                Console.WriteLine("Запись в конец файла чисел { 21.1, 22.2, 23.3 }...");
                double[] array = new double[] { 21.1, 22.2, 23.3 };
                await file.WriteDataAsync(array);                
                await file.PrintFileDataToConsoleAsync();
                #endregion

                #region 3. Редактирование элементов данных
                Console.WriteLine($"\n3. Редактирование элементов данных");               
                Console.WriteLine($"Редактирование элемента с индексом 1 -> 0.1...");
                await file.EditDataAsync(0.1, 1);
                Console.WriteLine($"Редактирование элемента с индексом 2 -> 0.2...");
                await file.EditDataAsync(0.2, 2);
                await file.PrintFileDataToConsoleAsync();
                #endregion

                #region 4. Редактирование массива последовательно записанных элементов данных
                Console.WriteLine($"\n4. Редактирование массива последовательно записанных элементов данных");
                Console.WriteLine($"Редактирование массива...");
                await file.EditDataAsync(new double[] { 5.4, 4.4, 3.4, 2.4, 1.4 }, 4);                
                await file.PrintFileDataToConsoleAsync();
                #endregion

                #region 5. Удаление элементов данных
                Console.WriteLine($"\n5. Удаление элементов данных");
                Console.WriteLine($"Удаление элемента с индексом 0...");
                await file.RemoveAsync(0);
                Console.WriteLine($"Удаление элемента с индексом 2...");
                await file.RemoveAsync(2);
                Console.WriteLine($"Попытка удаления несуществующего элемента с индексом 1000...");
                try
                {
                    await file.RemoveAsync(1000);
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
                await file.PrintFileDataToConsoleAsync();
                #endregion

                #region 6. Удаление массива элементов данных
                Console.WriteLine($"\n6. Удаление массива элементов данных");
                Console.WriteLine($"Удаление 3 элементов, начиная с индекса 2...");                
                try
                {
                    await file.RemoveAsync(2,3);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
                Console.WriteLine($"Попытка удаления 20 элементов, начиная с индекса 4...");
                try
                {
                    await file.RemoveAsync(4, 20);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }

                Console.WriteLine($"Попытка удаления 2 элементов, начиная с индекса 40...");
                try
                {
                    await file.RemoveAsync(4, 20);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
                await file.PrintFileDataToConsoleAsync();
                #endregion

                #region 7. Вставка элементов по указанному индексу
                Console.WriteLine($"\n7. Вставка элементов по указанному индексу");
                Console.WriteLine($"Вставка числа -7.0 по индексу 0...");
                await file.InsertDataAsync(-7.0, 0);
                Console.WriteLine($"Вставка числа -7.2 по индексу 2...");
                await file.InsertDataAsync(-7.2,2);
                Console.WriteLine($"Попытка вставка числа -7.3 по несуществующему индексу 333...");
                try
                {
                    await file.InsertDataAsync(-7.2, 333);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
                await file.PrintFileDataToConsoleAsync();
                #endregion

                #region 8. Вставка массива элементов по указанному индексу
                Console.WriteLine($"\n8. Вставка массива элементов по указанному индексу");
                Console.WriteLine("Вставка массива { 8.1, 8.2, 8.3 } по индексу 4...");
                await file.InsertDataAsync(new double[] { 8.1, 8.2, 8.3 }, 4);
                Console.WriteLine("Попытка вставка массива { 8.4, 8.5, 8.6 } по несуществующему индексу 333...");
                try
                {
                    await file.InsertDataAsync(new double[] { 8.4, 8.5, 8.6 }, 333);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
                await file.PrintFileDataToConsoleAsync();
                #endregion

                #region 9. Чтение массива элементов по указанному индексу
                Console.WriteLine($"\n9. Чтение массива элементов по указанному индексу");
                Console.WriteLine("Чтение 1 элемента по индексу 3...");
                var readedElement = await file.ReadAsync(3);
                Console.WriteLine(readedElement);
                Console.WriteLine("Чтение 3 элементов по индексу 4...");
                var readedData = await file.ReadAsync(4, 3);
                foreach (var item in readedData)
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine("Попытка чтения по несуществующему индексу 333...");
                try
                {
                    var readedData2 = await file.ReadAsync(333, 2);
                    foreach (var item in readedData2)
                    {
                        Console.WriteLine(item);
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
                await file.PrintFileDataToConsoleAsync();
                #endregion
            }

        }

        /// <summary>
        /// Удаляет элемент с указанным индексом из файла с данными
        /// </summary>
        private static async Task RemoveElementInDataFileByIndexAsync()
        {
            using (SparseMatrixRepSeq sparseMatrixRepSeq = new SparseMatrixRepSeq("9"))
            {
                if (sparseMatrixRepSeq.GetNumElementsInDataFile > 0)
                {
                    Console.WriteLine($"Очистка массива...");
                    sparseMatrixRepSeq.ClearDataFile();
                }

                Console.WriteLine($"Создание массива...");
                for (int i = 0; i < 10; i++)
                {
                    int data = i * 2;
                    await sparseMatrixRepSeq.WriteDataAsync(data);
                    Console.WriteLine($"{i}: {data}");
                }

                Console.WriteLine($"До удаления:");
                for (int i = 0; i < sparseMatrixRepSeq.GetNumElementsInDataFile; i++)
                {
                    double readedElement = await sparseMatrixRepSeq.ReadDataAsync(i);
                    Console.WriteLine($"{i}: {readedElement}");
                }

                Console.WriteLine($"Удаление элемента 0:");                
                await sparseMatrixRepSeq.RemoveDataAsync(0);

                Console.WriteLine($"После удаления:");
                for (int i = 0; i < sparseMatrixRepSeq.GetNumElementsInDataFile; i++)
                {
                    double readedElement = await sparseMatrixRepSeq.ReadDataAsync(i);
                    Console.WriteLine($"{i}: {readedElement}");
                }

                Console.WriteLine($"Удаление элемента 2:");
                await sparseMatrixRepSeq.RemoveDataAsync(2);

                Console.WriteLine($"После удаления:");
                for (int i = 0; i < sparseMatrixRepSeq.GetNumElementsInDataFile; i++)
                {
                    double readedElement = await sparseMatrixRepSeq.ReadDataAsync(i);
                    Console.WriteLine($"{i}: {readedElement}");
                }

                Console.WriteLine($"Удаление элемента 7:");
                await sparseMatrixRepSeq.RemoveDataAsync(7);

                Console.WriteLine($"После удаления:");
                for (int i = 0; i < sparseMatrixRepSeq.GetNumElementsInDataFile; i++)
                {
                    double readedElement = await sparseMatrixRepSeq.ReadDataAsync(i);
                    Console.WriteLine($"{i}: {readedElement}");
                }

                Console.WriteLine($"Удаление несуществующего элемента 77:");
                await sparseMatrixRepSeq.RemoveDataAsync(77);

                Console.WriteLine($"После удаления:");
                for (int i = 0; i < sparseMatrixRepSeq.GetNumElementsInDataFile; i++)
                {
                    double readedElement = await sparseMatrixRepSeq.ReadDataAsync(i);
                    Console.WriteLine($"{i}: {readedElement}");
                }
            }
        }


        /// <summary>
        /// Вставляет массив элементов данных в указанную позицию
        /// </summary>
        private static async Task InsertElementsInDataFileByIndexAsync()
        {
            using (SparseMatrixRepSeq sparseMatrixRepSeq = new SparseMatrixRepSeq("8"))
            {
                if (sparseMatrixRepSeq.GetNumElementsInDataFile > 0)
                {
                    Console.WriteLine($"Очистка массива...");
                    sparseMatrixRepSeq.ClearDataFile();
                }

                Console.WriteLine($"Создание массива...");
                for (int i = 0; i < 10; i++)
                {
                    int data = i * 2;
                    await sparseMatrixRepSeq.WriteDataAsync(data);
                    Console.WriteLine($"{i}: {data}");
                }

                Console.WriteLine($"До вставки:");
                for (int i = 0; i < sparseMatrixRepSeq.GetNumElementsInDataFile; i++)
                {
                    double readedElement = await sparseMatrixRepSeq.ReadDataAsync(i);
                    Console.WriteLine($"{i}: {readedElement}");
                }

                Console.WriteLine($"Вставка элементов:");
                double[] addingElements = new double[] { -1, -2, -3, -4, -5 };
                await sparseMatrixRepSeq.InsertDataAsync(addingElements, 2);

                Console.WriteLine($"После вставки:");
                for (int i = 0; i < sparseMatrixRepSeq.GetNumElementsInDataFile; i++)
                {
                    double readedElement = await sparseMatrixRepSeq.ReadDataAsync(i);
                    Console.WriteLine($"{i}: {readedElement}");
                }
            }
        }


        /// <summary>
        /// Вставляет один элемент данных в указанную позицию
        /// </summary>
        private static async Task InsertElementInDataFileByIndexAsync()
        {
            using (SparseMatrixRepSeq sparseMatrixRepSeq = new SparseMatrixRepSeq("7"))
            {
                if(sparseMatrixRepSeq.GetNumElementsInDataFile>0)
                {
                    Console.WriteLine($"Очистка массива...");
                    sparseMatrixRepSeq.ClearDataFile();
                }

                Console.WriteLine($"Создание массива...");
                for (int i = 0; i < 10; i++)
                {
                    int data = i * 2;
                    await sparseMatrixRepSeq.WriteDataAsync(data);
                    Console.WriteLine($"{i}: {data}");
                }

                Console.WriteLine($"До замены:");
                for (int i = 0; i < sparseMatrixRepSeq.GetNumElementsInDataFile; i++)
                {
                    double readedElement = await sparseMatrixRepSeq.ReadDataAsync(i);
                    Console.WriteLine($"{i}: {readedElement}");
                }

                Console.WriteLine($"Вставка элементов:");
                await sparseMatrixRepSeq.InsertDataAsync(-150.98765, 2);                

                Console.WriteLine($"После вставки:");
                for (int i = 0; i < sparseMatrixRepSeq.GetNumElementsInDataFile; i++)
                {
                    double readedElement = await sparseMatrixRepSeq.ReadDataAsync(i);
                    Console.WriteLine($"{i}: {readedElement}");
                }
            }
        }

        /// <summary>
        /// Перезапись (замена) элментов в указанных позициях
        /// </summary>
        private static async Task RewriteElementsInDataByIndexAsync()
        {
            using (SparseMatrixRepSeq sparseMatrixRepSeq = new SparseMatrixRepSeq("6"))
            {
                if(sparseMatrixRepSeq.GetNumElementsInDataFile<=0)
                {
                    Console.WriteLine($"Создание массива...");
                    for (int i = 0; i < 10; i++)
                    {
                        int data = i * 2;
                        await sparseMatrixRepSeq.WriteDataAsync(data);
                        Console.WriteLine($"{i}: {data}");
                    }
                }
                else
                {
                    Console.WriteLine($"Файл с данными существует. Количество элементов: {sparseMatrixRepSeq.GetNumElementsInDataFile}");
                }
                
                

                Console.WriteLine($"До замены:");
                for (int i = 0; i < 10; i++)
                {
                    double readedElement = await sparseMatrixRepSeq.ReadDataAsync(i);
                    Console.WriteLine($"{i}: {readedElement}");
                }

                Console.WriteLine($"Замена элементов:");
                await sparseMatrixRepSeq.RewriteDataAsync(-150.98765, 2);
                await sparseMatrixRepSeq.RewriteDataAsync(-222.12345, 0);
                await sparseMatrixRepSeq.RewriteDataAsync(1000d, 9);
                await sparseMatrixRepSeq.RewriteDataAsync(0d, 8);

                Console.WriteLine($"После замены:");
                for (int i = 0; i < 10; i++)
                {
                    double readedElement = await sparseMatrixRepSeq.ReadDataAsync(i);
                    Console.WriteLine($"{i}: {readedElement}");
                }
            }
        }

        /// <summary>
        /// Определение количества элементов в строках массива
        /// </summary>
        private static async Task GetNumElementsInRowAsync()
        {
            using (SparseMatrixRepSeq sparseMatrixRepSeq = new SparseMatrixRepSeq("5"))
            {
                Console.WriteLine($"Создание массива...");
                for (int i = 0; i < 11; i++)
                {
                    int dataIndex = i * i * 2;
                    await sparseMatrixRepSeq.WriteRowIndexAsync(i, dataIndex);
                    Console.WriteLine($"{i}: {dataIndex}");
                }                
            }

            using (SparseMatrixRepSeq sparseMatrixRepSeq = new SparseMatrixRepSeq("5"))
            {
                Console.WriteLine($"\nКоличество элементов в строках массива...");
                for (int i = 0; i < 10; i++)
                {
                    int numElementsInRow = await sparseMatrixRepSeq.ReadRowElementsCountAsync(i);
                    Console.WriteLine($"{i}: {numElementsInRow}");
                }                
            }            

            using (SparseMatrixRepSeq sparseMatrixRepSeq = new SparseMatrixRepSeq("5"))
            {                
                int rowStartIndex = 3;
                int numRows = 4;
                Console.WriteLine($"\nКоличество элементов в {numRows} строках массива, начиная с {rowStartIndex}...");
                int[] numElementsInRows = await sparseMatrixRepSeq.ReadRowsElementsCountAsync(rowStartIndex, numRows);

                for (int i = 0; i < numElementsInRows.Length; i++)
                {
                    Console.WriteLine($"{rowStartIndex+i}: {numElementsInRows[i]}");
                }

                Console.WriteLine(sparseMatrixRepSeq.ToString());
                Console.WriteLine(sparseMatrixRepSeq.GetPathToDataFile);
            }

            
        }

        /// <summary>
        /// Создание объекта SparceMatrixRepSeq
        /// с записью и считыванием индексов первых элементов 10 строк
        /// </summary>
        private static async Task CreateSparceMatrixRepSeqWithWritingAndReadingIndexesOfRowsFirstElementsAsync()
        {
            using (SparseMatrixRepSeq sparseMatrixRepSeq = new SparseMatrixRepSeq("4"))
            {                
                for (int i = 0; i < 10; i++)
                {
                    await sparseMatrixRepSeq.WriteRowIndexAsync(i,i*2);
                    Console.WriteLine($"{i}: {i*2}");
                }

                for (int i = 0; i < 10; i++)
                {
                    int rowIndex = await sparseMatrixRepSeq.ReadRowIndexAsync(i);
                    Console.WriteLine($"{i}: {i * 2}");
                }

                Console.WriteLine(sparseMatrixRepSeq.ToString());
                Console.WriteLine(sparseMatrixRepSeq.GetPathToDataFile);
            }
        }

        private static async Task CreateSparceMatrixRepSeqWithReading10ElementsAsync()
        {
            using (SparseMatrixRepSeq sparseMatrixRepSeq = new SparseMatrixRepSeq("2"))
            {
                double d;
                for (int i = 0; i < 10; i++)
                {
                    d = await sparseMatrixRepSeq.ReadDataAsync(i);
                    Console.WriteLine(d);
                }

                Console.WriteLine(sparseMatrixRepSeq.ToString());
                Console.WriteLine(sparseMatrixRepSeq.GetPathToDataFile);
            }
            
        }

        private static void CreateSparceMatrixRepSeqWith0Elements()
        {
            SparseMatrixRepSeq sparseMatrixRepSeq = new SparseMatrixRepSeq("1");

            Console.WriteLine(sparseMatrixRepSeq.ToString());
            Console.WriteLine(sparseMatrixRepSeq.GetPathToDataFile);
        }

        /// <summary>
        /// Создаёт объект и записывает 10 элементов данных
        /// </summary>
        private static async Task CreateSparceMatrixRepSeqWithWriting10ElementsAsync()
        {
            using (SparseMatrixRepSeq sparseMatrixRepSeq = new SparseMatrixRepSeq("2"))                
            {
                double d = -5.678901;
                for (int i = 0; i < 10; i++)
                {                    
                    await sparseMatrixRepSeq.WriteDataAsync(d * i * i);
                }

                Console.WriteLine(sparseMatrixRepSeq.ToString());
                Console.WriteLine(sparseMatrixRepSeq.GetPathToDataFile);
            }            
        }

        
    }
}
