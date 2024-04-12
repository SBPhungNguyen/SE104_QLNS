﻿using System;
using System.Collections.Generic;
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

namespace SE104_QLNS.View
{
    /// <summary>
    /// Interaction logic for Uct_Books.xaml
    /// </summary>
    public partial class Uct_Books : UserControl
    {
        public Uct_Books()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public string BookURL
        {
            get; set;
        }
        public string BookName
        { get; set; }
        public string BookNum
        {
            get; set;
        }
        public string BookPriceImport
        { get; set; }
        public string BookPriceExport
        { get; set; }
        public string BookID
        { get; set; }
        public string Icon
        { get; set; }
        public void LoadData(string BookCode, string BookName, string URL, string NumOfBook, string PriceImport, string PriceExport, string Icon)
        {
            this.BookID = BookCode;
            this.BookName = BookName;
            this.BookURL = URL;
            BookNum = NumOfBook;
            this.BookPriceImport = PriceImport;
            this.BookPriceExport = PriceExport;
            this.Icon = Icon;
        }
    }
}