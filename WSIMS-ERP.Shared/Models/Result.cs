namespace WSIMS_ERP.Shared.Models;

public class Result<T>
{
    public bool IsSuccess => RespType == EnumRespType.Success;
    public bool IsError => !IsSuccess;
    public EnumRespType RespType { get; set; }
    public string? RespCode { get; set; }
    public string RespDesp { get; set; }
    public T Data { get; set; }


    #region Success
    public static Result<T> Success(T? data, string respCode = "S000")
    {
        return new Result<T>
        {
            Data = data,
            RespDesp = respCode,
            RespType = EnumRespType.Success
        };
    }

    public static Result<T> Success(string respCode)
    {
        return new Result<T>
        {
            RespCode = respCode,
            RespType = EnumRespType.Success
        };
    }
    #endregion

    #region Error
    public static Result<T> Error(T? data = default, string respCode = "E999")
    {
        return new Result<T>
        {
            Data = data,
            RespCode = respCode,
            RespType = EnumRespType.Error
        };
    }

    public static Result<T> Error(string respCode)
    {
        return new Result<T> 
        {
            RespCode = respCode,
            RespType = EnumRespType.Error
        };
    }

    public static Result<T> Error(Exception ex)
    {
        return new Result<T>
        {
            RespDesp = ex.Message,
            RespType = EnumRespType.Error
        };
    }
    #endregion
}
