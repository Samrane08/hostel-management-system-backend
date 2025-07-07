using Dapper;
using MasterService.Service.Interface;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace MasterService.Service.Implementation;

public class SQLDapper : ISQLDapper
{
    private readonly IConfiguration _config;
    private string Connectionstring = "PostMatricSQLConnection";
    public SQLDapper(IConfiguration config)
    {
        _config = config;
    }
    public DbConnection GetDbconnection(string aadharFirstNumber)
    {
      
            return new SqlConnection(_config.GetConnectionString(Connectionstring));
    }
    public void Dispose()
    {

    }
    public int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
    {
        using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
        return db.Execute(sp, parms, commandType: commandType);
 
    }
    public T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
    {
        
        using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
        return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
    }
    public List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
    {
        
        using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
        return db.Query<T>(sp, parms, commandType: commandType).ToList();
    }
    public T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
    {
        T result;
        using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
        try
        {
            if (db.State == ConnectionState.Closed)
                db.Open();

            using var tran = db.BeginTransaction();
            try
            {
                result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                tran.Commit();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (db.State == ConnectionState.Open)
                db.Close();
        }
        return result;
    }
    public T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
    {
      
        T result;
        using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
        try
        {
            if (db.State == ConnectionState.Closed)
                db.Open();

            using var tran = db.BeginTransaction();
            try
            {
                result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (db.State == ConnectionState.Open)
                db.Close();
        }
        return result;
    }
}

