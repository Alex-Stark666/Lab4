using System;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Data.SQLite;

namespace LeapYearCalculator
{
    class calendar
    {
        //Лабораторная работа 3
        //Требуется дополнить реализацию лабораторной работы 2 таким образом,
        //чтобы она осуществляла сохранение и загрузку данных в форматах  JSON и XML,
        //а также в базу данных SQLite.
        //Выбор варианта сохранения и загрузки данных осуществляется из меню,
        //отображаемого пользователю при запуске программы, дополняя уже существующее (разработанное в рамках работы 2).

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Leap Year Calculator!");
            Console.WriteLine("Enter '1' to check if a year is a leap year.");
            Console.WriteLine("Enter '2' to calculate the length of the interval between two dates.");
            Console.WriteLine("Enter '3' to output the names of the day of the week according to the entered date.");
            Console.WriteLine("Enter '4' to save data in JSON format.");
            Console.WriteLine("Enter '5' to save data in XML format.");
            Console.WriteLine("Enter '6' to save data in SQLite database.");
            Console.WriteLine("Enter '7' to load data from JSON file.");
            Console.WriteLine("Enter '8' to load data from XML file.");
            Console.WriteLine("Enter '9' to load data from SQLite database.");
            Console.WriteLine("Enter '0' to exit the programme.");

            int choice;
            while (true)
            {
                Console.Write("Enter your choice: ");
                choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        CheckLeapYear();
                        break;
                    case 2:
                        CalculateDateInterval();
                        break;
                    case 3:
                        OutputDayOfWeek();
                        break;
                    case 4:
                        SaveDataInJson();
                        break;
                    case 5:
                        SaveDataInXml();
                        break;
                    case 6:
                        SaveDataInSqlite();
                        break;
                    case 7:
                        LoadDataFromJson();
                        break;
                    case 8:
                        LoadDataFromXml();
                        break;
                    case 9:
                        LoadDataFromSqlite();
                        break;
                    case 0:
                        Console.WriteLine("Exiting the programme...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void CheckLeapYear()
        {
            Console.Write("Enter a year: ");
            int year = Convert.ToInt32(Console.ReadLine());
            if (DateTime.IsLeapYear(year))
            {
                Console.WriteLine($"{year} is a leap year.");
            }
            else
            {
                Console.WriteLine($"{year} is not a leap year.");
            }
        }

        static void CalculateDateInterval()
        {
            Console.Write("Enter the first date (dd/mm/yyyy): ");
            DateTime date1 = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", null);
            Console.Write("Enter the second date (dd/mm/yyyy): ");
            DateTime date2 = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", null);
            TimeSpan interval = date2 - date1;
            Console.WriteLine($"The length of the interval between the two dates is {interval.Days} days.");
        }

        static void OutputDayOfWeek()
        {
            Console.Write("Enter a date (dd/mm/yyyy): ");
            DateTime date = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", null);
            Console.WriteLine($"The day of the week according to the entered date is {date.DayOfWeek}.");
        }

        static void SaveDataInJson()
        {
            Console.Write("Enter a year to save: ");
            int year = Convert.ToInt32(Console.ReadLine());
            string json = JsonConvert.SerializeObject(year);
            File.WriteAllText("data.json", json);
            Console.WriteLine("Data saved in JSON format.");
        }

        static void SaveDataInXml()
        {
            Console.Write("Enter a year to save: ");
            int year = Convert.ToInt32(Console.ReadLine());
            XmlSerializer serializer = new XmlSerializer(typeof(int));
            using (StreamWriter writer = new StreamWriter("data.xml"))
            {
                serializer.Serialize(writer, year);
            }
            Console.WriteLine("Data saved in XML format.");
        }

        static void SaveDataInSqlite()
        {
            Console.Write("Enter a year to save: ");
            int year = Convert.ToInt32(Console.ReadLine());
            SQLiteConnection connection = new SQLiteConnection("Data Source=data.db");
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "CREATE TABLE IF NOT EXISTS data (year INTEGER)";
            command.ExecuteNonQuery();
            command.CommandText = "INSERT INTO data (year) VALUES (@year)";
            command.Parameters.AddWithValue("@year", year);
            command.ExecuteNonQuery();
            connection.Close();
            Console.WriteLine("Data saved in SQLite database.");
        }

        static void LoadDataFromJson()
        {
            string json = File.ReadAllText("data.json");
            int year = JsonConvert.DeserializeObject<int>(json);
            Console.WriteLine($"Loaded year from JSON file: {year}");
        }

        static void LoadDataFromXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(int));
            using (StreamReader reader = new StreamReader("data.xml"))
            {
                int year = (int)serializer.Deserialize(reader);
                Console.WriteLine($"Loaded year from XML file: {year}");
            }
        }

        static void LoadDataFromSqlite()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=data.db");
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT year FROM data";
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                int year = reader.GetInt32(0);
                Console.WriteLine($"Loaded year from SQLite database: {year}");
            }
            else
            {
                Console.WriteLine("No data found in SQLite database.");
            }
            connection.Close();
        }
    }
}