using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Zad2.Models;
using Zad2.Models.TodoViewModels;


namespace Zad2.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        // Inject user manager into constructor
        public TodoController(ITodoRepository repository,
        UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public IActionResult NewItem()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NewItem(AddTodoViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
                Guid userId = new Guid(currentUser.Id);
                var item = new TodoItem(model.Text, userId);
                _repository.Add(item);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Text is required field.");
            }
            return View();
        }

        //public IActionResult Get(Guid todoId, Guid userId)
        //{
        //    var item = _repository.Get(todoId, userId);
        //    return View(item);
        //}

        //public IActionResult Add(TodoItem todoItem)
        //{
        //    _repository.Add(todoItem);
        //    return View();
        //}

        public async Task<IActionResult> Remove(Guid todoId)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            Guid userId = new Guid(currentUser.Id);
            _repository.Remove(todoId, userId);
            return RedirectToAction("GetCompleted");
        }

        //public IActionResult Update(TodoItem todoItem, Guid userId)
        //{
        //    _repository.Update(todoItem, userId);
        //    return View();
        //}

        public async Task<IActionResult> MarkAsCompleted(Guid id)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            Guid userId = new Guid(currentUser.Id);
            _repository.MarkAsCompleted(id, userId);
            return RedirectToAction("Index");
        }

        //public IActionResult GetActive(Guid userId)
        //{
        //    var activeItems = _repository.GetActive(userId);
        //    return View(activeItems);
        //}

        //public IActionResult GetAll(Guid userId)
        //{
        //    var allItems = _repository.GetAll(userId);
        //    return View(allItems);
        //}

        public async Task<IActionResult> GetCompleted()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            Guid userId = new Guid(currentUser.Id);
            var completedItems = _repository.GetCompleted(userId);
            return View(completedItems);
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            Guid userId = new Guid(currentUser.Id);
            var activeItems = _repository.GetActive(userId);
            return View(activeItems);
        }

        

        
    }
}