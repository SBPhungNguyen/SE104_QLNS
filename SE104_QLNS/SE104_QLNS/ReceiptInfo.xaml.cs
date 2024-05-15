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
    /// Interaction logic for ReceiptInfo.xaml
    /// </summary>
    public partial class Uct_CustomerReceipt
    {
    public string ReceiptID { get; set; }
    public string CustomerID { get; set; }
    public string CustomerName { get; set; }
    public string CustomerNumber { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerAddress { get; set; }
    public string PaymentDate { get; set; }
    public string DebtBefore { get; set; }
    public string Payment { get; set; }
    public string DebtAfter { get; set; }
    public Uct_CustomerReceipt() { }
    public Uct_CustomerReceipt(string receiptID, string customerID, string customerName, string customerNumber, string customerEmail, string customerAddress, string paymentDate, string debtBefore, string payment, string debtAfter)
    {
            ReceiptID=receiptID;
        CustomerID = customerID;
        CustomerName = customerName;
        CustomerNumber = customerNumber;
        CustomerEmail = customerEmail;
        CustomerAddress = customerAddress;
        PaymentDate = paymentDate;
        DebtBefore = debtBefore;
        Payment = payment;
        DebtAfter = debtAfter;
    }
}
public partial class ReceiptInfo : Window
    {
        public ReceiptInfo()
        {
            InitializeComponent();
        }
        public ReceiptInfo(Uct_CustomerReceipt info)
        {
            InitializeComponent();
            tbl_Date.Text = info.PaymentDate;
            tbl_CustomerID.Text = info.CustomerID;
            tbl_CustomerPhone.Text = info.CustomerNumber;
            tbl_CustomerEmail.Text = info.CustomerEmail;

            tbl_CustomerAddress.Text = info.CustomerAddress;
            tbl_DebtBefore.Text = info.DebtBefore;
            tbl_Paid.Text = info.Payment;
            tbl_DebtAfter.Text = info.DebtAfter;
        }

        private void btn_ExitApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
