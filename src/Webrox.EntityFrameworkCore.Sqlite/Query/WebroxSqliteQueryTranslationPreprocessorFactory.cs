using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Sqlite.Query.Internal;

namespace Webrox.EntityFrameworkCore.Sqlite.Query
{
    /// <summary>
    /// Factory for creation of the <see cref="WebroxSqliteRelationalSqlTranslatingExpressionVisitor"/>.
    /// </summary>
    public sealed class WebroxSqliteQueryTranslationPreprocessorFactory :
        QueryTranslationPreprocessorFactory
    {
        private readonly ISqlExpressionFactory _sqlExpressionFactory;
        public WebroxSqliteQueryTranslationPreprocessorFactory(QueryTranslationPreprocessorDependencies dependencies, ISqlExpressionFactory sqlExpressionFactory)
            : base(dependencies)
        {
            _sqlExpressionFactory = sqlExpressionFactory;
        }

        public override QueryTranslationPreprocessor Create(QueryCompilationContext queryCompilationContext)
        {
            return new WebroxSqliteQueryTranslationPreprocessor(Dependencies,queryCompilationContext, _sqlExpressionFactory);
        }
    }
}