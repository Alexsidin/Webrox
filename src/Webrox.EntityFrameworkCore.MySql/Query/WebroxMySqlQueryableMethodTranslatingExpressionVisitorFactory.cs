using Microsoft.EntityFrameworkCore.Query;

namespace Webrox.EntityFrameworkCore.MySql.Query
{
    /// <summary>
    /// Factory for creation of the <see cref="WebroxMySqlQueryableMethodTranslatingExpressionVisitor"/>.
    /// </summary>
    public sealed class WebroxMySqlQueryableMethodTranslatingExpressionVisitorFactory : IQueryableMethodTranslatingExpressionVisitorFactory
    {
        private readonly QueryableMethodTranslatingExpressionVisitorDependencies _dependencies;
        private readonly RelationalQueryableMethodTranslatingExpressionVisitorDependencies _relationalDependencies;
//#if NET8_0_OR_GREATER
//        private readonly IMySQLSingletonOptions _options;
//#endif
        /// <summary>
        /// Initializes new instance of <see cref="ThinktectureSqliteQueryableMethodTranslatingExpressionVisitorFactory"/>.
        /// </summary>
        /// <param name="dependencies">Dependencies.</param>
        /// <param name="relationalDependencies">Relational dependencies.</param>
        public WebroxMySqlQueryableMethodTranslatingExpressionVisitorFactory(
           QueryableMethodTranslatingExpressionVisitorDependencies dependencies,
           RelationalQueryableMethodTranslatingExpressionVisitorDependencies relationalDependencies
//#if NET8_0_OR_GREATER
//           , INpgsqlSingletonOptions options
//#endif
            )
        {
            _dependencies = dependencies ?? throw new ArgumentNullException(nameof(dependencies));
            _relationalDependencies = relationalDependencies ?? throw new ArgumentNullException(nameof(relationalDependencies));
//#if NET8_0_OR_GREATER
//            _options = options;
//#endif
        }

        /// <inheritdoc />
        public QueryableMethodTranslatingExpressionVisitor Create(QueryCompilationContext queryCompilationContext)
        {
            return new WebroxMySqlQueryableMethodTranslatingExpressionVisitor(_dependencies, _relationalDependencies, queryCompilationContext
//#if NET8_0_OR_GREATER
//            ,_options
//#endif
                );
        }
    }
}
