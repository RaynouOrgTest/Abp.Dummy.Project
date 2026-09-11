using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Abp.Dummy.Project.Authors;
using Abp.Dummy.Project.Books;

namespace Abp.Dummy.Project;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ProjectBookToBookDtoMapper : MapperBase<Book, BookDto>
{
    [MapperIgnoreTarget(nameof(BookDto.AuthorName))]
    public override partial BookDto Map(Book source);

    [MapperIgnoreTarget(nameof(BookDto.AuthorName))]
    public override partial void Map(Book source, BookDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ProjectCreateUpdateBookDtoToBookMapper : MapperBase<CreateUpdateBookDto, Book>
{
    public override partial Book Map(CreateUpdateBookDto source);

    public override partial void Map(CreateUpdateBookDto source, Book destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ProjectAuthorToAuthorDtoMapper : MapperBase<Author, AuthorDto>
{
    public override partial AuthorDto Map(Author source);

    public override partial void Map(Author source, AuthorDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ProjectCreateUpdateAuthorDtoToAuthorMapper : MapperBase<CreateUpdateAuthorDto, Author>
{
    public override partial Author Map(CreateUpdateAuthorDto source);

    public override partial void Map(CreateUpdateAuthorDto source, Author destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ProjectAuthorToAuthorExcelDtoMapper : MapperBase<Author, AuthorExcelDto>
{
    public override partial AuthorExcelDto Map(Author source);

    public override partial void Map(Author source, AuthorExcelDto destination);
}
