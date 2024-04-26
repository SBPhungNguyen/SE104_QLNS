﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SE104_QLNS.View
{
    /// <summary>
    /// Interaction logic for Uct_Customer.xaml
    /// </summary>
    public partial class Uct_Customer : UserControl
    {
        Connection connect = new Connection();

        public int state = 0;
        public MainWindow parent;
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhonenumber { get; set; }
        public string CustomerBirthday { get; set; }  
        public string CustomerGender { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerSpending { get; set; }
        public string CustomerDebt { get; set; }

        public string Img_Type = "/Images/Img_user_icon.png";
        public Uct_Customer()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public Uct_Customer(MainWindow mainwindow)
        {
            InitializeComponent();
            this.DataContext = this;
            parent = mainwindow;
        }

        public void CustomerSetState(int state)
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
        public void LoadData(string customerID, string customerName, string customerBirthday, string customerGender,
            string customerPhonenumber, string customerAddress, string customerEmail,  string customerSpending,
            string customerDebt)
        {
            CustomerID = customerID;
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            CustomerPhonenumber = customerPhonenumber;
            CustomerBirthday = customerBirthday;
            CustomerGender = customerGender;
            CustomerAddress = customerAddress;
            CustomerSpending = customerSpending;
            CustomerDebt = customerDebt;
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string connectionString = connect.connection;

            if (state == 0) //Default
            {
                CustomerInfo customer = new CustomerInfo(CustomerID, CustomerName, CustomerEmail,
                    CustomerPhonenumber, CustomerBirthday,
                   CustomerGender, CustomerAddress, CustomerSpending, CustomerDebt);
                customer.Visibility = Visibility.Visible;
                customer.Topmost = true;
            }
            else if ( state == 1) //Update
            {
                CustomerUpdate customer = new CustomerUpdate(this, parent);
                customer.Visibility = Visibility.Visible;
                customer.Topmost = true;
            }
            else //Delete
            {
                CustomerDelete customer = new CustomerDelete(this, parent);
                customer.Visibility = Visibility.Visible;
                customer.Topmost = true;
            }
        }
    }
}
