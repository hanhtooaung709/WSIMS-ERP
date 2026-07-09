using System;
using System.Collections.Generic;

namespace Module.CommonDbService.EfAppDbContextModels;

public partial class TblBox
{
    public string BoxId { get; set; } = null!;

    public string BoxCode { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string Size { get; set; } = null!;

    public string TareWeight { get; set; } = null!;

    public string MaxNetWeight { get; set; } = null!;

    public string CreatedUserId { get; set; } = null!;

    public DateTime CreatedDateTime { get; set; }

    public string? ModifiedUserId { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public int DelFlag { get; set; }
}
