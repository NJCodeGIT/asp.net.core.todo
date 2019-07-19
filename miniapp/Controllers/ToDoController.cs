using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using miniapp.EntityFrameworkCore.Entities;
using miniapp.EntityFrameworkCore.Repository;
using miniapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace miniapp.Controllers
{
    public class ToDoController: Controller
    {
        
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;
        private readonly IGenericRepository<ToDo> repository;

        private Task<AppUser> GetCurrentUserAsync() => this.userManager.GetUserAsync(HttpContext.User);

        public ToDoController(IMapper mapper, UserManager<AppUser> userManager, IGenericRepository<ToDo> repository)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.repository = repository;
        }

        [Authorize]
        [HttpGet("todo")]
        public async Task<IActionResult> Index()
        {
            ToDoViewModel viewModel = new ToDoViewModel();
            var user = await GetCurrentUserAsync();
            viewModel.ToDoViewModelList = this.mapper.Map<IEnumerable<ToDo>, IEnumerable<ToDoViewModel>>(this.repository.GetAllByUser(user)).ToList();
            return View(viewModel);
        }

        [Authorize]
        [HttpPost("todo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ToDoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = false;
                var newModel = this.mapper.Map<ToDoViewModel, ToDo>(model);
                var currentUser = await GetCurrentUserAsync();

                newModel.ModifiedBy = currentUser;

                if (newModel.Id > 0)
                {
                    result = this.repository.Update(newModel, currentUser);
                }
                else
                {
                    newModel.CreatedBy = currentUser;
                    result = this.repository.Insert(newModel);
                }
                return RedirectToAction("Index", "ToDo");
            }
            else
            {
                ModelState.AddModelError("", "Invalid entry");
            }
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await GetCurrentUserAsync();
            var list = this.mapper.Map<IEnumerable<ToDo>, IEnumerable<ToDoViewModel>>(this.repository.GetAllByUser(user)).ToList();
            var returnModel = list.FirstOrDefault(rw => rw.Id == id);
            return View(returnModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ToDoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = false;
                var newModel = this.mapper.Map<ToDoViewModel, ToDo>(model);
                var currentUser = await GetCurrentUserAsync();

                newModel.ModifiedBy = currentUser;
                newModel.ModifiedOn = DateTime.UtcNow;

                if (newModel.Id > 0)
                {
                    var existEntity = this.repository.Entities.FirstOrDefault(item => item.Id == newModel.Id);
                    if (existEntity!=null)
                    {
                        existEntity.Title = newModel.Title;
                        existEntity.DueDate = newModel.DueDate;
                        existEntity.Status = newModel.Status;
                        result = this.repository.Update(existEntity, currentUser);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Update action failed");
                    }
                }
                else
                {
                    newModel.CreatedBy = currentUser;
                    newModel.CreatedOn = DateTime.UtcNow;
                    result = this.repository.Insert(newModel);
                }
                return RedirectToAction("Index", "ToDo");
            }
            else
            {
                ModelState.AddModelError("", "Invalid entry");
            }
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
            {
                var currentUser = await GetCurrentUserAsync();
                var existEntity = this.repository.Entities.FirstOrDefault(item => item.Id == id);
                if (existEntity != null)
                {
                    this.repository.Delete(existEntity, currentUser);
                }
                else
                {
                    ModelState.AddModelError("", "Delete action failed");
                }
            }

            return RedirectToAction("Index", "ToDo");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> StatusChange(int id)
        {
            if (id > 0)
            {
                var currentUser = await GetCurrentUserAsync();
                var existEntity = this.repository.Entities.FirstOrDefault(item => item.Id == id);
                if (existEntity != null)
                {
                    existEntity.Status = !existEntity.Status;
                    this.repository.Update(existEntity, currentUser);
                }
                else
                {
                    ModelState.AddModelError("", "Delete action failed");
                }
            }

            return RedirectToAction("Index", "ToDo");
        }
    }
}
