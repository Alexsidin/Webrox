using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using System.Reflection;
using Webrox.EntityFrameworkCore.Core;
using Webrox.EntityFrameworkCore.Sqlite.Query;
using MySqlLib = MySql.EntityFrameworkCore;

namespace MySql.EntityFrameworkCore.Design.Tests
{
    public class WebroxMySqlSqlTranslatingExpressionVisitorProxy
    {
        public RelationalSqlTranslatingExpressionVisitor Create(
            RelationalSqlTranslatingExpressionVisitorDependencies dependencies, 
            QueryCompilationContext model, 
            QueryableMethodTranslatingExpressionVisitor queryableMethodTranslatingExpressionVisitor, 
            ISqlExpressionFactory sqlExpressionFactory)
        {
            return new WebroxMySqlSqlTranslatingExpressionVisitor(dependencies,
             model, queryableMethodTranslatingExpressionVisitor, sqlExpressionFactory);
        }
    }

    internal class WebroxMySqlSqlTranslatingExpressionVisitor :
        MySql.EntityFrameworkCore.Query.Internal.MySQLSqlTranslatingExpressionVisitor
    {
        private readonly ISqlExpressionFactory _sqlExpressionFactory;

        static Dictionary<string, List<MethodInfo>> queryableMethodGroups = typeof(Queryable)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .GroupBy(mi => mi.Name)
            .ToDictionary(e => e.Key, l => l.ToList());

        static MethodInfo GetMethod(string name, int genericParameterCount, Func<Type[], Type[]> parameterGenerator)
            => queryableMethodGroups[name].Single(
                mi => ((genericParameterCount == 0 && !mi.IsGenericMethod)
                        || (mi.IsGenericMethod && mi.GetGenericArguments().Length == genericParameterCount))
                    && mi.GetParameters().Select(e => e.ParameterType).SequenceEqual(
                        parameterGenerator(mi.IsGenericMethod ? mi.GetGenericArguments() : Array.Empty<Type>())));

        static MethodInfo _methodSelect;

        static WebroxMySqlSqlTranslatingExpressionVisitor()
        {
            _methodSelect = GetMethod(
             nameof(Queryable.Select), 2,
             types => new[]
             {
                typeof(IQueryable<>).MakeGenericType(types[0]),
                typeof(Expression<>).MakeGenericType(typeof(Func<,,>).MakeGenericType(types[0], typeof(int), types[1]))
             });
        }


        //private readonly ISqlExpressionFactory _sqlExpressionFactory;
        ///// <summary>
        /////     Creates a new instance of the <see cref="QueryTranslationPreprocessor" /> class.
        ///// </summary>
        ///// <param name="dependencies">Parameter object containing dependencies for this class.</param>
        ///// <param name="queryCompilationContext">The query compilation context object to use.</param>
        //public WebroxMySqlQueryTranslationPreprocessor(
        //    QueryTranslationPreprocessorDependencies dependencies,
        //    QueryCompilationContext queryCompilationContext,
        //    RelationalQueryTranslationPreprocessorDependencies relationalDependencies,
        //    ISqlExpressionFactory sqlExpressionFactory
        //    )
        //    : base(dependencies, queryCompilationContext)
        //{
        //    _sqlExpressionFactory = sqlExpressionFactory;
        //}

        ///// <summary>
        /////     Applies preprocessing transformations to the query.
        ///// </summary>
        ///// <param name="query">The query to process.</param>
        ///// <returns>A query expression after transformations.</returns>
        //public override Expression Process(Expression query)
        //{
        //    query = new InvocationExpressionRemovingExpressionVisitor().Visit(query);
        //    query = NormalizeQueryableMethod(query);
        //    query = new CallForwardingExpressionVisitor().Visit(query);
        //    query = new NullCheckRemovingExpressionVisitor().Visit(query);
        //    query = new SubqueryMemberPushdownExpressionVisitor(QueryCompilationContext.Model).Visit(query);
        //    query = new WebroxNavigationExpandingExpressionVisitor(
        //            this,
        //            QueryCompilationContext,
        //            Dependencies.EvaluatableExpressionFilter,
        //            Dependencies.NavigationExpansionExtensibilityHelper,
        //            _sqlExpressionFactory)
        //        .Expand(query);
        //    query = new QueryOptimizingExpressionVisitor().Visit(query);
        //    query = new NullCheckRemovingExpressionVisitor().Visit(query);

        //    return query;
        //}
        public WebroxMySqlSqlTranslatingExpressionVisitor(RelationalSqlTranslatingExpressionVisitorDependencies dependencies, QueryCompilationContext model, QueryableMethodTranslatingExpressionVisitor queryableMethodTranslatingExpressionVisitor, ISqlExpressionFactory sqlExpressionFactory)
            : base(dependencies, model, queryableMethodTranslatingExpressionVisitor)
        {
            _sqlExpressionFactory = sqlExpressionFactory;
        }

        //protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
        //{
        //    return base.VisitMethodCall(methodCallExpression);
        //}

        protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
        {
            var method = methodCallExpression.Method;

            if (method.DeclaringType == typeof(Queryable)
                && method.Name == nameof(Queryable.Select)
                && method.IsGenericMethod)
            {
                var typesGenerics = method.GetGenericArguments();

                if ((typesGenerics.Length == 2) && (method == _methodSelect?.MakeGenericMethod(typesGenerics)))
                {
                    var firstArgument = Visit(methodCallExpression.Arguments[0]);
                    var lambda = methodCallExpression.Arguments[1].UnwrapLambdaFromQuote();

                    return ProcessSelect(firstArgument, lambda);
                }
            }

            return base.VisitMethodCall(methodCallExpression);
        }

        private Expression ProcessSelect(Expression source, LambdaExpression selector)
        {
            var piPending = source.GetType().GetProperty("PendingSelector");
            var pendingSelector = piPending.GetValue(source) as Expression;

            var selectorBody = ReplacingExpressionVisitor.Replace(
                selector.Parameters[0],
                pendingSelector,
                selector.Body);

            //replace index by rownumber expression order by NULL
            selectorBody = ReplacingExpressionVisitor.Replace(
                selector.Parameters[1],
                Expression.Convert(new RowNumberExpression(null,
                new List<OrderingExpression>(new[] { new OrderingExpression(_sqlExpressionFactory.Fragment("NULL"), true) }), RelationalTypeMapping.NullMapping), typeof(int)),
                selectorBody);

            var miApplySelector = source.GetType().GetMethod("ApplySelector");
            miApplySelector?.Invoke(source, new object[] { selectorBody });

            return source;
        }
    }
}