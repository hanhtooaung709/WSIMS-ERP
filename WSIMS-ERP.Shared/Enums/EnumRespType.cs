using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSIMS_ERP.Shared.Enums;

public enum EnumRespType
{
    [Description("None")] None,
    [Description("Success")] Success,
    [Description("Information")] MI,
    [Description("Warning")] Warning,
    [Description("Error")] Error,
    [Description("System Error")] SystemError
}
