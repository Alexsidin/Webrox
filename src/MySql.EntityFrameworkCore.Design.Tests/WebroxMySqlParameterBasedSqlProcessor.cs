using Microsoft.EntityFrameworkCore.Query;
using MySqlLib = MySql.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using MySql.EntityFrameworkCore.Infrastructure.Internal;
using Webrox.EntityFrameworkCore.Core.Interfaces;
using Webrox.EntityFrameworkCore.Core;
using Webrox.EntityFrameworkCore.Core.Infrastructure;

namespace MySql.EntityFrameworkCore.Design.Tests
{
    public class WebroxMySqlParameterBasedSqlProcessorProxy
    {
        public RelationalParameterBasedSqlProcessor Create(RelationalParameterBasedSqlProcessorDependencies dependencies,
            bool useRelationalNulls,
            IMySQLOptions mySQLOptions)
        {
            return new WebroxMySqlParameterBasedSqlProcessor(dependencies,
             useRelationalNulls,
             mySQLOptions);
        }
    }
    //  RelationalParameterBasedSqlProcessor
    /// <summary>
    /// Extends <see cref="RelationalParameterBasedSqlProcessor"/>.
    /// </summary>
    [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.")]
    internal class WebroxMySqlParameterBasedSqlProcessor
#if NET7_0_OR_GREATER
        : MySql.EntityFrameworkCore.Query.Internal.MySQLParameterBasedSqlProcessor
#else
        : RelationalParameterBasedSqlProcessor
#endif
    {
        private readonly IMySQLOptions _mySQLOptions;

        QuerySqlGenerator _mySQLQuerySqlGenerator;
        /// <inheritdoc />
        public WebroxMySqlParameterBasedSqlProcessor(
           RelationalParameterBasedSqlProcessorDependencies dependencies,
            bool useRelationalNulls,
            IMySQLOptions mySQLOptions
            )
           : base(dependencies, useRelationalNulls
#if NET7_0_OR_GREATER
                 , mySQLOptions
#endif
                 )
        {
            _mySQLOptions = mySQLOptions;
        }

        /// <inheritdoc />
        protected override
#if NET7_0_OR_GREATER
            Expression
#else
            SelectExpression
#endif
            ProcessSqlNullability(
#if NET7_0_OR_GREATER
            Expression
#else
            SelectExpression
#endif
            expression, IReadOnlyDictionary<string, object?> parametersValues, out bool canCache)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            if (parametersValues == null)
            {
                throw new ArgumentNullException(nameof(parametersValues));
            }

            return new WebroxMySqlNullabilityProcessor(Dependencies, UseRelationalNulls)
                  .Process(expression, parametersValues, out canCache);
        }
    }

    class WebroxMySqlNullabilityProcessor : WebroxSqlNullabilityProcessor
    {
        private readonly ISqlExpressionFactory _sqlExpressionFactory;
#if NET7_0_OR_GREATER
        private MySql.EntityFrameworkCore.Query.Internal.MySQLSqlNullabilityProcessor _MySQLSqlNullabilityProcessor; //reflection
#endif
        private MethodInfo _VisitCustomSqlExpression;

        /// <inheritdoc />
        public WebroxMySqlNullabilityProcessor(
           RelationalParameterBasedSqlProcessorDependencies dependencies,
           bool useRelationalNulls)
           : base(dependencies, useRelationalNulls)
        {
            this._sqlExpressionFactory = dependencies.SqlExpressionFactory;
#if NET7_0_OR_GREATER
            _MySQLSqlNullabilityProcessor = new Query.Internal.MySQLSqlNullabilityProcessor(dependencies, useRelationalNulls);
            _VisitCustomSqlExpression = _MySQLSqlNullabilityProcessor.GetType().GetMethod(nameof(VisitCustomSqlExpression), BindingFlags.Instance | BindingFlags.NonPublic);
#endif
        }

        /// <inheritdoc />
        protected override SqlExpression VisitCustomSqlExpression(SqlExpression sqlExpression, bool allowOptimizedExpansion, out bool nullable)
        {
            if (sqlExpression is INotNullableSqlExpression)
            {
                nullable = false;
                return sqlExpression;
            }
#if NET7_0_OR_GREATER
            var parameters = new object[]
            {
                sqlExpression, allowOptimizedExpansion, null
            };
            var ret = _VisitCustomSqlExpression?.Invoke(_MySQLSqlNullabilityProcessor, parameters) as SqlExpression;

            nullable = (bool)parameters[2];
            return ret;
#else
            return base.VisitCustomSqlExpression(sqlExpression, allowOptimizedExpansion, out nullable);
#endif
            
        }
    }
}