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
using static Grocery_App.Controls.Danh_sach_mat_hang;

namespace Grocery_App.Controls
{
    /// <summary>
    /// Interaction logic for History.xaml
    /// </summary>
    public partial class History : UserControl
    {
        public ObservableCollection<HistoryRecord> HistoryRecords { get; set; }
        public History()
        {
            InitializeComponent();
            LoadHistoryData();
            Lich_su_HD.ItemsSource = HistoryRecords;
        }
        private void LoadHistoryData()
        {
            string historyFilePath = "history.json";
            if (File.Exists(historyFilePath))
            {
                string json = File.ReadAllText(historyFilePath);
                HistoryRecords = JsonConvert.DeserializeObject<ObservableCollection<HistoryRecord>>(json);
            }
            else
            {
                HistoryRecords = new ObservableCollection<HistoryRecord>();
            }
        }
        private void Return_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.Iborder_Menu.Content = new Danh_sach_mat_hang();
            }
        }

    }
}
