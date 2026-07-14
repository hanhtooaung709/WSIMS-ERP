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
    ///Box is successfully Deteted.
    /// </summary>
    public static readonly string WHS062 = "WH#S062";

    /// <summary>
    /// Box delete fail!
    /// </summary>
    public static readonly string WHE061 = "WH#E061";

    /// <summary>
    /// Box is successfully Updated.
    /// </summary>
    public static readonly string WHS060 = "WH#S060";

    /// <summary>
    /// Box does not exist!
    /// </summary>
    public static readonly string WHE059 = "WH#E059";

    /// <summary>
    /// Box is successfully Created.
    /// </summary>
    public static readonly string WHS058 = "WH#S058";

    /// <summary>
    /// Box Type is already exist!
    /// </summary>
    public static readonly string WHE057 = "WH#E057";

    /// <summary>
    /// Box Code is already exist!
    /// </summary>
    public static readonly string WHE056 = "WH#E056";

    /// <summary>
    /// Currency is successfully Deteted.
    /// </summary>
    public static readonly string WHS055 = "WH#S055";

    /// <summary>
    /// Currency delete fail!
    /// </summary>
    public static readonly string WHE054 = "WH#E054";

    /// <summary>
    /// Currency is successfully Updated.
    /// </summary>
    public static readonly string WHS053 = "WH#S053";

    /// <summary>
    /// Currency does not exist!
    /// </summary>
    public static readonly string WHE052 = "WH#E052";

    /// <summary>
    /// Currency is successfully Created.
    /// </summary>
    public static readonly string WHS051 = "WH#S051";

    /// <summary>
    /// Currency Description is already exist!
    /// </summary>
    public static readonly string WHE050 = "WH#E050";

    /// <summary>
    /// Currency Code is already exist!
    /// </summary>
    public static readonly string WHE049 = "WH#E049";

    /// <summary>
    /// Requsted Product is successfully Deteted.
    /// </summary>
    public static readonly string WHS048 = "WH#S048";

    /// <summary>
    /// Requsted Change Product delete fail!
    /// </summary>
    public static readonly string WHE047 = "WH#E047";

    /// <summary>
    /// Requested Change Product is successfully Updated.
    /// </summary>
    public static readonly string WHS046 = "WH#S046";

    /// <summary>
    /// Requseted Change Product is not pending status!
    /// </summary>
    public static readonly string WHE045 = "WH#E045";

    /// <summary>
    /// Requested Change Product does not exist!
    /// </summary>
    public static readonly string WHE044 = "WH#E044";

    /// <summary>
    /// Requsted Product is successfully Deteted.
    /// </summary>
    public static readonly string WHS043 = "WH#S043";

    /// <summary>
    /// Requsted Product delete fail!
    /// </summary>
    public static readonly string WHE042 = "WH#E042";

    /// <summary>
    /// Requseted Product is not pending status!
    /// </summary>
    public static readonly string WHE041 = "WH#E041";

    /// <summary>
    /// Requested Product is successfully updated.
    /// </summary>
    public static readonly string WHS040 = "WH#S040";

    /// <summary>
    /// Requseted User is not pending status!
    /// </summary>
    public static readonly string WHE039 = "WH#E039";

    /// <summary>
    /// Requested Product does not exist!
    /// </summary>
    public static readonly string WHE038 = "WH#E038";

    /// <summary>
    /// Product is already Change Requested!
    /// </summary>
    public static readonly string WHE037 = "WH#E037";

    /// <summary>
    /// Product is already Requested!
    /// </summary>
    public static readonly string WHE036 = "WH#E036";

    /// <summary>
    /// Product does not exist!
    /// </summary>
    public static readonly string WHE035 = "WH#E035";

    /// <summary>
    /// Product Code is already Change Requested!
    /// </summary>
    public static readonly string WHE034 = "WH#E034";

    /// <summary>
    /// Product Code is already Requested!
    /// </summary>
    public static readonly string WHE033 = "WH#E033";

    /// <summary>
    /// Product Code is already exist!
    /// </summary>
    public static readonly string WHE032 = "WH#E032";

    /// <summary>
    /// Product Name is already Change Requested!
    /// </summary>
    public static readonly string WHE031 = "WH#E031";

    /// <summary>
    /// Product Name is already Requested!
    /// </summary>
    public static readonly string WHE030 = "WH#E030";

    /// <summary>
    /// Product Name is already exist!
    /// </summary>
    public static readonly string WHE029 = "WH#E029";

    /// <summary>
    /// Requsted Change User is successfully Deteted.
    /// </summary>
    public static readonly string WHS028 = "WH#S028";

    /// <summary>
    /// Requsted Change User delete fail!
    /// </summary>
    public static readonly string WHE027 = "WH#E027";

    /// <summary>
    /// Requested Change User is successfully Updated.
    /// </summary>
    public static readonly string WHS026 = "WH#S026";

    /// <summary>
    /// Requseted Change User is not pending status!
    /// </summary>
    public static readonly string WHE025 = "WH#E025";

    /// <summary>
    /// Requseted Change User does not exist!
    /// </summary>
    public static readonly string WHE024 = "WH#E024";

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
