using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Webrox.EntityFrameworkCore.Core.Interfaces;

namespace Webrox.EntityFrameworkCore.Core.Infrastructure
{
    /// <summary>
    /// Extends <see cref="SqlNullabilityProcessor"/>.
    /// </summary>
    public class WebroxSqlNullabilityProcessor : SqlNullabilityProcessor
    {
        private readonly ISqlExpressionFactory _sqlExpressionFactory;

        /// <inheritdoc />
        public WebroxSqlNullabilityProcessor(
           RelationalParameterBasedSqlProcessorDependencies dependencies,
           bool useRelationalNulls)
           : base(dependencies, useRelationalNulls)
        {
            _sqlExpressionFactory = dependencies.SqlExpressionFactory;
        }

        /// <inheritdoc />
        protected override SqlExpression VisitCustomSqlExpression(SqlExpression sqlExpression, bool allowOptimizedExpansion, out bool nullable)
        {
            if (sqlExpression is INotNullableSqlExpression)
            {
                nullable = false;
                return sqlExpression;
            }

            return base.VisitCustomSqlExpression(sqlExpression, allowOptimizedExpansion, out nullable);
        }
    }
}