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
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.IO;
namespace Grocery_App.Controls
{
    /// <summary>
    /// Interaction logic for Danh_sach_mat_hang.xaml
    /// </summary>
    public partial class Danh_sach_mat_hang : UserControl
    {
        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                if (_selectedProduct != value)
                {
                    _selectedProduct = value;
                    // Thực hiện thêm bất kỳ logic nào khi item được chọn thay đổi
                }
            }
        }
        public class HistoryRecord
        {
            public string EmployeeName { get; set; }
            public DateTime ActionTime { get; set; }
            public string Action { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
        }

        public ObservableCollection<Product> DanhSachMatHang { get; set; }
        public ObservableCollection<Product> LoadDataFromFile()
        {
            string filePath = "du_lieu_mat_hang.json";
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<ObservableCollection<Product>>(json);
            }
            return new ObservableCollection<Product>();
        }
        public string SerializeMatHang()
        {
            return JsonConvert.SerializeObject(DanhSachMatHang);
        }
        public void SaveDataToFile(string json)
        {
            string filePath = "du_lieu_mat_hang.json";
            File.WriteAllText(filePath, json);
        }
 
        public Danh_sach_mat_hang()
        {
            InitializeComponent();
            DanhSachMatHang = LoadDataFromFile();
            ProductList.ItemsSource = DanhSachMatHang;
            this.DataContext = this;
        }

        private void OpenPopup_Window_Click(object sender, RoutedEventArgs e)
        {
            var popup = new PopupWindow();
            popup.OnProductAdded += product =>
            {
                DanhSachMatHang.Add(product);
                SaveDataToFile(SerializeMatHang());// Lưu dữ liệu sau khi thêm mặt hàng
                RecordAction("Thêm", product);
            };
            popup.ShowDialog();
            
        }
        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedProduct == null)
            {
                MessageBox.Show("Vui lòng chọn một mặt hàng để xóa.");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa mặt hàng này không?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                RecordAction("Xóa", SelectedProduct);
                DanhSachMatHang.Remove(SelectedProduct);
                SaveDataToFile(SerializeMatHang()); // Lưu lại dữ liệu vào file JSON
                SelectedProduct = null;
                
            }
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.Iborder_Menu.Content = new History();
            }
        }
        public void RecordAction(string action, Product product)
        {
            var historyRecord = new HistoryRecord
            {
                EmployeeName = UserSession.Instance.CurrentEmployeeName, // Giả sử bạn lưu tên nhân viên đã đăng nhập
                ActionTime = DateTime.Now,
                Action = action,
                ProductName = product.Ten,
                Quantity = product.SoLuong
            };

            string historyFilePath = "history.json";
            List<HistoryRecord> history;
            if (File.Exists(historyFilePath))
            {
                string json = File.ReadAllText(historyFilePath);
                history = JsonConvert.DeserializeObject<List<HistoryRecord>>(json);
            }
            else
            {
                history = new List<HistoryRecord>();
            }

            history.Add(historyRecord);
            string newJson = JsonConvert.SerializeObject(history);
            File.WriteAllText(historyFilePath, newJson);
        }
    }
}
