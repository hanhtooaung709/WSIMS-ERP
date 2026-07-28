namespace WSIMS_ERP.Shared.Enums;

public enum EnumRequestedType
{
    [Description("Register")]
    Register,
    [Description("Update")]
    Update,
    [Description("ResetPassword")]
    ResetPassword,
    [Description("Delete")]
    Delete,
    [Description("Transfer")]
    Transfer
}
