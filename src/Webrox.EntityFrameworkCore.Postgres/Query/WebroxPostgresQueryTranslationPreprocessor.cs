#if NET8_0_OR_GREATER

using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Internal;
using System.Linq.Expressions;
using Webrox.EntityFrameworkCore.Core;
using Webrox.EntityFrameworkCore.Sqlite.Query;

namespace Webrox.EntityFrameworkCore.Postgres.Query
{
    public class WebroxPostgresQueryTranslationPreprocessor : 
        NpgsqlQueryTranslationPreprocessor
    {
        private readonly ISqlExpressionFactory _sqlExpressionFactory;
        /// <summary>
        ///     Creates a new instance of the <see cref="QueryTranslationPreprocessor" /> class.
        /// </summary>
        /// <param name="dependencies">Parameter object containing dependencies for this class.</param>
        /// <param name="queryCompilationContext">The query compilation context object to use.</param>
        public WebroxPostgresQueryTranslationPreprocessor(
            QueryTranslationPreprocessorDependencies dependencies,
            QueryCompilationContext queryCompilationContext,
            RelationalQueryTranslationPreprocessorDependencies relationalDependencies,
            INpgsqlSingletonOptions npgsqlSingletonOptions,
            ISqlExpressionFactory sqlExpressionFactory
            )
            : base(dependencies, relationalDependencies, npgsqlSingletonOptions, queryCompilationContext)
        {
            _sqlExpressionFactory = sqlExpressionFactory;
        }

        /// <summary>
        ///     Applies preprocessing transformations to the query.
        /// </summary>
        /// <param name="query">The query to process.</param>
        /// <returns>A query expression after transformations.</returns>
        public override Expression Process(Expression query)
        {
            query = new InvocationExpressionRemovingExpressionVisitor().Visit(query);
            query = NormalizeQueryableMethod(query);
#if NET8_0_OR_GREATER
            query = new CallForwardingExpressionVisitor().Visit(query);
#endif
            query = new NullCheckRemovingExpressionVisitor().Visit(query);
            query = new SubqueryMemberPushdownExpressionVisitor(QueryCompilationContext.Model).Visit(query);
            query = new WebroxNavigationExpandingExpressionVisitor(
                    this,
                    QueryCompilationContext,
                    Dependencies.EvaluatableExpressionFilter,
                    Dependencies.NavigationExpansionExtensibilityHelper,
                    _sqlExpressionFactory)
                .Expand(query);
            query = new QueryOptimizingExpressionVisitor().Visit(query);
            query = new NullCheckRemovingExpressionVisitor().Visit(query);

            return query;
        }
    }
}
#endif