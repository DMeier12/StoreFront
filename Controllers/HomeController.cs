using Microsoft.AspNetCore.Mvc;
using StoreFront.Models;
using System.Diagnostics;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StoreFront.Data;
using Microsoft.EntityFrameworkCore;
using StoreFront.Helpers;
using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using JsonResult = Microsoft.AspNetCore.Mvc.JsonResult;

namespace StoreFront.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly StoreFrontContext _storeFrontContext;
        private readonly AuthorizationFilterContext _filterContext;
        public HomeController(ILogger<HomeController> logger, StoreFrontContext storeFrontContext, AuthorizationFilterContext filterContext)
        {
            _logger = logger;
            _storeFrontContext = storeFrontContext;
            _filterContext = filterContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        #region Inventory
        public IActionResult Inventory()
        {
            return View();
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public JsonResult GetProducts()
        {
            var products = _storeFrontContext.Products.ToList();
            return Json(products);
        }
        
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public JsonResult AddProduct(Products product)
        {
            var isSuccess = true;
            try
            {
                if (product.ProductID == 0)
                {
                    _storeFrontContext.Products.Add(product);
                }
                else
                {
                    _storeFrontContext.Products.Update(product);
                }
                _storeFrontContext.SaveChanges();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                isSuccess = false;
                throw;
            }
            
            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }

        [Microsoft.AspNetCore.Mvc.HttpDelete]
        public JsonResult DeleteProduct(Products product)
        {
            var isSuccess = true;
            try
            {
                _storeFrontContext.Products.Remove(product);
                _storeFrontContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                isSuccess = false;
                throw;
            }
            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }


        #endregion
        
        #region Catigories
        public IActionResult Catigories()
        {
            return View();
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public JsonResult GetCatigories()
        {
            var catigories = _storeFrontContext.Categories.ToList();
            return Json(catigories);
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public JsonResult AddCatigory(Categories catigory)
        {
            var isSuccess = true;
            try
            {
                if (catigory.CategoryID == 0)
                {
                    _storeFrontContext.Categories.Add(catigory);
                }
                else
                {
                    _storeFrontContext.Categories.Update(catigory);
                }
                _storeFrontContext.SaveChanges();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                isSuccess = false;
                throw;
            }

            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }

        [Microsoft.AspNetCore.Mvc.HttpDelete]
        public JsonResult DeleteCatigory(Categories catigory)
        {
            var isSuccess = true;
            try
            {
                _storeFrontContext.Categories.Remove(catigory);
                _storeFrontContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                isSuccess = false;
                throw;
            }
            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region UserManagement
        public IActionResult UserManagement()
        {
            return View();
        }
        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public JsonResult CheckLogin(string username, string password)
        {
            var session = _filterContext.HttpContext.Session;
            var md5StringPassword = password;//AppHelper.GetMd5Hash(password);
            var userValue = (from user in _storeFrontContext.User
                join role in _storeFrontContext.RolePermission on user.RoleId equals role.RoleID
                             where user.UserName == username && user.Password == md5StringPassword
                             select new {user.UserName,role.Role}).FirstOrDefault();

            var isLogged = true;
            if (userValue != null)
            {
                session.SetString("username", userValue.UserName);
                session.SetString("role", userValue.Role);
                isLogged = true;
            }
            else
            {
                isLogged = false;
            }
            return Json(isLogged, JsonRequestBehavior.AllowGet);
        }


        [AuthorizationFilter]
        public ActionResult UserCreate()
        {
            return View();
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public JsonResult SaveUser(User user)
        {
            var db = _storeFrontContext;
            bool isSuccess = true;

            if (user.UserId > 0)
            {
                db.Entry(user).State = EntityState.Modified;
            }
            else
            {
                user.Status = 1;
                user.Password = user.Password;//AppHelper.GetMd5Hash(user.Password);
                db.User.Add(user);
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                isSuccess = false;
            }

            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        public JsonResult GetAllUser()
        {
            var dataList = _storeFrontContext.User.Where(x => x.Status == 1).ToList();
            return Json(dataList, JsonRequestBehavior.AllowGet);
        }


        #endregion
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}