using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Grocery_App.Controls
{
    public class ProductDisplay : INotifyPropertyChanged
    {
        private int soLuong;
        //private double tong;
        private readonly int soLuongTrongKho;
        public string MaMatHang { get; set; }
        public string TenMatHang { get; set; }
        public double Price { get; set; }

        public int SoLuong
        {
            get => soLuong;
            set
            {
                // Kiểm tra xem giá trị có phải là số nguyên hợp lệ
                if (int.TryParse(value.ToString(), out int newValue))
                {
                    if (newValue > 0 && newValue <= soLuongTrongKho) // Kiểm tra giới hạn số lượng
                    {
                        soLuong = newValue;
                        OnPropertyChanged(nameof(SoLuong));
                        OnPropertyChanged(nameof(Tong));
                        
                    }
                    else
                    {
                        MessageBox.Show("Số lượng nhập vào không hợp lệ hoặc vượt quá số lượng trong kho.", "Thông báo");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập một số nguyên hợp lệ.", "Thông báo");
                }
            }
        }


        public double Tong
        {
            get => Price * soLuong;
        }
        public ProductDisplay(Product productFromStock, Action<object> deleteAction)
        {
            MaMatHang = productFromStock.MaHang;
            TenMatHang = productFromStock.Ten;
            Price = productFromStock.Gia;
            soLuongTrongKho = productFromStock.SoLuong;
            DeleteCommand = new RelayCommand(deleteAction);
        }
        public ICommand DeleteCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ProductDisplay(Action<object> deleteAction)
        {
            DeleteCommand = new RelayCommand(deleteAction);
        }

    }
}
