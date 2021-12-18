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

        public List<BookmarkDTO> GetBookmarks()
        {
            var bookmarks = new List<BookmarkDTO>();
            try
            {
                var dbBookmarks = _ReadLaterDataContext.Bookmark.Include("Category").ToList();
                bookmarks = _mapper.Map<List<BookmarkDTO>>(dbBookmarks);
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {0} Exception: {1}", ex.Message, ex);
            }
            return bookmarks;
        }

        public BookmarkDTO GetBookmark(int Id)
        {
            BookmarkDTO bookmark = null;
            try
            {
                var dbBookmark = _ReadLaterDataContext.Bookmark.Include("Category").Where(c => c.ID == Id).FirstOrDefault();
                bookmark = _mapper.Map<BookmarkDTO>(dbBookmark);
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {0} Exception: {1}", ex.Message, ex);
            }

            return bookmark;
        }

        public ResultDTO CreateBookmark(UpsertBookmarkDTO bookmark)
        {
            var result = new ResultDTO();

            try
            {
                var dbBookmark = new Bookmark()
                {
                    URL = bookmark.URL,
                    ShortDescription = bookmark.ShortDescription,
                    CreateDate = DateTime.UtcNow,
                    CategoryId = GetBookmarkCategoryId(bookmark)
                };

                _ReadLaterDataContext.Add(dbBookmark);
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

        public ResultDTO UpdateBookmark(UpsertBookmarkDTO bookmark)
        {
            var result = new ResultDTO();

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
                dbBookmark.CategoryId = GetBookmarkCategoryId(bookmark);

                _ReadLaterDataContext.Update(dbBookmark);
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

        public ResultDTO DeleteBookmark(int bookmarkId)
        {
            var result = new ResultDTO();

            try
            {
                var dbBookmark = _ReadLaterDataContext.Bookmark.Where(c => c.ID == bookmarkId).FirstOrDefault();

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

        private int? GetBookmarkCategoryId(UpsertBookmarkDTO bookmark)
        {
            try
            {
                int? categoryId;

                if (!bookmark.CategoryId.HasValue && String.IsNullOrEmpty(bookmark.CategoryName))
                    categoryId = null;
                else if (!bookmark.CategoryId.HasValue)
                {
                    var category = _mapper.Map<CategoryDTO>(_categoryService.GetCategory(bookmark.CategoryName.Trim()));
                    if (category == null)
                    {
                        var dbCategory = _categoryService.CreateCategory(new Category() { Name = bookmark.CategoryName.Trim() });
                        categoryId = dbCategory.ID;
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
