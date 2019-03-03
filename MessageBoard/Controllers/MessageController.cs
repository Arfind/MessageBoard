﻿using MessageBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MessageBoard.Controllers
{
    public class MessageController : Controller
    {
        private DataContext dataContext = new DataContext();

        public ActionResult Index(int id)
        {
            var topic = dataContext.Topics.Find(id);
            var messages = dataContext.Messages.Where(m => m.TopicId == id).ToList();
            var replies = dataContext.Replies.Where(r => r.Message.TopicId == id).ToList();
            ViewBag.Topic = topic;
            ViewBag.Messages = messages;
            ViewBag.Replies = replies;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Message message)
        {
            message.UserId = (int)Session["UserId"];
            message.CreateTime = DateTime.Now;
            dataContext.Messages.Add(message);
            dataContext.SaveChanges();
            return RedirectToAction("Index/" + message.TopicId);
        }

        public ActionResult MyMessage()
        {
            if (Session["UserId"] != null)
            {
                int UserId = (int)Session["UserId"];
                var messages = dataContext.Messages.Where(m => m.UserId == UserId).ToList();
                return View(messages);
            }
            else
                return Content("未登陆");
        }

        public ActionResult Delete(int id)
        {
            var message=dataContext.Messages.Find(id);
            if (message != null)
            {
                dataContext.Messages.Remove(message);
                dataContext.SaveChanges();
                return RedirectToAction("MyMessage");
            }
            else
                return Content("该留言不存在");
        }

        public ActionResult Edit(int id)
        {
            var message = dataContext.Messages.Find(id);
            if(message!=null)
            {
                return View(message);
            }
            else
                return Content("该留言不存在");
        }

        [HttpPost]
        public ActionResult Edit(Message message)
        {
            var updateMessage=dataContext.Messages.Find(message.MessageId);
            updateMessage.MessageContent = message.MessageContent;
            dataContext.SaveChanges();
            return RedirectToAction("MyMessage");
        }
    }
}