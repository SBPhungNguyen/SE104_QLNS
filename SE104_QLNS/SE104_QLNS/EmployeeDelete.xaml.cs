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
    /// Interaction logic for EmployeeDelete.xaml
    /// </summary>
    public partial class EmployeeDelete : Window
    {
        public MainWindow parent;
        public Uct_Employee selectedemployee;
        public bool IsClosing = false;
        public string PicURL {  get; set; }
        public EmployeeDelete()
        {
            InitializeComponent();
        }
        public EmployeeDelete(Uct_Employee employee, MainWindow mainwindow)
        {
            InitializeComponent();
            parent = mainwindow;
            selectedemployee = employee;
            this.tbl_EmployeeID.Text = employee.EmployeeID;
            this.tbl_EmployeeName.Text = employee.EmployeeName;
            this.tbl_EmployeeGender.Text = employee.EmployeeGender;
            this.tbl_EmployeeBirthday.Text = employee.EmployeeBirthday;
            this.tbl_EmployeeCard.Text = employee.EmployeeCard;
            this.tbl_EmployeePhone.Text = employee.EmployeePhonenumber;
            this.tbl_EmployeeAddress.Text = employee.EmployeeAddress;
            this.tbl_EmployeeOccupation.Text = employee.EmployeeOccupation;
            this.tbl_EmployeeShift.Text = employee.EmployeeShift;
            PicURL = employee.PicURL;

            BitmapImage bimage = new BitmapImage();
            bimage.BeginInit();
            bimage.UriSource = new Uri(PicURL, UriKind.RelativeOrAbsolute);
            bimage.EndInit();
            img_EmployeeImage.Source = bimage;
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
                    string sqlQuery = $"DELETE FROM NGUOIDUNG WHERE MANV = @MaNV";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaNV", tbl_EmployeeID.Text); // Assuming MaKH is stored in txt_EmployeeID textbox

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Employee deleted successfully
                        Notification noti = new Notification("Success", "Employee deleted successfully!");
                    }
                    else
                    {
                        // No employee found with the given MaKH
                        Notification noti = new Notification("Error", "Employee not found!");
                    }
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error deleting employee: " + ex.Message);
                }
                parent.LoadEmployee(parent, 0);
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
