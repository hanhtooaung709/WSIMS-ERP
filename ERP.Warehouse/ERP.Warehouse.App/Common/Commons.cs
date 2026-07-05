using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Enums;
using WSIMS_ERP.Shared.Models;

namespace ERP.Warehouse.App.Common;

public class Commons
{
    public static List<SelectListModel> GetStatusList()
    {
        List<SelectListModel> lstStatus = new List<SelectListModel>
        {
            new()
            {
                Value = EnumRequestedStatus.Approved.ToString(),
                Text = EnumRequestedStatus.Approved.GetEnumDescription()
            },
            new()
            {
                Value = EnumRequestedStatus.Rejected.ToString(),
                Text = EnumRequestedStatus.Rejected.GetEnumDescription()
            },
            new()
            {
                Value = EnumRequestedStatus.Pending.ToString(),
                Text = EnumRequestedStatus.Pending.GetEnumDescription()
            },
        };
        return lstStatus;
    }
}
