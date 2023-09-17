using System;

using System.Data;
using System.Data.SqlClient;

using System.Collections.Generic;

namespace MqttNetApplication
{
     public static class MsSqlDataAccess
    {
        public static string ConnStr;


        public static int NonQuery(string CommandText)
        {
            var res = 0;
            SqlConnection SqlConn=new SqlConnection(MsSqlDataAccess.ConnStr);
            try
            {
                SqlConn.Open();
                SqlCommand SqlComm = new SqlCommand(CommandText, SqlConn);
                res = SqlComm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                SqlConn.Close();
                SqlConn.Dispose();
            }
            return res;
        }

        public static int NonQuery(string CommandText, CommandType CommandType, SqlParameter[] Parameters)
        {
            var res = 0;
            SqlConnection SqlConn = new SqlConnection(MsSqlDataAccess.ConnStr);

            try
            {
                SqlConn.Open();
                SqlCommand SqlComm = new SqlCommand(CommandText, SqlConn);
                SqlComm.CommandType = CommandType;
                if (Parameters != null)
                {
                    for (var i = 0; i <= Parameters.Length - 1; i++)
                    {
                        SqlComm.Parameters.Add(Parameters[i]);
                    }
                }
                res = SqlComm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                SqlConn.Close();
                SqlConn.Dispose();
            }
            return res;
        }



        public static SqlDataReader GetSqlDataReader(string sql)
        {
            SqlDataReader res = null;
            SqlConnection SqlConn = new SqlConnection(MsSqlDataAccess.ConnStr);
            try
            {
                SqlConn.Open();
                SqlCommand SqlComm = new SqlCommand(sql, SqlConn);
                SqlComm.CommandType = CommandType.Text;

                res = SqlComm.ExecuteReader();
                SqlComm.Dispose();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
      
                SqlConn.Close();
                SqlConn.Dispose();
            }
            return res;
        }


        public static SqlDataReader GetSqlDataReader(string CommandText, CommandType CommandType, SqlParameter[] Parameters)
        {
            SqlDataReader res = null;
            SqlConnection SqlConn = new SqlConnection(MsSqlDataAccess.ConnStr);
            try
            {
                SqlConn.Open();
                SqlCommand SqlComm = new SqlCommand(CommandText, SqlConn);
                SqlComm.CommandType = CommandType;
                if (Parameters != null)
                {
                    for (var i = 0; i <= Parameters.Length - 1; i++)
                    {
                        SqlComm.Parameters.Add(Parameters[i]);
                    }
                }
                res = SqlComm.ExecuteReader();
                SqlComm.Dispose();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {

                SqlConn.Close();
                SqlConn.Dispose();
            }
            return res;
        }


        public static DataSet GetDataset(string sql)
        {
            var res = new DataSet();
            SqlConnection SqlConn = new SqlConnection(MsSqlDataAccess.ConnStr);
            try
            {
                SqlConn.Open();
                SqlCommand SqlComm = new SqlCommand(sql, SqlConn);
                SqlComm.CommandType = CommandType.Text;

                SqlDataAdapter SqlDA = new SqlDataAdapter(SqlComm);
                SqlDA.Fill(res);
                SqlComm.Dispose();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
           
                SqlConn.Close();
                SqlConn.Dispose();
            }
            return res;
        }

        public static DataSet GetDataset(string CommandText, CommandType CommandType, SqlParameter[] Parameters)
        {
            var res = new DataSet();

            SqlConnection SqlConn = new SqlConnection(MsSqlDataAccess.ConnStr);

            try
            {
                SqlConn.Open();
                SqlCommand SqlComm = new SqlCommand(CommandText, SqlConn);
                SqlComm.CommandType = CommandType;
                if (Parameters != null)
                {
                    for (var i = 0; i <= Parameters.Length - 1; i++)
                    {
                        SqlComm.Parameters.Add(Parameters[i]);
                    }
                }
                SqlDataAdapter SqlDA = new SqlDataAdapter(SqlComm);
                SqlDA.Fill(res);
                SqlDA.Dispose();
                SqlComm.Dispose();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
          
                SqlConn.Close();
                SqlConn.Dispose();
            }
            return res;
        }


        public static DataTable GetDataTable(string sql)
        {
            var res = new DataSet();
            SqlConnection SqlConn = new SqlConnection(MsSqlDataAccess.ConnStr);
            try
            {
                SqlConn.Open();
                SqlCommand SqlComm = new SqlCommand(sql, SqlConn);
                SqlComm.CommandType = CommandType.Text;

                SqlDataAdapter SqlDA = new SqlDataAdapter(SqlComm);
                SqlDA.Fill(res);
                SqlDA.Dispose();
                SqlComm.Dispose();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
 
                SqlConn.Close();
                SqlConn.Dispose();
            }
            return res.Tables[0];
        }


        public static DataTable GetDataTable(string CommandText, CommandType CommandType, SqlParameter[] Parameters)
        {
            var res = new DataSet();
            SqlConnection SqlConn = new SqlConnection(MsSqlDataAccess.ConnStr);
            try
            {
                SqlConn.Open();
                SqlCommand SqlComm = new SqlCommand(CommandText, SqlConn);
                SqlComm.CommandType = CommandType;
                if (Parameters != null)
                {
                    for (var i = 0; i <= Parameters.Length - 1; i++)
                    {
                        SqlComm.Parameters.Add(Parameters[i]);
                    }
                }
                SqlDataAdapter SqlDA = new SqlDataAdapter(SqlComm);
                SqlDA.Fill(res);
                SqlDA.Dispose();
                SqlComm.Dispose();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
      
                SqlConn.Close();
                SqlConn.Dispose();
            }
            return res.Tables[0];
        }


        public static object ExecuteScalar(string sqlstr)
        {
            object result;
            SqlConnection SqlConn = new SqlConnection(MsSqlDataAccess.ConnStr);
            try
            {
                SqlConn.Open();
                var cmd = SqlConn.CreateCommand();
                cmd.CommandText = sqlstr;

                if (SqlConn.State == ConnectionState.Closed)
                {
                    SqlConn.Open();
                }
                result = cmd.ExecuteScalar();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                SqlConn.Close();
                SqlConn.Dispose();
            }


            return result;
        }


        public static object ExecuteScalar(string CommandText, CommandType CommandType, SqlParameter[] Parameters)
        {
            object result;

            SqlConnection SqlConn = new SqlConnection(MsSqlDataAccess.ConnStr);

            try
            {
                SqlConn.Open();
                SqlCommand SqlComm = new SqlCommand(CommandText, SqlConn);
                SqlComm.CommandType = CommandType;
                if (Parameters != null)
                {
                    for (var i = 0; i <= Parameters.Length - 1; i++)
                    {
                        SqlComm.Parameters.Add(Parameters[i]);
                    }
                }
                result = SqlComm.ExecuteScalar();
                SqlComm.Dispose();
            }
            catch (Exception ex)
            {
       
                throw (ex);
            }
            finally
            {
          
                SqlConn.Close();
                SqlConn.Dispose();
            }

            return result;
        }
    }

}
