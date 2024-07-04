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
using System.Windows.Threading;
using System.IO;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;

namespace Grocery_App.Controls
{
    /// <summary>
    /// Interaction logic for Them_hoa_don.xaml
    /// </summary>
    public partial class Them_hoa_don : UserControl
    {
        private DispatcherTimer _timer;
        public ObservableCollection<Product> DanhSachMatHang { get; set; }
        public ObservableCollection<ProductDisplay> ProductDisplays { get; set; }
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

        public Them_hoa_don()
        {
            InitializeComponent();
            this.DataContext = this;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1); // Cập nhật mỗi giây
            _timer.Tick += Timer_Tick;
            _timer.Start();
            TenNV.Text ="Nhân viên: " + UserSession.Instance.CurrentEmployeeName;
            DanhSachMatHang = LoadDataFromFile();
            ProductList.ItemsSource = DanhSachMatHang;
            ProductDisplays = new ObservableCollection<ProductDisplay>();
            ItemsListView.ItemsSource = ProductDisplays;
            PaymentPopup.DataContext = this;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTimeTextBlock.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }
        private void Search_Txb_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filteredList = DanhSachMatHang.Where(p => p.Ten.ToLower().Contains(Search_Txb.Text.ToLower())).ToList();
            ProductList.ItemsSource = new ObservableCollection<Product>(filteredList);
        }

        private void Open_Popup_danh_sach_Click(object sender, RoutedEventArgs e)
        {
            PopupResult.IsOpen = true;
        }
        private void ThemSanPham_Click(object sender, RoutedEventArgs e)
        {
            if (ProductList.SelectedItem is Product selectedProduct)
            {
                // Lấy thông tin số lượng sản phẩm từ DanhSachMatHang
                var productInStock = DanhSachMatHang.FirstOrDefault(p => p.MaHang == selectedProduct.MaHang);

                if (productInStock != null)
                {
                    // Tìm sản phẩm đã tồn tại trong danh sách ProductDisplays
                    var existingProductDisplay = ProductDisplays.FirstOrDefault(p => p.MaMatHang == selectedProduct.MaHang);

                    if (existingProductDisplay != null)
                    {
                        // Kiểm tra số lượng trước khi tăng
                        if (existingProductDisplay.SoLuong < productInStock.SoLuong)
                        {
                            existingProductDisplay.SoLuong++;
                        }
                        else
                        {
                            MessageBox.Show("Số lượng sản phẩm vượt quá số lượng có trong kho.", "Thông báo");
                        }
                    }
                    else
                    {
                        // Thêm sản phẩm mới nếu số lượng nhỏ hơn số lượng trong kho
                        if (1 <= productInStock.SoLuong) 
                        {
                            var newProductDisplay = new ProductDisplay(productInStock, DeleteProduct)
                            {
                                MaMatHang = selectedProduct.MaHang,
                                TenMatHang = selectedProduct.Ten,
                                Price = selectedProduct.Gia,
                                SoLuong = 1
                            };
                            ProductDisplays.Add(newProductDisplay);
                        }
                        else
                        {
                            MessageBox.Show("Không đủ số lượng sản phẩm trong kho.", "Thông báo");
                        }
                    }
                }
                PopupResult.IsOpen = false;
            }
            CalculateTotalAmount();
        }

        public void DeleteProduct(object product)
        {
            if (product is ProductDisplay productDisplay)
            {
                ProductDisplays.Remove(productDisplay);
            }
        }
        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ProductDisplay productDisplay)
            {
                ProductDisplays.Remove(productDisplay);
                CalculateTotalAmount();
            }
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.DataContext is ProductDisplay productDisplay)
            {
                if (!int.TryParse(textBox.Text, out int soLuong))
                {
                    MessageBox.Show("Vui lòng nhập một số nguyên hợp lệ.", "Thông báo");
                    textBox.Text = productDisplay.SoLuong.ToString(); // Đặt lại với giá trị số lượng hiện tại
                }
            }
            CalculateTotalAmount();
        }
        private void CalculateTotalAmount()
        {
            double tongTienHang = ProductDisplays.Sum(item => item.Tong);
            Tong_tien_hang_Txbl.Text = tongTienHang.ToString();
            UpdateAmountsToPay();
        }
        private void UpdateAmountsToPay()
        {
            double.TryParse(Tong_tien_hang_Txbl.Text, out double tongTienHang);
            double.TryParse(Giam_gia_Txb.Text, out double giamGia);
            double khachCanTra = tongTienHang - giamGia;
            Khach_can_tra_Txbl.Text = khachCanTra.ToString();
            CalculateChange();
        }
        private void CalculateChange()
        {
            double.TryParse(Khach_thanh_toan_Txb.Text, out double khachThanhToan);
            double.TryParse(Khach_can_tra_Txbl.Text, out double khachCanTra);
            double traLai = khachThanhToan - khachCanTra;
            Tra_lai_Txbl.Text = traLai > 0 ? traLai.ToString() : "0";
        }
        private void GiamGia_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAmountsToPay();
        }

        private void KhachThanhToan_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateChange();
        }
        private void ThanhToan_Click(object sender, RoutedEventArgs e)
        {
            PaymentPopup.IsOpen = true;
            PopupListView.ItemsSource = ProductDisplays;
        }
        private void Luu_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "hoadon.json"; 
            List<HoaDonData> hoaDonList = new List<HoaDonData>();

            if (File.Exists(filePath))
            {
                // Đọc dữ liệu từ file
                string existingData = File.ReadAllText(filePath);
                hoaDonList = JsonConvert.DeserializeObject<List<HoaDonData>>(existingData) ?? new List<HoaDonData>();
            }

            var hoaDonData = new HoaDonData
            {
                TenKhachHang = Ten_khach_Txb.Text,
                TongTienHang = double.Parse(Tong_tien_hang_Txbl.Text),
                GiamGia = double.Parse(Giam_gia_Txb.Text),
                KhachCanTra = double.Parse(Khach_can_tra_Txbl.Text),
                KhachThanhToan = double.Parse(Khach_thanh_toan_Txb.Text),
                TraLai = double.Parse(Tra_lai_Txbl.Text),
                NgayGio = DateTime.Now
            };

            hoaDonList.Add(hoaDonData);
            string json = JsonConvert.SerializeObject(hoaDonList, Formatting.Indented);
            File.WriteAllText(filePath, json);
            PaymentPopup.IsOpen = false;
            MessageBox.Show("Lưu hóa đơn thành công");
        }

        private void XuatHoaDon_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "hoadon.json";
            List<HoaDonData> hoaDonList = new List<HoaDonData>();

            if (File.Exists(filePath))
            {
                // Đọc dữ liệu từ file
                string existingData = File.ReadAllText(filePath);
                hoaDonList = JsonConvert.DeserializeObject<List<HoaDonData>>(existingData) ?? new List<HoaDonData>();
            }

            var hoaDonData = new HoaDonData
            {
                TenKhachHang = Ten_khach_Txb.Text,
                TongTienHang = double.Parse(Tong_tien_hang_Txbl.Text),
                GiamGia = double.Parse(Giam_gia_Txb.Text),
                KhachCanTra = double.Parse(Khach_can_tra_Txbl.Text),
                KhachThanhToan = double.Parse(Khach_thanh_toan_Txb.Text),
                TraLai = double.Parse(Tra_lai_Txbl.Text),
                NgayGio = DateTime.Now
            };

            hoaDonList.Add(hoaDonData);
            string json = JsonConvert.SerializeObject(hoaDonList, Formatting.Indented);
            File.WriteAllText(filePath, json);
            PaymentPopup.IsOpen = false;
            MessageBox.Show("Lưu hóa đơn thành công");

            //Chuyển sang file PDF
            string fontPath = @"D:\C#\Grocery_App-Test 27_1_co_pdf\Montserrat-VariableFont_wght.ttf";
            BaseFont customBaseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "PDF file (*.pdf)|*.pdf";
            if (saveFileDialog.ShowDialog() != true)
            {
                return;
            }

            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(saveFileDialog.FileName, FileMode.Create));
            pdfDoc.Open();
            pdfDoc.Add(new iTextSharp.text.Paragraph("Hóa đơn", new Font(customBaseFont, 20, Font.BOLD)));
            pdfDoc.Add(new iTextSharp.text.Paragraph("Hóa đơn", FontFactory.GetFont("TimesNewRoman", 20, Font.BOLD)));

            pdfDoc.Add(new iTextSharp.text.Paragraph($"Tên khách hàng: {Ten_khach_Txb.Text}", new Font(customBaseFont, 16)));

            // Tạo bảng cho danh sách sản phẩm
            PdfPTable table = new PdfPTable(5); // Số cột bằng với ListView
            table.WidthPercentage = 100;

            // Thêm các tiêu đề cột
            table.AddCell("Mã MH");
            table.AddCell("Tên mặt hàng");
            table.AddCell("SL");
            table.AddCell("Giá");
            table.AddCell("Tổng");

            // Thêm dữ liệu sản phẩm từ ListView
            foreach (var item in PopupListView.Items)
            {
                var product = item as ProductDisplay;
                if (product != null)
                {
                    table.AddCell(product.MaMatHang);
                    table.AddCell(product.TenMatHang);
                    table.AddCell(product.SoLuong.ToString());
                    table.AddCell(product.Price.ToString("C"));
                    table.AddCell(product.Tong.ToString("C"));
                }
            }

            pdfDoc.Add(table);

            // Thêm các thông tin khác như giảm giá, tổng số tiền, số tiền khách đưa, và số tiền trả lại
            pdfDoc.Add(new iTextSharp.text.Paragraph($"Giảm giá: {Giam_gia_Txb.Text}", FontFactory.GetFont("TimesNewRoman", 16, Font.NORMAL)));
            pdfDoc.Add(new iTextSharp.text.Paragraph($"Tổng số tiền: {Khach_can_tra_Txbl.Text}", FontFactory.GetFont("TimesNewRoman", 16, Font.NORMAL)));
            pdfDoc.Add(new iTextSharp.text.Paragraph($"Số tiền khách đưa: {Khach_thanh_toan_Txb.Text}", FontFactory.GetFont("TimesNewRoman", 16, Font.NORMAL)));
            pdfDoc.Add(new iTextSharp.text.Paragraph($"Số tiền trả lại: {Tra_lai_Txbl.Text}", FontFactory.GetFont("TimesNewRoman", 16, Font.NORMAL)));

            pdfDoc.Close();

        }

    }
}
