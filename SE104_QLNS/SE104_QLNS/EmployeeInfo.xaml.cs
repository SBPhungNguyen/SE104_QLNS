using System;
using System.Collections.Generic;
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
    /// Interaction logic for EmployeeInfo.xaml
    /// </summary>
    public partial class EmployeeInfo : Window
    {
        public bool IsClosing = false;
        public string PicURL { get; set; }
        public EmployeeInfo()
        {
            InitializeComponent();
        }
        public EmployeeInfo(string employeeID, string employeeName, string employeeBirthday, string employeeGender, string employeeCard,
            string employeePhonenumber, string employeeAddress, string employeeOccupation, string employeeShift, string employeeTK, string employeePass, string picURL)
        {
            InitializeComponent();
            tbl_EmployeeID.Text = employeeID;
            tbl_EmployeeName.Text = employeeName;
            tbl_EmployeeGender.Text = employeeGender;
            tbl_EmployeeBirthday.Text = employeeBirthday;
            tbl_EmployeeCard.Text = employeeCard;
            tbl_EmployeePhone.Text = employeePhonenumber;

            tbl_EmployeeAddress.Text = employeeAddress;
            tbl_EmployeeOccupation.Text = employeeOccupation;
            tbl_EmployeeShift.Text = employeeShift;
            tbl_EmployeeTK.Text = employeeTK;
            tbl_EmployeePass.Text = employeePass;
            PicURL = picURL;
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
        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            if (IsClosing)
            {
                return;
            }
            IsClosing = true;
            this.Close();
        }
    
    }
}
