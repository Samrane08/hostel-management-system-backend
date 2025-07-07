using Dapper;
using System.Data.Common;
using System.Data;

namespace MasterService.Service.Interface;
public interface IDapper: IDisposable
{
    DbConnection GetDbconnection();
    T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
    List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
    int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

    int ExecuteQuery(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
    T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
    T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
    Task<List<IEnumerable<dynamic>>> MultiResult(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

}
