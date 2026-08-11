using Ardalis.Specification;

namespace Irrigation.Domain.Repository
{
    public interface IRepository<T> : IRepositoryBase<T> where T : class;
}
