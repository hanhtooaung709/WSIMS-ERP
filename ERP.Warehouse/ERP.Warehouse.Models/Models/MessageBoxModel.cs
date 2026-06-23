using Microsoft.AspNetCore.Components;
using WSIMS_ERP.Shared.Enums;

namespace ERP.Warehouse.Models.Models;

public class MessageBoxModel
{
    public string Title { get; set; }
    public string Message { get; set; }
    public MarkupString MessageHtml { get; set; }
    public EnumRespType MessageType { get; set; }
    public EnumMessageBoxType MessageBoxType { get; set; }
    public string? Url { get; set; }
    public bool Reload { get; set; }
}

public class MarkupMessageBoxModel
{
    public string Title { get; set; }
    public MarkupString Message { get; set; }
    public EnumRespType MessageType { get; set; }
    public EnumMessageBoxType MessageBoxType { get; set; }
    public string? Url { get; set; }
    public bool Reload { get; set; }
}
