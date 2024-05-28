using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ShopDoChoiLEGO.Models;

namespace ShopDoChoiLEGO.Controllers
{
    public class QLSanPhamsController : Controller
    {
        private Model1 db = new Model1();

        // GET: QLSanPhams
        public ActionResult TrangChu()
        {
            var sanPham = db.SanPham.Include(s => s.Kho).Include(s => s.KhuyenMai).Include(s => s.LoaiSanPham).Include(s => s.NhaSanXuat);
            return View(sanPham.ToList());
        }
       
        // GET: QLSanPhams/Create
        public ActionResult TaoMoi()
        {
            ViewBag.MaKho = new SelectList(db.Kho, "MaKho", "TenKho");
            ViewBag.MaKhuyenMai = new SelectList(db.KhuyenMai, "MaKhuyenMai", "TenKhuyenMai");
            ViewBag.MaLoaiSanPham = new SelectList(db.LoaiSanPham, "MaLoaiSanPham", "TenLoaiSanPham");  
            ViewBag.MaNhaSanXuat = new SelectList(db.NhaSanXuat, "MaNhaSanXuat", "TenNhaSanXuat");
            return View();
        }

        // POST: QLSanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoMoi([Bind(Include = "MaSanPham,TenSanPham,MoTa,Gia,URLHinhAnh,SoLuongTrongKho,NgayThemVao,TrangThai,MaLoaiSanPham,MaNhaSanXuat,MaKhuyenMai,MaKho")] SanPham sanPham, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem sản phẩm có tên đã tồn tại trong cơ sở dữ liệu chưa
                var existingSanPham = db.SanPham.FirstOrDefault(p => p.TenSanPham == sanPham.TenSanPham);
                if (existingSanPham != null)
                {
                    ModelState.AddModelError("", "Tên sản phẩm đã tồn tại. Vui lòng chọn tên khác.");
                    return View(sanPham);
                }

                // Kiểm tra số lượng và giá
                if (sanPham.SoLuongTrongKho < 0 || sanPham.Gia < 0)
                {
                    ModelState.AddModelError("", "Số lượng và giá không được nhỏ hơn 0.");
                    return View(sanPham);
                }
                // Tạo số ngẫu nhiên
                Random random = new Random();
                int randomNumber = random.Next(1000, 9999);
                //Tạo mã sản phẩm 
                sanPham.MaSanPham = $"MSP_{randomNumber}";
                // Lưu sản phẩm vào cơ sở dữ liệu
                db.SanPham.Add(sanPham);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
                // Kiểm tra và lưu ảnh
                //if (imageFile != null && imageFile.ContentLength > 0)
                //{
                //    var fileName = Path.GetFileName(imageFile.FileName);
                //    var path = Path.Combine(Server.MapPath("~/Public/image/"), fileName);
                //    imageFile.SaveAs(path);

                //    // Lưu đường dẫn vào thuộc tính URLHinhAnh
                //    sanPham.URLHinhAnh = fileName;

                //    db.SaveChanges();
                //}
                return RedirectToAction("TrangChu");
            }

            ViewBag.MaKho = new SelectList(db.Kho, "MaKho", "TenKho", sanPham.MaKho);
            ViewBag.MaKhuyenMai = new SelectList(db.KhuyenMai, "MaKhuyenMai", "TenKhuyenMai", sanPham.MaKhuyenMai);
            ViewBag.MaLoaiSanPham = new SelectList(db.LoaiSanPham, "MaLoaiSanPham", "TenLoaiSanPham", sanPham.MaLoaiSanPham);
            ViewBag.MaNhaSanXuat = new SelectList(db.NhaSanXuat, "MaNhaSanXuat", "TenNhaSanXuat", sanPham.MaNhaSanXuat);
            return View(sanPham);
        }




        // GET: QLSanPhams/Edit/5
        public ActionResult ChinhSua(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKho = new SelectList(db.Kho, "MaKho", "TenKho", sanPham.MaKho);
            ViewBag.MaKhuyenMai = new SelectList(db.KhuyenMai, "MaKhuyenMai", "TenKhuyenMai", sanPham.MaKhuyenMai);
            ViewBag.MaLoaiSanPham = new SelectList(db.LoaiSanPham, "MaLoaiSanPham", "TenLoaiSanPham", sanPham.MaLoaiSanPham);
            ViewBag.MaNhaSanXuat = new SelectList(db.NhaSanXuat, "MaNhaSanXuat", "TenNhaSanXuat", sanPham.MaNhaSanXuat);
            return View(sanPham);
        }

        // POST: QLSanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChinhSua(string id, SanPham sanPham, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {

                // Kiểm tra số lượng và giá
                if (sanPham.SoLuongTrongKho < 0 || sanPham.Gia < 0)
                {
                    ModelState.AddModelError("", "Số lượng và giá không được nhỏ hơn 0.");
                    return View(sanPham);
                }
                // Tìm sản phẩm theo id
                SanPham existingProduct = db.SanPham.Find(id);

                if (existingProduct == null)
                {
                    return HttpNotFound();
                }

                // Kiểm tra tệp tin ảnh đã tải lên
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    // Lưu tệp tin ảnh mới
                    string extension = Path.GetExtension(imageFile.FileName);
                    string newImageName = Guid.NewGuid().ToString() + extension;
                    string newImagePath = Server.MapPath("~/Public/image/" + newImageName);
                    imageFile.SaveAs(newImagePath);

                    // Cập nhật đường dẫn ảnh mới trong sản phẩm
                    existingProduct.URLHinhAnh = newImageName;
                }

                // Cập nhật thông tin sản phẩm
                existingProduct.TenSanPham = sanPham.TenSanPham;
                existingProduct.MoTa = sanPham.MoTa;
                existingProduct.Gia = sanPham.Gia;
                existingProduct.SoLuongTrongKho = sanPham.SoLuongTrongKho;
                existingProduct.NgayThemVao = sanPham.NgayThemVao;
                existingProduct.TrangThai = sanPham.TrangThai;
                existingProduct.MaLoaiSanPham = sanPham.MaLoaiSanPham;
                existingProduct.MaNhaSanXuat = sanPham.MaNhaSanXuat;
                existingProduct.MaKhuyenMai = sanPham.MaKhuyenMai;
                existingProduct.MaKho = sanPham.MaKho;

                TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";


                // Lưu thay đổi vào cơ sở dữ liệu
                db.Entry(existingProduct).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("TrangChu");
            }

            ViewBag.MaKho = new SelectList(db.Kho, "MaKho", "TenKho", sanPham.MaKho);
            ViewBag.MaKhuyenMai = new SelectList(db.KhuyenMai, "MaKhuyenMai", "TenKhuyenMai", sanPham.MaKhuyenMai);
            ViewBag.MaLoaiSanPham = new SelectList(db.LoaiSanPham, "MaLoaiSanPham", "TenLoaiSanPham", sanPham.MaLoaiSanPham);
            ViewBag.MaNhaSanXuat = new SelectList(db.NhaSanXuat, "MaNhaSanXuat", "TenNhaSanXuat", sanPham.MaNhaSanXuat);
            return View(sanPham);
        }



        // GET: QLSanPhams/Delete/5
        public ActionResult Xoa(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: QLSanPhams/Delete/5
        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanXoa(string id)
        {
            SanPham sanPham = db.SanPham.Find(id);
            TempData["SuccessMessage"] = "Xóa sản phẩm thành công!";

            db.SanPham.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("TrangChu");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
