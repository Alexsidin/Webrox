using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Internal;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Webrox.EntityFrameworkCore.Core.Infrastructure;

namespace Webrox.EntityFrameworkCore.Postgres.Query
{
    //  RelationalParameterBasedSqlProcessor
    /// <summary>
    /// Extends <see cref="RelationalParameterBasedSqlProcessor"/>.
    /// </summary>
    [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.")]
    public class WebroxPostgreSqlParameterBasedSqlProcessor : NpgsqlParameterBasedSqlProcessor
    {
        /// <inheritdoc />
        public WebroxPostgreSqlParameterBasedSqlProcessor(
           RelationalParameterBasedSqlProcessorDependencies dependencies,
           bool useRelationalNulls)
           : base(dependencies, useRelationalNulls)
        {
        }

        /// <inheritdoc />
        protected override
#if NET7_0_OR_GREATER
            Expression
#else
            SelectExpression
#endif
            ProcessSqlNullability(
#if NET7_0_OR_GREATER
            Expression
#else
            SelectExpression
#endif
            expression, IReadOnlyDictionary<string, object?> parametersValues, out bool canCache)
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