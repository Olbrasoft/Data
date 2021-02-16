using Olbrasoft.Dispatching;
using Olbrasoft.Mapping;
using System.Threading;
using System.Threading.Tasks;

namespace Olbrasoft.Data.Cqrs
{
    public abstract class CommandHandler<TCommand, TResult> : IRequestHandler<TCommand, TResult> where TCommand : IRequest<TResult>
    {
        private readonly IMapper _mapper;

        protected CommandHandler(IMapper mapper) => _mapper = mapper;

        public abstract Task<TResult> HandleAsync(TCommand command, CancellationToken token);

        protected TDestination MapTo<TDestination>(object source) => _mapper.MapTo<TDestination>(source);
    }
}