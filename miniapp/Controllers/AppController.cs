using miniapp.EntityFrameworkCore.Context;
using miniapp.EntityFrameworkCore.Repository;
using miniapp.Services;
using miniapp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using miniapp.EntityFrameworkCore.Entities;
using miniapp.ApiControllers;

namespace miniapp.Controllers
{
    public class AppController: Controller
    {
        private readonly IMailService mailService;
        private readonly IMenuRepository menuRepository;

        public AppController(IMailService mailService, IMenuRepository menuRepository)
        {
            this.mailService = mailService;
            this.menuRepository = menuRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet("angular")]
        public IActionResult Angular()
        {
            return View();
        }

        [Authorize]
        [HttpGet("contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [Authorize]
        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                this.mailService.SendMessage("niju.mn@live.com", model.Subject, model.Message);
                ViewBag.UserMessage = "Message Sent";
                ModelState.Clear();
            }
            else
            {
                //show errors
            }
            return View();
        }

        [HttpGet("MenuList")]
        public IActionResult MenuList()
        {
            var results = this.menuRepository.GetAllMenus();
            return View(results );
        }
    }
}
