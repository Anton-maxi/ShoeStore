using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            MySqlConnection myConnection;
            string myConnectionString;

            //Налаштування рядка підключення
            myConnectionString = "server=localhost;database=ShoeStore; uid=labuser;pwd=lab123;";

            try
            {
                myConnection = new MySqlConnection(myConnectionString);
                myConnection.Open();

                //Створюємо команду
                MySqlCommand myCommand = new MySqlCommand();
                myCommand.Connection = myConnection;
                myCommand.CommandText = "SELECT * FROM shoe";

                //Створюємо адаптер, який сам виконає команду та зчитує дані
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
        private bool ExecuteDatabaseOperation(string query, params MySqlParameter[] parameters)
        {
            //Налаштування рядка підключення
            string myConnectionString = "server=localhost;database=ShoeStore; uid=labuser;pwd=lab123;";
            try
            {
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
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка бази даних: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtArticle.Text) || string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtCount.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Будь ласка, заповніть всі поля перед збереженням!", "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@article", txtArticle.Text),
                new MySqlParameter("@name", txtName.Text),
                new MySqlParameter("@count", txtCount.Text),
                new MySqlParameter("@price", txtPrice.Text.Replace(',', '.'))
            };

            bool isUpdate = false;
            string myConnectionString = "server=localhost;database=ShoeStore; uid=labuser;pwd=lab123;";
            using (MySqlConnection myConnection = new MySqlConnection(myConnectionString))
            {
                myConnection.Open();
                using (MySqlCommand checkCmd = new MySqlCommand("SELECT COUNT(*) FROM shoe WHERE item_number = @article", myConnection))
                {
                    checkCmd.Parameters.AddWithValue("@article", txtArticle.Text);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    isUpdate = (count > 0);
                }
            }

            string query;
            if (isUpdate)
            {
                query = "UPDATE shoe SET name = @name, count = @count, price_one_pair = @price WHERE item_number = @article";
            }
            else
            {
                query = "INSERT INTO shoe (item_number, name, count, price_one_pair) VALUES (@article, @name, @count, @price)";
            }

            bool isSuccess = ExecuteDatabaseOperation(query, parameters);

            if (isSuccess)
            {
                string message = isUpdate ? "Дані моделі успішно оновлено!" : "Нову модель успішно додано!";
                MessageBox.Show(message, "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();

                if (!isUpdate)
                {
                    ClearInputFields();
                }
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
            txtArticle.Clear();
            txtName.Clear();
            txtCount.Clear();
            txtPrice.Clear();
            txtArticle.Focus();
        }

    }
}