namespace DoomLauncher.Interfaces
{
    using System;
    using System.Data.Common;

    public interface IDatabaseAdapter
    {
        DbDataAdapter CreateAdapter();
        DbConnection CreateConnection(string connectionString);
        DbParameter CreateParameter(string name, object value);
    }
}

