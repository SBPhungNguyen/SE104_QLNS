using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Interaction logic for ReceiptDelete.xaml
    /// </summary>
    public partial class ReceiptDelete : Window
    {
        string ReceiptID;
        public MainWindow parent;
        public ReceiptDelete()
        {
            InitializeComponent();
        }
        public ReceiptDelete(Uct_CustomerReceipt info, MainWindow parent)
        {
            InitializeComponent();
            ReceiptID = info.ReceiptID;
            this.tbl_CustomerID.Text = info.CustomerID;
            this.tbl_CustomerName.Text = info.CustomerName;
            this.tbl_CustomerPhone.Text = info.CustomerNumber;
            this.tbl_CustomerEmail.Text = info.CustomerEmail;
            this.tbl_CustomerAddress.Text = info.CustomerAddress;
            this.tbl_DebtBefore.Text = info.DebtBefore;
            this.tbl_Paid.Text = info.Payment;
            this.tbl_DebtAfter.Text = info.DebtAfter;
            this.parent = parent;
        }

        private void btn_ExitApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
                    string sqlQuery = $"DELETE FROM PHIEUTHUTIEN WHERE MaPhieuThuTien = @MaPhieuThuTien";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaPhieuThuTien", ReceiptID); // Assuming MaKH is stored in txt_CustomerID textbox

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Customer deleted successfully
                        Notification noti = new Notification("Thành công", "Xóa Phiếu thu thành công!");
                    }
                    else
                    {
                        // No customer found with the given MaKH
                        Notification noti = new Notification("Lỗi", "Không tìm thấy phiếu thu!");
                    }
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi xóa phiếu thu: " + ex.Message);
                }
                parent.LoadAll(parent);
                this.Close();
            }
        }

        private void btn_Delete_Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
