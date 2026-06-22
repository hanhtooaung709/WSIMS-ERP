using System;
using System.Collections.Generic;

namespace Module.CommonDbService.EfAppDbContextModels;

public partial class TblTownship
{
    public string TownshipId { get; set; } = null!;

    public string TownshipCode { get; set; } = null!;

    public string TownshipName { get; set; } = null!;

    public string CityCode { get; set; } = null!;

    public string CreatedUserId { get; set; } = null!;

    public DateTime CreatedDateTime { get; set; }

    public string? ModifiedUserId { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public int DelFlag { get; set; }
}
