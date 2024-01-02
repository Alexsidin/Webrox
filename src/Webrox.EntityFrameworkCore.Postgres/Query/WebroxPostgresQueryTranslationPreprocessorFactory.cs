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
        private readonly INpgsqlSingletonOptions _npgsqlSingletonOptions;

        public WebroxPostgresQueryTranslationPreprocessorFactory(
            QueryTranslationPreprocessorDependencies dependencies,
            RelationalQueryTranslationPreprocessorDependencies relationalDependencies,
            ISqlExpressionFactory sqlExpressionFactory,
            INpgsqlSingletonOptions npgsqlSingletonOptions
            )
            : base(dependencies
#if NET8_0_OR_GREATER
                  , relationalDependencies, npgsqlSingletonOptions
#endif
                  )
        {
            _sqlExpressionFactory = sqlExpressionFactory;
            _npgsqlSingletonOptions = npgsqlSingletonOptions;
        }

        public override QueryTranslationPreprocessor Create(QueryCompilationContext queryCompilationContext)
        {
#if NET8_0_OR_GREATER
            
            return new WebroxPostgresQueryTranslationPreprocessor(Dependencies,
                queryCompilationContext, 
                RelationalDependencies,
                _npgsqlSingletonOptions,
                _sqlExpressionFactory);
            
#else
            return base.Create(queryCompilationContext);
#endif
        }
    }
}