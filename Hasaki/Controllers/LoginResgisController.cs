using Hasaki.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Migrations;
using System.Web.UI.HtmlControls;
using System.Web.Helpers;

namespace Hasaki.Controllers
{
    public class LoginResgisController : Controller
    {
        // GET: LoginResgis
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(KhachHang kh)
        {
            var db = new HasakiDatabaseEntities1();
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(kh.Email))
                    ModelState.AddModelError(string.Empty, "Email không được để trống");
                if (string.IsNullOrEmpty(kh.MatKhau))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if(ModelState.IsValid)
                {
                    var sha256 = SHA256.Create();
                    byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(kh.MatKhau));

                    var sb = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        sb.Append(bytes[i].ToString("x2"));
                    }
                    string matkhau = sb.ToString();
                    var khach = db.KhachHangs.FirstOrDefault(k => k.Email == kh.Email && k.MatKhau.ToLower() == matkhau.ToString());
                    if (khach != null)
                    {
                        Session["Name1"] = khach.TenKhachHang;
                        Session["IDuser"] = khach.KhachHangID;
                        Session["TaiKhoan"] = khach;
                        Session["email"] = khach.Email;
                    }
                    else
                    {
                        var mail = db.KhachHangs.FirstOrDefault(k => k.Email == kh.Email);
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
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Regis()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Regis(string email, string tenkhachhang, string matkhau1, string matkhau2)
        {
            var db = new HasakiDatabaseEntities1();
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(email))
                    ModelState.AddModelError(string.Empty, "Email không được để trống");
                if (string.IsNullOrEmpty(tenkhachhang))
                    ModelState.AddModelError(string.Empty, "Tên không được để trống");
                if (string.IsNullOrEmpty(matkhau1))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (string.IsNullOrEmpty(matkhau2))
                    ModelState.AddModelError(string.Empty, "Nhập lại mật khẩu");
                if (matkhau1 != matkhau2)
                    ModelState.AddModelError(string.Empty, "Mật khẩu không khớp");
                if (ModelState.IsValid)
                {
                    var sha256 = SHA256.Create();
                    byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(matkhau1));

                    var sb = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        sb.Append(bytes[i].ToString("x2"));
                    }
                    string matkhau = sb.ToString();
                    var khach = db.KhachHangs.FirstOrDefault(k => k.Email == email);
                    if (khach != null)
                    {
                        ModelState.AddModelError(string.Empty, "Tài khoản đã tồn tại");
                    }
                    else if (khach == null)
                    {
                        KhachHang khachhang = new KhachHang();
                        khachhang.Email = email;
                        khachhang.MatKhau = sb.ToString();
                        khachhang.TenKhachHang = tenkhachhang;
                        db.KhachHangs.Add(khachhang);
                        db.SaveChanges();
                        // Gửi email đăng ký thành công
                        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                        client.EnableSsl = true;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential("lomki12231@gmail.com", "poka gibp icls gkvh");

                        MailMessage mailMessage = new MailMessage();
                        mailMessage.From = new MailAddress("lomki12231@gmail.com");
                        mailMessage.To.Add(email);
                        mailMessage.Body = $"Đăng ký thành công \n Tài khoản: {email} \n Mật khẩu: {matkhau1}";
                        mailMessage.Subject = "Hasaki xin chào khách hàng mới !";
                        client.Send(mailMessage);
                        ViewBag.ThongBao = "Đăng ký thành công";
                    }
                    else
                        ViewBag.ThongBao = "Lỗi";
                }
            }
            return RedirectToAction("Index","Home");
        }
        [HttpGet]
        public ActionResult ForgotPass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPass(string email)
        {
            var db = new HasakiDatabaseEntities1();
            if (string.IsNullOrEmpty(email))
                ModelState.AddModelError(string.Empty, "Vui lòng nhập email");
            if (ModelState.IsValid)
            {
                var khach = db.KhachHangs.FirstOrDefault(k => k.Email == email);
                if (khach != null)
                {
                    // Gửi email đăng ký thành công
                    SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("lomki12231@gmail.com", "poka gibp icls gkvh");
                    Random random = new Random();
                    int rdn = random.Next(100000, 999999);
                    Session["rdn"] = rdn;
                    Session["email"] = email;
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress("lomki12231@gmail.com");
                    mailMessage.To.Add(email);
                    //mailMessage.Body = $"";
                    string url = Url.Action("SetPass", "LoginResgis", new { rdn = rdn }, protocol: Request.Url.Scheme);
                    mailMessage.Body = $"Mã xác nhận của bạn là:  {rdn} \nHoặc click vào link sau:  {url}";
                    mailMessage.Subject = "Verify code Hasaki";
                    client.Send(mailMessage);
                }
                else
                {
                    ViewBag.ThongBao = "Không tồn tại tài khoản";
                }
                return RedirectToAction("ChekCode", "LoginResgis");
            }
            return RedirectToAction("ChekCode", "LoginResgis");
        }
        [HttpGet]
        public ActionResult ChekCode()
        {
            if (Session["email"] != null)
            {
                return View();
            }
            return RedirectToAction("ForgotPass","LoginResgis");

        }
        [HttpPost]
        public ActionResult ChekCode(int code)
        {
            if (Session["email"] != null)
            {
                int rdn = int.Parse(Session["rdn"].ToString());
                if (code != rdn)
                {
                    ViewBag.ThongBao = "Sai mã";
                    return View();
                }
                Session["check"] = 1;
                return RedirectToAction("SetPass", "LoginResgis");
            }
            Session["check"] = null;
            return RedirectToAction("ForgotPass", "LoginResgis");
        }

        [HttpGet]
        public ActionResult SetPass(int? rdn)
        {
            if (rdn != null || (Session["email"] != null && Session["check"] != null))
            {
                return View();
            }
            return RedirectToAction("ForgotPass", "LoginResgis");
        }

        [HttpPost]
        public ActionResult SetPass(string matkhau1, string matkhau2, int? rdn)
        {
            if ((rdn != null || (Session["email"] != null) && Session["check"] != null))
            {
                if (matkhau1 != matkhau2)
                {
                    ModelState.AddModelError(string.Empty, "Mật khẩu không hợp lệ");
                }
                else
                {
                    var db = new HasakiDatabaseEntities1();
                    string email = Session["email"] as string;
                    var sha256 = SHA256.Create();
                    byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(matkhau1));

                    var sb = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        sb.Append(bytes[i].ToString("x2"));
                    }
                    string matkhau = sb.ToString();
                    var khach = db.KhachHangs.FirstOrDefault(k => k.Email == email);
                    khach.MatKhau = matkhau;
                    db.KhachHangs.AddOrUpdate(khach);
                    db.SaveChanges();
                }
                return RedirectToAction("Finish","LoginResgis");
            }
            ViewBag.ThongBao = "Hệ thống đang gặp lỗi. Hãy sử dụng mã xác thực của bạn";
            return View();
        }

        public ActionResult Finish() { return View(); }

        public ActionResult UserPatial()
        {
            return PartialView();
        }
    }
}