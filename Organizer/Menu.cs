using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Organizer
{
    class Menu
    {
        private void DisplayMainMenu()
        {
            {
                Console.Clear();
                //Выводим меню, его пункты с соответствующими цифрами\символами
                Console.WriteLine("### MENU ###");
                Console.WriteLine("1. Узнать погоду");
                Console.WriteLine("2. Чтение и запись файлов");
                Console.WriteLine("3. Запустить программу");
                Console.WriteLine("4. Отправить письмо");
                Console.Write("\n" + "Введите команду: ");

                char ch = char.Parse(Console.ReadLine()); //Тут желательно сделать проверку, или считывать всю строку, и в switch уже отсеивать

                switch (ch)
                {
                    case '1':
                        GetInfo();
                        break;
                    case '2':
                        DisplayReadWrite();
                        break;
                    case '3':
                        StartApp();
                        break;
                    case '4':
                        SendMessage();
                        break;
                }
            }
        }

        private void DisplayReadWrite()
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
        
        private void StartApp()
        {
            Console.WriteLine("1. Блокнот");
            Console.WriteLine("2. Калькулятор");
            Console.WriteLine("3. Paint");
            Console.WriteLine("4. Excel");
            int input = int.Parse(Console.ReadLine());
            string[] programs = { "Notepad.exe", "calc", "mspaint", "excel" };
            Process.Start(programs[input - 1]);
        }

        private void GetInfo()
        {
            Console.Write("Ваш город: ");
            string city = Console.ReadLine();
            string url = "http://api.openweathermap.org/data/2.5/weather?q=" + city + "&units=metric&appid=714c8a7af174f3905091375bfd001e49";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string response;
            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }
            WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(response);
            Console.WriteLine("Temperature in {0}: {1} C", weatherResponse.Name, weatherResponse.Main.Temp);
            Console.WriteLine("Temperature feels like: {0} C", weatherResponse.Main.Feels_Like);
            Console.WriteLine("Min Temperature: {0} C", weatherResponse.Main.Temp_Min);
            Console.WriteLine("MaxTemperature: {0} C", weatherResponse.Main.Temp_Max);
            Console.WriteLine("Pressure in {0}: {1} hpa", weatherResponse.Name, weatherResponse.Main.Pressure);
            Console.WriteLine("Humidity in {0}: {1} %", weatherResponse.Name, weatherResponse.Main.Humidity);
            Console.ReadKey();
        }
        
        private void SendMessage()
        {
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            Console.Write("Ваш e-mail: ");
            string yourmail = Console.ReadLine();
            Console.Write("Ваше имя: ");
            string yourname = Console.ReadLine();
            MailAddress from = new MailAddress(yourmail, yourname);
            // кому отправляем
            Console.Write("E-mail получателя: ");
            string tomail = Console.ReadLine();
            MailAddress to = new MailAddress(tomail);
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            Console.Write("Тема: ");
            string subj = Console.ReadLine();
            m.Subject = subj;
            // текст письма
            Console.Write("Текст: ");
            string text = Console.ReadLine();
            m.Body = "<h2>" + text + "</h2>";
            // письмо представляет код html
            m.IsBodyHtml = true;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            // логин и пароль
            Console.Write("Ваш login: ");
            string log = Console.ReadLine();
            Console.Write("Ваш password: ");
            string pas = Console.ReadLine();
            smtp.Credentials = new NetworkCredential(log, pas);
            smtp.EnableSsl = true;
            smtp.Send(m);
            Console.WriteLine("Письмо отправлено");
            Console.ReadKey();
        }

        public void Start()
        {
            while (true)
            {
                DisplayMainMenu();
            }
        }
    }
}
