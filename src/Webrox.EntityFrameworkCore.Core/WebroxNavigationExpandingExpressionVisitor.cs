using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Webrox.EntityFrameworkCore.Core
{
    public class WebroxNavigationExpandingExpressionVisitor : NavigationExpandingExpressionVisitor
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

        static WebroxNavigationExpandingExpressionVisitor()
        {
            _methodSelect = GetMethod(
             nameof(Queryable.Select), 2,
             types => new[]
             {
                typeof(IQueryable<>).MakeGenericType(types[0]),
                typeof(Expression<>).MakeGenericType(typeof(Func<,,>).MakeGenericType(types[0], typeof(int), types[1]))
             });
        }

        public WebroxNavigationExpandingExpressionVisitor(QueryTranslationPreprocessor queryTranslationPreprocessor,
            QueryCompilationContext queryCompilationContext,
            IEvaluatableExpressionFilter evaluatableExpressionFilter,
            INavigationExpansionExtensibilityHelper extensibilityHelper,
            ISqlExpressionFactory sqlExpressionFactory)
            : base(queryTranslationPreprocessor, queryCompilationContext, evaluatableExpressionFilter, extensibilityHelper)
        {
           
            _sqlExpressionFactory = sqlExpressionFactory;
        }

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

    public static class ExpressionExtensions
    {
        public static LambdaExpression UnwrapLambdaFromQuote(this Expression expression)
       => (LambdaExpression)(expression is UnaryExpression unary && expression.NodeType == ExpressionType.Quote
           ? unary.Operand
           : expression);
    }
}
