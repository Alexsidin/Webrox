using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;
using Webrox.EntityFrameworkCore.Core.Expressions;
using Webrox.Models;

namespace Webrox.EntityFrameworkCore.Core
{
    /// <summary>
    /// Row Number Translator
    /// </summary>
    public class RowNumberTranslator : IMethodCallTranslator
    {
        private readonly ISqlExpressionFactory _sqlExpressionFactory;

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
                default:
                    return null;
            }
        }
    }
}