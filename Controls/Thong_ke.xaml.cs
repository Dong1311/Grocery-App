using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.IO;

namespace Grocery_App.Controls
{
    /// <summary>
    /// Interaction logic for Thong_ke.xaml
    /// </summary>
    public partial class Thong_ke : UserControl
    {
        public ObservableCollection<HoaDonData> HoaDonList { get; set; }
        public Thong_ke()
        {
            InitializeComponent();
            LoadData();
            Thong_ke_Grid.ItemsSource = HoaDonList;
            UpdateDoanhThu();
        }
        private void LoadData()
        {
            string filePath = "hoadon.json";
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                HoaDonList = JsonConvert.DeserializeObject<ObservableCollection<HoaDonData>>(json);
            }
            else
            {
                HoaDonList = new ObservableCollection<HoaDonData>();
            }
        }
        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;

            if (startDate == null || endDate == null)
            {
                MessageBox.Show("Vui lòng chọn cả ngày bắt đầu và ngày kết thúc.");
                return;
            }

            if (startDate > endDate)
            {
                MessageBox.Show("Ngày bắt đầu phải trước ngày kết thúc.");
                return;
            }

            var filteredData = HoaDonList.Where(hoaDon => hoaDon.NgayGio.Date >= startDate.Value.Date && hoaDon.NgayGio.Date <= endDate.Value.Date).ToList();
            Thong_ke_Grid.ItemsSource = filteredData;
            UpdateDoanhThu();
        }
        private void UpdateDoanhThu()
        {
            double doanhThu = 0;

            if (Thong_ke_Grid.ItemsSource is IEnumerable<HoaDonData> hoaDons)
            {
                doanhThu = hoaDons.Sum(hd => hd.KhachCanTra);
            }

            DoanhThuTextBlock.Text = $"Doanh thu: {doanhThu}";
        }

        private void ShowAll_Click(object sender, RoutedEventArgs e)
        {
            LoadData(); 
            Thong_ke_Grid.ItemsSource = HoaDonList; 

            UpdateDoanhThu();
        }

    }
}
