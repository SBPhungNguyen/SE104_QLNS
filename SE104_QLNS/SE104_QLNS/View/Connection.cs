﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE104_QLNS
{
    internal class Connection
    {
        public string connection = @"Server=DESKTOP-EPI23U4;Database=QLNS;Trusted_Connection=Yes;";
        public Connection()
        { 
        }
    }
}
