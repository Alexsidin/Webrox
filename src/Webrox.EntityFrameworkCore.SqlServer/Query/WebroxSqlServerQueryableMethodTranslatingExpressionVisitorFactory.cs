using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;

namespace Webrox.EntityFrameworkCore.SqlServer.Query
{
    /// <summary>
    /// Factory for creation of the <see cref="WebroxSqlServerQueryableMethodTranslatingExpressionVisitor"/>.
    /// </summary>
    public sealed class WebroxSqlServerQueryableMethodTranslatingExpressionVisitorFactory : IQueryableMethodTranslatingExpressionVisitorFactory
    {
        private readonly QueryableMethodTranslatingExpressionVisitorDependencies _dependencies;
        private readonly RelationalQueryableMethodTranslatingExpressionVisitorDependencies _relationalDependencies;
        private readonly ISqlServerSingletonOptions _sqlServerSingletonOptions;

        /// <summary>
        /// Initializes new instance of <see cref="WebroxSqlServerQueryableMethodTranslatingExpressionVisitorFactory"/>.
        /// </summary>
        /// <param name="dependencies">Dependencies.</param>
        /// <param name="relationalDependencies">Relational dependencies.</param>
        public WebroxSqlServerQueryableMethodTranslatingExpressionVisitorFactory(
           QueryableMethodTranslatingExpressionVisitorDependencies dependencies,
           RelationalQueryableMethodTranslatingExpressionVisitorDependencies relationalDependencies,
           ISqlServerSingletonOptions sqlServerSingletonOptions)
        {
            _dependencies = dependencies ?? throw new ArgumentNullException(nameof(dependencies));
            _relationalDependencies = relationalDependencies ?? throw new ArgumentNullException(nameof(relationalDependencies));
            _sqlServerSingletonOptions = sqlServerSingletonOptions;
        }

        /// <inheritdoc />
        public QueryableMethodTranslatingExpressionVisitor Create(QueryCompilationContext queryCompilationContext)
        {
            return new WebroxSqlServerQueryableMethodTranslatingExpressionVisitor(_dependencies, _relationalDependencies, queryCompilationContext, _sqlServerSingletonOptions);
        }
    }
}