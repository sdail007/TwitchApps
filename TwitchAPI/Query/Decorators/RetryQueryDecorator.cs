using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TwitchAPI.Query.Decorators
{
    public class RetryQueryDecorator<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
        where TResponse : ResponseBase
        where TQuery : QueryBase<TResponse>
    {
        private readonly IQueryHandler<TQuery, TResponse> decorated;

        public RetryQueryDecorator(IQueryHandler<TQuery, TResponse> decorated)
        {
            this.decorated = decorated;
        }

        public async Task<TResponse> HandleAsync(TQuery query)
        {
            int delay = 1;

            while (true)
            {
                try
                {
                    return await decorated.HandleAsync(query);
                }
                catch (HttpRequestException)
                {
                    await Task.Delay(delay * 1000);
                    
                    //Increase delay exponentially, capped at 60
                    delay = Math.Min(delay * 2, 60);
                }
            }
        }
    }
}
