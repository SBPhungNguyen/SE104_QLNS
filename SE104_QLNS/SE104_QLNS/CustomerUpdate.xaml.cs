﻿using SE104_QLNS.View;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
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
    /// Interaction logic for CustomerUpdate.xaml
    /// </summary>
    public partial class CustomerUpdate : Window
    {

        public MainWindow parent;
        public Uct_Customer selectedcustomer;
        public bool IsClosing = false;
        public CustomerUpdate()
        {
            InitializeComponent();
        }
        public CustomerUpdate(Uct_Customer customer, MainWindow mainwindow)
        {
            InitializeComponent();
            parent = mainwindow;
            selectedcustomer = customer;
            this.tbl_CustomerID.Text = customer.CustomerID;
            this.txt_CustomerMail.Text = customer.CustomerEmail;
            this.txt_CustomerBirthday.Text = customer.CustomerBirthday;
            this.txt_CustomerName.Text = customer.CustomerName;
            this.txt_CustomerPhone.Text = customer.CustomerPhonenumber;
            this.cbx_Gender.Text = customer.CustomerGender;
            this.txt_CustomerAddress.Text = customer.CustomerAddress;
            this.tbl_CustomerSpent.Text = customer.CustomerSpending;
            this.tbl_CustomerDebt.Text = customer.CustomerDebt;
        }

        private void btn_ExitApp_Click(object sender, RoutedEventArgs e)
        {
            IsClosing = true;
            this.Close();
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            IsClosing = true;
            this.Close();
        }

        private void btn_Update_Click(object sender, RoutedEventArgs e)
        {
            Connection connect = new Connection();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                string sqlQuery = "UPDATE KHACHHANG SET HoTenKH = @HoTenKH, SDT = @SDT, " +
                                   "DiaChi = @DiaChi, Email = @Email, SoTienNo = @SoTienNo, " +
                                   "GioiTinh = @GioiTinh, NgaySinh = @NgaySinh, SoTienMua = @SoTienMua " +
                                   "WHERE MAKH = @MaKH"; // Use @MaKH only once in WHERE clause
                SqlCommand command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@MaKH", tbl_CustomerID.Text);
                command.Parameters.AddWithValue("@HoTenKH", txt_CustomerName.Text);
                command.Parameters.AddWithValue("@SDT", txt_CustomerPhone.Text);
                command.Parameters.AddWithValue("@DiaChi", txt_CustomerAddress.Text);
                command.Parameters.AddWithValue("@Email", txt_CustomerMail.Text);
                command.Parameters.AddWithValue("@SoTienNo", tbl_CustomerDebt.Text);
                string gender;
                if (cbx_Gender.Text == "Nam")
                    gender = "1";
                else
                    gender = "0";
                command.Parameters.AddWithValue("@GioiTinh", gender);
                command.Parameters.AddWithValue("@NgaySinh", DateTime.Parse(txt_CustomerBirthday.Text));
                command.Parameters.AddWithValue("@SoTienMua", tbl_CustomerSpent.Text);

                SqlDataReader reader = command.ExecuteReader();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error updatong customer: " + ex.Message);
                }
                parent.LoadCustomer(parent, 0);
                IsClosing = true;
                this.Close();
            }
        }

    }
}