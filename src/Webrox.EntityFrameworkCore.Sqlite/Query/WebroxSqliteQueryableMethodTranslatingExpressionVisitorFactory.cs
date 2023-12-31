using Microsoft.EntityFrameworkCore.Query;

namespace Webrox.EntityFrameworkCore.Sqlite.Query
{
    /// <summary>
    /// Factory for creation of the <see cref="WebroxSqliteQueryableMethodTranslatingExpressionVisitor"/>.
    /// </summary>
    public sealed class WebroxSqliteQueryableMethodTranslatingExpressionVisitorFactory : IQueryableMethodTranslatingExpressionVisitorFactory
    {
        private readonly QueryableMethodTranslatingExpressionVisitorDependencies _dependencies;
        private readonly RelationalQueryableMethodTranslatingExpressionVisitorDependencies _relationalDependencies;

        /// <summary>
        /// Initializes new instance of <see cref="WebroxSqliteQueryableMethodTranslatingExpressionVisitorFactory"/>.
        /// </summary>
        /// <param name="dependencies">Dependencies.</param>
        /// <param name="relationalDependencies">Relational dependencies.</param>
        public WebroxSqliteQueryableMethodTranslatingExpressionVisitorFactory(
           QueryableMethodTranslatingExpressionVisitorDependencies dependencies,
           RelationalQueryableMethodTranslatingExpressionVisitorDependencies relationalDependencies)
        {
            _dependencies = dependencies ?? throw new ArgumentNullException(nameof(dependencies));
            _relationalDependencies = relationalDependencies ?? throw new ArgumentNullException(nameof(relationalDependencies));
        }

        /// <inheritdoc />
        public QueryableMethodTranslatingExpressionVisitor Create(QueryCompilationContext queryCompilationContext)
        {
            return new WebroxSqliteQueryableMethodTranslatingExpressionVisitor(_dependencies, _relationalDependencies, queryCompilationContext);
        }
    }
}