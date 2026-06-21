namespace WSIMS_ERP.Shared.Services;

public class DapperService
{
    private readonly ILogger<DapperService> _logger;
    private readonly CustomSettingModel _setting;

    public DapperService(IOptionsMonitor<CustomSettingModel> setting, ILogger<DapperService> logger)
    {
        _setting = setting.CurrentValue;
        _logger = logger;
    }

    public List<T> QueryStoredProcedure<T>(string query, object parameters)
    {
        try
        {
            using IDbConnection dbConnection = new SqlConnection(_setting.DbConnection);
            dbConnection.Open();
            var lst = dbConnection.Query<T>(query, parameters, commandType: CommandType.StoredProcedure);
            List<T> result = lst.ToList();

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }

        return new List<T>();
    }

    public async Task<T> GetDetailAsync<T>(
        string query,
        object parameters,
        CommandType commandType = CommandType.Text)
    {
        using (IDbConnection dbConnection = new SqlConnection(_setting.DbConnection))
        {
            var result = await dbConnection.QueryAsync<T>(query, parameters, commandType: commandType);
            return result.FirstOrDefault()!;
        }
    }

    public async Task<(R, List<T>)> GetMultipleListAsync<R, T>(string query, object? parameters = null)
    {
        using (IDbConnection dbConnection = new SqlConnection(_setting.DbConnection))
        {
            using (var result = dbConnection.QueryMultiple(query, parameters, commandType: CommandType.StoredProcedure))
            {
                var pageSetting = await result.ReadFirstAsync<R>();
                var lstResult = await result.ReadAsync<T>();
                var lst = lstResult.ToList();
                return (pageSetting, lst);
            }
        }
    }
}
