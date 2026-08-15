using Ardalis.Specification.EntityFrameworkCore;
using Irrigation.Domain.Repository;

namespace Irrigation.Infrastructure.Persistence;

public class Repository<T>(IrrigationDbContext db) : RepositoryBase<T>(db), IRepository<T> where T : class;