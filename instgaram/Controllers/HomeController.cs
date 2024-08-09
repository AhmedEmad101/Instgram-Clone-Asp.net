using instgaram.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace instgaram.Controllers
{
    public class HomeController : Controller
    {
        private Model Database = new Model();


        [HttpGet]
        public ActionResult Index()
        {
            
            Session["Userid"] = "0";

            return View();
        }

        [HttpPost]
        public ActionResult Index(String username , String password)
        {
            User user = 
                Database.Users.Where(u => u.Username == username && u.Password == password).First();
            Session["Userid"] = user.Id.ToString();
            return RedirectToAction("Index", "Porfile");
        }


        [HttpGet]
        public ActionResult Signup()
        {
            
            Session["Stuts"] = "0";
            Session["Userid"] = "0";

            return View();
        }


        [HttpPost]
        public ActionResult Signup(FormCollection form,string Repass,HttpPostedFileBase photo)
        {
            User user = new User();
            user.FName = form["Fname"].ToString(); 
            user.LName = form["Lname"].ToString(); 
            user.Username = form["username"].ToString();
            user.Mobile = form["Phone"].ToString();
            user.Password = form["password"].ToString();
            
            HttpPostedFileBase postedFile = Request.Files["photo"];
            string path = Server.MapPath("~/Uploads/");
            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName)); 
            user.Photo = "/Uploads/" + Path.GetFileName(postedFile.FileName).ToString();

            
            Database.Users.Add(user);
            Database.SaveChanges();
            
            return View();
        }
    }
}