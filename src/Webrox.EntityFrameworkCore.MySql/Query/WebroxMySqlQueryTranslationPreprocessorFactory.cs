using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Webrox.EntityFrameworkCore.MySql.Query
{
    /// <summary>
    /// Factory for creation of the <see cref="WebroxSqliteRelationalSqlTranslatingExpressionVisitor"/>.
    /// </summary>
    public sealed class WebroxMySqlQueryTranslationPreprocessorFactory :
        QueryTranslationPreprocessorFactory
    {
        private readonly ISqlExpressionFactory _sqlExpressionFactory;
        public WebroxMySqlQueryTranslationPreprocessorFactory(QueryTranslationPreprocessorDependencies dependencies, ISqlExpressionFactory sqlExpressionFactory)
            : base(dependencies)
        {
            _sqlExpressionFactory = sqlExpressionFactory;
        }

        public override QueryTranslationPreprocessor Create(QueryCompilationContext queryCompilationContext)
        {
            return new WebroxMySqlQueryTranslationPreprocessor(Dependencies,queryCompilationContext, _sqlExpressionFactory);
        }
    }
}