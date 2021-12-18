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
        List<BookmarkDTO> GetBookmarks();
        BookmarkDTO GetBookmark(int Id);
        ResultDTO CreateBookmark(UpsertBookmarkDTO bookmark);
        ResultDTO UpdateBookmark(UpsertBookmarkDTO bookmark);
        ResultDTO DeleteBookmark(int bookmarkId);
    }
}
