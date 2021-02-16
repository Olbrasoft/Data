using Microsoft.EntityFrameworkCore;
using Olbrasoft.Dispatching;
using Olbrasoft.Mapping;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Olbrasoft.Data.Cqrs.EntityFrameworkCore
{
    internal class AwesomeBooleanCommandHandler : DbCommandHandler<IRequest<bool>, DbContext, AwesomeEntity>
    {
        public AwesomeBooleanCommandHandler(IMapper mapper, IDbContextFactory<DbContext> factory) : base(mapper, factory)
        {
        }

        public override Task<bool> HandleAsync(IRequest<bool> command, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}