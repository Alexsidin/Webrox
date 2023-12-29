using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;
using Webrox.EntityFrameworkCore.Core.Expressions;

namespace Webrox.EntityFrameworkCore.Core
{
    public class RowNumberTranslator : IMethodCallTranslator
    {
        protected readonly ISqlExpressionFactory _sqlExpressionFactory;

        public RowNumberTranslator(ISqlExpressionFactory sqlExpressionFactory)
        {
            _sqlExpressionFactory = sqlExpressionFactory;
        }

        public SqlExpression? Translate(SqlExpression? instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
        {
            if (method.DeclaringType != typeof(DbFunctionsExtensions))
                return null;

            switch (method.Name)
            {
                case nameof(DbFunctionsExtensions.OrderBy):
                    {
                        var orderBy = arguments.Skip(1).Select(e => new OrderingExpression(_sqlExpressionFactory.ApplyDefaultTypeMapping(e), true)).ToList();
                        return new OrderByExpression(orderBy);
                    }
                case nameof(DbFunctionsExtensions.OrderByDescending):
                    {
                        var orderBy = arguments.Skip(1).Select(e => new OrderingExpression(_sqlExpressionFactory.ApplyDefaultTypeMapping(e), false)).ToList();
                        return new OrderByExpression(orderBy);
                    }
                case nameof(DbFunctionsExtensions.ThenBy):
                    {
                        var orderBy = arguments.Skip(1).Select(e => new OrderingExpression(_sqlExpressionFactory.ApplyDefaultTypeMapping(e), true));
                        return ((OrderByExpression)arguments[0]).AddColumns(orderBy);
                    }
                case nameof(DbFunctionsExtensions.ThenByDescending):
                    {
                        var orderBy = arguments.Skip(1).Select(e => new OrderingExpression(_sqlExpressionFactory.ApplyDefaultTypeMapping(e), false));
                        return ((OrderByExpression)arguments[0]).AddColumns(orderBy);
                    }
                case nameof(DbFunctionsExtensions.PartitionBy):
                    {
                        var partitionBy = arguments.Skip(1).ToList();
                        return new PartitionByExpression(partitionBy);
                    }
                case nameof(DbFunctionsExtensions.ThenPartitionBy):
                    {
                        var partitionBy = arguments.Skip(1).ToList();
                        return ((PartitionByExpression)arguments[0]).AddColumns(partitionBy);
                    }
                case nameof(DbFunctionsExtensions.RowNumber):
                    {
                        var partitionBy = arguments[^2] as PartitionByExpression;
                        var orderings = arguments[^1] as OrderByExpression;
                        return new RowNumberExpression(partitionBy?.Partitions, orderings.Orderings, RelationalTypeMapping.NullMapping);
                    }
                default:
                    return null;
            }
        }
    }
}
