namespace WSIMS_ERP.Shared.Models;

public class Result<T>
{
    public bool IsSuccess => RespType == EnumRespType.Success;
    public bool IsError => !IsSuccess;
    public EnumRespType RespType { get; set; }
    public string RespDesp { get; set; }
    public T Data { get; set; }


    #region Success
    public static Result<T> Success(T? data, string respDesp = "Success")
    {
        return new Result<T>
        {
            Data = data,
            RespDesp = respDesp,
            RespType = EnumRespType.Success
        };
    }

    public static Result<T> Success(string respDesp)
    {
        return new Result<T>
        {
            RespDesp = respDesp,
            RespType = EnumRespType.Success
        };
    }
    #endregion

    #region Error
    public static Result<T> Error(T? data = default, string respDesp = "Error")
    {
        return new Result<T>
        {
            Data = data,
            RespDesp = respDesp,
            RespType = EnumRespType.Error
        };
    }

    public static Result<T> Error(string respDesp)
    {
        return new Result<T> 
        {
            RespDesp = respDesp,
            RespType = EnumRespType.Error
        };
    }

    public static Result<T> Error(Exception ex, string code = "ME#999")
    {
        return new Result<T>
        {
            RespDesp = ex.Message,
            RespType = EnumRespType.Error
        };
    }
    #endregion
}
