using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace ShoeStoreManager
{
    public class Shoe
    {
        private string _article = string.Empty;
        private string _name = string.Empty;
        private int _count;
        private decimal _price;

        public string Article
        {
            get { return _article; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Артикул не може бути порожнім.");
                _article = value;
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Назва не може бути порожньою.");
                _name = value;
            }
        }

        public int Count
        {
            get { return _count; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Кількість не може бути від'ємною.");
                _count = value;
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Ціна не може бути від'ємною.");
                _price = value;
            }
        }

        // Конструктор
        public Shoe(string article, string name, int count, decimal price)
        {
            Article = article;
            Name = name;
            Count = count;
            Price = price;
        }
    }

    public class ShoeStoreService
    {
        // Рядок підключення прихований (private) всередині сервісу
        private readonly string _connectionString = "server=localhost;database=ShoeStore; uid=labuser;pwd=lab123;";

        //Завантаження даних
        public DataTable GetAllShoes()
        {
            MySqlConnection myConnection = new MySqlConnection(_connectionString);
            myConnection.Open();

            //Створюємо команду
            MySqlCommand myCommand = new MySqlCommand();
            myCommand.Connection = myConnection;
            myCommand.CommandText = "SELECT * FROM shoe";

            //Cтворюємо адаптер, який сам виконає команду та зчитує дані
            MySqlDataAdapter myAdapter = new MySqlDataAdapter(myCommand);

            //Створюємо віртуальну таблицю в пам'яті
            DataTable dataTable = new DataTable();

            //Заповнюємо таблицю даними з БД
            myAdapter.Fill(dataTable);
            myConnection.Close();

            //Передаємо ці дані у ваш DataGrid
            return dataTable;


        }

        //Авторизація (Повертає true/false)
        public bool AuthenticateUser(string login, string password)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM users WHERE login = @user AND password = @pass";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@user", login);
                    cmd.Parameters.AddWithValue("@pass", password);

                    //Отримуємо кількість знайдених рядків (буде 1, якщо користувач є, або 0, якщо немає)
                    long userExists = (long)cmd.ExecuteScalar();
                    return userExists > 0;
                }
            }
        }
        public bool SaveShoe(Shoe shoe)
        {

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@article", shoe.Article),
                new MySqlParameter("@name", shoe.Name),
                new MySqlParameter("@count", shoe.Count),
                new MySqlParameter("@price", shoe.Price)
            };

            bool isUpdate = false;
            string myConnectionString = "server=localhost;database=ShoeStore; uid=labuser;pwd=lab123;";
            using (MySqlConnection myConnection = new MySqlConnection(myConnectionString))
            {
                myConnection.Open();
                using (MySqlCommand checkCmd = new MySqlCommand("SELECT COUNT(*) FROM shoe WHERE item_number = @article", myConnection))
                {
                    checkCmd.Parameters.AddWithValue("@article", shoe.Article);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    isUpdate = (count > 0);
                }
            }

            string sql;
            if (isUpdate)
            {
                sql = "UPDATE shoe SET name = @name, count = @count, price_one_pair = @price WHERE item_number = @article";
            }
            else
            {
                sql = "INSERT INTO shoe (item_number, name, count, price_one_pair) VALUES (@article, @name, @count, @price)";
            }

            bool isSuccess = ExecuteDatabaseOperation(sql, parameters);

            if (isSuccess)
            {
                string message;
                if (isUpdate)
                    message = "Дані моделі успішно оновлено!";
                else
                    message="Нову модель успішно додано!";
                MessageBox.Show(message, "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);

                if (!isUpdate)
                {
                    return false; 
                }
            }
            return isSuccess;
        }
        private bool ExecuteDatabaseOperation(string query, params MySqlParameter[] parameters)
        {
            //Налаштування рядка підключення
            string myConnectionString = "server=localhost;database=ShoeStore; uid=labuser;pwd=lab123;";
            using (MySqlConnection myConnection = new MySqlConnection(myConnectionString))
            {
                myConnection.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myConnection))
                {
                    if (parameters != null)
                    {
                        myCommand.Parameters.AddRange(parameters);
                    }

                    int affectedRows = myCommand.ExecuteNonQuery();
                    return affectedRows > 0;
                }
            }
        }

    
    }
}