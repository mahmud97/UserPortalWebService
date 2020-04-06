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



        public ActionResult Login()
        {
            return View();
        }



        public ActionResult LoginNext(User user, string button)
        {


            if (user == null)
            {
                return HttpNotFound();
            }

            if (button == "Clear")
            {

                ModelState.Clear();
                return View("Login");

            }




            using (MainDataContext db = new MainDataContext())
            {

                var userExist = db.Users.Where(u => u.Email.Equals(user.Email) ).FirstOrDefault();

                if (userExist == null)
                    return HttpNotFound("Email does not found in the database");


                var check = userExist.Email.Equals(user.Email) && userExist.Password.Equals(user.Password);

                if (!check)
                {
                    ModelState.AddModelError("error", "email and password do not match");
                    return View("Login");
                }

                if (userExist != null && check)
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






            return View("Login", user);
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





        public ActionResult ChangePassword(string email)
        {
            
            var user = db.Users.Where(u => u.Email == email).SingleOrDefault();
            if (user == null)
            {
                return HttpNotFound("email was not found");
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
            //if (!ModelState.IsValid)
            //{

            //    var viewModel = new UserViewModel
            //    {
            //        User = user

            //    };

            //    return View("ChangePassword", viewModel);

            //}

            if (uvm.NewPassword == null)
                return HttpNotFound("New password entered null");

            var isPassenteredSame = db.Users.Where(p => p.Password == uvm.NewPassword).FirstOrDefault();

            
            if (isPassenteredSame!=null)
            {
                ModelState.AddModelError("Error", "Please Enter a new password you entered the previous password");
                return View("ChangePassword");
            }


            var updatePassword = db.Users.Single(c => c.Id == uvm.User.Id);
            updatePassword.Password = uvm.NewPassword;
            db.SaveChanges();
            return RedirectToAction("Successful", updatePassword);


        }

        public ActionResult Successful(User user)
        {
            return View(user);
        }


        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Users");
        }


    }
}