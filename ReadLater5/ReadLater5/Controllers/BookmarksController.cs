using AutoMapper;
using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReadLater5.Helpers;
using ReadLater5.Models;
using Services.DTOs;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReadLater5.Controllers
{
    [Authorize]
    public class BookmarksController : Controller
    {
        IBookmarkService _bookmarkService;
        ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private UserManager<ApplicationUser> _userManager;

        public BookmarksController(IBookmarkService bookmarkService, ICategoryService categoryService, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _bookmarkService = bookmarkService;
            _categoryService = categoryService;
            _mapper = mapper;
            _userManager = userManager;
        }
        // GET: Bookmarks
        public IActionResult Index()
        {
            var model = _mapper.Map<List<BookmarkModel>>(_bookmarkService.GetBookmarks(_userManager.GetUserId(User)));
            return View(model);
        }

        // GET: Bookmarks/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            var bookmark = _mapper.Map<BookmarkModel>(_bookmarkService.GetBookmark((int)id, _userManager.GetUserId(User)));
            if (bookmark == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }
            return View(bookmark);

        }

        // GET: Bookmarks/Create
        public IActionResult Create()
        {
            var model = new UpsertBookmarkModel();
            model.PossibleCategories = _mapper.Map<List<CategoryModel>>(_categoryService.GetCategories(_userManager.GetUserId(User)));
            return View(model);
        }

        //// POST: Bookmarks/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UpsertBookmarkModel bookmark)
        {
            if (ModelState.IsValid)
            {
                bool validationResult = Uri.IsWellFormedUriString(bookmark.URL, UriKind.Absolute);
                if (validationResult)
                {
                    var result = _bookmarkService.CreateBookmark(_mapper.Map<UpsertBookmarkDTO>(bookmark), _userManager.GetUserId(User));
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        ModelState.AddModelError("General", result.ErrorMessage);
                }
                else
                    ModelState.AddModelError("URL", "Invalid URL");
            }

            bookmark.Categories = MvcHelpers.GetBookmarkCompanySelectList(bookmark.PossibleCategories);
            return View(bookmark);
        }

        // GET: Bookmarks/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            var bookmark = _mapper.Map<UpsertBookmarkModel>(_bookmarkService.GetBookmark((int)id, _userManager.GetUserId(User)));
            if (bookmark == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }
            bookmark.PossibleCategories = _mapper.Map<List<CategoryModel>>(_mapper.Map<List<CategoryDTO>>(_categoryService.GetCategories(_userManager.GetUserId(User))));
            return View(bookmark);
        }

        // POST: Bookmarks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UpsertBookmarkModel bookmark)
        {
            if (ModelState.IsValid)
            {
                bool validationResult = Uri.IsWellFormedUriString(bookmark.URL, UriKind.Absolute);
                if (validationResult)
                {
                    var result = _bookmarkService.UpdateBookmark(_mapper.Map<UpsertBookmarkDTO>(bookmark), _userManager.GetUserId(User));
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        ModelState.AddModelError("General", result.ErrorMessage);
                }
                else
                    ModelState.AddModelError("URL", "Invalid URL");
            }

            bookmark.Categories = MvcHelpers.GetBookmarkCompanySelectList(bookmark.PossibleCategories);
            return View(bookmark);
        }

        // GET: Bookmarks/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            var bookmark = _mapper.Map<BookmarkModel>(_bookmarkService.GetBookmark((int)id, _userManager.GetUserId(User)));
            if (bookmark == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }
            return View(bookmark);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _bookmarkService.DeleteBookmark(id, _userManager.GetUserId(User));
            if (result.Succeeded)
                return RedirectToAction("Index");

            ModelState.AddModelError("General", result.ErrorMessage);
            return View();
        }
    }
}
