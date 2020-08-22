using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TwitchAPI
{
    public static class ReflectionExtensions
    {
        //This code can be modified to be used in BetterContainer to register all implementations of a generic interface
        //For example: Container.Register(typeof(IRepository<>), DataAccessAssembly);
        public static IEnumerable<Type> GetImplementationsOfOpenInterface(Type interfaceType, params Assembly[] assemblies)
        {
            if (!interfaceType.IsInterface)
            {
                throw new InvalidOperationException("Operation is only valid on Interface Types");
            }

            if (!interfaceType.IsGenericTypeDefinition)
            {
                throw new InvalidOperationException("Operation is only valid on Generic Interfaces");
            }

            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    foreach (Type implementedInterface in type.GetInterfaces())
                    {
                        if (implementedInterface.IsGenericType &&
                            implementedInterface.GetGenericTypeDefinition() == interfaceType)
                        {
                            //implementedInterface registers to type
                            yield return type;
                        }
                    }
                }
            }
        }

        public static IEnumerable<Type> GetImplementationsOfAbstractType(Type abstractType, params Assembly[] assemblies)
        {
            if (!abstractType.IsAbstract)
                throw new InvalidOperationException("Operation is only valid on Abstract Types");

            if (!abstractType.IsGenericTypeDefinition)
            {
                throw new InvalidOperationException("Operation is only valid on Generic Abstract Types");
            }

            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    Type implementation = GetGenericAbstractTypeDefinition(type, abstractType);

                    if (implementation != null)
                        yield return type;
                }
            }
        }

        /// <summary>
        /// Attempts to find the abstractType extended by the supplied type.
        /// </summary>
        /// <param name="type">A Concrete Implementation Type</param>
        /// <param name="abstractType">An Open Generic Interface type</param>
        /// <returns>A Type matching the supplied open generic interface</returns>
        public static Type GetGenericAbstractTypeDefinition(this Type type, Type abstractType)
        {
            if (!abstractType.IsAbstract)
            {
                throw new InvalidOperationException($"Supplied {nameof(abstractType)} must be an Abstract Type");
            }

            if (!abstractType.IsGenericTypeDefinition)
            {
                throw new InvalidOperationException($"Supplied {nameof(abstractType)} must be an Open Generic");
            }

            Type baseType = type.BaseType;

            while (baseType != typeof(object) && baseType != null)
            {
                if (baseType.IsGenericType && abstractType.IsAssignableFrom(baseType.GetGenericTypeDefinition()))
                    return baseType;

                baseType = baseType.BaseType;
            }

            return null;
        }
    }
}
