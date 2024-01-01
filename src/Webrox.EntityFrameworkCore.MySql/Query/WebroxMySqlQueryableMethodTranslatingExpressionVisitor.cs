using Microsoft.EntityFrameworkCore.Query;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Internal;
using System.Linq.Expressions;

namespace Webrox.EntityFrameworkCore.MySql.Query
{
    /// <summary>
    /// Extends the capabilities of <see cref="SqliteQueryableMethodTranslatingExpressionVisitor"/>.
    /// </summary>
    public class WebroxMySqlQueryableMethodTranslatingExpressionVisitor : NpgsqlQueryableMethodTranslatingExpressionVisitor
    {
        /// <inheritdoc />
        public WebroxMySqlQueryableMethodTranslatingExpressionVisitor(
           QueryableMethodTranslatingExpressionVisitorDependencies dependencies,
           RelationalQueryableMethodTranslatingExpressionVisitorDependencies relationalDependencies,
           QueryCompilationContext queryCompilationContext)
           : base(dependencies, relationalDependencies, queryCompilationContext)
        {
        }

        /// <inheritdoc />
        protected WebroxMySqlQueryableMethodTranslatingExpressionVisitor(
           WebroxMySqlQueryableMethodTranslatingExpressionVisitor parentVisitor)
           : base(parentVisitor)
        {
        }

        /// <inheritdoc />
        protected override QueryableMethodTranslatingExpressionVisitor CreateSubqueryVisitor()
        {
            return new WebroxMySqlQueryableMethodTranslatingExpressionVisitor(this);
        }

        /// <inheritdoc />
        protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
        {
            return base.VisitMethodCall(methodCallExpression);
        }
    }
}