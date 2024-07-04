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
using Newtonsoft.Json;
using System.IO;
using System.Collections.ObjectModel;

namespace Grocery_App.Controls
{
    /// <summary>
    /// Interaction logic for Popup_danh_sach.xaml
    /// </summary>
    public partial class Popup_danh_sach : Window
    {
        public ObservableCollection<Product> DanhSachMatHang { get; set; }
        public string SerializeMatHang()
        {
            return JsonConvert.SerializeObject(DanhSachMatHang);
        }
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
        public Popup_danh_sach()
        {
            InitializeComponent();
            DanhSachMatHang = LoadDataFromFile();
            ProductList.ItemsSource = DanhSachMatHang;
            this.DataContext = this;
        }
    }
}
