using System;
using System.Collections.Generic;

namespace Module.CommonDbService.EfAppDbContextModels;

public partial class TblBox
{
    public string BoxId { get; set; } = null!;

    public string BoxCode { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string Size { get; set; } = null!;

    public int TareWeight { get; set; }

    public int MaxNetWeight { get; set; }

    public string CreatedUserId { get; set; } = null!;

    public string CreatedDateTime { get; set; } = null!;

    public string ModifiedUserId { get; set; } = null!;

    public DateTime ModifiedDateTime { get; set; }

    public int DelFlag { get; set; }
}
