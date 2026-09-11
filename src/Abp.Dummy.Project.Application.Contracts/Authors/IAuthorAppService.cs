using System;
using System.Threading.Tasks;
using Abp.Dummy.Project.Shared;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Abp.Dummy.Project.Authors;

public interface IAuthorAppService :
    ICrudAppService<
        AuthorDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateAuthorDto>
{
    Task<IRemoteStreamContent> GetListAsExcelFileAsync(AuthorExcelDownloadDto input);

    Task<DownloadTokenResultDto> GetDownloadTokenAsync();
}
