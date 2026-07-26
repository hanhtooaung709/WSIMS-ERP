namespace WSIMS_ERP.Shared.Enums;

public enum EnumFormType
{
    None = 0,
    List,
    [Description("Save")]
    Create,
    [Description("Update")]
    Edit,
    [Description("Register")]
    Register,
    [Description("Detail")]
    Detail,
    Delete,
    [Description("StockModifly")]
    StockModifly,
    [Description("StockTransfer")]
    StockTransfer,
}
