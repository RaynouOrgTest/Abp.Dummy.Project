using System;

namespace Abp.Dummy.Project.Authors;

public class AuthorExcelDto
{
    public string Name { get; set; }

    public DateTime BirthDate { get; set; }

    public string? ShortBio { get; set; }
}
