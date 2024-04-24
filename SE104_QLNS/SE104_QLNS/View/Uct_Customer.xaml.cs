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
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhonenumber { get; set; }
        public string CustomerBirthday { get; set; }  
        public string CustomerGender { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerSpending { get; set; }
        public string CustomerDebt { get; set; }
        public string Img_Type { get; set; }
        public Uct_Customer()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public void LoadData(string customerID, string customerName, 
            string customerEmail, string customerPhonenumber, string customerBirthday, 
            string customerGender, string customerAddress, string customerSpending,
            string customerDebt, string img_Type)
        {
            CustomerID = customerID;
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            CustomerPhonenumber = customerPhonenumber;
            CustomerBirthday = customerBirthday;
            CustomerGender = customerGender;
            CustomerAddress = customerAddress;
            CustomerSpending = customerSpending;
            CustomerDebt = customerDebt;
            Img_Type = img_Type;
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CustomerInfo customer = new CustomerInfo(CustomerID, CustomerName, CustomerEmail, 
                CustomerPhonenumber, CustomerBirthday, 
               CustomerGender, CustomerAddress, CustomerSpending, CustomerDebt);
            customer.Visibility = Visibility.Visible;
            customer.Topmost = true;
        }
    }
}
