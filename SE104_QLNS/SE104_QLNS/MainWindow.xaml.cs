using SE104_QLNS;
using SE104_QLNS.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace SE104_QLNS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Uct_Books> Books { get; set; } = new ObservableCollection<Uct_Books>();
        public ObservableCollection<Uct_Books> BooksSell { get; set; } = new ObservableCollection<Uct_Books>();
        public ObservableCollection<string> BookSearchItemsOriginal { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> BookSearchItemsSearched { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<Uct_Books> FilteredBooks { get; set; } = new ObservableCollection<Uct_Books>();
        public ObservableCollection<Uct_Customer> Customers { get; set; } = new ObservableCollection<Uct_Customer>();
        public ObservableCollection<Uct_Employee> Employees { get; set; } = new ObservableCollection<Uct_Employee>();
        

        public ObservableCollection<ImportedBookReceiptInfo> ImportBookReceipts { get; set; } = new ObservableCollection<ImportedBookReceiptInfo>();
        public ObservableCollection<BillInfo> Bills { get; set; } = new ObservableCollection<BillInfo>();

        Connection connect = new Connection();


        Uct_Books selectedbook = null;
        ImportedBookReceiptInfo selectedimportreceipt = null;
        BillInfo selectedbillinfo = null;

        bool isBookDelete = false;
        bool isBookUpdate = false;
        bool isBookList = true;

        bool isImportPaperDelete = false;
        bool isExportPaperDelete = false;

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
            LoadExportPaper(this, 0);
            txb_BillDate.Text = DateTime.Now.ToString();
            LoadAuthor(this, 0);
            Cbx_SearchBook.SelectedIndex = 0;
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
        public string GetNextExportPaperID(MainWindow mainwindow)
        {
            string connectionString = connect.connection;
            string nextExportPaperID = "HD000001"; // Initial value

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Use a transaction to ensure data consistency
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        string sqlQuery = "SELECT MAX(MaHD) AS LastMaHD FROM HOADON";
                        SqlCommand command = new SqlCommand(sqlQuery, connection, transaction);

                        object LastMaHD = command.ExecuteScalar();

                        if (LastMaHD != DBNull.Value)
                        {
                            int currentID = Convert.ToInt32(LastMaHD.ToString().Substring(2)); // Extract numeric part
                            currentID++; // Increment for next ID
                            nextExportPaperID = "HD" + currentID.ToString("D6"); // Format with leading zeros
                        }

                        transaction.Commit(); // Commit the transaction if successful
                    }
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error generating customer ID: " + ex.Message);
                    nextExportPaperID = null; // Indicate error
                }
            }
            return nextExportPaperID;
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
            int bookcount = 0;
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
                            BookSearchItemsOriginal.Add(TenDauSach + " - " + MaSach);
                            order++;
                            bookcount += Convert.ToInt32(SoLuongTon);
                        }
                        lbl_BookCount.Content = bookcount.ToString();
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
                cbx_CustomerID.Items.Clear();
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
                            SoTienNo = reader["SoTienNo"].ToString().Replace(",0000", ""); // Assuming numeric data type
                            GioiTinh = reader["GioiTinh"].ToString();
                            NgaySinh = reader["NgaySinh"].ToString(); // Assuming date/time data type
                            SoTienMua = reader["SoTienMua"].ToString().Replace(",0000",""); // Assuming numeric data type

                            string gender;
                            if (GioiTinh == "1")
                                gender = "Nam";
                            else
                                gender = "Nữ";
                            Uct_Customer customer = new Uct_Customer(this);
                            customer.CustomerSetState(state);
                            customer.LoadData(MaKH, HoTenKH, NgaySinh, gender, SDT, DiaChi, Email, SoTienMua, SoTienNo);
                            mainwindow.Customers.Add(customer);

                            cbx_CustomerID.Items.Add(MaKH);
                        }
                    }
                    reader.Close();
                    cbx_CustomerID.Items.Add("Thêm mới");
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
        public void LoadExportPaper(MainWindow mainwindow, int state)
        {
            mainwindow.Bills.Clear();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string billID="", creationDate = "",
                    customerID = "", customerName = "",
                    customerPhoneNumber = "", customerEmail = "",
                    customerAddress = "", billTotal = "",
                    billPaid = "", billRemaining = "";

                try
                {
                    connection.Open();
                    string sqlQuery = "SELECT DISTINCT * FROM HOADON JOIN KHACHHANG ON HOADON.MAKH=KHACHHANG.MAKH";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    int order = 1;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            billID = reader["MaHD"].ToString();
                            customerID= reader["MaKh"].ToString();

                            creationDate = reader["NgayLap"].ToString();

                            customerID = reader["MaKh"].ToString();
                            customerName = reader["HoTenKH"].ToString();

                            customerPhoneNumber = reader["SDT"].ToString();
                            customerEmail = reader["Email"].ToString();
                            customerAddress = reader["DiaChi"].ToString();

                            billTotal = reader["TongTien"].ToString().Replace(",0000", "");
                            billPaid = reader["SoTienTra"].ToString().Replace(",0000", "");
                            billRemaining = reader["ConLai"].ToString().Replace(",0000", "");

                            BillInfo bill = new BillInfo(this, state);
                            bill.SetState(state);
                            bill.LoadData(order, billID, creationDate,customerID,customerName,customerPhoneNumber,
                                customerEmail,customerAddress,billTotal,billPaid,billRemaining);
                            mainwindow.Bills.Add(bill);
                            order++;
                        }
                    }
                    mainwindow.dtg_BillList.Items.Refresh();
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


        // Sell
        
        private void btn_DeleteBookFromSellList_Click(object sender, RoutedEventArgs e)
        {
            BooksSell = new ObservableCollection<Uct_Books>();
            cbx_BookSearch.Text = null;
        }
        private void cbx_BookSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbx_BookSearch.Text= cbx_BookSearch.SelectedItem.ToString();
            foreach(Uct_Books book in Books)
            {
                if (book.BookID == cbx_BookSearch.Text.Substring(Math.Max(0, cbx_BookSearch.Text.Length - 8)))
                {
                    foreach (Uct_Books book2 in BooksSell)
                        if (book2 == book)
                            return;
                        BooksSell.Add(book);
                }
            }
        }
        private void dtg_SellList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedbook = (Uct_Books)dtg_SellList.SelectedItem;
        }
        private void btn_DeleteBookFromSell_Click(object sender, RoutedEventArgs e)
        {
            if (selectedbook != null)
                BooksSell.Remove(selectedbook);
        }
        private void cbx_CustomerID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cbx_CustomerID.SelectedItem!=null)
            cbx_CustomerID.Text = cbx_CustomerID.SelectedItem.ToString();
            if (cbx_CustomerID.Text == "Thêm mới")
            {
                tbl_CustomerName.IsReadOnly = false;
                tbl_CustomerPhoneNumber.IsReadOnly = false;
                tbl_CustomerDetailAdress.IsReadOnly = false;
                tbl_CustomerEmail.IsReadOnly = false;
                txt_CustomerDateOfBirth.IsReadOnly = false;
                cbx_CustomerGender.IsReadOnly = false;

                btn_SaveNewCustomerInfomation.Visibility = Visibility.Visible;
                tbl_CustomerName.Text = "Tên khách hàng";
                tbl_CustomerPhoneNumber.Text = "Số điện thoại";
                tbl_CustomerDetailAdress.Text = "Địa chỉ";
                tbl_CustomerEmail.Text = "Email";
                txt_CustomerDateOfBirth.Text = "Ngày Sinh";
                cbx_CustomerGender.Text = "Giới Tính";
            }
            else
            {
                btn_SaveNewCustomerInfomation.Visibility = Visibility.Hidden;
                tbl_CustomerName.IsReadOnly = true;
                tbl_CustomerPhoneNumber.IsReadOnly = true;
                tbl_CustomerDetailAdress.IsReadOnly = true;
                tbl_CustomerEmail.IsReadOnly = true;
                txt_CustomerDateOfBirth.IsReadOnly = true;
                cbx_CustomerGender.IsReadOnly = true;

                string connectionString = connect.connection;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string sqlQuery = "SELECT * FROM KHACHHANG WHERE MaKH=@MaKH";
                        SqlCommand command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@MaKH", cbx_CustomerID.Text);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            reader.Read();

                            tbl_CustomerName.Text = reader["HoTenKH"].ToString();
                            tbl_CustomerPhoneNumber.Text = reader["SDT"].ToString();
                            tbl_CustomerDetailAdress.Text = reader["DiaChi"].ToString();
                            tbl_CustomerEmail.Text = reader["Email"].ToString();
                            string GioiTinh = reader["GioiTinh"].ToString();
                            txt_CustomerDateOfBirth.Text = reader["NgaySinh"].ToString(); // Assuming date/time data type

                            if (GioiTinh == "1")
                                cbx_CustomerGender.Text = "Nam";
                            else
                                cbx_CustomerGender.Text = "Nữ";
                            reader.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Notification noti = new Notification("Error", "Error retrieving data: " + ex.Message);
                    }
                }
            }
        }
        private void btn_SaveNewCustomerInfomation_Click(object sender, RoutedEventArgs e)
        {
            string MaKH = GetNextCustomerID(this);
            Connection connect = new Connection();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sqlQuery = $"INSERT INTO KHACHHANG (MAKH, HoTenKH, Email, SDT, NgaySinh, GioiTinh, DiaChi, SoTienNo, SoTienMua) " +
                      $"VALUES (@MaKH, @HoTenKH, @Email, @SDT, @NgaySinh, @GioiTinh, @DiaChi, @SoTienNo, @SoTienMua)";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaKH", MaKH);
                    command.Parameters.AddWithValue("@HoTenKH", tbl_CustomerName.Text);
                    command.Parameters.AddWithValue("@Email", tbl_CustomerEmail.Text);
                    command.Parameters.AddWithValue("@SDT", tbl_CustomerPhoneNumber.Text);
                    command.Parameters.AddWithValue("@NgaySinh", txt_CustomerDateOfBirth.Text);
                    string gender;
                    if (cbx_CustomerGender.Text == "Nam")
                        gender = "1";
                    else
                        gender = "0";
                    command.Parameters.AddWithValue("@GioiTinh", gender);
                    command.Parameters.AddWithValue("@DiaChi", tbl_CustomerDetailAdress.Text);
                    command.Parameters.AddWithValue("@SoTienNo", "0");
                    command.Parameters.AddWithValue("@SoTienMua", "0");

                    SqlDataReader reader = command.ExecuteReader();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error retrieving data: " + ex.Message);
                }
            }
            LoadCustomer(this, 0);
        }
        private void dtg_SellList_CurrentCellChanged(object sender, EventArgs e)
        {
            int money = 0;
            foreach (Uct_Books book in BooksSell)
            {
                book.BookTotalSellPrice = Convert.ToInt32(book.BookPriceExport.Replace(",0000", "")) * book.BookSellAmount;
                money += book.BookTotalSellPrice;
            }
            txt_ReceiptPrice.Text = money.ToString();
        }
        private void txb_CustomerPayment_TextChanged(object sender, TextChangedEventArgs e)
        {
            int convertedPrice, PaymentPrice;
            if ((int.TryParse(txt_ReceiptPrice.Text, out convertedPrice))&& (int.TryParse(txb_CustomerPayment.Text, out PaymentPrice)))
            {
                if(PaymentPrice> convertedPrice) 
                {
                    txb_CustomerPayment.Text = txt_ReceiptPrice.Text;
                    PaymentPrice = convertedPrice;
                }
                txb_MoneyOwe.Text = (convertedPrice- PaymentPrice).ToString();
            }
            else
            {
                txb_CustomerPayment.Text = "0";
            }
        }
        private void btn_SaveBillToDatabase_Click(object sender, RoutedEventArgs e)
        {
            string NextExportId = GetNextExportPaperID(this);
            if (cbx_CustomerID.Text == "Thêm mới")
                return;
            Connection connect = new Connection();
            string connectionString = connect.connection;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //Create The BillID first
                    string sqlQuery = $"INSERT INTO HOADON (MaHD, MaKH, NgayLap, TongTien, SoTienTra, ConLai) " +
                          $"VALUES (@MaHD, @MaKH, @NgayLap, @TongTien, @SoTienTra, @ConLai)";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaHD", NextExportId);
                    command.Parameters.AddWithValue("@MaKH", cbx_CustomerID.Text);
                    command.Parameters.AddWithValue("@NgayLap", DateTime.Now);
                    command.Parameters.AddWithValue("@TongTien", txt_ReceiptPrice.Text);
                    command.Parameters.AddWithValue("@SoTienTra", txb_CustomerPayment.Text);
                    command.Parameters.AddWithValue("@ConLai", txb_MoneyOwe.Text);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();

                    //Create Each Detail
                    foreach (Uct_Books book in BooksSell)
                    {
                        //Create Details
                        sqlQuery = $"INSERT INTO CT_HOADON (MaHD, MaSach, SoLuong, DonGiaBan) " +
                              $"VALUES (@MaHD, @MaSach, @SoLuong, @DonGiaBan)";
                        command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@MaHD", NextExportId);
                        command.Parameters.AddWithValue("@MaSach", book.BookID);
                        command.Parameters.AddWithValue("@SoLuong", book.BookSellAmount);
                        command.Parameters.AddWithValue("@DonGiaBan", book.BookPriceExport);
                        reader = command.ExecuteReader();
                        reader.Read();
                        reader.Close();

                        //Reduce Book
                        sqlQuery = $"UPDATE SACH SET SOLUONGTON-=@SOLUONGTON WHERE MASACH=@MASACH";
                        command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@SOLUONGTON", book.BookSellAmount);
                        command.Parameters.AddWithValue("@MaSach", book.BookID);
                        reader = command.ExecuteReader();
                        reader.Read();
                        reader.Close();
                    }

                    //Update Debt
                    sqlQuery = $"UPDATE KHACHHANG SET SOTIENMUA+=@SOTIENMUA WHERE MaKH=@MaKH";
                    command = new SqlCommand(sqlQuery, connection);

                    command.Parameters.AddWithValue("@SOTIENMUA", Convert.ToInt32(txt_ReceiptPrice.Text));
                    command.Parameters.AddWithValue("@MaKH", cbx_CustomerID.Text);
                    reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();

                    sqlQuery = $"UPDATE KHACHHANG SET SOTIENNO+=@SOTIENNO WHERE MaKH=@MaKH";
                    command = new SqlCommand(sqlQuery, connection);

                    command.Parameters.AddWithValue("@SOTIENNO", Convert.ToInt32(txb_MoneyOwe.Text));
                    command.Parameters.AddWithValue("@MaKH", cbx_CustomerID.Text);
                    reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();

                    LoadCustomer(this, 0);
                    LoadBook(this, 0);
                    BooksSell = new ObservableCollection<Uct_Books>();
                    cbx_CustomerID.Text = "";
                    tbl_CustomerName.Text = "Tên khách hàng";
                    tbl_CustomerPhoneNumber.Text = "Số điện thoại";
                    tbl_CustomerEmail.Text = "Email";
                    txt_CustomerDateOfBirth.Text = "Ngày Sinh";
                    cbx_CustomerGender.Text = "";
                    tbl_CustomerDetailAdress.Text = "Địa Chỉ";
                    txb_MoneyOwe.Text = "";
                    txb_CustomerPayment.Text = "";
                    txt_ReceiptPrice.Text = "";
                }
            }
            catch (Exception ex) 
            {
                Notification noti = new Notification("Error", "Error creating bills: " + ex.Message);
            }
            LoadExportPaper(this, isExportPaperDelete ? 1 : 0);
        }

        //view import export
        private void dtg_ImportBookReceiptList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedimportreceipt = (ImportedBookReceiptInfo)dtg_ImportBookReceiptList.SelectedItem;
        }
        private void ViewImportBook_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((selectedimportreceipt != null) && (!isImportPaperDelete))
            {
                selectedimportreceipt.Show();
            }
        }
        private void btn_ImportBookReceiptDelete_Click(object sender, RoutedEventArgs e)
        {
            isExportPaperDelete = false;
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
        private void btn_DeleteBill_Click(object sender, RoutedEventArgs e)
        {
            isImportPaperDelete = false;
            if (!isExportPaperDelete) //Switch from normal to delete
            {
                dtg_BillList.Columns[0].Visibility = Visibility.Visible;
                btn_ImportBookReceiptDelete.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                isExportPaperDelete = true;
                LoadExportPaper(this, 1);
            }
            else
            {
                dtg_BillList.Columns[0].Visibility = Visibility.Hidden;
                isExportPaperDelete = false;
                btn_ImportBookReceiptDelete.Background = new SolidColorBrush(Colors.Transparent);
                LoadExportPaper(this, 0);
            }

        }
        private void btn_DeleteImportBook_Click(object sender, RoutedEventArgs e)
        {
            selectedimportreceipt.Show();
        }
        private void SelectImportExport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dtg_BillList.Columns[0].Visibility = Visibility.Visible;
            dtg_BillList.Columns[0].Visibility = Visibility.Hidden;
            if (SelectImportExport.SelectedItem == null)
                return;
            cvs_ImportBookReceipt.Visibility = Visibility.Hidden;
            cvs_Bill.Visibility = Visibility.Visible;
            SelectImportExport.SelectedItem = null;
        }
        private void SwapExportImport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dtg_ImportBookReceiptList.Columns[0].Visibility = Visibility.Visible;
            dtg_ImportBookReceiptList.Columns[0].Visibility = Visibility.Hidden;
            if (SwapExportImport.SelectedItem == null)
                return;
            cvs_ImportBookReceipt.Visibility = Visibility.Visible;
            cvs_Bill.Visibility = Visibility.Hidden;
            SwapExportImport.SelectedItem = null;
        }

        private void btn_DeleteExportBook_Click(object sender, RoutedEventArgs e)
        {
            if (selectedbillinfo != null)
            {
                selectedbillinfo.Show();
            }
        }
        private void dtg_BillList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtg_BillList.SelectedItem != null)
                selectedbillinfo = (BillInfo)dtg_BillList.SelectedItem;
        }

        private void dtg_BillList_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((selectedbillinfo != null) && (!isExportPaperDelete))
                selectedbillinfo.Show();
        }
        //Books
        private void dtg_Books_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedbook = (Uct_Books)dtg_Books.SelectedItem;
        }
            //Search Book
        private void txt_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isBookList)
            {
                if (string.IsNullOrEmpty(txt_Search.Text))
                {
                    dtg_Books.ItemsSource = Books; // Reset to all items if no search text
                    return;
                }
                if (Cbx_SearchBook.Text == "Tất Cả" || (Cbx_SearchBook.Text == null))
                {
                    var filteredItems = Books.Where(book =>
                    book.BookID.ToLower().Contains(txt_Search.Text.ToLower())||
                        book.BookName.ToLower().Contains(txt_Search.Text.ToLower()) ||
                        book.BookGenre.ToLower().Contains(txt_Search.Text.ToLower()) ||
                        book.BookAuthor.ToLower().Contains(txt_Search.Text.ToLower())
                    ).ToList();

                    dtg_Books.ItemsSource = filteredItems;
                }
                else if (Cbx_SearchBook.Text == "Tên Sách")
                {
                    var filteredItems = Books.Where(book =>
                        book.BookName.ToLower().Contains(txt_Search.Text.ToLower())
                    ).ToList();

                    dtg_Books.ItemsSource = filteredItems;
                }
                else if (Cbx_SearchBook.Text == "Thể Loại")
                {
                    var filteredItems = Books.Where(book =>
                        book.BookGenre.ToLower().Contains(txt_Search.Text.ToLower())
                    ).ToList();
                }
                else if (Cbx_SearchBook.Text == "Tác Giả")
                {
                    var filteredItems = Books.Where(book =>
                        book.BookAuthor.ToLower().Contains(txt_Search.Text.ToLower())
                    ).ToList();
                }
            }
            else
            {
                if (string.IsNullOrEmpty(txt_Search.Text))
                {
                    foreach (Uct_Books child in wpn_Books.Children)
                    {
                            child.Visibility = Visibility.Visible;
                    }
                }
                if (Cbx_SearchBook.Text == "Tất Cả" || (Cbx_SearchBook.Text == null))
                {
                    foreach(Uct_Books child in wpn_Books.Children)
                    {
                        if (child.BookName.Contains(txt_Search.Text)
                            || child.BookAuthor.Contains(txt_Search.Text)
                            ||child.BookID.Contains(txt_Search.Text)
                            || child.BookGenre.Contains(txt_Search.Text))
                        {
                            child.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            child.Visibility = Visibility.Collapsed;
                        }
                    }
                }
                else if (Cbx_SearchBook.Text == "Tên Sách")
                {
                    foreach (Uct_Books child in wpn_Books.Children)
                    {
                        if (child.BookName.Contains(txt_Search.Text))
                        {
                            child.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            child.Visibility = Visibility.Collapsed;
                        }
                    }
                }
                else if (Cbx_SearchBook.Text == "Thể Loại")
                {
                    foreach (Uct_Books child in wpn_Books.Children)
                    {
                        if (child.BookGenre.Contains(txt_Search.Text))
                        {
                            child.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            child.Visibility = Visibility.Collapsed;
                        }
                    }
                }
                else if (Cbx_SearchBook.Text == "Tác Giả")
                {
                    foreach (Uct_Books child in wpn_Books.Children)
                    {
                        if (child.BookAuthor.Contains(txt_Search.Text))
                        {
                            child.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            child.Visibility = Visibility.Collapsed;
                        }
                    }
                }
        }
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
            if (!isBookUpdate && !isBookDelete && selectedbook!=null)
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
                        string sqlQuery = "SELECT MAX(MaTacGia) AS LastMaTG FROM TACGIA";
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
            

            //connect to database
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
               
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
                            MaTG = reader["MaTacGia"].ToString();
                            TenTG = reader["TenTacGia"].ToString();

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
               
            }
        }
        private void txt_CustomerSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            foreach (Uct_Customer child in wpn_Customer.Children)
            {
                if (child is Uct_Customer)
                {
                    if (child.CustomerName.Contains(txt_CustomerSearch.Text)|| child.CustomerPhonenumber.Contains(txt_CustomerSearch.Text))
                    {
                        child.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        child.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void CustomerSearchComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CustomerSearchComboBox.Text= CustomerSearchComboBox.SelectedItem.ToString();
        }

        private void btn_SaveBillAsPDF_Click(object sender, RoutedEventArgs e)
        {
            Bills[Bills.Count - 1].Show();
        }

        private void dtg_AuthorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }


        private void SelectAuthorGenre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dtg_AuthorList.Columns[0].Visibility = Visibility.Visible;
            dtg_AuthorList.Columns[0].Visibility = Visibility.Hidden;
            if (SelectAuthorGenre.SelectedItem == null)
                return;
            cvs_Author.Visibility = Visibility.Hidden;
            cvs_Genre.Visibility = Visibility.Visible;
            SelectImportExport.SelectedItem = null;
        }
        private void SwapGenreAuthor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dtg_GenreList.Columns[0].Visibility = Visibility.Visible;
            dtg_GenreList.Columns[0].Visibility = Visibility.Hidden;
            if (SwapGenreAuthor.SelectedItem == null)
                return;
            cvs_Author.Visibility = Visibility.Visible;
            cvs_Genre.Visibility = Visibility.Hidden;
            SwapGenreAuthor.SelectedItem = null;
        }
    }
}

