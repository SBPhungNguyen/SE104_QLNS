using SE104_QLNS.View;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace SE104_QLNS
{
    public partial class Uct_Author
    {
        public int AuthorNum { get; set; }
        public string AuthorID { get; set; }
        public string AuthorName { get; set; }
        public Uct_Author()
        {

        }
        public Uct_Author(int AuthorNum, string AuthorID, string AuthorName)
        {
            this.AuthorNum = AuthorNum;
            this.AuthorID = AuthorID;
            this.AuthorName = AuthorName;
        }
    }
    public partial class AuthorAdd : Window
    {
        int state = 0;
        MainWindow parent;
        Uct_Author author;
        public AuthorAdd()
        {
            InitializeComponent();
        }
        public AuthorAdd(MainWindow mainwindow, int state, Uct_Author author)
        {
            InitializeComponent();
            this.state = state;
            parent = mainwindow;
            this.author = author;
            switch (state)
            {
                case 0://normal add
                    TextAuthor.Text = "Đặt tên cho tác giả được tạo:";
                    btn_AddAuthor.Content = "Tạo";
                    txt_AuthorName.IsReadOnly = false;
                    break;
                case 1://update
                    TextAuthor.Text = "Cập nhật tên cho tác giả " + author.AuthorID + ":";
                    btn_AddAuthor.Content = "Cập nhật";
                    txt_AuthorName.Text = author.AuthorName;
                    txt_AuthorName.IsReadOnly = false;

                    break;
                case 2://delete
                    TextAuthor.Text = "Bạn có chắc muốn xóa tác giả " + author.AuthorID + ":";
                    btn_AddAuthor.Content = "Xóa";
                    txt_AuthorName.Text = author.AuthorName;
                    txt_AuthorName.IsReadOnly = true;

                    break;
            }
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_ExitApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_AddAuthor_Click(object sender, RoutedEventArgs e)
        {
            Connection connect = new Connection();
            string connectionString = connect.connection;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    if (state == 0)//Add
                    {
                        string sqlQuery = $"INSERT INTO TACGIA (MaTacGia, TenTacGia) " +
                        $"VALUES (@MaTacGia, @TenTacGia)";

                        SqlCommand command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@MaTacGia", parent.GetNextAuthorID(parent));
                        command.Parameters.AddWithValue("@TenTacGia", txt_AuthorName.Text);

                        SqlDataReader reader = command.ExecuteReader();
                        reader.Read();
                        reader.Close();

                        Notification notification = new Notification("Thêm Thành Công", "Thêm tác giả thành công!");
                    }
                    else if (state == 1)//Update
                    {
                        string sqlQuery = $"UPDATE TACGIA SET TenTacGia = @TenTacGia WHERE MaTacGia=@MaTacGia";

                        SqlCommand command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@MaTacGia", author.AuthorID);
                        command.Parameters.AddWithValue("@TenTacGia", txt_AuthorName.Text);

                        SqlDataReader reader = command.ExecuteReader();
                        reader.Read();
                        reader.Close();

                        Notification notification = new Notification("Sửa Thành Công", "Sửa tác giả thành công!");
                    }
                    else if (state == 2)//Delete
                    {
                        string sqlQuery = $"DELETE FROM TACGIA WHERE MaTacGia=@MaTacGia";

                        SqlCommand command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@MaTacGia", author.AuthorID);

                        SqlDataReader reader = command.ExecuteReader();
                        reader.Read();
                        reader.Close();
                        Notification notification = new Notification("Xóa Thành Công", "Xóa tác giả thành công!");
                    }
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Lỗi", "Đã gặp lỗi khi Thao tác trên Tác Giả: " + ex.Message);
                }
                parent.LoadAll(parent);
                parent.dtg_AuthorList.Items.Refresh();
            }
            this.Close();
        }
    }
}
