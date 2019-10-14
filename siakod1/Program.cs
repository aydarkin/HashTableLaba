using System;

namespace siakod1
{
    class Program
    {
        //27. Построить хеш - таблицу, содержащую последовательность из m = 56 элементов размерности n = 5. Элементы генерируются с помощью датчика случайных чисел.
        //Хеш - функция - f(k) =(k / 19) mod t.
        //Метод разрешения конфликта - квадратичные пробы.

        const int NumberElements = 56; //кол-во элементов

        struct MyHashTable
        {
            public int[] Table;
            public bool[] Busy;
            public int SizeTable { get { return Table.Length; } }
            public int CountAttempt { get; set; }
            public bool IsLog { get; set; }

            public void IncCountAttempt()
            {
                CountAttempt++;
            }

            public MyHashTable(int sizeTable, bool isLog = false)
            {
                Table = new int[sizeTable];
                Busy = new bool[sizeTable];
                CountAttempt = 0;
                IsLog = isLog;
            }
            public int Hash(int key)
            {
                return (key / 19) % SizeTable;
            }

            public void Add(int key)
            {
                //Использование хэш-функции
                var index = this.Hash(key);
                var index0 = index;

                for (int i = 1; ; i++)
                {
                    this.IncCountAttempt(); //+1 проба
                    //если ячейка таблицы свободна, то вносим значение элемента
                    if (!Busy[index])
                    {
                        Table[index] = key;
                        Busy[index] = true;
                        if(IsLog)
                            Console.WriteLine($"В таблицу вносится ключ {key} по индексу {index}");
                        break;
                    }
                    //если занята, то используем метод разрешения коллизий
                    //index0 хранит значение первой пробы
                    else
                    {
                        if (IsLog)
                            Console.WriteLine($"\tДля ключа {key} индекс {index} занят. Будет попытка №{i + 1}");
                        index = (index0 + (i * i)) % SizeTable;
                    }
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

            //создается массив элементов размерности N = 56
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
            Console.WriteLine();
            Console.WriteLine("Хэш-таблица:");
            OutArray(hashTable.Table);

            Console.WriteLine();
            Console.WriteLine("Коэффициент заполнения таблицы = {0:0.000}", (double)NumberElements / sizeTable);
            Console.WriteLine("Среднее число проб = {0:0.000}", (double)hashTable.CountAttempt / NumberElements);
            Console.WriteLine();
            Console.WriteLine("Готово");
            Console.Beep();
            Console.ReadKey();
        }

        
    }
}
