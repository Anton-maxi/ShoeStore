using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ShoeStoreManager
{
    /// <summary>
    /// Interaction logic for Shoes.xaml
    /// </summary>
    public partial class Shoes : Window
    {


        public Shoes()
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

                // Створюємо команду
                MySqlCommand myCommand = new MySqlCommand();
                myCommand.Connection = myConnection;
                myCommand.CommandText = "SELECT * FROM shoe";

                //ОНОВЛЕНИЙ ТА ПРАВИЛЬНИЙ ШЛЯХ ВИВЕДЕННЯ ДАНИХ

                // Створюємо адаптер, який сам виконає команду та зчитує дані
                MySqlDataAdapter myAdapter = new MySqlDataAdapter(myCommand);

                // Створюємо віртуальну таблицю в пам'яті
                DataTable dataTable = new DataTable();

                // Заповнюємо таблицю даними з БД
                myAdapter.Fill(dataTable);

                // Передаємо ці дані у ваш DataGrid (який в MainWindow.xaml має назву x:Name="ShoesDataGrid")
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

        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

    }
}
