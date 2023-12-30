using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage;

namespace Webrox.EntityFrameworkCore.Core.SqlExpressions
{
    //
    // Summary:
    //     An expression that represents a Window (ROW_NUMBER,RANK,DENSE_RANK,etc) operation in a SQL tree.
    //     This type is typically used by database providers (and other extensions). It
    //     is generally not used in application code.
    public class WindowExpression : SqlExpression
    {
        //
        // Summary:
        //  A function name like ROW_NUMBER, RANK, DENSE_RANK etc.
        public string AggregateFunction { get; set; }

        //
        // Summary:
        //  An expression (for SUM,MAX,MIN,AVG window functions).
        public SqlExpression ColumnExpression { get; set; }

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
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }
            
            bool flag = false;

            var expr = new SqlFunctionExpression(AggregateFunction,
                true, typeof(long), RelationalTypeMapping.NullMapping);
                
            var visitedAggregateFunction = (SqlExpression)visitor.Visit(expr);

            List<SqlExpression> list = new List<SqlExpression>();
            foreach (SqlExpression partition in Partitions)
            {
                SqlExpression sqlExpression = (SqlExpression)visitor.Visit(partition);
                flag = flag || sqlExpression != partition;
                list.Add(sqlExpression);
            }

            List<OrderingExpression> list2 = new List<OrderingExpression>();
            foreach (OrderingExpression ordering in Orderings)
            {
                OrderingExpression orderingExpression = (OrderingExpression)visitor.Visit(ordering);
                flag = flag || orderingExpression != ordering;
                list2.Add(orderingExpression);
            }

            if (!flag)
            {
                return this;
            }

            return new WindowExpression(AggregateFunction, ColumnExpression, list, list2, TypeMapping);
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
        public virtual WindowExpression Update(IReadOnlyList<SqlExpression> partitions,
            IReadOnlyList<OrderingExpression> orderings)
        {
            if (orderings == null)
            {
                throw new ArgumentNullException(nameof(orderings));
            }

            if (!((Partitions == null) ? (partitions == null) : Partitions.SequenceEqual(partitions)) || !Orderings.SequenceEqual(orderings))
            {
                return new WindowExpression(AggregateFunction, ColumnExpression, partitions, orderings, TypeMapping);
            }

            return this;
        }

        protected override void Print(ExpressionPrinter expressionPrinter)
        {
            if (expressionPrinter == null)
            {
                throw new ArgumentNullException(nameof(expressionPrinter));
            }

            expressionPrinter.Append($"{AggregateFunction}(");
            if (ColumnExpression != null)
            {
                expressionPrinter.Visit(ColumnExpression);
            }
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

        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                if (this != obj)
                {
                    WindowExpression WindowExpression = obj as WindowExpression;
                    if (WindowExpression != null)
                    {
                        return Equals(WindowExpression);
                    }

                    return false;
                }

                return true;
            }

            return false;
        }

        private bool Equals(WindowExpression WindowExpression)
        {
            if (base.Equals((object)WindowExpression) && ((Partitions == null) ? (WindowExpression.Partitions == null) : Partitions.SequenceEqual(WindowExpression.Partitions)))
            {
                return Orderings.SequenceEqual(WindowExpression.Orderings);
            }

            return false;
        }

        public override int GetHashCode()
        {
            HashCode hashCode = default(HashCode);
            hashCode.Add(base.GetHashCode());
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