using AutoMapper;
using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReadLater5.Models;
using Services.DTOs;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ReadLater5.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private UserManager<ApplicationUser> _userManager;

        public CategoriesController(ICategoryService categoryService, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _userManager = userManager;
        }
        // GET: Categories
        public IActionResult Index()
        {
            var model = _mapper.Map<List<CategoryModel>>(_categoryService.GetCategories(_userManager.GetUserId(User)));
            return View(model);
        }

        // GET: Categories/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            var category = _mapper.Map<CategoryModel>(_categoryService.GetCategory((int)id, _userManager.GetUserId(User)));
            if (category == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }
            return View(category);

        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                var result = _categoryService.CreateCategory(_mapper.Map<CategoryDTO>(category), _userManager.GetUserId(User));
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    ModelState.AddModelError("General", result.ErrorMessage);
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            var category = _mapper.Map<CategoryModel>(_categoryService.GetCategory((int)id, _userManager.GetUserId(User)));
            if (category == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                var result = _categoryService.UpdateCategory(_mapper.Map<CategoryDTO>(category), _userManager.GetUserId(User));
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    ModelState.AddModelError("General", result.ErrorMessage);
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            var category = _mapper.Map<CategoryModel>(_categoryService.GetCategory((int)id, _userManager.GetUserId(User)));
            if (category == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _categoryService.DeleteCategory(id, _userManager.GetUserId(User));
            if (result.Succeeded)
                return RedirectToAction("Index");

            ModelState.AddModelError("General", result.ErrorMessage);
            return View();
        }
    }
}
