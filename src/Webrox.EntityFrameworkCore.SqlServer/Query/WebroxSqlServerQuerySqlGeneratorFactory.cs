using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics.CodeAnalysis;
using Webrox.EntityFrameworkCore.Core;

namespace Webrox.EntityFrameworkCore.SqlServer.Query
{
    /// <inheritdoc />
    [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.")]
    public class WebroxSqlServerQuerySqlGeneratorFactory : IQuerySqlGeneratorFactory
    {
        private readonly QuerySqlGeneratorDependencies _dependencies;
        private readonly IRelationalTypeMappingSource _typeMappingSource;
#if NET8_0_OR_GREATER
        private readonly ISqlServerSingletonOptions _sqlServerSingletonOptions;
#endif
        // private readonly ITenantDatabaseProviderFactory _databaseProviderFactory;
        private readonly WebroxQuerySqlGenerator _webroxQuerySqlGenerator;

        /// <summary>
        /// Initializes new instance of <see cref="WebroxSqlServerQuerySqlGeneratorFactory"/>.
        /// </summary>
        /// <param name="dependencies">Dependencies.</param>
        /// <param name="typeMappingSource">Type mapping source.</param>
        /// <param name="sqlServerSingletonOptions">Options.</param>
        /// <param name="databaseProviderFactory">Factory.</param>
        public WebroxSqlServerQuerySqlGeneratorFactory(
           QuerySqlGeneratorDependencies dependencies,
           IRelationalTypeMappingSource typeMappingSource,
            WebroxQuerySqlGenerator webroxQuerySqlGenerator
#if NET8_0_OR_GREATER
            ,ISqlServerSingletonOptions sqlServerSingletonOptions
#endif
            )
        {
            _dependencies = dependencies ?? throw new ArgumentNullException(nameof(dependencies));
            _typeMappingSource = typeMappingSource ?? throw new ArgumentNullException(nameof(typeMappingSource));
            _webroxQuerySqlGenerator = webroxQuerySqlGenerator;
#if NET8_0_OR_GREATER
            _sqlServerSingletonOptions = sqlServerSingletonOptions;
#endif
        }

        /// <inheritdoc />
        public QuerySqlGenerator Create()
        {
            return new WebroxSqlServerQuerySqlGenerator(_dependencies, _typeMappingSource,
#if NET8_0_OR_GREATER
                _sqlServerSingletonOptions, 
#endif
                _webroxQuerySqlGenerator);
        }
    }
}
