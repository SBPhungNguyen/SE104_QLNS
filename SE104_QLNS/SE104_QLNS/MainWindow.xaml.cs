using SE104_QLNS.View;
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

namespace SE104_QLNS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Day la code mau (vi du ve load thong tin sach)
            Uct_Books book = new Uct_Books();
            book.LoadData("TT012321", "Alice in da WonderLand", "/Images/Chill Wallpaper On.png", "76", "10000", "110000", "/Images/Img_Information.png");
            wpn_Books.Children.Add(book);

        }

        //Khi nhan nut ImportBooks
        private void btn_ImportBooks_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
