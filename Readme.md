# Webrox.EntityFrameworkCore

Use the [repo on GitHub](https://github.com/Poppyto/Webrox.EntityFrameworkCore) to create issues and feature requests.

## Features

Implement various Linq functions with Entity Framework Core 

| SQL    | Linq |
| ------: | ------- |
| `ROW_NUMBER` |   [`EF.Functions.RowNumber`](#rownumber-syntax) | 
| `ROW_NUMBER`|   [`Select((entity, index)=>...)`](#linq-select-syntax)  | 
| `RANK` |   [`EF.Functions.Rank`](#rank-syntax)| 
| `DENSE_RANK` |   [`EF.Functions.DenseRank`](#denserank-syntax)| 
| `AVG` |   [`EF.Functions.Average`](#average-syntax)| 
| `SUM` |   [`EF.Functions.Sum`](#sum-syntax)| 
| `MIN` |   [`EF.Functions.Min`](#min-syntax)| 
| `MAX` |   [`EF.Functions.Max`](#max-syntax)| 


Supported providers : 

| Provider | Nuget Package | .net 6 | .net 7 | .net 8 |
| ------: | ------- | ------- | ----- | ---- |
| SQL Server | Microsoft.EntityFrameworkCore.SqlServer | :heavy_check_mark:  EF6 | :heavy_check_mark: + EF7 | :heavy_check_mark: + EF8 |
| SQLite | Microsoft.EntityFrameworkCore.Sqlite | :heavy_check_mark: + EF6 | :heavy_check_mark: + EF7 | :heavy_check_mark: + EF8 |
| MySQL | MySql.EntityFrameworkCore | :heavy_check_mark: + EF6 | :heavy_check_mark: + EF7 | :x: (waiting for Oracle EF8 support) |
| PostgreSQL | Npgsql.EntityFrameworkCore.PostgreSQL | :heavy_check_mark: + EF6 | :heavy_check_mark: + EF7 | :heavy_check_mark: + EF8 |

## Setup
### SQLServer
```
using Webrox.EntityFrameworkCore.SqlServer;

protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    // SQL Server
    optionsBuilder.UseSqlServer("connectionstring", opts |  
    {
        opts.AddWebroxFeatures();
    });
}
```
### MySQL

```
using Webrox.EntityFrameworkCore.MySql;

protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseMySQL("connectionstring", opts |  
    {
        opts.AddWebroxFeatures();
    });
}
```

### SQLite

```
using Webrox.EntityFrameworkCore.Sqlite;

protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseSqlite("connectionstring", opts |  
    {
        opts.AddWebroxFeatures();
    });
}
```

### PostgreSQL

```
using Webrox.EntityFrameworkCore.Postgres;

protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseNpgsql("connectionstring", opts |  
    {
        opts.AddWebroxFeatures();
    });
}
```


# Functions Usage


## RowNumber Syntax

### EF Functions

```
using Webrox.EntityFrameworkCore.[Provider];

// simple RowNumber + OrderBy
context.Table1.Select(t => new
{
    Id = t.Id,
    RowNumber = EF.Functions.RowNumber(EF.Functions.OrderBy(t.Id))
}

// complex RowNumber + PartitionBy + OrderBy
context.Table1.Select(t =>  new
{
    Id = t.Id,
    RowNumber = EF.Functions.RowNumber(
            EF.Functions.PartitionBy(t.GroupId)
                        .ThenPartitionBy(t.SubGroupId),
            EF.Functions.OrderByDescending(t.Id)
                        .ThenBy(t.SubId)
            )
}
```

### Linq Select syntax

You can also use the function Select((e, index)=>{}) to access RowNumber. 

It will not be ordered or partitionned and the parameter "index" is int.

```
context.Table1.Select((t, index) =>  new
{
    Id = t.Id,
    RowNumber = index
}
```


## Rank Syntax


```
using Webrox.EntityFrameworkCore.[Provider];

// simple Rank + OrderBy
context.Table1.Select(t => new
{
    Id = t.Id,
    Rank = EF.Functions.Rank(EF.Functions.OrderBy(t.Id))
}

// complex Rank + PartitionBy + OrderBy
context.Table1.Select(t =>  new
{
    Id = t.Id,
    Rank = EF.Functions.Rank(
            EF.Functions.PartitionBy(t.GroupId)
                        .ThenPartitionBy(t.SubGroupId),
            EF.Functions.OrderByDescending(t.Id)
                        .ThenBy(t.SubId)
            )
}
```

## DenseRank Syntax


```
using Webrox.EntityFrameworkCore.[Provider];

// simple DenseRank + OrderBy
context.Table1.Select(t => new
{
    Id = t.Id,
    DenseRank = EF.Functions.DenseRank(EF.Functions.OrderBy(t.Id))
}

// complex DenseRank + PartitionBy + OrderBy
context.Table1.Select(t =>  new
{
    Id = t.Id,
    DenseRank = EF.Functions.DenseRank(
            EF.Functions.PartitionBy(t.GroupId)
                        .ThenPartitionBy(t.SubGroupId),
            EF.Functions.OrderByDescending(t.Id)
                        .ThenBy(t.SubId)
            )
}
```

## Average Syntax


```
using Webrox.EntityFrameworkCore.[Provider];

// simple Average + OrderBy
context.Table1.Select(t => new
{
    Id = t.Id,
    Average = EF.Functions.Average(EF.Functions.OrderBy(t.Id))
}

// complex Average + PartitionBy + OrderBy
context.Table1.Select(t =>  new
{
    Id = t.Id,
    Average = EF.Functions.Average(
            EF.Functions.PartitionBy(t.GroupId)
                        .ThenPartitionBy(t.SubGroupId),
            EF.Functions.OrderByDescending(t.Id)
                        .ThenBy(t.SubId)
            )
}
```

## Sum Syntax


```
using Webrox.EntityFrameworkCore.[Provider];

// simple Sum + OrderBy
context.Table1.Select(t => new
{
    Id = t.Id,
    Sum = EF.Functions.Sum(EF.Functions.OrderBy(t.Id))
}

// complex Sum + PartitionBy + OrderBy
context.Table1.Select(t =>  new
{
    Id = t.Id,
    Sum = EF.Functions.Sum(
            EF.Functions.PartitionBy(t.GroupId)
                        .ThenPartitionBy(t.SubGroupId),
            EF.Functions.OrderByDescending(t.Id)
                        .ThenBy(t.SubId)
            )
}
```

## Min Syntax


```
using Webrox.EntityFrameworkCore.[Provider];

// simple Min + OrderBy
context.Table1.Select(t => new
{
    Id = t.Id,
    Min = EF.Functions.Min(EF.Functions.OrderBy(t.Id))
}

// complex Min + PartitionBy + OrderBy
context.Table1.Select(t =>  new
{
    Id = t.Id,
    Min = EF.Functions.Min(
            EF.Functions.PartitionBy(t.GroupId)
                        .ThenPartitionBy(t.SubGroupId),
            EF.Functions.OrderByDescending(t.Id)
                        .ThenBy(t.SubId)
            )
}
```

## Max Syntax


```
using Webrox.EntityFrameworkCore.[Provider];

// simple Max + OrderBy
context.Table1.Select(t => new
{
    Id = t.Id,
    Max = EF.Functions.Max(EF.Functions.OrderBy(t.Id))
}

// complex Max + PartitionBy + OrderBy
context.Table1.Select(t =>  new
{
    Id = t.Id,
    Max = EF.Functions.Max(
            EF.Functions.PartitionBy(t.GroupId)
                        .ThenPartitionBy(t.SubGroupId),
            EF.Functions.OrderByDescending(t.Id)
                        .ThenBy(t.SubId)
            )
}
```
