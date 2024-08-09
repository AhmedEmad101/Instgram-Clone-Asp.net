using instgaram.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace instgaram.Controllers
{
    public class CommentsController : ApiController
    {
        private Model model = new Model(); 
       public string AddPost()
       {
            
            int userId = Convert.ToInt32(HttpContext.Current.Request.Form["iduser"]);
            int postId = Convert.ToInt32(HttpContext.Current.Request.Form["idpost"]);
            
            string UserComment = HttpContext.Current.Request.Form["comment"];

            comment comment = new comment();
            comment.commenttext = UserComment;
            comment.User = model.Users.Find(userId);
            comment.Post = model.posts.Find(postId);
            
            model.comments.Add(comment);
            model.SaveChanges();
            return "";
       }
       
    }
}
