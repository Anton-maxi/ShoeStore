using MySql.Data.MySqlClient;
using System;
using System.Data; //Обов'язково для роботи з DataTable
using System.Windows;

namespace ShoeStoreManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MySqlConnection myConnection;
            string myConnectionString;

            // Налаштування рядка підключення
            myConnectionString = "server=localhost;database=ShoeStore; uid=labuser;pwd=lab123;";

            try
            {
                myConnection = new MySqlConnection(myConnectionString);
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

                //Передаємо ці дані у ваш DataGrid
                ShoesDataGrid.ItemsSource = dataTable.DefaultView;

                myConnection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Помилка бази даних: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Загальна помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void AuthMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LoginForm authWindow = new LoginForm();

            //Якщо авторизація пройшла успішно (ввели правильний логін і пароль)
            if (authWindow.ShowDialog() == true)
            {
                //Створюємо нове вікно Shoes
                Shoes shoesWindow = new Shoes();

                //Відкриваємо його
                shoesWindow.Show();

                //Закриваємо поточне вікно (MainWindow), щоб воно не висіло на фоні
                this.Close();
            }
        }
    }
}