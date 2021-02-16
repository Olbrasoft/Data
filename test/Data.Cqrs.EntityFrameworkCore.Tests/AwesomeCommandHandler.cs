using Microsoft.EntityFrameworkCore;
using Olbrasoft.Dispatching;
using Olbrasoft.Mapping;
using System.Threading;
using System.Threading.Tasks;

namespace Olbrasoft.Data.Cqrs.EntityFrameworkCore
{
    internal class AwesomeCommandHandler : DbCommandHandler<IRequest<int>, int, DbContext, AwesomeEntity>
    {
        public AwesomeCommandHandler(IMapper mapper, IDbContextFactory<DbContext> factory) : base(mapper, factory)
        {
        }

        public override Task<int> HandleAsync(IRequest<int> command, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        internal DbContext GetProtectedPropertyContext()
        {
            return Context;
        }

        internal DbSet<AwesomeEntity> GetProtectedPropertyEntities()
        {
            return Entities;
        }
    }
}