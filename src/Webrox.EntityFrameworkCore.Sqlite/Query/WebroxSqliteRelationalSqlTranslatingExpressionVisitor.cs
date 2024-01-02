using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Sqlite.Query.Internal;
using System.Linq.Expressions;

namespace Webrox.EntityFrameworkCore.Sqlite.Query
{
    /// <summary>
    /// Factory for creation of the <see cref="WebroxSqliteQueryableMethodTranslatingExpressionVisitor"/>.
    /// </summary>
    public sealed class WebroxSqliteRelationalSqlTranslatingExpressionVisitor :
        SqliteSqlTranslatingExpressionVisitor
    {

        public WebroxSqliteRelationalSqlTranslatingExpressionVisitor(
             RelationalSqlTranslatingExpressionVisitorDependencies dependencies,
            QueryCompilationContext queryCompilationContext,
            QueryableMethodTranslatingExpressionVisitor queryableMethodTranslatingExpressionVisitor)
            : base(dependencies, queryCompilationContext, queryableMethodTranslatingExpressionVisitor)
        {
        }

        protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
        {
            return base.VisitMethodCall(methodCallExpression);
        }
    }
}