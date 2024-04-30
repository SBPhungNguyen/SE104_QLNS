using SE104_QLNS;
using SE104_QLNS.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

        public ObservableCollection<Uct_Employee> Employees { get; set; } = new ObservableCollection<Uct_Employee>();
        public ObservableCollection<Uct_Author> Authors { get; set; } = new ObservableCollection<Uct_Author>();

        public ObservableCollection<ImportedBookReceiptInfo> ImportBookReceipts { get; set; } = new ObservableCollection<ImportedBookReceiptInfo>();
        Connection connect = new Connection();


        Uct_Books selectedbook = null;
        ImportedBookReceiptInfo selectedimportreceipt = null;
        bool isBookDelete = false;
        bool isBookUpdate = false;
        bool isBookList = true;

        bool isImportPaperDelete = false;

        bool isCustomerUpdate = false;
        bool isCustomerDelete = false;

        bool isEmployeeUpdate = false;
        bool isEmployeeDelete = false;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            LoadBook(this, 0);
            LoadCustomer(this, 0);
            LoadEmployee(this, 0);
            LoadImportPaper(this, 0);
            LoadAuthor(this, 0);

        }

        //Miscellaneous
        private void btn_ExitApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }
        private void btn_MinimizeApp_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        //Get ID
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
        public string GetNextImportPaperID(MainWindow mainwindow)
        {
            string connectionString = connect.connection;
            string nextImportPaperID = "PN000001"; // Initial value

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Use a transaction to ensure data consistency
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        string sqlQuery = "SELECT MAX(MaPhieuNhap) AS LastMaPhieuNhap FROM PHIEUNHAP";
                        SqlCommand command = new SqlCommand(sqlQuery, connection, transaction);

                        object LastMaPhieuNhap = command.ExecuteScalar();

                        if (LastMaPhieuNhap != DBNull.Value)
                        {
                            int currentID = Convert.ToInt32(LastMaPhieuNhap.ToString().Substring(2)); // Extract numeric part
                            currentID++; // Increment for next ID
                            nextImportPaperID = "PN" + currentID.ToString("D6"); // Format with leading zeros
                        }

                        transaction.Commit(); // Commit the transaction if successful
                    }
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error generating customer ID: " + ex.Message);
                    nextImportPaperID = null; // Indicate error
                }
            }
            return nextImportPaperID;
        }
        public string GetNextEmployeeID(MainWindow mainwindow)
        {
            string connectionString = connect.connection;
            string nextEmployeeID = "NV000001"; // Initial value

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Use a transaction to ensure data consistency
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        // Get the maximum existing MaKH value
                        string sqlQuery = "SELECT MAX(MaNV) AS LastMaNV FROM NGUOIDUNG";
                        SqlCommand command = new SqlCommand(sqlQuery, connection, transaction);

                        object lastMaNV = command.ExecuteScalar();

                        // Check if any MaNV exists in the table
                        if (lastMaNV != DBNull.Value)
                        {
                            int currentID = Convert.ToInt32(lastMaNV.ToString().Substring(2)); // Extract numeric part
                            currentID++; // Increment for next ID
                            nextEmployeeID = "NV" + currentID.ToString("D6"); // Format with leading zeros
                        }

                        transaction.Commit(); // Commit the transaction if successful
                    }
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error generating employee ID: " + ex.Message);
                    nextEmployeeID = null; // Indicate error
                }
            }

            return nextEmployeeID;
        }

        //Load
        public void LoadBook(MainWindow mainwindow, int state)
        {
            //connect to database
            mainwindow.Books.Clear();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string MaSach = "", TacGia = "", TheLoai = "", TenDauSach = "", NXB = "", NamXB = "",
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
                mainwindow.wpn_ImportBooks.Children.Clear();
                if (state == 3)
                {
                    foreach (Uct_Books child in Books)
                    {
                        wpn_ImportBooks.Children.Add(child);
                    }
                }
                else
                {
                    foreach (Uct_Books child in Books)
                    {
                        mainwindow.wpn_Books.Children.Add(child);
                    }
                }
            }
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
                    Notification noti = new Notification("Error", "Error retrieving data: " + ex.Message);
                }
                wpn_Customer.Children.Clear();
                foreach (Uct_Customer child in Customers)
                {
                    wpn_Customer.Children.Add(child);
                }
            }
        }
        public void LoadImportPaper(MainWindow mainwindow, int state)
        {
            mainwindow.ImportBookReceipts.Clear();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string MaPhieuNhap = "", NgayNhap = "", TongTien = "";

                try
                {
                    connection.Open();
                    string sqlQuery = "SELECT DISTINCT * FROM PHIEUNHAP";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    int order = 1;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MaPhieuNhap = reader["MaPhieuNhap"].ToString();
                            NgayNhap = reader["NgayNhap"].ToString();
                            TongTien = reader["TongTien"].ToString().Replace(",0000", "");

                            ImportedBookReceiptInfo receipt = new ImportedBookReceiptInfo(this, state);
                            receipt.SetState(state);
                            receipt.LoadData(order.ToString(), MaPhieuNhap, NgayNhap, TongTien);
                            mainwindow.ImportBookReceipts.Add(receipt);
                            order++;
                        }
                    }
                    mainwindow.dtg_ImportBookReceiptList.Items.Refresh();
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error retrieving data: " + ex.Message);
                }
            }
        }
        public void LoadEmployee(MainWindow mainwindow, int state)
        {
            Employees = new ObservableCollection<Uct_Employee>();

            //connect to database
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                mainwindow.Customers.Clear();
                string MaNV = "", HoTenNV = "", SDT = "", Email = "", DiaChi = "",
                    GioiTinh = "", NgaySinh = "", CCCD = "", ViTri = "", Ca = "", TenTK = "", MatKhau = "";

                try
                {
                    connection.Open();
                    string sqlQuery = "SELECT * FROM NGUOIDUNG";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MaNV = reader["MaNV"].ToString();
                            HoTenNV = reader["HoTenNV"].ToString();
                            SDT = reader["SDT"].ToString();
                            Email = reader["Email"].ToString();
                            DiaChi = reader["DiaChi"].ToString();
                            GioiTinh = reader["GioiTinh"].ToString();
                            NgaySinh = reader["NgaySinh"].ToString(); // Assuming date/time data type
                            CCCD = reader["CCCD"].ToString();
                            ViTri = reader["ViTri"].ToString();
                            Ca = reader["Ca"].ToString();
                            TenTK = reader["TenTK"].ToString();
                            MatKhau = reader["MatKhau"].ToString();
                            string gender;
                            if (GioiTinh == "1")
                                gender = "Nam";
                            else
                                gender = "Nữ";
                            Uct_Employee employee = new Uct_Employee(this);
                            employee.EmployeeSetState(state);
                            employee.LoadData(MaNV, HoTenNV, NgaySinh, gender, CCCD, SDT, DiaChi, ViTri, Ca, TenTK, MatKhau);
                            mainwindow.Employees.Add(employee);
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error retrieving data: " + ex.Message);
                }
                wpn_Employee.Children.Clear();
                foreach (Uct_Employee child in Employees)
                {
                    wpn_Customer.Children.Add(child);
                }
            }
        }

        //Import Tab
        private void dtg_ImportBookReceiptList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedimportreceipt = (ImportedBookReceiptInfo)dtg_ImportBookReceiptList.SelectedItem;
        }
        private void ViewImportBook_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((selectedimportreceipt!=null)&&(!isImportPaperDelete))
            {
                selectedimportreceipt.Show();
            }
        }

        private void btn_ImportBookReceiptDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!isImportPaperDelete) //Switch from normal to delete
            {
                dtg_ImportBookReceiptList.Columns[0].Visibility = Visibility.Visible;
                btn_ImportBookReceiptDelete.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                isImportPaperDelete = true;
                LoadImportPaper(this, 1);
            }
            else
            {
                dtg_ImportBookReceiptList.Columns[0].Visibility = Visibility.Hidden;
                isImportPaperDelete = false;
                btn_ImportBookReceiptDelete.Background = new SolidColorBrush(Colors.Transparent);
                LoadImportPaper(this, 0);
            }
        }

        private void btn_DeleteImportBook_Click(object sender, RoutedEventArgs e)
        {
            selectedimportreceipt.Show();
        }
        //Books
        private void dtg_Books_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedbook = (Uct_Books)dtg_Books.SelectedItem;
        }
            //Add Book To ImportPanel
        private void btn_AddBook_Click(object sender, RoutedEventArgs e) // Click on Them/Add Button
        {
            wpn_ImportPaper.Children.Clear();
            if (isBookList) //ListView
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
                Uct_BookImport bookimport = new Uct_BookImport(this);
                bookimport.BookID = selectedbook.BookID;
                bookimport.BookName = selectedbook.BookName;
                bookimport.BookImportPrice = selectedbook.BookPriceImport;
                bookimport.BookQuantity = "1";
                bookimport.BookURL = selectedbook.BookURL;
                this.wpn_ImportPaper.Children.Add(bookimport);
            }
        }
            //Switch View
        private void btn_SwitchView_Click(object sender, RoutedEventArgs e) //Switch between List and Table
        {
            if (cvs_ImportBooks.Visibility == Visibility.Hidden)
            {
                if (isBookList) //Swap From List (Default) to Table
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
                    if (!isBookDelete && !isBookUpdate)
                        state = 0;
                    else if (isBookDelete)
                        state = 1;
                    else if (isBookUpdate)
                        state = 2;
                    else
                        state = 0;
                    LoadBook(this, state);
                    isBookList = false;
                }
                else //Swap From Table To List (Default)
                {
                    cvs_BooksDataGridList.Visibility = Visibility.Visible;

                    cvs_BooksGridList.Visibility = Visibility.Hidden;
                    cvs_ImportBooks.Visibility = Visibility.Hidden;

                    dtg_Books.Items.Refresh();
                    btn_SwitchView.Background = new SolidColorBrush(Colors.Transparent);

                    int state;
                    if (!isBookDelete && !isBookUpdate)
                        state = 0;
                    else if (isBookDelete)
                        state = 1;
                    else if (isBookUpdate)
                        state = 2;
                    else
                        state = 0;
                    LoadBook(this, state);

                    isBookList = true;
                }
            }
        }
            //Delete Book
        private void btn_DeleteBook_Click(object sender, RoutedEventArgs e) //Click DeleteBook
        {
            wpn_Books.Children.Clear();
            if (cvs_ImportBooks.Visibility == Visibility.Visible)
                return;
            if (cvs_BooksDataGridList.Visibility == Visibility.Visible) //List View
            {
                if (!isBookDelete) //Disabled to Enabled
                {
                    dtg_Books.Columns[0].Visibility = Visibility.Visible;
                    dtg_Books.Columns[1].Visibility = Visibility.Hidden;
                    btn_DeleteBook.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                    btn_AddBook.Background = new SolidColorBrush(Colors.Transparent);
                    btn_UpdateBook.Background = new SolidColorBrush(Colors.Transparent);
                    isBookDelete = true;
                    isBookUpdate = false;
                }
                else //Enabled to Disabled
                {
                    dtg_Books.Columns[0].Visibility = Visibility.Hidden;
                    btn_DeleteBook.Background = new SolidColorBrush(Colors.Transparent);
                    isBookDelete = false;
                }
            }
            else if (cvs_BooksGridList.Visibility == Visibility.Visible)//TableView
            {
                if (!isBookDelete)
                {
                    LoadBook(this, 1);
                    btn_DeleteBook.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                    btn_AddBook.Background = new SolidColorBrush(Colors.Transparent);
                    btn_UpdateBook.Background = new SolidColorBrush(Colors.Transparent);
                    isBookDelete = true;
                    isBookUpdate = false;
                }
                else
                {
                    LoadBook(this, 0);
                    dtg_Books.Columns[0].Visibility = Visibility.Hidden;
                    btn_DeleteBook.Background = new SolidColorBrush(Colors.Transparent);
                    isBookDelete = false;
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
            //Update Book
        private void btn_UpdateBook_Click(object sender, RoutedEventArgs e) //Click UpdateBook
        {
            if (cvs_ImportBooks.Visibility == Visibility.Visible)
                return;
            if (cvs_BooksDataGridList.Visibility == Visibility.Visible) //List View
            {
                if (!isBookUpdate) //Disabled to Enabled
                {
                    dtg_Books.Columns[1].Visibility = Visibility.Visible;
                    dtg_Books.Columns[0].Visibility = Visibility.Hidden;
                    btn_UpdateBook.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                    btn_DeleteBook.Background = new SolidColorBrush(Colors.Transparent);
                    btn_AddBook.Background = new SolidColorBrush(Colors.Transparent);

                    isBookDelete = false;
                    isBookUpdate = true;
                }
                else //Enabled to Disabled
                {
                    dtg_Books.Columns[1].Visibility = Visibility.Hidden;
                    btn_UpdateBook.Background = new SolidColorBrush(Colors.Transparent);
                    isBookUpdate = false;
                }

            }
            else if (cvs_BooksGridList.Visibility == Visibility.Visible)//TableView
            {
                if (!isBookUpdate)
                {
                    LoadBook(this, 2);
                    btn_UpdateBook.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                    btn_DeleteBook.Background = new SolidColorBrush(Colors.Transparent);
                    btn_AddBook.Background = new SolidColorBrush(Colors.Transparent);
                    isBookDelete = false;
                    isBookUpdate = true;
                }
                else
                {
                    LoadBook(this, 0);
                    btn_UpdateBook.Background = new SolidColorBrush(Colors.Transparent);
                    isBookUpdate = false;
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
            //Create New Book
        private void btn_AddNewBook_Click(object sender, RoutedEventArgs e)
        {
            BookAddPopup bookaddpopup = new BookAddPopup(this);
            bookaddpopup.Show();
        }
            //Imports
        private void btn_ImportBooks_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                //Checking if the thing is null
                if (wpn_ImportPaper.Children.Count == 0)
                    return;
                //Get the Requirement amount of books
                int limit = 0;
                try
                {
                    string sqlQuery = "SELECT GiaTri FROM THAMSO WHERE TenThamSo='SoLuongNhapToiThieu'";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    limit = Convert.ToInt32(reader["GiaTri"]);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error retrieving data: " + ex.Message);
                }
                //Checking if the book fit the requirement
                limit = 0;                                       // TEMPORARY
                foreach (Uct_BookImport child in wpn_ImportPaper.Children.OfType<Uct_BookImport>())
                {
                    if (Convert.ToInt32(child.BookQuantity) < limit)
                    {
                        Notification noti = new Notification("Policy Violation", "Amount of Book Imported for " + child.BookID + " is less than limit: " + limit);
                        return;
                    }
                }
                //Actually Importing:
                foreach (Uct_BookImport child in wpn_ImportPaper.Children.OfType<Uct_BookImport>())
                {
                    try
                    {
                        //Get the amount of books
                        string sqlQuery = "SELECT SoLuongTon FROM SACH WHERE MaSach = @MaSach";
                        SqlCommand command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@MaSach", child.BookID);
                        SqlDataReader reader = command.ExecuteReader();
                        reader.Read();
                        int Amount = Convert.ToInt32(reader["SoLuongTon"]);
                        Amount += Convert.ToInt32(child.BookQuantity);
                        reader.Close();

                        //Update The Actual Book
                        sqlQuery = "UPDATE SACH SET SOLUONGTON=@SOLUONGTON WHERE MaSach = @MaSach";
                        command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@SOLUONGTON", Amount);
                        command.Parameters.AddWithValue("@MaSach", child.BookID);
                        reader = command.ExecuteReader();
                        reader.Read();
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        Notification noti = new Notification("Error", "Error: " + ex.Message);
                    }
                }
                //Creating Import Paper for all
                string nextImportPaperID = GetNextImportPaperID(this);
                try
                {
                    //Creating
                    string sqlQuery = "INSERT INTO PHIEUNHAP (MaPhieuNhap, NgayNhap, TongTien) " +
                  $"VALUES (@MaPhieuNhap, @NgayNhap, @TongTien)";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaPhieuNhap", nextImportPaperID);
                    command.Parameters.AddWithValue("@NgayNhap", DateTime.Now);
                    command.Parameters.AddWithValue("@TongTien", 0);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();

                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error: " + ex.Message);
                }
                //Creating ImportPaper for each one
                foreach (Uct_BookImport child in wpn_ImportPaper.Children.OfType<Uct_BookImport>())
                {
                    try
                    {
                        //Creating
                        string sqlQuery = "INSERT INTO CT_PHIEUNHAP (MaPhieuNhap, MaSach, SoLuongNhap, DonGiaNhap) " +
                      $"VALUES (@MaPhieuNhap, @MaSach, @SoLuongNhap, @DonGiaNhap)";
                        SqlCommand command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@MaPhieuNhap", nextImportPaperID);
                        command.Parameters.AddWithValue("@MaSach", child.BookID);
                        command.Parameters.AddWithValue("@SoLuongNhap", child.BookQuantity);
                        command.Parameters.AddWithValue("@DonGiaNhap", Convert.ToInt32(child.BookImportPrice.Replace(",0000", "")));
                        SqlDataReader reader = command.ExecuteReader();
                        reader.Read();
                        reader.Close();
                        //Adding to MoneySum


                        //Get the current SUM
                        sqlQuery = "SELECT TongTien FROM PHIEUNHAP WHERE MaPhieuNhap = @MaPhieuNhap";
                        command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@MaPhieuNhap", nextImportPaperID);
                        reader = command.ExecuteReader();
                        reader.Read();
                        int SUM = Convert.ToInt32(reader["TongTien"]);
                        SUM += Convert.ToInt32(child.BookQuantity) * Convert.ToInt32(child.BookImportPrice);
                        reader.Close();


                        //Update The SUM
                        sqlQuery = "UPDATE PHIEUNHAP SET TongTien=@TongTien WHERE MaPhieuNhap = @MaPhieuNhap";
                        command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@TongTien", SUM);
                        command.Parameters.AddWithValue("@MaPhieuNhap", nextImportPaperID);
                        reader = command.ExecuteReader();
                        reader.Read();
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        Notification noti = new Notification("Error", "Error: " + ex.Message);
                    }
                }
                wpn_ImportPaper.Children.Clear();
                LoadBook(this, 3);
                LoadImportPaper(this, 0);
            }
        }
        private void dtg_ImportBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedbook = (Uct_Books)dtg_ImportBooks.SelectedItem;
        }


        //Customer
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
            if (!isCustomerDelete) //Swap to Delete
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
            if (!isBookUpdate && !isBookDelete)
            {
                BookInfoPopup bookInfoPopup = new BookInfoPopup(selectedbook);
                bookInfoPopup.Show();
            }
        }
        private void btn_EmployeeAdd_Click(object sender, RoutedEventArgs e)
        {
            btn_EmployeeUpdate.Background = new SolidColorBrush(Colors.Transparent);
            isEmployeeUpdate = false;
            isEmployeeDelete = false;
            EmployeeAdd employee = new EmployeeAdd(this);
            employee.Show();
        }
        private void btn_EmployeeUpdate_Click(object sender, RoutedEventArgs e)
        {
            isEmployeeDelete = false;
            if (!isEmployeeUpdate) //Switch to Update
            {
                wpn_Employee.Children.Clear();
                foreach (Uct_Employee child in Employees)
                {
                    child.EmployeeSetState(1);
                    wpn_Employee.Children.Add(child);
                }
                btn_EmployeeUpdate.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                btn_EmployeeDelete.Background = new SolidColorBrush(Colors.Transparent);
                btn_EmployeeAdd.Background = new SolidColorBrush(Colors.Transparent);
                isEmployeeUpdate = true;
            }
            else //Turn off update
            {
                isEmployeeUpdate = false;
                btn_EmployeeUpdate.Background = new SolidColorBrush(Colors.Transparent);
                wpn_Employee.Children.Clear();
                foreach (Uct_Employee child in Employees)
                {
                    child.EmployeeSetState(0);
                    wpn_Employee.Children.Add(child);
                }
            }
        }
        private void btn_EmployeeDelete_Click(object sender, RoutedEventArgs e)
        {
            isEmployeeUpdate = false;
            if (!isEmployeeDelete) //Swap to Delete
            {
                wpn_Employee.Children.Clear();
                foreach (Uct_Employee child in Employees)
                {
                    child.EmployeeSetState(2);
                    wpn_Employee.Children.Add(child);
                }
                btn_EmployeeDelete.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                btn_EmployeeUpdate.Background = new SolidColorBrush(Colors.Transparent);
                btn_EmployeeAdd.Background = new SolidColorBrush(Colors.Transparent);
                isEmployeeDelete = true;
            }
            else //Turn off delete
            {
                isEmployeeDelete = false;
                btn_EmployeeDelete.Background = new SolidColorBrush(Colors.Transparent);
                wpn_Employee.Children.Clear();
                foreach (Uct_Employee child in Employees)
                {
                    child.EmployeeSetState(0);
                    wpn_Employee.Children.Add(child);
                }
            }
        }
        public string GetNextAuthorID(MainWindow mainwindow)
        {
            string connectionString = connect.connection;
            string nextAuthorID = "TG000001"; // Initial value

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Use a transaction to ensure data consistency
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        // Get the maximum existing MaKH value
                        string sqlQuery = "SELECT MAX(MaTG) AS LastMaTG FROM TACGIA";
                        SqlCommand command = new SqlCommand(sqlQuery, connection, transaction);

                        object lastMaTG = command.ExecuteScalar();

                        // Check if any MaTG exists in the table
                        if (lastMaTG != DBNull.Value)
                        {
                            int currentID = Convert.ToInt32(lastMaTG.ToString().Substring(2)); // Extract numeric part
                            currentID++; // Increment for next ID
                            nextAuthorID = "TG" + currentID.ToString("D6"); // Format with leading zeros
                        }

                        transaction.Commit(); // Commit the transaction if successful
                    }
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error generating author ID: " + ex.Message);
                    nextAuthorID = null; // Indicate error
                }
            }

            return nextAuthorID;
        }
        public void LoadAuthor(MainWindow mainwindow, int state)
        {
            Authors = new ObservableCollection<Uct_Author>();

            //connect to database
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                mainwindow.Authors.Clear();
                string MaTG = "", TenTG = ""; ;

                try
                {
                    connection.Open();
                    string sqlQuery = "SELECT * FROM TACGIA";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MaTG = reader["MaTG"].ToString();
                            TenTG = reader["TenTG"].ToString();

                            Uct_Author author = new Uct_Author(this);
                            author.AuthorSetState(state);
                            author.LoadData(MaTG, TenTG);
                            mainwindow.Authors.Add(author);
                        }
                    }
                    mainwindow.dtg_AuthorList.Items.Refresh();
                    mainwindow.dtg_GenreList.Items.Refresh();
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error retrieving data: " + ex.Message);
                }
                wpn_Author.Children.Clear();
                foreach (Uct_Employee child in Employees)
                {
                    child.EmployeeSetState(0);
                    wpn_Author.Children.Add(child);
                }
            }
        }

    }
}

