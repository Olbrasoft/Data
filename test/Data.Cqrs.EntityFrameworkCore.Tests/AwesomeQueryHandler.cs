using Microsoft.EntityFrameworkCore;
using Olbrasoft.Dispatching;
using Olbrasoft.Mapping;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Olbrasoft.Data.Cqrs.EntityFrameworkCore
{
    internal class AwesomeQueryHandler : DbQueryHandler<Request<object>, object, DbContext, AwesomeEntity>
    {
        public new DbContext Context => base.Context;

        public new IQueryable<AwesomeEntity> Entities => base.Entities;

        public AwesomeQueryHandler(IProjector projector, IDbContextFactory<DbContext> factory) : base(projector, factory)
        {
        }

        public override Task<object> HandleAsync(Request<object> query, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }
    }
}