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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SE104_QLNS.View
{
    /// <summary>
    /// Interaction logic for Uct_Employee.xaml
    /// </summary>
    public partial class Uct_Employee : UserControl
    {
        public string Icon
        { get; set; }
        public string EmployeeID
        { get; set; }
        public string EmployeeName
        { get; set; }
        public string EmployeePhonenumber
        { get; set; }
        public string EmployeeOccupation
        { get; set; }
        public string EmployeeTime
        { get; set; }
        public string PicURL
        { get; set; }
        public Uct_Employee()
        {
            InitializeComponent();
            this.DataContext = this;

        }
    }
}
