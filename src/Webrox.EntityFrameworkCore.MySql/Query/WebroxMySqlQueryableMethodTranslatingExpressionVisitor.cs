using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using MySql.EntityFrameworkCore.Infrastructure.Internal;

namespace Webrox.EntityFrameworkCore.MySql.Query
{
    /// <summary>
    /// Extends the capabilities of <see cref="SqliteQueryableMethodTranslatingExpressionVisitor"/>.
    /// </summary>
    public class WebroxMySqlQueryableMethodTranslatingExpressionVisitor :
        RelationalSqlTranslatingExpressionVisitor
    //MySql.EntityFrameworkCore.Query.Internal.MySQLSqlTranslatingExpressionVisitor
    {
        /// <inheritdoc />
        public WebroxMySqlQueryableMethodTranslatingExpressionVisitor(RelationalSqlTranslatingExpressionVisitorDependencies dependencies, QueryCompilationContext model, QueryableMethodTranslatingExpressionVisitor queryableMethodTranslatingExpressionVisitor) 
            : base(dependencies, model, queryableMethodTranslatingExpressionVisitor)
        {
        }

        /// <inheritdoc />
        //protected WebroxMySqlQueryableMethodTranslatingExpressionVisitor(
        //   WebroxMySqlQueryableMethodTranslatingExpressionVisitor parentVisitor)
        //   : base(parentVisitor)
        //{
        //}

        /// <inheritdoc />
        //protected override QueryableMethodTranslatingExpressionVisitor CreateSubqueryVisitor()
        //{
        //    return new WebroxMySqlQueryableMethodTranslatingExpressionVisitor(this);
        //}

        /// <inheritdoc />
        protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
        {
            return base.VisitMethodCall(methodCallExpression);
        }
    }
}