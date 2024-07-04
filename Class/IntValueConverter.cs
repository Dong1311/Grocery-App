using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace Grocery_App.Controls
{
    public class IntValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Chuyển đổi từ Model đến View
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Chuyển đổi từ View về Model
            if (int.TryParse(value.ToString(), out int result))
            {
                return result;
            }
            else
            {
                return DependencyProperty.UnsetValue; // Trả về giá trị không xác định nếu không hợp lệ
            }
        }
    }
}
