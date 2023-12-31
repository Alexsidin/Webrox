using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Linq.Expressions;

namespace Webrox.EntityFrameworkCore.SqlServer.Query
{
    /// <summary>
    /// Extends the capabilities of <see cref="SqliteQueryableMethodTranslatingExpressionVisitor"/>.
    /// </summary>
    public class WebroxSqlServerQueryableMethodTranslatingExpressionVisitor : SqlServerQueryableMethodTranslatingExpressionVisitor
    {
        /// <inheritdoc />
        public WebroxSqlServerQueryableMethodTranslatingExpressionVisitor(
           QueryableMethodTranslatingExpressionVisitorDependencies dependencies,
           RelationalQueryableMethodTranslatingExpressionVisitorDependencies relationalDependencies,
           QueryCompilationContext queryCompilationContext,
           ISqlServerSingletonOptions sqlServerSingletonOptions)
           : base(dependencies, relationalDependencies, queryCompilationContext, sqlServerSingletonOptions)
        {
        }

        /// <inheritdoc />
        protected WebroxSqlServerQueryableMethodTranslatingExpressionVisitor(
           WebroxSqlServerQueryableMethodTranslatingExpressionVisitor parentVisitor)
           : base(parentVisitor)
        {
        }

        /// <inheritdoc />
        protected override QueryableMethodTranslatingExpressionVisitor CreateSubqueryVisitor()
        {
            return new WebroxSqlServerQueryableMethodTranslatingExpressionVisitor(this);
        }

        /// <inheritdoc />
        protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
        {
            return base.VisitMethodCall(methodCallExpression);
        }
    }
}