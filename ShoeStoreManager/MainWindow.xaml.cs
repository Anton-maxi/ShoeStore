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
        private readonly ShoeStoreService _storeService = new ShoeStoreService();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Викликаємо інкапсульований метод і передаємо дані в таблицю
                DataTable shoesTable = _storeService.GetAllShoes();
                ShoesDataGrid.ItemsSource = shoesTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження даних: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
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