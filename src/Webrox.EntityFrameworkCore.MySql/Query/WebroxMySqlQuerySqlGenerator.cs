using Microsoft.EntityFrameworkCore.Query;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Webrox.EntityFrameworkCore.Core;
using Webrox.EntityFrameworkCore.Core.SqlExpressions;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace Webrox.EntityFrameworkCore.MySql.Query
{
    /// <inheritdoc />
    [SuppressMessage("Usage", "EF1001", MessageId = "Internal EF Core API usage.")]
    public class WebroxMySqlQuerySqlGenerator : NpgsqlQuerySqlGenerator
    {
        //private readonly ITenantDatabaseProvider _databaseProvider;
        private readonly WebroxQuerySqlGenerator _webroxQuerySqlGenerator;
        /// <inheritdoc />
        public WebroxMySqlQuerySqlGenerator(
           QuerySqlGeneratorDependencies dependencies,
           IRelationalTypeMappingSource typeMappingSource,
           INpgsqlSingletonOptions npgsqlSingletonOptions,
           WebroxQuerySqlGenerator webroxQuerySqlGenerator
            //ITenantDatabaseProvider databaseProvider
            )
           : base(dependencies, typeMappingSource, npgsqlSingletonOptions.ReverseNullOrderingEnabled, npgsqlSingletonOptions.PostgresVersion)
        {
            _webroxQuerySqlGenerator = webroxQuerySqlGenerator;
           // _databaseProvider = databaseProvider ?? throw new ArgumentNullException(nameof(databaseProvider));
        }

        /// <inheritdoc />
        protected override Expression VisitExtension(Expression extensionExpression)
        {
            switch (extensionExpression)
            {
                case WindowExpression windowExpression:
                    return _webroxQuerySqlGenerator.VisitWindowFunction(Sql, windowExpression, Visit, VisitOrdering);
                default:
                    return base.VisitExtension(extensionExpression);
            }
        }
    }
}