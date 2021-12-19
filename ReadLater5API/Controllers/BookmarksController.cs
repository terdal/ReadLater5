using AutoMapper;
using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReadLater5API.Models;
using Services.DTOs;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ReadLater5API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookmarksController : ControllerBase
    {
        private readonly ILogger<BookmarksController> _logger;
        private readonly IMapper _mapper;
        IBookmarkService _bookmarkService;

        public BookmarksController(ILogger<BookmarksController> logger, IMapper mapper, IBookmarkService bookmarkService)
        {
            _logger = logger;
            _mapper = mapper;
            _bookmarkService = bookmarkService;
        }

        [Route("~/api/GetAllBookmarks")]
        [HttpGet]
        public IActionResult GetAllBookmarks()
        {
            var userId = User.FindFirst(x => x.Type == "UserId").Value;
            var result = _mapper.Map<List<BookmarkModel>>(_bookmarkService.GetBookmarks(userId));
            return Ok(result);
        }

        [Route("~/api/GetBookmark/{id}")]
        [HttpGet]
        public IActionResult GetBookmark(int? id)
        {
            if (id == null) return BadRequest();
            var userId = User.FindFirst(x => x.Type == "UserId").Value;
            var bookmark = _mapper.Map<BookmarkModel>(_bookmarkService.GetBookmark((int)id, userId));
            if (bookmark == null) return NotFound();
            return Ok(bookmark);
        }

        [Route("~/api/CreateBookmark")]
        [HttpPost]
        public IActionResult CreateBookmark([FromBody] UpsertBookmarkModel bookmark)
        {
            if (ModelState.IsValid)
            {
                bool validationResult = Uri.IsWellFormedUriString(bookmark.URL, UriKind.Absolute);
                if (validationResult)
                {
                    var userId = User.FindFirst(x => x.Type == "UserId").Value;
                    var result = _bookmarkService.CreateBookmark(_mapper.Map<UpsertBookmarkDTO>(bookmark), userId);
                    if (result.Succeeded)
                        return Ok(result.Data);
                    else
                        return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage);
                }
                else
                    throw new HttpListenerException((int)HttpStatusCode.BadRequest, "Invalid URL property");
            }

            return BadRequest(ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList());
        }

        [Route("~/api/UpdateBookmark")]
        [HttpPut]
        public IActionResult UpdateBookmark([FromBody] UpsertBookmarkModel bookmark)
        {
            if (ModelState.IsValid)
            {
                bool validationResult = Uri.IsWellFormedUriString(bookmark.URL, UriKind.Absolute);
                if (validationResult)
                {
                    var userId = User.FindFirst(x => x.Type == "UserId").Value;
                    var result = _bookmarkService.UpdateBookmark(_mapper.Map<UpsertBookmarkDTO>(bookmark), userId);
                    if (result.Succeeded)
                        return Ok(result.Data);
                    else
                        return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage);
                }
                else
                    return BadRequest("Invalid URL property");
            }
            return BadRequest(ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList());
        }

        [Route("~/api/DeleteBookmark/{id}")]
        [HttpDelete]
        public IActionResult DeleteBookmark(int id)
        {
            var userId = User.FindFirst(x => x.Type == "UserId").Value;
            var result = _bookmarkService.DeleteBookmark(id, userId);
            if (result.Succeeded)
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage);
        }
    }
}
