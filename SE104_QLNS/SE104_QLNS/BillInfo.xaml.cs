using SE104_QLNS;
using SE104_QLNS.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Interaction logic for BillInfo.xaml
    /// </summary>
    public partial class ExportedBookInfo
    {
        public ExportedBookInfo()
        {
        }
        public int BookNum { get; set; }
        public string ExportedBookReceiptID { get; set; }
        public string ExportBookID { get; set; }
        public string ExportBookURL { get; set; }
        public string ExportedBookName { get; set; }
        public int ExportedBookAmount { get; set; }
        public string ExportedBookPrice {  get; set; }
        public string ExportedBookTotal { get; set; }
        public ExportedBookInfo(int bookNum, string exportedBookReceiptID, string exportBookID, string exportBookURL, string exportedBookName, int exportedBookAmount, string exportedBookPrice, string exportedBookTotal)
        {
            BookNum = bookNum;
            ExportedBookReceiptID = exportedBookReceiptID;
            ExportBookID = exportBookID;
            ExportBookURL = exportBookURL;
            ExportedBookName = exportedBookName;
            ExportedBookAmount = exportedBookAmount;
            ExportedBookPrice = exportedBookPrice;
            ExportedBookTotal = exportedBookTotal;
        }
    }
    public partial class BillInfo : Window
    {
        MainWindow parent;
        int state = 0;
        public ObservableCollection<ExportedBookInfo> bills { get; set; } = new ObservableCollection<ExportedBookInfo>();
        Connection connect = new Connection();
        public int BillNum { get; set; }
        public string BillID { get; set; }
        public string CreationDate { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public string BillTotal { get; set; }
        public string BillPaid { get; set; }
        public string BillRemaining { get; set; }
        public BillInfo()
        {
            InitializeComponent();
        }
        public BillInfo(MainWindow mainwindow, int state)
        {
            InitializeComponent();
            parent = mainwindow;
            this.state = state;

            this.tbl_Date.Text = CreationDate;
            this.tbl_CustomerID.Text = CustomerID;
            this.tbl_CustomerName.Text = CustomerName;
            this.tbl_CustomerPhone.Text = CustomerPhoneNumber;
            this.tbl_CustomerEmail.Text = CustomerEmail;
            this.tbl_CustomerAddress.Text = CustomerAddress;
            this.tbl_DebtBefore.Text = BillTotal;
            this.tbl_Paid.Text = BillPaid;
            this.tbl_DebtAfter.Text = BillRemaining;

            if(state==0)
            {
                BillIDName.Text = "Hóa đơn ";
                btn_Delete.Visibility = Visibility.Hidden;
                btn_Cancel.Visibility = Visibility.Hidden;
            }
            else
            {
                BillIDName.Text = "Bạn có chắc muốn xóa Hóa đơn ";
                btn_Ok.Visibility= Visibility.Hidden;
            }
        }
        public void LoadData(int BillNum, string billID, string creationDate, string customerID, string customerName, string customerPhoneNumber, 
            string customerEmail, string customerAddress, 
            string billTotal, string billPaid, string billRemaining)
        {
            this.BillNum = BillNum;
            BillID=billID;
            BillIDName.Text+=  billID;
            CreationDate = creationDate;
            CustomerID = customerID;
            CustomerName = customerName;
            CustomerPhoneNumber = customerPhoneNumber;
            CustomerEmail = customerEmail;
            CustomerAddress = customerAddress;
            BillTotal = billTotal;
            BillPaid = billPaid;
            BillRemaining = billRemaining;

            this.tbl_Date.Text = CreationDate;
            this.tbl_CustomerID.Text = CustomerID;
            this.tbl_CustomerName.Text = CustomerName;
            this.tbl_CustomerPhone.Text = CustomerPhoneNumber;
            this.tbl_CustomerEmail.Text = CustomerEmail;
            this.tbl_CustomerAddress.Text = CustomerAddress;
            this.tbl_DebtBefore.Text = BillTotal;
            this.tbl_Paid.Text = BillPaid;
            this.tbl_DebtAfter.Text = BillRemaining;

            LoadBillInfo(this);

            dtg_BillDetail.ItemsSource = bills;
            dtg_BillDetail.Items.Refresh();
        }

        public void LoadBillInfo(BillInfo billinfo)
        {
            //connect to database
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sqlQuery = "SELECT * FROM CT_HOADON JOIN SACH ON CT_HOADON.MASACH = SACH.MASACH " +
                        " JOIN DAUSACH ON DAUSACH.MADAUSACH = SACH.MADAUSACH WHERE MaHD=@MaHD";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaHD", billinfo.BillID);

                    int order = 1;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int bookNum = order;
                            string exportedBookReceiptID = reader["MaHD"].ToString();
                            string exportBookID = reader["MaSach"].ToString();
                            string exportBookURL = reader["HinhAnhSach"].ToString();
                            string exportedBookName = reader["TenDauSach"].ToString();
                            int exportedBookAmount = Convert.ToInt32(reader["SoLuong"].ToString());
                            string exportedBookPrice = reader["DonGiaBan"].ToString();
                            exportedBookPrice = exportedBookPrice.Substring(0, exportedBookPrice.Length - 5);
                            string exportedBookTotal = (Convert.ToInt32(exportedBookPrice)* exportedBookAmount).ToString();

                            ExportedBookInfo info = new ExportedBookInfo
                                (bookNum, exportedBookReceiptID, exportBookID, exportBookURL, 
                                exportedBookName, exportedBookAmount,
                                exportedBookPrice, exportedBookTotal);
                            billinfo.bills.Add(info);
                            order++;
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi lấy thông tin Hóa Đơn: " + ex.Message);
                }
            }
        }

        public void SetState(int state) { this.state = state; }

        private void btn_ExitApp_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            Connection connect = new Connection();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Delete from CT_PHIEUNHAP
                    string sqlQuery = "DELETE FROM CT_HOADON WHERE MaHD = @MaHD";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaHD", BillID);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();

                    //Delete from CT_PHIEUNHAP
                    sqlQuery = "DELETE FROM HOADON WHERE MaHD = @MaHD";
                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaHD", BillID);
                    reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi xóa Hóa Đơn: " + ex.Message);
                }
                parent.LoadAll(parent);
                this.Hide();
            }
        }

        private void btn_SaveAsPDF_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
