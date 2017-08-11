namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Data.Common;
    using System.Data.SqlClient;

    internal class MSSQLDataAdapter : IDatabaseAdapter
    {
        public DbDataAdapter CreateAdapter() => 
            new SqlDataAdapter();

        public DbConnection CreateConnection(string connectionString) => 
            new SqlConnection(connectionString);

        public DbParameter CreateParameter(string name, object value) => 
            new SqlParameter(name, value);
    }
}

