using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage;
using Webrox.EntityFrameworkCore.Core.Interfaces;
using System.Collections.Concurrent;

namespace Webrox.EntityFrameworkCore.Core.SqlExpressions
{
    //
    // Summary:
    //     An expression that represents a Window (ROW_NUMBER,RANK,DENSE_RANK,etc) operation in a SQL tree.
    //     This type is typically used by database providers (and other extensions). It
    //     is generally not used in application code.
    public class WindowExpression : SqlExpression, INotNullableSqlExpression
    {
        //
        // Summary:
        //  A function name like ROW_NUMBER, RANK, DENSE_RANK etc.
        public string AggregateFunction { get; set; }

        //
        // Summary:
        //  An expression (for SUM,MAX,MIN,AVG window functions).
        public virtual SqlExpression ColumnExpression { get; set; }

        //
        // Summary:
        //     The list of expressions used in partitioning.
        public virtual IReadOnlyList<SqlExpression> Partitions { get; }

        //
        // Summary:
        //     The list of ordering expressions used to order inside the given partition.
        public virtual IReadOnlyList<OrderingExpression> Orderings { get; }

        //
        // Summary:
        //     Creates a new instance of the Microsoft.EntityFrameworkCore.Query.SqlExpressions.WindowExpression
        //     class.
        //
        // Parameters:
        //    
        //   partitions:
        //     A list expressions to partition by.
        //
        //   orderings:
        //     A list of ordering expressions to order by.
        //
        //   typeMapping:
        //     The Microsoft.EntityFrameworkCore.Storage.RelationalTypeMapping associated with
        //     the expression.
        public WindowExpression(string aggregateFunction,
                                SqlExpression? columnExpression,
                                IReadOnlyList<SqlExpression> partitions,
                                IReadOnlyList<OrderingExpression> orderings,
                                RelationalTypeMapping typeMapping)
            : base(typeof(long), typeMapping)
        {
            if (aggregateFunction == null)
            {
                throw new ArgumentNullException(nameof(aggregateFunction));
            }

            if (orderings == null)
            {
                throw new ArgumentNullException(nameof(orderings));
            }

            AggregateFunction = aggregateFunction;
            ColumnExpression = columnExpression;
            Partitions = partitions ?? Array.Empty<SqlExpression>();
            Orderings = orderings;
        }

        protected override Expression VisitChildren(ExpressionVisitor visitor)
        {
            var changed = false;
            var partitions = new List<SqlExpression>();

            var newColumn = (SqlExpression)visitor.Visit(ColumnExpression);
            changed |= newColumn != ColumnExpression;
            partitions.Add(newColumn);

            foreach (var partition in Partitions)
            {
                var newPartition = (SqlExpression)visitor.Visit(partition);
                changed |= newPartition != partition;
                partitions.Add(newPartition);
            }

            var orderings = new List<OrderingExpression>();
            foreach (var ordering in Orderings)
            {
                var newOrdering = (OrderingExpression)visitor.Visit(ordering);
                changed |= newOrdering != ordering;
                orderings.Add(newOrdering);
            }

            return changed
                ? new WindowExpression(AggregateFunction, ColumnExpression, partitions, orderings, TypeMapping)
                : this;
        }

        //
        // Summary:
        //     Creates a new expression that is like this one, but using the supplied children.
        //     If all of the children are the same, it will return this expression.
        //
        // Parameters:
        //   partitions:
        //     The Microsoft.EntityFrameworkCore.Query.SqlExpressions.WindowExpression.Partitions
        //     property of the result.
        //
        //   orderings:
        //     The Microsoft.EntityFrameworkCore.Query.SqlExpressions.WindowExpression.Orderings
        //     property of the result.
        //
        // Returns:
        //     This expression if no children changed, or an expression with the updated children.
        public virtual WindowExpression Update(
            SqlExpression columnExpression,
            IReadOnlyList<SqlExpression> partitions,
            IReadOnlyList<OrderingExpression> orderings)
        {
            if (orderings == null)
            {
                throw new ArgumentNullException(nameof(orderings));
            }

            if (!((Partitions == null) ? (partitions == null) : Partitions.SequenceEqual(partitions))
                || !Orderings.SequenceEqual(orderings)
                || !columnExpression.Equals(ColumnExpression))
            {
                return new WindowExpression(AggregateFunction, columnExpression, partitions, orderings, TypeMapping);
            }

            return this;
        }

        protected override void Print(ExpressionPrinter expressionPrinter)
        {
            expressionPrinter.Append($"{AggregateFunction}(");
            expressionPrinter.Visit(ColumnExpression);
            expressionPrinter.Append(") OVER(");
            if (Partitions.Any())
            {
                expressionPrinter.Append("PARTITION BY ");
                expressionPrinter.VisitCollection(Partitions);
                expressionPrinter.Append(" ");
            }

            expressionPrinter.Append("ORDER BY ");
            expressionPrinter.VisitCollection(Orderings);
            expressionPrinter.Append(")");
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        => obj != null
            && (ReferenceEquals(this, obj)
                || obj is WindowExpression windowExpression
                && Equals(windowExpression));

        private bool Equals(WindowExpression windowExpression)
       => base.Equals(windowExpression)
           && AggregateFunction?.Equals(windowExpression?.AggregateFunction) == true
           && ColumnExpression?.Equals(windowExpression?.ColumnExpression) == true
           && (Partitions == null ? windowExpression.Partitions == null : Partitions.SequenceEqual(windowExpression.Partitions))
           && Orderings.SequenceEqual(windowExpression.Orderings);

        /// <inheritdoc />
        public override int GetHashCode()
        {
            HashCode hashCode = default(HashCode);
            hashCode.Add(base.GetHashCode());
            hashCode.Add(AggregateFunction?.GetHashCode());
            hashCode.Add(ColumnExpression);
            foreach (SqlExpression partition in Partitions)
            {
                hashCode.Add(partition);
            }

            foreach (OrderingExpression ordering in Orderings)
            {
                hashCode.Add(ordering);
            }

            return hashCode.ToHashCode();
        }
    }
}