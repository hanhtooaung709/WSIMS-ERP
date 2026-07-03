namespace WSIMS_ERP.Shared.Models.DynamicModel;

public static class ExtensionModel
{
    public static void Add(this List<DynamicReportModel>? lstData, string key, string value)
    {
        lstData.Add(new DynamicReportModel
        {
            Key = key,
            Value = value
        });
    }
}
