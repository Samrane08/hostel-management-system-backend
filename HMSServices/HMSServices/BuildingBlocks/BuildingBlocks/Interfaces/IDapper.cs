
using System.Data.Common;
using System.Data;
using Dapper;
using BuildingBlocks.Models.QueryModels;

namespace BuildingBlocks.Interfaces
{
    public interface IDapper : IDisposable
    {
        DbConnection GetDbconnection();
        T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
       Task< int> ExecuteQueryAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

      // Task<int> ExecuteQuery(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<List<Dictionary<string, object>>> GetAllAsDictionaryAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<List<IEnumerable<dynamic>>> MultiResult(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
    }

    public interface ICommonFunction
    {
          Task<(string fin1, string fin2)> GetFinancialYearsBySchemeIdAsync(int schemeId);
        Task<DDODetailsModel> GetDDODetails(int schemeID, string userId);

    }
}