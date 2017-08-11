namespace DoomLauncher
{
    using DoomLauncher.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.CompilerServices;

    internal class DataAccess
    {
        public DataAccess(IDatabaseAdapter dbAdapter, string connectionString)
        {
            this.DbAdapter = dbAdapter;
            this.ConnectionString = connectionString;
        }

        public void ExecuteNonQuery(string sql)
        {
            DbConnection connection1 = this.DbAdapter.CreateConnection(this.ConnectionString);
            connection1.Open();
            DbCommand command1 = connection1.CreateCommand();
            command1.CommandText = sql;
            command1.ExecuteNonQuery();
            connection1.Close();
        }

        public void ExecuteNonQuery(string sql, IEnumerable<DbParameter> parameters)
        {
            DbConnection connection = this.DbAdapter.CreateConnection(this.ConnectionString);
            connection.Open();
            DbCommand command = connection.CreateCommand();
            command.CommandText = sql;
            foreach (DbParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }
            command.ExecuteNonQuery();
            connection.Close();
        }

        public DataSet ExecuteSelect(string sql)
        {
            DbConnection connection1 = this.DbAdapter.CreateConnection(this.ConnectionString);
            connection1.Open();
            DbCommand command = connection1.CreateCommand();
            command.CommandText = sql;
            DataSet dataSet = new DataSet();
            DbDataAdapter adapter1 = this.DbAdapter.CreateAdapter();
            adapter1.SelectCommand = command;
            adapter1.Fill(dataSet);
            connection1.Close();
            return dataSet;
        }

        public DataSet ExecuteSelect(string sql, IEnumerable<DbParameter> parameters)
        {
            DbConnection connection = this.DbAdapter.CreateConnection(this.ConnectionString);
            connection.Open();
            DbCommand command = connection.CreateCommand();
            command.CommandText = sql;
            foreach (DbParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }
            DataSet dataSet = new DataSet();
            DbDataAdapter adapter1 = this.DbAdapter.CreateAdapter();
            adapter1.SelectCommand = command;
            adapter1.Fill(dataSet);
            connection.Close();
            return dataSet;
        }

        public string ConnectionString { get; private set; }

        public IDatabaseAdapter DbAdapter { get; private set; }
    }
}

