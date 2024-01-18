using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Webrox.EntityFrameworkCore.Core;
using Webrox.EntityFrameworkCore.Core.Infrastructure;
using Webrox.EntityFrameworkCore.Core.SqlExpressions;

namespace Webrox.EntityFrameworkCore.SqlServer.Query
{
    /// <summary>
    /// Extends the capabilities of <see cref="Microsoft.EntityFrameworkCore.Sqlite.Query.Internal.SqliteQueryableMethodTranslatingExpressionVisitor"/>.
    /// </summary>
    [SuppressMessage("Usage", "EF1001", MessageId = "Internal EF Core API usage.")]
    public class WebroxSqlServerQueryableMethodTranslatingExpressionVisitor :
        SqlServerQueryableMethodTranslatingExpressionVisitor
    {
        /// <inheritdoc />
        public WebroxSqlServerQueryableMethodTranslatingExpressionVisitor(
           QueryableMethodTranslatingExpressionVisitorDependencies dependencies,
           RelationalQueryableMethodTranslatingExpressionVisitorDependencies relationalDependencies,
           QueryCompilationContext queryCompilationContext
#if NET8_0_OR_GREATER
           , ISqlServerSingletonOptions options
#endif
            )
           : base(dependencies, relationalDependencies, queryCompilationContext
#if NET8_0_OR_GREATER                 
                 , options
#endif
                 )
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
            return this.TranslateRelationalMethods(methodCallExpression) ??
                   base.VisitMethodCall(methodCallExpression);
        }

        Expression _parentExpression;

        [return: NotNullIfNotNull("node")]
        public override Expression? Visit(Expression? node)
        {
            _parentExpression = node;
            return base.Visit(node);
        }

        /// <summary>
        /// Translates custom methods like <see cref="RelationalQueryableExtensions.AsSubQuery{TEntity}"/>.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        /// <param name="methodCallExpression">Method call to translate.</param>
        /// <returns>Translated method call if a custom method is found; otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="visitor"/> or <paramref name="methodCallExpression"/> is <c>null</c>.
        /// </exception>
        public Expression? TranslateRelationalMethods(
           MethodCallExpression methodCallExpression)
        {
            ArgumentNullException.ThrowIfNull(methodCallExpression);

            if (methodCallExpression.Method.DeclaringType == typeof(RelationalQueryableExtensions))
            {
                if (methodCallExpression.Method.Name == nameof(RelationalQueryableExtensions.AsSubQuery))
                {
                    var expression = this.Visit(methodCallExpression.Arguments[0]);
                    
                    if (expression is ShapedQueryExpression shapedQueryExpression)
                    {
                        ((SelectExpression)shapedQueryExpression.QueryExpression).PushdownIntoSubquery();
                        return shapedQueryExpression;
                    }
                }
            }

            //if (methodCallExpression.Method.DeclaringType == typeof(Queryable))
            //{
            //    if (methodCallExpression.Method.Name == nameof(Queryable.Select))
            //    {
            //        var expression = this.Visit(methodCallExpression.Arguments[0]);

            //        if (expression is ShapedQueryExpression shapedQueryExpression)
            //        {
            //            ((SelectExpression)shapedQueryExpression.QueryExpression).PushdownIntoSubquery();
            //            return shapedQueryExpression;
            //        }
            //    }
            //}

            return null;
        }
    }

}
