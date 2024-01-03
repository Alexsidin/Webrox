using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;
using Webrox.EntityFrameworkCore.Core.Expressions;
using Webrox.EntityFrameworkCore.Core.Interfaces;
using Webrox.EntityFrameworkCore.Core.Models;
using Webrox.EntityFrameworkCore.Core.SqlExpressions;

namespace Webrox.EntityFrameworkCore.Core.Translators
{
    /// <summary>
    /// Row Number Translator
    /// </summary>
    public class DbFunctionsExtensionsTranslator : IMethodCallTranslator
    {
        private readonly ISqlExpressionFactory _sqlExpressionFactory;

        //private MethodInfo methodSelectIndex = typeof()

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sqlExpressionFactory"></param>
        public DbFunctionsExtensionsTranslator(ISqlExpressionFactory sqlExpressionFactory)
        {
            _sqlExpressionFactory = sqlExpressionFactory;
        }

        /// <thendoc />
        public SqlExpression? Translate(SqlExpression? instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
        {
            if (!method.DeclaringType.Namespace.StartsWith("Webrox.EntityFrameworkCore"))
                return null;

            switch (method.Name)
            {
                case nameof(IDbFunctionsExtensions.OrderBy):
                    {
                        var orderByAsc = arguments.Skip(1).Select(e => new OrderingExpression(_sqlExpressionFactory.ApplyDefaultTypeMapping(e), true)).ToList();
                        return new ListExpressions<OrderingExpression, OrderByClause>(orderByAsc);
                    }
                case nameof(IDbFunctionsExtensions.OrderByDescending):
                    {
                        var orderByDesc = arguments.Skip(1).Select(e => new OrderingExpression(_sqlExpressionFactory.ApplyDefaultTypeMapping(e), false)).ToList();
                        return new ListExpressions<OrderingExpression, OrderByClause>(orderByDesc);
                    }
                case nameof(IDbFunctionsExtensions.ThenBy):
                    {
                        var thenByAsc = arguments.Skip(1).Select(e => new OrderingExpression(_sqlExpressionFactory.ApplyDefaultTypeMapping(e), true));
                        return ((ListExpressions<OrderingExpression, OrderByClause>)arguments[0]).AddColumns(thenByAsc);
                    }
                case nameof(IDbFunctionsExtensions.ThenByDescending):
                    {
                        var thenByDesc = arguments.Skip(1).Select(e => new OrderingExpression(_sqlExpressionFactory.ApplyDefaultTypeMapping(e), false));
                        return ((ListExpressions<OrderingExpression, OrderByClause>)arguments[0]).AddColumns(thenByDesc);
                    }
                case nameof(IDbFunctionsExtensions.PartitionBy):
                    {
                        var partitionBy = arguments.Skip(1).ToList();
                        return new ListExpressions<SqlExpression, PartitionByClause>(partitionBy);
                    }
                case nameof(IDbFunctionsExtensions.ThenPartitionBy):
                    {
                        var thenPartitionBy = arguments.Skip(1);
                        return ((ListExpressions<SqlExpression, PartitionByClause>)arguments[0]).AddColumns(thenPartitionBy);
                    }
                case nameof(IDbFunctionsExtensions.RowNumber):
                    {
                        var partitionBy = arguments[^2] as ListExpressions<SqlExpression, PartitionByClause>;
                        var partitions = partitionBy?.Expressions;

                        var ordering = arguments[^1] as ListExpressions<OrderingExpression, OrderByClause>;
                        var orderings = ordering?.Expressions;

                        return new RowNumberExpression(partitions, orderings!, RelationalTypeMapping.NullMapping);
                    }
                case nameof(IDbFunctionsExtensions.Rank):
                    return CreateWindowExpression("RANK", arguments, isNoColumnExpression: true);
                case nameof(IDbFunctionsExtensions.DenseRank):
                    return CreateWindowExpression("DENSE_RANK", arguments, isNoColumnExpression: true);
                case nameof(IDbFunctionsExtensions.Sum):
                    return CreateWindowExpression("SUM", arguments);
                case nameof(IDbFunctionsExtensions.Average):
                    return CreateWindowExpression("AVG", arguments);
                case nameof(IDbFunctionsExtensions.Min):
                    return CreateWindowExpression("MIN", arguments, convertToType: typeof(long));
                case nameof(IDbFunctionsExtensions.Max):
                    return CreateWindowExpression("MAX", arguments, convertToType: typeof(long));
                case nameof(IDbFunctionsExtensions.NTile):
                    return CreateWindowExpression("NTILE", arguments);
                default:
                    return null;
            }
        }

        SqlExpression CreateWindowExpression(string aggregateFunction,
            IReadOnlyList<SqlExpression> arguments,
            bool isNoColumnExpression = false,
            Type convertToType = null
            )
        {
            var partitionBy = arguments[^2] as ListExpressions<SqlExpression, PartitionByClause>;
            var partitions = partitionBy?.Expressions;

            var expression = isNoColumnExpression ? null : _sqlExpressionFactory.ApplyDefaultTypeMapping(arguments[^(partitions != null ? 3 : 2)]);

            var ordering = arguments[^1] as ListExpressions<OrderingExpression, OrderByClause>;
            var orderings = ordering?.Expressions;

            SqlExpression retExpression = new WindowExpression(aggregateFunction, expression, partitions, orderings!, RelationalTypeMapping.NullMapping);
            if (convertToType != null)
            {
                retExpression = _sqlExpressionFactory.Convert(retExpression, convertToType);
            }
            return retExpression;
        }
    }
}