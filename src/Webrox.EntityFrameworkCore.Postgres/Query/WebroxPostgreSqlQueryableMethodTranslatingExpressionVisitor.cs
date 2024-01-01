using Microsoft.EntityFrameworkCore.Query;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Internal;
using System.Linq.Expressions;

namespace Webrox.EntityFrameworkCore.Postgres.Query
{
    /// <summary>
    /// Extends the capabilities of <see cref="SqliteQueryableMethodTranslatingExpressionVisitor"/>.
    /// </summary>
    public class WebroxPostgreSqlQueryableMethodTranslatingExpressionVisitor : NpgsqlQueryableMethodTranslatingExpressionVisitor
    {
        /// <inheritdoc />
        public WebroxPostgreSqlQueryableMethodTranslatingExpressionVisitor(
           QueryableMethodTranslatingExpressionVisitorDependencies dependencies,
           RelationalQueryableMethodTranslatingExpressionVisitorDependencies relationalDependencies,
           QueryCompilationContext queryCompilationContext)
           : base(dependencies, relationalDependencies, queryCompilationContext)
        {
        }

        /// <inheritdoc />
        protected WebroxPostgreSqlQueryableMethodTranslatingExpressionVisitor(
           WebroxPostgreSqlQueryableMethodTranslatingExpressionVisitor parentVisitor)
           : base(parentVisitor)
        {
        }

        /// <inheritdoc />
        protected override QueryableMethodTranslatingExpressionVisitor CreateSubqueryVisitor()
        {
            return new WebroxPostgreSqlQueryableMethodTranslatingExpressionVisitor(this);
        }

        /// <inheritdoc />
        protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
        {
            return base.VisitMethodCall(methodCallExpression);
        }
    }
}