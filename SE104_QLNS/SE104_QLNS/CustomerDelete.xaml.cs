using SE104_QLNS.View;
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
    /// Interaction logic for CustomerDelete.xaml
    /// </summary>
    public partial class CustomerDelete : Window
    {
        public MainWindow parent;
        public Uct_Customer selectedcustomer;
        public bool IsClosing = false;
        public CustomerDelete()
        {
            InitializeComponent();
        }
        public CustomerDelete(Uct_Customer customer, MainWindow mainwindow)
        {
            InitializeComponent();
            parent = mainwindow;
            selectedcustomer = customer;
            this.tbl_CustomerID.Text = customer.CustomerID;
            this.tbl_CustomerMail.Text = customer.CustomerEmail;
            this.tbl_CustomerBirthday.Text = customer.CustomerBirthday;
            this.tbl_CustomerName.Text = customer.CustomerName;
            this.tbl_CustomerPhone.Text = customer.CustomerPhonenumber;
            this.tbl_CustomerGender.Text = customer.CustomerGender;
            this.tbl_CustomerAddress.Text = customer.CustomerAddress;
            this.tbl_CustomerSpent.Text = customer.CustomerSpending;
            this.tbl_CustomerDebt.Text = customer.CustomerDebt;
        }
    
        private void btn_ExitApp_Click(object sender, RoutedEventArgs e)
        {
            IsClosing = true;
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

                    string sqlQuery = $"DELETE FROM PHIEUTHUTIEN WHERE MAKH = @MaKH";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaKH", tbl_CustomerID.Text); // Assuming MaKH is stored in txt_CustomerID textbox

                    command.ExecuteNonQuery();

                    sqlQuery = @"
            DELETE CT_HOADON
            FROM CT_HOADON
            INNER JOIN HOADON ON CT_HOADON.MAHD = HOADON.MAHD
            WHERE HOADON.MAKH = @MaKH;

            DELETE HOADON
            WHERE MAKH = @MaKH;

            DELETE BAOCAOCONGNO
            WHERE MAKH = @MaKH";
                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaKH", tbl_CustomerID.Text); // Assuming MaKH is stored in txt_CustomerID textbox

                    command.ExecuteNonQuery();

                    sqlQuery = $"DELETE FROM KHACHHANG WHERE MAKH = @MaKH";
                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaKH", tbl_CustomerID.Text); // Assuming MaKH is stored in txt_CustomerID textbox

                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error deleting customer: " + ex.Message);
                }
                parent.LoadCustomer(parent, 0);
                parent.LoadBaoCaoCongNo(parent, 0);
                IsClosing = true;
                this.Close();
            }
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            IsClosing = true;
            this.Close();
        }
    }
}
