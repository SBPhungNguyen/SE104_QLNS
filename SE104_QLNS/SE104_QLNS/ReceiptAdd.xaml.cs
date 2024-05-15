using SE104_QLNS.View;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for ReceiptAdd.xaml
    /// </summary>

    public partial class ReceiptAdd : Window
    {
        public MainWindow parent;
        public ReceiptAdd()
        {
            InitializeComponent();
        }
        public ReceiptAdd(MainWindow mainwindow)
        {
            InitializeComponent();
            parent = mainwindow;
            LoadCustomerID();
            cbx_CustomerID.SelectedIndex = 0;
        }
        public void LoadCustomerID()
        {
            Connection connect = new Connection();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = "SELECT * FROM KHACHHANG";
                SqlCommand command = new SqlCommand(sqlQuery, connection);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string MaKH = reader["MaKH"].ToString();
                        cbx_CustomerID.Items.Add(MaKH);
                    }
                }
                reader.Close();
            }
        }
        private void btn_ExitApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_SaveCustomerReceipt_Click(object sender, RoutedEventArgs e)
        {
            string ReceiptID = parent.GetNextCustomerReceiptID(parent);
            Connection connect = new Connection();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    //make phieuthutien
                    connection.Open();
                    string sqlQuery = $"INSERT INTO PHIEUTHUTIEN (MaPhieuThuTien, MaKh, SoTienThu, NgayThuTien, SoTienSauThu, SoTienTruocThu) " +
                      $"VALUES (@MaPhieuThuTien, @MaKH, @SoTienThu, @NgayThuTien, @SoTienSauThu, @SoTienTruocThu)";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaPhieuThuTien", ReceiptID);
                    command.Parameters.AddWithValue("@MaKH", cbx_CustomerID.Text);
                    command.Parameters.AddWithValue("@SoTienThu", tbl_Paid.Text);
                    command.Parameters.AddWithValue("@NgayThuTien", DateTime.Now);
                    command.Parameters.AddWithValue("@SoTienTruocThu", tbx_DebtBefore.Text);
                    command.Parameters.AddWithValue("@SoTienSauThu", tbl_DebtAfter.Text);

                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();

                    sqlQuery = $"UPDATE KHACHHANG SET SOTIENNO=@SOTIENNO WHERE MaKH=@MaKH";
                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@SOTIENNO", Convert.ToInt32(tbl_DebtAfter.Text));
                    command.Parameters.AddWithValue("@MaKH", cbx_CustomerID.Text);
                    reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();

                    parent.UpdateBaoCaoCongNo(connection, cbx_CustomerID.Text, -Convert.ToInt32(tbl_Paid.Text));
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error retrieving data: " + ex.Message);
                }
            }
            parent.LoadCustomer(parent, 0);
            parent.LoadCustomerReceipt(parent, 0);
            this.Close();
        }

        private void cbx_CustomerID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbx_CustomerID.SelectedItem == null) return;
            cbx_CustomerID.Text = cbx_CustomerID.SelectedItem.ToString();
            Connection connect = new Connection();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sqlQuery = "SELECT * FROM KHACHHANG WHERE MaKH=@MaKH";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaKH", cbx_CustomerID.Text);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        tbl_CustomerName.Text = reader["HoTenKH"].ToString();
                        tbl_CustomerPhone.Text = reader["SDT"].ToString();
                        tbl_CustomerAddress.Text = reader["DiaChi"].ToString();
                        tbl_CustomerEmail.Text = reader["Email"].ToString();
                        string TienNo = reader["SoTienNo"].ToString();
                        tbx_DebtBefore.Text= TienNo.Substring(0, TienNo.Length - 5);
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error retrieving data from KHACHHANG: " + ex.Message);
                }
            }
        }

        private void tbl_Paid_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((tbx_DebtBefore == null)|| (tbx_DebtBefore.Text=="")) return;
            int parsedValue;

            if (int.TryParse(tbl_Paid.Text, out parsedValue))
            {
                if (parsedValue > Convert.ToInt32(tbx_DebtBefore.Text))
                {
                    tbl_Paid.Text = tbx_DebtBefore.Text;
                    tbl_DebtAfter.Text = "0";
                }
                else
                {
                    parsedValue = Convert.ToInt32(tbx_DebtBefore.Text) - parsedValue;
                    tbl_DebtAfter.Text = parsedValue.ToString();
                }
            }
            else
            {
                tbl_Paid.Text = ""; // Clear the textbox
            }
        }

    }
}
