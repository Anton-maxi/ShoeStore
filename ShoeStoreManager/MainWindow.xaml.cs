using MySql.Data.MySqlClient;
using System;
using System.Data; // Обов'язково для роботи з DataTable
using System.Windows;

namespace ShoeStoreManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); // Залишаємо лише це, ніяких ручних створення DataGrid!
        }

        // Цей метод автоматично спрацює при відкритті вікна
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

                // Створюємо команду
                MySqlCommand myCommand = new MySqlCommand();
                myCommand.Connection = myConnection;
                myCommand.CommandText = "SELECT * FROM shoe"; // Переконайтеся, що в БД назва з маленької або великої літери (shoe/Shoe)

                // --- ОНОВЛЕНИЙ ТА ПРАВИЛЬНИЙ ШЛЯХ ВИВЕДЕННЯ ДАНИХ ---

                // Створюємо адаптер, який сам виконає команду та зчитує дані
                MySqlDataAdapter myAdapter = new MySqlDataAdapter(myCommand);

                // Створюємо віртуальну таблицю в пам'яті
                DataTable dataTable = new DataTable();

                // Заповнюємо таблицю даними з БД
                myAdapter.Fill(dataTable);

                // Передаємо ці дані у ваш DataGrid (який в MainWindow.xaml має назву x:Name="ShoesDataGrid")
                ShoesDataGrid.ItemsSource = dataTable.DefaultView;

                // --------------------------------------------------

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
    }
}