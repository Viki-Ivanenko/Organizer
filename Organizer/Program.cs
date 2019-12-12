using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu();
        }

        static public void Menu()
        {
            while (true)
            {
                Console.Clear();
                //Выводим меню, его пункты с соответствующими цифрами\символами
                Console.WriteLine("### MENU ###");
                Console.WriteLine("1. Пункт меню");
                Console.WriteLine("2. Чтение и запись файлов");
                Console.WriteLine("3. Пункт меню");
                Console.WriteLine("4. Exit");
                Console.Write("\n" + "Введите команду: ");

                char ch = char.Parse(Console.ReadLine()); //Тут желательно сделать проверку, или считывать всю строку, и в switch уже отсеивать

                switch (ch)
                {
                    case '1':
                        Console.WriteLine("### Hello1 ###");
                        Console.ReadKey();
                        break;
                    case '2':
                        Read_Write();
                        break;
                    case '3':
                        Start_app();
                        break;
                    case '4':
                        Console.WriteLine("### Hello4 ###");
                        break;
                }
            }
        }


        public static void Read_Write()
        {
            // создаем каталог для файла
            string path = @"E:\\Files";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            Console.WriteLine("Вы хотите записать (1) или читать (2) файл?");
            char answer = char.Parse((Console.ReadLine()));


            if (answer == '1')
            {
                Console.WriteLine("Введите название файла:");
                string file_name = Console.ReadLine();
                Console.WriteLine("Введите строку для записи в файл:");
                string text = Console.ReadLine();

                // запись в файл
                using (FileStream fstream = new FileStream($"{path}\\{file_name}", FileMode.OpenOrCreate))
                {
                    // преобразуем строку в байты
                    byte[] array = System.Text.Encoding.Default.GetBytes(text);
                    // запись массива байтов в файл
                    fstream.Write(array, 0, array.Length);
                    Console.WriteLine("Текст записан в файл");
                }
            }

            else if (answer == '2')
            {
                Console.WriteLine("Введите название файла:");
                string file_name_out = Console.ReadLine();
                // чтение из файла
                using (FileStream fstream = File.OpenRead($"{path}\\{file_name_out}"))
                {
                    // преобразуем строку в байты
                    byte[] array = new byte[fstream.Length];
                    // считываем данные
                    fstream.Read(array, 0, array.Length);
                    // декодируем байты в строку
                    string textFromFile = System.Text.Encoding.Default.GetString(array);
                    Console.WriteLine($"Текст из файла: {textFromFile}");
                }
            }

            Console.ReadLine();

        }

        public static void Start_app()
        {
            //Process.Start("Notepad.exe");
            //Process.Start("calc");
            //Process.Start("mspaint");
            Process.Start("excel");
        }
    }
}
