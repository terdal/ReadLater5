using AutoMapper;
using Data;
using Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.DTOs;
using Services.Helpers;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BookmarkService : IBookmarkService
    {
        private ReadLaterDataContext _ReadLaterDataContext;
        private readonly ILogger<BookmarkService> _logger;
        private readonly IMapper _mapper;
        private ICategoryService _categoryService;

        public BookmarkService(ReadLaterDataContext readLaterDataContext, ILogger<BookmarkService> logger, IMapper mapper, ICategoryService categoryService)
        {
            _ReadLaterDataContext = readLaterDataContext;
            _logger = logger;
            _mapper = mapper;
            _categoryService = categoryService;
        }

        public List<BookmarkDTO> GetBookmarks(string userId)
        {
            var bookmarks = new List<BookmarkDTO>();
            try
            {
                var dbBookmarks = _ReadLaterDataContext.Bookmark.Where(x => x.UserId == userId).Include("Category").ToList();
                bookmarks = _mapper.Map<List<BookmarkDTO>>(dbBookmarks);
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {0} Exception: {1}", ex.Message, ex);
            }
            return bookmarks;
        }

        public BookmarkDTO GetBookmark(int Id, string userId)
        {
            BookmarkDTO bookmark = null;
            try
            {
                var dbBookmark = _ReadLaterDataContext.Bookmark.Where(c => c.ID == Id && c.UserId == userId).Include("Category").FirstOrDefault();
                bookmark = _mapper.Map<BookmarkDTO>(dbBookmark);
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {0} Exception: {1}", ex.Message, ex);
            }

            return bookmark;
        }

        public ResultDTO<BookmarkDTO> CreateBookmark(UpsertBookmarkDTO bookmark, string userId)
        {
            var result = new ResultDTO<BookmarkDTO>();

            try
            {
                var dbBookmark = new Bookmark()
                {
                    URL = bookmark.URL,
                    ShortDescription = bookmark.ShortDescription,
                    CreateDate = DateTime.UtcNow,
                    CategoryId = GetBookmarkCategoryId(bookmark, userId),
                    UserId = userId
                };

                _ReadLaterDataContext.Add(dbBookmark);
                _ReadLaterDataContext.SaveChanges();
                result.Data = _mapper.Map<BookmarkDTO>(dbBookmark);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {0} Exception: {1}", ex.Message, ex);
                result.ErrorMessage = CommonHelpers.GetGeneralErrorMessage();
            }
            return result;
        }

        public ResultDTO<BookmarkDTO> UpdateBookmark(UpsertBookmarkDTO bookmark, string userId)
        {
            var result = new ResultDTO<BookmarkDTO>();

            try
            {
                var dbBookmark = _ReadLaterDataContext.Bookmark.Where(c => c.ID == bookmark.ID).FirstOrDefault();

                if (dbBookmark == null)
                {
                    result.ErrorMessage = $"Bookmark with id: {bookmark.ID} doesn't exist";
                    return result;
                }

                dbBookmark.URL = bookmark.URL;
                dbBookmark.ShortDescription = bookmark.ShortDescription;
                dbBookmark.CategoryId = GetBookmarkCategoryId(bookmark, userId);
                dbBookmark.UserId = userId;

                _ReadLaterDataContext.Update(dbBookmark);
                _ReadLaterDataContext.SaveChanges();
                result.Data = _mapper.Map<BookmarkDTO>(dbBookmark);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {0} Exception: {1}", ex.Message, ex);
                result.ErrorMessage = CommonHelpers.GetGeneralErrorMessage();
            }

            return result;
        }

        public ResultDTO DeleteBookmark(int bookmarkId, string userId)
        {
            var result = new ResultDTO();

            try
            {
                var dbBookmark = _ReadLaterDataContext.Bookmark.Where(c => c.ID == bookmarkId && c.UserId == userId).FirstOrDefault();

                if (dbBookmark == null)
                {
                    result.ErrorMessage = $"Bookmark with id: {bookmarkId} doesn't exist";
                    return result;
                }

                _ReadLaterDataContext.Bookmark.Remove(dbBookmark);
                _ReadLaterDataContext.SaveChanges();

                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {0} Exception: {1}", ex.Message, ex);
                result.ErrorMessage = CommonHelpers.GetGeneralErrorMessage();
            }

            return result;
        }

        private int? GetBookmarkCategoryId(UpsertBookmarkDTO bookmark, string userId)
        {
            try
            {
                int? categoryId;

                if (!bookmark.CategoryId.HasValue && String.IsNullOrEmpty(bookmark.CategoryName))
                    categoryId = null;
                else if (!bookmark.CategoryId.HasValue)
                {
                    var category = _mapper.Map<CategoryDTO>(_categoryService.GetCategory(bookmark.CategoryName.Trim(), userId));
                    if (category == null)
                    {
                        var dbCategory = _categoryService.CreateCategory(new CategoryDTO() { Name = bookmark.CategoryName.Trim() }, userId);
                        categoryId = dbCategory.Data?.ID;
                    }
                    else
                        categoryId = category.ID;
                }
                else
                    categoryId = bookmark.CategoryId.Value;

                return categoryId;
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {0} Exception: {1}", ex.Message, ex);
                throw;
            }
        }
    }
}
