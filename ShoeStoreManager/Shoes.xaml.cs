using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ShoeStoreManager
{
    public partial class Shoes : Window
    {
        public Shoes()
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


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (string.IsNullOrWhiteSpace(txtArticle.Text) || string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtCount.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
                {
                    MessageBox.Show("Будь ласка, заповніть всі поля перед збереженням!", "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                //Безпечна конвертація кількості
                if (!int.TryParse(txtCount.Text.Trim(), out int count))
                {
                    MessageBox.Show("Поле 'Кількість' повинно містити лише цілі числа!", "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtCount.Focus();
                    return;
                }

                //Безпечна конвертація ціни (робимо так, щоб приймало і крапку, і кому)
                string priceInput = txtPrice.Text.Trim().Replace(',', '.');
                if (!decimal.TryParse(priceInput, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price))
                {
                    MessageBox.Show("Поле 'Вартість' введено некоректно! Використовуйте формат на зразок: 150.50", "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtPrice.Focus();
                    return;
                }

                Shoe currentShoe = new Shoe(
                    txtArticle.Text.Trim(),
                    txtName.Text.Trim(),
                    count,
                    price
                );
                bool flag_for_ClearInputFields =_storeService.SaveShoe(currentShoe);
                ShoesDataGrid.ItemsSource = _storeService.GetAllShoes().DefaultView;
                if (!flag_for_ClearInputFields)
                {
                    ClearInputFields();
                }

            }
            catch (ArgumentException ex)
            {
                // Сюди прилетить помилка, якщо користувач ввів некоректні дані (наприклад, ціну -50)
                MessageBox.Show(ex.Message, "Увага (Валідація)", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                // Сюди прилетять помилки конвертації або проблеми з самою БД (try-catch, як ви і хотіли)
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка виконання", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ClearInputFields();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            LoadSelectedDataToEditor();
        }

        private void AddMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ClearInputFields();
        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LoadSelectedDataToEditor();
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
 
        }

        private void LoadSelectedDataToEditor()
        {
            if (ShoesDataGrid.SelectedItem is DataRowView selectedRow)
            {
                txtArticle.Text = selectedRow["item_number"].ToString();
                txtArticle.IsEnabled = false;
                txtName.Text = selectedRow["name"].ToString();
                txtCount.Text = selectedRow["count"].ToString();
                txtPrice.Text = selectedRow["price_one_pair"].ToString();
            }
            else
            {
                MessageBox.Show("Будь ласка, спочатку виберіть модель зі списку для редагування.", "Увага", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ClearInputFields()
        {
            txtArticle.IsEnabled = true;
            txtArticle.Clear();
            txtName.Clear();
            txtCount.Clear();
            txtPrice.Clear();
            txtArticle.Focus();
        }

    }
}