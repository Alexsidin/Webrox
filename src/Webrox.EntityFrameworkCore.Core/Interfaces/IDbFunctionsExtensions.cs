using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webrox.EntityFrameworkCore.Core.Models;

namespace Webrox.EntityFrameworkCore.Core.Interfaces
{
    interface IDbFunctionsExtensions
    {
        ulong RowNumber();
        ulong Rank();
        ulong DenseRank();
        object Sum<T>();
        decimal Average<T>();
        long Min<T>();
        long Max<T>();
        ulong NTile<T>();
        public PartitionByClause PartitionBy<T>();
        public PartitionByClause ThenPartitionBy<T>();
        public OrderByClause OrderBy<T>();
        public OrderByClause OrderByDescending<T>();
        public OrderByClause ThenBy<T>();
        public OrderByClause ThenByDescending<T>();
    }
}
