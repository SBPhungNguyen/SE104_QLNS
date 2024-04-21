using SE104_QLNS.View;
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
    /// Interaction logic for BookDeletePopup.xaml
    /// </summary>
    public partial class BookDeletePopup : Window
    {
        public bool IsClosing = false;
        public string BookURL
        {
            get; set;
        }
        public BookDeletePopup()
        {
            InitializeComponent();
        }
        public BookDeletePopup(Uct_Books book)
        {
            InitializeComponent();
            this.tbl_BookID.Text = book.BookID;
            this.tbl_BookName.Text = book.BookName;
            this.tbl_Gerne.Text = book.BookGenre;
            this.tbl_Author.Text = book.BookAuthor;
            this.tbl_ImportPrice.Text = book.BookPriceImport;
            this.tbl_ExportPrice.Text = book.BookPriceExport;
            this.tbl_Quantity.Text = book.Amount;
            this.BookURL = book.BookURL;
            BitmapImage bimage = new BitmapImage();
            bimage.BeginInit();
            bimage.UriSource = new Uri(BookURL, UriKind.Relative);
            bimage.EndInit();
            img_BookImg.Source = bimage;
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            IsClosing = true;
            this.Close();
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            IsClosing = true;
            this.Close();
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
