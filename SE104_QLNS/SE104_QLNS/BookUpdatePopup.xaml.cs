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
    /// Interaction logic for BookUpdatePopup.xaml
    /// </summary>
    public partial class BookUpdatePopup : Window
    {
        public MainWindow parent;
        public Uct_Books selectedbook;
        public bool IsClosing = false;
        public string BookURL
        {
            get; set;
        }
        public BookUpdatePopup()
        {
            InitializeComponent();
        }
        public BookUpdatePopup(Uct_Books book, MainWindow mainwindow)
        {
            InitializeComponent();
            this.tbl_BookID.Text = book.BookID;
            this.txt_BookName.Text = book.BookName;
            this.cbx_TheLoai.Text = book.BookGenre;
            this.tbl_Author.Text = book.BookAuthor;
            this.txt_ImportPrice.Text = book.BookPriceImport;
            this.txt_ExportPrice.Text = book.BookPriceExport;
            this.txt_Quantity.Text = book.Amount;
            this.BookURL = book.BookURL;
            BitmapImage bimage = new BitmapImage();
            bimage.BeginInit();
            bimage.UriSource = new Uri(BookURL, UriKind.Relative);
            bimage.EndInit();
            img_BookImg.Source = bimage;
            this.parent = mainwindow;
            this.selectedbook = book;
        }
        private void btn_Back_Click(object sender, RoutedEventArgs e)
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

        private void btn_Update_Click(object sender, RoutedEventArgs e)
        {
            foreach (Uct_Books book in parent.Books)
            {
                if (book.BookID == selectedbook.BookID)
                {
                    parent.Books.Remove(book);
                    break;
                }
            }
            selectedbook.LoadData(tbl_BookID.Text, txt_BookName.Text, tbl_Author.Text, cbx_TheLoai.Text, BookURL, selectedbook.BookNum, txt_ImportPrice.Text, txt_ExportPrice.Text, txt_Quantity.Text, selectedbook.Icon);
            parent.Books.Add(selectedbook);
            WrapPanel bookPanel = (WrapPanel)parent.FindName("wpn_Books");
            bookPanel.Children.Clear();
            foreach (Uct_Books bookAdd in parent.Books)
            {
                Uct_Books placeholder = new Uct_Books(1, parent);
                placeholder.BookID = bookAdd.BookID;
                placeholder.BookURL = bookAdd.BookURL;
                placeholder.BookName = bookAdd.BookName;
                placeholder.BookPriceImport = bookAdd.BookPriceImport;
                placeholder.Amount = bookAdd.Amount;
                placeholder.BookStateURL = "/Images/icon_bin.png";
                Uct_Books bookImport = placeholder;
                bookPanel.Children.Add(bookImport);
            }
            IsClosing = true;
            this.Close();
            IsClosing = true;
            this.Close();
        }
    }
}
