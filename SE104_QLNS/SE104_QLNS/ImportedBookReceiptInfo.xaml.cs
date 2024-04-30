using SE104_QLNS.Model;
using SE104_QLNS.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
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
    /// Interaction logic for ImportedBookReceiptInfo.xaml
    /// </summary>
    /// 
    public partial class ImportedBookInfo
    {
        public ImportedBookInfo()
        {
        }
    public int BookNum { get; set; }
        public string ImportBookReceiptID { get; set; }
        public string ImportBookID { get; set; }
        public string ImportBookURL { get; set; }
        public string ImportNum { get; set; }
        public string ImportPrice { get; set; }
        public ImportedBookInfo(int bookNum, string importBookReceiptID, string importBookID, string importBookURL, string importNum, string importPrice)
        {
            BookNum = bookNum;
            ImportBookReceiptID = importBookReceiptID;
            ImportBookID = importBookID;
            ImportBookURL = importBookURL;
            ImportNum = importNum;
            ImportPrice = importPrice;
        }
    }
    public partial class ImportedBookReceiptInfo : Window
    {
        MainWindow parent;
        int state = 0;
        public ObservableCollection<ImportedBookInfo> ImportInfo { get; set; } = new ObservableCollection<ImportedBookInfo>();
        Connection connect = new Connection();
        public ImportedBookReceiptInfo()
        {
            InitializeComponent();
        }
        public string ImportBookReceiptNum { get; set; }
        public string ImportBookReceiptID { get; set; }
        public string Date { get; set; }
        public string Total { get; set; }
        public ImportedBookReceiptInfo(MainWindow mainwindow, int state)
        {
            InitializeComponent();
            this.parent = mainwindow;
            this.state = state;
            if(state==0)
            {
                btn_DeleteOk.Visibility = Visibility.Hidden;
                btn_DeleteCancel.Visibility= Visibility.Hidden;
                btn_Ok.Visibility = Visibility.Visible;
                ImportedBookReceiptID.Text = "Phiếu Nhập ";
            }
            else
            {
                btn_DeleteOk.Visibility = Visibility.Visible;
                btn_DeleteCancel.Visibility = Visibility.Visible;
                btn_Ok.Visibility = Visibility.Hidden;
                ImportedBookReceiptID.Text = "Bạn có chắc muốn xóa Phiếu Nhập ";
            }
        }
        public void LoadData(string ImportBookReceiptNum, string ImportBookReceiptID, string Date, string Total)
        {
            this.ImportBookReceiptNum = ImportBookReceiptNum;
            this.ImportBookReceiptID = ImportBookReceiptID;
            this.Date = Date;
            this.Total = Total;
            ImportedBookReceiptID.Text += ImportBookReceiptID;
            tbl_Date.Text = Date;
            tbl_MoneySum.Text = Total;
            LoadReceiptInfo(this);
            dtg_BillDetail.ItemsSource = ImportInfo;
            dtg_BillDetail.Items.Refresh();
        }

        public void LoadReceiptInfo(ImportedBookReceiptInfo importedBookReceiptInfo)
        {
            //connect to database
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string MaSach = "", SoLuongNhap = "", DonGiaNhap = "", BookURL="";

                try
                {
                    connection.Open();
                    string sqlQuery = "SELECT * FROM CT_PHIEUNHAP JOIN SACH ON CT_PHIEUNHAP.MASACH = SACH.MASACH WHERE MaPhieuNhap=@MaPhieuNhap ";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaPhieuNhap", importedBookReceiptInfo.ImportBookReceiptID);

                    int order = 1;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MaSach = reader["MaSach"].ToString();
                            SoLuongNhap = reader["SoLuongNhap"].ToString();
                            BookURL = reader["HinhAnhSach"].ToString();
                            DonGiaNhap = reader["DonGiaNhap"].ToString().Replace(",0000", "");
                            
                            ImportedBookInfo info = new ImportedBookInfo(order, importedBookReceiptInfo.ImportBookReceiptID
                                , MaSach, BookURL, SoLuongNhap, DonGiaNhap);
                            importedBookReceiptInfo.ImportInfo.Add(info);
                            order++;

                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error retrieving data: " + ex.Message);
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

        private void btn_DeleteOk_Click(object sender, RoutedEventArgs e)
        {
            Connection connect = new Connection();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Delete from CT_PHIEUNHAP
                    string sqlQuery = "DELETE FROM CT_PHIEUNHAP WHERE MaPhieuNhap = @MaPhieuNhap";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaPhieuNhap", ImportBookReceiptID);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();

                    //Delete from CT_PHIEUNHAP
                     sqlQuery = "DELETE FROM PHIEUNHAP WHERE MaPhieuNhap = @MaPhieuNhap";
                     command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaPhieuNhap", ImportBookReceiptID);
                     reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error Updating Book: " + ex.Message);
                }
                parent.LoadImportPaper(parent, 1);
                this.Close();
            }
        }

        private void btn_DeleteCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
