using System.Linq.Expressions;

namespace Webrox.EntityFrameworkCore.Core.Expressions
{
    internal static class ExpressionsExtensions
    {
        /// <summary>
        /// Visits a collection of <paramref name="expressions"/> and returns new collection if it least one expression has been changed.
        /// Otherwise the provided <paramref name="expressions"/> are returned if there are no changes.
        /// </summary>
        /// <param name="visitor">Visitor to use.</param>
        /// <param name="expressions">Expressions to visit.</param>
        /// <returns>
        /// New collection with visited expressions if at least one visited expression has been changed; otherwise the provided <paramref name="expressions"/>.
        /// </returns>
        public static IReadOnlyList<T> VisitExpressions<T>(this ExpressionVisitor visitor, IReadOnlyList<T> expressions)
           where T : Expression
        {
            if (visitor == null) throw new ArgumentNullException(nameof(visitor));
            if (expressions == null) throw new ArgumentNullException(nameof(expressions));

            var visitedExpressions = new List<T>();
            var hasChanges = false;

            foreach (var expression in expressions)
            {
                var visitedExpression = (T)visitor.Visit(expression);
                visitedExpressions.Add(visitedExpression);
                hasChanges |= !ReferenceEquals(visitedExpression, expression);
            }

            return hasChanges ? visitedExpressions.AsReadOnly() : expressions;
        }
    }
}