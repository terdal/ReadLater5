using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReadLater5API.Models;
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
    //[Authorize]
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

        [HttpGet]
        public IActionResult Get()
        {
            //var result = _mapper.Map<List<BookmarkModel>>(_bookmarkService.GetBookmarks());
            //return Ok(result);
            return Ok();
        }
    }
}
