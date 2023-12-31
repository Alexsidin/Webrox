using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Webrox.EntityFrameworkCore.Core.Interfaces;

namespace Webrox.EntityFrameworkCore.Sqlite.Query
{
    /// <summary>
    /// Extends <see cref="SqlNullabilityProcessor"/>.
    /// </summary>
    public class WebroxSqlNullabilityProcessor : SqlNullabilityProcessor
    {
        /// <inheritdoc />
        public WebroxSqlNullabilityProcessor(
           RelationalParameterBasedSqlProcessorDependencies dependencies,
           bool useRelationalNulls)
           : base(dependencies, useRelationalNulls)
        {
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