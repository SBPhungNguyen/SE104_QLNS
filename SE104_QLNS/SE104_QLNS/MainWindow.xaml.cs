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
        bool isDelete = false;
        bool isUpdate = false;
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
                cvs_BooksDataGridList.Visibility = Visibility.Hidden;

                //set this visible
                cvs_ImportBooks.Visibility = Visibility.Visible;

                dtg_ImportBooks.Items.Refresh();

                btn_AddBook.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                btn_SwitchView.Background = new SolidColorBrush(Colors.Transparent);
                btn_DeleteBook.Background = new SolidColorBrush(Colors.Transparent);
                btn_UpdateBook.Background = new SolidColorBrush(Colors.Transparent);

                dtg_Books.Columns[0].Visibility = Visibility.Hidden;
                dtg_Books.Columns[1].Visibility = Visibility.Hidden;
            }
            else //Swap from Add to Default
            {
                cvs_BooksDataGridList.Visibility=Visibility.Visible;
                cvs_ImportBooks.Visibility = Visibility.Hidden;
                dtg_Books.Items.Refresh(); 
                btn_AddBook.Background = new SolidColorBrush(Colors.Transparent);
            }
        }
        private void btn_SwitchView_Click(object sender, RoutedEventArgs e) //Switch between List and Table
        {
            if (cvs_BooksDataGridList.Visibility == Visibility.Visible) //Swap From List (Default) to Table
            {
                //set other hidden
                cvs_BooksDataGridList.Visibility = Visibility.Hidden;
                cvs_ImportBooks.Visibility = Visibility.Hidden;

                //set this visible
                cvs_BooksGridList.Visibility = Visibility.Visible;

                //button color
                btn_SwitchView.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                btn_DeleteBook.Background = new SolidColorBrush(Colors.Transparent);
                btn_AddBook.Background = new SolidColorBrush(Colors.Transparent);

                WrapPanel bookPanel = (WrapPanel)this.FindName("wpn_Books");
                bookPanel.Children.Clear();
                int state;
                if (!isDelete && !isUpdate)
                {
                    state = 0;
                }
                else if (isDelete)
                {
                    state = 1;
                }
                else if (isUpdate)
                {
                    state = 2;
                }
                else
                {
                    state = 0;
                }
                    foreach (Uct_Books bookAdd in Books)
                {
                    Uct_Books placeholder = new Uct_Books(state, this);
                    placeholder.BookID = bookAdd.BookID;
                    placeholder.BookURL = bookAdd.BookURL;
                    placeholder.BookName = bookAdd.BookName;
                    placeholder.BookPriceImport = bookAdd.BookPriceImport;
                    placeholder.Amount = bookAdd.Amount;
                    switch(state)
                    {
                        case 0:
                            placeholder.BookStateURL = "/Images/icon_info.png";
                            break;
                        case 1:
                            placeholder.BookStateURL = "/Images/icon_bin.png";
                            break;
                        case 2:
                            placeholder.BookStateURL = "/Images/icon_pencil.png";
                            break;
                    }

                    Uct_Books bookImport = placeholder;
                    bookPanel.Children.Add(bookImport);
                }

            }
            else //Swap From Table To List (Default)
            {
                cvs_BooksDataGridList.Visibility = Visibility.Visible;

                cvs_BooksGridList.Visibility = Visibility.Hidden;
                cvs_ImportBooks.Visibility = Visibility.Hidden;

                dtg_Books.Items.Refresh();
                btn_SwitchView.Background = new SolidColorBrush(Colors.Transparent);

                WrapPanel bookPanel = (WrapPanel)this.FindName("wpn_Books");
                bookPanel.Children.Clear();
            }
            isUpdate = false;

            isDelete = false;
            dtg_Books.Columns[0].Visibility = Visibility.Hidden;
        }

        private void dtg_Books_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isUpdate == true)
            {
                selectedbook = (Uct_Books)dtg_Books.SelectedItem;
            }
            else if (isDelete == true)
            {
                selectedbook = (Uct_Books)dtg_Books.SelectedItem;
            }
            else
            {
                selectedbook = (Uct_Books)dtg_Books.SelectedItem;
                BookInfoPopup bookInfoPopup = new BookInfoPopup(selectedbook);
                bookInfoPopup.Show();
            }
        }
        private void btn_DeleteBook_Click(object sender, RoutedEventArgs e) //Click DeleteBook
        {
            WrapPanel bookPanel = (WrapPanel)this.FindName("wpn_Books");
            bookPanel.Children.Clear();
            if (cvs_ImportBooks.Visibility == Visibility.Visible)
                return;
            if (cvs_BooksDataGridList.Visibility == Visibility.Visible) //List View
            {
                if (!isDelete) //Disabled to Enabled
                {
                    dtg_Books.Columns[0].Visibility = Visibility.Visible;
                    dtg_Books.Columns[1].Visibility = Visibility.Hidden;
                    btn_DeleteBook.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                    btn_AddBook.Background = new SolidColorBrush(Colors.Transparent);
                    btn_UpdateBook.Background = new SolidColorBrush(Colors.Transparent);
                    isDelete = true;
                    isUpdate = false;
                }
                else //Enabled to Disabled
                {
                    dtg_Books.Columns[0].Visibility = Visibility.Hidden;
                    btn_DeleteBook.Background = new SolidColorBrush(Colors.Transparent);
                    isDelete = false;
                }
                btn_AddBook.Background = new SolidColorBrush(Colors.Transparent);
                btn_UpdateBook.Background = new SolidColorBrush(Colors.Transparent);
                isDelete = true;
                isUpdate = false;
            }
            else //TableView
            {
               
                foreach (Uct_Books bookAdd in Books)
                {
                    Uct_Books placeholder = new Uct_Books(1, this);
                    placeholder.BookID = bookAdd.BookID;
                    placeholder.BookURL = bookAdd.BookURL;
                    placeholder.BookName = bookAdd.BookName;
                    placeholder.BookPriceImport = bookAdd.BookPriceImport;
                    placeholder.Amount = bookAdd.Amount;
                    placeholder.BookStateURL = "/Images/icon_bin.png";
                    Uct_Books bookImport = placeholder;
                    bookPanel.Children.Add(bookImport);
                }
            }
        }

        private void btn_DeleteBook_Click2(object sender, RoutedEventArgs e) //DeleteBook for each Book
        {
            if (selectedbook == null)
                return;
            BookDeletePopup bookdeletepopup = new BookDeletePopup(selectedbook, this);
            bookdeletepopup.Show(); 
            WrapPanel bookPanel = (WrapPanel)this.FindName("wpn_Books");
            bookPanel.Children.Clear();
        }

        private void btn_UpdateBook_Click(object sender, RoutedEventArgs e) //Click UpdateBook
        {
            if (cvs_ImportBooks.Visibility == Visibility.Visible)
                return;
            if (cvs_BooksDataGridList.Visibility == Visibility.Visible) //List View
            {
                if (!isUpdate) //Disabled to Enabled
                {
                    dtg_Books.Columns[1].Visibility = Visibility.Visible;
                    dtg_Books.Columns[0].Visibility = Visibility.Hidden;
                    btn_UpdateBook.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                    btn_DeleteBook.Background = new SolidColorBrush(Colors.Transparent);
                    btn_AddBook.Background = new SolidColorBrush(Colors.Transparent);

                    isDelete = false;
                    isUpdate = true;
                }
                else //Enabled to Disabled
                {
                    dtg_Books.Columns[1].Visibility = Visibility.Hidden;
                    btn_UpdateBook.Background = new SolidColorBrush(Colors.Transparent);
                    isUpdate = false;
                }

            }
            else //TableView
            {
                WrapPanel bookPanel = (WrapPanel)this.FindName("wpn_Books");
                bookPanel.Children.Clear();
                foreach (Uct_Books bookAdd in Books)
                {
                    Uct_Books placeholder = new Uct_Books(2, this);
                    placeholder.BookID = bookAdd.BookID;
                    placeholder.BookURL = bookAdd.BookURL;
                    placeholder.BookName = bookAdd.BookName;
                    placeholder.BookPriceImport = bookAdd.BookPriceImport;
                    placeholder.Amount = bookAdd.Amount;
                    placeholder.BookStateURL = "/Images/icon_pencil.png";
                    Uct_Books bookImport = placeholder;
                    bookPanel.Children.Add(bookImport);
                }
                btn_UpdateBook.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                btn_DeleteBook.Background = new SolidColorBrush(Colors.Transparent);
                btn_AddBook.Background = new SolidColorBrush(Colors.Transparent);
                isDelete = false;
                isUpdate = true;
            }
        }

        private void btn_UpdateBook_Click2(object sender, RoutedEventArgs e)
        {
            if (selectedbook == null)
                return;
            BookUpdatePopup bookupdatepopup = new BookUpdatePopup(selectedbook, this);
            bookupdatepopup.Show();
            selectedbook = null;
        }

        private void btn_AddNewBook_Click(object sender, RoutedEventArgs e)
        {
            BookAddPopup bookaddpopup = new BookAddPopup();
            bookaddpopup.Show();
        }
    }
    }

