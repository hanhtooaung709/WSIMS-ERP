using System;
using System.Collections.Generic;

namespace Module.CommonDbService.EfAppDbContextModels;

public partial class TblDistrict
{
    public string DistrictId { get; set; } = null!;

    public string DistrictCode { get; set; } = null!;

    public string DistrictName { get; set; } = null!;

    public string StateCode { get; set; } = null!;

    public string CreatedUserId { get; set; } = null!;

    public DateTime CreatedDateTime { get; set; }

    public string? ModifiedUserId { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public int DelFlag { get; set; }
}
