using SE104_QLNS;
using SE104_QLNS.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
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
        public ObservableCollection<Uct_Customer> Customers { get; set; } = new ObservableCollection<Uct_Customer>();
        public ObservableCollection<Uct_Employee> Employees { get; set; } = new ObservableCollection<Uct_Employee>();
        public ObservableCollection<Uct_Genre> Genres { get; set; } = new ObservableCollection<Uct_Genre>();
        public ObservableCollection<Uct_Author> Authors { get; set; } = new ObservableCollection<Uct_Author>();
        public ObservableCollection<Uct_CustomerReceipt> CustomerReceipt { get; set; } = new ObservableCollection<Uct_CustomerReceipt>();
        public ObservableCollection<ImportedBookReceiptInfo> ImportBookReceipts { get; set; } = new ObservableCollection<ImportedBookReceiptInfo>();
        public ObservableCollection<BillInfo> Bills { get; set; } = new ObservableCollection<BillInfo>();
        public ObservableCollection<Uct_BaoCaoTon> BaoCaoTon { get; set; } = new ObservableCollection<Uct_BaoCaoTon>();
        public ObservableCollection<Uct_BaoCaoCongNo> BaoCaoCongNo { get; set; } = new ObservableCollection<Uct_BaoCaoCongNo>();

        Connection connect = new Connection();

        Uct_Books selectedbook = null;
        ImportedBookReceiptInfo selectedimportreceipt = null;
        BillInfo selectedbillinfo = null;
        Uct_Genre selectedgenre = null;
        Uct_Author selectedauthor = null;
        Uct_CustomerReceipt selectedcustomerreceipt = null;

        public string ApDungQuyDinhKiemTraSoTienThu = "";
        public string SoLuongNhapToiThieu = "";
        public string SoLuongTonToiDaTruocNhap = "";
        public string SoLuongTonToiThieuSauBan = "";
        public string SoTienNoToiDa = "";
        public string TiLeGiaBan = "";


        public bool isBookDelete = false;
        public bool isBookUpdate = false;
        public bool isBookList = true;

        public bool isImportPaperDelete = false;
        public bool isExportPaperDelete = false;

        public bool isCustomerUpdate = false;
        public bool isCustomerDelete = false;

        public bool isEmployeeUpdate = false;
        public bool isEmployeeDelete = false;

        public bool isCustomerReceiptDelete = false;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            LoadAll(this);
            txb_BillDate.Text = DateTime.Now.ToString();
            Cbx_SearchBook.SelectedIndex = 0;
            ImportExportComboBox.SelectedIndex = 0;
            ImportDate.Text = DateTime.Now.ToString();
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
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi tạo BookTitle ID: " + ex.Message);
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
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi tạo Book ID: " + ex.Message);
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
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi tạo customer ID: " + ex.Message);
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
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi tạo PHIEUNHAP ID: " + ex.Message);
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
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi tạo HOADON ID: " + ex.Message);
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
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi tạo employee ID: " + ex.Message);
                    nextEmployeeID = null; // Indicate error
                }
            }

            return nextEmployeeID;
        }
        public string GetNextGenreID(MainWindow mainwindow)
        {
            string connectionString = connect.connection;
            string nextBookID = "TL000001"; // Initial value

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Use a transaction to ensure data consistency
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        string sqlQuery = "SELECT MAX(MaTheLoai) AS LastMaTheLoai FROM THELOAI";
                        SqlCommand command = new SqlCommand(sqlQuery, connection, transaction);

                        object LastMaTheLoai = command.ExecuteScalar();

                        if (LastMaTheLoai != DBNull.Value)
                        {
                            int currentID = Convert.ToInt32(LastMaTheLoai.ToString().Substring(2)); // Extract numeric part
                            currentID++; // Increment for next ID
                            nextBookID = "TL" + currentID.ToString("D6"); // Format with leading zeros
                        }

                        transaction.Commit(); // Commit the transaction if successful
                    }
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi tạo Book ID: " + ex.Message);
                    nextBookID = null; // Indicate error
                }
            }

            return nextBookID;
        }
        public string GetNextCustomerReceiptID(MainWindow mainwindow)
        {
            string connectionString = connect.connection;
            string nextBookID = "PT000001"; // Initial value

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Use a transaction to ensure data consistency
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        string sqlQuery = "SELECT MAX(MaPhieuThuTien) AS LastMaMaPhieuThuTien FROM PHIEUTHUTIEN";
                        SqlCommand command = new SqlCommand(sqlQuery, connection, transaction);

                        object LastMaMaPhieuThuTien = command.ExecuteScalar();

                        if (LastMaMaPhieuThuTien != DBNull.Value)
                        {
                            int currentID = Convert.ToInt32(LastMaMaPhieuThuTien.ToString().Substring(2)); // Extract numeric part
                            currentID++; // Increment for next ID
                            nextBookID = "PT" + currentID.ToString("D6"); // Format with leading zeros
                        }

                        transaction.Commit(); // Commit the transaction if successful
                    }
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi tạo Book ID: " + ex.Message);
                    nextBookID = null; // Indicate error
                }
            }

            return nextBookID;
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
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi tạo author ID: " + ex.Message);
                    nextAuthorID = null; // Indicate error
                }
            }

            return nextAuthorID;
        }
        //Load
        public void LoadAll(MainWindow mainwindow)
        {
            //Book
            if(mainwindow.isBookDelete)
                mainwindow.LoadBook(mainwindow, 1);
            else if(mainwindow.isBookUpdate)
                mainwindow.LoadBook(mainwindow, 2);
            else
                mainwindow.LoadBook(mainwindow, 0);

            //CustomerReceipt
            mainwindow.LoadCustomerReceipt(mainwindow);

            //BaoCaoTon
            mainwindow.LoadBaoCaoTon(mainwindow);

            //BaoCaoCongNo
            mainwindow.LoadBaoCaoCongNo(mainwindow);

            //Customer
            if (mainwindow.isCustomerDelete)
                mainwindow.LoadCustomer(mainwindow, 2);
            else if (mainwindow.isCustomerUpdate)
                mainwindow.LoadCustomer(mainwindow, 1);
            else
                mainwindow.LoadCustomer(mainwindow, 0);

            //Import
            if(mainwindow.isImportPaperDelete)
                mainwindow.LoadImportPaper(mainwindow, 1);
            else
                mainwindow.LoadImportPaper(mainwindow, 0);

            //Bills
            if (mainwindow.isExportPaperDelete)
                mainwindow.LoadExportPaper(mainwindow, 1);
            else
                mainwindow.LoadExportPaper(mainwindow, 0);

            //Tham So
            mainwindow.LoadTHAMSO(mainwindow);

            //Genre
            mainwindow.LoadGenre(mainwindow);

            //Author
            mainwindow.LoadAuthor(mainwindow);

            //employee
            if (mainwindow.isEmployeeDelete)
                mainwindow.LoadEmployee(mainwindow,2);
            else if (mainwindow.isEmployeeUpdate)
                mainwindow.LoadEmployee(mainwindow, 1);
            else
                mainwindow.LoadEmployee(mainwindow, 0);
        }
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
                BookSearchItemsOriginal.Clear();
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
                            DonGiaNhap = reader["DonGiaNhap"].ToString();
                            DonGiaBan = reader["DonGiaBan"].ToString(); // Assuming date/time data type
                            DonGiaNhap = DonGiaNhap.Substring(0, DonGiaNhap.Length - 5);
                            DonGiaBan = DonGiaBan.Substring(0, DonGiaBan.Length - 5);

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
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi lấy thông tin từ SACH: " + ex.Message);
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
        public void LoadCustomerReceipt(MainWindow mainwindow)
        {
            //connect to database
            mainwindow.CustomerReceipt.Clear();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sqlQuery = "SELECT DISTINCT PHIEUTHUTIEN.*, KHACHHANG.* FROM PHIEUTHUTIEN " +
                        " LEFT JOIN KHACHHANG ON PHIEUTHUTIEN.MaKH = KHACHHANG.MaKH";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string MaPhieuThuTien = reader["MaPhieuThuTien"].ToString();
                            string MaKH = reader["MaKH"].ToString();
                            string NgayThuTien = reader["NgayThuTien"].ToString();
                            string TenKH = reader["HoTenKH"].ToString();
                            string SDT = reader["SDT"].ToString();
                            string Email = reader["Email"].ToString();
                            string DiaChi = reader["DiaChi"].ToString();
                            string SoTienThu = reader["SoTienThu"].ToString();
                            SoTienThu = SoTienThu.Substring(0, SoTienThu.Length - 5);
                            string SoTienTruocThu = reader["SoTienTruocThu"].ToString();
                            SoTienTruocThu = SoTienTruocThu.Substring(0, SoTienTruocThu.Length - 5);
                            string SoTienSauThu = reader["SoTienSauThu"].ToString();
                            SoTienSauThu = SoTienSauThu.Substring(0, SoTienSauThu.Length - 5);

                            Uct_CustomerReceipt receipt = new Uct_CustomerReceipt(MaPhieuThuTien, MaKH, TenKH, SDT, Email, DiaChi, NgayThuTien, SoTienTruocThu, SoTienThu, SoTienSauThu);
                            mainwindow.CustomerReceipt.Add(receipt);
                        }
                    }
                    mainwindow.dtg_CustomerPaymentReceipt.Items.Refresh();
                    reader.Close();

                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi lấy thông tin từ PHIEUTHUTIEN: " + ex.Message);
                }
            }
        }
        public void LoadBaoCaoTon(MainWindow mainwindow)
        {
            //connect to database
            mainwindow.BaoCaoTon.Clear();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string MaSach = "", HinhAnhSach = "", TenSach = "";
                int TonDau = 0, PhatSinh = 0, TonCuoi = 0, Month = 0, Year = 0;
                try
                {
                    connection.Open();
                    string sqlQuery = "SELECT DISTINCT BAOCAOTON.*, DAUSACH.*, SACH.* FROM BAOCAOTON " +
                        "JOIN SACH ON BAOCAOTON.MASACH = SACH.MASACH " +
                        "JOIN DAUSACH ON SACH.MADAUSACH = DAUSACH.MADAUSACH";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MaSach = reader["MaSach"].ToString();
                            TenSach = reader["TenDauSach"].ToString();
                            HinhAnhSach = reader["HinhAnhSach"].ToString();
                            TonDau = Convert.ToInt32(reader["TonDau"]);
                            PhatSinh = Convert.ToInt32(reader["PhatSinh"]);
                            TonCuoi = Convert.ToInt32(reader["TonCuoi"]);
                            Month = Convert.ToInt32(reader["Thang"]);
                            Year = Convert.ToInt32(reader["Nam"]);

                            Uct_BaoCaoTon baocao = new Uct_BaoCaoTon(Month, Year, MaSach, HinhAnhSach, TenSach, TonDau, PhatSinh, TonCuoi);
                            mainwindow.BaoCaoTon.Add(baocao);
                        }
                    }
                    reader.Close();
                    string Thang = "", Nam = "";

                    //Add to Month from BAOCAOTON
                    sqlQuery = "SELECT Thang FROM BAOCAOTON";
                    command = new SqlCommand(sqlQuery, connection);
                    reader = command.ExecuteReader();
                    cbx_AmountReportMonth.Items.Clear();
                    cbx_AmountReportMonth.Items.Add("Tất cả");
                    cbx_AmountReportMonth.SelectedIndex = 0;
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            bool isDuplicate = false;
                            Thang = reader["Thang"].ToString();
                            foreach (string item in cbx_AmountReportMonth.Items)
                            {
                                if (item == Thang)
                                {
                                    isDuplicate = true;
                                    break;
                                }
                            }
                            if (!isDuplicate)
                            {
                                cbx_AmountReportMonth.Items.Add(Thang);
                            }
                        }
                    }
                    reader.Close();

                    //Add to Year from BAOCAOTON
                    sqlQuery = "SELECT Nam FROM BAOCAOTON";
                    command = new SqlCommand(sqlQuery, connection);
                    reader = command.ExecuteReader();
                    cbx_AmountReportYear.Items.Clear();
                    cbx_AmountReportYear.Items.Add("Tất cả");
                    cbx_AmountReportYear.SelectedIndex = 0;
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            bool isDuplicate = false;
                            Nam = reader["Nam"].ToString();
                            foreach (string item in cbx_AmountReportYear.Items)
                            {
                                if (item == Nam)
                                {
                                    isDuplicate = true;
                                    break;
                                }
                            }
                            if (!isDuplicate)
                            {
                                cbx_AmountReportYear.Items.Add(Nam);
                            }
                        }
                    }
                    reader.Close();

                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi lấy thông tin từ BAOCAOTON: " + ex.Message);
                }
            }
        }
        public void LoadBaoCaoCongNo(MainWindow mainwindow)
        {
            //connect to database
            mainwindow.BaoCaoCongNo.Clear();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string MaKH = "", HoTenKH = "";
                int NoDau = 0, PhatSinh = 0, NoCuoi = 0, Month = 0, Year = 0;
                try
                {
                    connection.Open();
                    string sqlQuery = "SELECT DISTINCT BAOCAOCONGNO.*, KHACHHANG.* FROM BAOCAOCONGNO " +
                        "JOIN KHACHHANG ON BAOCAOCONGNO.MaKH = KHACHHANG.MaKH ";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MaKH = reader["MaKH"].ToString();
                            HoTenKH = reader["HoTenKH"].ToString();
                            NoDau = Convert.ToInt32(reader["NoDau"]);
                            PhatSinh = Convert.ToInt32(reader["PhatSinh"]);
                            NoCuoi = Convert.ToInt32(reader["NoCuoi"]);
                            Month = Convert.ToInt32(reader["Thang"]);
                            Year = Convert.ToInt32(reader["Nam"]);

                            Uct_BaoCaoCongNo baocao = new Uct_BaoCaoCongNo(Month, Year, MaKH, HoTenKH, NoDau, PhatSinh, NoCuoi);
                            mainwindow.BaoCaoCongNo.Add(baocao);
                        }
                    }
                    reader.Close();
                    string Thang = "", Nam = "";

                    //Add to Month from BAOCAOTON
                    sqlQuery = "SELECT Thang FROM BAOCAOCONGNO";
                    command = new SqlCommand(sqlQuery, connection);
                    reader = command.ExecuteReader();
                    cbx_CustomerDebtReportMonth.Items.Clear();
                    cbx_CustomerDebtReportMonth.Items.Add("Tất cả");
                    cbx_CustomerDebtReportMonth.SelectedIndex = 0;
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            bool isDuplicate = false;
                            Thang = reader["Thang"].ToString();
                            foreach (string item in cbx_CustomerDebtReportMonth.Items)
                            {
                                if (item == Thang)
                                {
                                    isDuplicate = true;
                                    break;
                                }
                            }
                            if (!isDuplicate)
                            {
                                cbx_CustomerDebtReportMonth.Items.Add(Thang);
                            }
                        }
                    }
                    reader.Close();

                    //Add to Year from BaoCAOCONGNO
                    sqlQuery = "SELECT Nam FROM BAOCAOCONGNO";
                    command = new SqlCommand(sqlQuery, connection);
                    reader = command.ExecuteReader();

                    cbx_CustomerDebtReportYear.Items.Clear();
                    cbx_CustomerDebtReportYear.Items.Add("Tất cả");
                    cbx_CustomerDebtReportYear.SelectedIndex = 0;
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            bool isDuplicate = false;
                            Nam = reader["Nam"].ToString();
                            foreach (string item in cbx_CustomerDebtReportYear.Items)
                            {
                                if (item == Nam)
                                {
                                    isDuplicate = true;
                                    break;
                                }
                            }
                            if (!isDuplicate)
                            {
                                cbx_CustomerDebtReportYear.Items.Add(Nam);
                            }
                        }
                    }
                    reader.Close();

                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi lấy thông tin từ BAOCAOCONGNO: " + ex.Message);
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
                    string sqlQuery = "SELECT *, CONVERT(varchar(10), NgaySinh, 103) AS FormattedDate FROM KHACHHANG";
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
                            SoTienNo = SoTienNo.Substring(0, SoTienNo.Length - 5);
                            GioiTinh = reader["GioiTinh"].ToString();
                            NgaySinh = reader["FormattedDate"].ToString(); // Assuming date/time data type
                            SoTienMua = reader["SoTienMua"].ToString(); // Assuming numeric data type
                            SoTienMua = SoTienMua.Substring(0, SoTienMua.Length - 5);

                            string gender;
                            if (GioiTinh == "True")
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
                    cbx_CustomerID.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi lấy thông tin từ KHACHHANG: " + ex.Message);
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
                            TongTien = reader["TongTien"].ToString();
                            TongTien = TongTien.Substring(0, TongTien.Length - 5);

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
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi lấy thông tin từ PHIEUNHAP: " + ex.Message);
                }
            }
        }
        public void LoadExportPaper(MainWindow mainwindow, int state)
        {
            mainwindow.Bills.Clear();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string billID = "", creationDate = "",
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
                            customerID = reader["MaKh"].ToString();

                            creationDate = reader["NgayLap"].ToString();

                            customerID = reader["MaKh"].ToString();
                            customerName = reader["HoTenKH"].ToString();

                            customerPhoneNumber = reader["SDT"].ToString();
                            customerEmail = reader["Email"].ToString();
                            customerAddress = reader["DiaChi"].ToString();

                            billTotal = reader["TongTien"].ToString();
                            billPaid = reader["SoTienTra"].ToString();
                            billRemaining = reader["ConLai"].ToString();

                            billTotal = billTotal.Substring(0, billTotal.Length - 5);
                            billPaid = billPaid.Substring(0, billPaid.Length - 5);
                            billRemaining = billRemaining.Substring(0, billRemaining.Length - 5);

                            BillInfo bill = new BillInfo(this, state);
                            bill.SetState(state);
                            bill.LoadData(order, billID, creationDate, customerID, customerName, customerPhoneNumber,
                                customerEmail, customerAddress, billTotal, billPaid, billRemaining);
                            mainwindow.Bills.Add(bill);
                            order++;
                        }
                    }
                    mainwindow.dtg_BillList.Items.Refresh();
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi lấy thông tin từ HOADON: " + ex.Message);
                }
            }
        }
        public void LoadTHAMSO(MainWindow mainwindow)
        {
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                try
                {
                    //ApDungQuyDinhKiemTraSoTienThu
                    connection.Open();
                    string sqlQuery = "SELECT GIATRI FROM THAMSO WHERE TenThamSo='ApDungQuyDinhKiemTraSoTienThu'";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    ApDungQuyDinhKiemTraSoTienThu = reader["GIATRI"].ToString();
                    reader.Close();
                    if (Convert.ToInt32(ApDungQuyDinhKiemTraSoTienThu) == 1)
                        cbx_CheckMoneyReceivedFromCustomer.IsChecked = true;
                    else
                        cbx_CheckMoneyReceivedFromCustomer.IsChecked = false;

                    //SoLuongNhapToiThieu
                    sqlQuery = "SELECT GIATRI FROM THAMSO WHERE TenThamSo='SoLuongNhapToiThieu'";
                    command = new SqlCommand(sqlQuery, connection);
                    reader = command.ExecuteReader();
                    reader.Read();
                    SoLuongNhapToiThieu = reader["GIATRI"].ToString();
                    reader.Close();
                    tbx_BookMinimumImportQuantity.Text = SoLuongNhapToiThieu;

                    //SoLuongTonToiDaTruocNhap
                    sqlQuery = "SELECT GIATRI FROM THAMSO WHERE TenThamSo='SoLuongTonToiDaTruocNhap'";
                    command = new SqlCommand(sqlQuery, connection);
                    reader = command.ExecuteReader();
                    reader.Read();
                    SoLuongTonToiDaTruocNhap = reader["GIATRI"].ToString();
                    reader.Close();
                    tbx_BookMaximumStockQuantityBeforeImporting.Text = SoLuongTonToiDaTruocNhap;

                    //SoLuongTonToiThieuSauBan
                    sqlQuery = "SELECT GIATRI FROM THAMSO WHERE TenThamSo='SoLuongTonToiThieuSauBan'";
                    command = new SqlCommand(sqlQuery, connection);
                    reader = command.ExecuteReader();
                    reader.Read();
                    SoLuongTonToiThieuSauBan = reader["GIATRI"].ToString();
                    reader.Close();
                    tbx_BookMinimumStockQuantityAfterSelling.Text = SoLuongTonToiThieuSauBan;

                    //SoTienNoToiDa
                    sqlQuery = "SELECT GIATRI FROM THAMSO WHERE TenThamSo='SoTienNoToiDa'";
                    command = new SqlCommand(sqlQuery, connection);
                    reader = command.ExecuteReader();
                    reader.Read();
                    SoTienNoToiDa = reader["GIATRI"].ToString();
                    reader.Close();
                    tbx_CustomerMaximumDebt.Text = SoTienNoToiDa;

                    //TiLeGiaBan
                    sqlQuery = "SELECT GIATRI FROM THAMSO WHERE TenThamSo='TiLeGiaBan'";
                    command = new SqlCommand(sqlQuery, connection);
                    reader = command.ExecuteReader();
                    reader.Read();
                    TiLeGiaBan = reader["GIATRI"].ToString();
                    reader.Close();
                    tbx_ImportToExportRatio.Text = TiLeGiaBan;

                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi lấy thông tin từ THAMSO: " + ex.Message);
                }
            }
        }
        public void LoadGenre(MainWindow mainwindow)
        {
            mainwindow.Genres.Clear();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string GenreID = "", GenreName = "";

                try
                {
                    connection.Open();
                    string sqlQuery = "SELECT DISTINCT * FROM THELOAI";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    int order = 1;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            GenreID = reader["MaTheLoai"].ToString();
                            GenreName = reader["TenTheLoai"].ToString();


                            Uct_Genre genre = new Uct_Genre(order, GenreID, GenreName);
                            mainwindow.Genres.Add(genre);
                            order++;
                        }
                    }
                    mainwindow.dtg_GenreList.Items.Refresh();
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi lấy thông tin từ GENRE: " + ex.Message);
                }
            }
        }
        public void LoadAuthor(MainWindow mainwindow)
        {
            mainwindow.Authors.Clear();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string AuthorID = "", AuthorName = "";

                try
                {
                    connection.Open();
                    string sqlQuery = "SELECT DISTINCT * FROM TACGIA";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    int order = 1;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            AuthorID = reader["MaTacGia"].ToString();
                            AuthorName = reader["TenTacGia"].ToString();


                            Uct_Author author = new Uct_Author(order, AuthorID, AuthorName);
                            mainwindow.Authors.Add(author);
                            order++;
                        }
                    }
                    mainwindow.dtg_AuthorList.Items.Refresh();
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi lấy thông tin từ AUTHOR: " + ex.Message);
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
                mainwindow.Employees.Clear();
                string MaNV = "", HoTenNV = "", SDT = "", HinhAnh = "", DiaChi = "",
                    GioiTinh = "", NgaySinh = "", CCCD = "", ViTri = "", Ca = "", TenTK = "", MatKhau = "";

                try
                {
                    connection.Open();
                    string sqlQuery = "SELECT *, CONVERT(varchar(10), NgaySinh, 103) AS FormattedDate FROM NGUOIDUNG;";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MaNV = reader["MaNV"].ToString();
                            HoTenNV = reader["HoTenNV"].ToString();
                            SDT = reader["SDT"].ToString();
                            DiaChi = reader["DiaChi"].ToString();
                            GioiTinh = reader["GioiTinh"].ToString();
                            NgaySinh = reader["FormattedDate"].ToString(); // Assuming date/time data type
                            CCCD = reader["CCCD"].ToString();
                            ViTri = reader["ViTri"].ToString();
                            Ca = reader["Ca"].ToString();
                            TenTK = reader["TenTK"].ToString();
                            MatKhau = reader["MatKhau"].ToString();
                            HinhAnh = reader["HinhAnh"].ToString();
                            string gender;
                            if (GioiTinh == "True")
                                gender = "Nam";
                            else
                                gender = "Nữ";
                            Uct_Employee employee = new Uct_Employee(this, state);
                            employee.LoadData(MaNV, HoTenNV, NgaySinh, gender, CCCD, SDT, DiaChi, ViTri, Ca, TenTK, MatKhau, HinhAnh);
                            mainwindow.Employees.Add(employee);
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi lấy thông tin từ NGUOIDUNG: " + ex.Message);
                }
                wpn_Employee.Children.Clear();
                foreach (Uct_Employee child in Employees)
                {
                    wpn_Employee.Children.Add(child);
                }
            }
        }

        // Sell

        private void btn_DeleteBookFromSellList_Click(object sender, RoutedEventArgs e)
        {
            BooksSell.Clear();
            cbx_BookSearch.Text = null;
            txb_CustomerPayment.Text = "0";
            txb_MoneyOwe.Text = "0";
            txt_ReceiptPrice.Text = "0";
            UpdateSellPrice();
        }
        private void cbx_BookSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //// Get the current text from the combobox
            //string searchText = cbx_BookSearch.Text;

            //BookSearchItemsSearched.Clear();
            //foreach (string Searched in BookSearchItemsOriginal)
            //{
            //    if(Searched.ToLower().Contains(searchText.ToLower()))
            //    BookSearchItemsSearched.Add(Searched);
            //}

            //cbx_BookSearch.ItemsSource = BookSearchItemsSearched;
            if (cbx_BookSearch.SelectedItem == null) return;
            cbx_BookSearch.Text = cbx_BookSearch.SelectedItem.ToString();
            foreach (Uct_Books book in Books)
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
        private void ImportExportComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ImportExportComboBox.SelectedItem == null) return;

            ComboBoxItem selectedItem = (ComboBoxItem)ImportExportComboBox.SelectedItem;
            string selectedValue = selectedItem.Content.ToString();
            ImportExportComboBox.Text = selectedValue;
        }
        private void txt_BillImportBookReceiptSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txt_BillImportBookReceiptSearch.Text))
            {
                dtg_BillList.ItemsSource = Bills; // Reset to all items if no search text

                dtg_ImportBookReceiptList.ItemsSource = ImportBookReceipts;
                return;
            }
            if (ImportExportComboBox.Text == "Tất Cả" || (ImportExportComboBox.Text == null))
            {
                var filteredItems = Bills.Where(bills =>
                bills.BillID.ToLower().Contains(txt_BillImportBookReceiptSearch.Text.ToLower()) ||
                    bills.CreationDate.ToLower().Contains(txt_BillImportBookReceiptSearch.Text.ToLower())
                ).ToList();

                dtg_BillList.ItemsSource = filteredItems;

                var filteredItems2 = ImportBookReceipts.Where(receipt =>
                receipt.ImportBookReceiptID.ToLower().Contains(txt_BillImportBookReceiptSearch.Text.ToLower()) ||
                    receipt.Date.ToLower().Contains(txt_BillImportBookReceiptSearch.Text.ToLower())
                ).ToList();

                dtg_ImportBookReceiptList.ItemsSource = filteredItems2;
            }
            else if (ImportExportComboBox.Text == "Mã phiếu")
            {
                var filteredItems = Bills.Where(bills =>
               bills.BillID.ToLower().Contains(txt_BillImportBookReceiptSearch.Text.ToLower())
               ).ToList();

                dtg_BillList.ItemsSource = filteredItems;

                var filteredItems2 = ImportBookReceipts.Where(receipt =>
                receipt.ImportBookReceiptID.ToLower().Contains(txt_BillImportBookReceiptSearch.Text.ToLower())
                ).ToList();

                dtg_ImportBookReceiptList.ItemsSource = filteredItems2;
            }
            else if (ImportExportComboBox.Text == "Ngày lập")
            {
                var filteredItems = Bills.Where(bills =>
                   bills.CreationDate.ToLower().Contains(txt_BillImportBookReceiptSearch.Text.ToLower())
               ).ToList();

                dtg_BillList.ItemsSource = filteredItems;

                var filteredItems2 = ImportBookReceipts.Where(receipt =>
                    receipt.Date.ToLower().Contains(txt_BillImportBookReceiptSearch.Text.ToLower())
                ).ToList();

                dtg_ImportBookReceiptList.ItemsSource = filteredItems2;
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
            UpdateSellPrice();
        }
        private void cbx_CustomerID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbx_CustomerID.SelectedItem != null)
                cbx_CustomerID.Text = cbx_CustomerID.SelectedItem.ToString();
            if (cbx_CustomerID.Text == "Thêm mới")
            {
                tbl_CustomerName.IsReadOnly = false;
                tbl_CustomerPhoneNumber.IsReadOnly = false;
                tbl_CustomerDetailAdress.IsReadOnly = false;
                tbl_CustomerEmail.IsReadOnly = false;
                txt_CustomerDateOfBirth.IsReadOnly = false;
                cbx_CustomerGender.SelectedIndex = 1;

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
                            tbl_CustomerDebtDisplay.Text = reader["SoTienNo"].ToString();
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
                        Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi lấy thông tin từ KHACHHANG: " + ex.Message);
                    }
                }
            }
        }
        private void btn_SaveNewCustomerInfomation_Click(object sender, RoutedEventArgs e)
        {
            string MaKH = GetNextCustomerID(this);
            if (tbl_CustomerName.Text == "Tên khách hàng")
            {
                Notification noti = new Notification("Thiếu Thông Tin", "Vui lòng nhập đầy đủ thông tin cho khách hàng.");
                return;
            }
            if (tbl_CustomerPhoneNumber.Text == "Số điện thoại")
            {
                Notification noti = new Notification("Thiếu Thông Tin", "Vui lòng nhập đầy đủ thông tin cho khách hàng.");
                return;
            }
            if (tbl_CustomerDetailAdress.Text == "Địa chỉ")
            {
                Notification noti = new Notification("Thiếu Thông Tin", "Vui lòng nhập đầy đủ thông tin cho khách hàng.");
                return;
            }
            if (tbl_CustomerEmail.Text == "Email")
            {
                Notification noti = new Notification("Thiếu Thông Tin", "Vui lòng nhập đầy đủ thông tin cho khách hàng.");
                return;
            }
            if (txt_CustomerDateOfBirth.Text == "Ngày Sinh")
            {
                Notification noti = new Notification("Thiếu Thông Tin", "Vui lòng nhập đầy đủ thông tin cho khách hàng.");
                return;
            }
            if (cbx_CustomerGender.Text == "Giới Tính")
            {
                Notification noti = new Notification("Thiếu Thông Tin", "Vui lòng nhập đầy đủ thông tin cho khách hàng.");
                return;
            }
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
                    command.Parameters.AddWithValue("@NgaySinh", DateTime.Parse(txt_CustomerDateOfBirth.Text));
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
                    reader.Read();
                    reader.Close();

                    //Create for BAOCAOCONGNO
                    sqlQuery = "INSERT INTO BAOCAOCONGNO (Thang, Nam, MaKH, NoDau, PhatSinh, NoCuoi) " +
                  $"VALUES (@Thang, @Nam, @MaKH, @NoDau, @PhatSinh, @NoCuoi)";
                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@Thang", DateTime.Now.Month.ToString());
                    command.Parameters.AddWithValue("@Nam", DateTime.Now.Year.ToString());
                    command.Parameters.AddWithValue("@MaKH", MaKH);
                    command.Parameters.AddWithValue("@NoDau", 0);
                    command.Parameters.AddWithValue("@PhatSinh", Convert.ToInt32(0));
                    command.Parameters.AddWithValue("@NoCuoi", Convert.ToInt32(0));
                    reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();

                    btn_SaveNewCustomerInfomation.Visibility = Visibility.Hidden;
                    Notification notification = new Notification("Tạo Thành Công", "Thêm Khách hàng mã " + MaKH + " thành công!");

                }
                catch (Exception ex)
                {
                    btn_SaveNewCustomerInfomation.Visibility = Visibility.Hidden;
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi thêm vào KHACHHANG: " + ex.Message);
                }
            }
            LoadAll(this);
        }
        private void dtg_SellList_CurrentCellChanged(object sender, EventArgs e)
        {
            UpdateSellPrice();
        }
        private void UpdateSellPrice()
        {
            int money = 0;
            foreach (Uct_Books book in BooksSell)
            {
                book.BookTotalSellPrice = Convert.ToInt32(book.BookPriceExport) * book.BookSellAmount;
                money += book.BookTotalSellPrice;
            }
            dtg_SellList.Items.Refresh();
            txt_ReceiptPrice.Text = money.ToString();
            txb_MoneyOwe.Text = (money - Convert.ToInt32(txb_CustomerPayment.Text)).ToString();
        }
        private void txb_CustomerPayment_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txb_MoneyOwe == null) return;
            int convertedPrice, PaymentPrice;
            if ((int.TryParse(txt_ReceiptPrice.Text, out convertedPrice)) && (int.TryParse(txb_CustomerPayment.Text, out PaymentPrice)))
            {
                if (PaymentPrice > convertedPrice)
                {
                    txb_CustomerPayment.Text = txt_ReceiptPrice.Text;
                    PaymentPrice = convertedPrice;
                }
                if (txb_MoneyOwe != null)
                {
                    txb_MoneyOwe.Text = (convertedPrice - PaymentPrice).ToString();
                }
                txb_MoneyOwe.Text = (convertedPrice - PaymentPrice).ToString();
            }
            else
            {
                txb_CustomerPayment.Text = "0";
            }
        }
        private void UpdateBaoCaoTon(SqlConnection connection, string BookID, int PhatSinhChanges, int TonCuoiChanges)
        {
            //Check if BAOCAOTON exists
            string sqlQuery = $"Select MaSach, Thang, Nam From BAOCAOTON WHERE THANG=@THANG AND NAM=@NAM AND MASACH=@MASACH";
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@MaSach", BookID);
            command.Parameters.AddWithValue("@Thang", DateTime.Now.Month.ToString());
            command.Parameters.AddWithValue("@Nam", DateTime.Now.Year.ToString());
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows) //if has row, just update.
            {
                reader.Close();
                sqlQuery = $"UPDATE BAOCAOTON SET PhatSinh+=@PhatSinh, TonCuoi+=@TonCuoi WHERE THANG=@THANG AND NAM=@NAM AND MASACH=@MASACH";
                command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@MaSach", BookID);
                command.Parameters.AddWithValue("@Thang", DateTime.Now.Month.ToString());
                command.Parameters.AddWithValue("@Nam", DateTime.Now.Year.ToString());
                command.Parameters.AddWithValue("@PhatSinh", PhatSinhChanges);
                command.Parameters.AddWithValue("@TonCuoi", TonCuoiChanges);
                reader = command.ExecuteReader();
                reader.Read();
                reader.Close();
            }
            else //if doesn't, check previous month (it is impossible not to have previous
                 //month since you always add books and BAOCAOTON at the same time
                 //create new 
            {
                reader.Close();
                int previousMonth = int.Parse(DateTime.Now.Month.ToString()) - 1;
                int previousYear = int.Parse(DateTime.Now.Year.ToString());

                // Handle January of the following year
                if (previousMonth == 0)
                {
                    previousMonth = 12;
                    previousYear--;
                }

                //Get TonCuoi from previous
                string thangTruoc = previousMonth.ToString();
                string namTruoc = previousYear.ToString();
                int TonCuoiTruoc = 0;
                sqlQuery = $"SELECT TonCuoi FROM BAOCAOTON WHERE THANG=@THANG AND NAM=@NAM AND MASACH=@MASACH";
                command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@MaSach", BookID);
                command.Parameters.AddWithValue("@Thang", thangTruoc);
                command.Parameters.AddWithValue("@Nam", namTruoc);
                reader = command.ExecuteReader();
                reader.Read();
                TonCuoiTruoc = Convert.ToInt32(reader["TonCuoi"]);
                reader.Close();

                //Create for this BAOCAOTON
                sqlQuery = "INSERT INTO BAOCAOTON (Thang, Nam, MaSach, TonDau, PhatSinh, TonCuoi) " +
              $"VALUES (@Thang, @Nam, @MaSach, @TonDau, @PhatSinh, @TonCuoi)";
                command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@Thang", DateTime.Now.Month.ToString());
                command.Parameters.AddWithValue("@Nam", DateTime.Now.Year.ToString());
                command.Parameters.AddWithValue("@MaSach", BookID);
                command.Parameters.AddWithValue("@TonDau", TonCuoiTruoc);
                command.Parameters.AddWithValue("@PhatSinh", PhatSinhChanges);
                command.Parameters.AddWithValue("@TonCuoi", TonCuoiTruoc + TonCuoiChanges);
                reader = command.ExecuteReader();
                reader.Read();
                reader.Close();
            }
            LoadAll(this);
        }

        public void UpdateBaoCaoCongNo(SqlConnection connection, string MaKH, int PhatSinhChanges, int NoCuoiChanges)
        {
            //Check if BAOCAOCONGNO exists
            string sqlQuery = $"Select MaKH, Thang, Nam From BAOCAOCONGNO WHERE THANG=@THANG AND NAM=@NAM AND MaKH=@MaKH";
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@MaKH", MaKH);
            command.Parameters.AddWithValue("@Thang", DateTime.Now.Month.ToString());
            command.Parameters.AddWithValue("@Nam", DateTime.Now.Year.ToString());
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows) //if has row, just update.
            {
                reader.Close();
                sqlQuery = $"UPDATE BAOCAOCONGNO SET PhatSinh+=@PhatSinh, NoCuoi+=@NoCuoi WHERE THANG=@THANG AND NAM=@NAM AND MaKH=@MaKH";
                command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@MaKH", MaKH);
                command.Parameters.AddWithValue("@Thang", DateTime.Now.Month.ToString());
                command.Parameters.AddWithValue("@Nam", DateTime.Now.Year.ToString());
                command.Parameters.AddWithValue("@PhatSinh", PhatSinhChanges);
                command.Parameters.AddWithValue("@NoCuoi", NoCuoiChanges);
                reader = command.ExecuteReader();
                reader.Read();
                reader.Close();
            }
            else //if doesn't, check previous month (it is impossible not to have previous
                 //month since you always add khachhang and BAOCAOCONGNO at the same time
                 //create new 
            {
                reader.Close();
                int previousMonth = int.Parse(DateTime.Now.Month.ToString()) - 1;
                int previousYear = int.Parse(DateTime.Now.Year.ToString());

                // Handle January of the following year
                if (previousMonth == 0)
                {
                    previousMonth = 12;
                    previousYear--;
                }

                //Get NoCuoi from previous
                string thangTruoc = previousMonth.ToString();
                string namTruoc = previousYear.ToString();
                int NoCuoiTruoc = 0;
                sqlQuery = $"SELECT NoCuoi FROM BAOCAOCONGNO WHERE THANG=@THANG AND NAM=@NAM AND MaKH=@MaKH";
                command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@MaKH", MaKH);
                command.Parameters.AddWithValue("@Thang", thangTruoc);
                command.Parameters.AddWithValue("@Nam", namTruoc);
                reader = command.ExecuteReader();
                reader.Read();
                NoCuoiTruoc = Convert.ToInt32(reader["NoCuoi"]);
                reader.Close();

                //Create for this BAOCAOCONGNO
                sqlQuery = "INSERT INTO BAOCAOCONGNO (Thang, Nam, MaKH, NoDau, PhatSinh, NoCuoi) " +
              $"VALUES (@Thang, @Nam, @MaKH, @NoDau, @PhatSinh, @NoCuoi)";
                command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@Thang", DateTime.Now.Month.ToString());
                command.Parameters.AddWithValue("@Nam", DateTime.Now.Year.ToString());
                command.Parameters.AddWithValue("@MaKH", MaKH);
                command.Parameters.AddWithValue("@NoDau", NoCuoiTruoc);
                command.Parameters.AddWithValue("@PhatSinh", PhatSinhChanges);
                command.Parameters.AddWithValue("@NoCuoi", NoCuoiTruoc + NoCuoiChanges);
                reader = command.ExecuteReader();
                reader.Read();
                reader.Close();
            }
            LoadAll(this);
        }
        private void btn_SaveBillToDatabase_Click(object sender, RoutedEventArgs e)
        {
            string NextExportId = GetNextExportPaperID(this);
            string MaKH = cbx_CustomerID.Text;
            if (MaKH == "Thêm mới")
                return;
            Connection connect = new Connection();
            string connectionString = connect.connection;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //check if the books can be sold first
                    foreach (Uct_Books book in BooksSell)
                    {
                        if (Convert.ToInt32(book.Amount) - book.BookSellAmount < Convert.ToInt32(SoLuongTonToiThieuSauBan))
                        {
                            Notification noti = new Notification("Vi phạm quy định", "Số lượng sách mã " + book.BookID + " sau khi bán bé hơn quy định: " + SoLuongTonToiThieuSauBan);
                            return;
                        }
                    }
                    //check if the customer can also handle the debt first
                    {
                        string query = $"SELECT SOTIENNO FROM KHACHHANG WHERE MaKH = @MaKH";
                        SqlCommand com = new SqlCommand(query, connection);
                        com.Parameters.AddWithValue("@MaKH", MaKH);
                        SqlDataReader read = com.ExecuteReader();
                        read.Read();
                        string SoTienNo = read["SoTienNo"].ToString(); // Assuming numeric data type
                        SoTienNo = SoTienNo.Substring(0, SoTienNo.Length - 5);
                        read.Close();
                        if (Convert.ToInt32(SoTienNo) + Convert.ToInt32(txb_MoneyOwe.Text) > Convert.ToInt32(SoTienNoToiDa))
                        {
                            Notification noti = new Notification("Vi phạm quy định", "Số nợ của khách " + MaKH + " sau khi bán lớn hơn quy định: " + SoTienNoToiDa);
                            return;
                        }
                    }
                    //Create The BillID first
                    string sqlQuery = $"INSERT INTO HOADON (MaHD, MaKH, NgayLap, TongTien, SoTienTra, ConLai) " +
                          $"VALUES (@MaHD, @MaKH, @NgayLap, @TongTien, @SoTienTra, @ConLai)";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaHD", NextExportId);
                    command.Parameters.AddWithValue("@MaKH", MaKH);
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

                        UpdateBaoCaoTon(connection, book.BookID, 0,-book.BookSellAmount);
                    }
                    //Update customer
                    sqlQuery = $"UPDATE KHACHHANG SET SoTienMua+=@SoTienMua WHERE MaKH=@MaKH";
                    command = new SqlCommand(sqlQuery, connection);

                    command.Parameters.AddWithValue("@SoTienMua", Convert.ToInt32(txt_ReceiptPrice.Text));
                    command.Parameters.AddWithValue("@MaKH", MaKH);
                    reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close(); ;

                    sqlQuery = $"UPDATE KHACHHANG SET SoTienNo+=@SoTienNo WHERE MaKH=@MaKH";
                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@SOTIENNO", Convert.ToInt32(txb_MoneyOwe.Text));
                    command.Parameters.AddWithValue("@MaKH", MaKH);
                    reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();

                    UpdateBaoCaoCongNo(connection, MaKH, Convert.ToInt32(txb_MoneyOwe.Text), Convert.ToInt32(txb_MoneyOwe.Text));

                    LoadAll(this);

                    BooksSell.Clear();

                    cbx_CustomerID.Text = "";
                    tbl_CustomerName.Text = "Tên khách hàng";
                    tbl_CustomerPhoneNumber.Text = "Số điện thoại";
                    tbl_CustomerEmail.Text = "Email";
                    txt_CustomerDateOfBirth.Text = "Ngày Sinh";
                    cbx_CustomerGender.Text = "";
                    tbl_CustomerDetailAdress.Text = "Địa Chỉ";

                    txt_ReceiptPrice.Text = "0";
                    txb_CustomerPayment.Text = "0";
                    txb_MoneyOwe.Text = "0";

                    Notification notification = new Notification("Tạo thành công!", "Tạo Hóa đơn với mã " + NextExportId + " thành công!"); ;
                }
            }
            catch (Exception ex)
            {
                Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi thêm vào HOADON: " + ex.Message);
            }
            LoadAll(this);
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
            btn_DeleteBill_Click(sender, e);
            
        }
        private void btn_DeleteBill_Click(object sender, RoutedEventArgs e)
        {
            if (!isExportPaperDelete) //Switch from normal to delete
            {
                dtg_BillList.Columns[0].Visibility = Visibility.Visible;
                btn_DeleteBill.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                isExportPaperDelete = true;
                LoadAll(this);
            }
            else
            {
                dtg_BillList.Columns[0].Visibility = Visibility.Hidden;
                isExportPaperDelete = false;
                btn_DeleteBill.Background = new SolidColorBrush(Colors.Transparent);

                LoadAll(this);
            }
            if (!isImportPaperDelete) //Switch from normal to delete
            {
                dtg_ImportBookReceiptList.Columns[0].Visibility = Visibility.Visible;
                btn_ImportBookReceiptDelete.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                isImportPaperDelete = true;
                LoadAll(this);
            }
            else
            {
                dtg_ImportBookReceiptList.Columns[0].Visibility = Visibility.Hidden;
                isImportPaperDelete = false;
                btn_ImportBookReceiptDelete.Background = new SolidColorBrush(Colors.Transparent);
                LoadAll(this);
            }
        }
        private void btn_DeleteImportBook_Click(object sender, RoutedEventArgs e)
        {
            selectedimportreceipt.Show();
        }
        private void cbx_SwapFromImportToExport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbx_SwapFromImportToExport.SelectedIndex == 0)
                return;

            if (!isExportPaperDelete)
            {
                dtg_BillList.Columns[0].Visibility = Visibility.Visible;
                dtg_ImportBookReceiptList.Columns[0].Visibility = Visibility.Visible;
                dtg_BillList.Columns[0].Visibility = Visibility.Hidden;
                dtg_ImportBookReceiptList.Columns[0].Visibility = Visibility.Hidden;
            }
            else
            {

                dtg_BillList.Columns[0].Visibility = Visibility.Hidden;
                dtg_ImportBookReceiptList.Columns[0].Visibility = Visibility.Hidden;
                dtg_BillList.Columns[0].Visibility = Visibility.Visible;
                dtg_ImportBookReceiptList.Columns[0].Visibility = Visibility.Visible;
            }

            cvs_ImportBookReceipt.Visibility = Visibility.Hidden;
            cvs_Bill.Visibility = Visibility.Visible;
            cbx_SwapFromImportToExport.SelectedIndex = 0;
        }
        private void cbx_SwapFromExportToImport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtg_ImportBookReceiptList == null) return;
            if (cbx_SwapFromExportToImport.SelectedIndex == 0)
                return;
            if (!isExportPaperDelete)
            {
                dtg_BillList.Columns[0].Visibility = Visibility.Visible;
                dtg_ImportBookReceiptList.Columns[0].Visibility = Visibility.Visible;
                dtg_BillList.Columns[0].Visibility = Visibility.Hidden;
                dtg_ImportBookReceiptList.Columns[0].Visibility = Visibility.Hidden;
            }
            else
            {

                dtg_BillList.Columns[0].Visibility = Visibility.Hidden;
                dtg_ImportBookReceiptList.Columns[0].Visibility = Visibility.Hidden;
                dtg_BillList.Columns[0].Visibility = Visibility.Visible;
                dtg_ImportBookReceiptList.Columns[0].Visibility = Visibility.Visible;
            }
            cvs_ImportBookReceipt.Visibility = Visibility.Visible;
            cvs_Bill.Visibility = Visibility.Hidden;
            cbx_SwapFromExportToImport.SelectedIndex = 0;
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
            lbl_BookCount.Content = "0";
            if (cvs_ImportBooks.Visibility == Visibility.Hidden)
            {
                if (isBookList)
                {
                    if (string.IsNullOrEmpty(txt_Search.Text))
                    {
                        dtg_Books.ItemsSource = Books; // Reset to all items if no search text
                        int amount = 0;
                        foreach (Uct_Books child in Books)
                        {
                            amount += Convert.ToInt32(child.Amount);
                        }
                        lbl_BookCount.Content = amount.ToString();
                        return;
                    }
                    if (Cbx_SearchBook.Text == "Tất Cả" || (Cbx_SearchBook.Text == null))
                    {
                        var filteredItems = Books.Where(book =>
                        book.BookID.ToLower().Contains(txt_Search.Text.ToLower()) ||
                            book.BookName.ToLower().Contains(txt_Search.Text.ToLower()) ||
                            book.BookGenre.ToLower().Contains(txt_Search.Text.ToLower()) ||
                            book.BookAuthor.ToLower().Contains(txt_Search.Text.ToLower())
                        ).ToList();

                        dtg_Books.ItemsSource = filteredItems;
                        int amount = 0;
                        foreach (Uct_Books child in filteredItems)
                        {
                            amount += Convert.ToInt32(child.Amount);
                        }
                        lbl_BookCount.Content = amount.ToString();
                    }
                    else if (Cbx_SearchBook.Text == "Tên Sách")
                    {
                        var filteredItems = Books.Where(book =>
                            book.BookName.ToLower().Contains(txt_Search.Text.ToLower())
                        ).ToList();

                        dtg_Books.ItemsSource = filteredItems;
                        int amount = 0;
                        foreach (Uct_Books child in filteredItems)
                        {
                            amount += Convert.ToInt32(child.Amount);
                        }
                        lbl_BookCount.Content = amount.ToString();
                    }
                    else if (Cbx_SearchBook.Text == "Thể Loại")
                    {
                        var filteredItems = Books.Where(book =>
                            book.BookGenre.ToLower().Contains(txt_Search.Text.ToLower())
                        ).ToList();

                        dtg_Books.ItemsSource = filteredItems;
                        int amount = 0;
                        foreach (Uct_Books child in filteredItems)
                        {
                            amount += Convert.ToInt32(child.Amount);
                        }
                        lbl_BookCount.Content = amount.ToString();
                    }
                    else if (Cbx_SearchBook.Text == "Tác Giả")
                    {
                        var filteredItems = Books.Where(book =>
                            book.BookAuthor.ToLower().Contains(txt_Search.Text.ToLower())
                        ).ToList();

                        dtg_Books.ItemsSource = filteredItems;
                        int amount = 0;
                        foreach (Uct_Books child in filteredItems)
                        {
                            amount += Convert.ToInt32(child.Amount);
                        }
                        lbl_BookCount.Content = amount.ToString();
                    }
                }
                else
                {
                    int amount = 0;
                    if (string.IsNullOrEmpty(txt_Search.Text))
                    {
                        foreach (Uct_Books child in wpn_Books.Children)
                        {
                            child.Visibility = Visibility.Visible;
                            amount += Convert.ToInt32(child.Amount);
                        }
                    }
                    else
                    {
                        if (Cbx_SearchBook.Text == "Tất Cả" || (Cbx_SearchBook.Text == null))
                        {
                            foreach (Uct_Books child in wpn_Books.Children)
                            {
                                if (child.BookName.ToLower().Contains(txt_Search.Text.ToLower())
                                    || child.BookAuthor.ToLower().Contains(txt_Search.Text.ToLower())
                                    || child.BookID.ToLower().Contains(txt_Search.Text.ToLower())
                                    || child.BookGenre.ToLower().Contains(txt_Search.Text.ToLower()))
                                {
                                    child.Visibility = Visibility.Visible;
                                    amount += Convert.ToInt32(child.Amount);
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
                                if (child.BookName.ToLower().Contains(txt_Search.Text.ToLower()))
                                {
                                    child.Visibility = Visibility.Visible;
                                    amount += Convert.ToInt32(child.Amount);
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
                                if (child.BookGenre.ToLower().Contains(txt_Search.Text.ToLower()))
                                {
                                    child.Visibility = Visibility.Visible;
                                    amount += Convert.ToInt32(child.Amount);
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
                                if (child.BookAuthor.ToLower().Contains(txt_Search.Text.ToLower()))
                                {
                                    child.Visibility = Visibility.Visible;
                                    amount += Convert.ToInt32(child.Amount);
                                }
                                else
                                {
                                    child.Visibility = Visibility.Collapsed;
                                }
                            }
                        }
                    }
                    lbl_BookCount.Content = amount;
                }
            }
            else
            {
                if (isBookList)
                {
                    if (string.IsNullOrEmpty(txt_Search.Text))
                    {
                        dtg_ImportBooks.ItemsSource = Books; // Reset to all items if no search text
                        int amount = 0;
                        foreach (Uct_Books child in Books)
                        {
                            amount += Convert.ToInt32(child.Amount);
                        }
                        lbl_BookCount.Content = amount.ToString();
                        return;
                    }
                    if (Cbx_SearchBook.Text == "Tất Cả" || (Cbx_SearchBook.Text == null))
                    {
                        var filteredItems = Books.Where(book =>
                        book.BookID.ToLower().Contains(txt_Search.Text.ToLower()) ||
                            book.BookName.ToLower().Contains(txt_Search.Text.ToLower()) ||
                            book.BookGenre.ToLower().Contains(txt_Search.Text.ToLower()) ||
                            book.BookAuthor.ToLower().Contains(txt_Search.Text.ToLower())
                        ).ToList();

                        dtg_ImportBooks.ItemsSource = filteredItems;
                        int amount = 0;
                        foreach (Uct_Books child in filteredItems)
                        {
                            amount += Convert.ToInt32(child.Amount);
                        }
                        lbl_BookCount.Content = amount.ToString();
                    }
                    else if (Cbx_SearchBook.Text == "Tên Sách")
                    {
                        var filteredItems = Books.Where(book =>
                            book.BookName.ToLower().Contains(txt_Search.Text.ToLower())
                        ).ToList();

                        dtg_ImportBooks.ItemsSource = filteredItems;
                        int amount = 0;
                        foreach (Uct_Books child in filteredItems)
                        {
                            amount += Convert.ToInt32(child.Amount);
                        }
                        lbl_BookCount.Content = amount.ToString();
                    }
                    else if (Cbx_SearchBook.Text == "Thể Loại")
                    {
                        var filteredItems = Books.Where(book =>
                            book.BookGenre.ToLower().Contains(txt_Search.Text.ToLower())
                        ).ToList();

                        dtg_ImportBooks.ItemsSource = filteredItems;
                        int amount = 0;
                        foreach (Uct_Books child in filteredItems)
                        {
                            amount += Convert.ToInt32(child.Amount);
                        }
                        lbl_BookCount.Content = amount.ToString();
                    }
                    else if (Cbx_SearchBook.Text == "Tác Giả")
                    {
                        var filteredItems = Books.Where(book =>
                            book.BookAuthor.ToLower().Contains(txt_Search.Text.ToLower())
                        ).ToList();

                        dtg_ImportBooks.ItemsSource = filteredItems;
                        int amount = 0;
                        foreach (Uct_Books child in filteredItems)
                        {
                            amount += Convert.ToInt32(child.Amount);
                        }
                        lbl_BookCount.Content = amount.ToString();
                    }
                }
                else
                {
                    int amount = 0;
                    if (string.IsNullOrEmpty(txt_Search.Text))
                    {
                        foreach (Uct_Books child in wpn_ImportBooks.Children)
                        {
                            child.Visibility = Visibility.Visible;
                            amount += Convert.ToInt32(child.Amount);
                        }
                    }
                    else
                    {
                        if (Cbx_SearchBook.Text == "Tất Cả" || (Cbx_SearchBook.Text == null))
                        {
                            foreach (Uct_Books child in wpn_ImportBooks.Children)
                            {
                                if (child.BookName.ToLower().Contains(txt_Search.Text.ToLower())
                                    || child.BookAuthor.ToLower().Contains(txt_Search.Text.ToLower())
                                    || child.BookID.ToLower().Contains(txt_Search.Text.ToLower())
                                    || child.BookGenre.ToLower().Contains(txt_Search.Text.ToLower()))
                                {
                                    child.Visibility = Visibility.Visible;
                                    amount += Convert.ToInt32(child.Amount);
                                }
                                else
                                {
                                    child.Visibility = Visibility.Collapsed;
                                }
                            }
                        }
                        else if (Cbx_SearchBook.Text == "Tên Sách")
                        {
                            foreach (Uct_Books child in wpn_ImportBooks.Children)
                            {
                                if (child.BookName.ToLower().Contains(txt_Search.Text.ToLower()))
                                {
                                    child.Visibility = Visibility.Visible;
                                    amount += Convert.ToInt32(child.Amount);
                                }
                                else
                                {
                                    child.Visibility = Visibility.Collapsed;
                                }
                            }
                        }
                        else if (Cbx_SearchBook.Text == "Thể Loại")
                        {
                            foreach (Uct_Books child in wpn_ImportBooks.Children)
                            {
                                if (child.BookGenre.ToLower().Contains(txt_Search.Text.ToLower()))
                                {
                                    child.Visibility = Visibility.Visible;
                                    amount += Convert.ToInt32(child.Amount);
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
                                if (child.BookAuthor.ToLower().Contains(txt_Search.Text.ToLower()))
                                {
                                    child.Visibility = Visibility.Visible;
                                    amount += Convert.ToInt32(child.Amount);
                                }
                                else
                                {
                                    child.Visibility = Visibility.Collapsed;
                                }
                            }
                        }
                    }
                    lbl_BookCount.Content = amount;
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
                    txt_ImportID.Text = GetNextImportPaperID(this);


                    dtg_ImportBooks.Items.Refresh();
                    dtg_ImportBooks.Columns[0].Visibility = Visibility.Hidden;
                    dtg_ImportBooks.Columns[0].Visibility = Visibility.Visible;

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
                    txt_ImportID.Text = GetNextImportPaperID(this);

                    dtg_ImportBooks.Items.Refresh();

                    LoadAll(this);
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
                    LoadAll(this);
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
                bookimport.UpdateMoney();
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

                    //set this visible
                    cvs_BooksGridList.Visibility = Visibility.Visible;

                    //button color
                    btn_SwitchView.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                    
                    LoadAll(this);
                    isBookList = false;
                }
                else //Swap From Table To List (Default)
                {
                    cvs_BooksDataGridList.Visibility = Visibility.Visible;

                    cvs_BooksGridList.Visibility = Visibility.Hidden;
                    cvs_ImportBooks.Visibility = Visibility.Hidden;

                    dtg_Books.Items.Refresh();
                    btn_SwitchView.Background = new SolidColorBrush(Colors.Transparent);

                    LoadAll(this);

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
            LoadAll(this);
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
            LoadAll(this);
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
                //check if the book can be imported
                foreach (Uct_BookImport child in wpn_ImportPaper.Children.OfType<Uct_BookImport>())
                {
                    try
                    {
                        string sqlQuery = "SELECT SoLuongTon FROM SACH WHERE MaSach = @MaSach";
                        SqlCommand command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@MaSach", child.BookID);
                        SqlDataReader reader = command.ExecuteReader();
                        reader.Read();
                        int Amount = Convert.ToInt32(reader["SoLuongTon"]);
                        reader.Close();
                        if (Amount > Convert.ToInt32(SoLuongTonToiDaTruocNhap))
                        {
                            Notification noti = new Notification("Vi phạm quy định", "Số lượng sách được nhập cho " + child.BookID + " lớn hơn quy định: " + SoLuongTonToiDaTruocNhap);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi lấy thông tin từ SACH: " + ex.Message);
                    }
                }
                //Checking if the imported amount of book fit the requirement
                foreach (Uct_BookImport child in wpn_ImportPaper.Children.OfType<Uct_BookImport>())
                {
                    if (Convert.ToInt32(child.BookQuantity) < Convert.ToInt32(SoLuongNhapToiThieu))
                    {
                        Notification noti = new Notification("Vi phạm quy định", "Số lượng sách được nhập cho " + child.BookID + " bé hơn quy định: " + SoLuongNhapToiThieu);
                        return;
                    }
                }
                //Actually Importing:
                foreach (Uct_BookImport child in wpn_ImportPaper.Children.OfType<Uct_BookImport>())
                {
                    try
                    {
                        //Update The Actual Book
                        string sqlQuery = "UPDATE SACH SET SOLUONGTON+=@SOLUONGTON WHERE MaSach = @MaSach";
                        SqlCommand command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@SOLUONGTON", Convert.ToInt32(child.BookQuantity));
                        command.Parameters.AddWithValue("@MaSach", child.BookID);
                        SqlDataReader reader = command.ExecuteReader();
                        reader.Read();
                        reader.Close();

                        UpdateBaoCaoTon(connection, child.BookID, Convert.ToInt32(child.BookQuantity), Convert.ToInt32(child.BookQuantity));
                    }
                    catch (Exception ex)
                    {
                        Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi lấy thông tin từ SACH: " + ex.Message);
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
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi thêm vào PHIEUNHAP: " + ex.Message);
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
                        command.Parameters.AddWithValue("@DonGiaNhap", Convert.ToInt32(child.BookImportPrice));
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

                        Notification noti = new Notification("Thành Công", "Thêm phiếu nhập thành công");
                    }
                    catch (Exception ex)
                    {
                        Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi thêm vào PHIEUNHAP: " + ex.Message);
                    }
                }
                wpn_ImportPaper.Children.Clear();
                LoadAll(this);
                LoadBook(this, 3);
                tbl_SumImportMoney.Text = "0";
                txt_ImportID.Text = GetNextImportPaperID(this);
                ImportDate.Text = DateTime.Now.ToString();
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
            if (!isBookUpdate && !isBookDelete && selectedbook != null)
            {
                BookInfoPopup bookInfoPopup = new BookInfoPopup(selectedbook);
                bookInfoPopup.Show();
            }
        }
        private void btn_EmployeeAdd_Click(object sender, RoutedEventArgs e)
        {
            EmployeeAdd employee = new EmployeeAdd(this);
            employee.Show();
        }
        private void btn_EmployeeUpdate_Click(object sender, RoutedEventArgs e)
        {
            isEmployeeDelete = false;
            if (!isEmployeeUpdate) //Switch to Update
            {
                btn_EmployeeUpdate.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                btn_EmployeeDelete.Background = new SolidColorBrush(Colors.Transparent);
                btn_EmployeeAdd.Background = new SolidColorBrush(Colors.Transparent);
                isEmployeeUpdate = true;
                LoadAll(this);
            }
            else //Turn off update
            {
                isEmployeeUpdate = false;
                btn_EmployeeUpdate.Background = new SolidColorBrush(Colors.Transparent);
                LoadAll(this);
            }
        }
        private void btn_EmployeeDelete_Click(object sender, RoutedEventArgs e)
        {
            isEmployeeUpdate = false;
            if (!isEmployeeDelete) //Swap to Delete
            {
                btn_EmployeeDelete.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                btn_EmployeeUpdate.Background = new SolidColorBrush(Colors.Transparent);
                btn_EmployeeAdd.Background = new SolidColorBrush(Colors.Transparent);
                isEmployeeDelete = true;
                LoadAll(this);
            }
            else //Turn off delete
            {
                isEmployeeDelete = false;
                btn_EmployeeDelete.Background = new SolidColorBrush(Colors.Transparent);
                LoadAll(this);
            }
        }

        private void txt_EmployeeSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (cbx_EmployeeSearch.SelectedIndex == 0)
            {
                foreach (Uct_Employee child in wpn_Employee.Children)
                {
                    try
                    {
                        if (child is Uct_Employee)
                        {
                            if (child.EmployeeName.ToLower().Contains(txt_EmployeeSearch.Text.ToLower()))
                            {
                                child.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                child.Visibility = Visibility.Collapsed;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi lấy thông tin từ TACGIA: " + ex.Message);
                    }
                }
            }
            else
            {
                foreach (Uct_Employee child in wpn_Employee.Children)
                {
                    try
                    {
                        if (child is Uct_Employee)
                        {
                            if (child.EmployeePhonenumber.ToLower().Contains(txt_EmployeeSearch.Text.ToLower()))
                            {
                                child.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                child.Visibility = Visibility.Collapsed;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi lấy thông tin từ TACGIA: " + ex.Message);
                    }
                }
            }
        }

        private void CustomerSearchComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void cbx_EmployeeSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbx_EmployeeSearch.SelectedItem == null) return;
        }

        private void btn_SaveBillAsPDF_Click(object sender, RoutedEventArgs e)
        {
            foreach (Uct_Customer child in wpn_Customer.Children)
            {
                if (child is Uct_Customer)
                {
                    if (child.CustomerName.Contains(txt_CustomerSearch.Text) || child.CustomerPhonenumber.Contains(txt_CustomerSearch.Text))
                    {
                        child.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        child.Visibility = Visibility.Collapsed;
                    }
                }
                Bills[Bills.Count - 1].Show();
            }
        }
        //Author
        private void SwapGenreAuthor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SwapGenreAuthor.SelectedIndex == 0) return;
            SwapGenreAuthor.SelectedIndex = 1;
            if (dtg_AuthorList.Columns[0].Visibility == Visibility.Hidden)
            {
                dtg_AuthorList.Columns[0].Visibility = Visibility.Visible;
                dtg_AuthorList.Columns[0].Visibility = Visibility.Hidden;
            }
            else
            {
                dtg_AuthorList.Columns[0].Visibility = Visibility.Hidden;
                dtg_AuthorList.Columns[0].Visibility = Visibility.Visible;
            }

            if (SwapGenreAuthor.SelectedItem == null)
                return;

            cvs_Author.Visibility = Visibility.Visible;
            cvs_Genre.Visibility = Visibility.Hidden;

            SwapGenreAuthor.SelectedIndex = 0;
            txt_AuthorGenreSearch.Text = "";
        }

        private void btn_EditAuthor_Click(object sender, RoutedEventArgs e)
        {
            dtg_AuthorList.Columns[1].Visibility = Visibility.Hidden;
            btn_DeleteAuthorList.Background = new SolidColorBrush(Colors.Transparent);
            if (dtg_AuthorList.Columns[0].Visibility == Visibility.Hidden)
            {
                dtg_AuthorList.Columns[0].Visibility = Visibility.Visible;
                btn_EditAuthor.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
            }
            else
            {
                dtg_AuthorList.Columns[0].Visibility = Visibility.Hidden;
                btn_EditAuthor.Background = new SolidColorBrush(Colors.Transparent);
            }
            dtg_GenreList.Columns[1].Visibility = Visibility.Hidden;
            btn_DeleteGenreMode.Background = new SolidColorBrush(Colors.Transparent);
            if (dtg_GenreList.Columns[0].Visibility == Visibility.Hidden)
            {
                dtg_GenreList.Columns[0].Visibility = Visibility.Visible;
                btn_EditGenre.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
            }
            else
            {
                dtg_GenreList.Columns[0].Visibility = Visibility.Hidden;
                btn_EditGenre.Background = new SolidColorBrush(Colors.Transparent);
            }
        }

        private void btn_DeleteAuthor_Click(object sender, RoutedEventArgs e)
        {
            if (selectedauthor == null) return;
            AuthorAdd genreadd = new AuthorAdd(this, 2, selectedauthor);
            genreadd.Show();
        }

        private void btn_UpdateAuthor_Click(object sender, RoutedEventArgs e)
        {
            if (selectedauthor == null) return;
            AuthorAdd authoradd = new AuthorAdd(this, 1, selectedauthor);
            authoradd.Show();
        }

        private void btn_DeleteAuthorList_Click(object sender, RoutedEventArgs e)
        {
            btn_DeleteGenreMode_Click(sender, e);
        }

        private void btn_AddAuthor_Click(object sender, RoutedEventArgs e)
        {
            dtg_AuthorList.Columns[0].Visibility = Visibility.Hidden;
            dtg_AuthorList.Columns[1].Visibility = Visibility.Hidden;
            btn_DeleteAuthorList.Background = new SolidColorBrush(Colors.Transparent);
            btn_EditAuthor.Background = new SolidColorBrush(Colors.Transparent); 
            
            dtg_GenreList.Columns[0].Visibility = Visibility.Hidden;
            dtg_GenreList.Columns[1].Visibility = Visibility.Hidden;
            btn_DeleteGenreMode.Background = new SolidColorBrush(Colors.Transparent);
            btn_EditGenre.Background = new SolidColorBrush(Colors.Transparent);

            AuthorAdd authoradd = new AuthorAdd(this, 0, selectedauthor);
            authoradd.Show();
        }
        private void dtg_AuthorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            selectedauthor = (Uct_Author)dtg_AuthorList.SelectedItem;
        }

        //Genre
        private void cbx_SwitchToGenre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbx_SwitchToGenre.SelectedIndex == 0) return;

            if (dtg_GenreList.Columns[0].Visibility == Visibility.Visible)
            {
                dtg_GenreList.Columns[0].Visibility = Visibility.Hidden;
                dtg_GenreList.Columns[0].Visibility = Visibility.Visible;
            }
            else
            {
                dtg_GenreList.Columns[0].Visibility = Visibility.Visible;
                dtg_GenreList.Columns[0].Visibility = Visibility.Hidden;
            }

            if (cbx_SwitchToGenre.SelectedItem == null)
                return;

            cvs_Author.Visibility = Visibility.Hidden;
            cvs_Genre.Visibility = Visibility.Visible;

            cbx_SwitchToGenre.SelectedIndex = 0;
            txt_AuthorGenreSearch.Text = "";
        }

        private void btn_EditGenre_Click(object sender, RoutedEventArgs e)
        {
            btn_EditAuthor_Click(sender, e);
        }

        private void btn_DeleteGenre_Click(object sender, RoutedEventArgs e)
        {
            if (selectedgenre == null) return;
            GenreAdd genreadd = new GenreAdd(this, 2, selectedgenre);
            genreadd.Show();
        }

        private void btn_UpdateGenre_Click(object sender, RoutedEventArgs e)
        {
            if (selectedgenre == null) return;
            GenreAdd genreadd = new GenreAdd(this, 1, selectedgenre);
            genreadd.Show();
        }

        private void btn_DeleteGenreMode_Click(object sender, RoutedEventArgs e)
        {
            dtg_GenreList.Columns[0].Visibility = Visibility.Hidden;
            btn_EditGenre.Background = new SolidColorBrush(Colors.Transparent);
            if (dtg_GenreList.Columns[1].Visibility == Visibility.Hidden)
            {
                dtg_GenreList.Columns[1].Visibility = Visibility.Visible;
                btn_DeleteGenreMode.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
            }
            else
            {
                dtg_GenreList.Columns[1].Visibility = Visibility.Hidden;
                btn_DeleteGenreMode.Background = new SolidColorBrush(Colors.Transparent);
            }
            dtg_AuthorList.Columns[0].Visibility = Visibility.Hidden;
            btn_EditAuthor.Background = new SolidColorBrush(Colors.Transparent);
            if (dtg_AuthorList.Columns[1].Visibility == Visibility.Hidden)
            {
                dtg_AuthorList.Columns[1].Visibility = Visibility.Visible;
                btn_DeleteAuthorList.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
            }
            else
            {
                dtg_AuthorList.Columns[1].Visibility = Visibility.Hidden;
                btn_DeleteAuthorList.Background = new SolidColorBrush(Colors.Transparent);
            }
        }

        private void btn_AddGenre_Click(object sender, RoutedEventArgs e)
        {
            dtg_AuthorList.Columns[0].Visibility = Visibility.Hidden;
            dtg_AuthorList.Columns[1].Visibility = Visibility.Hidden;
            btn_DeleteAuthorList.Background = new SolidColorBrush(Colors.Transparent);
            btn_EditAuthor.Background = new SolidColorBrush(Colors.Transparent);

            dtg_GenreList.Columns[0].Visibility = Visibility.Hidden;
            dtg_GenreList.Columns[1].Visibility = Visibility.Hidden;
            btn_DeleteGenreMode.Background = new SolidColorBrush(Colors.Transparent);
            btn_EditGenre.Background = new SolidColorBrush(Colors.Transparent);

            GenreAdd genreadd = new GenreAdd(this, 0, selectedgenre);
            genreadd.Show();
        }

        private void dtg_GenreList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedgenre = (Uct_Genre)dtg_GenreList.SelectedItem;
        }

        private void txt_AuthorGenreSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txt_AuthorGenreSearch.Text))
            {
                dtg_GenreList.ItemsSource = Genres;
                dtg_AuthorList.ItemsSource = Authors;
            }
            else 
            {
                var filteredItems = Genres.Where(genres =>
                genres.GenreID.ToLower().Contains(txt_AuthorGenreSearch.Text.ToLower()) ||
                    genres.GenreName.ToLower().Contains(txt_AuthorGenreSearch.Text.ToLower())
                ).ToList();

                dtg_GenreList.ItemsSource = filteredItems;

                var filteredItems2 = Authors.Where(author =>
                author.AuthorID.ToLower().Contains(txt_AuthorGenreSearch.Text.ToLower()) ||
                    author.AuthorName.ToLower().Contains(txt_AuthorGenreSearch.Text.ToLower())
                ).ToList();

                dtg_AuthorList.ItemsSource = filteredItems2;
            }
            dtg_GenreList.Items.Refresh();
            dtg_AuthorList.Items.Refresh();
        }

        private void btn_UpdateThamSo_Click(object sender, RoutedEventArgs e)
        {
            Connection connect = new Connection();
            string connectionString = connect.connection;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery; SqlCommand command; SqlDataReader reader;
                    connection.Open();

                    //ApDungQuyDinhKiemTraSoTienThu
                    sqlQuery = $"UPDATE THAMSO SET GiaTri = @GiaTri WHERE TenThamSo='ApDungQuyDinhKiemTraSoTienThu'";
                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@GiaTri", cbx_CheckMoneyReceivedFromCustomer.IsChecked ?? false ? "1" : "0");
                    reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();

                    //SoLuongNhapToiThieu
                    sqlQuery = $"UPDATE THAMSO SET GiaTri = @GiaTri WHERE TenThamSo='SoLuongNhapToiThieu'";
                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@GiaTri", tbx_BookMinimumImportQuantity.Text);
                    reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();

                    //SoLuongTonToiDaTruocNhap
                    sqlQuery = $"UPDATE THAMSO SET GiaTri = @GiaTri WHERE TenThamSo='SoLuongTonToiDaTruocNhap'";
                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@GiaTri", tbx_BookMaximumStockQuantityBeforeImporting.Text);
                    reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();

                    //SoLuongTonToiThieuSauBan
                    sqlQuery = $"UPDATE THAMSO SET GiaTri = @GiaTri WHERE TenThamSo='SoLuongTonToiThieuSauBan'";
                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@GiaTri", tbx_BookMinimumStockQuantityAfterSelling.Text);
                    reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();

                    //SoTienNoToiDa
                    sqlQuery = $"UPDATE THAMSO SET GiaTri = @GiaTri WHERE TenThamSo='SoTienNoToiDa'";
                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@GiaTri", tbx_CustomerMaximumDebt.Text);
                    reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();

                    //TiLeGiaBan
                    sqlQuery = $"UPDATE THAMSO SET GiaTri = @GiaTri WHERE TenThamSo='TiLeGiaBan'";
                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@GiaTri", tbx_ImportToExportRatio.Text);
                    reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();

                    //Gia Ban Sach
                    sqlQuery = $"UPDATE SACH SET DonGiaBan=DonGiaNhap*@GiaTri/100";
                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@GiaTri", tbx_ImportToExportRatio.Text);
                    reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();

                    Notification noti2 = new Notification("Thành Công", "Cập nhật THAMSO thành công!");
                    LoadAll(this);
                }
            }
            catch (Exception ex)
            {
                Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi khi cập nhật ThamSo: " + ex.Message);
            }
        }

        private void txt_ReportSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txt_ReportSearch.Text))
            {
                dtg_AmountReport.ItemsSource = BaoCaoTon; // Reset to all items if no search text
                dtg_CustomerDebtReport.ItemsSource = BaoCaoCongNo;
                return;
            }
            if (cbx_SearchAmountCustomerReport.Text == "Tất Cả" || (cbx_SearchAmountCustomerReport.Text == null))
            {
                var filteredItems = BaoCaoTon.Where(baocao =>
                baocao.BookId.ToLower().Contains(txt_ReportSearch.Text.ToLower()) ||
                    baocao.BookName.ToLower().Contains(txt_ReportSearch.Text.ToLower())
                ).ToList();

                dtg_AmountReport.ItemsSource = filteredItems;

                var filteredItems2 = BaoCaoCongNo.Where(baocao =>
                baocao.CustomerID.ToLower().Contains(txt_ReportSearch.Text.ToLower()) ||
                    baocao.CustomerName.ToLower().Contains(txt_ReportSearch.Text.ToLower())
                ).ToList();

                dtg_CustomerDebtReport.ItemsSource = filteredItems2;
            }
            else if(cbx_SearchAmountCustomerReport.SelectedIndex==1)
            {
                var filteredItems = BaoCaoTon.Where(baocao =>
                    baocao.BookId.ToLower().Contains(txt_ReportSearch.Text.ToLower())
                ).ToList();

                dtg_AmountReport.ItemsSource = filteredItems;

                var filteredItems2 = BaoCaoCongNo.Where(baocao =>
                baocao.CustomerID.ToLower().Contains(txt_ReportSearch.Text.ToLower())
                ).ToList();

                dtg_CustomerDebtReport.ItemsSource = filteredItems2;
            }
            else if (cbx_SearchAmountCustomerReport.SelectedIndex == 2)
            {
                var filteredItems = BaoCaoTon.Where(baocao =>
                    baocao.BookName.ToLower().Contains(txt_ReportSearch.Text.ToLower())
                ).ToList();

                dtg_AmountReport.ItemsSource = filteredItems;

                var filteredItems2 = BaoCaoCongNo.Where(baocao =>
                    baocao.CustomerName.ToLower().Contains(txt_ReportSearch.Text.ToLower())
                ).ToList();

                dtg_CustomerDebtReport.ItemsSource = filteredItems2;
            }

        }

        private void cbx_AmountReportMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbx_AmountReportMonth.SelectedIndex == 0)
            {
                dtg_AmountReport.ItemsSource = BaoCaoTon;
            }
            else
            {
                var filteredItems = BaoCaoTon.Where(baocao =>
                baocao.Month == Convert.ToInt32(cbx_AmountReportMonth.SelectedItem)
                ).ToList();
                if (cbx_AmountReportYear.SelectedIndex == 0) { }
                else
                {
                    filteredItems = filteredItems.Where(baocao =>
                baocao.Year == Convert.ToInt32(cbx_AmountReportYear.SelectedItem)
                ).ToList();
                }
                dtg_AmountReport.ItemsSource = filteredItems;
            }
        }

        private void cbx_AmountReportYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbx_AmountReportYear.SelectedIndex == 0)
            {
                dtg_AmountReport.ItemsSource = BaoCaoTon;
            }
            else
            {
                var filteredItems = BaoCaoTon.Where(baocao =>
                baocao.Year == Convert.ToInt32(cbx_AmountReportYear.SelectedItem)
                ).ToList();
                if (cbx_AmountReportMonth.SelectedIndex == 0) { }
                else
                {
                    filteredItems = filteredItems.Where(baocao =>
                baocao.Month == Convert.ToInt32(cbx_AmountReportMonth.SelectedItem)
                ).ToList();
                }
                dtg_AmountReport.ItemsSource = filteredItems;
            }
        }

        private void btn_ReceiptAdd_Click(object sender, RoutedEventArgs e)
        {
            btn_ReceiptAdd.Background = new SolidColorBrush(Colors.Transparent);
            isCustomerReceiptDelete = false;
            ReceiptAdd receipt = new ReceiptAdd(this);
            receipt.Show();
        }

        private void dtg_CustomerPaymentReceipt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isCustomerReceiptDelete) return;
            selectedcustomerreceipt = (Uct_CustomerReceipt)dtg_CustomerPaymentReceipt.SelectedItem;
        }

        private void dtg_CustomerPaymentReceipt_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isCustomerReceiptDelete) return;
            if ((selectedcustomerreceipt != null) && (!isCustomerReceiptDelete))
            {
                ReceiptInfo receiptinfo = new ReceiptInfo(selectedcustomerreceipt);
                receiptinfo.Show();
            }
        }

        private void btn_DeleteReceipt_Click(object sender, RoutedEventArgs e)
        {
            if(!isCustomerReceiptDelete)
            {
                btn_ReceiptDelete.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                isCustomerReceiptDelete = true;
                dtg_CustomerPaymentReceipt.Columns[0].Visibility = Visibility.Visible;
            }
            else
            {
                btn_ReceiptDelete.Background = new SolidColorBrush(Colors.Transparent);
                isCustomerReceiptDelete = false;
                dtg_CustomerPaymentReceipt.Columns[0].Visibility = Visibility.Hidden;
            }
        }

        private void btn_DeleteCustomerReceipt_Click(object sender, RoutedEventArgs e)
        {
            if (selectedcustomerreceipt != null)
            {
                ReceiptDelete receiptinfo = new ReceiptDelete(selectedcustomerreceipt, this);
                receiptinfo.Show();
            }
        }

        private void txt_ReceiptSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txt_ReceiptSearch.Text))
            {
                dtg_CustomerPaymentReceipt.ItemsSource = CustomerReceipt; // Reset to all items if no search text
                return;
            }
            if (cbx_ReceiptSearch.SelectedIndex==0)
            {
                var filteredItems = CustomerReceipt.Where(receipt =>
                    (receipt.CustomerName.ToLower().Contains(txt_ReceiptSearch.Text.ToLower())||
                    (receipt.CustomerID.ToLower().Contains(txt_ReceiptSearch.Text.ToLower()))
                )).ToList();

                dtg_CustomerPaymentReceipt.ItemsSource = filteredItems;
            }
            else if (cbx_ReceiptSearch.SelectedIndex == 1)
            {
                var filteredItems = CustomerReceipt.Where(receipt =>
                    receipt.CustomerID.ToLower().Contains(txt_ReceiptSearch.Text.ToLower())
                ).ToList();

                dtg_CustomerPaymentReceipt.ItemsSource = filteredItems;
            }
            else
            {
                var filteredItems = CustomerReceipt.Where(receipt =>
                    receipt.CustomerName.ToLower().Contains(txt_ReceiptSearch.Text.ToLower())
                ).ToList();

                dtg_CustomerPaymentReceipt.ItemsSource = filteredItems;
            }
        }

        private void txt_CustomerSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            foreach (Uct_Customer child in wpn_Customer.Children)
            {
                child.Visibility = Visibility.Collapsed;
                try
                {
                    if (cbx_CustomerSearch.SelectedIndex == 0) // Search by Name or Phone Number
                    {
                        if (child.CustomerName.ToLower().Contains(txt_CustomerSearch.Text.ToLower()) ||
                            child.CustomerPhonenumber.ToLower().Contains(txt_CustomerSearch.Text.ToLower())||
                            child.CustomerID.ToLower().Contains(txt_CustomerSearch.Text.ToLower()))
                        {
                            child.Visibility = Visibility.Visible;
                        }
                    }
                    else if (cbx_CustomerSearch.SelectedIndex == 1) // Search by Name only
                    {
                        if (child.CustomerID.ToLower().Contains(txt_CustomerSearch.Text.ToLower()))
                        {
                            child.Visibility = Visibility.Visible;
                        }
                    }
                    else if (cbx_CustomerSearch.SelectedIndex == 2) // Search by Name only
                    {
                        if (child.CustomerName.ToLower().Contains(txt_CustomerSearch.Text.ToLower()))
                        {
                            child.Visibility = Visibility.Visible;
                        }
                    }
                    else // Search by Phone Number only (assuming cbx_CustomerSearch.SelectedIndex == 2)
                    {
                        if (child.CustomerPhonenumber.ToLower().Contains(txt_CustomerSearch.Text.ToLower()))
                        {
                            child.Visibility = Visibility.Visible;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi lấy thông tin từ KHACHHANG: " + ex.Message);
                }
            }
        }

        private void btn_DeleteCustomerReceipt_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void btn_AddCustomerFromSettingSection_Click(object sender, RoutedEventArgs e)
        {
            CustomerAdd customer = new CustomerAdd(this);
            customer.Show();
        }

        private void btn_AddEmployeeFromSettingSection_Click(object sender, RoutedEventArgs e)
        {
            EmployeeAdd employee = new EmployeeAdd(this);
            employee.Show();
        }

        private void btn_AddBookFromSettingSection_Click(object sender, RoutedEventArgs e)
        {
            BookAddPopup bookaddpopup = new BookAddPopup(this);
            bookaddpopup.Show();
        }

        private void btn_AddGenreFromSettingSection_Click(object sender, RoutedEventArgs e)
        {
            GenreAdd genreadd = new GenreAdd(this, 0, selectedgenre);
            genreadd.Show();
        }

        private void btn_AddAuthorFromSettingSection_Click(object sender, RoutedEventArgs e)
        {
            AuthorAdd authoradd = new AuthorAdd(this, 0, selectedauthor);
            authoradd.Show();
        }

        private void cbx_SwapReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cvs_AmountReport == null) return;
            if (cvs_CustomerDebtReport == null) return;
            if(cbx_SwapReport.SelectedIndex==1)
            {
                cvs_AmountReport.Visibility = Visibility.Hidden;
                cvs_CustomerDebtReport.Visibility = Visibility.Visible;
                dtg_CustomerDebtReport.Columns[0].Visibility = Visibility.Hidden;
                dtg_CustomerDebtReport.Columns[0].Visibility = Visibility.Visible;
                cbx_SearchAmountCustomerReport.Items.Clear();
                cbx_SearchAmountCustomerReport.Items.Add("Tất Cả");
                cbx_SearchAmountCustomerReport.Items.Add("Mã KH");
                cbx_SearchAmountCustomerReport.Items.Add("Tên KH");
                cbx_SearchAmountCustomerReport.SelectedIndex = 0;
                txt_ReportSearch.Text = "";
            }
            else
            {
                cvs_AmountReport.Visibility = Visibility.Visible;
                cvs_CustomerDebtReport.Visibility = Visibility.Hidden;
                dtg_AmountReport.Columns[0].Visibility= Visibility.Hidden;
                dtg_AmountReport.Columns[0].Visibility = Visibility.Visible;
                cbx_SearchAmountCustomerReport.Items.Clear();
                cbx_SearchAmountCustomerReport.Items.Add("Tất Cả");
                cbx_SearchAmountCustomerReport.Items.Add("Mã Sách");
                cbx_SearchAmountCustomerReport.Items.Add("Tên Sách");
                cbx_SearchAmountCustomerReport.SelectedIndex = 0;
                txt_ReportSearch.Text = "";
            }
        }

        private void btn_ReceiptDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dtg_CustomerPaymentReceipt.Columns[0].Visibility == Visibility.Hidden)
            {
                dtg_CustomerPaymentReceipt.Columns[0].Visibility = Visibility.Visible;
                btn_ReceiptDelete.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C2DECE");
                isCustomerReceiptDelete = true;
            }
            else
            {
                dtg_CustomerPaymentReceipt.Columns[0].Visibility = Visibility.Hidden;
                btn_ReceiptDelete.Background = new SolidColorBrush(Colors.Transparent);
                isCustomerReceiptDelete = false;
            }
        }

        private void cbx_CustomerDebtReportMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbx_CustomerDebtReportMonth.SelectedIndex == 0)
            {
                dtg_CustomerDebtReport.ItemsSource = BaoCaoCongNo;
            }
            else
            {
                var filteredItems = BaoCaoCongNo.Where(baocao =>
                baocao.Month == Convert.ToInt32(cbx_CustomerDebtReportMonth.SelectedItem)
                ).ToList();
                if (cbx_CustomerDebtReportYear.SelectedIndex == 0) { }
                else
                {
                    filteredItems = filteredItems.Where(baocao =>
                baocao.Year == Convert.ToInt32(cbx_CustomerDebtReportYear.SelectedItem)
                ).ToList();
                }
                dtg_CustomerDebtReport.ItemsSource = filteredItems;
            }
        }

        private void cbx_CustomerDebtReportYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbx_CustomerDebtReportYear.SelectedIndex == 0)
            {
                dtg_CustomerDebtReport.ItemsSource = BaoCaoCongNo;
            }
            else
            {
                var filteredItems = BaoCaoCongNo.Where(baocao =>
                baocao.Year == Convert.ToInt32(cbx_CustomerDebtReportYear.SelectedItem)
                ).ToList();
                if (cbx_CustomerDebtReportMonth.SelectedIndex == 0) { }
                else
                {
                    filteredItems = filteredItems.Where(baocao =>
                baocao.Month == Convert.ToInt32(cbx_CustomerDebtReportMonth.SelectedItem)
                ).ToList();
                }
                dtg_CustomerDebtReport.ItemsSource = filteredItems;
            }
        }

        private void Cbx_SearchBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txt_Search.Text = "";
        }

        private void dpk_CustomerBirthday_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            txt_CustomerDateOfBirth.Text = dpk_CustomerBirthday.SelectedDate.Value.Date.ToString().Substring(0, 10);
            
        }

        private void tbx_BookMaximumStockQuantityBeforeImporting_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(tbx_BookMaximumStockQuantityBeforeImporting.Text, out int parsedValue))
            {
                tbx_BookMaximumStockQuantityBeforeImporting.Text = SoLuongTonToiDaTruocNhap;
            }
        }

        private void tbx_BookMinimumStockQuantityAfterSelling_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(tbx_BookMinimumStockQuantityAfterSelling.Text, out int parsedValue))
            {
                tbx_BookMinimumStockQuantityAfterSelling.Text = SoLuongTonToiThieuSauBan;
            }
        }

        private void tbx_ImportToExportRatio_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(tbx_ImportToExportRatio.Text, out int parsedValue))
            {
                tbx_ImportToExportRatio.Text = TiLeGiaBan;
            }
        }

        private void tbx_BookMinimumImportQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(tbx_BookMinimumImportQuantity.Text, out int parsedValue))
            {
                tbx_BookMinimumImportQuantity.Text = SoLuongNhapToiThieu;
            }
        }

        private void tbx_CustomerMaximumDebt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(tbx_CustomerMaximumDebt.Text, out int parsedValue))
            {
                tbx_CustomerMaximumDebt.Text = SoTienNoToiDa;
            }
        }

        private void btn_ResetThamSo_Click(object sender, RoutedEventArgs e)
        {
            tbx_BookMaximumStockQuantityBeforeImporting.Text = SoLuongTonToiDaTruocNhap;
            tbx_BookMinimumStockQuantityAfterSelling.Text = SoLuongTonToiThieuSauBan;
            tbx_ImportToExportRatio.Text = TiLeGiaBan;
            tbx_BookMinimumImportQuantity.Text = SoLuongNhapToiThieu;
            tbx_CustomerMaximumDebt.Text = SoTienNoToiDa;
            if (Convert.ToInt32(ApDungQuyDinhKiemTraSoTienThu) == 1)
                cbx_CheckMoneyReceivedFromCustomer.IsChecked = true;
            else
                cbx_CheckMoneyReceivedFromCustomer.IsChecked = false;
        }
    }
}
