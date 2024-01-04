using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using System.Diagnostics.CodeAnalysis;
using Webrox.EntityFrameworkCore.Core.Infrastructure;

namespace Webrox.EntityFrameworkCore.Postgres.Query
{
    /// <inheritdoc />
    [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.")]
    public class WebroxPostgreSqlQuerySqlGeneratorFactory : IQuerySqlGeneratorFactory
    {
        private readonly QuerySqlGeneratorDependencies _dependencies;
        private readonly IRelationalTypeMappingSource _typeMappingSource;
#if NET7_0_OR_GREATER
           INpgsqlSingletonOptions _npgsqlSingletonOptions;
#else
        Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal.INpgsqlOptions _npgsqlSingletonOptions;
#endif
        // private readonly ITenantDatabaseProviderFactory _databaseProviderFactory;
        private readonly WebroxQuerySqlGenerator _webroxQuerySqlGenerator;

        /// <summary>
        /// Initializes new instance of <see cref="WebroxPostgreSqlQuerySqlGeneratorFactory"/>.
        /// </summary>
        /// <param name="dependencies">Dependencies.</param>
        /// <param name="typeMappingSource">Type mapping source.</param>
        /// <param name="sqlServerSingletonOptions">Options.</param>
        /// <param name="databaseProviderFactory">Factory.</param>
        public WebroxPostgreSqlQuerySqlGeneratorFactory(
           QuerySqlGeneratorDependencies dependencies,
           IRelationalTypeMappingSource typeMappingSource,
            WebroxQuerySqlGenerator webroxQuerySqlGenerator,
#if NET7_0_OR_GREATER
           INpgsqlSingletonOptions npgsqlSingletonOptions
#else
           Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal.INpgsqlOptions npgsqlSingletonOptions
#endif
           //ITenantDatabaseProviderFactory databaseProviderFactory
           )
        {
            _dependencies = dependencies ?? throw new ArgumentNullException(nameof(dependencies));
            _typeMappingSource = typeMappingSource ?? throw new ArgumentNullException(nameof(typeMappingSource));
            _webroxQuerySqlGenerator = webroxQuerySqlGenerator;
            _npgsqlSingletonOptions = npgsqlSingletonOptions;
            //_databaseProviderFactory = databaseProviderFactory ?? throw new ArgumentNullException(nameof(databaseProviderFactory));
        }

        /// <inheritdoc />
        public QuerySqlGenerator Create()
        {
            return new WebroxPostgreSqlQuerySqlGenerator(_dependencies, _typeMappingSource, _npgsqlSingletonOptions, _webroxQuerySqlGenerator);//, _sqlServerSingletonOptions, _databaseProviderFactory.Create());
        }
    }
}
