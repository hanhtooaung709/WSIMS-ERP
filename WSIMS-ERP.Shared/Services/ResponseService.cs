using Microsoft.EntityFrameworkCore;
using Module.CommonDbService.EfAppDbContextModels;

namespace WSIMS_ERP.Shared.Services;

public class ResponseService : IResponseService
{
    private readonly AppDbContext _db;

    public ResponseService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<string> GetResponseData(string respCode , string resDesp)
    {
        string translation = resDesp;
        if (respCode.IsNullOrEmpty())
        {
            return translation;
        }
        if (respCode.StartsWith("S000"))
        {
            translation = "Success";
        }
        if (respCode.StartsWith("E999"))
        {
            translation = "Fail";
        }
        if (respCode.StartsWith("WH"))
        {
            translation = await GetWarehouseResponseCode(respCode);
        }
        return translation;
    }

    public async Task<string> GetWarehouseResponseCode(string respCode)
    {
        var message = await _db.TblWarehouseResponseCodes
            .AsNoTracking()
            .Where(x => x.LanguageCode == respCode)
            .Select(x => x.Translation)
            .FirstOrDefaultAsync();
        return message ?? string.Empty;
    }
}

public interface IResponseService
{
    Task<string> GetResponseData(string respCode, string resDesp);
    Task<string> GetWarehouseResponseCode(string respCode);
}
