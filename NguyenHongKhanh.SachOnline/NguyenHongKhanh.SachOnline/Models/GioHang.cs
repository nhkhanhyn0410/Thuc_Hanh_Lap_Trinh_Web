using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NguyenHongKhanh.SachOnline.Models
{
    public class GioHang
    {
        SachOnlineDataEntities data = new SachOnlineDataEntities();

        public int iMaSach { get; set; }
        public string sTenSach { get; set; }
        public string sAnhBia { get; set; }
        public double dDonGia { get; set; }
        public int iSoLuong { get; set; }
        public double dThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }

        // Constructor khởi tạo giỏ hàng
        public GioHang(int ms)
        {
            iMaSach = ms;
            SACH s = data.SACHes.Single(n => n.MaSach == iMaSach);
            sTenSach = s.TenSach;
            sAnhBia = s.AnhBia;
            dDonGia = double.Parse(s.GiaBan.ToString());
            iSoLuong = 1;
        }
    }
}