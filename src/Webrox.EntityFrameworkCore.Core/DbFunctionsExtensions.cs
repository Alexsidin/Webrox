using Microsoft.EntityFrameworkCore;
using Webrox.Models;

namespace Webrox.EntityFrameworkCore.Core
{
    /// <summary>
    /// Extensions for EF.Functions
    /// </summary>
    public static class DbFunctionsExtensions
    {
        const string _ErrorLinq = "This method is for use with Entity Framework Core only and has no in-memory implementation.";

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
            throw new InvalidOperationException(_ErrorLinq);
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
            throw new InvalidOperationException(_ErrorLinq);
        }

        #endregion

        #region Rank

        /// <summary>
        /// Rank without partitions
        /// </summary>
        /// <param name="_">DbFunctions</param>
        /// <param name="orderBy">order by using EF.Functions.OrderBy(...).ThenBy(...)</param>
        /// <returns></returns>
        public static long Rank(this DbFunctions _,
                                       OrderByClause orderBy)
        {
            throw new InvalidOperationException(_ErrorLinq);
        }

        /// <summary>
        /// Rank with partitions
        /// </summary>
        /// <param name="_">DbFunctions</param>
        /// <param name="partition">partition by using EF.Functions.PartitionBy(...).ThenPartitionBy(...)</param>
        /// <param name="orderBy">order by using EF.Functions.OrderBy(...).ThenBy(...)</param>
        /// <returns></returns>
        public static long Rank(this DbFunctions _,
                                       PartitionByClause partition,
                                       OrderByClause orderBy)
        {
            throw new InvalidOperationException(_ErrorLinq);
        }

        #endregion

        #region DenseRank

        /// <summary>
        /// DenseRank without partitions
        /// </summary>
        /// <param name="_">DbFunctions</param>
        /// <param name="orderBy">order by using EF.Functions.OrderBy(...).ThenBy(...)</param>
        /// <returns></returns>
        public static long DenseRank(this DbFunctions _,
                                       OrderByClause orderBy)
        {
            throw new InvalidOperationException(_ErrorLinq);
        }

        /// <summary>
        /// DenseRank with partitions
        /// </summary>
        /// <param name="_">DbFunctions</param>
        /// <param name="partition">partition by using EF.Functions.PartitionBy(...).ThenPartitionBy(...)</param>
        /// <param name="orderBy">order by using EF.Functions.OrderBy(...).ThenBy(...)</param>
        /// <returns></returns>
        public static long DenseRank(this DbFunctions _,
                                       PartitionByClause partition,
                                       OrderByClause orderBy)
        {
            throw new InvalidOperationException(_ErrorLinq);
        }

        #endregion

        #region Sum

        /// <summary>
        /// Sum without partitions
        /// </summary>
        /// <param name="_">DbFunctions</param>
        /// <param name="expression">an Expression</param>
        /// <param name="orderBy">order by using EF.Functions.OrderBy(...).ThenBy(...)</param>
        /// <returns></returns>
        public static long Sum<T>(this DbFunctions _,
                                  T expression,
                                  OrderByClause orderBy)
        {
            throw new InvalidOperationException(_ErrorLinq);
        }

        /// <summary>
        /// Sum with partitions
        /// </summary>
        /// <param name="_">DbFunctions</param>
        /// <param name="expression">an Expression</param>
        /// <param name="partition">partition by using EF.Functions.PartitionBy(...).ThenPartitionBy(...)</param>
        /// <param name="orderBy">order by using EF.Functions.OrderBy(...).ThenBy(...)</param>
        /// <returns></returns>
        public static long Sum<T>(this DbFunctions _,
                                       T expression,
                                       PartitionByClause partition,
                                       OrderByClause orderBy)
        {
            throw new InvalidOperationException(_ErrorLinq);
        }

        #endregion

        #region Average

        /// <summary>
        /// Avg without partitions
        /// </summary>
        /// <param name="_">DbFunctions</param>
        /// <param name="expression">an Expression</param>
        /// <param name="orderBy">order by using EF.Functions.OrderBy(...).ThenBy(...)</param>
        /// <returns></returns>
        public static long Average<T>(this DbFunctions _,
                                  T expression,
                                  OrderByClause orderBy)
        {
            throw new InvalidOperationException(_ErrorLinq);
        }

        /// <summary>
        /// Avg with partitions
        /// </summary>
        /// <param name="_">DbFunctions</param>
        /// <param name="expression">an Expression</param>
        /// <param name="partition">partition by using EF.Functions.PartitionBy(...).ThenPartitionBy(...)</param>
        /// <param name="orderBy">order by using EF.Functions.OrderBy(...).ThenBy(...)</param>
        /// <returns></returns>
        public static long Average<T>(this DbFunctions _,
                                       T expression,
                                       PartitionByClause partition,
                                       OrderByClause orderBy)
        {
            throw new InvalidOperationException(_ErrorLinq);
        }

        #endregion

        #region Min

        /// <summary>
        /// Min without partitions
        /// </summary>
        /// <param name="_">DbFunctions</param>
        /// <param name="expression">an Expression</param>
        /// <param name="orderBy">order by using EF.Functions.OrderBy(...).ThenBy(...)</param>
        /// <returns></returns>
        public static long Min<T>(this DbFunctions _,
                                  T expression,
                                  OrderByClause orderBy)
        {
            throw new InvalidOperationException(_ErrorLinq);
        }

        /// <summary>
        /// Min with partitions
        /// </summary>
        /// <param name="_">DbFunctions</param>
        /// <param name="expression">an Expression</param>
        /// <param name="partition">partition by using EF.Functions.PartitionBy(...).ThenPartitionBy(...)</param>
        /// <param name="orderBy">order by using EF.Functions.OrderBy(...).ThenBy(...)</param>
        /// <returns></returns>
        public static long Min<T>(this DbFunctions _,
                                       T expression,
                                       PartitionByClause partition,
                                       OrderByClause orderBy)
        {
            throw new InvalidOperationException(_ErrorLinq);
        }

        #endregion

        #region Sum

        /// <summary>
        /// Max without partitions
        /// </summary>
        /// <param name="_">DbFunctions</param>
        /// <param name="expression">an Expression</param>
        /// <param name="orderBy">order by using EF.Functions.OrderBy(...).ThenBy(...)</param>
        /// <returns></returns>
        public static long Max<T>(this DbFunctions _,
                                  T expression,
                                  OrderByClause orderBy)
        {
            throw new InvalidOperationException(_ErrorLinq);
        }

        /// <summary>
        /// Max with partitions
        /// </summary>
        /// <param name="_">DbFunctions</param>
        /// <param name="expression">an Expression</param>
        /// <param name="partition">partition by using EF.Functions.PartitionBy(...).ThenPartitionBy(...)</param>
        /// <param name="orderBy">order by using EF.Functions.OrderBy(...).ThenBy(...)</param>
        /// <returns></returns>
        public static long Max<T>(this DbFunctions _,
                                       T expression,
                                       PartitionByClause partition,
                                       OrderByClause orderBy)
        {
            throw new InvalidOperationException(_ErrorLinq);
        }

        #endregion

        #region Sum

        /// <summary>
        /// NTile without partitions
        /// </summary>
        /// <param name="_">DbFunctions</param>
        /// <param name="expression">an Expression</param>
        /// <param name="orderBy">order by using EF.Functions.OrderBy(...).ThenBy(...)</param>
        /// <returns></returns>
        public static long NTile<T>(this DbFunctions _,
                                  T expression,
                                  OrderByClause orderBy)  
        {
            throw new InvalidOperationException(_ErrorLinq);
        }

        /// <summary>
        /// NTile with partitions
        /// </summary>
        /// <param name="_">DbFunctions</param>
        /// <param name="expression">an Expression</param>
        /// <param name="partition">partition by using EF.Functions.PartitionBy(...).ThenPartitionBy(...)</param>
        /// <param name="orderBy">order by using EF.Functions.OrderBy(...).ThenBy(...)</param>
        /// <returns></returns>
        public static long NTile<T>(this DbFunctions _,
                                       T expression,
                                       PartitionByClause partition,
                                       OrderByClause orderBy)
        {
            throw new InvalidOperationException(_ErrorLinq);
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
            throw new InvalidOperationException(_ErrorLinq);
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
            throw new InvalidOperationException(_ErrorLinq);
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
            throw new InvalidOperationException(_ErrorLinq);
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
            throw new InvalidOperationException(_ErrorLinq);
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
            throw new InvalidOperationException(_ErrorLinq);
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
            throw new InvalidOperationException(_ErrorLinq);
        }

        #endregion
    }
}
