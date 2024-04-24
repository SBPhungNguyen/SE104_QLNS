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
    /// Interaction logic for CustomerInfo.xaml
    /// </summary>
    public partial class CustomerInfo : Window
    {
        public bool IsClosing = false;
        public CustomerInfo()
        {
            InitializeComponent();
        }
        public CustomerInfo(string customerID, string customerName, string customerEmail,
            string customerPhonenumber, string customerBirthday, string customerGender,
            string customerAddress, string customerSpending, string customerDebt)
        {
            InitializeComponent();
            tbl_CustomerID.Text = customerID;
            tbl_CustomerMail.Text = customerEmail;
            tbl_CustomerBirthday.Text = customerBirthday;
            tbl_CustomerName.Text = customerName;
            tbl_CustomerPhone.Text = customerPhonenumber;
            tbl_CustomerGender.Text = customerGender;
            tbl_CustomerAddress.Text = customerAddress;
            tbl_CustomerSpent.Text = customerSpending;
            tbl_CustomerDebt.Text = customerDebt;
        }

        private void btn_ExitApp_Click(object sender, RoutedEventArgs e)
        {
            IsClosing = true;
            this.Close();
        }
        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            if (IsClosing)
            {
                return;
            }
            IsClosing = true;
            this.Close();
        }
    }
}
