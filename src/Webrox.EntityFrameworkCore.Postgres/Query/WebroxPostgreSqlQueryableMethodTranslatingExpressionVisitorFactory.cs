using Microsoft.EntityFrameworkCore.Query;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
namespace Webrox.EntityFrameworkCore.Postgres.Query
{
    /// <summary>
    /// Factory for creation of the <see cref="WebroxPostgreSqlQueryableMethodTranslatingExpressionVisitor"/>.
    /// </summary>
    public sealed class WebroxPostgreSqlQueryableMethodTranslatingExpressionVisitorFactory : IQueryableMethodTranslatingExpressionVisitorFactory
    {
        private readonly QueryableMethodTranslatingExpressionVisitorDependencies _dependencies;
        private readonly RelationalQueryableMethodTranslatingExpressionVisitorDependencies _relationalDependencies;

        /// <summary>
        /// Initializes new instance of <see cref="WebroxPostgreSqlQueryableMethodTranslatingExpressionVisitorFactory"/>.
        /// </summary>
        /// <param name="dependencies">Dependencies.</param>
        /// <param name="relationalDependencies">Relational dependencies.</param>
        public WebroxPostgreSqlQueryableMethodTranslatingExpressionVisitorFactory(
           QueryableMethodTranslatingExpressionVisitorDependencies dependencies,
           RelationalQueryableMethodTranslatingExpressionVisitorDependencies relationalDependencies)
        {
            _dependencies = dependencies ?? throw new ArgumentNullException(nameof(dependencies));
            _relationalDependencies = relationalDependencies ?? throw new ArgumentNullException(nameof(relationalDependencies));
        }

        /// <inheritdoc />
        public QueryableMethodTranslatingExpressionVisitor Create(QueryCompilationContext queryCompilationContext)
        {
            return new WebroxPostgreSqlQueryableMethodTranslatingExpressionVisitor(_dependencies, _relationalDependencies, queryCompilationContext);
        }
    }
}