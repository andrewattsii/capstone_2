using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Week6Capstone.Models;

namespace Week6Capstone.Controllers
{
    public class UserTaskController : Controller
    {
        UserTaskListDBEntities ORM = new UserTaskListDBEntities();
        

        // GET: UserTask
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        public ActionResult SaveNewUser(User newUser)
        {
            if (ModelState.IsValid)
            {
                ORM.Users.Add(newUser);
                ORM.SaveChanges();
                return RedirectToAction("../Home/Index");
            }
            else
            {
                ViewBag.ErrorMessage = "Something didn't go right..";
                return View("CreateUser");
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult UserPage(User loginUser)
        {
            if (Session["CurrentUser"] != null)
            {
                return View();
            }

            List<User> userList = ORM.Users.ToList();

            User user = userList.Find(u => u.Email == loginUser.Email);

            if (user != null)
            {
                if (loginUser.Password == user.Password)
                {
                    loginUser.Id = user.Id;
                    Session["CurrentUser"] = loginUser;
                    return View();
                }
                else
                {
                    ViewBag.ErrorMessage = "Email or Password was incorrect...";
                    return RedirectToAction("Login");
                }
            }

            ViewBag.ErrorMessage = "There are no users with this email address..";
            return RedirectToAction("Login");
        }

        public ActionResult TaskList()
        {
            User user = (User)Session["CurrentUser"];
            ViewBag.taskList = ORM.Tasks.ToList().FindAll(t=>t.UserID == user.Id).ToList();

            return View();
        }
        public ActionResult AddTask(Task newTask)
        {
            User user = (User)Session["CurrentUser"];

            if (ModelState.IsValid)
            {
                newTask.UserID = user.Id;
                ORM.Tasks.Add(newTask);
                ORM.SaveChanges();
                return RedirectToAction("TaskList");
                
            }
            else
            {
                ViewBag.ErrorMessage = "Something didn't go right..";
                return View("TaskForm");
            }
        }

        public ActionResult TaskForm()
        {
            return View();
        }

        public ActionResult DeleteTask(int TaskID)
        {
            
            Task found = ORM.Tasks.Find(TaskID);
            ORM.Tasks.Remove(found);
            ORM.SaveChanges();
            return RedirectToAction("TaskList");
        }

        public ActionResult UpdateTask(int TaskID)
        {

            ORM.Tasks.Find(TaskID).Status = "True";
            ORM.SaveChanges();
            return RedirectToAction("TaskList");
        }

        public ActionResult Logout()
        {
            Session.Remove("CurrentUser");
            return RedirectToAction("../Home/Index");
        }
    }
}