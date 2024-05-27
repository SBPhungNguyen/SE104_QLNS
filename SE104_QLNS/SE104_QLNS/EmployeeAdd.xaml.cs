using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
                    command.Parameters.AddWithValue("@NgaySinh", DateTime.Parse(tbx_EmployeeBirthday.Text));
                    string gender;
                    if (cbx_Gender.Text == "Nam")
                        gender = "True";
                    else
                        gender = "False";
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
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi thêm nhân viên: " + ex.Message);
                }
            }

            parent.LoadAll(parent);
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
                    }

                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi khi mở hình ảnh: " + ex.Message);
                }
            });
        }

        private void dpk_EmployeeBirthday_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            tbx_EmployeeBirthday.Text= dpk_EmployeeBirthday.SelectedDate.Value.Date.ToString().Substring(0, 10); ;
        }

        private void txt_EmployeePhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(txt_EmployeePhone.Text, out int parsedValue))
            {
                txt_EmployeePhone.Text = "0";
            }
        }

        private void txt_EmployeeCard_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "\b")
            {
                return;
            }

            // Only allow numbers and decimal point (if allowed)
            e.Handled = !Regex.IsMatch(e.Text, "[0-9.]");
        }

        private void txt_EmployeePhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "\b")
            {
                return;
            }

            // Only allow numbers and decimal point (if allowed)
            e.Handled = !Regex.IsMatch(e.Text, "[0-9.]");
        }
    }
}
