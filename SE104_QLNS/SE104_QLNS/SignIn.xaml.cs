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
using static System.Net.Mime.MediaTypeNames;

namespace SE104_QLNS
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class SignIn : Window
    {
        public SignIn()
        {
            InitializeComponent();
            //tb_username.Text = "admin";
            //tb_password.Password = "admin";
        }
        private void Login(string TenTK, string password)
        {
            Connection connect = new Connection();
            string connectionString = connect.connection;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sqlQuery = "SELECT TenTK, MatKhau FROM NGUOIDUNG WHERE TenTK = @TenTK";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@TenTK", TenTK);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        string dbTenTK = reader["TenTK"].ToString();
                        string dbPassword = reader["MatKhau"].ToString();

                        if (password == dbPassword)
                        {
                            // Show login successful notification
                            MainWindow main = new MainWindow();
                            main.Show();
                            this.Close();
                        }
                        else
                        {
                            // Show incorrect password notification
                            Notification notification = new Notification("Lỗi", "Sai mât khẩu!");
                            notification.Show();
                        }
                    }
                    else
                    {
                        // Show username not found notification
                        Notification notification = new Notification("Lỗi", "Không tìm thấy username!");
                        notification.Show();
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Notification notification = new Notification("Lỗi", "Đã gặp lỗi khi lấy thông tin đăng nhập: " + ex.Message);
                    notification.Show();
                }
            }

        }
        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            string TenTK = tb_username.Text;
            string password = tb_password.Password;
            Login(TenTK, password);
        }

        private void btn_ExitApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
