using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class SinhVienService
    {
        Model1 db = new Model1();   
        public List<SinhVien> GetAll()
        {
            return db.SinhVien.ToList();
        }
        public SinhVien GetItem(string id)
        {
            return db.SinhVien.FirstOrDefault(p => p.MaSV == id);
        }
        //public void Add(SinhVien sinhvien)
        //{
        //    db.SinhVien.Add(sinhvien);
        //    db.SaveChanges();
        //}

        //public void Update(SinhVien sinhvien)
        //{
        //    db.Entry(sinhvien).State = EntityState.Modified;
        //    db.SaveChanges();
        //}
        public void Delete(string sinhvienid)
        {
            var sinhvien = db.SinhVien.FirstOrDefault(p => p.MaSV == sinhvienid);
            if (sinhvien != null)
            {
                db.SinhVien.Remove(sinhvien);
                db.SaveChanges();
            }
        }
        public void InsertUpdate(SinhVien sinhvien)
        {
            Model1 db = new Model1();
            db.SinhVien.AddOrUpdate(sinhvien);
            db.SaveChanges();
        }
        
        public List<SinhVien> SearchSach(string keyword)
        {
            keyword = keyword.ToLower();
            // Thực hiện tìm kiếm sách trong cơ sở dữ liệu hoặc danh sách
            // Dựa trên từ khóa keyword
            // Trả về danh sách kết quả
            var result = db.SinhVien.Where(s => s.MaSV.ToLower().Contains(keyword) ||
                                           s.HotenSV.ToLower().Contains(keyword) ||
                                           s.NgaySinh.ToString().Contains(keyword)).ToList();
            return result;
        }
    }
}
