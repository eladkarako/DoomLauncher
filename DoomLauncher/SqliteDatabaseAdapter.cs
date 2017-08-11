namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Data.Common;
    using System.Data.SQLite;

    internal class SqliteDatabaseAdapter : IDatabaseAdapter
    {
        public DbDataAdapter CreateAdapter() => 
            new SQLiteDataAdapter();

        public DbConnection CreateConnection(string connectionString) => 
            new SQLiteConnection(connectionString);

        public DbParameter CreateParameter(string name, object value) => 
            new SQLiteParameter(name, value);
    }
}

