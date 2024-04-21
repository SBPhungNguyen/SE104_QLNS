using SE104_QLNS.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SE104_QLNS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Uct_Books> Books { get; set; } = new ObservableCollection<Uct_Books>();

        Uct_Books selectedbook = null;
        Uct_Books selectedbookdelete = null;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            //Day la code mau (vi du ve load thong tin sach)
            Uct_Books book = new Uct_Books();
            book.LoadData("TT012321", "Alice in da WonderLand", "Tac Gia", "Tieu Thuyet", "/Images/Chill_Vibes_R_Wallpaper.png", "76", "10000", "110000", "100", "/Images/Img_Information.png");
            //wpn_Books.Children.Add(book);
            Books.Add(book);
            book = new Uct_Books();
            book.LoadData("TT012322", "Alice in da hell", "Tac Gia", "Tieu Thuyet", "/Images/Chill_Vibes_R_Wallpaper.png", "76", "10000", "110000", "100", "/Images/Img_Information.png");
            Books.Add(book);
            book = new Uct_Books();
            book.LoadData("TT012323", "Alice in da AAAAAA", "Tac Gia", "Tieu Thuyet", "/Images/Chill_Vibes_R_Wallpaper.png", "76", "10000", "110000", "100", "/Images/Img_Information.png");
            Books.Add(book);
            book = new Uct_Books();
            book.LoadData("TT012324", "Alice in da doanxem", "Tac Gia", "Tieu Thuyet", "/Images/Chill_Vibes_R_Wallpaper.png", "76", "10000", "110000", "100", "/Images/Img_Information.png");
            Books.Add(book);

            //Add Book to Wrap Panel
            WrapPanel bookPanel = (WrapPanel)this.FindName("wpn_Books");

            // Loop through Books and create Uct_BookImport elements (assuming this is the view for each book)
            foreach (Uct_Books bookAdd in Books)
            {
                Uct_Books bookImport = new Uct_Books();
                bookImport.BookID= bookAdd.BookID;
                bookImport.BookURL = bookAdd.BookURL;
                bookImport.BookName = bookAdd.BookName;
                bookImport.BookPriceImport = bookAdd.BookPriceImport;
                bookImport.Amount = bookAdd.Amount;
                bookPanel.Children.Add(bookImport);
            }
        }

        //Khi nhan nut ImportBooks
        private void btn_ImportBooks_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_ExitApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }

        private void btn_MinimizeApp_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btn_AddBook_Click(object sender, RoutedEventArgs e) // Click on Them/Add Button
        {
            if (cvs_ImportBooks.Visibility == Visibility.Hidden) //Swap from Default to Add
            {
                //set other hidden
                cvs_BooksGridList.Visibility = Visibility.Hidden;
                cvs_BooksDataGridListDelete.Visibility = Visibility.Hidden;
                cvs_BooksDataGridList.Visibility = Visibility.Hidden;

                //set this visible
                cvs_ImportBooks.Visibility = Visibility.Visible;

                dtg_ImportBooks.Items.Refresh();
                btn_AddBook.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                btn_SwitchView.Background = new SolidColorBrush(Colors.Transparent);
                btn_DeleteBook.Background = new SolidColorBrush(Colors.Transparent);
            }
            else //Swap from Add to Default
            {
                cvs_BooksDataGridList.Visibility=Visibility.Visible;
                cvs_ImportBooks.Visibility = Visibility.Hidden;
                dtg_Books.Items.Refresh(); 
                btn_AddBook.Background = new SolidColorBrush(Colors.Transparent);

            }
        }
        private void btn_SwitchView_Click(object sender, RoutedEventArgs e)
        {
            if(cvs_BooksGridList.Visibility == Visibility.Hidden) //Swap From List (Default) to Table
            {
                //set other hidden
                cvs_BooksDataGridList.Visibility = Visibility.Hidden;
                cvs_ImportBooks.Visibility= Visibility.Hidden;
                cvs_BooksDataGridListDelete.Visibility = Visibility.Hidden;

                //set this visible
                cvs_BooksGridList.Visibility = Visibility.Visible;

                //button color
                btn_SwitchView.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                btn_DeleteBook.Background = new SolidColorBrush(Colors.Transparent);
                btn_AddBook.Background = new SolidColorBrush(Colors.Transparent);
            }
            else //Swap From Table To List (Default)
            {
                cvs_BooksDataGridList.Visibility = Visibility.Visible;
                cvs_BooksGridList.Visibility = Visibility.Hidden;
                dtg_Books.Items.Refresh();
                btn_SwitchView.Background = new SolidColorBrush(Colors.Transparent);
            }
        }
        private void btn_DeleteBook_Click(object sender, RoutedEventArgs e)
        {
            if (cvs_BooksDataGridListDelete.Visibility == Visibility.Hidden) //Switch from Default to Delete
            {
                //set other hidden
                cvs_BooksDataGridList.Visibility = Visibility.Hidden;
                cvs_BooksGridList.Visibility = Visibility.Hidden;
                cvs_ImportBooks.Visibility = Visibility.Hidden;

                //set delete visible
                cvs_BooksDataGridListDelete.Visibility = Visibility.Visible;

                dtg_BooksDelete.Items.Refresh();

                //set button color
                btn_DeleteBook.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                btn_SwitchView.Background = new SolidColorBrush(Colors.Transparent);
                btn_AddBook.Background = new SolidColorBrush(Colors.Transparent);
            }
            else //Switch from Delete to Default
            {
                cvs_BooksDataGridList.Visibility = Visibility.Visible;
                cvs_BooksDataGridListDelete.Visibility = Visibility.Hidden;
                dtg_Books.Items.Refresh();
                btn_DeleteBook.Background = new SolidColorBrush(Colors.Transparent);
            }

        }

        private void dtg_Books_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedbook = (Uct_Books)dtg_Books.SelectedItem;
            BookInfoPopup bookInfoPopup = new BookInfoPopup(selectedbook);
            bookInfoPopup.Show();
            selectedbook = null;
        }

        private void dtg_BooksDelete_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedbookdelete = (Uct_Books)dtg_BooksDelete.SelectedItem;
        }

        private void btn_DeleteBook_Click2(object sender, RoutedEventArgs e)
        {
            if (selectedbookdelete == null)
                return;
            BookDeletePopup bookdeletepopup = new BookDeletePopup(selectedbookdelete);
            bookdeletepopup.Show();
            selectedbookdelete = null;
        }
    }
}
