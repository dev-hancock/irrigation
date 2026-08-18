using Ardalis.Specification;

namespace Irrigation.Application.Common;

public interface IRepository<T> : IRepositoryBase<T>
    where T : class;