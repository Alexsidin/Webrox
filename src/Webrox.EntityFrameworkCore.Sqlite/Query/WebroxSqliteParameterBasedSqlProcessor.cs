
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Sqlite.Query.Internal;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Webrox.EntityFrameworkCore.Sqlite.Query
{
    //  RelationalParameterBasedSqlProcessor
    /// <summary>
    /// Extends <see cref="RelationalParameterBasedSqlProcessor"/>.
    /// </summary>
    [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.")]
    public class WebroxSqliteParameterBasedSqlProcessor :
#if NET8_0_OR_GREATER
        SqliteParameterBasedSqlProcessor
#else
        RelationalParameterBasedSqlProcessor
#endif
    {
        /// <inheritdoc />
        public WebroxSqliteParameterBasedSqlProcessor(
           RelationalParameterBasedSqlProcessorDependencies dependencies,
           bool useRelationalNulls)
           : base(dependencies, useRelationalNulls)
        {
            
        }

        /// <inheritdoc />
        protected override Expression ProcessSqlNullability(Expression expression, IReadOnlyDictionary<string, object?> parametersValues, out bool canCache)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            if (parametersValues == null)
            {
                throw new ArgumentNullException(nameof(parametersValues));
            }

            return new WebroxSqlNullabilityProcessor(Dependencies, UseRelationalNulls).Process(expression, parametersValues, out canCache);
        }
    }
}
