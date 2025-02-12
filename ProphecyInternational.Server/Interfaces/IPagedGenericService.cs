using ProphecyInternational.Server.Models;

namespace ProphecyInternational.Server.Interfaces
{
    public interface IPagedGenericService<TModel>
    {
        Task<PagedResult<TModel>> GetPaginatedItemsAsync(int pageNumber, int pageSize);
    }
}
