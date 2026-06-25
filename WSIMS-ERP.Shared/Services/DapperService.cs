using System.Data.SqlClient;
using WSIMS_ERP.Shared;

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

    public async Task<List<T>> GetListAsync<T>(string query, object? parameter = null)
    {
        using IDbConnection dbConnection = new SqlConnection(_setting.DbConnection);
        dbConnection.Open();
        var lst = await dbConnection.QueryAsync<T>(query, parameter);
        List<T> result = lst.ToList();

        return result;
    }

    public async Task<DapperResponseModel<T>> GetListExecute<T>(string query, object parameters)
    {
        DapperResponseModel<T> model = new();
        using IDbConnection dbConnection = new SqlConnection(_setting.DbConnection);
        dbConnection.Open();
        var result = await dbConnection.QueryAsync<T>(query, parameters);
        model.TotalRowCount = result.Count();
        model.Data = result
            .ToList();
        return model;
    }

    public async Task<T> QueryFirstAsync<T>(string query, object parameter)
    {
        using IDbConnection dbConnection = new SqlConnection(_setting.DbConnection);
        dbConnection.Open();
        var result = await dbConnection.QueryFirstOrDefaultAsync<T>(query, parameter);

        return result;
    }

    public async Task<int> ExecuteAsync(string query, object? parameters = null)
    {
        using IDbConnection dbConnection = new SqlConnection(_setting.DbConnection);
        dbConnection.Open();
        var result = await dbConnection.ExecuteAsync(query, parameters);

        return result;
    }

    public async Task<dynamic> QueryStoredProcedure(string query, object parameters)
    {
        using IDbConnection dbConnection = new SqlConnection(_setting.DbConnection);
        dbConnection.Open();
        dynamic result = await dbConnection.QueryAsync(query, parameters, commandType: CommandType.StoredProcedure);
        return result;
    }

    public async Task<List<T>> QueryStoredProcedureAsync<T>(string query, object parameters)
    {
        return await Query<T>(query, parameters, CommandType.StoredProcedure);
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

    public async Task<T> QueryStoredProcedureFirstOrDefault<T>(string query, object parameters)
    {
        try
        {
            using IDbConnection dbConnection = new SqlConnection(_setting.DbConnection);
            dbConnection.Open();
            var lst = await dbConnection.QueryAsync<T>(
                query,
                parameters,
                commandTimeout: 0,
                commandType: CommandType.StoredProcedure);
            var result = lst.FirstOrDefault()!;

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }

        return default(T);
    }
    
    public async Task<List<T>> Query<T>(string query, object? parameters = null,
        CommandType commandType = CommandType.Text)
    {
        try
        {
            using IDbConnection dbConnection = new SqlConnection(_setting.DbConnection);
            dbConnection.Open();
            var lst = await dbConnection.QueryAsync<T>(query, parameters, commandType: commandType);
            List<T> result = lst.ToList();

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }

        return new List<T>();
    }

    public async Task<T> GetDetailAsync<T>(string query, object parameters)
    {
        using IDbConnection dbConnection = new SqlConnection(_setting.DbConnection);
        dbConnection.Open();
        var result = await dbConnection.QueryAsync<T>(query, parameters);
        return result.FirstOrDefault()!;
    }

    public async Task<int> ExecuteCountAsync(string query, object parameters)
    {
        using IDbConnection dbConnection = new SqlConnection(_setting.DbConnection);
        dbConnection.Open();
        var result = await dbConnection.QuerySingleAsync<int>(query, parameters);
        return result;
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

    public async Task<(R, List<T>)> GetMultipleList<R, T>(string query, object? parameters = null)
    {
        using (IDbConnection db = new SqlConnection(_setting.DbConnection))
        {
            using (var multi = await db.QueryMultipleAsync(
                       query,
                       parameters,
                       commandType: CommandType.StoredProcedure))
            {
                var data = (await multi.ReadAsync<T>()).ToList();
                var pageInfo = await multi.ReadFirstOrDefaultAsync<R>();
                return (pageInfo, data);
            }
        }
    }

    public async Task<(R, List<T>)> GetVoucherMultipleList<R, T>(string query, object? parameters = null)
    {
        using (IDbConnection db = new SqlConnection(_setting.DbConnection))
        {
            using (var multi = await db.QueryMultipleAsync(
                       query,
                       parameters,
                       commandType: CommandType.StoredProcedure))
            {
                var data = await multi.ReadFirstOrDefaultAsync<R>();
                var dataDetail = (await multi.ReadAsync<T>()).ToList();
                return (data, dataDetail);
            }
        }
    }

    public async Task<(List<T>, R)> GetListWithSingleData<T, R>(string query, object? parameters = null)
    {
        using (IDbConnection db = new SqlConnection(_setting.DbConnection))
        {
            using (var multi = await db.QueryMultipleAsync(query, parameters, commandType: CommandType.StoredProcedure))
            {
                var data = await multi.ReadFirstOrDefaultAsync<R>();
                var dataDetail = (await multi.ReadAsync<T>()).ToList();

                return (dataDetail!, data!);
            }
        }
    }

    public async Task<(int totalCount, int readCount, List<T> results)> GetNotificationResultsAsync<T>(
        string query,
        object? parameters = null)
    {
        using (IDbConnection dbConnection = new SqlConnection(_setting.DbConnection))
        {
            using (var reader = await dbConnection.QueryMultipleAsync(
                       query,
                       parameters,
                       commandType: CommandType.StoredProcedure))
            {
                var totalCount = await reader.ReadFirstAsync<int>();
                var readCount = await reader.ReadFirstAsync<int>();
                var results = (await reader.ReadAsync<T>()).ToList();

                return (totalCount, readCount, results);
            }
        }
    }

    public async Task<(T? Result, Dictionary<string, object> Outputs)> GetDetailWithOutputAsync<T>(
        string procedureName,
        object? inputParams = null,
        Dictionary<string, DbType>? outputParams = null,
        Dictionary<string, int>? outputSizes = null, // New parameter for individual sizes
        CommandType commandType = CommandType.StoredProcedure
    ) where T : class, new()
    {
        using IDbConnection dbConnection = new SqlConnection(_setting.DbConnection);
        var dynamicParams = new DynamicParameters();

        // Add input parameters
        if (inputParams != null)
        {
            dynamicParams.AddDynamicParams(inputParams);
        }

        // Add output parameters with specific sizes
        if (outputParams != null)
        {
            foreach (var kvp in outputParams)
            {
                int size = (outputSizes != null && outputSizes.ContainsKey(kvp.Key)) ? outputSizes[kvp.Key] : 200;
                dynamicParams.Add(kvp.Key, dbType: kvp.Value, size: size, direction: ParameterDirection.Output);
            }
        }

        try
        {
            var result = await dbConnection.QueryFirstOrDefaultAsync<T>(
                procedureName,
                dynamicParams,
                commandType: commandType
            );

            // Collect output values
            var outputs = new Dictionary<string, object>();
            if (outputParams != null)
            {
                foreach (var kvp in outputParams)
                {
                    var outputValue = dynamicParams.Get<object>(kvp.Key);
                    if (outputValue != null) // Safer check
                    {
                        outputs[kvp.Key] = outputValue;
                    }
                }
            }

            return (result, outputs);
        }
        catch (SqlException ex)
        {
            // Log the exception
            // Rethrow or return a failure state
            throw new ApplicationException($"An error occurred while executing the stored procedure: {procedureName}",
                ex);
        }
    }

    public async Task<(R, List<T1>, List<T2>)> GetThreeListAsync<R, T1, T2>(string query, object? parameters = null)
    {
        using (IDbConnection dbConnection = new SqlConnection(_setting.DbConnection))
        {
            using (var result = dbConnection.QueryMultiple(query, parameters, commandType: CommandType.StoredProcedure))
            {
                var pageSetting = await result.ReadFirstAsync<R>();
                var list1 = (await result.ReadAsync<T1>()).ToList();
                var list2 = (await result.ReadAsync<T2>()).ToList();
                return (pageSetting, list1, list2);
            }
        }
    }

    public async Task<string> ExecuteAsyncWithOutput(string storedProcedureName, object parameters,
        string outputParameterName)
    {
        if (!(parameters is DynamicParameters dynamicParameters))
        {
            throw new ArgumentException("Parameters must be a Dapper DynamicParameters instance for stored procedures with output.");
        }

        using IDbConnection dbConnection = new SqlConnection(_setting.DbConnection);
        dbConnection.Open();
        _ = await dbConnection.ExecuteAsync(
            sql: storedProcedureName,
            param: dynamicParameters,
            commandType: CommandType.StoredProcedure
        );
        return dynamicParameters.Get<string>(outputParameterName);
    }
}

public class DapperResponseModel<T>
{
    public int TotalRowCount { get; set; }
    public List<T> Data { get; set; }
}

public class TransactionResultModel
{
    public string WalletAccount { get; set; }
    public bool Success { get; set; }
    public string RespCode { get; set; }
    public string RespMessage { get; set; }

    public int WalletTranLogId { get; set; }
    public string? TranDate { get; set; }
    public string TranId { get; set; }
    public int BillPaymentTranLogId { get; set; }
    public string? EpcPaymentTranLogId { get; set; }
    public decimal TranAmount { get; set; }
}