using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Sqlite.Query.Internal;
using System.Linq.Expressions;

namespace Webrox.EntityFrameworkCore.Sqlite.Query
{
    /// <summary>
    /// Extends the capabilities of <see cref="SqliteQueryableMethodTranslatingExpressionVisitor"/>.
    /// </summary>
    public class WebroxSqliteQueryableMethodTranslatingExpressionVisitor : SqliteQueryableMethodTranslatingExpressionVisitor
    {
        /// <inheritdoc />
        public WebroxSqliteQueryableMethodTranslatingExpressionVisitor(
           QueryableMethodTranslatingExpressionVisitorDependencies dependencies,
        RelationalQueryableMethodTranslatingExpressionVisitorDependencies relationalDependencies,
        QueryCompilationContext queryCompilationContext)
           : base(dependencies, relationalDependencies, queryCompilationContext)
        {
        }

        /// <inheritdoc />
        protected WebroxSqliteQueryableMethodTranslatingExpressionVisitor(
           WebroxSqliteQueryableMethodTranslatingExpressionVisitor parentVisitor)
           : base(parentVisitor)
        {
        }

        /// <inheritdoc />
        protected override QueryableMethodTranslatingExpressionVisitor CreateSubqueryVisitor()
        {
            return new WebroxSqliteQueryableMethodTranslatingExpressionVisitor(this);
        }

        /// <inheritdoc />
        protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
        {
            return base.VisitMethodCall(methodCallExpression);
        }
    }
}