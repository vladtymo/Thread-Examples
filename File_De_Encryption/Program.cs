using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace File_De_Encryption
{
    class Program
    {
        static void Encryption(object obj)
        {
            string fileName = (string)obj;
            for (int i = 0; /*i < 100000*/; i++)
            {
                if (i % 250 == 0)
                    Console.WriteLine("{1} - Encrypting {0}...", obj, i);
                //Thread.Sleep(100);
            }
        }
        static void Decryption(object obj)
        {
            string fileName = (string)obj;
            for (int i = 0; /*i < 100000*/; i++)
            {
                if (i % 250 == 0)
                    Console.WriteLine("{1} - Decrypting {0}...", obj, i);
                //Thread.Sleep(100);
            }
        }
        static void Main(string[] args)
        {
            // Создание делегата, который будет связан с методом
            // шифрования или дешифрования
            ParameterizedThreadStart Param = null;
            while (true)
            {
                // Меню, где пользователю предлагается выбрать
                // действие. 
                Console.Clear();
                Console.WriteLine("1. Encryption");
                Console.WriteLine("2. Decryption");
                ConsoleKeyInfo Select = Console.ReadKey(true);
                Console.Clear();           

                if (ConsoleKey.D1 == Select.Key)
                {
                    // Если пользователь выбрал шифрование,
                    // мы связаваем делегат с методом шифрования
                    Param = new ParameterizedThreadStart(Encryption);
                    Console.WriteLine("Enter file name to encrypt: ");
                }
                else if (ConsoleKey.D2 == Select.Key)
                {
                    // Если пользователь выбрал дешифрование,
                    // мы связываем делегат с методом дешифрования
                    Param = new ParameterizedThreadStart(Decryption);
                    Console.WriteLine("Enter file name to decrypt: ");
                }
                if (ConsoleKey.D1 == Select.Key || ConsoleKey.D2 == Select.Key)
                {
                    // Пользователь вводит путь к файлу, 
                    // с которым собирается работать
                    string FilePath = Console.ReadLine();
                    // Создание потока, который будет шифровать/дешифровать
                    Thread thread = new Thread(Param);
                    // Старт потока в параметр передается путь к файлу
                    thread.Start((object)FilePath);

                    Console.WriteLine("Enter symbol to do action...");
                    do
                    {
                        // В цикле пользователю предлагаются выбрать
                        // действие с потоком
                        Console.WriteLine("[c] Close thread");

                        Console.WriteLine("[p] Pause/Resume thread");

                        ConsoleKeyInfo Selects = Console.ReadKey(true);
                        if (Selects.Key == ConsoleKey.C)
                        {
                            if (thread.ThreadState == ThreadState.Running)
                            {
                                // Если поток выполнялся
                                // и пользователь выбрал завершение
                                thread.Abort(); // Завершаем работу потока
                                Console.WriteLine("Thread is stoped");
                            }
                        }

                        else if (Selects.Key == ConsoleKey.P)
                        {
                            if (thread.ThreadState == ThreadState.Running)
                            {                                
                                // Если поток выполнялся
                                // и пользователь выбрал приостановление

                                thread.Suspend(); // Приостанавливаем поток
                                Console.WriteLine("Thread is paused");
                            }
                            else if (thread.ThreadState == ThreadState.Suspended)
                            {
                                Console.WriteLine("Going to resume !!!");
                                // если поток остановлен
                                // и пользователь выбрал возобновление
                                thread.Resume(); // Возобновляем поток
                                Console.WriteLine("Thread is resumed");
                            }
                            Thread.Sleep(100);
                        }
                        // Если поток приостановлен или работает,
                        // то показать это меню пользователю еще
                    } while (thread.ThreadState == ThreadState.Suspended || thread.ThreadState == ThreadState.Running);

                    Console.ReadKey(true);
                    Console.Clear();
                }
            }
        }
    }
}