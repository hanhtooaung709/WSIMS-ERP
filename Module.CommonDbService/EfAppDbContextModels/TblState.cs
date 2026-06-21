using System;
using System.Collections.Generic;

namespace Module.CommonDbService.EfAppDbContextModels;

public partial class TblState
{
    public string StateId { get; set; } = null!;

    public string StateCode { get; set; } = null!;

    public string StateName { get; set; } = null!;

    public byte[] CreatedUserId { get; set; } = null!;

    public DateTime CreatedDateTime { get; set; }

    public byte[]? ModifiedUserId { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public int DelFlag { get; set; }
}
