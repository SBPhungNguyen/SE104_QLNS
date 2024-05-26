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

namespace SE104_QLNS.View
{
    /// <summary>
    /// Interaction logic for Uct_BookImport.xaml
    /// </summary>
    public partial class Uct_BookImport : UserControl
    {
        public MainWindow parent;
        public string BookID
        { get; set; }
        public string BookName
        { get; set; }
        public string BookURL
        { get; set; }
        public string BookImportPrice
        { get; set; }
        public string BookQuantity
        { get; set; }
        public Uct_BookImport()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public Uct_BookImport(MainWindow mainwindow)
        {
            InitializeComponent();
            this.DataContext = this;
            parent = mainwindow;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (parent.wpn_ImportPaper == null) return; ;
            foreach (Uct_BookImport child in parent.wpn_ImportPaper.Children.OfType<Uct_BookImport>())
            {
                if (this == child)
                {
                    parent.wpn_ImportPaper.Children.Remove(this);
                    break;
                }
            }
            UpdateMoney();
        }

        private void tbl_BookQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!int.TryParse(textBox.Text, out int parsedQuantity))
            {
                textBox.Text = "1";
            }
            BookQuantity = textBox.Text;
            UpdateMoney();
        }
        public void UpdateMoney()
        {
            int money = 0;
            foreach (Uct_BookImport child in parent.wpn_ImportPaper.Children.OfType<Uct_BookImport>())
            {
                money += Convert.ToInt32(child.BookImportPrice) * Convert.ToInt32(child.BookQuantity);
            }
            parent.tbl_SumImportMoney.Text = money.ToString();
        }
    }
}
