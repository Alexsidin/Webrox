using Microsoft.EntityFrameworkCore.Query;
using Webrox.EntityFrameworkCore.Core.Translators;

namespace Webrox.EntityFrameworkCore.Core
{
    /// <summary>
    /// Webrox MethodCallTranslatorPlugin 
    /// </summary>
    public class WebroxMethodCallTranslatorPlugin : IMethodCallTranslatorPlugin
    {
        private readonly ISqlExpressionFactory _sqlExpressionFactory;

        /// <inheritdoc/>
        public IEnumerable<IMethodCallTranslator> Translators { get; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="sqlExpressionFactory">sql ExpressionFactory</param>
        public WebroxMethodCallTranslatorPlugin(ISqlExpressionFactory sqlExpressionFactory)
        {
            _sqlExpressionFactory = sqlExpressionFactory;
            Translators = new List<IMethodCallTranslator>
                    {
                        new RowNumberTranslator(_sqlExpressionFactory)
                    };
        }
    }
}
