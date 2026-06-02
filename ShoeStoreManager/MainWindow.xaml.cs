using MySql.Data.MySqlClient;
using System;
using System.Data;
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

            //Якщо авторизація пройшла успішно
            if (authWindow.ShowDialog() == true)
            {
                Shoes shoesWindow = new Shoes();
                shoesWindow.Show();

                this.Close();
            }
        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            //  Зчитуємо назву
            string searchName = "";
            if (SearchNameTextBox.Text != "Назва")
            {
                searchName = SearchNameTextBox.Text.Trim();
            }

            //  Безпечно зчитуємо мінімальну ціну
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

            int selectedCategoryIndex = CategoryComboBox.SelectedIndex;

            try
            {
                DataTable filteredShoes = _storeService.GetFilteredShoes(searchName, minPrice, maxPrice, selectedCategoryIndex);

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

        private void FocusCategory_Click(object sender, RoutedEventArgs e)
        {
            // Переводимо фокус на випадаючий список
            CategoryComboBox.Focus();

            // Автоматично розгортаємо його
            CategoryComboBox.IsDropDownOpen = true;
        }

        private void FocusName_Click(object sender, RoutedEventArgs e)
        {
            // Переводимо фокус на текстове поле пошуку за назвою
            SearchNameTextBox.Focus();
        }

        private void SaveSelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ShoesDataGrid.ItemsSource is DataView dataView)
            {
                DataTable filteredTable = dataView.ToTable();

                if (filteredTable.Rows.Count == 0)
                {
                    MessageBox.Show("Немає даних для збереження! Спочатку виберіть товари.", "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Зчитуємо назву
                string searchName = SearchNameTextBox.Text;

                // Безпечно зчитуємо мінімальну ціну
                int? minPrice = null;
                if (MinPriceTextBox.Text != "Ціна від" && int.TryParse(MinPriceTextBox.Text, out int parsedMin))
                {
                    minPrice = parsedMin;
                }

                // Безпечно зчитуємо максимальну ціну
                int? maxPrice = null;
                if (MaxPriceTextBox.Text != "Ціна до" && int.TryParse(MaxPriceTextBox.Text, out int parsedMax))
                {
                    maxPrice = parsedMax;
                }

                // Зчитуємо категорію
                string selectedCategory = CategoryComboBox.Text;

                // Викликаємо експорт з усіма параметрами
                WordExportService exportService = new WordExportService();
                exportService.WriteData(filteredTable, searchName, minPrice, maxPrice, selectedCategory);
            }
        }
    }
}