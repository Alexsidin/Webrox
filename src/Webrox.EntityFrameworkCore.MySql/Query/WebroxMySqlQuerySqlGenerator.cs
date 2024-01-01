using Microsoft.EntityFrameworkCore.Query;
using MySqlLib = MySql.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MySql.EntityFrameworkCore.Infrastructure.Internal;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using Webrox.EntityFrameworkCore.Core;
using Webrox.EntityFrameworkCore.Core.SqlExpressions;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Webrox.EntityFrameworkCore.MySql.Query
{
    /// <inheritdoc />
    [SuppressMessage("Usage", "EF1001", MessageId = "Internal EF Core API usage.")]
    public class WebroxMySqlQuerySqlGenerator : QuerySqlGenerator
    {
        //private readonly ITenantDatabaseProvider _databaseProvider;
        private readonly WebroxQuerySqlGenerator _webroxQuerySqlGenerator;
        private readonly IMySQLOptions _mySQLOptions;
        object _MySQLQuerySqlGenerator;
        MethodInfo _VisitExtension, _VisitCrossApply,
            _VisitOuterApply, _VisitSqlBinary,
            _VisitSqlFunction, _VisitSqlUnary
            ;

        /// <inheritdoc />
        public WebroxMySqlQuerySqlGenerator(
           QuerySqlGeneratorDependencies dependencies,
           IRelationalTypeMappingSource typeMappingSource,
           IMySQLOptions mySQLOptions,
           WebroxQuerySqlGenerator webroxQuerySqlGenerator
            //ITenantDatabaseProvider databaseProvider
            )
           : base(dependencies)//, typeMappingSource, mySQLOptions.ReverseNullOrderingEnabled, mySQLOptions.PostgresVersion)
        {
            _webroxQuerySqlGenerator = webroxQuerySqlGenerator;
            _mySQLOptions = mySQLOptions;
            // _databaseProvider = databaseProvider ?? throw new ArgumentNullException(nameof(databaseProvider));
            var assembly = typeof(MySqlLib.Query.Internal.MySQLCommandParser).Assembly;
            var type = assembly.GetType("MySql.EntityFrameworkCore.Query.MySQLQuerySqlGenerator");
            
            _MySQLQuerySqlGenerator = Activator.CreateInstance(type, new object[] { dependencies });
            _VisitExtension = type.GetMethod(nameof(VisitExtension), BindingFlags.Instance | BindingFlags.NonPublic);
            _VisitCrossApply = type.GetMethod(nameof(VisitCrossApply), BindingFlags.Instance | BindingFlags.NonPublic);
            _VisitOuterApply = type.GetMethod(nameof(VisitOuterApply), BindingFlags.Instance | BindingFlags.NonPublic);
            _VisitSqlBinary = type.GetMethod(nameof(VisitSqlBinary), BindingFlags.Instance | BindingFlags.NonPublic);
            _VisitSqlFunction = type.GetMethod(nameof(VisitSqlFunction), BindingFlags.Instance | BindingFlags.NonPublic);
            _VisitSqlUnary = type.GetMethod(nameof(VisitSqlUnary), BindingFlags.Instance | BindingFlags.NonPublic);
        }

        /// <inheritdoc />
        protected override Expression VisitExtension(Expression extensionExpression)
        {
            switch (extensionExpression)
            {
                case WindowExpression windowExpression:
                    return _webroxQuerySqlGenerator.VisitWindowFunction(Sql, windowExpression, Visit, VisitOrdering);
                default:
                    return _VisitExtension?.Invoke(_MySQLQuerySqlGenerator, new[] { extensionExpression }) as Expression;
            }
        }

        protected override Expression VisitCrossApply(CrossApplyExpression crossApplyExpression)
        {
            return _VisitCrossApply?.Invoke(_MySQLQuerySqlGenerator, new[] { crossApplyExpression }) as Expression;
        }

        protected override Expression VisitOuterApply(OuterApplyExpression outerApplyExpression)
        {
            return _VisitOuterApply?.Invoke(_MySQLQuerySqlGenerator, new[] { outerApplyExpression }) as Expression;
        }
        protected override Expression VisitSqlBinary(SqlBinaryExpression sqlBinaryExpression)
        {
            return _VisitSqlBinary?.Invoke(_MySQLQuerySqlGenerator, new[] { sqlBinaryExpression }) as Expression;
        }
        protected override Expression VisitSqlFunction(SqlFunctionExpression sqlFunctionExpression)
        {
            return _VisitSqlFunction?.Invoke(_MySQLQuerySqlGenerator, new[] { sqlFunctionExpression }) as Expression;
        }
        protected override Expression VisitSqlUnary(SqlUnaryExpression sqlUnaryExpression)
        {
            return _VisitSqlUnary?.Invoke(_MySQLQuerySqlGenerator, new[] { sqlUnaryExpression }) as Expression;
        }
    }
}