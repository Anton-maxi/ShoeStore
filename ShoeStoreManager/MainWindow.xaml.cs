using System;
using System.Collections.Generic;
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
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Цей метод можна використовувати для переходу без авторизації (як гість)
            Shoes shoes = new Shoes();
            shoes.Show();
            this.Close();
        }

        private void AuthMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LoginForm authWindow = new LoginForm();

            // Якщо авторизація пройшла успішно (ввели правильний логін і пароль)
            if (authWindow.ShowDialog() == true)
            {
                // 1. Створюємо нове вікно Shoes
                Shoes shoesWindow = new Shoes();

                // 2. Відкриваємо його
                shoesWindow.Show();

                // 3. Закриваємо поточне вікно (MainWindow), щоб воно не висіло на фоні
                this.Close();
            }
        }
    }
}