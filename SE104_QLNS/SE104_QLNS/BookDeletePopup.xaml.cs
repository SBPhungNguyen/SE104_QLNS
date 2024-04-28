using SE104_QLNS.View;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace SE104_QLNS
{
    /// <summary>
    /// Interaction logic for BookDeletePopup.xaml
    /// </summary>
    public partial class BookDeletePopup : Window
    {
        public MainWindow parent;
        public Uct_Books selectedbook;
        public bool IsClosing = false;
        public string BookURL
        {
            get; set;
        }
        public BookDeletePopup()
        {
            InitializeComponent();
        }
        public BookDeletePopup(Uct_Books book, MainWindow mainWindow)
        {
            InitializeComponent();
            this.tbl_BookID.Text = book.BookID;
            this.tbl_BookName.Text = book.BookName;
            this.tbl_Gerne.Text = book.BookGenre;
            this.tbl_Author.Text = book.BookAuthor;
            this.tbl_ImportPrice.Text = book.BookPriceImport;
            this.tbl_ExportPrice.Text = book.BookPriceExport;
            this.tbl_Quantity.Text = book.Amount;
            this.txt_Distribute.Text = book.BookDistribution;
            this.txt_DistributeYear.Text = book.BookDistributionYear;
            this.BookURL = book.BookURL;
            BitmapImage bimage = new BitmapImage();
            bimage.BeginInit();
            bimage.UriSource = new Uri(BookURL, UriKind.RelativeOrAbsolute);
            bimage.EndInit();
            img_BookImg.Source = bimage;
            parent = mainWindow;
            selectedbook = book;
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            IsClosing = true;
            this.Close();
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
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

                    //Delete from Author Details
                    sqlQuery = "DELETE FROM CT_TACGIA WHERE MaDauSach = @MaDauSach";
                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaDauSach", BookTitleID);
                    reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();

                    //Delete The Book
                    sqlQuery = "DELETE FROM SACH WHERE MaSach = @MaSach";

                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaSach", tbl_BookID.Text);
                    reader = command.ExecuteReader();
                    reader.Read();
                    reader.Close();

                    //Delete Book Title
                    sqlQuery = "DELETE FROM DAUSACH WHERE MaDauSach = @MaDauSach";
                    command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@MaDauSach", BookTitleID);
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
    }
}
