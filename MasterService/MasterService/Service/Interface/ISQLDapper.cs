using Dapper;
using System.Data.Common;
using System.Data;

namespace MasterService.Service.Interface;
public interface ISQLDapper : IDisposable
{
    DbConnection GetDbconnection(string aadharFirstNumber);
    T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
    List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
    int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
    T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
    T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
}
