using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using instgaram.Models;
namespace instgaram.Controllers
{ 
    public class PorfileController : Controller
    {
        private Model db = new Model();
        // GET: Porfile
        public ActionResult Index()
        {
            if(Session["Userid"] == "0" && Session["Userid"]==null)
                { return RedirectToAction("Index", "Home"); }
            User_account an_Account =db.Users.Find(Convert.ToInt32(Session["Userid"]));
            return View(an_Account);
        }
        [HttpGet]
        public ActionResult add_post()
        {
            if (Session["Userid"] == "0" && Session["Userid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult add_post(HttpPostedFileBase photo)
        {
            HttpPostedFileBase postedFile = Request.Files["photo"];
            string path = Server.MapPath("~/photopost/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
            post post = new post();
            post.photo = "/photopost/" + Path.GetFileName(postedFile.FileName);
            post.user = db.Users.Find(Convert.ToInt32(Session["Userid"]));
            db.posts.Add(post);
            db.SaveChanges();
            return RedirectToAction("index");
        }
        public ActionResult All_Users()
        {
            int usrID = Convert.ToInt32(Session["Userid"]);
            List<User_account> users = db.Users.Where(
                m => m.Id != usrID
                ).ToList();
            
            List<Friend_Request> friend_Res = db.friend_Res.
                Where(m => m.sender1_id == usrID).ToList();
                    foreach (var item in friend_Res)
                    {
                        users.Remove(item.resever);
                    }
                    return View(users);
        }

        public ActionResult Sent_Request()
        {
            int usrID = Convert.ToInt32(Session["Userid"]);
            List<Friend_Request> friend_Res = db.friend_Res.Where(
                m => m.sender1_id == usrID).ToList();
            return View(friend_Res);
        }

        public ActionResult add_Friend(int? id)
        {
            Friend_Request friend_Re = new Friend_Request();

            int usrID = Convert.ToInt32(Session["Userid"]);
            int idi = Convert.ToInt32(id);
            try
            {
               List<Friend_Request> request =
                    db.friend_Res.Where(
                        m => m.sender1_id == usrID && m.resever1_id == idi
                        ).ToList();
                db.friend_Res.RemoveRange(request);
                db.SaveChanges();
            }
            catch{}
            friend_Re.sender1_id = usrID;
            friend_Re.sender = db.Users.Find(usrID);
            friend_Re.resever1_id = idi;
            friend_Re.resever = db.Users.Find(idi);
            db.friend_Res.Add(friend_Re);
            db.SaveChanges();
            return RedirectToAction("Sent_Request");
        }
     
        public ActionResult cancle(int? id)
        {
            int usrID = Convert.ToInt32(Session["Userid"]);
            int idi = Convert.ToInt32(id);
            try
            {
                List<Friend_Request> request =
                   db.friend_Res.Where(
                       m => m.sender1_id == usrID && m.resever1_id == idi
                       ).ToList();
                db.friend_Res.RemoveRange(request);
                db.SaveChanges();
            }
            catch{ }
            return RedirectToAction("Sent_Request");
        }

        public ActionResult Get_Friends (FormCollection form)
        {
            int usrID = Convert.ToInt32(Session["Userid"]);
            string name = form["name"].ToString();
            List<User_account> users = db.Users.Where(m => m.FName == name).ToList();
            return View(users);
        }

        public ActionResult Friend_Requests()
        {
            int usrID = Convert.ToInt32(Session["Userid"]);
            List<Friend_Request> users =
                db.friend_Res.Where(m => m.resever1_id == usrID).ToList();
            return View(users);
        }
    }
}