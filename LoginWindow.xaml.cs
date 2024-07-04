using Newtonsoft.Json;
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
using System.IO;
using Grocery_App.Controls;

namespace Grocery_App
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string json = File.ReadAllText("account.json");
            var loginData = JsonConvert.DeserializeObject<LoginData>(json);

            var username = UsernameTextBox.Text;
            var password = PasswordTextBox.Password;

            var user = loginData.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                // Đăng nhập thành công, mở MainWindow
                UserSession.Instance.SetCurrentUser(user.Username, user.EmployeeName);
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close(); // Đóng cửa sổ đăng nhập
            }
            else
            {
                // Đăng nhập thất bại, hiển thị thông báo lỗi
                Check_info_Txbl.Visibility = Visibility.Visible;
            }
        }

        public class LoginData
        {
            public List<User> Users { get; set; }
        }
        public class User
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string EmployeeName { get; set; }
        }
        
    }
}
