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
    /// Interaction logic for Uct_Author.xaml
    /// </summary>
    public partial class Uct_Author : UserControl
    {
        Connection connect = new Connection();

        public int state = 0;
        public MainWindow parent;
        public string AuthorID { get; set; }
        public string AuthorName { get; set; }
        public string Img_Type = "/Images/Img_user_icon.png";
        public Uct_Author()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public Uct_Author(MainWindow mainwindow)
        {
            InitializeComponent();
            this.DataContext = this;
            parent = mainwindow;
        }

        public void AuthorSetState(int state)
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
        public void LoadData(string authorID, string authorName)
        {
            AuthorID = authorID;
            AuthorName = authorName;

        }

    }
}
