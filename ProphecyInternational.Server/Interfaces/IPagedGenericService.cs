using ProphecyInternational.Server.Models;

namespace ProphecyInternational.Server.Interfaces
{
    public interface IPagedGenericService<TModel>
    {
        Task<PagedResult<TModel>> GetAllPaginatedAsync(int pageNumber, int pageSize);
    }
}
