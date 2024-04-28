﻿using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SE104_QLNS.View
{
    /// <summary>
    /// Interaction logic for Uct_Employee.xaml
    /// </summary>
    public partial class Uct_Employee : UserControl
    {
        Connection connect = new Connection();

        public int state = 0;
        public MainWindow parent;
        public string Icon
        { get; set; }
        public string EmployeeID
        { get; set; }
        public string EmployeeName
        { get; set; }
        public string EmployeeGender
        { get; set; }
        public string EmployeeBirthday
        { get; set; }
        public string EmployeeCard
        { get; set; }
        public string EmployeePhonenumber
        { get; set; }
       
        public string EmployeeAddress
        { get; set; }
        public string EmployeeOccupation
        { get; set; }
        public string EmployeeTime
        { get; set; }
        public string EmployeeTK
        { get; set; }
        public string EmployeePass
        { get; set; }
        public string PicURL
        { get; set; }
        public string Img_Type = "/Images/Img_user_icon.png";
        public Uct_Employee()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public Uct_Employee(MainWindow mainwindow)
        {
            InitializeComponent();
            this.DataContext = this;
            parent = mainwindow;
        }
        public void EmployeeSetState(int state)
        {
            this.state = state;
            switch (state)
            {
                case 0:
                    Img_Type = "/Images/Img_user_icon.png";
                    break;
                case 1:
                    Img_Type = "/Images/icon_pencil.png";
                    break;
                case 2:
                    Img_Type = "/Images/icon_bin.png";
                    break;
            }
        }
        public void LoadData(string employeeID, string employeeName, string employeeBirthday, string employeeGender, string employeeCard,
            string employeePhonenumber, string employeeAddress, string employeeOccupation, string employeeTime, string employeeTK, string employeePass )
        {
            EmployeeID = employeeID;
            EmployeeName = employeeName;
            EmployeeGender = employeeGender;
            EmployeeBirthday = employeeBirthday;
            EmployeeCard = employeeCard;
            EmployeePhonenumber = employeePhonenumber;            
            EmployeeAddress = employeeAddress;
            EmployeeOccupation = employeeOccupation;
            EmployeeTime = employeeTime;
            EmployeeTK = employeeTK;
            EmployeePass = employeePass;
        }
        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string connectionString = connect.connection;

            if (state == 0) //Default
            {
                EmployeeInfo employee = new EmployeeInfo(EmployeeID, EmployeeName,
                    EmployeePhonenumber, EmployeeBirthday, EmployeeGender, EmployeeCard, 
                    EmployeeAddress, EmployeeOccupation, EmployeeTime, EmployeeTK, EmployeePass);
                employee.Visibility = Visibility.Visible;
                employee.Topmost = true;
            }
           
        }

    }
}
