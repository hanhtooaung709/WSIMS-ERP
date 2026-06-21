namespace WSIMS_ERP.Shared.Models;

public class Result<T>
{
    public bool IsSuccess => RespType == EnumRespType.Success;
    public bool IsError => !IsSuccess;
    public EnumRespType RespType { get; set; }
    public string RespCode { get; set; }
    public string RespDesp { get; set; }
    public object[]? RespDespParameter { get; set; }
    public T Data { get; set; }

    public static Result<T> Success(T data, string code = "MS#000")
    {
        return new Result<T> { Data = data, RespCode = code, RespType = EnumRespType.Success };
    }

    public static Result<T> Success(string code = "MS#000")
    {
        return new Result<T> { RespCode = code, RespType = EnumRespType.Success };
    }

    public static Result<T> Error(T data, string code)
    {
        return new Result<T> { Data = data, RespCode = code, RespType = EnumRespType.Error };
    }

    public static Result<T> Error(string code)
    {
        return new Result<T> { RespCode = code, RespType = EnumRespType.Error };
    }

    public static Result<T> Error(Exception ex, string code = "ME#999")
    {
        return new Result<T> { RespDesp = ex.Message, RespCode = code, RespType = EnumRespType.Error };
    }

    public static Result<T> Error(string code = "ME#999", string respDesp = "")
    {
        return new Result<T> { RespDesp = respDesp, RespCode = code, RespType = EnumRespType.Error };
    }

    public static Result<T> DataError(string messageCode, T Data, string? message = null)
    {
        return new Result<T> { RespDesp = message!, RespType = EnumRespType.Error, Data = Data, RespCode = messageCode };
    }

    public static Result<T> Error(string code, params object[] parameters)
    {
        return new Result<T>
        {
            RespCode = code,
            RespType = EnumRespType.Error,
            RespDespParameter = parameters
        };
    }

    public static Result<T> Success(string code, params object[] parameters)
    {
        return new Result<T>
        {
            RespCode = code,
            RespType = EnumRespType.Success,
            RespDespParameter = parameters
        };
    }

    public static Result<T> Success(string code, T Data, params object[] parameters)
    {
        return new Result<T>
        {
            RespCode = code,
            RespType = EnumRespType.Success,
            RespDespParameter = parameters,
            Data = Data
        };
    }
}
