using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Webrox.EntityFrameworkCore.Core.Expressions
{
    /// <summary>
    /// List of expressions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TBase"></typeparam>
    internal sealed class ListExpressions<T, TBase> : SqlExpression
         where T : Expression
    {
        /// <summary>
        /// Expressions.
        /// </summary>
        public IReadOnlyList<T> Expressions { get; }

        /// <inheritdoc />
        public ListExpressions(IReadOnlyList<T> expressions)
           : base(typeof(TBase), RelationalTypeMapping.NullMapping)
        {
            Expressions = expressions ?? throw new ArgumentNullException(nameof(expressions));
        }

        /// <inheritdoc />
        protected override Expression Accept(ExpressionVisitor visitor)
        {
            if (visitor is QuerySqlGenerator)
                throw new NotSupportedException("The window function contains some expressions not supported by the Entity Framework. One of the reason is the creation of new objects like: 'new { e.MyProperty, e.MyOtherProperty }'.");

            return base.Accept(visitor);
        }

        /// <inheritdoc />
        protected override Expression VisitChildren(ExpressionVisitor visitor)
        {
            var visited = visitor.VisitExpressions(Expressions);

            return ReferenceEquals(visited, Expressions) ? this : new ListExpressions<T, TBase>(visited);
        }

        /// <inheritdoc />
        protected override void Print(ExpressionPrinter expressionPrinter)
        {
            if (expressionPrinter == null) throw new ArgumentNullException(nameof(expressionPrinter));

            expressionPrinter.VisitCollection(Expressions);
        }

        /// <summary>
        /// Adds provided <paramref name="expressions"/> to existing <see cref="Expressions"/> and returns a new <see cref="ListExpressions{T, TBase}"/>.
        /// </summary>
        /// <param name="expressions">Expressions to add.</param>
        /// <returns>New instance of <see cref="ListExpressions{T, TBase}"/>.</returns>
        public ListExpressions<T, TBase> AddColumns(IEnumerable<T> expressions)
        {
            if (expressions == null) throw new ArgumentNullException(nameof(expressions));
            
            return new ListExpressions<T,TBase>(Expressions.Concat(expressions).ToList());
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj != null && (ReferenceEquals(this, obj) || Equals(obj as ListExpressions<T, TBase>));
        }

        private bool Equals(ListExpressions<T, TBase>? expression)
        {
            return base.Equals(expression) && Expressions.SequenceEqual(expression.Expressions);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(base.GetHashCode());

            foreach(var expression in Expressions)
            {
                hash.Add(expression);
            }

            return hash.ToHashCode();
        }
    }
}