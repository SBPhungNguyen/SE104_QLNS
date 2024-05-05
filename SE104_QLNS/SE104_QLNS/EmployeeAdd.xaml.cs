using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
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
    /// Interaction logic for EmployeeAdd.xaml
    /// </summary>
    public partial class EmployeeAdd : Window
    {
        public bool IsClosing = false;
        public MainWindow parent;
        public string HinhAnh = "/Images/Img_user_icon.png";
        public EmployeeAdd()
        {
            InitializeComponent();
        }
        public EmployeeAdd(MainWindow mainwindow)
        {
            InitializeComponent();
            this.parent = mainwindow;
            txt_EmployeeID.Text = mainwindow.GetNextEmployeeID(mainwindow);
            CreateImage(HinhAnh);
        }
        public void CreateImage(string url)
        {
            BitmapImage bimage = new BitmapImage();
            bimage.BeginInit();
            bimage.UriSource = new Uri(url, UriKind.RelativeOrAbsolute);
            bimage.EndInit();
            img_EmployeeImage.Source = bimage;
        }
        private void btn_EmployeeAdd_Click(object sender, RoutedEventArgs e)
        {
            Connection connect = new Connection();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sqlQuery = $"INSERT INTO NGUOIDUNG (MANV, HoTenNV, SDT, NgaySinh, GioiTinh, DiaChi, CCCD, ViTri, Ca, TenTK, MatKhau, HinhAnh) " +
                      $"VALUES (@MaNV, @HoTenNV, @SDT, @NgaySinh, @GioiTinh, @DiaChi, @CCCD, @ViTri, @Ca, @TenTK, @MatKhau, @HinhAnh)";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaNV", txt_EmployeeID.Text);

                    command.Parameters.AddWithValue("@HoTenNV", txt_EmployeeName.Text);
                    command.Parameters.AddWithValue("@SDT", txt_EmployeePhone.Text);
                    command.Parameters.AddWithValue("@NgaySinh", txt_EmployeeBirthday.Text);
                    string gender;
                    if (cbx_Gender.Text == "Nam")
                        gender = "1";
                    else
                        gender = "0";
                    command.Parameters.AddWithValue("@GioiTinh", gender);
                    command.Parameters.AddWithValue("@DiaChi", txt_EmployeeAddress.Text);
                    command.Parameters.AddWithValue("@CCCD", txt_EmployeeCard.Text);
                    command.Parameters.AddWithValue("@ViTri", txt_EmployeeOccupation.Text);

                    command.Parameters.AddWithValue("@HinhAnh", HinhAnh);

                    command.Parameters.AddWithValue("@Ca", txt_EmployeeShift.Text);
                    command.Parameters.AddWithValue("@TenTK", txt_EmployeeTK.Text);
                    command.Parameters.AddWithValue("@MatKhau", txt_EmployeePass.Text);
                    SqlDataReader reader = command.ExecuteReader();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error retrieving data: " + ex.Message);
                }
            }

            parent.LoadEmployee(parent, 0);
            IsClosing = true;
            this.Close();
        }

        private void btn_ExitApp_Click(object sender, RoutedEventArgs e)
        {
            IsClosing = true;
            this.Close();
        }

        private void btn_EmployeeImage_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                try
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*";
                    if (openFileDialog.ShowDialog() == true)
                    {
                        string selectedImagePath = openFileDialog.FileName;
                        HinhAnh = selectedImagePath;
                        CreateImage(HinhAnh);
                        //Notification noti = new Notification("Updated", BookURL); .Replace("\\","/")
                    }

                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error opening file: " + ex.Message);
                }
            });
        }
    }
}
