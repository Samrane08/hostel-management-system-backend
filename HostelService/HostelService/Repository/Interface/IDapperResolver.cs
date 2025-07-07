
using Microsoft.Extensions.Configuration;
using Repository.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface;


public interface IDapperResolver
{
    IBaseDapper Resolve(string departmentId);
}

public class DapperResolver : IDapperResolver
{
    private readonly IConfiguration _config;

    public DapperResolver(IConfiguration config)
    {
        _config = config;

    }
    public IBaseDapper Resolve(string departmentId)
    {
        if (string.IsNullOrEmpty(departmentId))
            throw new ArgumentNullException(nameof(departmentId), "Department ID cannot be null or empty.");

        if (!int.TryParse(departmentId, out int parsedDepartmentId) || parsedDepartmentId <= 0)
            throw new ArgumentException("Department ID must be a positive integer.", nameof(departmentId));

        return parsedDepartmentId switch
        {
            1 => new SJSADapperr(_config),
            2 => new VJNTDapperr(_config),
            _ => throw new ArgumentException("Unknown Department ID. Please provide a valid department ID.", nameof(departmentId))
        };
    }
}
