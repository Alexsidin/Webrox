using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using MySql.EntityFrameworkCore.Design.Tests;
using MySql.EntityFrameworkCore.Infrastructure.Internal;
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
        private readonly IMySQLOptions _mySQLOptions;
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
            IMySQLOptions mySQLOptions
           //ITenantDatabaseProviderFactory databaseProviderFactory
           )
        {
            _dependencies = dependencies ?? throw new ArgumentNullException(nameof(dependencies));
            _typeMappingSource = typeMappingSource ?? throw new ArgumentNullException(nameof(typeMappingSource));
            _webroxQuerySqlGenerator = webroxQuerySqlGenerator;
            _mySQLOptions = mySQLOptions;
            //_databaseProviderFactory = databaseProviderFactory ?? throw new ArgumentNullException(nameof(databaseProviderFactory));
        }

        /// <inheritdoc />
        public QuerySqlGenerator Create()
        {
            return new WebroxMySqlQuerySqlGeneratorProxy().Create(_dependencies, _webroxQuerySqlGenerator);//, _sqlServerSingletonOptions, _databaseProviderFactory.Create());
        }
    }
}
