using System;
using System.Collections.Generic;

namespace Module.CommonDbService.EfAppDbContextModels;

public partial class TblWarehouseResponseCode
{
    public int WarehouseResponseCodeId { get; set; }

    public int ResourceId { get; set; }

    public string LanguageCode { get; set; } = null!;

    public string Translation { get; set; } = null!;
}
