using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Webrox.EntityFrameworkCore.Core
{
    /// <summary>
    /// Extensions for <see cref="IQueryable{T}"/>.
    /// </summary>
    public static class RelationalQueryableExtensions
    {
        internal static readonly MethodInfo _asSubQuery = typeof(RelationalQueryableExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
                                                                                              .Single(m => m.Name == nameof(AsSubQuery) && m.IsGenericMethod);


        /// <summary>
        /// Executes provided query as a sub query.
        /// </summary>
        /// <param name="source">Query to execute as as sub query.</param>
        /// <typeparam name="TEntity">Type of the entity.</typeparam>
        /// <returns>Query that will be executed as a sub query.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
        public static IQueryable<TEntity> AsSubQuery<TEntity>(this IQueryable<TEntity> source)
        {
            ArgumentNullException.ThrowIfNull(source);

            return source.Provider.CreateQuery<TEntity>(Expression.Call(null, _asSubQuery.MakeGenericMethod(typeof(TEntity)), source.Expression));
        }
    }
}
