using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchAPI.Query
{
    public interface IQueryHandler<in TQuery, TResponse>
        where TResponse : ResponseBase
        where TQuery : QueryBase<TResponse>
    {
        Task<TResponse> HandleAsync(TQuery query);
    }
}
