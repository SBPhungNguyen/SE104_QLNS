using Microsoft.Win32;
using SE104_QLNS.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Globalization;
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
using static System.Net.Mime.MediaTypeNames;

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
        public float ImportExportRate = 1;
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
            this.cbx_Genre.Text = book.BookGenre;
            this.cbx_Author.Text = book.BookAuthor;
            this.txt_ImportPrice.Text = book.BookPriceImport;
            this.txt_ExportPrice.Text = book.BookPriceExport;
            this.txt_Quantity.Text = book.Amount;
            this.BookURL = book.BookURL;
            txt_Distribute.Text = book.BookDistribution;
            txt_DistributeYear.Text = book.BookDistributionYear;
            BitmapImage bimage = new BitmapImage();
            bimage.BeginInit();
            bimage.UriSource = new Uri(BookURL, UriKind.RelativeOrAbsolute);
            bimage.EndInit();
            img_BookImg.Source = bimage;
            this.parent = mainwindow;
            this.selectedbook = book;
            LoadGenres();
            LoadAuthors();

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
                    Notification noti = new Notification("Error", "Error Retrieving Data: " + ex.Message);
                }
            }
        }

        public void CreateImage(string url)
        {
            BitmapImage bimage = new BitmapImage();
            bimage.BeginInit();
            bimage.UriSource = new Uri(url, UriKind.RelativeOrAbsolute);
            bimage.EndInit();
            img_BookImg.Source = bimage;
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
                        //Notification noti = new Notification("Updated", BookURL); .Replace("\\","/")
                    }

                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error opening file: " + ex.Message);
                }
            });
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
                        cbx_Author.Items.Add(authorName); // Add author name to ComboBox
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error retrieving authors: " + ex.Message);
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
                        string genres = reader["TenTheLoai"].ToString();
                        cbx_Genre.Items.Add(genres);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error retrieving genres: " + ex.Message);
                }
            }
        }

        private void btn_Update_Click(object sender, RoutedEventArgs e)
        {
            Connection connect = new Connection();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    //Get BookTitile Code
                    string sqlQuery = "SELECT MaDauSach FROM SACH WHERE MaSach = @MaSach";
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaSach", tbl_BookID.Text);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    string BookTitleID = reader["MaDauSach"].ToString();
                    reader.Close();

                    //Get Genre Code
                    sqlQuery = "SELECT TOP 1 MaTheLoai FROM THELOAI WHERE TenTheLoai = @TenTheLoai";
                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@TenTheLoai", cbx_Genre.Text);
                    reader = command.ExecuteReader();
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

                    //Update Name and Genre
                    sqlQuery = "UPDATE DAUSACH SET TenDauSach = @TenDauSach, MaTheLoai = @MaTheLoai WHERE MaDauSach = @MaDauSach";
                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@TenDauSach", txt_BookName.Text);
                    command.Parameters.AddWithValue("@MaTheLoai", BookGenreID);
                    command.Parameters.AddWithValue("@MaDauSach", BookTitleID);
                    reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();

                    //Update Author
                    sqlQuery = "UPDATE CT_TACGIA SET MaTacGia = @MaTacGia WHERE MaDauSach = @MaDauSach";
                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaTacGia", AuthorID);
                    command.Parameters.AddWithValue("@MaDauSach", BookTitleID);
                    reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();

                    //Update the Rest
                    sqlQuery = "UPDATE SACH SET NXB = @NXB, NamXB = @NamXB, HinhAnhSach = @HinhAnhSach, " +
                        "SoLuongTon = @SoLuongTon, DonGiaNhap = @DonGiaNhap, DonGiaBan = @DonGiaBan WHERE MaSach = @MaSach";

                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@NXB", txt_Distribute.Text); 
                    command.Parameters.AddWithValue("@NamXB", txt_DistributeYear.Text);
                    command.Parameters.AddWithValue("@HinhAnhSach", BookURL);
                    command.Parameters.AddWithValue("@SoLuongTon", txt_Quantity.Text);
                    command.Parameters.AddWithValue("@DonGiaNhap", txt_ImportPrice.Text);
                    command.Parameters.AddWithValue("@DonGiaBan", txt_ExportPrice.Text);
                    command.Parameters.AddWithValue("@MaSach", tbl_BookID.Text);

                    reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();

                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error Updating Book: " + ex.Message);
                }
                parent.LoadBook(parent, 0);
                IsClosing = true;
                this.Close();
            }

        }

        private void txt_ImportPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txt_ExportPrice == null) return;
            txt_ExportPrice.Text = "";
            int convertedImportPrice;
            if (int.TryParse(txt_ImportPrice.Text, out convertedImportPrice))
            {
                txt_ExportPrice.Text = (convertedImportPrice * ImportExportRate / 100).ToString();
            }
            else
            {
                txt_ImportPrice.Text = "0";
            }
        }

        private void txt_DistributeYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            int test;
            if (!int.TryParse(txt_DistributeYear.Text, out test))
            {
                txt_DistributeYear.Text = "0";
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
    }
}
