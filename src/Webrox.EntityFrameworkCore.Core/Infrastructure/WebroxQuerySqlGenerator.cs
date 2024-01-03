using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Webrox.EntityFrameworkCore.Core.SqlExpressions;

namespace Webrox.EntityFrameworkCore.Core.Infrastructure
{
    public class WebroxQuerySqlGenerator
    {
        public Expression VisitWindowFunction(IRelationalCommandBuilder sql,
            WindowExpression windowExpression,
            Func<Expression?, Expression?> visit,
            Func<OrderingExpression, Expression> visitOrdering
            )
        {
            sql.Append(windowExpression.AggregateFunction).Append("(");

            if (windowExpression.ColumnExpression != null)
            {
                visit(windowExpression.ColumnExpression);
            }

            sql.Append(")").Append(" ").Append("OVER (");

            if (windowExpression.Partitions.Count != 0)
            {
                sql.Append("PARTITION BY ");

                for (var i = 0; i < windowExpression.Partitions.Count; i++)
                {
                    if (i != 0)
                        sql.Append(", ");

                    var partition = windowExpression.Partitions[i];
                    visit(partition);
                }
            }

            if (windowExpression.Orderings.Count != 0)
            {
                sql.Append(" ORDER BY ");

                for (var i = 0; i < windowExpression.Orderings.Count; i++)
                {
                    if (i != 0)
                        sql.Append(", ");

                    var ordering = windowExpression.Orderings[i];
                    visitOrdering(ordering);
                }
            }

            sql.Append(")");

            return windowExpression;
        }
    }
}
