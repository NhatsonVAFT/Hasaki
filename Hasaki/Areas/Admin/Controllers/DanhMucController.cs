using Hasaki.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace Hasaki.Areas.Admin.Controllers
{
    public class DanhMucController : Controller
    {
        HasakiDatabaseEntities1 db = new HasakiDatabaseEntities1();
        // GET: Admin/DanhMuc
        public ActionResult DanhSachDM()
        {
            if (Session["Name"] != null)
            {
            var cate = db.DanhMucSanPhams.ToList();
            return View(cate);
            }
            return RedirectToAction("Login", "LoginRegis");
        }
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["Name"] != null)
            {
                return View();
            }
            return RedirectToAction("Login", "LoginRegis");
        }
        [HttpPost]
        public ActionResult Create(DanhMucSanPham cate)
        {
            if (Session["Name"] != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(cate);
                }

                db.DanhMucSanPhams.Add(cate);
                db.SaveChanges();

                return RedirectToAction("DanhSachDM");
            }
            return RedirectToAction("Login", "LoginRegis");
        }
        public ActionResult Delete(int id)
        {
            if (Session["Name"] != null)
            {
                try
                {
                    var cate = db.DanhMucSanPhams.Find(id);
                    db.DanhMucSanPhams.Remove(cate);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    // Dính liên kết tới các bảng khác sẽ reload lại trang không thực hiện tác vụ
                    return RedirectToAction("DanhSachDM");
                }

                return RedirectToAction("DanhSachDM");
            }
            return RedirectToAction("Login", "LoginRegis");
        }

        public ActionResult Details(int? id)
        {
            if (Session["Name"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                DanhMucSanPham cate = db.DanhMucSanPhams.Find(id);
                if (cate == null)
                {
                    return HttpNotFound();
                }
                return View(cate);
            }
            return RedirectToAction("Login", "LoginRegis");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Name"] != null)
            {
                var cate = db.DanhMucSanPhams.Find(id);
                if (cate == null)
                {
                    // Xử lý trường hợp không tìm thấy sản phẩm
                    return HttpNotFound();
                }

                return View(cate);
            }
            return RedirectToAction("Login", "LoginRegis");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DanhMucSanPham cate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DanhSachDM");
            }
            return View(cate);
        }
    }
}