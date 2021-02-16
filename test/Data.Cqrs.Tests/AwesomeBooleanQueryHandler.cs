using Olbrasoft.Dispatching;
using System.Threading;
using System.Threading.Tasks;

namespace Olbrasoft.Data.Cqrs
{
    internal class AwesomeBooleanQueryHandler : QueryHandler<IRequest<bool>>
    {
        public AwesomeBooleanQueryHandler()
        {
        }

        public override Task<bool> HandleAsync(IRequest<bool> query, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }
    }
}