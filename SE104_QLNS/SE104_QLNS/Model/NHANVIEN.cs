//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SE104_QLNS.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class NHANVIEN
    {
        public string MaNV { get; set; }
        public string HoTenNV { get; set; }
        public string MaNhom { get; set; }
        public string Email { get; set; }
        public string MatKhau { get; set; }
        public string HinhAnhNV { get; set; }
        public bool LaQuanLy { get; set; }
    
        public virtual NHOMNGUOIDUNG NHOMNGUOIDUNG { get; set; }
    }
}
