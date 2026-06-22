using System;
using System.Collections.Generic;

namespace Module.CommonDbService.EfAppDbContextModels;

public partial class TblCity
{
    public string CityId { get; set; } = null!;

    public string CityCode { get; set; } = null!;

    public string CityName { get; set; } = null!;

    public string StateCode { get; set; } = null!;

    public string CreatedUserId { get; set; } = null!;

    public DateTime CreatedDateTime { get; set; }

    public string? ModifiedUserId { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public int DelFlag { get; set; }
}
