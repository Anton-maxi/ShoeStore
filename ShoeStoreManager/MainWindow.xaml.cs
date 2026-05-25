using MySql.Data.MySqlClient;
using System;
using System.Data; //Обов'язково для роботи з DataTable
using System.Windows;
using System.Windows.Controls;

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

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            // 1. Зчитуємо назву (якщо там висить підказка "Назва", передаємо порожній рядок)
            string searchName = "";
            if (SearchNameTextBox.Text != "Назва")
            {
                searchName = SearchNameTextBox.Text.Trim();
            }

            // 2. Безпечно зчитуємо мінімальну ціну
            int? minPrice = null;
            if (MinPriceTextBox.Text != "Ціна від" && int.TryParse(MinPriceTextBox.Text, out int parsedMin))
            {
                minPrice = parsedMin;
            }

            int? maxPrice = null;
            if (MaxPriceTextBox.Text != "Ціна до" && int.TryParse(MaxPriceTextBox.Text, out int parsedMax))
            {
                maxPrice = parsedMax;
            }

            // Отримуємо індекс категорії з випадаючого списку
            int selectedCategoryIndex = CategoryComboBox.SelectedIndex;

            try
            {
                // Викликаємо метод сервісу для відбору даних
                DataTable filteredShoes = _storeService.GetFilteredShoes(searchName, minPrice, maxPrice, selectedCategoryIndex);

                // Оновлюємо таблицю на екрані
                ShoesDataGrid.ItemsSource = filteredShoes.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час відбору даних: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Метод спрацьовує, коли ми клікаємо на текстове поле
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox? textBox = sender as TextBox;
            if (textBox != null)
            {
                // Якщо текст у полі співпадає з підказкою, очищаємо його
                if (textBox.Text == "Назва" || textBox.Text == "Ціна від" || textBox.Text == "Ціна до")
                {
                    textBox.Text = "";
                }
            }
        }

        // Метод спрацьовує, коли ми прибираємо курсор з текстового поля
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox? textBox = sender as TextBox;
            if (textBox != null)
            {
                // Якщо користувач нічого не ввів, повертаємо підказку залежно від імені поля
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    if (textBox.Name == "SearchNameTextBox")
                        textBox.Text = "Назва";
                    else if (textBox.Name == "MinPriceTextBox")
                        textBox.Text = "Ціна від";
                    else if (textBox.Name == "MaxPriceTextBox")
                        textBox.Text = "Ціна до";
                }
            }
        }
    }
}