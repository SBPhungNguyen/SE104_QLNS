﻿using SE104_QLNS.View;
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
    /// Interaction logic for EmployeeUpdate.xaml
    /// </summary>
    public partial class EmployeeUpdate : Window
    {
        public MainWindow parent;
        public Uct_Employee selectedemployee;
        public bool IsClosing = false;
        public EmployeeUpdate()
        {
            InitializeComponent();
        }
        public EmployeeUpdate(Uct_Employee employee, MainWindow mainwindow)
        {
            InitializeComponent();
            parent = mainwindow;
            selectedemployee = employee;
            this.tbl_EmployeeID.Text = employee.EmployeeID;
            this.txt_EmployeeName.Text = employee.EmployeeName;
            this.cbx_Gender.Text = employee.EmployeeGender;
            this.txt_EmployeeBirthday.Text = employee.EmployeeBirthday;
            this.txt_EmployeeCard.Text = employee.EmployeeCard;
            this.txt_EmployeePhone.Text = employee.EmployeePhonenumber;
            this.txt_EmployeeAddress.Text = employee.EmployeeAddress;
            this.txt_EmployeeOccupation.Text = employee.EmployeeOccupation;
            this.txt_EmployeeShift.Text = employee.EmployeeShift;
            this.txt_EmployeeTK.Text = employee.EmployeeTK;
            this.txt_EmployeePass.Text = employee.EmployeePass;
        }

        private void btn_ExitApp_Click(object sender, RoutedEventArgs e)
        {
            IsClosing = true;
            this.Close();
        }

        private void btn_EmployeeUpdate_Click(object sender, RoutedEventArgs e)
        {
            Connection connect = new Connection();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sqlQuery = "UPDATE NGUOIDUNG SET HoTenNV = @HoTenNV, SDT = @SDT, DiaChi = @DiaChi, " +
                                       "GioiTinh = @GioiTinh, NgaySinh = @NgaySinh, CCCD = @CCCD, " +
                                       "ViTri = @ViTri, Ca = @Ca, TenTK = @TenTK, MatKhau = @MatKhau, " +
                                       "WHERE MANV = @MaNV"; // Use @MaNV only once in WHERE clause
                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    command.Parameters.AddWithValue("@MaNV", tbl_EmployeeID.Text);
                    command.Parameters.AddWithValue("@HoTenNV", txt_EmployeeName.Text);
                    command.Parameters.AddWithValue("@SDT", txt_EmployeePhone.Text);
                    command.Parameters.AddWithValue("@DiaChi", txt_EmployeeAddress.Text);
                    string gender;
                    if (cbx_Gender.Text == "Nam")
                        gender = "1";
                    else
                        gender = "0";
                    command.Parameters.AddWithValue("@GioiTinh", gender);
                    command.Parameters.AddWithValue("@NgaySinh", DateTime.Parse(txt_EmployeeBirthday.Text));
                    command.Parameters.AddWithValue("@CCCD", txt_EmployeeCard.Text);
                    command.Parameters.AddWithValue("@Ca", txt_EmployeeShift.Text);
                    command.Parameters.AddWithValue("@ViTri", txt_EmployeeOccupation.Text);
                    command.Parameters.AddWithValue("@TenTK", txt_EmployeeTK.Text);
                    command.Parameters.AddWithValue("@MatKhau", txt_EmployeePass.Text);
                    SqlDataReader reader = command.ExecuteReader();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error updatong employee: " + ex.Message);
                }
                parent.LoadEmployee(parent, 0);
                IsClosing = true;
                this.Close();
            }
        }
    }
}
