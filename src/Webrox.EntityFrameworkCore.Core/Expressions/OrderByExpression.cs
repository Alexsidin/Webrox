using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Webrox.EntityFrameworkCore.Core.Expressions
{
    internal sealed class OrderByExpression : SqlExpression
    {
        /// <summary>
        /// Orderings.
        /// </summary>
        public IReadOnlyList<OrderingExpression> Orderings { get; }

        /// <inheritdoc />
        public OrderByExpression(IReadOnlyList<OrderingExpression> orderings)
           : base(typeof(OrderByClause), RelationalTypeMapping.NullMapping)
        {
            Orderings = orderings ?? throw new ArgumentNullException(nameof(orderings));
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
            var visited = visitor.VisitExpressions(Orderings);

            return ReferenceEquals(visited, Orderings) ? this : new OrderByExpression(visited);
        }

        /// <inheritdoc />
        protected override void Print(ExpressionPrinter expressionPrinter)
        {
            ArgumentNullException.ThrowIfNull(expressionPrinter);

            expressionPrinter.VisitCollection(Orderings);
        }

        /// <summary>
        /// Adds provided <paramref name="orderings"/> to existing <see cref="Orderings"/> and returns a new <see cref="OrderByExpression"/>.
        /// </summary>
        /// <param name="orderings">Orderings to add.</param>
        /// <returns>New instance of <see cref="OrderByExpression"/>.</returns>
        public OrderByExpression AddColumns(IEnumerable<OrderingExpression> orderings)
        {
            ArgumentNullException.ThrowIfNull(orderings);

            return new OrderByExpression(Orderings.Concat(orderings).ToList());
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj != null && (ReferenceEquals(this, obj) || Equals(obj as OrderByExpression));
        }

        private bool Equals(OrderByExpression? expression)
        {
            return base.Equals(expression) && Orderings.SequenceEqual(expression.Orderings);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(base.GetHashCode());

            foreach(var ordering in Orderings)
            {
                hash.Add(ordering);
            }

            return hash.ToHashCode();
        }
    }
}