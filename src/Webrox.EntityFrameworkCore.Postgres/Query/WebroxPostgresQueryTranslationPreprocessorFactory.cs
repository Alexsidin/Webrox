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
        NpgsqlQueryTranslationPreprocessorFactory
        //QueryTranslationPreprocessorFactory
    {
        private readonly ISqlExpressionFactory _sqlExpressionFactory;
        public WebroxPostgresQueryTranslationPreprocessorFactory(
            QueryTranslationPreprocessorDependencies dependencies,
            RelationalQueryTranslationPreprocessorDependencies relationalDependencies,
            ISqlExpressionFactory sqlExpressionFactory,
            INpgsqlSingletonOptions npgsqlSingletonOptions
            )
            : base(dependencies, relationalDependencies, npgsqlSingletonOptions)
        {
            _sqlExpressionFactory = sqlExpressionFactory;
        }

        public override QueryTranslationPreprocessor Create(QueryCompilationContext queryCompilationContext)
        {
            return new WebroxPostgresQueryTranslationPreprocessor(Dependencies,queryCompilationContext, _sqlExpressionFactory);
        }
    }
}