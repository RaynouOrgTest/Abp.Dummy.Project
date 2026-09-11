using System;

namespace Abp.Dummy.Project.Books;

[Serializable]
public class BookExcelDownloadTokenCacheItem
{
    public string Token { get; set; } = string.Empty;
}
