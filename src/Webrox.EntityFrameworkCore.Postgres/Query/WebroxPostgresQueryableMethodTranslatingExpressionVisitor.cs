using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Internal;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Webrox.EntityFrameworkCore.Core;

namespace Webrox.EntityFrameworkCore.Postgres.Query
{
    /// <summary>
    /// Extends the capabilities of <see cref="Microsoft.EntityFrameworkCore.Sqlite.Query.Internal.SqliteQueryableMethodTranslatingExpressionVisitor"/>.
    /// </summary>
    [SuppressMessage("Usage", "EF1001", MessageId = "Internal EF Core API usage.")]
    public class WebroxPostgresQueryableMethodTranslatingExpressionVisitor :
#if NET6_0
        RelationalQueryableMethodTranslatingExpressionVisitor
#else
        NpgsqlQueryableMethodTranslatingExpressionVisitor
#endif
    {
        /// <inheritdoc />
        public WebroxPostgresQueryableMethodTranslatingExpressionVisitor(
           QueryableMethodTranslatingExpressionVisitorDependencies dependencies,
           RelationalQueryableMethodTranslatingExpressionVisitorDependencies relationalDependencies,
           QueryCompilationContext queryCompilationContext
#if NET8_0_OR_GREATER
           , INpgsqlSingletonOptions options
#endif
            )
           : base(dependencies, relationalDependencies, queryCompilationContext
                 //#if NET8_0_OR_GREATER                 
                 //                 , options
                 //#endif
                 )
        {
        }
#if NET7_0
#else
        /// <inheritdoc />
        protected WebroxPostgresQueryableMethodTranslatingExpressionVisitor(
           WebroxPostgresQueryableMethodTranslatingExpressionVisitor parentVisitor)
           : base(parentVisitor)
        {
        }
#endif
        //#endif
        /// <inheritdoc />
        protected override QueryableMethodTranslatingExpressionVisitor CreateSubqueryVisitor()
        {
#if NET7_0
            return new WebroxPostgresQueryableMethodTranslatingExpressionVisitor(Dependencies, RelationalDependencies, QueryCompilationContext);
#else
            return new WebroxPostgresQueryableMethodTranslatingExpressionVisitor(this);
#endif
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
