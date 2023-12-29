using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Webrox.EntityFrameworkCore.Core.Expressions
{
    internal sealed class PartitionByExpression : SqlExpression
    {
        /// <summary>
        /// Partitions.
        /// </summary>
        public IReadOnlyList<SqlExpression> Partitions { get; }

        /// <inheritdoc />
        public PartitionByExpression(IReadOnlyList<SqlExpression> partitions)
           : base(typeof(PartitionByClause), RelationalTypeMapping.NullMapping)
        {
            Partitions = partitions ?? throw new ArgumentNullException(nameof(partitions));
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
            var visited = visitor.VisitExpressions(Partitions);

            return ReferenceEquals(visited, Partitions) ? this : new PartitionByExpression(visited);
        }

        /// <inheritdoc />
        protected override void Print(ExpressionPrinter expressionPrinter)
        {
            ArgumentNullException.ThrowIfNull(expressionPrinter);

            expressionPrinter.VisitCollection(Partitions);
        }

        /// <summary>
        /// Adds provided <paramref name="partitions"/> to existing <see cref="Partitions"/> and returns a new <see cref="PartitionByExpression"/>.
        /// </summary>
        /// <param name="partitions">Partitions to add.</param>
        /// <returns>New instance of <see cref="PartitionByExpression"/>.</returns>
        public PartitionByExpression AddColumns(IEnumerable<SqlExpression> partitions)
        {
            ArgumentNullException.ThrowIfNull(partitions);

            return new PartitionByExpression(Partitions.Concat(partitions).ToList());
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj != null && (ReferenceEquals(this, obj) || Equals(obj as PartitionByExpression));
        }

        private bool Equals(PartitionByExpression? expression)
        {
            return base.Equals(expression) && Partitions.SequenceEqual(expression.Partitions);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(base.GetHashCode());

            for (var i = 0; i < Partitions.Count; i++)
            {
                hash.Add(Partitions[i]);
            }

            return hash.ToHashCode();
        }

    }
}