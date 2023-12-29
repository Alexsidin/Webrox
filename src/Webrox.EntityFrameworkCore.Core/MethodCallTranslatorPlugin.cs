using Microsoft.EntityFrameworkCore.Query;

namespace Webrox.EntityFrameworkCore.Core
{
    public class MethodCallTranslatorPlugin : IMethodCallTranslatorPlugin
    {
        protected readonly ISqlExpressionFactory _sqlExpressionFactory;

        public IEnumerable<IMethodCallTranslator> Translators { get; }

        public MethodCallTranslatorPlugin(ISqlExpressionFactory sqlExpressionFactory)
        {
            _sqlExpressionFactory = sqlExpressionFactory;
            Translators = new List<IMethodCallTranslator>
                    {
                        new RowNumberTranslator(_sqlExpressionFactory)
                    };
        }
    }
}
