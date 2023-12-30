using Microsoft.EntityFrameworkCore;
using Webrox.Models;

namespace Webrox.EntityFrameworkCore.Core
{
    /// <summary>
    /// Extensions for EF.Functions
    /// </summary>
    public static class DbFunctionsExtensions
    {
        #region RowNumber

        /// <summary>
        /// RowNumber without partitions
        /// </summary>
        /// <param name="_">DbFunctions</param>
        /// <param name="orderBy">order by using EF.Functions.OrderBy(...).ThenBy(...)</param>
        /// <returns></returns>
        public static long RowNumber(this DbFunctions _,
                                       OrderByClause orderBy)
        {
            throw new InvalidOperationException("...");
        }

        /// <summary>
        /// RowNumber with partitions
        /// </summary>
        /// <param name="_">DbFunctions</param>
        /// <param name="partition">partition by using EF.Functions.PartitionBy(...).ThenPartitionBy(...)</param>
        /// <param name="orderBy">order by using EF.Functions.OrderBy(...).ThenBy(...)</param>
        /// <returns></returns>
        public static long RowNumber(this DbFunctions _,
                                       PartitionByClause partition,
                                       OrderByClause orderBy)
        {
            throw new InvalidOperationException("...");
        }

        #endregion

        #region PartitionBy

        /// <summary>
        /// Partition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        /// <param name="column">Column</param>
        /// <returns><see cref="PartitionByClause "/></returns>
        public static PartitionByClause PartitionBy<T>(this DbFunctions _, T column)
        {
            throw new InvalidOperationException("This method is for use with Entity Framework Core only and has no in-memory implementation.");
        }
        /// <summary>
        /// Chained Partition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        /// <param name="column">Column</param>
        /// <returns><see cref="PartitionByClause "/></returns>
        public static PartitionByClause ThenPartitionBy<T>(this PartitionByClause _, T column)
        {
            throw new InvalidOperationException("This method is for use with Entity Framework Core only and has no in-memory implementation.");
        }

        #endregion

        #region OrderBy

        /// <summary>
        /// OrderBy ASC
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        /// <param name="column">Column</param>
        /// <returns><see cref="OrderByClause "/></returns>
        public static OrderByClause OrderBy<T>(this DbFunctions _, T column)
        {
            throw new InvalidOperationException("This method is for use with Entity Framework Core only and has no in-memory implementation.");
        }

        /// <summary>
        /// OrderBy DESC
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        /// <param name="column">Column</param>
        /// <returns><see cref="OrderByClause "/></returns>
        public static OrderByClause OrderByDescending<T>(this DbFunctions _, T column)
        {
            throw new InvalidOperationException("This method is for use with Entity Framework Core only and has no in-memory implementation.");
        }

        /// <summary>
        /// Subsequent OrderBy ASC
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        /// <param name="column">Column</param>
        /// <returns><see cref="OrderByClause "/></returns>
        public static OrderByClause ThenBy<T>(this OrderByClause _, T column)
        {
            throw new InvalidOperationException("This method is for use with Entity Framework Core only and has no in-memory implementation.");
        }

        /// <summary>
        /// Subsequent OrderBy DESC
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        /// <param name="column">Column</param>
        /// <returns><see cref="OrderByClause "/></returns>
        public static OrderByClause ThenByDescending<T>(this OrderByClause _, T column)
        {
            throw new InvalidOperationException("This method is for use with Entity Framework Core only and has no in-memory implementation.");
        }

        #endregion
    }
}
