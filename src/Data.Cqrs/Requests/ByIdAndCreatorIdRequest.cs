using Olbrasoft.Dispatching;

namespace Olbrasoft.Data.Cqrs.Requests
{
    public class ByIdAndCreatorIdRequest : ByIdRequest
    {
        public int CreatorId { get; set; }

        public ByIdAndCreatorIdRequest(IRequestHandler<Request<bool>, bool> handler) : base(handler)
        {
        }

        public ByIdAndCreatorIdRequest(IDispatcher dispatcher) : base(dispatcher)
        {
        }
    }
}