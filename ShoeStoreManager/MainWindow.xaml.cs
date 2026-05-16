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
            InitializeComponent();
        }


namespace ShoeStoreManager
{
    /// <summary>
    /// Interaction logic for LoginForm.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
=========
                myCommand.CommandText = "SELECT * FROM shoe";
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
        private void AuthMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LoginForm authWindow = new LoginForm();
                myCommand.Connection = myConnection;
            // Якщо авторизація пройшла успішно (ввели правильний логін і пароль)
            if (authWindow.ShowDialog() == true)
            {
                // 1. Створюємо нове вікно Shoes
                Shoes shoesWindow = new Shoes();

                // 2. Відкриваємо його
                shoesWindow.Show();
                MySqlDataAdapter myAdapter = new MySqlDataAdapter(myCommand);
                // 3. Закриваємо поточне вікно (MainWindow), щоб воно не висіло на фоні
                this.Close();
            }
        }
    }
}                // Передаємо ці дані у ваш DataGrid (який в MainWindow.xaml має назву x:Name="ShoesDataGrid")
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
>>>>>>>>> Temporary merge branch 2
        {

        }
    }
<<<<<<<<< Temporary merge branch 1
}
=========
}
>>>>>>>>> Temporary merge branch 2
