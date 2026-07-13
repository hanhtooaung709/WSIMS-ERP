namespace WSIMS_ERP.Shared.Services;

public class JsonResource
{
    #region General Response

    /// <summary>
    /// Success
    /// </summary>
    public static readonly string Success = "S000";

    /// <summary>
    /// System Error.
    /// </summary>
    public static readonly string Fail = "E999";

    #endregion

    #region Warehouse Response

    /// <summary>
    /// User does not exist.
    /// </summary>
    public static readonly string WHE001 = "WH#S001";

    /// <summary>
    /// Your account is lock.
    /// </summary>
    public static readonly string WHE002 = "WH#S002";

    /// <summary>
    /// User Name or Password is wrong.
    /// </summary>
    public static readonly string WHE003 = "WH#S003";


    #endregion
}
