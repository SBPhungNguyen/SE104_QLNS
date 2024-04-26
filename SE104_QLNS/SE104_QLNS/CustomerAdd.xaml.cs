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
    /// Interaction logic for CustomerAdd.xaml
    /// </summary>
    public partial class CustomerAdd : Window
    {
        public bool IsClosing = false;
        public MainWindow parent;
        public CustomerAdd()
        {
            InitializeComponent();
        }
        public CustomerAdd(MainWindow mainwindow)
        {
            InitializeComponent();
            this.parent = mainwindow;
            txt_CustomerID.Text = mainwindow.GetNextCustomerID(mainwindow);
        }

        private void btn_AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            Connection connect = new Connection();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sqlQuery = $"INSERT INTO KHACHHANG (MAKH, HoTenKH, Email, SDT, NgaySinh, GioiTinh, DiaChi, SoTienNo, SoTienMua) " +
                      $"VALUES (@MaKH, @HoTenKH, @Email, @SDT, @NgaySinh, @GioiTinh, @DiaChi, @SoTienNo, @SoTienMua)";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaKH", txt_CustomerID.Text);
                    command.Parameters.AddWithValue("@HoTenKH", txt_CustomerName.Text);
                    command.Parameters.AddWithValue("@Email", txt_CustomerEmail.Text);
                    command.Parameters.AddWithValue("@SDT", txt_CustomerPhone.Text);
                    command.Parameters.AddWithValue("@NgaySinh", txt_CustomeBirth.Text);
                    string gender;
                    if (cbx_Gender.Text == "Nam")
                        gender = "1";
                    else
                        gender = "0";
                    command.Parameters.AddWithValue("@GioiTinh", gender);
                    command.Parameters.AddWithValue("@DiaChi", txt_CustomerAddress.Text);
                    command.Parameters.AddWithValue("@SoTienNo", "0");
                    command.Parameters.AddWithValue("@SoTienMua", "0");

                    SqlDataReader reader = command.ExecuteReader();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error retrieving data: " + ex.Message);
                }
            }

            parent.LoadCustomer(parent, 0);
            IsClosing = true;
            this.Close();
        }

        private void btn_ExitApp_Click(object sender, RoutedEventArgs e)
        {
            IsClosing = true;
            this.Close();
        }
    }
}
