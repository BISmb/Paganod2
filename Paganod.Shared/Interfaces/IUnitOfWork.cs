using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Paganod.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepositoryAsync<T> Repository<T>() where T : class; // AuditableEntity;

        Task<int> Commit(CancellationToken cancellationToken);

        Task Rollback();
    }
}
