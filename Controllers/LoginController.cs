using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuctionProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace AuctionProject.Controllers
{
    public class LoginController : Controller
        {
        private UserContext _context;
    
        public LoginController(UserContext context)
        {
            _context = context;
        }
    
        [HttpGet]
        [Route("")]
        public IActionResult LoginPage()
        {
            // List<Person> AllUsers = _context.Users.ToList();
            // Other code
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(UserViewModel UserInput)
        {
            if(ModelState.IsValid)
            {
                List<User> CheckUsername = _context.users.Where(user => user.Username==UserInput.Username).ToList();
                //Checks to see if Username already exists in DB
                if(CheckUsername.Count==0)
                {
                    PasswordHasher<UserViewModel> Hasher = new PasswordHasher<UserViewModel>();
                    UserInput.Password = Hasher.HashPassword(UserInput, UserInput.Password);
                    User NewUser = new User
                    {
                        FirstName = UserInput.FirstName,
                        LastName = UserInput.LastName,
                        Username = UserInput.Username,
                        Password = UserInput.Password,
                        Wallet = 1000.00f
                    };
                    _context.Add(NewUser);
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("CurId", NewUser.UserId);
                    HttpContext.Session.SetString("CurUser",NewUser.FirstName);
                    return RedirectToAction("Dashboard", "Home");
                }
                ViewBag.error="Username already exisits in database";
                return View("LoginPage");   
            }
            return View("LoginPage", UserInput);
        }
        
        
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(string LogUsername, string LogPass)
        {
            //Checks if Username exists, if not redirects
            List<User> CheckUser = _context.users.Where(user => user.Username == LogUsername).ToList();
            if(CheckUser.Count==0){
                ViewBag.error="Username does not exist in database";
                return View("LoginPage");
            }
            //Checks if password is correct, if not redirects
            var Hasher = new PasswordHasher<User>();
            if(Hasher.VerifyHashedPassword(CheckUser[0], CheckUser[0].Password, LogPass)==0){
                ViewBag.error="password is incorrect";
                return View("LoginPage");
            }
            //If all checks pass, allows the user in
            HttpContext.Session.SetInt32("CurId", CheckUser[0].UserId);
            HttpContext.Session.SetString("CurUser",CheckUser[0].FirstName);
            return RedirectToAction("Dashboard", "Home");
        }


        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LoginPage");
        }

        public IActionResult Error()
        {
            return RedirectToAction("Dashboard", "Home");
        }
    }
}
