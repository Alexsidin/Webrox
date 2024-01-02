using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Sqlite.Query.Internal;

namespace Webrox.EntityFrameworkCore.Sqlite.Query
{
    /// <summary>
    /// Factory for creation of the <see cref="WebroxSqliteRelationalSqlTranslatingExpressionVisitor"/>.
    /// </summary>
    public sealed class WebroxSqliteRelationalSqlTranslatingExpressionVisitorFactory :
        SqliteSqlTranslatingExpressionVisitorFactory
    {
        private readonly RelationalSqlTranslatingExpressionVisitorDependencies _dependencies;

        /// <summary>
        /// Initializes new instance of <see cref="WebroxSqliteQueryableMethodTranslatingExpressionVisitorFactory"/>.
        /// </summary>
        /// <param name="dependencies">Dependencies.</param>
        /// <param name="relationalDependencies">Relational dependencies.</param>
        public WebroxSqliteRelationalSqlTranslatingExpressionVisitorFactory(
           RelationalSqlTranslatingExpressionVisitorDependencies dependencies)
            : base(dependencies) 
        {
            _dependencies = dependencies ?? throw new ArgumentNullException(nameof(dependencies));
        }

        public RelationalSqlTranslatingExpressionVisitor Create(
            QueryCompilationContext queryCompilationContext,
            QueryableMethodTranslatingExpressionVisitor queryableMethodTranslatingExpressionVisitor            
            )
        {
            return new WebroxSqliteRelationalSqlTranslatingExpressionVisitor(Dependencies, queryCompilationContext,
            queryableMethodTranslatingExpressionVisitor);
        }
    }
}