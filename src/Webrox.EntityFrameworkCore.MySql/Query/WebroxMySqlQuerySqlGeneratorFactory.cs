using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using System.Diagnostics.CodeAnalysis;
using Webrox.EntityFrameworkCore.Core;

namespace Webrox.EntityFrameworkCore.MySql.Query
{
    /// <inheritdoc />
    [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.")]
    public class WebroxMySqlQuerySqlGeneratorFactory : IQuerySqlGeneratorFactory
    {
        private readonly QuerySqlGeneratorDependencies _dependencies;
        private readonly IRelationalTypeMappingSource _typeMappingSource;
        private readonly INpgsqlSingletonOptions _npgsqlSingletonOptions;
        // private readonly ITenantDatabaseProviderFactory _databaseProviderFactory;
        private readonly WebroxQuerySqlGenerator _webroxQuerySqlGenerator;

        /// <summary>
        /// Initializes new instance of <see cref="WebroxMySqlQuerySqlGeneratorFactory"/>.
        /// </summary>
        /// <param name="dependencies">Dependencies.</param>
        /// <param name="typeMappingSource">Type mapping source.</param>
        /// <param name="sqlServerSingletonOptions">Options.</param>
        /// <param name="databaseProviderFactory">Factory.</param>
        public WebroxMySqlQuerySqlGeneratorFactory(
           QuerySqlGeneratorDependencies dependencies,
           IRelationalTypeMappingSource typeMappingSource,
            WebroxQuerySqlGenerator webroxQuerySqlGenerator,
            INpgsqlSingletonOptions npgsqlSingletonOptions
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
            return new WebroxMySqlQuerySqlGenerator(_dependencies, _typeMappingSource, _npgsqlSingletonOptions, _webroxQuerySqlGenerator);//, _sqlServerSingletonOptions, _databaseProviderFactory.Create());
        }
    }
}
