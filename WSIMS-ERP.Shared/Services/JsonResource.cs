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
    /// Requsted User is successfully Deteted.
    /// </summary>
    public static readonly string WHS023 = "WH#S023";

    /// <summary>
    /// Requsted User delete fail!
    /// </summary>
    public static readonly string WHE022 = "WH#E022";

    /// <summary>
    /// Requested User is successfully Updated.
    /// </summary>
    public static readonly string WHS021 = "WH#S021";

    /// <summary>
    /// Requseted User is not pending status!
    /// </summary>
    public static readonly string WHE020 = "WH#E020";

    /// <summary>
    /// Requested User does not exist!
    /// </summary>
    public static readonly string WHE019 = "WH#E019";

    /// <summary>
    /// User is already Change Requested!
    /// </summary>
    public static readonly string WHE018 = "WH#E018";

    /// <summary>
    /// User is already Requested!
    /// </summary>
    public static readonly string WHE017 = "WH#E017";

    /// <summary>
    /// Branch does not exist!
    /// </summary>
    public static readonly string WHE016 = "WH#E016";

    /// <summary>
    /// User Role does not exist!
    /// </summary>
    public static readonly string WHE015 = "WH#E015";

    /// <summary>
    /// Your request is pending for Approval.
    /// </summary>
    public static readonly string WHS014 = "WH#S014";

    /// <summary>
    /// Email is already Change Requested!
    /// </summary>
    public static readonly string WHE013 = "WH#E013";

    /// <summary>
    /// Email is already Requested!
    /// </summary>
    public static readonly string WHE012 = "WH#E012";

    /// <summary>
    /// Email is already exist!
    /// </summary>
    public static readonly string WHE011 = "WH#E011";

    /// <summary>
    /// Phone Number is already Change Requested!
    /// </summary>
    public static readonly string WHE010 = "WH#E010";

    /// <summary>
    /// Phone Number is already Requested!
    /// </summary>
    public static readonly string WHE009 = "WH#E009";

    /// <summary>
    /// Phone Number is already exist!
    /// </summary>
    public static readonly string WHE008 = "WH#E008";

    /// <summary>
    /// StaffId is already Requested!
    /// </summary>
    public static readonly string WHE007 = "WH#E007";

    /// <summary>
    /// StaffId is already exist!
    /// </summary>
    public static readonly string WHE006 = "WH#E006";

    /// <summary>
    /// UserName is already Requested!
    /// </summary>
    public static readonly string WHE005 = "WH#E005";

    /// <summary>
    /// UserName is already exist!
    /// </summary>
    public static readonly string WHE004 = "WH#E004";

    /// <summary>
    /// User Name or Password is wrong.
    /// </summary>
    public static readonly string WHE003 = "WH#E003";

    /// <summary>
    /// Your account is lock.
    /// </summary>
    public static readonly string WHE002 = "WH#E002";

    /// <summary>
    /// User does not exist.
    /// </summary>
    public static readonly string WHE001 = "WH#E001";

    #endregion
}
