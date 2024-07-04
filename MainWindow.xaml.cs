using Grocery_App.Controls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Grocery_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TenNV.Text = UserSession.Instance.CurrentEmployeeName;
        }

        private void Danh_Sach_Hang_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.Iborder_Menu.Content = new Danh_sach_mat_hang();
            }
        }

        private void Them_Hoa_Don_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.Iborder_Menu.Content = new Them_hoa_don();
            }
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            if (Log_out_Grid.Visibility == Visibility.Visible)
            {
                Log_out_Grid.Visibility = Visibility.Collapsed;
            }
            else
            {
                Log_out_Grid.Visibility = Visibility.Visible;
            }

        }
        private void Log_out_Btn_CLick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }

        private void Thong_ke_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.Iborder_Menu.Content = new Thong_ke();
            }
        }
    }
}
