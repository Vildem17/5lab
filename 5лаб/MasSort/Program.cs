using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MergeSortThreading
{
    class Program
    {
#pragma warning disable IDE0060 // Remove unused parameter
        static void Main(string[] args)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            _ = Thread.CurrentThread; //Инициализация потока 

            /* t.Name = "Главный поток"; //Вывод информации о потоке
             Console.WriteLine($"Имя потока: {t.Name}");
             Console.WriteLine($"Включён ли поток: {t.IsAlive}");
             Console.WriteLine($"Приоритет потока: {t.Priority}");
             Console.WriteLine($"Статус потока: {t.ThreadState}");*/

            int[] arr = new int[65]; 

            Random rnd = new Random(); 

            for (int i = 0; i < arr.Length; i++) 
            {
                arr[i] = rnd.Next(100);
            }

            for (int i = 0; i < arr.Length; i++) 
            {
                Console.Write($"{arr[i]}, ");
            }
            Console.WriteLine();

            int n;

            while (true) 
            {
                Console.Write("Введите количество потоков(N)(1,2,4,8) = ");
                int.TryParse(Console.ReadLine(), out n);
                if (n == 1 || n == 2 || n == 4 || n == 8) break;
                else Console.WriteLine("Некорректный ввод");
            }


            MultithreadedSort(arr, n); 


            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write($"{arr[i]}, ");
            }
            Console.WriteLine();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void MultithreadedSort(int[] arr, int threads)
        {
            int[][] arrs = new int[threads][];

            Thread[] sorting = new Thread[threads]; 

            for (int i = 0; i < threads; i++) 
            {
                arrs[i] = new int[arr.Length / threads]; 

                Array.Copy(arr, i * (arr.Length / threads), arrs[i], 0, arr.Length / threads); 

                sorting[i] = new Thread(new ParameterizedThreadStart(Sort)); 

                sorting[i].Start(arrs[i]); 
            }

            for (int i = 0; i < threads; i++) 

                sorting[i].Join(); 

            bool norm = false; 

            while (arrs.Length != 1) 
            {
                int k = 0;
                int[][] tmp = new int[arrs.Length / 2][];

                for (int i = 0; i < arrs.Length; i++) 
                {
                    if (arr.Length % 2 != 0 && !norm) 
                    {
                        tmp[k] = new int[arrs[i].Length + arrs[i + 1].Length + 1];
                        tmp[k][tmp[k].Length - 1] = arr[arr.Length - 1];
                        norm = true;
                    }
                    else
                    {
                        tmp[k] = new int[arrs[i].Length + arrs[i + 1].Length]; 
                    }
                    Array.Copy(arrs[i], 0, tmp[k], 0, arrs[i].Length); 
                    try
                    {
                        Array.Copy(arrs[i + 1], 0, tmp[k], arrs[i].Length, arrs[i + 1].Length); 
                    }
                    catch (Exception)
                    {

                    }
                    i++;
                    k++;
                }

                Thread[] merging = new Thread[tmp.Length];  

                for (int j = 0; j < tmp.Length; j++)
                {
                    merging[j] = new Thread(new ParameterizedThreadStart(Sort)); 

                    merging[j].Start(tmp[j]); 
                }

                for (int i = 0; i < tmp.Length; i++) 
                    merging[i].Join();

                arrs = tmp; 
            }

            arrs[0].CopyTo(arr, 0); 
        }

        public static void Sort(object arr) 
        {
            Array.Sort((int[])arr, 0, ((int[])arr).Length);
        }
    }
}
