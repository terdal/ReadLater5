using Entity;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBookmarkService
    {
        List<BookmarkDTO> GetBookmarks(string userId);
        BookmarkDTO GetBookmark(int Id, string userId);
        ResultDTO<BookmarkDTO> CreateBookmark(UpsertBookmarkDTO bookmark, string userId);
        ResultDTO<BookmarkDTO> UpdateBookmark(UpsertBookmarkDTO bookmark, string userId);
        ResultDTO DeleteBookmark(int bookmarkId, string userId);
    }
}
