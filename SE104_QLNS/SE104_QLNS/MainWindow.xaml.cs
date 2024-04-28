using SE104_QLNS.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SE104_QLNS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Uct_Books> Books { get; set; } = new ObservableCollection<Uct_Books>();
        public ObservableCollection<Uct_Customer> Customers { get; set; } = new ObservableCollection<Uct_Customer>();

        Connection connect = new Connection();
        

        Uct_Books selectedbook = null;
        bool isDelete = false;
        bool isUpdate = false;
        bool isList = true;
        bool isCustomerUpdate = false;
        bool isCustomerDelete = false;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            // LOAD BOOK CODE
            LoadBook(this, 0);
            // LOAD CUSTOMER  CODE
            LoadCustomer(this, 0);
        }
        public string GetNextBookTitleID(MainWindow mainwindow)
        {
            string connectionString = connect.connection;
            string nextBookID = "DS000001"; // Initial value

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Use a transaction to ensure data consistency
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        string sqlQuery = "SELECT MAX(MaDauSach) AS LastMaDauSach FROM DAUSACH";
                        SqlCommand command = new SqlCommand(sqlQuery, connection, transaction);

                        object LastMaDauSach = command.ExecuteScalar();

                        if (LastMaDauSach != DBNull.Value)
                        {
                            int currentID = Convert.ToInt32(LastMaDauSach.ToString().Substring(2)); // Extract numeric part
                            currentID++; // Increment for next ID
                            nextBookID = "DS" + currentID.ToString("D6"); // Format with leading zeros
                        }

                        transaction.Commit(); // Commit the transaction if successful
                    }
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error generating BookTitle ID: " + ex.Message);
                    nextBookID = null; // Indicate error
                }
            }

            return nextBookID;
        }
        public string GetNextBookID(MainWindow mainwindow)
        {
            string connectionString = connect.connection;
            string nextBookID = "SA000001"; // Initial value

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Use a transaction to ensure data consistency
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        string sqlQuery = "SELECT MAX(MaSach) AS LastMaSach FROM SACH";
                        SqlCommand command = new SqlCommand(sqlQuery, connection, transaction);

                        object lastMaSach = command.ExecuteScalar();

                        if (lastMaSach != DBNull.Value)
                        {
                            int currentID = Convert.ToInt32(lastMaSach.ToString().Substring(2)); // Extract numeric part
                            currentID++; // Increment for next ID
                            nextBookID = "SA" + currentID.ToString("D6"); // Format with leading zeros
                        }

                        transaction.Commit(); // Commit the transaction if successful
                    }
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error generating Book ID: " + ex.Message);
                    nextBookID = null; // Indicate error
                }
            }

            return nextBookID;
        }
        public void LoadBook(MainWindow mainwindow, int state)
        {
            //connect to database
            mainwindow.Books.Clear();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
               
                string MaSach = "", TacGia="", TheLoai="", TenDauSach = "", NXB = "", NamXB = "",
                HinhAnhSach = "", SoLuongTon = "", DonGiaNhap = "", DonGiaBan = "";

                try
                {
                    connection.Open();
                    string sqlQuery = "SELECT DISTINCT SACH.*, DAUSACH.*, TACGIA.*, THELOAI.* FROM SACH " +
                        "LEFT JOIN DAUSACH ON SACH.MADAUSACH = DAUSACH.MADAUSACH " +
                        "LEFT JOIN CT_TACGIA ON CT_TACGIA.MADAUSACH = DAUSACH.MADAUSACH " +
                        "LEFT JOIN TACGIA ON CT_TACGIA.MATACGIA = TACGIA.MATACGIA " +
                        "LEFT JOIN THELOAI ON THELOAI.MATHELOAI = DAUSACH.MATHELOAI ";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    int order = 1;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MaSach = reader["MaSach"].ToString();
                            TenDauSach = reader["TenDauSach"].ToString();
                            TacGia = reader["TenTacGia"].ToString();
                            TheLoai = reader["TenTheLoai"].ToString();
                            NXB = reader["NXB"].ToString();
                            NamXB = reader["NamXB"].ToString();
                            HinhAnhSach = reader["HinhAnhSach"].ToString();
                            SoLuongTon = reader["SoLuongTon"].ToString(); // Assuming numeric data type
                            DonGiaNhap = reader["DonGiaNhap"].ToString().Replace(",0000", "");
                            DonGiaBan = reader["DonGiaBan"].ToString().Replace(",0000", ""); // Assuming date/time data type
                            
                            Uct_Books book = new Uct_Books(0, this);
                            book.BookSetState(state);
                            book.LoadData(MaSach, TenDauSach, TacGia, NXB, NamXB, TheLoai, HinhAnhSach, order.ToString(), DonGiaNhap, DonGiaBan, SoLuongTon);
                            mainwindow.Books.Add(book);
                            order++;
                        }
                    }
                    mainwindow.dtg_Books.Items.Refresh();
                    mainwindow.dtg_ImportBooks.Items.Refresh();
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error retrieving data: " + ex.Message);
                }
                mainwindow.wpn_Books.Children.Clear();
                foreach (Uct_Books child in Books)
                {
                    mainwindow.wpn_Books.Children.Add(child);
                }
            }
        }
        public string GetNextCustomerID(MainWindow mainwindow)
        {
            string connectionString = connect.connection;
            string nextCustomerID = "KH000001"; // Initial value

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Use a transaction to ensure data consistency
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        // Get the maximum existing MaKH value
                        string sqlQuery = "SELECT MAX(MaKH) AS LastMaKH FROM KHACHHANG";
                        SqlCommand command = new SqlCommand(sqlQuery, connection, transaction);

                        object lastMaKH = command.ExecuteScalar();

                        // Check if any MaKH exists in the table
                        if (lastMaKH != DBNull.Value)
                        {
                            int currentID = Convert.ToInt32(lastMaKH.ToString().Substring(2)); // Extract numeric part
                            currentID++; // Increment for next ID
                            nextCustomerID = "KH" + currentID.ToString("D6"); // Format with leading zeros
                        }

                        transaction.Commit(); // Commit the transaction if successful
                    }
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error generating customer ID: " + ex.Message);
                    nextCustomerID = null; // Indicate error
                }
            }

            return nextCustomerID;
        }
        public void LoadCustomer(MainWindow mainwindow, int state)
        {
            Customers = new ObservableCollection<Uct_Customer>();

            //connect to database
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                mainwindow.Customers.Clear();
                string MaKH = "", HoTenKH = "", SDT = "", DiaChi = "",
                Email = "", SoTienNo = "", GioiTinh = "", NgaySinh = "", SoTienMua = "";

                try
                {
                    connection.Open();
                    string sqlQuery = "SELECT * FROM KHACHHANG";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MaKH = reader["MaKH"].ToString();
                            HoTenKH = reader["HoTenKH"].ToString();
                            SDT = reader["SDT"].ToString();
                            DiaChi = reader["DiaChi"].ToString();
                            Email = reader["Email"].ToString();
                            SoTienNo = reader["SoTienNo"].ToString(); // Assuming numeric data type
                            GioiTinh = reader["GioiTinh"].ToString();
                            NgaySinh = reader["NgaySinh"].ToString(); // Assuming date/time data type
                            SoTienMua = reader["SoTienMua"].ToString(); // Assuming numeric data type

                            string gender;
                            if (GioiTinh == "1")
                                gender = "Nam";
                            else
                                gender = "Nữ";
                            Uct_Customer customer = new Uct_Customer(this);
                            customer.CustomerSetState(state);
                            customer.LoadData(MaKH, HoTenKH, NgaySinh, gender, SDT, DiaChi, Email, SoTienMua, SoTienNo);
                            mainwindow.Customers.Add(customer);
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error","Error retrieving data: " + ex.Message);
                }
                wpn_Customer.Children.Clear();
                foreach (Uct_Customer child in Customers)
                {
                    wpn_Customer.Children.Add(child);
                }
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
            wpn_ImportPaper.Children.Clear();
            if (isList) //ListView
            {
                if (cvs_ImportBooks.Visibility == Visibility.Hidden) //Swap from Default to Add
                {
                    //set other hidden
                    cvs_BooksGridList.Visibility = Visibility.Hidden;
                    cvs_BooksDataGridList.Visibility = Visibility.Hidden;
                    cvs_ImportBooks_Grid.Visibility = Visibility.Hidden;

                    //set this visible
                    cvs_ImportBooks.Visibility = Visibility.Visible;
                    cvs_ImportBooks_List.Visibility = Visibility.Visible;

                    dtg_ImportBooks.Items.Refresh();

                    btn_AddBook.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                    btn_DeleteBook.Background = new SolidColorBrush(Colors.Transparent);
                    btn_UpdateBook.Background = new SolidColorBrush(Colors.Transparent);

                    dtg_Books.Columns[0].Visibility = Visibility.Hidden;
                    dtg_Books.Columns[1].Visibility = Visibility.Hidden;
                }
                else //Swap from Add to Default
                {
                    cvs_BooksDataGridList.Visibility = Visibility.Visible;

                    cvs_ImportBooks.Visibility = Visibility.Hidden;
                    cvs_ImportBooks_List.Visibility = Visibility.Hidden;

                    dtg_Books.Items.Refresh();
                    btn_AddBook.Background = new SolidColorBrush(Colors.Transparent);

                }
            }
            else //TableView
            {
                if (cvs_ImportBooks.Visibility == Visibility.Hidden) //Swap from Default to Add
                {
                    //set other hidden
                    cvs_BooksGridList.Visibility = Visibility.Hidden;
                    cvs_BooksDataGridList.Visibility = Visibility.Hidden;
                    cvs_ImportBooks_List.Visibility = Visibility.Hidden;

                    //set this visible
                    cvs_ImportBooks.Visibility = Visibility.Visible;
                    cvs_ImportBooks_Grid.Visibility = Visibility.Visible;

                    dtg_ImportBooks.Items.Refresh();

                    this.LoadBook(this, 3);
                    wpn_Books.Children.Clear();
                    wpn_ImportBooks.Children.Clear();
                    foreach (Uct_Books child in Books)
                    {
                        wpn_ImportBooks.Children.Add(child);
                    }
                    btn_AddBook.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                    btn_DeleteBook.Background = new SolidColorBrush(Colors.Transparent);
                    btn_UpdateBook.Background = new SolidColorBrush(Colors.Transparent);

                    dtg_Books.Columns[0].Visibility = Visibility.Hidden;
                    dtg_Books.Columns[1].Visibility = Visibility.Hidden;
                }
                else //Swap from Add to Default
                {
                    cvs_BooksGridList.Visibility = Visibility.Visible;

                    cvs_ImportBooks.Visibility = Visibility.Hidden;
                    cvs_ImportBooks_Grid.Visibility = Visibility.Hidden;
                    this.LoadBook(this, 0);
                    dtg_Books.Items.Refresh();
                    btn_AddBook.Background = new SolidColorBrush(Colors.Transparent);

                }
            }
        }
        private void btn_AddBook_Click_2(object sender, RoutedEventArgs e)
        {
            if (selectedbook == null)
                return;
            bool isDuplicate = false;

            foreach (Uct_BookImport child in wpn_ImportPaper.Children.OfType<Uct_BookImport>())
            {
                if (selectedbook.BookID == child.BookID)
                {
                    isDuplicate = true;
                    break;  // Exit the loop after finding the first duplicate
                }
            }

            if (!isDuplicate)
            {
                Uct_BookImport bookimport = new Uct_BookImport();
                bookimport.BookID = selectedbook.BookID;
                bookimport.BookName = selectedbook.BookName;
                bookimport.BookImportPrice = selectedbook.BookPriceImport;
                bookimport.BookQuantity = "1";
                bookimport.BookURL = selectedbook.BookURL;
                this.wpn_ImportPaper.Children.Add(bookimport);
            }
        }
        private void btn_SwitchView_Click(object sender, RoutedEventArgs e) //Switch between List and Table
        {
            if (cvs_ImportBooks.Visibility == Visibility.Hidden)
            {
                if (isList) //Swap From List (Default) to Table
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

                    int state;
                    if (!isDelete && !isUpdate)
                        state = 0;
                    else if (isDelete)
                        state = 1;
                    else if (isUpdate)
                        state = 2;
                    else
                        state = 0;
                    LoadBook(this, state);
                    isList = false;
                }
                else //Swap From Table To List (Default)
                {
                    cvs_BooksDataGridList.Visibility = Visibility.Visible;

                    cvs_BooksGridList.Visibility = Visibility.Hidden;
                    cvs_ImportBooks.Visibility = Visibility.Hidden;

                    dtg_Books.Items.Refresh();
                    btn_SwitchView.Background = new SolidColorBrush(Colors.Transparent);

                    int state;
                    if (!isDelete && !isUpdate)
                        state = 0;
                    else if (isDelete)
                        state = 1;
                    else if (isUpdate)
                        state = 2;
                    else
                        state = 0;
                    LoadBook(this, state);
                    
                    isList = true;
                }
            }
        }
        private void dtg_Books_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedbook = (Uct_Books)dtg_Books.SelectedItem;
        }
        private void btn_DeleteBook_Click(object sender, RoutedEventArgs e) //Click DeleteBook
        {
            wpn_Books.Children.Clear();
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
            }
            else if (cvs_BooksGridList.Visibility == Visibility.Visible)//TableView
            {
                if (!isDelete)
                {
                    LoadBook(this, 1);
                    btn_DeleteBook.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                    btn_AddBook.Background = new SolidColorBrush(Colors.Transparent);
                    btn_UpdateBook.Background = new SolidColorBrush(Colors.Transparent);
                    isDelete = true;
                    isUpdate = false;
                }
                else
                {
                    LoadBook(this, 0);
                    dtg_Books.Columns[0].Visibility = Visibility.Hidden;
                    btn_DeleteBook.Background = new SolidColorBrush(Colors.Transparent);
                    isDelete = false;
                }
            }
        }
        private void btn_DeleteBook_Click2(object sender, RoutedEventArgs e) //DeleteBook for each Book
        {
            if (selectedbook == null)
                return;
            BookDeletePopup bookdeletepopup = new BookDeletePopup(selectedbook, this);
            bookdeletepopup.Show();
            wpn_Books.Children.Clear();
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
            else if (cvs_BooksGridList.Visibility == Visibility.Visible)//TableView
            {
                if (!isUpdate)
                {
                    LoadBook(this, 2);
                    btn_UpdateBook.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                    btn_DeleteBook.Background = new SolidColorBrush(Colors.Transparent);
                    btn_AddBook.Background = new SolidColorBrush(Colors.Transparent);
                    isDelete = false;
                    isUpdate = true;
                }
                else
                {
                    LoadBook(this, 0);
                    btn_UpdateBook.Background = new SolidColorBrush(Colors.Transparent);
                    isUpdate = false;
                }
            }
        }
        private void btn_UpdateBook_Click2(object sender, RoutedEventArgs e)
        {
            if (selectedbook == null)
                return;
            BookUpdatePopup bookupdatepopup = new BookUpdatePopup(selectedbook, this);
            bookupdatepopup.Show();
        }
        private void btn_AddNewBook_Click(object sender, RoutedEventArgs e)
        {
            BookAddPopup bookaddpopup = new BookAddPopup(this);
            bookaddpopup.Show();
        }
        private void dtg_ImportBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedbook = (Uct_Books)dtg_ImportBooks.SelectedItem;
        }
        
        private void btn_CustomerAdd_Click(object sender, RoutedEventArgs e)
        {
            btn_CustomerUpdate.Background = new SolidColorBrush(Colors.Transparent);
            isCustomerUpdate = false;
            isCustomerDelete = false;
            CustomerAdd customer = new CustomerAdd(this);
            customer.Show();
        }
        private void btn_CustomerUpdate_Click(object sender, RoutedEventArgs e)
        {
            isCustomerDelete = false;
            if (!isCustomerUpdate) //Switch to Update
            {
                wpn_Customer.Children.Clear();
                foreach (Uct_Customer child in Customers)
                {
                    child.CustomerSetState(1);
                    wpn_Customer.Children.Add(child);
                }
                btn_CustomerUpdate.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                btn_CustomerDelete.Background = new SolidColorBrush(Colors.Transparent);
                btn_CustomerAdd.Background = new SolidColorBrush(Colors.Transparent);
                isCustomerUpdate = true;
            }
            else //Turn off update
            {
                isCustomerUpdate = false;
                btn_CustomerUpdate.Background = new SolidColorBrush(Colors.Transparent);
                wpn_Customer.Children.Clear();
                foreach (Uct_Customer child in Customers)
                {
                    child.CustomerSetState(0);
                   wpn_Customer.Children.Add(child);
                }
            }
        }

        private void btn_CustomerDelete_Click(object sender, RoutedEventArgs e)
        {
            isCustomerUpdate = false;
            if(!isCustomerDelete) //Swap to Delete
            {
                wpn_Customer.Children.Clear();
                foreach (Uct_Customer child in Customers)
                {
                    child.CustomerSetState(2);
                    wpn_Customer.Children.Add(child);
                }
                btn_CustomerDelete.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                btn_CustomerUpdate.Background = new SolidColorBrush(Colors.Transparent);
                btn_CustomerAdd.Background = new SolidColorBrush(Colors.Transparent);
                isCustomerDelete = true;
            }
            else //Turn off delete
            {
                isCustomerDelete = false;
                btn_CustomerDelete.Background = new SolidColorBrush(Colors.Transparent);
                wpn_Customer.Children.Clear();
                foreach (Uct_Customer child in Customers)
                {
                    child.CustomerSetState(0);
                    wpn_Customer.Children.Add(child);
                }
            }
        }

        private void cvs_BooksDataGridList_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!isUpdate && !isDelete)
            {
                BookInfoPopup bookInfoPopup = new BookInfoPopup(selectedbook);
                bookInfoPopup.Show();
            }
        }
    }
}

