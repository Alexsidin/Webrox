using Microsoft.EntityFrameworkCore.Query;
using MySqlLib = MySql.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Webrox.EntityFrameworkCore.Sqlite.Query;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using MySql.EntityFrameworkCore.Infrastructure.Internal;
using Webrox.EntityFrameworkCore.Core.Interfaces;

namespace Webrox.EntityFrameworkCore.MySql.Query
{
    //  RelationalParameterBasedSqlProcessor
    /// <summary>
    /// Extends <see cref="RelationalParameterBasedSqlProcessor"/>.
    /// </summary>
    [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.")]
    public class WebroxMySqlParameterBasedSqlProcessor : RelationalParameterBasedSqlProcessor
    {
        private readonly IMySQLOptions _mySQLOptions;

        QuerySqlGenerator _mySQLQuerySqlGenerator;
        /// <inheritdoc />
        public WebroxMySqlParameterBasedSqlProcessor(
           RelationalParameterBasedSqlProcessorDependencies dependencies,
            bool useRelationalNulls,
            IMySQLOptions mySQLOptions
            )
           : base(dependencies, useRelationalNulls)
        {
            _mySQLOptions = mySQLOptions;
        }

        /// <inheritdoc />
        protected override Expression ProcessSqlNullability(Expression expression, IReadOnlyDictionary<string, object?> parametersValues, out bool canCache)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            if (parametersValues == null)
            {
                throw new ArgumentNullException(nameof(parametersValues));
            }

            //return new WebroxSqlNullabilityProcessor(Dependencies, UseRelationalNulls)
            //    .Process(expression, parametersValues, out canCache);
            return new WebroxMySqlNullabilityProcessor(Dependencies, UseRelationalNulls)
                  .Process(expression, parametersValues, out canCache);
        }
    }

    class WebroxMySqlNullabilityProcessor : WebroxSqlNullabilityProcessor
    {
        private readonly ISqlExpressionFactory _sqlExpressionFactory;
        
        private object _MySQLSqlNullabilityProcessor; //reflection
        private MethodInfo _VisitCustomSqlExpression;

        /// <inheritdoc />
        public WebroxMySqlNullabilityProcessor(
           RelationalParameterBasedSqlProcessorDependencies dependencies,
           bool useRelationalNulls)
           : base(dependencies, useRelationalNulls)
        {
            this._sqlExpressionFactory = dependencies.SqlExpressionFactory;

            var assembly = typeof(MySqlLib.Query.Internal.MySQLCommandParser).Assembly;
            var type = assembly.GetType("MySql.EntityFrameworkCore.Query.Internal.MySQLSqlNullabilityProcessor");

            _MySQLSqlNullabilityProcessor = Activator.CreateInstance(type, new object[] { dependencies, useRelationalNulls });
            _VisitCustomSqlExpression = type.GetMethod(nameof(VisitCustomSqlExpression), BindingFlags.Instance | BindingFlags.NonPublic);
        }

        /// <inheritdoc />
        protected override SqlExpression VisitCustomSqlExpression(SqlExpression sqlExpression, bool allowOptimizedExpansion, out bool nullable)
        {
            if (sqlExpression is INotNullableSqlExpression)
            {
                nullable = false;
                return sqlExpression;
            }

            var parameters = new object[]
            {
                sqlExpression, allowOptimizedExpansion, null
            };
            var ret = _VisitCustomSqlExpression?.Invoke(_MySQLSqlNullabilityProcessor, parameters) as SqlExpression;

            nullable = (bool)parameters[2];
            return ret;
            //return base.VisitCustomSqlExpression(sqlExpression, allowOptimizedExpansion, out nullable);
        }
    }
}