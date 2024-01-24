using Microsoft.EntityFrameworkCore.Query;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace Webrox.EntityFrameworkCore.Postgres.Query
{
    /// <summary>
    /// Factory for creation of the <see cref="WebroxPostgresQueryableMethodTranslatingExpressionVisitor"/>.
    /// </summary>
    public sealed class WebroxPostgresQueryableMethodTranslatingExpressionVisitorFactory : IQueryableMethodTranslatingExpressionVisitorFactory
    {
        private readonly QueryableMethodTranslatingExpressionVisitorDependencies _dependencies;
        private readonly RelationalQueryableMethodTranslatingExpressionVisitorDependencies _relationalDependencies;
#if NET8_0_OR_GREATER
        private readonly INpgsqlSingletonOptions _options;
#endif
        /// <summary>
        /// Initializes new instance of <see cref="ThinktectureSqliteQueryableMethodTranslatingExpressionVisitorFactory"/>.
        /// </summary>
        /// <param name="dependencies">Dependencies.</param>
        /// <param name="relationalDependencies">Relational dependencies.</param>
        public WebroxPostgresQueryableMethodTranslatingExpressionVisitorFactory(
           QueryableMethodTranslatingExpressionVisitorDependencies dependencies,
           RelationalQueryableMethodTranslatingExpressionVisitorDependencies relationalDependencies
#if NET8_0_OR_GREATER
           , INpgsqlSingletonOptions options
#endif
            )
        {
            _dependencies = dependencies ?? throw new ArgumentNullException(nameof(dependencies));
            _relationalDependencies = relationalDependencies ?? throw new ArgumentNullException(nameof(relationalDependencies));
#if NET8_0_OR_GREATER
            _options = options;
#endif
        }

        /// <inheritdoc />
        public QueryableMethodTranslatingExpressionVisitor Create(QueryCompilationContext queryCompilationContext)
        {
            return new WebroxPostgresQueryableMethodTranslatingExpressionVisitor(_dependencies, _relationalDependencies, queryCompilationContext
#if NET8_0_OR_GREATER
            ,_options
#endif
                );
        }
    }
}
