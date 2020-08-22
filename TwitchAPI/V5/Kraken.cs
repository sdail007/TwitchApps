using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TwitchAPI;
using System.Text;
using System.Threading.Tasks;
using TwitchAPI.Command;
using TwitchAPI.Command.Decorators;
using TwitchAPI.Endpoints;
using TwitchAPI.Query;
using TwitchAPI.Query.Decorators;
using TwitchAPI.Query.GetChannelInfo;
using TwitchAPI.Query.GetGames;
using TwitchAPI.V5.Commands.UpdateChannelInfo;
using TwitchAPI.V5.Queries.GetChannelInfo;
using TwitchAPI.V5.Queries.GetGames;
using TwitchAPI.V5.Commands;
using TwitchAPI.V5.Queries;

namespace TwitchAPI.V5
{
    public class Kraken
    {
        private readonly KrakenEndpoint endpoint;

        private readonly Dictionary<Type, dynamic> queryHandlers;
        private readonly Dictionary<Type, dynamic> commandHandlers;

        private static bool TypeIsQueryType(Type type) => type.GetGenericAbstractTypeDefinition(typeof(QueryBase<>)) != null;

        public Kraken(KrakenEndpoint endpoint)
        {
            this.endpoint = endpoint;

            queryHandlers = CreateQueryHandlers(endpoint);
            commandHandlers = CreateCommandHandlers(endpoint);
        }

        private static Dictionary<Type, dynamic> CreateCommandHandlers(KrakenEndpoint endpoint)
        {
            Dictionary<Type, dynamic> commandHandlers = new Dictionary<Type, dynamic>();

            //List<Type> commandTypes = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
            //                         from type in assembly.GetTypes()
            //                         where typeof(CommandBase).IsAssignableFrom(type) &&
            //                         !type.IsAbstract
            //                         select type).ToList();

            var handlers = ReflectionExtensions.GetImplementationsOfAbstractType(typeof(KrakenCommandHandler<>), AppDomain.CurrentDomain.GetAssemblies())
                .Where(t => !t.IsGenericTypeDefinition).ToList();

            foreach (Type handlerType in handlers)
            {
                MethodInfo handleMethod = handlerType.GetMethod("HandleAsync");
                Type commandType = handleMethod.GetParameters()[0].ParameterType;
                //Type responseType = handleMethod.ReturnType.GenericTypeArguments[0];

                object handler = Activator.CreateInstance(handlerType, endpoint);

                Type decoratorType = typeof(RetryCommandDecorator<>).MakeGenericType(commandType);
                object decorated = Activator.CreateInstance(decoratorType, handler);

                commandHandlers.Add(commandType, (dynamic)decorated);
            }

            return commandHandlers;
        }

        private static Dictionary<Type, dynamic> CreateQueryHandlers(KrakenEndpoint endpoint)
        {
            Dictionary<Type, dynamic> queryHandlers = new Dictionary<Type, dynamic>();

            //List<Type> queryTypes = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
            //                         from type in assembly.GetTypes()
            //                         where TypeIsQueryType(type) &&
            //                         !type.IsAbstract
            //                         select type).ToList();

            var handlers = ReflectionExtensions.GetImplementationsOfAbstractType(typeof(KrakenQueryHandler<,>), AppDomain.CurrentDomain.GetAssemblies())
                .Where(t => !t.IsGenericTypeDefinition).ToList();

            foreach (Type handlerType in handlers)
            {
                MethodInfo handleMethod = handlerType.GetMethod("HandleAsync");
                Type queryType = handleMethod.GetParameters()[0].ParameterType;
                Type responseType = handleMethod.ReturnType.GenericTypeArguments[0];

                object handler = Activator.CreateInstance(handlerType, endpoint);

                Type decoratorType = typeof(RetryQueryDecorator<,>).MakeGenericType(queryType, responseType);
                object decorated = Activator.CreateInstance(decoratorType, handler);

                queryHandlers.Add(queryType, (dynamic)decorated);
            }

            return queryHandlers;
        }

        public async Task<TResponse> ExecuteQueryAsync<TResponse>(QueryBase<TResponse> query)
                        where TResponse : ResponseBase
        {
            Type queryType = query.GetType();
            object result = await queryHandlers[queryType].HandleAsync((dynamic)query);
            return (TResponse)result;
        }

        public async Task ExecuteCommandAsync(CommandBase command)
        {
            Type commandType = command.GetType();
            await commandHandlers[commandType].HandleAsync((dynamic)command);
        }
    }
}
