using System;

namespace Abp.Dummy.Project.Authors;

[Serializable]
public class AuthorExcelDownloadTokenCacheItem
{
    public string Token { get; set; } = string.Empty;
}
