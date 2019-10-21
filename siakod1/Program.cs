using System;
using System.Collections.Generic;

namespace siakod1
{
    class Program
    {
        //27. Построить хеш - таблицу, содержащую последовательность из m = 56 элементов размерности n = 5. Элементы генерируются с помощью датчика случайных чисел.
        //Хеш - функция - f(k) =(k / 19) mod t.
        //Метод разрешения конфликта - квадратичные пробы.

        struct MyHashTable
        {
            public int?[] Table;
            public bool[] Deleted;
            public int SizeTable { get { return Table.Length; } }
            public int CountAttempt { get; set; }
            public bool IsLog { get; set; }

            public void IncCountAttempt()
            {
                CountAttempt++;
            }

            public MyHashTable(int sizeTable, bool isLog = false)
            {
                Table = new int?[sizeTable];
                Deleted = new bool[sizeTable];
                CountAttempt = 0;
                IsLog = isLog;
            }
            public int Hash(int key)
            {
                return (key / 19) % SizeTable;
            }

            public int Occupancy
            {
                get
                {
                    int count = 0;
                    for (int i = 0; i < Table.Length; i++)
                        if (Table[i] != null && !Deleted[i])
                            count++; 
                    return count;
                }      
            }

            public bool Remove(int key)
            {
                var findIndex = Find(key);
                if (findIndex != -1)
                {
                    Deleted[findIndex] = true;
                    if(IsLog)
                        Console.WriteLine($"[Удаление]Ключ {key} удален по индексу {findIndex}");
                    return true;
                }
                if (IsLog)
                    Console.WriteLine($"[Удаление]Ключ {key} не найден");
                return false;
            }

            //возвращает индекс, если не находит, то -1
            public int Find(int key) 
            {
                //Использование хэш-функции
                var index = this.Hash(key);
                var index0 = index;
                for (int i = 1; ; i++)
                {
                    //
                    if (Table[index] != null)
                    {
                        if (Table[index] == key && !Deleted[index])
                        {
                            if (this.IsLog)
                                Console.WriteLine($"\t[Поиск]Ключ {key} найден по индексу {index}");
                            
                            return index;
                        }
                        index = (index0 + (i * i)) % SizeTable;  
                    }
                    else
                    {
                        if (IsLog)
                            Console.WriteLine($"[Поиск]Ключ {key} не найден");
                        return -1;
                    }
                    if (i > SizeTable)
                        return -1;
                }
            }

            public bool Add(int key)
            {
                //Использование хэш-функции
                var index = this.Hash(key);
                var index0 = index;
                var maxAttempt = Occupancy;
                for (int i = 1; ; i++)
                {
                    this.IncCountAttempt(); //+1 проба
                    //если ячейка таблицы свободна, то вносим значение элемента
                    if (Table[index] == null)
                    {
                        Table[index] = key;
                        if(IsLog)
                            Console.WriteLine($"[Добавление]В таблицу вносится ключ {key} по индексу {index}");
                        break;
                    }
                    //если занята, то используем метод разрешения коллизий
                    //index0 хранит значение первой пробы
                    else
                    {
                        if(Table[index] == key)
                        {
                            if (this.IsLog)
                                Console.WriteLine($"\t[Добавление]Ключ {key} уже существует по индексу {index}");
                            return false;
                        }
                        if (this.IsLog)
                            Console.WriteLine($"\t[Добавление]Для ключа {key} индекс {index} занят. Будет попытка №{i + 1}");
                        index = (index0 + (i * i)) % SizeTable;
                    }
                    if(i > maxAttempt)
                    {
                        if (this.IsLog)
                            Console.WriteLine($"\t[Ошибка]Невозможно устранить коллизию");
                        return false;
                    }

                }
                return true;
            }

            public bool Replace(int source, int destination)
            {
                if (Remove(source))
                {
                    Add(destination);
                }
                
                return false;
            }

            public void PrintTable(int startNumber = 0, int columns = 4)
            {
                for (int i = 0; i < Table.Length; i++)
                {
                    var elem = Table[i];
                    if (Deleted[i])
                        elem = null;
                    Console.Write($"[{i + startNumber}] = {elem}   \t");
                    if (((i + 1) % columns == 0) && (i != 0))
                        Console.WriteLine();
                }
            }
        }

        

        static public void OutArray(int[] array, int startNumber = 0, int columns = 4)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write($"[{i + startNumber}] = { array[i]}   \t");
                if (((i + 1) % columns == 0) && (i != 0))
                    Console.WriteLine();
            }
        }


        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Title = "Хэш таблицы.html";
            //создается массив элементов размерности N = 56
            int NumberElements = 56;
            var array = new int[NumberElements];
            var rnd = new Random();

            //генерируются ключи размерности m = 5
            Console.WriteLine("Сгенерированные ключи:");
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = rnd.Next(10000, 100000);
            }
            OutArray(array, 1);

            
            int sizeTable = (int)Math.Round(NumberElements * 1.5);
            var hashTable = new MyHashTable(sizeTable, true);
            //var hashTable = new MyHashTable(sizeTable);

            Console.WriteLine();
            Console.WriteLine();

            //добавляем ключи в хэш таблицу
            for (int i = 0; i < array.Length; i++)
            {
                hashTable.Add(array[i]);
            }

            Console.WriteLine();

            Console.WriteLine("Коэффициент заполнения таблицы = {0:0.000}", (double)hashTable.Occupancy / sizeTable);
            Console.WriteLine("Среднее число проб = {0:0.000}", (double)hashTable.CountAttempt / NumberElements);

            Console.WriteLine();



            while (true)
            {
                Console.WriteLine("Хэш-таблица:");
                hashTable.PrintTable();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Интерактивный режим");
                Console.WriteLine("1.Добавить ключ");
                Console.WriteLine("2.Найти индекс по ключу");
                Console.WriteLine("3.Удалить ключ");
                Console.WriteLine("4.Заменить один ключ другим");
                Console.WriteLine("5.Вывести параметры");
                Console.WriteLine("0.Выход");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                Console.WriteLine("Выбор режима: ");
                var mode = Console.ReadLine();
                int key1, key2;
                string str;
                switch (mode[0])
                {
                    case '1':
                        Console.WriteLine();
                        Console.Write("Введите ключ = ");
                        str = Console.ReadLine();
                        if(!int.TryParse(str, out key1))
                            break;
                        if(hashTable.Add(key1))
                            NumberElements++;
                        Console.WriteLine("Нажмите любую клавишу, чтобы продолжить");
                        Console.ReadKey();
                        break;
                    case '2':
                        Console.WriteLine();
                        Console.Write("Введите ключ = ");
                        str = Console.ReadLine();
                        if (!int.TryParse(str, out key1))
                            break;
                        hashTable.Find(key1);
                        Console.WriteLine("Нажмите любую клавишу, чтобы продолжить");
                        Console.ReadKey();
                        break;
                    case '3':
                        Console.Write("Введите ключ = ");
                        str = Console.ReadLine();
                        if (!int.TryParse(str, out key1))
                            break;
                        if(hashTable.Remove(key1))
                            NumberElements--;
                        Console.WriteLine("Нажмите любую клавишу, чтобы продолжить");
                        Console.ReadKey();
                        break;
                    case '4':
                        Console.WriteLine();
                        Console.Write("Введите ключ для замены = ");
                        str = Console.ReadLine();
                        if (!int.TryParse(str, out key1))
                            break;
                        Console.Write("Введите ключ чем заменить = ");
                        str = Console.ReadLine();
                        if (!int.TryParse(str, out key2))
                            break;
                        hashTable.Replace(key1,key2);
                        Console.WriteLine("Нажмите любую клавишу, чтобы продолжить");
                        Console.ReadKey();
                        break;
                    case '5':
                        Console.WriteLine();
                        Console.WriteLine("Коэффициент заполнения таблицы = {0:0.000}", (double)hashTable.Occupancy / sizeTable);
                        Console.WriteLine("Среднее число проб = {0:0.000}", (double)hashTable.CountAttempt / NumberElements);
                        Console.WriteLine("Нажмите любую клавишу, чтобы продолжить");
                        Console.ReadKey();
                        break;
                    case '0':
                        return;
                    default:
                        break;
                }
            }
        }        
    }
}
