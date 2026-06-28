namespace WSIMS_ERP.Shared.Models.PageSetting;

public class PageSettingModel
{
    public static IEnumerable<int> PageSizeOptions { get; } = new int[] { 10, 20, 30 };
    public PageSettingModel() { }
    public PageSettingModel(int pageNo, int pageSize)
    {
        PageNo = pageNo;
        PageSize = pageSize;
    }
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int SkipRowCount => (PageNo - 1) * PageSize;
    public int TotalPageCount { get; set; }
    public PageSettingResponseModel Change(int totalRowCount)
    {
        return new PageSettingResponseModel()
        {
            PageNo = PageNo,
            RowCount = PageSize,
            TotalRowCount = totalRowCount
        };
    }

    public PageSettingModel(int totalRowCount)
    {
        var totalRow = totalRowCount / PageSize;
        var result = totalRowCount % PageSize;
        if (result > 0)
        {
            totalRow += 1;
        }
        TotalPageCount = totalRow;
    }
}
