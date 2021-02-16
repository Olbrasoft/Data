using Microsoft.EntityFrameworkCore;
using Olbrasoft.Dispatching;
using System.Threading;
using System.Threading.Tasks;

namespace Olbrasoft.Data.Cqrs.EntityFrameworkCore
{
    internal class AwesomeBooleanQueryHandler : DbQueryHandler<IRequest<bool>, DbContext, AwesomeEntity>
    {
        public AwesomeBooleanQueryHandler(IDbContextFactory<DbContext> contextFactory) : base(contextFactory)
        {
        }

        public override Task<bool> HandleAsync(IRequest<bool> query, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        internal DbContext GetProtectedPropertyContext()
        {
            return Context;
        }

        internal object GetProtectedProperyEntities()
        {
            return Entities;
        }
    }
}