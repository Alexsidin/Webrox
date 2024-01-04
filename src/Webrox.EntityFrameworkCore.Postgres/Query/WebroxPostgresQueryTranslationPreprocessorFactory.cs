using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Internal;

namespace Webrox.EntityFrameworkCore.Postgres.Query
{
    /// <summary>
    /// Factory for creation of the <see cref="WebroxSqliteRelationalSqlTranslatingExpressionVisitor"/>.
    /// </summary>
    public sealed class WebroxPostgresQueryTranslationPreprocessorFactory :
#if NET8_0_OR_GREATER
        NpgsqlQueryTranslationPreprocessorFactory
#else
        QueryTranslationPreprocessorFactory
#endif
    {
        private readonly ISqlExpressionFactory _sqlExpressionFactory;
#if NET7_0_OR_GREATER
           INpgsqlSingletonOptions _npgsqlSingletonOptions;
#else
        Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal.INpgsqlOptions _npgsqlSingletonOptions;
#endif
        private readonly RelationalQueryTranslationPreprocessorDependencies _relationalDependencies;

        public WebroxPostgresQueryTranslationPreprocessorFactory(
            QueryTranslationPreprocessorDependencies dependencies,
            RelationalQueryTranslationPreprocessorDependencies relationalDependencies,
            ISqlExpressionFactory sqlExpressionFactory,
#if NET7_0_OR_GREATER
            INpgsqlSingletonOptions npgsqlSingletonOptions
#else
            Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal.INpgsqlOptions npgsqlSingletonOptions
#endif
            )
            : base(dependencies
#if NET8_0_OR_GREATER
                  , relationalDependencies, npgsqlSingletonOptions
#endif
                  )
        {
            _sqlExpressionFactory = sqlExpressionFactory;
            _npgsqlSingletonOptions = npgsqlSingletonOptions;
            _relationalDependencies = relationalDependencies;
        }

        public override QueryTranslationPreprocessor Create(QueryCompilationContext queryCompilationContext)
        {
            return new WebroxPostgresQueryTranslationPreprocessor(Dependencies,
                queryCompilationContext,
                _relationalDependencies,
                _npgsqlSingletonOptions,
                _sqlExpressionFactory);
            

        }
    }
}