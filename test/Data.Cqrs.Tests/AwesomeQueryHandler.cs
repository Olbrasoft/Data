using Olbrasoft.Dispatching;
using Olbrasoft.Mapping;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Olbrasoft.Data.Cqrs
{
    public class AwesomeQueryHandler : QueryHandler<Request<string>, string>
    {
        public AwesomeQueryHandler(IProjector projector) : base(projector)
        {
        }

        public void CallProjectTo()
        {
            ProjectTo<string>(new List<char>().AsQueryable());
        }

        public override Task<string> HandleAsync(Request<string> query, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }
    }
}