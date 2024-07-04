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
using System.Windows.Shapes;

namespace Grocery_App.Controls
{
    /// <summary>
    /// Interaction logic for PopupWindow.xaml
    /// </summary>
    public partial class PopupWindow : Window
    {
        public event Action<Product> OnProductAdded;
        public PopupWindow()
        {
            InitializeComponent();
        }

        private void ThemButton_Click(object sender, RoutedEventArgs e)
        {
            // Xử lý trường hợp dữ liệu không hợp lệ hoặc trống
            if (!HanSuDung.SelectedDate.HasValue)
            {
                MessageBox.Show("Ngày hết hạn không hợp lệ.");
                return;
            }
            var hsdParsed = HanSuDung.SelectedDate.Value.Date;
            if (hsdParsed <= DateTime.Now.Date)
            {
                MessageBox.Show("Ngày hết hạn phải là một ngày sau ngày hiện tại.");
                return;
            }
            if (!double.TryParse(Gia.Text, out double giaParsed))
            {
                MessageBox.Show("Giá phải là một số.");
                return;
            }
            if (!int.TryParse(SoLuong.Text, out int soLuongParsed))
            {
                MessageBox.Show("Số lượng phải là một số nguyên.");
                return;
            }
            //var hsdParsed = HanSuDung.SelectedDate.Value.Date;
            string formattedDate = hsdParsed.ToString("dd/MM/yyyy");
            var matHangMoi = new Product
            {
                MaHang = NewNameKT.Text,
                Ten = TenMatHang.Text,
                Gia = giaParsed,
                HSD = hsdParsed,
                SoLuong = soLuongParsed
            };

            OnProductAdded?.Invoke(matHangMoi);
            this.Close();
        }
    }
}
