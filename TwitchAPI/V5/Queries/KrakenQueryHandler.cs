using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchAPI.Endpoints;
using TwitchAPI.Query;

namespace TwitchAPI.V5.Queries
{
    public abstract class KrakenQueryHandler<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
        where TResponse : ResponseBase
        where TQuery : QueryBase<TResponse>
    {
        public KrakenQueryHandler(KrakenEndpoint endpoint)
        {
            Endpoint = endpoint;
        }

        public KrakenEndpoint Endpoint { get; }

        public TResponse Handle(TQuery query) => HandleAsync(query)
                                                    .ConfigureAwait(false)
                                                    .GetAwaiter()
                                                    .GetResult();

        public abstract Task<TResponse> HandleAsync(TQuery query);
    }
}
