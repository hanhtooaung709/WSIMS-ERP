namespace ERP.Warehouse.App.Services.InjectionService;

public class DialogParameterModel
{
    public DialogParameterModel(string name, object value)
    {
        Name = name;
        Value = value;
    }

    public string Name { get; set; }
    public object Value { get; set; }
}
