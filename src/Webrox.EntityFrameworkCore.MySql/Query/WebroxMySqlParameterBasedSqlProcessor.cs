using Microsoft.EntityFrameworkCore.Query;
using MySqlLib = MySql.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Webrox.EntityFrameworkCore.Sqlite.Query;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Webrox.EntityFrameworkCore.MySql.Query
{
    //  RelationalParameterBasedSqlProcessor
    /// <summary>
    /// Extends <see cref="RelationalParameterBasedSqlProcessor"/>.
    /// </summary>
    [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.")]
    public class WebroxMySqlParameterBasedSqlProcessor : QuerySqlGenerator
    //RelationalParameterBasedSqlProcessor
    //MySqlLib.MySQLQuerySqlGenerator
    {
        QuerySqlGenerator _mySQLQuerySqlGenerator;
        /// <inheritdoc />
        public WebroxMySqlParameterBasedSqlProcessor(
           QuerySqlGeneratorDependencies dependencies)
           : base(dependencies)
        {
            var assemblyMySql = typeof(MySqlLib.Query.MySQLJsonString).Assembly;
            var typeMySQLQuerySqlGenerator = assemblyMySql.GetType("MySql.EntityFrameworkCore.Query.MySQLQuerySqlGenerator", false)
                ?? assemblyMySql.GetType("MySQLQuerySqlGenerator", false);

            var obj = Activator.CreateInstance(typeMySQLQuerySqlGenerator);
            _mySQLQuerySqlGenerator = obj as QuerySqlGenerator;
        }

        protected override Expression VisitExtension(Expression extensionExpression)
        {
            var method = _mySQLQuerySqlGenerator.GetType()
                .GetMethod(nameof(VisitExtension), BindingFlags.Instance | BindingFlags.NonPublic);

            return method?.Invoke(_mySQLQuerySqlGenerator, new[] { extensionExpression }) as Expression;
        }

        protected override Expression VisitSqlFunction(SqlFunctionExpression sqlFunctionExpression)
        {
            var method = _mySQLQuerySqlGenerator.GetType()
              .GetMethod(nameof(VisitSqlFunction), BindingFlags.Instance | BindingFlags.NonPublic);

            return method?.Invoke(_mySQLQuerySqlGenerator, new[] { sqlFunctionExpression }) as Expression;

        }

        protected override Expression VisitSqlBinary(SqlBinaryExpression sqlBinaryExpression)
        {
            var method = _mySQLQuerySqlGenerator.GetType()
             .GetMethod(nameof(VisitSqlBinary), BindingFlags.Instance | BindingFlags.NonPublic);

            return method?.Invoke(_mySQLQuerySqlGenerator, new[] { sqlBinaryExpression }) as Expression;

        }

        protected override Expression VisitSqlUnary(SqlUnaryExpression sqlUnaryExpression)
        {
            var method = _mySQLQuerySqlGenerator.GetType()
 .GetMethod(nameof(VisitSqlUnary), BindingFlags.Instance | BindingFlags.NonPublic);

            return method?.Invoke(_mySQLQuerySqlGenerator, new[] { sqlUnaryExpression }) as Expression;

        }

        protected override void GenerateLimitOffset(SelectExpression selectExpression)
        {
            var method = _mySQLQuerySqlGenerator.GetType()
.GetMethod(nameof(GenerateLimitOffset), BindingFlags.Instance | BindingFlags.NonPublic);

            method?.Invoke(_mySQLQuerySqlGenerator, new[] { selectExpression });
        }

        protected override Expression VisitCrossApply(CrossApplyExpression crossApplyExpression)
        {
            var method = _mySQLQuerySqlGenerator.GetType()
.GetMethod(nameof(VisitCrossApply), BindingFlags.Instance | BindingFlags.NonPublic);

            return method?.Invoke(_mySQLQuerySqlGenerator, new[] { crossApplyExpression }) as Expression;
        }

        protected override Expression VisitOuterApply(OuterApplyExpression outerApplyExpression)
        {
            var method = _mySQLQuerySqlGenerator.GetType()
.GetMethod(nameof(VisitOuterApply), BindingFlags.Instance | BindingFlags.NonPublic);

            return method?.Invoke(_mySQLQuerySqlGenerator, new[] { outerApplyExpression }) as Expression;
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

            return new WebroxSqlNullabilityProcessor(Dependencies, UseRelationalNulls).Process(expression, parametersValues, out canCache);
        }
    }
}