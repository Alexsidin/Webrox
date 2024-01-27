using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualBasic;
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

namespace Webrox.EntityFrameworkCore.Core.Infrastructure
{
    public class WebroxNavigationExpandingExpressionVisitor : NavigationExpandingExpressionVisitor
    {
        private readonly ISqlExpressionFactory _sqlExpressionFactory;
        private readonly QueryCompilationContext _queryCompilationContext;

        static Dictionary<string, List<MethodInfo>> queryableMethodGroups = typeof(Queryable)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .GroupBy(mi => mi.Name)
            .ToDictionary(e => e.Key, l => l.ToList());

        static MethodInfo GetMethod(string name, int genericParameterCount, Func<Type[], Type[]> parameterGenerator)
            => queryableMethodGroups[name].Single(
                mi => (genericParameterCount == 0 && !mi.IsGenericMethod
                        || mi.IsGenericMethod && mi.GetGenericArguments().Length == genericParameterCount)
                    && mi.GetParameters().Select(e => e.ParameterType).SequenceEqual(
                        parameterGenerator(mi.IsGenericMethod ? mi.GetGenericArguments() : Array.Empty<Type>())));

        static MethodInfo _methodSelectWithIndex;
        static MethodInfo _methodSelect;
        static MethodInfo _methodStringEqualsComparison;
        static MethodInfo _methodStringEquals; 
        static MethodInfo _methodContainsEqualsComparison;
        static MethodInfo _methodContainsEquals;
        static MethodInfo _methodEFCollate;

        static WebroxNavigationExpandingExpressionVisitor()
        {
            _methodSelect = GetMethod(
             nameof(Queryable.Select), 2,
             types => new[]
             {
                typeof(IQueryable<>).MakeGenericType(types[0]),
                 typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[1]))
             });

            _methodSelectWithIndex = GetMethod(
             nameof(Queryable.Select), 2,
             types => new[]
             {
                typeof(IQueryable<>).MakeGenericType(types[0]),
                typeof(Expression<>).MakeGenericType(typeof(Func<,,>).MakeGenericType(types[0], typeof(int), types[1]))
             });

            _methodStringEqualsComparison = typeof(string).GetMethod(nameof(string.Equals), BindingFlags.Instance | BindingFlags.Public,
                new Type[] { typeof(string), typeof(StringComparison) });
            _methodStringEquals = typeof(string).GetMethod(nameof(string.Equals), BindingFlags.Instance | BindingFlags.Public,
                new Type[] { typeof(string) });

            _methodContainsEqualsComparison = typeof(string).GetMethod(nameof(string.Contains), BindingFlags.Instance | BindingFlags.Public,
                new Type[] { typeof(string), typeof(StringComparison) });
            _methodContainsEquals = typeof(string).GetMethod(nameof(string.Contains), BindingFlags.Instance | BindingFlags.Public,
                new Type[] { typeof(string) });

            var allmethodsEFColate = typeof(RelationalDbFunctionsExtensions).GetMethods().Where(a => a.Name == nameof(RelationalDbFunctionsExtensions.Collate));
            _methodEFCollate = allmethodsEFColate.Where(a => a.IsGenericMethod).FirstOrDefault();

        }

        public WebroxNavigationExpandingExpressionVisitor(QueryTranslationPreprocessor queryTranslationPreprocessor,
            QueryCompilationContext queryCompilationContext,
            IEvaluatableExpressionFilter evaluatableExpressionFilter,
#if NET6_0_OR_GREATER
            INavigationExpansionExtensibilityHelper extensibilityHelper,
#endif
            ISqlExpressionFactory sqlExpressionFactory)
            : base(queryTranslationPreprocessor, queryCompilationContext, evaluatableExpressionFilter
#if NET6_0_OR_GREATER
                  , extensibilityHelper
#endif
                  )
        {

            _queryCompilationContext = queryCompilationContext;
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

                if (method == _methodSelectWithIndex?.MakeGenericMethod(typesGenerics))
                {
                    var firstArgument = Visit(methodCallExpression.Arguments[0]);
                    var lambda = methodCallExpression.Arguments[1].UnwrapLambdaFromQuote();

                    return ProcessSelect(firstArgument, lambda);
                }
            }

            if (method == _methodStringEqualsComparison)
            {
                return ProcessStringEquals(methodCallExpression);
            }

            if (method == _methodContainsEqualsComparison)
            {
                return ProcessStringContains(methodCallExpression);
            }

            return base.VisitMethodCall(methodCallExpression);
        }

        private Expression ProcessSubQuery(Expression source)//, ConstantExpression stringComparisonExpression)
        {
            var piPending = source.GetType().GetProperty("PendingSelector");
            var pendingSelector = piPending.GetValue(source) as Expression;

            return null;
        }

        private Expression ProcessStringContains(MethodCallExpression methodCallExpression)
        {
            var expression = methodCallExpression.Arguments[0];
            var stringOrdinal = methodCallExpression.Arguments[1] as ConstantExpression;
            var stringComparison = (StringComparison)stringOrdinal.Value;

            var collation = GetCollationName(stringComparison);

            var miEFCollate = _methodEFCollate.MakeGenericMethod(expression.Type);

            var exprLeft = Expression.Call(miEFCollate, Expression.Constant(EF.Functions), methodCallExpression.Object, Expression.Constant(collation));
            var exprRight = Expression.Call(miEFCollate, Expression.Constant(EF.Functions), expression, Expression.Constant(collation));


            // "variable".Contains(EF.Functions.Collate("obj","Collation"))
            var newMethodCallExpression = Expression.Call(exprLeft, _methodContainsEquals, exprRight);

            return base.VisitMethodCall(newMethodCallExpression);
        }

        private Expression ProcessStringEquals(MethodCallExpression methodCallExpression)
        {
            var expression = methodCallExpression.Arguments[0];
            var stringOrdinal = methodCallExpression.Arguments[1] as ConstantExpression;
            var stringComparison = (StringComparison)stringOrdinal.Value;

            var collation = GetCollationName(stringComparison);

            var miEFCollate = _methodEFCollate.MakeGenericMethod(expression.Type);

            var exprLeft = Expression.Call(miEFCollate, Expression.Constant(EF.Functions), expression, Expression.Constant(collation));

            // "variable".Equals(EF.Functions.Collate("obj","Collation"))
            var newMethodCallExpression = Expression.Call(methodCallExpression.Object, _methodStringEquals, exprLeft);

            return base.VisitMethodCall(newMethodCallExpression);
        }

        string DatabaseProvider => _queryCompilationContext.ContextOptions.Extensions.OfType<WebroxDbContextOptionsExtension>().FirstOrDefault()?.DatabaseProvider;


        string GetCollationName(StringComparison stringComparison)
        {
            string collation = null;

            switch (DatabaseProvider)
            {
                case "sqlite":
                    {
                        collation = $"Webrox_{stringComparison}";
                    }
                    break;
                case "sqlserver":
                    {
                        collation = "DATABASE_DEFAULT";

                        switch (stringComparison)
                        {
                            case StringComparison.Ordinal:
                                collation = "SQL_Latin1_General_CP1_CS_AS";
                                break;
                            case StringComparison.OrdinalIgnoreCase:
                                collation = "SQL_Latin1_General_CP1_CI_AS";
                                break;
                            case StringComparison.InvariantCulture:
                                collation = "SQL_Latin1_General_CP1_CS_AS";
                                break;
                            case StringComparison.InvariantCultureIgnoreCase:
                                collation = "SQL_Latin1_General_CP1_CI_AS";
                                break;
                            case StringComparison.CurrentCulture:
                                collation = "SQL_Latin1_General_CP1_CS_AS"; //GetCurrentCultureCollation(true, true);
                                break;
                            case StringComparison.CurrentCultureIgnoreCase:
                                collation = "SQL_Latin1_General_CP1_CI_AS";// GetCurrentCultureCollation(false, false);
                                break;

                        }
                    }
                    break;
                case "postgres":
                    {
                        collation = "default";

                        switch (stringComparison)
                        {
                            case StringComparison.Ordinal:
                                collation = "webrox_accent_case";
                                break;
                            case StringComparison.OrdinalIgnoreCase:
                                collation = "webrox_ignore_accent_case";
                                break;
                            case StringComparison.InvariantCulture:
                                collation = "webrox_accent_case";
                                break;
                            case StringComparison.InvariantCultureIgnoreCase:
                                collation = "webrox_ignore_accent_case";
                                break;
                            case StringComparison.CurrentCulture:
                                collation = "webrox_accent_case";
                                break;
                            case StringComparison.CurrentCultureIgnoreCase:
                                collation = "webrox_ignore_accent_case";
                                break;
                        }
                    }
                    break;
                case "mysql":
                    {
                        switch (stringComparison)
                        {
                            case StringComparison.Ordinal:
                                collation = "utf8mb4_0900_as_cs";
                                break;
                            case StringComparison.OrdinalIgnoreCase:
                                collation = "utf8mb4_0900_ai_ci";
                                break;
                            case StringComparison.InvariantCulture:
                                collation = "utf8mb4_0900_as_cs";
                                break;
                            case StringComparison.InvariantCultureIgnoreCase:
                                collation = "utf8mb4_0900_ai_ci";
                                break;
                            case StringComparison.CurrentCulture:
                                collation = "utf8mb4_0900_as_cs";
                                break;
                            case StringComparison.CurrentCultureIgnoreCase:
                                collation = "utf8mb4_0900_ai_ci";
                                break;
                        }
                    }
                    break;
            }

            return collation;
        }

        private Expression ProcessSelect(Expression source, LambdaExpression selector)
        {
            var piPending = source.GetType().GetProperty("PendingSelector");
            var pendingSelector = piPending.GetValue(source) as Expression;

            var selectorBody = ReplacingExpressionVisitor.Replace(
                selector.Parameters[0],
                pendingSelector,
                selector.Body);

            // RowNumber - 1
            selectorBody = ReplacingExpressionVisitor.Replace(
                selector.Parameters[1],
                Expression.Subtract(
                    Expression.Convert(
                        new RowNumberExpression(null, new List<OrderingExpression>(new[] { new OrderingExpression(_sqlExpressionFactory.Fragment("(SELECT NULL)"), true) })
                        ,RelationalTypeMapping.NullMapping), 
                        typeof(int))
                ,Expression.Constant(1, typeof(int))),
                selectorBody);

            var miApplySelector = source.GetType().GetMethod("ApplySelector");
            miApplySelector?.Invoke(source, new object[] { selectorBody });

            return source;
        }

        private Expression ProcessSelect2(Expression source, LambdaExpression selector)
        {
            //var piPending = source.GetType().GetProperty("PendingSelector");
            //var pendingSelector = piPending.GetValue(source) as Expression;

            //// var piType = pendingSelector.GetType().GetProperty("Type");

            //var selectorBody = ReplacingExpressionVisitor.Replace(
            //    selector.Parameters[0],
            //    pendingSelector,
            //    selector.Body);

            //var clrType = pendingSelector.Type;

            //MethodCallExpression predicateBody = null;

            //clrType = source.Type;
            // try
            // {
            //     var method = RelationalQueryableExtensions._asSubQuery.MakeGenericMethod(clrType.GenericTypeArguments[0]);

            //     var newSource = Expression.Call(
            //method,//QueryableMethods.Where.MakeGenericMethod(clrType),
            //Expression.Quote(Expression.Lambda(predicateBody, entityParameter)));


            //     predicateBody = Expression.Call(method,
            //         pendingSelector
            //         );
            // }
            // catch (Exception e)
            // {
            //     int uu = 0;
            //     uu++;
            // }


            //selectorBody = ReplacingExpressionVisitor.Replace(
            //    selector.Parameters[1],
            //    Expression.Convert(new RowNumberExpression(null,
            //    new List<OrderingExpression>(new[] { new OrderingExpression(_sqlExpressionFactory.Fragment("(SELECT NULL)"), true) }), RelationalTypeMapping.NullMapping), typeof(int)),
            //    selectorBody);

            //var miApplySelector = source.GetType().GetMethod("ApplySelector");
            //miApplySelector?.Invoke(source, new object[] { predicateBody });

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
