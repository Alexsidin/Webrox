﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;
using Webrox.EntityFrameworkCore.Core.Expressions;
using Webrox.EntityFrameworkCore.Core.SqlExpressions;
using Webrox.Models;

namespace Webrox.EntityFrameworkCore.Core.Translators
{
    /// <summary>
    /// Row Number Translator
    /// </summary>
    public class RowNumberTranslator : IMethodCallTranslator
    {
        private readonly ISqlExpressionFactory _sqlExpressionFactory;

        //private MethodInfo methodSelectIndex = typeof()

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sqlExpressionFactory"></param>
        public RowNumberTranslator(ISqlExpressionFactory sqlExpressionFactory)
        {
            _sqlExpressionFactory = sqlExpressionFactory;
        }

        /// <thendoc />
        public SqlExpression? Translate(SqlExpression? instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
        {
            if (method.DeclaringType != typeof(DbFunctionsExtensions))
                return null;

            switch (method.Name)
            {
                case "Select":
                    {
                        var orderByAsc = arguments.Skip(1).Select(e => new OrderingExpression(_sqlExpressionFactory.ApplyDefaultTypeMapping(e), true)).ToList();
                        return new ListExpressions<OrderingExpression, OrderByClause>(orderByAsc);
                    }
                    break;
                case nameof(DbFunctionsExtensions.OrderBy):
                    {
                        var orderByAsc = arguments.Skip(1).Select(e => new OrderingExpression(_sqlExpressionFactory.ApplyDefaultTypeMapping(e), true)).ToList();
                        return new ListExpressions<OrderingExpression, OrderByClause>(orderByAsc);
                    }
                case nameof(DbFunctionsExtensions.OrderByDescending):
                    {
                        var orderByDesc = arguments.Skip(1).Select(e => new OrderingExpression(_sqlExpressionFactory.ApplyDefaultTypeMapping(e), false)).ToList();
                        return new ListExpressions<OrderingExpression, OrderByClause>(orderByDesc);
                    }
                case nameof(DbFunctionsExtensions.ThenBy):
                    {
                        var thenByAsc = arguments.Skip(1).Select(e => new OrderingExpression(_sqlExpressionFactory.ApplyDefaultTypeMapping(e), true));
                        return ((ListExpressions<OrderingExpression, OrderByClause>)arguments[0]).AddColumns(thenByAsc);
                    }
                case nameof(DbFunctionsExtensions.ThenByDescending):
                    {
                        var thenByDesc = arguments.Skip(1).Select(e => new OrderingExpression(_sqlExpressionFactory.ApplyDefaultTypeMapping(e), false));
                        return ((ListExpressions<OrderingExpression, OrderByClause>)arguments[0]).AddColumns(thenByDesc);
                    }
                case nameof(DbFunctionsExtensions.PartitionBy):
                    {
                        var partitionBy = arguments.Skip(1).ToList();
                        return new ListExpressions<SqlExpression, PartitionByClause>(partitionBy);
                    }
                case nameof(DbFunctionsExtensions.ThenPartitionBy):
                    {
                        var thenPartitionBy = arguments.Skip(1);
                        return ((ListExpressions<SqlExpression, PartitionByClause>)arguments[0]).AddColumns(thenPartitionBy);
                    }
                case nameof(DbFunctionsExtensions.RowNumber):
                    {
                        var partitionBy = arguments[^2] as ListExpressions<SqlExpression, PartitionByClause>;
                        var partitions = partitionBy?.Expressions;

                        var ordering = arguments[^1] as ListExpressions<OrderingExpression, OrderByClause>;
                        var orderings = ordering?.Expressions;

                        return new RowNumberExpression(partitions, orderings!, RelationalTypeMapping.NullMapping);
                    }
                case nameof(DbFunctionsExtensions.Rank):
                    return CreateWindowExpression("RANK", arguments, isNoColumnExpression: true);
                case nameof(DbFunctionsExtensions.DenseRank):
                    return CreateWindowExpression("DENSE_RANK", arguments, isNoColumnExpression: true);
                case nameof(DbFunctionsExtensions.Sum):
                    return CreateWindowExpression("SUM", arguments);
                case nameof(DbFunctionsExtensions.Average):
                    return CreateWindowExpression("AVG", arguments);
                case nameof(DbFunctionsExtensions.Min):
                    return CreateWindowExpression("MIN", arguments);
                case nameof(DbFunctionsExtensions.Max):
                    return CreateWindowExpression("MAX", arguments);
                case nameof(DbFunctionsExtensions.NTile):
                    return CreateWindowExpression("NTILE", arguments);
                default:
                    return null;
            }
        }

        WindowExpression CreateWindowExpression(string aggregateFunction,
            IReadOnlyList<SqlExpression> arguments,
            bool isNoColumnExpression = false
            )
        {
            var partitionBy = arguments[^2] as ListExpressions<SqlExpression, PartitionByClause>;
            var partitions = partitionBy?.Expressions;

            var expression = isNoColumnExpression ? null : _sqlExpressionFactory.ApplyDefaultTypeMapping(arguments[^(partitions != null ? 3 : 2)]);

            var ordering = arguments[^1] as ListExpressions<OrderingExpression, OrderByClause>;
            var orderings = ordering?.Expressions;

            return new WindowExpression(aggregateFunction, expression, partitions, orderings!, RelationalTypeMapping.NullMapping);

        }
    }
}