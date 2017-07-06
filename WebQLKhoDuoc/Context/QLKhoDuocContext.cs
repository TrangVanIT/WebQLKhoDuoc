using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebQLKhoDuoc.Models;

namespace WebQLKhoDuoc.Context
{
    public class QLKhoDuocContext : DbContext
    {
        public QLKhoDuocContext()  : base("WebQLKhoDuoc")
        {
            Database.SetInitializer<QLKhoDuocContext>(new DropCreateDatabaseIfModelChanges<QLKhoDuocContext>());
          //  Database.SetInitializer<QLKhoDuocContext>(new CreateDatabaseIfNotExists<QLKhoDuocContext>());

        }
        public DbSet<LoaiThanhVien> LoaiThanhViens { get; set; }
        public DbSet<ThanhVien> ThanhViens { get; set; }
        public DbSet<HopDong> HopDongs { get; set; }
        public DbSet<Kho> Khos { get; set; }
        public DbSet<LoaiMatHang> LoaiMatHangs { get; set; }
        public DbSet<MatHang> MatHangs { get; set; }
        public DbSet<ChiTietHopDong> ChiTietHopDongs { get; set; }
        public DbSet<DonViGN> DonViGNs { get; set; }
        public DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public DbSet<PhieuNhap> PhieuNhaps { get; set; }
        public DbSet<PhieuXuat> PhieuXuats { get; set; }
        public DbSet<ChiTietPhieuXuat> ChiTietPhieuXuats { get; set; }
        public DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }
        public DbSet<HisLog> HisLogs { get; set; }
        public DbSet<HanhDong> HanhDongs { get; set; }
        public DbSet<DanhMucQuyen> DanhMucQuyens { get; set; }
        public DbSet<PhanQuyen> PhanQuyens { get; set; }
        public DbSet<PhanQuyenTV> PhanQuyenTVs { get; set; }


    }
}