using Dapper;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using Repository.Interface;
using System.Data;
using System.Data.Common;

namespace Repository.Implementation;

public class SJSADapperr : IBaseDapper
{
    private readonly IConfiguration _config;
    private string Connectionstring = "SJSAConnectionString";
  
    public SJSADapperr(IConfiguration config)
    {
        _config = config;

    }
    public DbConnection GetDbconnection()
    {
        return new MySqlConnection(_config.GetConnectionString(Connectionstring));
    }
    public void Dispose()
    {

    }
    public int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
    {
        using IDbConnection db = new MySqlConnection(_config.GetConnectionString(Connectionstring));
        return db.Execute(sp, parms, commandType: commandType);
    }
    public T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
    {
        using IDbConnection db = new MySqlConnection(_config.GetConnectionString(Connectionstring));
        return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
    }
    public List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
    {
        using IDbConnection db = new MySqlConnection(_config.GetConnectionString(Connectionstring));
        return db.Query<T>(sp, parms, commandType: commandType).ToList();
    }

    public async Task<List<Dictionary<string, object>>> GetAllAsDictionaryAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
    {
        using IDbConnection db = new MySqlConnection(_config.GetConnectionString(Connectionstring));
        // Asynchronously query the database using Dapper
        var result = db.Query(sp, parms, commandType: commandType);

       

        // Convert result to List<Dictionary<string, object>> in one step
        return result.Select(row => (IDictionary<string, object>)row)
                     .Select(dict => new Dictionary<string, object>(dict))
                     .ToList();
    }

    public T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
    {
        T result = default;
        using (IDbConnection db = new MySqlConnection(_config.GetConnectionString(Connectionstring)))
        {

            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using (var tran = db.BeginTransaction())
                {
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
                };
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
        };
        return result;
    }
    public T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
    {
        T result = default;
        using (IDbConnection db = new MySqlConnection(_config.GetConnectionString(Connectionstring)))
        {
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using (var tran = db.BeginTransaction())
                {
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
                };
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
        };
        return result;
    }
    public async Task<List<IEnumerable<dynamic>>> MultiResult(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
    {
        try
        {
            using IDbConnection db = new MySqlConnection(_config.GetConnectionString(Connectionstring));
            var multi = await db.QueryMultipleAsync(sp, parms, commandType: commandType);
            var resultSets = new List<IEnumerable<dynamic>>();
            while (!multi.IsConsumed)
            {
                var resultSet = await multi.ReadAsync();
                resultSets.Add(resultSet);
            }
            return resultSets;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}


public class VJNTDapperr : IBaseDapper
{
    private readonly IConfiguration _config;
    private string Connectionstring = "VJNTConnectionString";

    public VJNTDapperr(IConfiguration config)
    {
        _config = config;

    }
    public DbConnection GetDbconnection()
    {
        return new MySqlConnection(_config.GetConnectionString(Connectionstring));
    }
    public void Dispose()
    {

    }
    public int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
    {
        using IDbConnection db = new MySqlConnection(_config.GetConnectionString(Connectionstring));
        return db.Execute(sp, parms, commandType: commandType);
    }
    public T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
    {
        using IDbConnection db = new MySqlConnection(_config.GetConnectionString(Connectionstring));
        return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
    }
    public List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
    {
        using IDbConnection db = new MySqlConnection(_config.GetConnectionString(Connectionstring));
        return db.Query<T>(sp, parms, commandType: commandType).ToList();
    }

    public async Task<List<Dictionary<string, object>>> GetAllAsDictionaryAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
    {
        using IDbConnection db = new MySqlConnection(_config.GetConnectionString(Connectionstring));
        // Asynchronously query the database using Dapper
        var result = db.Query(sp, parms, commandType: commandType);



        // Convert result to List<Dictionary<string, object>> in one step
        return result.Select(row => (IDictionary<string, object>)row)
                     .Select(dict => new Dictionary<string, object>(dict))
                     .ToList();
    }

    public T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
    {
        T result = default;
        using (IDbConnection db = new MySqlConnection(_config.GetConnectionString(Connectionstring)))
        {

            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using (var tran = db.BeginTransaction())
                {
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
                };
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
        };
        return result;
    }
    public T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
    {
        T result = default;
        using (IDbConnection db = new MySqlConnection(_config.GetConnectionString(Connectionstring)))
        {
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using (var tran = db.BeginTransaction())
                {
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
                };
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
        };
        return result;
    }
    public async Task<List<IEnumerable<dynamic>>> MultiResult(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
    {
        try
        {
            using IDbConnection db = new MySqlConnection(_config.GetConnectionString(Connectionstring));
            var multi = await db.QueryMultipleAsync(sp, parms, commandType: commandType);
            var resultSets = new List<IEnumerable<dynamic>>();
            while (!multi.IsConsumed)
            {
                var resultSet = await multi.ReadAsync();
                resultSets.Add(resultSet);
            }
            return resultSets;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}