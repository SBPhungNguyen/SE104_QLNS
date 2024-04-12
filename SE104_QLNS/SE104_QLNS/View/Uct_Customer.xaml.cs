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
    /// Interaction logic for Uct_Customer.xaml
    /// </summary>
    public partial class Uct_Customer : UserControl
    {
        public string CustomerName { get; set; }
        public string CustomerID { get; set; }
        public string CustomerPhonenumber { get; set; }
        public string CustomerSpending { get; set; }
        public string CustomerDebt { get; set; }
        public string Img_Type { get; set; }
        public Uct_Customer()
        {
            InitializeComponent();
            this.DataContext = this;

        }
    }
}
