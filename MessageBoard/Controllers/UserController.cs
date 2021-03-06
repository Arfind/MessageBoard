﻿using MessageBoard.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MessageBoard.Controllers
{
    public class UserController : Controller
    {
        private DataContext dataContext = new DataContext();

        //注册页面
        public ActionResult Register()
        {
            return View();
        }

        //注册
        [HttpPost]
        public ActionResult Register(User user)
        {
            user.CreateTime = DateTime.Now;
            dataContext.Users.Add(user);
            dataContext.SaveChanges();
            return RedirectToAction("Login");
        }

        //登陆页面
        public ActionResult Login()
        {
            return View();
        }

        //登陆
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var user = from u in dataContext.Users
                       where u.Username == username && u.Password == password
                       select u;
            if (user.Count() > 0)
            {
                User u = user.First();

                Session["Username"] = u.Username;
                Session["UserId"] = u.UserId;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //登陆失败页面
                return Content("用户名或密码错误");
            }

        }

        //用户个人信息页面
        public ActionResult Detail()
        {
            string username = (string)Session["username"];
            if (username != null)
            {
                var user = from u in dataContext.Users
                           where u.Username == username
                           select u;
                User users = user.First();
                return View(users);
            }
            else
            {
                return Content("未登陆");
            }

        }

        //修改用户个人信息页面
        public ActionResult Edit(int id)
        {
            User user = dataContext.Users.Find(id);
            if (user == null)
                return HttpNotFound();
            return View(user);
        }

        //修改用户个人信息
        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (user != null)
            {
                var updateUser = dataContext.Users.Find(user.UserId);
                updateUser.Password = user.Password;
                updateUser.Sex = user.Sex;
                updateUser.Email = user.Email;
                dataContext.SaveChanges();
                return RedirectToAction("Detail");
            }
            return View(user);
        }

        //登出
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}