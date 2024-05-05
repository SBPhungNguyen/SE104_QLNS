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
    public partial class Uct_Genre
    {
        public int GenreNum { get; set; }
        public string GenreID { get; set; }
        public string GenreName { get; set; }
        public Uct_Genre()
        {

        }
        public Uct_Genre(int GenreNum, string GenreID, string GenreName)
        {
            this.GenreNum = GenreNum;
            this.GenreID = GenreID;
            this.GenreName = GenreName;
        }
    }
    public partial class GenreAdd : Window
    {
        int state = 0;
        MainWindow parent;
        Uct_Genre genre;
        public GenreAdd()
        {
            InitializeComponent();
        }
        public GenreAdd(MainWindow mainwindow, int state, Uct_Genre genre)
        {
            InitializeComponent();
            this.state = state;
            parent = mainwindow;
            this.genre = genre;
            switch (state)
            {
                case 0://normal add
                    TextGenre.Text = "Đặt tên cho thể loại được tạo:";
                    btn_AddGenre.Content = "Tạo";
                    txt_GenreName.IsReadOnly = false;
                    break;
                case 1://update
                    TextGenre.Text = "Cập nhật tên cho thể loại " + genre.GenreID + ":";
                    btn_AddGenre.Content = "Cập nhật";
                    txt_GenreName.Text = genre.GenreName;
                    txt_GenreName.IsReadOnly = false;

                    break;
                case 2://delete
                    TextGenre.Text = "Bạn có chắc muốn xóa thể loại " + genre.GenreID + ":";
                    btn_AddGenre.Content = "Xóa";
                    txt_GenreName.Text = genre.GenreName;
                    txt_GenreName.IsReadOnly = true;

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

        private void btn_AddGenre_Click(object sender, RoutedEventArgs e)
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
                        string sqlQuery = $"INSERT INTO THELOAI (MaTheLoai, TenTheLoai) " +
                        $"VALUES (@MaTheLoai, @TenTheLoai)";

                        SqlCommand command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@MaTheLoai", parent.GetNextGenreID(parent));
                        command.Parameters.AddWithValue("@TenTheLoai", txt_GenreName.Text);

                        SqlDataReader reader = command.ExecuteReader();
                        reader.Read();
                        reader.Close();
                    }
                    else if (state==1)//Update
                    {
                        string sqlQuery = $"UPDATE THELOAI SET TenTheLoai = @TenTheLoai WHERE MaTheLoai=@MaTheLoai";

                        SqlCommand command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@MaTheLoai", genre.GenreID);
                        command.Parameters.AddWithValue("@TenTheLoai", txt_GenreName.Text);

                        SqlDataReader reader = command.ExecuteReader();
                        reader.Read();
                        reader.Close();
                    }
                    else if (state == 2)//Delete
                    {
                        string sqlQuery = $"DELETE FROM THELOAI WHERE MaTheLoai=@MaTheLoai";

                        SqlCommand command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@MaTheLoai", genre.GenreID);

                        SqlDataReader reader = command.ExecuteReader();
                        reader.Read();
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Notification noti = new Notification("Error", "Error Inserting/Updating Genres: " + ex.Message);
                }
                parent.LoadGenre(parent, 0);
                parent.dtg_GenreList.Items.Refresh();
            }
            this.Close();
        }
    }
}
