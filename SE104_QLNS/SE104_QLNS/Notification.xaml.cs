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

namespace SE104_QLNS
{
    /// <summary>
    /// Interaction logic for Notification.xaml
    /// </summary>
    public partial class Notification : Window
    {
        public double ActualInfoHeight { get; set; }

        public Notification()
        {
            InitializeComponent();
        }

        public Notification(string title, string info)
        {
            InitializeComponent();
            this.Visibility = Visibility.Visible;
            this.Topmost = true;
            NotificationTitle.Text = title;
            NotificationInfo.Text = info;
            Size textSize = NotificationInfo.DesiredSize;
            ActualInfoHeight = textSize.Height + 100;  // Add some padding

            // Update window height based on ActualInfoHeight
            this.Height = ActualInfoHeight + 200;  // Add height of top row and some padding

        }
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_ExitApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
