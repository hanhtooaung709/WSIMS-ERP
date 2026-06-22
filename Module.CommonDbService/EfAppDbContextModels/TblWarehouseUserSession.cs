using System;
using System.Collections.Generic;

namespace Module.CommonDbService.EfAppDbContextModels;

public partial class TblWarehouseUserSession
{
    public string SessionId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string SessionToken { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime LoginTime { get; set; }

    public DateTime LogoutTime { get; set; }
}
