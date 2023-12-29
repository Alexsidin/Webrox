using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Webrox.EntityFrameworkCore.Core.Expressions;

namespace Webrox.EntityFrameworkCore.Core
{
    public record RowNumberParams
    {
        public object OrderBy { get; set; }
    }
    public static class DbFunctionsExtensions
    {
        #region RowNumber

        public static long RowNumber(this DbFunctions _,
                                       OrderByClause orderBy)
        {
            throw new InvalidOperationException("...");
        }

        public static long RowNumber(this DbFunctions _,
                                       PartitionByClause partition,
                                       OrderByClause orderBy)
        {
            throw new InvalidOperationException("...");
        }

        #endregion

        #region partitionBy

        public static PartitionByClause PartitionBy<T>(this DbFunctions _, T column)
        {
            throw new InvalidOperationException("This method is for use with Entity Framework Core only and has no in-memory implementation.");
        }
        public static PartitionByClause ThenPartitionBy<T>(this PartitionByClause _, T column)
        {
            throw new InvalidOperationException("This method is for use with Entity Framework Core only and has no in-memory implementation.");
        }
        
        #endregion

        #region orderBy

        public static OrderByClause OrderBy<T>(this DbFunctions _, T column)
        {
            throw new InvalidOperationException("This method is for use with Entity Framework Core only and has no in-memory implementation.");
        }

        public static OrderByClause OrderByDescending<T>(this DbFunctions _, T column)
        {
            throw new InvalidOperationException("This method is for use with Entity Framework Core only and has no in-memory implementation.");
        }

        public static OrderByClause ThenBy<T>(this OrderByClause clause, T column)
        {
            throw new InvalidOperationException("This method is for use with Entity Framework Core only and has no in-memory implementation.");
        }

        public static OrderByClause ThenByDescending<T>(this OrderByClause clause, T column)
        {
            throw new InvalidOperationException("This method is for use with Entity Framework Core only and has no in-memory implementation.");
        }

        #endregion
    }
}
