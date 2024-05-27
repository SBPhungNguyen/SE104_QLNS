using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
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
using System.Windows.Shapes;

namespace SE104_QLNS
{
    /// <summary>
    /// Interaction logic for BookAddPopup.xaml
    /// </summary>
    public partial class BookAddPopup : Window
    {
        public bool IsClosing = false;
        public MainWindow parent;
        string SelectedAuthor = "";
        string SelectedGenre = "";
        string BookTitleID;
        public float ImportExportRate=1;
        string BookURL { get; set; }
        
        public BookAddPopup()
        {
            InitializeComponent();
        }

        public BookAddPopup(MainWindow mainwindow)
        {
            InitializeComponent();
            LoadAuthors();
            LoadGenres();
            parent = mainwindow;
            txt_BookID.Text = parent.GetNextBookID(parent);
            BookTitleID=parent.GetNextBookTitleID(parent);

            BookURL = "/Images/icon_addcircle.png";
            CreateImage(BookURL);

            Connection connect = new Connection();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sqlQuery = " SELECT GiaTri FROM THAMSO Where TenThamSo='TiLeGiaBan' ";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    ImportExportRate = Convert.ToInt32(reader["GiaTri"].ToString());
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi đọc dữ liệu từ THAMSO: " + ex.Message);
                }
            }
        }

        public void CreateImage(string url)
        {
            BitmapImage bimage = new BitmapImage();
            bimage.BeginInit();
            bimage.UriSource = new Uri(url, UriKind.RelativeOrAbsolute);
            bimage.EndInit();
            img_BookImage.Source = bimage;
        }

        private void btn_ExitApp_Click(object sender, RoutedEventArgs e)
        {
            IsClosing = true;
            this.Close();
        }
        
        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            IsClosing = true;
            this.Close();
        }

        private void btn_Add_Click(object sender, RoutedEventArgs e) 
        {
            

            Connection connect = new Connection();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    if (txt_ImportPrice.Text == "0")
                    {
                        Notification noti = new Notification("Lỗi", "Giá nhập và giá bán không thể bằng 0");
                        return;
                    }
                    if (Convert.ToInt32(txt_Quantity.Text) < Convert.ToInt32(parent.SoLuongNhapToiThieu))
                    {
                        Notification noti = new Notification("Vi phạm quy định", "Số lượng sách nhập phải lớn hơn " + parent.SoLuongNhapToiThieu);
                        return;
                    }

                    connection.Open();
                    
                        //Get Genre Code
                    string sqlQuery = "SELECT TOP 1 MaTheLoai FROM THELOAI WHERE TenTheLoai = @TenTheLoai";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@TenTheLoai", cbx_Genre.Text);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    string BookGenreID = reader["MaTheLoai"].ToString();
                    reader.Close();


                        //Get Author Code
                    sqlQuery = "SELECT TOP 1 MaTacGia FROM TACGIA WHERE TenTacGia = @TenTacGia";
                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@TenTacGia", cbx_Author.Text);
                    reader = command.ExecuteReader();
                    reader.Read();
                    string AuthorID = reader["MaTacGia"].ToString();
                    reader.Close();


                        //Create Book Title
                    sqlQuery = $"INSERT INTO DAUSACH (MaDauSach, TenDauSach, MaTheLoai) " +
                      $"VALUES (@MaDauSach, @TenDauSach, @MaTheLoai)";

                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaDauSach", BookTitleID);
                    command.Parameters.AddWithValue("@TenDauSach", txt_BookName.Text);
                    command.Parameters.AddWithValue("@MaTheLoai", BookGenreID);

                    reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();


                        //Create AuthorDetails
                    sqlQuery = $"INSERT INTO CT_TACGIA (MaDauSach, MaTacGia)" +
                      $"VALUES (@MaDauSach, @MaTacGia)";

                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaDauSach", BookTitleID);
                    command.Parameters.AddWithValue("@MaTacGia", AuthorID);

                    reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();


                        //Create BOOK
                    sqlQuery = $"INSERT INTO SACH (MaSach, MaDauSach, NXB, NamXB, HinhAnhSach, SoLuongTon, DonGiaNhap, DonGiaBan) " +
                      $"VALUES (@MaSach, @MaDauSach, @NXB, @NamXB, @HinhAnhSach, @SoLuongTon, @DonGiaNhap, @DonGiaBan)";

                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaSach", txt_BookID.Text);
                    command.Parameters.AddWithValue("@MaDauSach", BookTitleID);
                    command.Parameters.AddWithValue("@NXB", txt_Distributor.Text);
                    command.Parameters.AddWithValue("@NamXB", int.Parse(txt_DistributeYear.Text));
                    command.Parameters.AddWithValue("@HinhAnhSach", BookURL);
                    command.Parameters.AddWithValue("@SoLuongTon", int.Parse(txt_Quantity.Text));
                    command.Parameters.AddWithValue("@DonGiaNhap", txt_ImportPrice.Text);
                    command.Parameters.AddWithValue("@DonGiaBan", txt_ExportPrice.Text);
                    reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();

                    string nextImportPaperID = parent.GetNextImportPaperID(parent);

                    //Add to Import Book
                     sqlQuery = "INSERT INTO PHIEUNHAP (MaPhieuNhap, NgayNhap, TongTien) " +
                  $"VALUES (@MaPhieuNhap, @NgayNhap, @TongTien)";
                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaPhieuNhap", nextImportPaperID);
                    command.Parameters.AddWithValue("@NgayNhap", DateTime.Now);
                    command.Parameters.AddWithValue("@TongTien", 0);
                    reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();

                    //Creating
                    sqlQuery = "INSERT INTO CT_PHIEUNHAP (MaPhieuNhap, MaSach, SoLuongNhap, DonGiaNhap) " +
                  $"VALUES (@MaPhieuNhap, @MaSach, @SoLuongNhap, @DonGiaNhap)";
                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaPhieuNhap", nextImportPaperID);
                    command.Parameters.AddWithValue("@MaSach", txt_BookID.Text);
                    command.Parameters.AddWithValue("@SoLuongNhap", txt_Quantity.Text);
                    command.Parameters.AddWithValue("@DonGiaNhap", txt_ImportPrice.Text);
                    reader = command.ExecuteReader();
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
                    SUM += Convert.ToInt32(txt_Quantity.Text) * Convert.ToInt32(txt_ImportPrice.Text);
                    reader.Close();


                    //Update The SUM
                    sqlQuery = "UPDATE PHIEUNHAP SET TongTien=@TongTien WHERE MaPhieuNhap = @MaPhieuNhap";
                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@TongTien", SUM);
                    command.Parameters.AddWithValue("@MaPhieuNhap", nextImportPaperID);
                    reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();

                    //Create for BAOCAOTON
                    sqlQuery = "INSERT INTO BAOCAOTON (Thang, Nam, MaSach, TonDau, PhatSinh, TonCuoi) " +
                  $"VALUES (@Thang, @Nam, @MaSach, @TonDau, @PhatSinh, @TonCuoi)";
                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@Thang", DateTime.Now.Month.ToString());
                    command.Parameters.AddWithValue("@Nam", DateTime.Now.Year.ToString());
                    command.Parameters.AddWithValue("@MaSach", txt_BookID.Text);
                    command.Parameters.AddWithValue("@TonDau", 0);
                    command.Parameters.AddWithValue("@PhatSinh", Convert.ToInt32(txt_Quantity.Text));
                    command.Parameters.AddWithValue("@TonCuoi", Convert.ToInt32(txt_Quantity.Text));
                    reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi tạo sách: " + ex.Message);
                }
            }
            parent.LoadAll(parent);
            IsClosing = true;
            this.Close();
        }

        private void LoadAuthors()
        {
            cbx_Author.Items.Clear(); // Clear existing items

            Connection connect = new Connection();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sqlQuery = "SELECT TenTacGia, MaTacGia FROM TACGIA";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string authorName = reader["TenTacGia"].ToString();
                        SelectedAuthor = reader["MaTacGia"].ToString();
                        cbx_Author.Items.Add(authorName); // Add author name to ComboBox
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi lấy thông tin tác giả: " + ex.Message);
                }
            }
        }
        private void LoadGenres()
        {
            cbx_Genre.Items.Clear(); // Clear existing items

            Connection connect = new Connection();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sqlQuery = "SELECT TenTheLoai, MaTheLoai FROM THELOAI";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string genresname = reader["TenTheLoai"].ToString();
                        SelectedGenre = reader["MaTheLoai"].ToString();
                        
                        cbx_Genre.Items.Add(genresname);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi lấy thông tin thể loại: " + ex.Message);
                }
            }
        }
        private void cbx_Author_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void btn_BookImage_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                try
                {

                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*";
                    if (openFileDialog.ShowDialog() == true)
                    {
                        string selectedImagePath = openFileDialog.FileName;
                        BookURL = selectedImagePath;
                        CreateImage(BookURL);
                    }

                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi mở hình ảnh: "+ex.Message);
                }
            });
        }

        private void txt_ImportPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            int convertedImportPrice=-1;
            if (int.TryParse(txt_ImportPrice.Text, out convertedImportPrice))
            {
                txt_ImportPrice.Text = convertedImportPrice.ToString();
                txt_ExportPrice.Text= (convertedImportPrice*ImportExportRate/100).ToString();
            }
            else
            {
                txt_ImportPrice.Text = "0";
                convertedImportPrice = -1;
            }
        }

        private void txt_DistributeYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            int test;
            if (!int.TryParse(txt_DistributeYear.Text, out test))
            {
                txt_DistributeYear.Text= "0";
            }
        }

        private void txt_Quantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            int test;
            if (!int.TryParse(txt_Quantity.Text, out test))
            {
                txt_Quantity.Text = "0";
            }
        }

        private void txt_ImportPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "\b")
            {
                return;
            }

            // Only allow numbers and decimal point (if allowed)
            e.Handled = !Regex.IsMatch(e.Text, "[0-9.]");
        }

        private void txt_Quantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            if (e.Text == "\b")
            {
                return;
            }

            // Only allow numbers and decimal point (if allowed)
            e.Handled = !Regex.IsMatch(e.Text, "[0-9.]");
        }

        private void txt_DistributeYear_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            if (e.Text == "\b")
            {
                return;
            }

            // Only allow numbers and decimal point (if allowed)
            e.Handled = !Regex.IsMatch(e.Text, "[0-9.]");
        }
    }
}
