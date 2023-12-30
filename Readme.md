# Webrox.EntityFrameworkCore

Use the [repo on GitHub](https://github.com/Poppyto/Webrox.EntityFrameworkCore) to create issues and feature requests.

## Features

Implement `ROW_NUMBER` Linq Function (`EF.Functions.RowNumber`) with Entity Framework Core for :

- SQL Server
- MySQL
- SQLite
- PostgreSQL

Entity Framework Version Support :

- min 5.0 (.net standard 2.1)

## Setup
### SQLServer
```
using Webrox.EntityFrameworkCore.SqlServer;

protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    // SQL Server
    optionsBuilder.UseSqlServer("connectionstring", opts =>
    {
        opts.AddRowNumberSupport();
    });
}
```
### MySQL

```
using Webrox.EntityFrameworkCore.MySql;

protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseMySQL("connectionstring", opts =>
    {
        opts.AddRowNumberSupport();
    });
}
```

### SQLite

```
using Webrox.EntityFrameworkCore.Sqlite;

protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseSqlite("connectionstring", opts =>
    {
        opts.AddRowNumberSupport();
    });
}
```

### PostgreSQL

```
using Webrox.EntityFrameworkCore.Postgres;

protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseNpgsql("connectionstring", opts =>
    {
        opts.AddRowNumberSupport();
    });
}
```

## RowNumber Syntax


```
using Webrox.EntityFrameworkCore.Core;

// simple RowNumber + OrderBy
context.Table1.Select(t => new
{
    Id = t.Id,
    RowNumber = EF.Functions.RowNumber(EF.Functions.OrderBy(t.Id))
}

// complex RowNumber + PartitionBy + OrderBy
context.Table1.Select(t => new
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