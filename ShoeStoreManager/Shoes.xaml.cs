using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace ShoeStoreManager
{
    public partial class Shoes : Window
    {
        private readonly string connectionString = "server=localhost;database=ShoeStore;uid=labuser;pwd=lab123;";

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
            try
            {
                using (MySqlConnection myConnection = new MySqlConnection(connectionString))
                {
                    myConnection.Open();
                    using (MySqlCommand myCommand = new MySqlCommand("SELECT * FROM shoe", myConnection))
                    {
                        MySqlDataAdapter myAdapter = new MySqlDataAdapter(myCommand);
                        DataTable dataTable = new DataTable();
                        myAdapter.Fill(dataTable);
                        ShoesDataGrid.ItemsSource = dataTable.DefaultView;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження даних: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool ExecuteDatabaseOperation(string query, params MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlConnection myConnection = new MySqlConnection(connectionString))
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
            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
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