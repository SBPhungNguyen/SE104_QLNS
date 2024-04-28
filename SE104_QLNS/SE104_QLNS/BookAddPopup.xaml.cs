using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
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
    /// Interaction logic for BookAddPopup.xaml
    /// </summary>
    public partial class BookAddPopup : Window
    {
        public bool IsClosing = false;
        public MainWindow parent;
        string SelectedAuthor = "";
        string SelectedGenre = "";
        string BookTitleID;
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
                    command.Parameters.AddWithValue("@NXB", txt_Distribute.Text);
                    command.Parameters.AddWithValue("@NamXB", txt_DistributeYear.Text);
                    command.Parameters.AddWithValue("@HinhAnhSach", BookURL);
                    command.Parameters.AddWithValue("@SoLuongTon", int.Parse(txt_Quantity.Text));
                    command.Parameters.AddWithValue("@DonGiaNhap", int.Parse(txt_ImportPrice.Text));
                    command.Parameters.AddWithValue("@DonGiaBan", int.Parse(txt_ExportPrice.Text));


                    reader = command.ExecuteReader();
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error retrieving data: " + ex.Message);
                }
            }

            parent.LoadCustomer(parent, 0);
            IsClosing = true;
            this.Close();
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
                        SelectedGenre = reader["MaTheLoai"].ToString();
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
                        //Notification noti = new Notification("Updated", BookURL); .Replace("\\","/")
                    }

                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error opening file: "+ex.Message);
                }
            });
        }

    }
}
