using Microsoft.EntityFrameworkCore.Query;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
namespace Webrox.EntityFrameworkCore.MySql.Query
{
    /// <summary>
    /// Factory for creation of the <see cref="WebroxMySqlQueryableMethodTranslatingExpressionVisitor"/>.
    /// </summary>
    public sealed class WebroxMySqlQueryableMethodTranslatingExpressionVisitorFactory : IQueryableMethodTranslatingExpressionVisitorFactory
    {
        private readonly QueryableMethodTranslatingExpressionVisitorDependencies _dependencies;
        private readonly RelationalQueryableMethodTranslatingExpressionVisitorDependencies _relationalDependencies;

        /// <summary>
        /// Initializes new instance of <see cref="WebroxMySqlQueryableMethodTranslatingExpressionVisitorFactory"/>.
        /// </summary>
        /// <param name="dependencies">Dependencies.</param>
        /// <param name="relationalDependencies">Relational dependencies.</param>
        public WebroxMySqlQueryableMethodTranslatingExpressionVisitorFactory(
           QueryableMethodTranslatingExpressionVisitorDependencies dependencies,
           RelationalQueryableMethodTranslatingExpressionVisitorDependencies relationalDependencies)
        {
            _dependencies = dependencies ?? throw new ArgumentNullException(nameof(dependencies));
            _relationalDependencies = relationalDependencies ?? throw new ArgumentNullException(nameof(relationalDependencies));
        }

        /// <inheritdoc />
        public QueryableMethodTranslatingExpressionVisitor Create(QueryCompilationContext queryCompilationContext)
        {
            return new WebroxMySqlQueryableMethodTranslatingExpressionVisitor(_dependencies, _relationalDependencies, queryCompilationContext);
        }
    }
}