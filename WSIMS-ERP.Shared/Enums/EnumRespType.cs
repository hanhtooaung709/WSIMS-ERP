using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSIMS_ERP.Shared.Enums;

public enum EnumRespType
{
    [Description("Success")] Success,
    [Description("Error")] Error,
    [Description("Warning")] Warning,
    [Description("System Error")] SystemError
}
