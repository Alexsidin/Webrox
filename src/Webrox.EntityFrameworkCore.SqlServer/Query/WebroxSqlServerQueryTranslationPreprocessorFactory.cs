using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Webrox.EntityFrameworkCore.SqlServer.Query
{
    /// <summary>
    /// Factory for creation of the <see cref="WebroxSqliteRelationalSqlTranslatingExpressionVisitor"/>.
    /// </summary>
    public sealed class WebroxSqlServerQueryTranslationPreprocessorFactory :
        QueryTranslationPreprocessorFactory
    {
        private readonly ISqlExpressionFactory _sqlExpressionFactory;
        public WebroxSqlServerQueryTranslationPreprocessorFactory(QueryTranslationPreprocessorDependencies dependencies, ISqlExpressionFactory sqlExpressionFactory)
            : base(dependencies)
        {
            _sqlExpressionFactory = sqlExpressionFactory;
        }

        public override QueryTranslationPreprocessor Create(QueryCompilationContext queryCompilationContext)
        {
            return new WebroxSqlServerQueryTranslationPreprocessor(Dependencies,queryCompilationContext, _sqlExpressionFactory);
        }
    }
}