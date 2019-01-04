using SparseMatrixRepSeqNamespace;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace SparseMatrixRepSeqConsoleApp
{
    class Program
    {
        static Stopwatch sWatch = new Stopwatch();

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

            Console.WriteLine("1. Тестирование класса FileBinaryDouble");
            Console.WriteLine("2. Тестирование класса FileBinaryInt");
            Console.WriteLine("3. Создание пустой матрицы");
            Console.WriteLine("4. Запросы на поиск элементов и групп элементов");
            Console.WriteLine("5. Работа с последовательностями элементов");
            Console.WriteLine("6. Добавление последовательностей элементов в массив");
            Console.WriteLine("7. Работа с ленточными последовательностями элементов");
            Console.WriteLine("8. Добавление ленточных последовательностей элементов в массив");
            Console.WriteLine("9. Создание комбинированной матрицы");
            Console.WriteLine("10. Создание СЛАУ");
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
                        TestingOfClassFileBinaryDouble();
                        Console.ReadKey(false);
                        break;
                    case "2":
                        TestingOfClassFileBinaryInt();
                        Console.ReadKey(false);
                        break;
                    case "3":
                        CreateSparceMatrixRepSeqWith0Elements();
                        Console.ReadKey(false);
                        break;
                    case "4":
                        GetElementTests();
                        Console.ReadKey(false);
                        break;
                    case "5":
                        DataSequencesTests();
                        Console.ReadKey(false);
                        break;
                    case "6":
                        DataSequenceAddRowsTests();
                        Console.ReadKey(false);
                        break;
                    case "7":
                        DataSequencesBandTests();
                        Console.ReadKey(false);
                        break;
                    case "8":
                        DataSequenceBandAddRowsTests();
                        Console.ReadKey(false);
                        break;
                    case "9":
                        SparseMatrixRepSeqCreateAndReadTests();
                        Console.ReadKey(false);
                        break;
                    case "10":
                        SlaeCreateAndReadTests();
                        Console.ReadKey(false);
                        break;
                    default:
                        break;
                    
                }
                
            }
        }
                

        /// <summary>
        /// 1. Тестирование класса FileBinaryDouble
        /// </summary>
        private static async Task TestingOfClassFileBinaryDouble()
        {
            string fileName = "1.dat";            
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
        /// 2. Тестирование класса FileBinaryInt
        /// </summary>
        private static async Task TestingOfClassFileBinaryInt()
        {
            string fileName = "2.dat";
            using (FileBinaryInt file = new FileBinaryInt(fileName))
            {

                if (file.GetNumElementsInFile > 0)
                {
                    await file.PrintFileDataToConsoleAsync();
                    Console.WriteLine($"Очистка содержимого файла...");
                    file.ClearFile();
                    Console.WriteLine($"Размер файла после очистки: {(new FileInfo(fileName)).Length} байт");
                }

                #region 1. Запись числа в конец файла
                Console.WriteLine($"\n1. Запись числа в конец файла");
                await file.PrintFileDataToConsoleAsync();
                Console.WriteLine($"Запись в конец файла числа 543...");
                await file.WriteDataAsync(543);
                Console.WriteLine($"Запись в конец файла числа 111...");
                await file.WriteDataAsync(111);
                await file.PrintFileDataToConsoleAsync();
                #endregion

                #region 2. Запись массива данных в конец файла
                Console.WriteLine($"\n2. Запись массива данных в конец файла");
                Console.WriteLine("Запись в конец файла чисел { 21, 22, 23 }...");
                int[] array = new int[] { 21, 22, 23 };
                await file.WriteDataAsync(array);
                await file.PrintFileDataToConsoleAsync();
                #endregion

                #region 3. Редактирование элементов данных
                Console.WriteLine($"\n3. Редактирование элементов данных");
                Console.WriteLine($"Редактирование элемента с индексом 1 -> 31...");
                await file.EditDataAsync(31, 1);
                Console.WriteLine($"Редактирование элемента с индексом 2 -> 32...");
                await file.EditDataAsync(32, 2);
                await file.PrintFileDataToConsoleAsync();
                #endregion

                #region 4. Редактирование массива последовательно записанных элементов данных
                Console.WriteLine($"\n4. Редактирование массива последовательно записанных элементов данных");
                Console.WriteLine($"Редактирование массива...");
                await file.EditDataAsync(new int[] { 44, 45, 46, 47, 48 }, 4);
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
                catch (Exception exc)
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
                    await file.RemoveAsync(2, 3);
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
                Console.WriteLine($"Вставка числа -71 по индексу 0...");
                await file.InsertDataAsync(-71, 0);
                Console.WriteLine($"Вставка числа -72 по индексу 2...");
                await file.InsertDataAsync(-72, 2);
                Console.WriteLine($"Попытка вставка числа -73 по несуществующему индексу 333...");
                try
                {
                    await file.InsertDataAsync(-73, 333);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
                await file.PrintFileDataToConsoleAsync();
                #endregion

                #region 8. Вставка массива элементов по указанному индексу
                Console.WriteLine($"\n8. Вставка массива элементов по указанному индексу");
                Console.WriteLine("Вставка массива { 81, 82, 83 } по индексу 4...");
                await file.InsertDataAsync(new int[] { 81, 82, 83 }, 4);
                Console.WriteLine("Попытка вставка массива { 84, 85, 86 } по несуществующему индексу 333...");
                try
                {
                    await file.InsertDataAsync(new int[] { 84, 85, 86 }, 333);
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
        /// 3. Тестирование класса SparseMatrixRepSeq
        /// </summary>
        private static async Task CreateSparceMatrixRepSeqWith0Elements()
        {
            using (SparseMatrixRepSeq sparseMatrixRepSeq = new SparseMatrixRepSeq("3"))
            {
                Console.WriteLine(sparseMatrixRepSeq.ToString());
                Console.WriteLine(sparseMatrixRepSeq.GetPathToDescriptionFile);
                Console.WriteLine(sparseMatrixRepSeq.GetPathToDataFile);
                Console.WriteLine(sparseMatrixRepSeq.GetPathToRowIndexFile);
                Console.WriteLine(sparseMatrixRepSeq.GetPathToColIndexFile);

                // 0. Очистка матрицы
                Console.WriteLine("0. Очистка матрицы");
                await sparseMatrixRepSeq.ClearAsync();
                await sparseMatrixRepSeq.PrintMatrixDescription();
                Console.WriteLine();

                #region Добавление отдельных строк

                // Добавление строки  | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
                Console.WriteLine("Добавление строки  | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |");
                // Добавление строки index:              0    | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 |
                //                   value:              0    | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
                await sparseMatrixRepSeq.AddRowAsync(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
                await sparseMatrixRepSeq.PrintMatrixDescription();
                await sparseMatrixRepSeq.PrintRowsFromNnzSeparateElements();
                Console.WriteLine();

                // Добавление строки  | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
                Console.WriteLine("Добавление строки  | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |");
                // Добавление строки index:              0    | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 |
                //                   value:              0    | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
                await sparseMatrixRepSeq.AddRowAsync(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
                await sparseMatrixRepSeq.PrintMatrixDescription();
                await sparseMatrixRepSeq.PrintRowsFromNnzSeparateElements();
                Console.WriteLine();

                // Добавление строки  | 0.11 | 1.11 | 2.22 | 0 | 0 | -5.55 | 0 | 0 | -8.88 | -9.99 |
                Console.WriteLine("1. Добавление строки  | 0.11 | 1.11 | 2.22 | 0 | 0 | -5.55 | 0 | 0 | -8.88 | -9.99 |");
                // Добавление строки index:              0    | 1    | 2    | 3 | 4 |  5    | 6 | 7 |  8    |  9    |
                //                   value:              0.11 | 1.11 | 2.22 | 0 | 0 | -5.55 | 0 | 0 | -8.88 | -9.99 |
                await sparseMatrixRepSeq.AddRowAsync(new double[] { 0.11 , 1.11 , 2.22 , 0 , 0 , -5.55 , 0 , 0 , -8.88 , -9.99 });
                await sparseMatrixRepSeq.PrintMatrixDescription();
                await sparseMatrixRepSeq.PrintRowsFromNnzSeparateElements();
                Console.WriteLine();

                // Добавление строки  | 0 | 0 | 2.55 | 0 | -1000 | 0 | 50.0001 | 0 | 0 | 0 |
                Console.WriteLine("Добавление строки  | 0 | 0 | 2.55 | 0 | -1000 | 0 | 50.01 | 0 | 0 | 0 |");
                // Добавление строки index:              0    | 1    | 2    | 3 | 4     | 5   |       6 | 7 | 8 | 9 |
                //                   value:              0    | 0    | 2.55 | 0 | -1000 | 0   | 50.01 | 0 | 0 | 0 |
                await sparseMatrixRepSeq.AddRowAsync(new double[] { 0, 0, 2.55, 0, -1000, 0, 50.01, 0, 0, 0 });
                await sparseMatrixRepSeq.PrintMatrixDescription();
                await sparseMatrixRepSeq.PrintRowsFromNnzSeparateElements();
                Console.WriteLine();

                // Добавление строки  | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
                Console.WriteLine("Добавление строки  | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |");
                // Добавление строки index:              0    | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 |
                //                   value:              0    | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
                await sparseMatrixRepSeq.AddRowAsync(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
                await sparseMatrixRepSeq.PrintMatrixDescription();
                await sparseMatrixRepSeq.PrintRowsFromNnzSeparateElements();
                Console.WriteLine();

                
                // Добавление строки  | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 |
                Console.WriteLine("Добавление строки  | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 |");
                // Добавление строки index:              | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 |
                //                   value:              | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 |
                await sparseMatrixRepSeq.AddRowAsync(new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 });
                await sparseMatrixRepSeq.PrintMatrixDescription();
                await sparseMatrixRepSeq.PrintRowsFromNnzSeparateElements();
                Console.WriteLine();

                sWatch.Start();
                int N = 100;
                for(int i=0;i<N;i++)
                {
                    await sparseMatrixRepSeq.AddRowAsync(new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
                }
                sWatch.Stop();
                Console.WriteLine($"Время добавления {N} строк с 10 ненулевыми элементами: { sWatch.ElapsedMilliseconds.ToString()} мс");

                sWatch.Start();
                N = 100;
                for (int i = 0; i < N; i++)
                {
                    await sparseMatrixRepSeq.AddRowAsync(new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 });
                }
                sWatch.Stop();
                Console.WriteLine($"Время добавления {N} строк с 20 ненулевыми элементами: { sWatch.ElapsedMilliseconds.ToString()} мс");

                sWatch.Restart();
                N = 1000;                
                for (int i = 0; i < N; i++)
                {
                    await sparseMatrixRepSeq.AddRowAsync(new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 });
                }
                sWatch.Stop();
                Console.WriteLine($"Время добавления {N} строк: { sWatch.ElapsedMilliseconds.ToString()} мс");
                #endregion
            }

        }

        /// <summary>
        /// 4. Запросы на поиск элементов и групп элементов
        /// </summary>
        private static async Task GetElementTests()
        {
            using (SparseMatrixRepSeq sparseMatrixRepSeq = new SparseMatrixRepSeq("4"))
            {
                sparseMatrixRepSeq.PrintMatrixFileNames();

                #region Добавление отдельных строк

                await CreateTestMatrix(sparseMatrixRepSeq);
                
                await GetElementAsyncTest(sparseMatrixRepSeq, 0, 0);
                await GetElementAsyncTest(sparseMatrixRepSeq, -1, 0);
                await GetElementAsyncTest(sparseMatrixRepSeq, 0, 100);
                await GetElementAsyncTest(sparseMatrixRepSeq, 1, 5);
                await GetElementAsyncTest(sparseMatrixRepSeq, 2, 5);
                await GetElementAsyncTest(sparseMatrixRepSeq, 5, 0);
                await GetElementAsyncTest(sparseMatrixRepSeq, 5, 5);
                await GetElementAsyncTest(sparseMatrixRepSeq, 5, 11);
                await GetElementAsyncTest(sparseMatrixRepSeq, 5, 12);
                #endregion
            }
        }

        /// <summary>
        /// 5. Работа с последовательностями элементов
        /// </summary>
        private static async Task DataSequencesTests()
        {
            using (SparseMatrixRepSeq sparseMatrixRepSeq = new SparseMatrixRepSeq("5"))
            {
                await sparseMatrixRepSeq.ClearAsync();

                Console.WriteLine(sparseMatrixRepSeq.GetNumSequences());
                // Объявляем последовательности элементов
                double[] seq0 = new double[] { -5.55, 0, 15, 211, 0.09 };
                double[] seq1 = new double[] { 1.1, 2.2, -0.6 };
                double[] seq2 = new double[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 };
                double[] seq3 = new double[] { 1000, 0, 15, 211, 0.09 };
                double[] seq4 = new double[] { -4.55, 4, 45, 411, 4.09, 43.98, 4, -4, -44.432 };
                #region Добавление последовательностей
                Console.WriteLine("Добавление последовательности double[] seq0 = new double[] { -5.55, 0, 15, 211, 0.09 };");
                int seqIndex0 = await sparseMatrixRepSeq.AddSequence(seq0);
                await sparseMatrixRepSeq.PrintMatrixSequenceIndexFile();
                await sparseMatrixRepSeq.PrintMatrixSequenceDataFile();

                Console.WriteLine("Добавление последовательности double[] seq1 = new double[] { 1.1, 2.2, -0.6 };");
                int seqIndex1 = await sparseMatrixRepSeq.AddSequence(seq1);
                await sparseMatrixRepSeq.PrintMatrixSequenceIndexFile();
                await sparseMatrixRepSeq.PrintMatrixSequenceDataFile();

                Console.WriteLine("Добавление последовательности double[] seq2 = new double[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 };");
                int seqIndex2 = await sparseMatrixRepSeq.AddSequence(seq2);
                await sparseMatrixRepSeq.PrintMatrixSequenceIndexFile();
                await sparseMatrixRepSeq.PrintMatrixSequenceDataFile();

                Console.WriteLine("Добавление последовательности double[] seq3 = new double[] { 1000, 0, 15, 211, 0.09 };");
                int seqIndex3 = await sparseMatrixRepSeq.AddSequence(seq3);
                await sparseMatrixRepSeq.PrintMatrixSequenceIndexFile();
                await sparseMatrixRepSeq.PrintMatrixSequenceDataFile();

                Console.WriteLine("Добавление последовательности double[] seq4 = new double[] { -4.55, 4, 45, 411, 4.09, 43.98, 4, -4, -44.432 };");
                int seqIndex4 = await sparseMatrixRepSeq.AddSequence(seq4);
                await sparseMatrixRepSeq.PrintMatrixSequenceIndexFile();
                await sparseMatrixRepSeq.PrintMatrixSequenceDataFile();
                #endregion

                #region Добавление координат размещения первых элементов последовательностей
                Console.WriteLine("Добавление координаты размещения последовательности AddSequenceCoord(seqIndex0, 0, 5)");
                int sequenceCoordIndex0   = await sparseMatrixRepSeq.AddSequenceCoord(seqIndex0, 0, 5);
                await sparseMatrixRepSeq.PrintMatrixSequenceCoordinatesFile();
                Console.WriteLine("Добавление координаты размещения последовательности AddSequenceCoord(seqIndex1, 1, 5)");
                int sequenceCoordIndex1   = await sparseMatrixRepSeq.AddSequenceCoord(seqIndex1, 1, 5);
                await sparseMatrixRepSeq.PrintMatrixSequenceCoordinatesFile();
                Console.WriteLine("Добавление координаты размещения последовательности AddSequenceCoord(seqIndex2, 2, 5)");
                int sequenceCoordIndex2   = await sparseMatrixRepSeq.AddSequenceCoord(seqIndex2, 2, 5);
                await sparseMatrixRepSeq.PrintMatrixSequenceCoordinatesFile();
                Console.WriteLine("Добавление координаты размещения последовательности AddSequenceCoord(seqIndex3, 3, 5)");
                int sequenceCoordIndex3   = await sparseMatrixRepSeq.AddSequenceCoord(seqIndex3, 3, 5);
                await sparseMatrixRepSeq.PrintMatrixSequenceCoordinatesFile();
                Console.WriteLine("Добавление координаты размещения последовательности AddSequenceCoord(seqIndex4, 4, 5)");
                int sequenceCoordIndex4_1 = await sparseMatrixRepSeq.AddSequenceCoord(seqIndex4, 4, 5);
                await sparseMatrixRepSeq.PrintMatrixSequenceCoordinatesFile();
                Console.WriteLine("Добавление координаты размещения последовательности AddSequenceCoord(seqIndex4, 5, 6)");
                int sequenceCoordIndex4_2 = await sparseMatrixRepSeq.AddSequenceCoord(seqIndex4, 5, 6);
                await sparseMatrixRepSeq.PrintMatrixSequenceCoordinatesFile();
                #endregion

                await sparseMatrixRepSeq.SetNumColsAsync(15);

                #region Запрос строк из последовательностей
                Console.WriteLine("Запрос строки 0");
                await sparseMatrixRepSeq.PrintRowFromSequence(0);
                Console.WriteLine("Запрос строки 1");
                await sparseMatrixRepSeq.PrintRowFromSequence(1);
                Console.WriteLine("Запрос строки 2");
                await sparseMatrixRepSeq.PrintRowFromSequence(2);
                Console.WriteLine("Запрос строки 3");
                await sparseMatrixRepSeq.PrintRowFromSequence(3);
                Console.WriteLine("Запрос строки 4");
                await sparseMatrixRepSeq.PrintRowFromSequence(4);
                Console.WriteLine("Запрос строки 5");
                await sparseMatrixRepSeq.PrintRowFromSequence(5);
                #endregion
                              
                
            }
            Console.WriteLine("---Successful end of method---");
        }

        /// <summary>
        /// 6. Добавление последовательностей элементов в массив
        /// </summary>
        private static async Task DataSequenceAddRowsTests()
        {
            using (SparseMatrixRepSeq sparseMatrixRepSeq = new SparseMatrixRepSeq("6"))
            {
                await sparseMatrixRepSeq.ClearAsync();
                double[] seq0 = new double[] { -5.55, 0, 15, 211, 0.09 };
                var tuple0 = await sparseMatrixRepSeq.AddRowsSequenceAsync(seq0,
                    new int[] { 0, 1, 2, 3, 4, 5 },
                    new int[] { 2, 2, 3, 3, 4, 4 });
                await sparseMatrixRepSeq.PrintRowsFromSequences();

                double[] seq1 = new double[] { -1, -2, -3 };
                var tuple1 = await sparseMatrixRepSeq.AddRowsSequenceAsync(seq1,
                    new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 },
                    new int[] { 0, 1, 2, 3, 4, 5, 6, 7,  8,  9 });
                await sparseMatrixRepSeq.PrintRowsFromSequences();
            }
            Console.WriteLine("---Successful end of method---");
        }

        /// <summary>
        /// 7. Работа с ленточными последовательностями элементов
        /// </summary>
        private static async Task DataSequencesBandTests()
        {
            using (SparseMatrixRepSeq sparseMatrixRepSeq = new SparseMatrixRepSeq("7"))
            {
                await sparseMatrixRepSeq.ClearAsync();

                Console.WriteLine(sparseMatrixRepSeq.GetNumSequences());
                // Объявляем последовательности элементов
                double[] seq0 = new double[] { -5.55, 0, 15, 211, 0.09 };
                double[] seq1 = new double[] { 1.1, 2.2, -0.6 };
                double[] seq2 = new double[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 };
                double[] seq3 = new double[] { 1000, 0, 15, 211, 0.09 };
                double[] seq4 = new double[] { -4.55, 4, 45, 411, 4.09, 43.98, 4, -4, -44.432 };
                #region Добавление последовательностей
                Console.WriteLine("Добавление последовательности double[] seq0 = new double[] { -5.55, 0, 15, 211, 0.09 };");
                int seqIndex0 = await sparseMatrixRepSeq.AddSequence(seq0);
                await sparseMatrixRepSeq.PrintMatrixSequenceIndexFile();
                await sparseMatrixRepSeq.PrintMatrixSequenceDataFile();

                Console.WriteLine("Добавление последовательности double[] seq1 = new double[] { 1.1, 2.2, -0.6 };");
                int seqIndex1 = await sparseMatrixRepSeq.AddSequence(seq1);
                await sparseMatrixRepSeq.PrintMatrixSequenceIndexFile();
                await sparseMatrixRepSeq.PrintMatrixSequenceDataFile();

                Console.WriteLine("Добавление последовательности double[] seq2 = new double[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 };");
                int seqIndex2 = await sparseMatrixRepSeq.AddSequence(seq2);
                await sparseMatrixRepSeq.PrintMatrixSequenceIndexFile();
                await sparseMatrixRepSeq.PrintMatrixSequenceDataFile();

                Console.WriteLine("Добавление последовательности double[] seq3 = new double[] { 1000, 0, 15, 211, 0.09 };");
                int seqIndex3 = await sparseMatrixRepSeq.AddSequence(seq3);
                await sparseMatrixRepSeq.PrintMatrixSequenceIndexFile();
                await sparseMatrixRepSeq.PrintMatrixSequenceDataFile();

                Console.WriteLine("Добавление последовательности double[] seq4 = new double[] { -4.55, 4, 45, 411, 4.09, 43.98, 4, -4, -44.432 };");
                int seqIndex4 = await sparseMatrixRepSeq.AddSequence(seq4);
                await sparseMatrixRepSeq.PrintMatrixSequenceIndexFile();
                await sparseMatrixRepSeq.PrintMatrixSequenceDataFile();
                #endregion

                #region Добавление координат размещения первых элементов последовательностей
                Console.WriteLine("Добавление координаты размещения последовательности AddSequenceBandCoord(seqIndex0, 0, 5, 2)");
                int sequenceCoordIndex0 = await sparseMatrixRepSeq.AddSequenceBandCoord(seqIndex0, 0, 5, 2);
                await sparseMatrixRepSeq.PrintMatrixSequenceBandCoordinatesFile();
                Console.WriteLine("Добавление координаты размещения последовательности AddSequenceBandCoord(seqIndex1, 2, 5, 2)");
                int sequenceCoordIndex1 = await sparseMatrixRepSeq.AddSequenceBandCoord(seqIndex1, 2, 5, 2);
                await sparseMatrixRepSeq.PrintMatrixSequenceBandCoordinatesFile();
                Console.WriteLine("Добавление координаты размещения последовательности AddSequenceBandCoord(seqIndex2, 4, 5, 2)");
                int sequenceCoordIndex2 = await sparseMatrixRepSeq.AddSequenceBandCoord(seqIndex2, 4, 5, 2);
                await sparseMatrixRepSeq.PrintMatrixSequenceBandCoordinatesFile();
                Console.WriteLine("Добавление координаты размещения последовательности AddSequenceBandCoord(seqIndex3, 6, 5, 2)");
                int sequenceCoordIndex3 = await sparseMatrixRepSeq.AddSequenceBandCoord(seqIndex3, 6, 5, 2);
                await sparseMatrixRepSeq.PrintMatrixSequenceBandCoordinatesFile();
                Console.WriteLine("Добавление координаты размещения последовательности AddSequenceBandCoord(seqIndex4, 8, 5, 2)");
                int sequenceCoordIndex4_1 = await sparseMatrixRepSeq.AddSequenceBandCoord(seqIndex4, 8, 5, 2);
                await sparseMatrixRepSeq.PrintMatrixSequenceBandCoordinatesFile();
                Console.WriteLine("Добавление координаты размещения последовательности AddSequenceBandCoord(seqIndex4, 10, 6, 2)");
                int sequenceCoordIndex4_2 = await sparseMatrixRepSeq.AddSequenceBandCoord(seqIndex4, 10, 6, 2);
                await sparseMatrixRepSeq.PrintMatrixSequenceBandCoordinatesFile();
                #endregion

                await sparseMatrixRepSeq.SetNumColsAsync(20);

                #region Запрос строк из последовательностей
                Console.WriteLine("Запрос строки 0");
                await sparseMatrixRepSeq.PrintRowFromSequenceBand(0);
                Console.WriteLine("Запрос строки 1");
                await sparseMatrixRepSeq.PrintRowFromSequenceBand(1);
                Console.WriteLine("Запрос строки 2");
                await sparseMatrixRepSeq.PrintRowFromSequenceBand(2);
                Console.WriteLine("Запрос строки 3");
                await sparseMatrixRepSeq.PrintRowFromSequenceBand(3);
                Console.WriteLine("Запрос строки 4");
                await sparseMatrixRepSeq.PrintRowFromSequenceBand(4);
                Console.WriteLine("Запрос строки 5");
                await sparseMatrixRepSeq.PrintRowFromSequenceBand(5);
                Console.WriteLine("Запрос строки 6");
                await sparseMatrixRepSeq.PrintRowFromSequenceBand(6);
                Console.WriteLine("Запрос строки 7");
                await sparseMatrixRepSeq.PrintRowFromSequenceBand(7);
                Console.WriteLine("Запрос строки 8");
                await sparseMatrixRepSeq.PrintRowFromSequenceBand(8);
                Console.WriteLine("Запрос строки 9");
                await sparseMatrixRepSeq.PrintRowFromSequenceBand(9);
                Console.WriteLine("Запрос строки 10");
                await sparseMatrixRepSeq.PrintRowFromSequenceBand(10);
                Console.WriteLine("Запрос строки 11");
                await sparseMatrixRepSeq.PrintRowFromSequenceBand(11);
                Console.WriteLine("Запрос несуществующей строки 12");
                await sparseMatrixRepSeq.PrintRowFromSequenceBand(12);
                #endregion


            }
            Console.WriteLine("---Successful end of method---");
        }

        /// <summary>
        /// 8. Добавление ленточных последовательностей элементов в массив
        /// </summary>
        private static async Task DataSequenceBandAddRowsTests()
        {
            using (SparseMatrixRepSeq sparseMatrixRepSeq = new SparseMatrixRepSeq("8"))
            {
                await sparseMatrixRepSeq.ClearAsync();
                double[] seq0 = new double[] { -5.55, 0, 15, 211, 0.09 };
                var tuple0 = await sparseMatrixRepSeq.AddRowsSequenceBandAsync(seq0,
                    5, 2, 15);
                await sparseMatrixRepSeq.PrintRowsFromSequencesBand();
                                
            }
            Console.WriteLine("---Successful end of method---");
        }

        /// <summary>
        /// 9. Создание комбинированной матрицы
        /// </summary>
        private static async Task SparseMatrixRepSeqCreateAndReadTests()
        {
            using (SparseMatrixRepSeq sparseMatrixRepSeq = new SparseMatrixRepSeq("9"))
            {
                await sparseMatrixRepSeq.ClearAsync();

                await sparseMatrixRepSeq.AddRowAsync(new double[] { 0.11, 1.11, 2.22, 0, 0, -5.55, 0, 0, -8.88, -9.99 });
                await sparseMatrixRepSeq.PrintMatrix();
                Console.WriteLine("-------------------\n");
                double[] seq0 = new double[] { -5.55, 0, 15, 211, 0.09 };
                var tuple0 = await sparseMatrixRepSeq.AddRowsSequenceBandAsync(seq0,
                    5, 2, 15);
                await sparseMatrixRepSeq.PrintMatrix();
                Console.WriteLine("-------------------\n");
                double[] seq1 = new double[] { -1, -2, -3 };
                var tuple1 = await sparseMatrixRepSeq.AddRowsSequenceAsync(seq1,
                    new int[] { 1, 2 },
                    new int[] { 1, 1 });
                await sparseMatrixRepSeq.PrintMatrix();               
                
            }
            Console.WriteLine("---Successful end of method---");
        }

        /// <summary>
        /// 10. Создание СЛАУ
        /// </summary>
        /// <returns></returns>
        private static async Task SlaeCreateAndReadTests()
        {
            using (SLAE slae = new SLAE("10"))
            {
                await slae.ClearAsync();

                await slae.SparseMatrixRepSeq.AddRowAsync(new double[] { 0.11, 1.11, 2.22, 0, 0, -5.55, 0, 0, -8.88, -9.99 });
                await slae.SparseMatrixRepSeq.AddRowAsync(new double[] { 4, 0, 2, 0, 0, -5, 0, 0, -9, -10 });
                await slae.SparseMatrixRepSeq.AddRowAsync(new double[] { 2, 1, 0, 0, 0, -6, 0, 0, -4, -20 });
                await slae.SparseMatrixRepSeq.AddRowAsync(new double[] { 1, 0, 8, 0, 0, -5, 0, 0, -1, -30 });
                await slae.SparseMatrixRepSeq.PrintMatrix();
                Console.WriteLine("-------------------\n");
                double[] seq0 = new double[] { -5.55, 0, 15, 211, 0.09 };
                var tuple0 = await slae.SparseMatrixRepSeq.AddRowsSequenceBandAsync(seq0,
                    4, 0, 11);
                await slae.SparseMatrixRepSeq.PrintMatrix();
                Console.WriteLine("-------------------\n");
                double[] seq1 = new double[] { -1, -2, -3 };
                var tuple1 = await slae.SparseMatrixRepSeq.AddRowsSequenceAsync(seq1,
                    new int[] { 5, 6 },
                    new int[] { 2, 1 });
                await slae.SparseMatrixRepSeq.PrintMatrix();

                try
                {
                    await slae.SetRightSideAsync(new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 });
                    await slae.PrintRightSide();

                    try
                    {
                        long elapsedTime = await slae.SolveCuda();
                        Console.WriteLine($"Время решения СЛАУ: {elapsedTime} мс");
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.Message);
                    }

                    await slae.PrintResult();
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
                
            }
            Console.WriteLine("---Successful end of method---");
        }        

        /// <summary>
        /// Тестовый запрос на выборку отдельного элемента массива с выводом информации в консоль
        /// </summary>
        /// <param name="sparseMatrixRepSeq"></param>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        private static async Task<double> GetElementAsyncTest(SparseMatrixRepSeq sparseMatrixRepSeq, int rowIndex, int colIndex)
        {
            double element = 0;
            try
            {
                element = await sparseMatrixRepSeq.GetElementAsync(rowIndex, colIndex);
                Console.WriteLine($"[{rowIndex}, {colIndex}] = {element}");
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }

            return element;
        }

        /// <summary>
        /// Создание тестовой матрицы
        /// </summary>
        /// <param name="sparseMatrixRepSeq"></param>
        /// <returns></returns>
        private static async Task CreateTestMatrix(SparseMatrixRepSeq sparseMatrixRepSeq)
        {
            // Очистка матрицы
            await sparseMatrixRepSeq.ClearAsync();

            // Добавление строки  | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
            Console.WriteLine("Добавление строки  | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |");
            // Добавление строки index:              0    | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 |
            //                   value:              0    | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
            await sparseMatrixRepSeq.AddRowAsync(new double[] { 0 }, 0);
            
            // Добавление строки  | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
            Console.WriteLine("Добавление строки  | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |");
            // Добавление строки index:              0    | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 |
            //                   value:              0    | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
            await sparseMatrixRepSeq.AddRowAsync(new double[] { 0 }, 5);
            
            // Добавление строки  | 0.11 | 1.11 | 2.22 | 0 | 0 | -5.55 | 0 | 0 | -8.88 | -9.99 |
            Console.WriteLine("Добавление строки  | 0.11 | 1.11 | 2.22 | 0 | 0 | -5.55 | 0 | 0 | -8.88 | -9.99 |");
            // Добавление строки index:              0    | 1    | 2    | 3 | 4 |  5    | 6 | 7 |  8    |  9    |
            //                   value:              0.11 | 1.11 | 2.22 | 0 | 0 | -5.55 | 0 | 0 | -8.88 | -9.99 |
            await sparseMatrixRepSeq.AddRowAsync(new double[] { 0.11, 1.11, 2.22, 0, 0, -5.55, 0, 0, -8.88, -9.99 });
            
            // Добавление строки  | 0 | 0 | 2.55 | 0 | -1000 | 0 | 50.0001 | 0 | 0 | 0 |
            Console.WriteLine("Добавление строки  | 0 | 0 | 2.55 | 0 | -1000 | 0 | 50.01 | 0 | 0 | 0 |");
            // Добавление строки index:              0    | 1    | 2    | 3 | 4     | 5   |       6 | 7 | 8 | 9 |
            //                   value:              0    | 0    | 2.55 | 0 | -1000 | 0   | 50.01 | 0 | 0 | 0 |
            await sparseMatrixRepSeq.AddRowAsync(new double[] { 0, 0, 2.55, 0, -1000, 0, 50.01, 0, 0, 0 });
            

            // Добавление строки  | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
            Console.WriteLine("Добавление строки  | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |");
            // Добавление строки index:              0    | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 |
            //                   value:              0    | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
            await sparseMatrixRepSeq.AddRowAsync(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            


            // Добавление строки  | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 |
            Console.WriteLine("Добавление строки  | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 |");
            // Добавление строки index:              | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 |
            //                   value:              | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 |
            await sparseMatrixRepSeq.AddRowAsync(new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 });

            // Добавление строки  | 0 | 1 | 2  |
            Console.WriteLine("Добавление строки  | 0 | 1 | 2  |");
            // Добавление строки index:              | 0 | 1 | 2 |
            //                   value:              | 0 | 1 | 2 |
            await sparseMatrixRepSeq.AddRowAsync(new double[] { 0, 1, 2 }, 5);

            // Добавление строки  | 0 |
            Console.WriteLine("Добавление строки  | 0 |");
            // Добавление строки index:              | 0 |
            //                   value:              | 0 |
            await sparseMatrixRepSeq.AddRowAsync(new double[] { 0 });

            await sparseMatrixRepSeq.PrintMatrixDescription();
            await sparseMatrixRepSeq.PrintRowsFromNnzSeparateElements();
            Console.WriteLine();
        }
    }
}
