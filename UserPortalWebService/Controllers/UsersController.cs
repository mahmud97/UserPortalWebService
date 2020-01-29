using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UserPortalWebService.Models;
using UserPortalWebService.ViewModel;

namespace UserPortalWebService.Controllers
{
    public class UsersController : Controller
    {
        private MainDataContext db = new MainDataContext();

        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(User user, string button)
        {


            if (button == "Cancel")
            {

                ModelState.Clear();
                return View();

            }
            var emailExist = db.Users.Where(l => l.Email == user.Email).FirstOrDefault();

            if (emailExist != null)
            {
                ModelState.AddModelError("error", "This email already exists");
                return View();
            }
            if (ModelState.IsValid)
            {

                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError("Error", "Please register agian correctly");
            }


            return View(user);
        }

        //[HttpPost]
        //public JsonResult CheckEmail(string email)
        //{

        //    bool isValid = !db.Users.ToList().Exists(u => u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
        //    return Json(isValid);

        //}

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user, string button)
        {
           
            if (button == "Clear")
            {

                ModelState.Clear();
                return View();

            }


            using (MainDataContext db = new MainDataContext())
            {

                var userExist = db.Users.Where(u => u.Email.Equals(user.Email) && u.Password.Equals(user.Password)).FirstOrDefault();

                if (userExist != null)
                {

                    Session["FirstName"] = userExist.FirstName.ToString();
                    Session["LastName"] = userExist.LastName.ToString();
                    Session["Address"] = userExist.Address.ToString();
                    Session["Phone"] = userExist.Phone.ToString();


                    Session["Email"] = userExist.Email.ToString();
                    Session["Password"] = userExist.Password.ToString();
                    Session["BirthDate"] = userExist.BirthDate.ToString();

                    return RedirectToAction("UserProfile");

                }

            }





            
            return View(user);
        }

        public ActionResult UserProfile()
        {
            if (Session["Email"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }

        }


        


        public ActionResult ChangePassword(int? id)
        {
            var user = db.Users.SingleOrDefault(u => u.Id == id);
            if (user == null)
            {

                return HttpNotFound();
            }
            var viewModel = new UserViewModel
            {
                User = user

            };

            return View("ChangePassword", viewModel);


        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(User user, UserViewModel uvm)
        {
            if (!ModelState.IsValid)
            {

                var viewModel = new UserViewModel
                {
                    User = user

                };

                return View("ChangePassword", viewModel);

            }


           
            else
            {
                var updatePassword = db.Users.Single(c => c.Id == user.Id);

                updatePassword.Password = uvm.NewPassword;


            }
            db.SaveChanges();
            return RedirectToAction("Login", "Users");


        }

        
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Users");
        }


    }
}