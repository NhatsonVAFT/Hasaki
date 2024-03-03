﻿using Hasaki.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Hasaki.Areas.Admin.Controllers
{
    public class LoginRegisController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(NhanVien nv)
        {
            var db = new HasakiDatabaseEntities1();
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(nv.TenDangNhap))
                    ModelState.AddModelError(string.Empty, "Email không được để trống");
                if (string.IsNullOrEmpty(nv.MatKhau))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (ModelState.IsValid)
                {
                    var nhanvien = db.NhanViens.FirstOrDefault(k => k.TenDangNhap == nv.TenDangNhap && k.MatKhau.ToLower() == nv.MatKhau.ToLower());
                    if (nhanvien != null)
                    {
                        Session["Name"] = nhanvien.TenNhanVien;
                        Session["IDuser"] = nhanvien.VaiTro;
                    }
                    else
                    {
                        var mail = db.NhanViens.FirstOrDefault(k => k.TenDangNhap == nv.TenDangNhap);
                        if (mail != null)
                        {
                            ModelState.AddModelError(string.Empty, "Sai mật khẩu");
                            return View();
                        }
                        ModelState.AddModelError(string.Empty, "Tài khoản không tồn tại");
                        return View();
                    }
                }
            }
            return RedirectToAction("DanhSachDH", "DonHang");
        }
    }
}