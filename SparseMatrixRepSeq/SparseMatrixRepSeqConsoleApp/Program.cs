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

            Console.WriteLine("1. Тестирование класса FileBinaryDouble");
            Console.WriteLine("2. Тестирование класса FileBinaryInt");
            Console.WriteLine("3. Создание пустой матрицы");            
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
        /// 3. Тестирование класса FileBinaryDouble
        /// </summary>
        private static void CreateSparceMatrixRepSeqWith0Elements()
        {
            SparseMatrixRepSeq sparseMatrixRepSeq = new SparseMatrixRepSeq("1");

            Console.WriteLine(sparseMatrixRepSeq.ToString());
            Console.WriteLine(sparseMatrixRepSeq.GetPathToDataFile);
        }
               

        
    }
}
