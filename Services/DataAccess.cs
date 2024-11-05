using System.Data.SqlClient;
using System;
using System.Configuration;

namespace Services
{
    public sealed class DataAccess
    {
        private SqlConnection _connection;
        private SqlCommand _command;
        private SqlDataReader _reader;
        public SqlDataReader Reader
        {
            get { return _reader; }
        }

        public DataAccess()
        {
            _connection = new SqlConnection(ConfigurationManager.AppSettings["connectionString"]);
            _command = new SqlCommand();
        }

        public void SetQuery(string query)
        {
            _command.CommandType = System.Data.CommandType.Text;
            _command.CommandText = query;
        }

        public void SetProcedure(string storedProcedure)
        {
            _command.CommandType = System.Data.CommandType.StoredProcedure;
            _command.CommandText = storedProcedure;

        }

        public void ExecuteReader()
        {
            _command.Connection = _connection;
            try
            {
                _connection.Open();
                _reader = _command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExecuteAction()
        {
            _command.Connection = _connection;
            try
            {
                _connection.Open();
                _command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ExecuteScalarAction()
        {
            _command.Connection = _connection;
            try
            {
                _connection.Open();
                return int.Parse(_command.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetParameter(string name, object value)
        {
            _command.Parameters.AddWithValue(name, value);
        }

        public void CloseConnection()
        {
            if (_reader != null)
                _reader.Close();

            _connection.Close();
        }
    }
}
