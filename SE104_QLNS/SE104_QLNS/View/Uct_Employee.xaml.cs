using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        public string EmployeeShift
        { get; set; }
        public string EmployeeTK
        { get; set; }
        public string EmployeePass
        { get; set; }
        public string PicURL
        { get; set; }
        public string Img_Type { get; set; }
        public Uct_Employee()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public Uct_Employee(MainWindow mainwindow, int state)
        {
            InitializeComponent();
            this.DataContext = this;
            parent = mainwindow;
            this.state = state;
            switch (state)
            {
                case 0:
                    Img_Type = "/Images/icon_info.png";
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
            string employeePhonenumber, string employeeAddress, string employeeOccupation, string employeeShift, string employeeTK, string employeePass, string HinhAnh)
        {
            EmployeeID = employeeID;
           
            EmployeeName = employeeName;
            EmployeeGender = employeeGender;
            EmployeeBirthday = employeeBirthday;
            EmployeeCard = employeeCard;
            EmployeePhonenumber = employeePhonenumber;            
            EmployeeAddress = employeeAddress;
            EmployeeOccupation = employeeOccupation;
            EmployeeShift = employeeShift;
            EmployeeTK = employeeTK;
            EmployeePass = employeePass;
            PicURL = HinhAnh;
        }
        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void btn_Button_Click(object sender, RoutedEventArgs e)
        {
            if (state == 0) //Default
            {
                EmployeeInfo employee = new EmployeeInfo(EmployeeID, EmployeeName,
                    EmployeePhonenumber, EmployeeBirthday, EmployeeGender, EmployeeCard,
                    EmployeeAddress, EmployeeOccupation, EmployeeShift, EmployeeTK, EmployeePass, PicURL);
                employee.Visibility = Visibility.Visible;
                employee.Topmost = true;

            }
            else if (state == 1) //Update
            {
                EmployeeUpdate employee = new EmployeeUpdate(this, parent);
                employee.Visibility = Visibility.Visible;
                employee.Topmost = true;
            }
            else //Delete
            {
                EmployeeDelete employee = new EmployeeDelete(this, parent);
                employee.Visibility = Visibility.Visible;
                employee.Topmost = true;
            }
        }
    }
}
