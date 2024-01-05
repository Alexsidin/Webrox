using Microsoft.EntityFrameworkCore;
using Webrox.EntityFrameworkCore.Core.Models;

namespace Webrox.EntityFrameworkCore.MySql
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
        public static ulong RowNumber(this DbFunctions _,
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
        public static ulong RowNumber(this DbFunctions _,
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
        public static ulong Rank(this DbFunctions _,
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
        public static ulong Rank(this DbFunctions _,
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
        public static ulong DenseRank(this DbFunctions _,
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
        public static ulong DenseRank(this DbFunctions _,
                                       PartitionByClause partition,
                                       OrderByClause orderBy)
        {
            throw new InvalidOperationException(_ErrorLinq);
        }

        #endregion

        #region Sum

        // https://learn.microsoft.com/en-us/sql/t-sql/functions/sum-transact-sql?view=sql-server-ver16

        /// <summary>
        /// Sum without partitions
        /// </summary>
        /// <param name="_">DbFunctions</param>
        /// <param name="expression">an Expression</param>
        /// <param name="orderBy">order by using EF.Functions.OrderBy(...).ThenBy(...)</param>
        /// <returns></returns>
        public static decimal Sum(this DbFunctions _, sbyte expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Sum(this DbFunctions _, byte expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Sum(this DbFunctions _, short expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Sum(this DbFunctions _, ushort expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Sum(this DbFunctions _, int expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Sum(this DbFunctions _, uint expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Sum(this DbFunctions _, long expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Sum(this DbFunctions _, ulong expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Sum(this DbFunctions _, decimal expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static double Sum(this DbFunctions _, float expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static double Sum(this DbFunctions _, double expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }

        /// <summary>
        /// Sum with partitions
        /// </summary>
        /// <param name="_">DbFunctions</param>
        /// <param name="expression">an Expression</param>
        /// <param name="partition">partition by using EF.Functions.PartitionBy(...).ThenPartitionBy(...)</param>
        /// <param name="orderBy">order by using EF.Functions.OrderBy(...).ThenBy(...)</param>
        /// <returns></returns>
        public static decimal Sum(this DbFunctions _, sbyte expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Sum(this DbFunctions _, byte expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Sum(this DbFunctions _, short expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Sum(this DbFunctions _, ushort expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Sum(this DbFunctions _, int expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Sum(this DbFunctions _, uint expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Sum(this DbFunctions _, long expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Sum(this DbFunctions _, ulong expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Sum(this DbFunctions _, decimal expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static double Sum(this DbFunctions _, float expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static double Sum(this DbFunctions _, double expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }

        #endregion

        #region Average

        /// <summary>
        /// Avg without partitions
        /// </summary>
        /// <param name="_">DbFunctions</param>
        /// <param name="expression">an Expression</param>
        /// <param name="orderBy">order by using EF.Functions.OrderBy(...).ThenBy(...)</param>
        /// <returns></returns>
        public static decimal Average(this DbFunctions _, sbyte expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Average(this DbFunctions _, byte expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Average(this DbFunctions _, short expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Average(this DbFunctions _, ushort expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Average(this DbFunctions _, int expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Average(this DbFunctions _, uint expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Average(this DbFunctions _, long expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Average(this DbFunctions _, ulong expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Average(this DbFunctions _, decimal expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static double Average(this DbFunctions _, float expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static double Average(this DbFunctions _, double expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }

        /// <summary>
        /// Avg with partitions
        /// </summary>
        /// <param name="_">DbFunctions</param>
        /// <param name="expression">an Expression</param>
        /// <param name="partition">partition by using EF.Functions.PartitionBy(...).ThenPartitionBy(...)</param>
        /// <param name="orderBy">order by using EF.Functions.OrderBy(...).ThenBy(...)</param>
        /// <returns></returns>
        public static decimal Average(this DbFunctions _, sbyte expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Average(this DbFunctions _, byte expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Average(this DbFunctions _, short expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Average(this DbFunctions _, ushort expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Average(this DbFunctions _, int expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Average(this DbFunctions _, uint expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Average(this DbFunctions _, long expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Average(this DbFunctions _, ulong expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Average(this DbFunctions _, decimal expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static double Average(this DbFunctions _, float expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static double Average(this DbFunctions _, double expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }

        #endregion

        #region Min

        /// <summary>
        /// Min without partitions
        /// </summary>
        /// <param name="_">DbFunctions</param>
        /// <param name="expression">an Expression</param>
        /// <param name="orderBy">order by using EF.Functions.OrderBy(...).ThenBy(...)</param>
        /// <returns></returns>
        public static int Min(this DbFunctions _, sbyte expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static uint Min(this DbFunctions _, byte expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static int Min(this DbFunctions _, short expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static uint Min(this DbFunctions _, ushort expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static long Min(this DbFunctions _, int expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static ulong Min(this DbFunctions _, uint expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static long Min(this DbFunctions _, long expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static ulong Min(this DbFunctions _, ulong expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Min(this DbFunctions _, decimal expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static double Min(this DbFunctions _, float expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static double Min(this DbFunctions _, double expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }

        /// <summary>
        /// Min with partitions
        /// </summary>
        /// <param name="_">DbFunctions</param>
        /// <param name="expression">an Expression</param>
        /// <param name="partition">partition by using EF.Functions.PartitionBy(...).ThenPartitionBy(...)</param>
        /// <param name="orderBy">order by using EF.Functions.OrderBy(...).ThenBy(...)</param>
        /// <returns></returns>
        public static int Min(this DbFunctions _, sbyte expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static uint Min(this DbFunctions _, byte expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static int Min(this DbFunctions _, short expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static uint Min(this DbFunctions _, ushort expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static long Min(this DbFunctions _, int expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static ulong Min(this DbFunctions _, uint expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static long Min(this DbFunctions _, long expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static ulong Min(this DbFunctions _, ulong expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Min(this DbFunctions _, decimal expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static double Min(this DbFunctions _, float expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static double Min(this DbFunctions _, double expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }

        #endregion

        #region Max

        /// <summary>
        /// Max without partitions
        /// </summary>
        /// <param name="_">DbFunctions</param>
        /// <param name="expression">an Expression</param>
        /// <param name="orderBy">order by using EF.Functions.OrderBy(...).ThenBy(...)</param>
        /// <returns></returns>
        public static int Max(this DbFunctions _, sbyte expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static uint Max(this DbFunctions _, byte expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static int Max(this DbFunctions _, short expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static uint Max(this DbFunctions _, ushort expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static long Max(this DbFunctions _, int expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static ulong Max(this DbFunctions _, uint expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static long Max(this DbFunctions _, long expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static ulong Max(this DbFunctions _, ulong expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Max(this DbFunctions _, decimal expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static double Max(this DbFunctions _, float expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static double Max(this DbFunctions _, double expression, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }


        /// <summary>
        /// Max with partitions
        /// </summary>
        /// <param name="_">DbFunctions</param>
        /// <param name="expression">an Expression</param>
        /// <param name="partition">partition by using EF.Functions.PartitionBy(...).ThenPartitionBy(...)</param>
        /// <param name="orderBy">order by using EF.Functions.OrderBy(...).ThenBy(...)</param>
        /// <returns></returns>
        public static int Max(this DbFunctions _, sbyte expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static uint Max(this DbFunctions _, byte expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static int Max(this DbFunctions _, short expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static uint Max(this DbFunctions _, ushort expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static long Max(this DbFunctions _, int expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static ulong Max(this DbFunctions _, uint expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static long Max(this DbFunctions _, long expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static ulong Max(this DbFunctions _, ulong expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static decimal Max(this DbFunctions _, decimal expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static double Max(this DbFunctions _, float expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }
        public static double Max(this DbFunctions _, double expression, PartitionByClause partition, OrderByClause orderBy) { throw new InvalidOperationException(_ErrorLinq); }

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
