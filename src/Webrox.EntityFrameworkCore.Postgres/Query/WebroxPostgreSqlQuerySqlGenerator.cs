using Microsoft.EntityFrameworkCore.Query;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Webrox.EntityFrameworkCore.Core.SqlExpressions;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using Webrox.EntityFrameworkCore.Core.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Internal;

namespace Webrox.EntityFrameworkCore.Postgres.Query
{
    /// <inheritdoc />
    [SuppressMessage("Usage", "EF1001", MessageId = "Internal EF Core API usage.")]
    public class WebroxPostgreSqlQuerySqlGenerator : NpgsqlQuerySqlGenerator
    {
        //private readonly ITenantDatabaseProvider _databaseProvider;
        private readonly WebroxQuerySqlGenerator _webroxQuerySqlGenerator;
        /// <inheritdoc />
        public WebroxPostgreSqlQuerySqlGenerator(
           QuerySqlGeneratorDependencies dependencies,
           IRelationalTypeMappingSource typeMappingSource,
#if NET7_0_OR_GREATER
           INpgsqlSingletonOptions npgsqlSingletonOptions,
#else
           Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal.INpgsqlOptions npgsqlSingletonOptions,
#endif
           WebroxQuerySqlGenerator webroxQuerySqlGenerator
            //ITenantDatabaseProvider databaseProvider
            )
           : base(dependencies,
#if NET8_0_OR_GREATER
                 typeMappingSource,
#endif
                 npgsqlSingletonOptions.ReverseNullOrderingEnabled, 
                 npgsqlSingletonOptions.PostgresVersion)
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