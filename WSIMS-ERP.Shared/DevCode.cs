namespace WSIMS_ERP.Shared;

public static class DevCode
{
    public static string FixPath(string path)
    {
        return path.Replace("/", Path.DirectorySeparatorChar.ToString())
            .Replace("\\", Path.DirectorySeparatorChar.ToString())
            .TrimEnd(Path.DirectorySeparatorChar);
    }

    public static string GetImagePath(string serverMapPath, string folderMapPath, out string fileName)
    {
        fileName = DevCode.GenerateUlid() + ".jpg";
        return Path.Combine(serverMapPath, folderMapPath, fileName);
    }

    public static T ToEnum<T>(this int val)
    {
        return (T)Enum.Parse(typeof(T), val.ToString(), true);
    }

    public static bool Base64ToFile1(string base64String, string filePath)
    {
        // Convert base 64 string to byte[]
        byte[] imageBytes = Convert.FromBase64String(base64String.Replace(" ", "+"));
        File.WriteAllBytes(filePath, imageBytes);
        return true;
    }

    public static bool WriteBase64ToFile(string base64String, string filePath)
    {
        try
        {
            // Strip base64 prefix if present (e.g., "data:image/jpeg;base64,")
            var data = base64String.Contains(",") ? base64String.Split(',')[1] : base64String;

            byte[] bytes = Convert.FromBase64String(data);
            File.WriteAllBytes(filePath, bytes);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WriteBase64ToFile Error] {ex.Message}");
            return false;
        }
    }

    public static async Task<bool> WriteBase64ToFileAsync(string base64String, string filePath)
    {
        try
        {
            // Strip base64 prefix if present (e.g., "data:image/jpeg;base64,")
            var data = base64String.Contains(",") ? base64String.Split(',')[1] : base64String;

            byte[] bytes = Convert.FromBase64String(data);
            await File.WriteAllBytesAsync(filePath, bytes);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WriteBase64ToFile Error] {ex.Message}");
            return false;
        }
    }


    public static string GetTimestamp(DateTime time)
    {
        return time.ToString("yyyyMMddHHmmss");
    }

    public static string GetFullImagePath(this string fileName, string serverPath)
    {
        string imagePath = "";
        if (!serverPath.IsNullOrEmpty())
        {
            if (!fileName.IsNullOrEmpty())
            {
                if (serverPath[serverPath.Length - 1].Equals('/') && fileName[0].Equals('/'))
                {
                    fileName = fileName.Remove(0, 1);
                }
            }

            imagePath = serverPath + fileName;
        }

        return imagePath.Replace(@"\", @"/");
    }

    public static bool IsPdf(this string base64String)
    {
        // Remove any data type prefixes, such as "data:application/pdf;base64,"
        const string prefixToRemove = "data:application/pdf;base64,";
        if (base64String.StartsWith(prefixToRemove, StringComparison.OrdinalIgnoreCase))
        {
            base64String = base64String.Substring(prefixToRemove.Length);
        }

        var binaryData = Convert.FromBase64String(base64String);
        return binaryData is [0x25, _, _, _, _, ..] && // %
               binaryData[1] == 0x50 && // P
               binaryData[2] == 0x44 && // D
               binaryData[3] == 0x46 && // F
               binaryData[4] == 0x2D;
    }

    public static decimal HexToDecimal(this string hex)
    {
        decimal decimalNumber = 0;

        // Loop through each character in the hexadecimal string
        for (int i = hex.Length - 1; i >= 0; i--)
        {
            char hexDigit = hex[i];

            // Convert hexadecimal digit to decimal value
            int decimalValue = 0;
            if (hexDigit >= '0' && hexDigit <= '9')
            {
                decimalValue = hexDigit - '0';
            }
            else if (hexDigit >= 'A' && hexDigit <= 'F')
            {
                decimalValue = hexDigit - 'A' + 10;
            }
            else if (hexDigit >= 'a' && hexDigit <= 'f')
            {
                decimalValue = hexDigit - 'a' + 10;
            }

            // Add the decimal value to the result
            decimalNumber += decimalValue * (int)Math.Pow(16, hex.Length - 1 - i);
        }

        return decimalNumber;
    }

    public static bool IsSameDigit(this string input)
    {
        return input.Distinct().Count() == 1;
    }

    public static bool IsSequential(this string input)
    {
        for (int i = 1; i < input.Length; i++)
        {
            if (input[i] - input[i - 1] != 1)
            {
                return false;
            }
        }

        return true;
    }

    public static bool IsDigitsOnly(string str)
    {
        return str.All(c => c is >= '0' and <= '9');
    }

    public static string ToCheckPasswordPolicy_DateOfBirth(this string str)
    {
        string resString = str;
        if (str.IsMyanmarMobileNo())
        {
            resString = str.Replace(MyanmarMobilePrefix, "");
        }

        if (str.IsMyanmarMobileNoPrefix())
        {
            resString = str.Replace(MyanmarMobileNoPrefix, "");
        }

        if (resString.Contains("-"))
        {
            resString = resString.Replace("-", "");
        }

        return resString;
    }

    public static string GetDesp<T>(this T val)
    {
        return val.GetEnumDescription();
    }

    public static long ToLong(this string str)
    {
        return Convert.ToInt64(str);
    }

    public static bool RangeInt32(this string no)
    {
        long number = no.ToLong();
        return number >= Int32.MinValue && number <= Int32.MaxValue;
    }

    public static bool IsBetween(this DateTime date, DateTime from, DateTime to)
    {
        return date >= from && date <= to;
    }

    #region DateTime

    public static bool IsDefaultDate(this DateTime dt)
    {
        return dt == default(DateTime) || dt == new DateTime(1990, 1, 1);
    }

    public static DateTime ToDateTimeyyyyMMdd(this string str)
    {
        string[] strAr;

        if (str.Contains('-'))
        {
            strAr = str.Split('-');
        }
        else
        {
            strAr = str.Split('/');
        }

        return Convert.ToDateTime($"{strAr[2]}-{strAr[1]}-{strAr[0]}");
    }

    public static DateTime TransactionDateFormat(this string str)
    {
        string[] strAr;

        int lastSpaceIndex = str.LastIndexOf(' ');
        string datePart = str.Substring(0, lastSpaceIndex);
        _ = str.Substring(lastSpaceIndex + 1);

        if (datePart.Contains('-'))
        {
            strAr = datePart.Split('-');
        }
        else
        {
            strAr = datePart.Split('/');
        }

        return Convert.ToDateTime($"{strAr[2]}-{strAr[1]}-{strAr[0]}");
    }

    #endregion

    #region Phone No

    private static readonly string MyanmarMobilePrefix = "+959-";
    private static readonly string MyanmarMobileNoPrefix = "+959";

    public static bool IsMyanmarMobileNo(this string str)
    {
        if (str.IsNullOrEmpty())
        {
            return false;
        }

        return str.Contains(MyanmarMobilePrefix);
    }

    public static bool IsMyanmarMobileNoPrefix(this string str)
    {
        if (str.IsNullOrEmpty())
        {
            return false;
        }

        return str.Contains(MyanmarMobileNoPrefix);
    }

    public static string AddPrefixNumberFromMobileNoMM(this string str)
    {
        if (!str.IsNullOrEmpty())
        {
            // if (str.Contains(myanmarMobilePrefix))
            // {
            //     str = myanmarMobilePrefix + str;
            // }
            if (str.StartsWith("09"))
            {
                _ = str.RemovePrefixPhoneNumber();
            }

            bool res = IsMyanmarMobileNoPrefix(str);
            if (!res)
            {
                str = MyanmarMobilePrefix + str;
            }
        }

        return str;
    }

    public static string RemovePrefixNumberFromMobileNoMM(this string str)
    {
        if (str.IsMyanmarMobileNo())
        {
            return _ = str.Replace(MyanmarMobilePrefix, "");
        }

        if (str.IsMyanmarMobileNoPrefix())
        {
            return _ = str.Replace(MyanmarMobileNoPrefix, "");
        }

        string res = str.RemovePrefixPhoneNumber();

        if (res.Contains("-"))
        {
            res = res.Replace("-", "");
        }

        return res;
    }

    private const string MobilePrefix = "09";

    public static string AddPrefixPhoneNumber(this string phone)
    {
        if (string.IsNullOrEmpty(phone))
        {
            return phone;
        }

        if (phone.StartsWith("+959"))
        {
            phone = phone.Substring(4); // Remove the "+959" prefix
        }

        if (phone.Contains(")"))
        {
            int index = phone.IndexOf(')');
            phone = phone.Substring(index + 1); // Remove the "+95(9)"
        }

        if (phone.StartsWith("+9509"))
        {
            phone = phone.Substring(5); // Remove the "+9509" prefix
        }

        if (!phone.StartsWith("09"))
        {
            phone = "09" + phone; // Add "09" prefix if not already present
        }

        return phone;
    }


    public static string RemovePrefixPhoneNumber(this string phone)
    {
        string res = phone;
        if (phone.StartsWith(MobilePrefix))
        {
            res = phone.Substring(2);
        }

        return res;
    }

    #endregion

    #region Log

    //public static void LogError(this ILogger logger,
    //    Exception ex,
    //    [CallerFilePath] string filePath = "",
    //    [CallerMemberName] string methodName = "")
    //{
    //    var fileName = Path.GetFileName(filePath);
    //    var message = $"File Name - {fileName} | Method Name - {methodName} | Error - {ex.ToJson()}";
    //    logger.LogCustomError(message);
    //}

    public static void LogCustomError(this ILogger logger,
        Exception ex,
        [CallerFilePath] string filePath = "",
        [CallerMemberName] string methodName = "")
    {
        // Suppress expected exceptions during circuit disconnection or task cancellation
        if (ex.GetType().Name == "JSDisconnectedException" || ex.GetType().Name == "TaskCanceledException")
            return;

        var fileName = Path.GetFileName(filePath);
        var message =
            $"File Name - {fileName} | Method Name - {methodName} | Result - {ex}";
        logger.LogError(message);
    }

    public static void LogCustomInformation(this ILogger logger,
        object str,
        [CallerFilePath] string filePath = "",
        [CallerMemberName] string methodName = "")
    {
        var fileName = Path.GetFileName(filePath);
        var message =
            $"File Name - {fileName} | Method Name - {methodName} | Result - {(str is string ? str : str.ToJson())}";
        logger.LogInformation(message);
    }

    #endregion

    public static T ToEntityItem<T>(this object obj)
    {
        T res = default(T)!;

        if (!obj.IsNullOrEmpty() && !string.IsNullOrEmpty(obj.ToString()) && obj is string)
        {
            res = (T)Convert.ChangeType(obj.ToString()!.Trim(), typeof(T));
        }
        else if (!obj.IsNullOrEmpty() && !string.IsNullOrEmpty(obj.ToString()))
        {
            res = (T)Convert.ChangeType(obj, typeof(T));
        }

        return res;
    }

    public static T CheckData<T>(string colName, DataRow dr, T res, bool isDefault = true)
    {
        T empty = default(T)!;
        try
        {
            if (!dr.Table.Columns.Contains(colName))
            {
                return empty;
            }

            object value = dr[colName];
            if (value.Equals(DBNull.Value) || value.IsNullOrEmpty())
            {
                return empty;
            }

            return string.IsNullOrEmpty(value.ToString())
                ? (isDefault ? res : empty)
                : (T)Convert.ChangeType(Convert.ToString(value)!.Trim(), typeof(T));
        }
        catch (Exception ex)
        {
            _ = ex.Message;
            return empty;
        }
    }

    public static decimal ToDecimal(this decimal? val)
    {
        return Convert.ToDecimal(val);
    }

    public static JObject ToJObject(this object? obj)
    {
        return JObject.FromObject(obj!);
    }

    public static T? ToObject<T>(this string? jsonStr)
    {
        try
        {
            if (!string.IsNullOrEmpty(jsonStr))
            {
                if (typeof(T) == typeof(string) && !jsonStr.TrimStart().StartsWith("\""))
                {
                    jsonStr = $"\"{jsonStr}\"";
                }

                var result = JsonConvert.DeserializeObject<T>(jsonStr,
                    new JsonSerializerSettings { DateParseHandling = DateParseHandling.DateTimeOffset });

                return result;
            }
            //if (jsonStr != null)
            //{
            //    var test = JsonConvert.DeserializeObject<T>(jsonStr,
            //        new JsonSerializerSettings { DateParseHandling = DateParseHandling.DateTimeOffset });
            //    return test;
            //}
        }
        catch
        {
            return (T)Convert.ChangeType(jsonStr, typeof(T))!;
        }

        return default;
    }

    public static object? ToObject(this string? jsonStr)
    {
        return ToObject<object>(jsonStr);
    }

    public static JObject ConvertJObject(this string? jsonStr)
    {
        return JObject.Parse(jsonStr!);
    }

    public static string? ToJson<T>(this T? obj, bool format = false)
    {
        if (obj == null)
        {
            return string.Empty;
        }

        string? result;
        if (obj is string)
        {
            result = obj.ToString();
            goto Result;
        }

        var settings = new JsonSerializerSettings
        {
            DateFormatString = "yyyy-MM-ddTHH:mm:ss.sssZ",
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        result = format
            ? JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented, settings)
            : JsonConvert.SerializeObject(obj, settings);
    Result:
        return result;
    }

    public static async Task<string> ToStringFromStream(this Stream value)
    {
        using var reader = new StreamReader(value, Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }

    public static bool IsNullOrEmpty(this object? str)
    {
        var result = true;
        try
        {
            result = str == null ||
                     string.IsNullOrEmpty(str.ToString()?.Trim()) ||
                     string.IsNullOrWhiteSpace(str.ToString()?.Trim());
        }
        catch
        {
            // ignored
        }

        return result;
    }

    public static T ToEnum<T>(this string? value) where T : Enum
    {
        try
        {
            return (T)Enum.Parse(typeof(T), value!, true);
        }
        catch
        {
            return (T)Enum.ToObject(typeof(T), 0);
        }
    }

    public static string ToFormat(this string str, params object?[] parameters)
    {
        return parameters.Length == 0 ? str : string.Format(str, parameters);
    }

    public static string GetEnumDescription<T>(this T val)
    {
        DescriptionAttribute[] attributes = (DescriptionAttribute[])val
            .GetType()
            .GetField(val.ToString()!)
            .GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : string.Empty;
    }

    public static string GetError(this Exception ex)
    {
        return ex.Message + ex.StackTrace;
    }

    public static JToken ToJToken(this object obj)
    {
        return JToken.FromObject(obj);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string GetCurrentMethod()
    {
        var st = new StackTrace();
        var sf = st.GetFrame(1);

        return sf!.GetMethod()!.Name;
    }

    public static DateTime GetServerDateTime()

    {
        // return _DevCode.GetServerDateTime();
        var timeUtc = DateTime.UtcNow;
        return TimeZoneInfo.ConvertTimeFromUtc(timeUtc, GetMyanmarTimeZoneInfo());
    }

    public static DateTime ParseExactDateTime(this string dateTime)

    {
        var dt = DateTime.ParseExact(dateTime, "d/M/yyyy", CultureInfo.InvariantCulture);
        return dt;
    }

    public static TimeZoneInfo GetMyanmarTimeZoneInfo()
    {
        return TimeZoneInfo.FindSystemTimeZoneById("Myanmar Standard Time");
    }

    public static string ToPrettyXml(this string xml)
    {
        try
        {
            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(xml);

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                element.Save(xmlWriter);
            }

            return stringBuilder.ToString();
        }
        catch (Exception)
        {
            // Handle and throw if fatal exception here; don't just ignore them
            return xml;
        }
    }

    public static int ToInt32(this string str)
    {
        if (str.IsNullOrEmpty())
        {
            return 0;
        }

        return Convert.ToInt32(str);
    }

    public static decimal ToDecimal(this object value)
    {
        return Convert.ToDecimal(value);
    }

    public static DateTime ToDateTime(this object? dt)
    {
        return dt == null ? default(DateTime) : Convert.ToDateTime(dt);
    }

    public static string ToDateTimeISO8601(this DateTime dt)
    {
        return dt.ToString("yyyy-MM-ddTHH:mm:ss.sssZ");
    }

    public static string AddHHMMSS(this DateTime dateTime)
    {
        string dateTimeStr = dateTime.ToString(_Format.DateFormat6);
        DateTime datetimeNow = GetServerDateTime();
        String formattedDateTime = dateTimeStr + " " + datetimeNow.ToString("HH:mm:ss.ffffff");
        return formattedDateTime;
    }

    public static int ToInt32(this object? str)
    {
        return str == null ? 0 : Convert.ToInt32(str);
    }

    public static bool ToBool(this object? obj)
    {
        if (obj == null || obj.ToString().IsNullOrEmpty())
        {
            return false;
        }

        var str = obj.ToString()!.ToLower();
        return str switch
        {
            "true" or "on" or "yes" or "1" => true,
            "false" or "off" or "no" or "0" => false,
            _ => false
        };
    }

    public static decimal ToDecimal(this string? str)
    {
        decimal res = 0;
        try
        {
            if (str == null)
            {
                goto Result;
            }

            res = Decimal.Parse(str, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint);
        }
        catch (Exception ex)
        {
            _ = ex.Message;
            if (str != null && str.Contains(","))
            {
                try
                {
                    str = str.ToEntityItem<decimal>().ToEntityItem<string>();
                    res = Decimal.Parse(str, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint);
                }
                catch (Exception ex1)
                {
                    _ = ex1.Message;
                }
            }
        }

    Result:
        return res;
    }

    public static decimal ToHex2Decimal(this string str)
    {
        return Convert.ToInt64(str, 16);
    }

    public static string ToBase64Encode(this string plainText)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }

    public static string ToBase64Decode(this string? base64EncodedData)
    {
        var base64EncodedBytes =
            Convert.FromBase64String(base64EncodedData ?? throw new ArgumentNullException(nameof(base64EncodedData)));
        return Encoding.UTF8.GetString(base64EncodedBytes);
    }

    public static string ToEncryptApiData(this string str, string key, string iv)
    {
        return EncryptProvider.AESEncrypt(str, key, iv);
    }

    public static string ToDecryptApiData(this string str, string key, string iv)
    {
        try
        {
            return EncryptProvider.AESDecrypt(str, key, iv);
        }
        catch
        {
            return str;
        }
    }

    public static string ToEnumCode<T>(this T val)
    {
        return Convert.ToInt64(val).ToString();
    }

    public static string GetServerDateTimeISO8601()
    {
        return GetServerDateTime().ToString(Format(_Format.DateFormat1));
    }


    public static T CheckEntityItem<T>(this object? obj)
    {
        var res = default(T);

        if (obj != null && !string.IsNullOrEmpty(obj.ToString()) && obj is string)
        {
            res = (T)Convert.ChangeType(obj.ToString()!.Trim(), typeof(T));
        }
        else if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
        {
            res = (T)Convert.ChangeType(obj, typeof(T));
        }

        return res!;
    }

    public static string Format(this string str, params object[] param)
    {
        return string.Format(str, param);
    }

    public static string UrlDecode(this string data)
    {
        return HttpUtility.UrlDecode(data);
    }

    public static void DeleteImage(this string imageNameUrl)
    {
        var fullPath = imageNameUrl;
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    public static string ToRemoveComma(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        if (str.EndsWith(","))
        {
            return str.Substring(0, str.Length - 1);
        }

        return str;
    }

    public static string GetInternalImageUrl(this string imageGuid, string profileImageUrl)
    {
        //string l_FolderPath = Path.Combine(WebHostEnvironmentCommon.ContentRootPath, "FileDownload");
        _ = Directory.CreateDirectory(profileImageUrl);
        return profileImageUrl + imageGuid;
    }

    public static string GetNewGUID()
    {
        return Guid.NewGuid().ToString().ToUpper();
    }

    public static string GenerateOTP()
    {
        var buffer = Encoding.ASCII.GetBytes(Guid.NewGuid().ToString().Replace("-", ""));
        var confirmationCode = BitConverter.ToInt32(buffer, 0).ToString();
        confirmationCode = confirmationCode.Substring(0, 6);

        return confirmationCode;
    }

    public static string GenerateCaptcha()
    {
        var buffer = Encoding.ASCII.GetBytes(Guid.NewGuid().ToString().Replace("-", ""));
        var confirmationCode = BitConverter.ToInt32(buffer, 0).ToString();
        confirmationCode = confirmationCode.Substring(0, 6);

        return confirmationCode;
    }

    public static T ChangeEnum<T>(this string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static string ToThousandSeparatorWith2Decimal(this decimal amount)
    {
        return amount.ToString(_Format.NumberPattern1);
    }

    public static string GetEnumCode<T>(this T val)
    {
        return Convert.ToInt64(val).ToString();
    }

    public static string ToBalanceHash(this string? balance, string hashString)
    {
        balance = balance!.Trim();
        hashString = hashString.Trim();

        string saltedCode = EncodedBySalted(hashString); //username
        string hashValue;
        using (var sha256 = SHA256.Create())
        {
            var hash = sha256.ComputeHash(Encoding.Default.GetBytes(balance + saltedCode));
            hashValue = ToHex(hash, false);
        }

        return hashValue;
    }

    public static string ToSHA256HexHashString(this string password, string mobileNo)
    {
        password = password.Trim();
        mobileNo = mobileNo.Trim();
        string saltedCode = EncodedBySalted(mobileNo); //salted user name
        string hashString;
        using (var sha256 = SHA256Managed.Create())
        {
            var hash = sha256.ComputeHash(Encoding.Default.GetBytes(password + saltedCode));
            hashString = ToHex(hash, false);
        }

        return hashString;
    }

    private static string EncodedBySalted(string decodeString)
    {
        decodeString = decodeString.ToLower()
            .Replace("a", "@")
            .Replace("i", "!")
            .Replace("l", "1")
            .Replace("e", "3")
            .Replace("o", "0")
            .Replace("s", "$")
            .Replace("n", "&");
        return decodeString;
    }

    public static string ToHex(byte[] bytes, bool upperCase)
    {
        StringBuilder result = new StringBuilder(bytes.Length * 2);
        for (int i = 0; i < bytes.Length; i++)
        {
            _ = result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));
        }

        return result.ToString();
    }

    public static object ToMimeType(this string fileName)
    {
        return MimeTypesMap.GetMimeType(fileName);
    }

    public static IQueryable<T> Pagination<T>(this IQueryable<T> query, int pageNo, int pageSize)
    {
        var skipRow = (pageNo - 1) * pageSize;
        return query.Skip(skipRow).Take(pageSize);
    }

    #region Encrypt/Decrypt

    private static byte[] _key = "C4162ECDB4594969BB1040E846869706".ToByte();

    private static byte[] _iv = "AC507B7DAC7B458C".ToByte();

    public static byte[] ToByte(this string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }

    public static string ToEncrypt(this string? str)
    {
        if (str is null)
        {
            return string.Empty;
        }

        var encrypted = Encryption.Encrypt(str, _key, _iv);
        return encrypted;
    }

    public static string ToEncode(this string queryString)
    {
        return HttpUtility.UrlEncode(queryString);
    }

    public static string ToDecode(this string queryString)
    {
        return HttpUtility.UrlDecode(queryString);
    }

    public static string ToDecrypt(this string str)
    {
        string decrypted = "";
        try
        {
            if (!str.IsNullOrEmpty())
            {
                decrypted = Encryption.Decrypt(str, _key, _iv);
            }
        }
        catch (Exception)
        {
            decrypted = str;
        }

        return decrypted;
    }

    #endregion

    public static string ToUrlObject(this object obj)
    {
        string jsonStr;
        if (obj is string)
        {
            jsonStr = obj.ToString()!;
        }
        else
        {
            jsonStr = obj.ToJson()!;
        }

        string encryptedStr = jsonStr.ToEncrypt();
        string encodedStr = encryptedStr.ToEncode();
        return encodedStr;
    }

    public static T ToUrlObject<T>(this string str, bool isDelete = true)
    {
        string decodedStr = str;
        if (isDelete)
        {
            decodedStr = str.ToDecode();
        }

        string decryptedStr = decodedStr.ToDecrypt();
        T obj = decryptedStr.ToObject<T>()!;
        return obj;
    }

    public static string GetAppSettingByStage(this string stage)
    {
        var settingFileName = "customsetting";
        Enum stageType = stage.ToEnum<EnumStageType>();
        settingFileName = stageType switch
        {
            EnumStageType.Dev => settingFileName + ".json",
            EnumStageType.Sit => settingFileName + "-sit.json",
            EnumStageType.Uat => settingFileName + "-uat.json",
            EnumStageType.Prod1 => settingFileName + "-Prod1.json",
            EnumStageType.Prod2 => settingFileName + "-Prod2.json",
            _ => settingFileName + ".json"
        };
        return settingFileName;
    }

    public static bool IsLinux()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    }

    public static DateTime ToDateTimeddMMyyyy(this string str)
    {
        string[] strAr = str.Split('-');
        return Convert.ToDateTime($"{strAr[2]}-{strAr[1]}-{strAr[0]}");
    }

    public static DateTime GetDateTimeFromString(this string date, string format)
    {
        return DateTime.ParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None);
    }

    public static string ToRemovePrefixNumberFromMobileNoMM(this string str)
    {
        string res = str;
        if (str.IsMyanmarMobileNo())
        {
            res = str.Replace(MyanmarMobilePrefix, "");
        }

        if (str.IsMyanmarMobileNoPrefix())
        {
            res = str.Replace(MyanmarMobileNoPrefix, "");
        }

        if (res.Contains("-"))
        {
            res = res.Replace("-", "");
        }

        return res;
    }

    #region QR

    public static string StringToHex(this string stringVal)
    {
        var output = string.Empty;
        var stringCount = stringVal.Length;
        output += String.Format("{0:X2}", stringCount);
        char[] values = stringVal.ToCharArray();
        foreach (char letter in values)
        {
            // Get the integral value of the character.
            int value = Convert.ToInt32(letter);
            // Convert the decimal value to a hexadecimal value in string form.
            string hexOutput = String.Format("{0:X}", value);
            //  Console.WriteLine("Hexadecimal value of {0} is {1}", letter, hexOutput);
            output += hexOutput;
        }

        return output;
    }

    public static byte[] ConvertFromStringToHex(this string inputHex)
    {
        inputHex = inputHex.Replace(" ", "");

        byte[] resultantArray = new byte[inputHex.Length / 2];
        for (int i = 0; i < resultantArray.Length; i++)
        {
            resultantArray[i] = Convert.ToByte(inputHex.Substring(i * 2, 2), 16);
        }

        return resultantArray;
    }

    #endregion

    public static string AccountNoMasking(this string accountNo, char symbol)
    {
        string result = string.Empty;
        if (!accountNo.IsNullOrEmpty())
        {
            result = "".PadLeft(accountNo.Length - 4, symbol) + accountNo.Substring(accountNo.Length - 4, 4);
        }

        return result;
    }

    public static string AccountNoMiddleMasking(this string accountNo, char symbol)
    {
        string result = string.Empty;
        if (!accountNo.IsNullOrEmpty())
        {
            string firstNumber = accountNo.Substring(0, 4);
            string lastNumber = accountNo.Substring(accountNo.Length - 4, 4);
            result = firstNumber + "".PadLeft(accountNo.Length - 8, symbol) + lastNumber;
        }
        return result;
    }

    public static string ConvertToThousandSeparator(this string amount)
    {
        if (amount.IsNullOrEmpty())
        {
            return "0";
        }

        return Convert.ToDecimal(amount).ToString("#,##0.00");
    }

    public static bool CheckEmailFormat(this string email)
    {
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(email);
        if (match.Success)
        {
            return true;
        }

        return false;
    }

    public static bool IsValidEmail(this string emailAddress)
    {
        try
        {
            MailAddress m = new MailAddress(emailAddress);

            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public static string AsNoTracking(this string query)
    {
        return $@"SET SESSION TRANSACTION ISOLATION LEVEL READ UNCOMMITTED ;
                {query}
                SET SESSION TRANSACTION ISOLATION LEVEL REPEATABLE READ ;";
    }

    public static string ToExportType(this EnumExportType exportType)
    {
        string? exportFileType;
        switch (exportType)
        {
            case EnumExportType.Word:
                exportFileType = "docx";
                break;
            case EnumExportType.Excel:
                exportFileType = "csv";
                break;
            case EnumExportType.Pdf:
                exportFileType = "pdf";
                break;
            case EnumExportType.None:
            default:
                exportFileType = null;
                break;
        }

        return $".{exportFileType}";
    }

    public static string ToEnumDescription<T>(this T val)
    {
        DescriptionAttribute[] attributes = (DescriptionAttribute[])val
            .GetType()
            .GetField(val.ToString()!)
            .GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : string.Empty;
    }

    public static string DebitTransaction(this string amount)
    {
        if (!amount.IsNullOrEmpty())
        {
            amount = "- " + amount.ConvertToThousandSeparator();
        }

        return amount;
    }

    public static string CreditTransaction(this string amount)
    {
        if (!amount.IsNullOrEmpty())
        {
            amount = "+ " + amount.ConvertToThousandSeparator();
        }

        return amount;
    }

    public static string ConvertDecimalToZeroRemoveStr(this decimal? data)
    {
        return data.ToDecimal().ToString("0.#########################");
    }

    public static string ToDashFromNull(this object data)
    {
        string result;
        if (data.IsNullOrEmpty() || string.IsNullOrEmpty(data.ToString()!.Trim()) ||
            string.IsNullOrWhiteSpace(data.ToString()!.Trim()))
        {
            result = "-";
        }
        else
        {
            result = data.ToString()!;
        }

        return result;
    }

    public static string ToDashFromNullDate(this DateTime? data)
    {
        string result;
        if (data == null || string.IsNullOrEmpty(data.ToString()!.Trim()) ||
            string.IsNullOrWhiteSpace(data.ToString()!.Trim()))
        {
            result = "-";
        }
        else
        {
            result = data.ToDateTime().ToString(_Format.DateFormat9);
        }

        return result;
    }

    public static string ToDashFromNullDate(this DateTime data)
    {
        string result;
        if (data.IsDefaultDate())
        {
            result = "-";
        }
        else
        {
            result = data.ToString(_Format.DateFormat9);
        }

        return result;
    }

    public static string ToStryyyyMMdd(this string str)
    {
        string[] strAr = str.Split('-');
        return $"{strAr[2]}-{strAr[1]}-{strAr[0]}";
    }

    public static string ToTitle(this string keyword)
    {
        // Replace the hyphen with a space
        string result = keyword.Replace("-", " ");

        // Capitalize the first letter of each word
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        result = textInfo.ToTitleCase(result);

        return result;
    }

    public static string GetNrcNo(this string nrc)
    {
        if (string.IsNullOrEmpty(nrc))
        {
            return string.Empty;
        }

        return nrc.Substring(nrc.IndexOf(')') + 1);
    }

    public static string GenerateUlid()
    {
        return Ulid.NewUlid().ToString();
    }

    #region Request Response Encryption And Decryption

    public static string ToEncryptApiResponseData(this string str, string key, string iv)
    {
        return str.EncryptData(key, iv);
    }

    public static string GenerateAesIv()
    {
        var iv = "EXW5l4431ahDTEPs";
        try
        {
            return EncryptProvider.CreateAesKey().IV;
        }
        catch
        {
            return iv;
        }
    }

    public static string EncryptData(this string str, string key, string iv)
    {
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(str);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
        //byte[] ivBytes = Convert.FromBase64String(iv);
        byte[] encryptedBytes = Encrypt(plainTextBytes, keyBytes, ivBytes);

        // Prepend the IV to the encrypted data (optional)
        byte[] result = new byte[ivBytes.Length + encryptedBytes.Length];
        Buffer.BlockCopy(ivBytes, 0, result, 0, ivBytes.Length);
        Buffer.BlockCopy(encryptedBytes, 0, result, ivBytes.Length, encryptedBytes.Length);
        //var base64String = Convert.ToBase64String(result);
        //var finalEncryptedStr = string.Concat(iv, base64String);
        //return finalEncryptedStr;
        return string.Concat(iv, Convert.ToBase64String(result));
    }

    private static byte[] Encrypt(byte[] plainTextBytes, byte[] key, byte[] iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // Create an encryptor to perform the encryption process
            using (ICryptoTransform encryptor = aes.CreateEncryptor())
            {
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        // Write the plain text bytes to the encryption stream
                        csEncrypt.Write(plainTextBytes, 0, plainTextBytes.Length);
                        csEncrypt.FlushFinalBlock();

                        // Return the encrypted data
                        return msEncrypt.ToArray();
                    }
                }
            }
        }
    }

    public static string ToDecryptApiRequestData(this string str, string key, string iv)
    {
        return str.DecryptData(key, iv);
    }

    public static string ToDecryptTransactionPassword(this string transactionPasswordEnc, string loginPasswordEnc, string key, string iv)
    {
        string? directDecrypted = null;
        try
        {
            directDecrypted = transactionPasswordEnc.DecryptData(key, iv);
            if (!string.IsNullOrEmpty(directDecrypted))
            {
                var json = JObject.Parse(directDecrypted);
                if (json.ContainsKey("NewTransactionPassword") || json.ContainsKey("NewLoginPassword"))
                {
                    return directDecrypted;
                }
            }
        }
        catch
        {
            // Direct decryption failed or JSON parsing failed
        }

        try
        {
            if (!string.IsNullOrEmpty(loginPasswordEnc))
            {
                byte[] loginBytes = Convert.FromBase64String(loginPasswordEnc);
                byte[] tranBytes = Convert.FromBase64String(transactionPasswordEnc);

                if (tranBytes.Length > loginBytes.Length)
                {
                    bool startsWithLogin = true;
                    for (int i = 0; i < loginBytes.Length; i++)
                    {
                        if (loginBytes[i] != tranBytes[i])
                        {
                            startsWithLogin = false;
                            break;
                        }
                    }

                    if (startsWithLogin)
                    {
                        int remainingLength = tranBytes.Length - loginBytes.Length;
                        byte[] tranOnlyBytes = new byte[remainingLength];
                        Buffer.BlockCopy(tranBytes, loginBytes.Length, tranOnlyBytes, 0, remainingLength);

                        byte[] originalIv = new byte[16];
                        Buffer.BlockCopy(tranBytes, 0, originalIv, 0, 16);
                        string originalIvBase64 = Convert.ToBase64String(originalIv);

                        byte[] decryptedPayloadBytes = new byte[16 + tranOnlyBytes.Length];
                        Buffer.BlockCopy(originalIv, 0, decryptedPayloadBytes, 0, 16);
                        Buffer.BlockCopy(tranOnlyBytes, 0, decryptedPayloadBytes, 16, tranOnlyBytes.Length);

                        string payloadBase64 = Convert.ToBase64String(decryptedPayloadBytes);
                        string decrypted = payloadBase64.DecryptData(key, originalIvBase64);
                        return decrypted;
                    }
                }
            }
        }
        catch
        {
            // If workaround fails, return directDecrypted if it was successful, or let it fall back
        }

        return directDecrypted ?? transactionPasswordEnc.DecryptData(key, iv);
    }

    public static string DecryptData(this string encryptedStr, string key, string iv)
    {
        byte[] encryptedBytes = Convert.FromBase64String(encryptedStr);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] ivByte = Convert.FromBase64String(iv);
        //byte[] ivByte = Encoding.UTF8.GetBytes(iv);

        //byte[] ivByte = new byte[16];
        Buffer.BlockCopy(encryptedBytes, 0, ivByte, 0, ivByte.Length);
        byte[] ciphertext = new byte[encryptedBytes.Length - ivByte.Length];
        Buffer.BlockCopy(encryptedBytes, ivByte.Length, ciphertext, 0, ciphertext.Length);
        byte[] decryptedBytes = Decrypt(ciphertext, keyBytes, ivByte);

        string decryptedText = Encoding.UTF8.GetString(decryptedBytes);
        return decryptedText;
    }

    public static string DecryptData1(this string encryptedStr, string key, string iv)
    {
        byte[] encryptedBytes = Convert.FromBase64String(encryptedStr);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);

        byte[] ivByte;
        try
        {
            ivByte = Convert.FromBase64String(iv);
        }
        catch (FormatException)
        {
            throw new ArgumentException("IV is not a valid Base64 string.", nameof(iv));
        }

        //byte[] ivByte = new byte[16];
        Buffer.BlockCopy(encryptedBytes, 0, ivByte, 0, ivByte.Length);
        byte[] ciphertext = new byte[encryptedBytes.Length - ivByte.Length];
        Buffer.BlockCopy(encryptedBytes, ivByte.Length, ciphertext, 0, ciphertext.Length);
        byte[] decryptedBytes = Decrypt(ciphertext, keyBytes, ivByte);

        string decryptedText = Encoding.UTF8.GetString(decryptedBytes);
        return decryptedText;
    }


    private static byte[] Decrypt(byte[] encryptedData, byte[] key, byte[] iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // Create a decryptor to perform the decryption process
            using (ICryptoTransform decryptor = aes.CreateDecryptor())
            {
                using (MemoryStream msDecrypt = new MemoryStream())
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write))
                    {
                        // Write the encrypted data to the decryption stream
                        csDecrypt.Write(encryptedData, 0, encryptedData.Length);
                        csDecrypt.FlushFinalBlock();

                        // Return the decrypted data
                        return msDecrypt.ToArray();
                    }
                }
            }
        }
    }

    #endregion

    #region Ace SoftGate Encryption And Decryption

    private static readonly string AceSoftGateKey = "i56CbSbjPbrXmlaz2Q/C1GP5nokuEp99ic6QLUXVhII=";

    public static string AceSoftGateGenerateToken(this string userId)
    {
        var token = $"UserId:{userId},GeneratedDate:{GetServerDateTime():o}";
        return token.AceSoftGateEncrypt();
    }

    private static string AceSoftGateEncrypt(this string? plainText)
    {
        using var aes = Aes.Create();
        aes.Key = Convert.FromBase64String(AceSoftGateKey);
        aes.GenerateIV();
        var iv = aes.IV;

        using var encryptor = aes.CreateEncryptor(aes.Key, iv);
        var plainBytes = Encoding.UTF8.GetBytes(plainText!);
        var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        var result = new byte[iv.Length + encryptedBytes.Length];
        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
        Buffer.BlockCopy(encryptedBytes, 0, result, iv.Length, encryptedBytes.Length);

        return Convert.ToBase64String(result);
    }

    public static (string userId, DateTime expiredDate) AceSoftGateDecryptToken(this string token)
    {
        var fullCipher = Convert.FromBase64String(token);

        using var aes = Aes.Create();
        aes.Key = Convert.FromBase64String(AceSoftGateKey);

        var iv = new byte[aes.BlockSize / 8];
        var cipherText = new byte[fullCipher.Length - iv.Length];

        Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(fullCipher, iv.Length, cipherText, 0, cipherText.Length);

        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        var decryptedBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
        var decryptedText = Encoding.UTF8.GetString(decryptedBytes);

        var parts = decryptedText.Split(',');
        var userId = parts[0].Substring("UserId:".Length).Trim();
        var expiredDate = DateTime.Parse(parts[1].Substring("ExpiredDate:".Length).Trim());

        return (userId, expiredDate);
    }

    #endregion

    public static T XmlToObject<T>(this string? xmlString)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        using (StringReader reader = new StringReader(xmlString!))
        {
            T result = (T)serializer.Deserialize(reader)!;

            return result;
        }
    }

    public static string DefaultDashIfNullString(this string str)
    {
        if (str.IsNullOrEmpty())
        {
            return "-";
        }

        return str;
    }

    private static void UpdateFieldIfEmpty(JObject rawObj, string fieldName, Func<string?> getValue)
    {
        if (!getValue().IsNullOrEmpty() &&
            rawObj.Value<string>(fieldName).IsNullOrEmpty())
        {
            rawObj[fieldName] = getValue();
        }
    }

    public static string ToExportType(this string exportType)
    {
        string exportFileType = "pdf";
        if (exportType == "excel")
        {
            exportFileType = "csv";
        }
        else if (exportType == "word")
        {
            exportFileType = "docx";
        }

        return $".{exportFileType}";
    }

    public static string WithLockInShareMode(this string query)
    {
        return $"{query} LOCK IN SHARE MODE";
    }

    public static string AppendMessage(this string message, string nextMessage = "")
    {
        return message + "\n" + nextMessage;
    }

    public static string ToLogMessageWithDateTime(this string message)
    {
        string result = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
        return result;
    }

    public static void AppLogging(this Exception ex, string projectName)
    {
        string logFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? string.Empty,
            "log");
        string logFilePath = Path.Combine(logFolder, $"{projectName}_AppLog_{DateTime.Now:yyyy-MM-dd HH tt}.txt");
        using (StreamWriter writer = new StreamWriter(logFilePath, true))
        {
            writer.WriteLine("Application terminated unexpectedly".ToLogMessageWithDateTime());
            writer.WriteLine(ex.Message.ToLogMessageWithDateTime());
        }
    }

    public static int ToInt(this string str)
    {
        return Convert.ToInt32(str);
    }

    public static string GeneratePassword(this int length, Boolean useLowercase, Boolean useUppercase,
        Boolean useSpecialchar)
    {
        // Create a string of characters, numbers, special characters that allowed in the password
        string lower = "abcdefghijkmnopqrstuvwxyz";
        string upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        string special = "@#%&$^"; // special characters defined by Bank

        Random random = new Random();
        List<char> chars = new List<char>();
        string validChars = "123456789"; //don't use zero because of conflict with letter 'O'
        //one number
        chars.Insert(random.Next(0, chars.Count),
            validChars[random.Next(0, validChars.Length)]);

        if (useLowercase)
        {
            validChars = validChars.Insert(validChars.Length, lower);
            chars.Insert(random.Next(0, chars.Count),
                lower[random.Next(0, lower.Length)]);
        }

        if (useUppercase)
        {
            validChars = validChars.Insert(validChars.Length, upper);
            chars.Insert(random.Next(0, chars.Count),
                upper[random.Next(0, upper.Length)]);
        }

        if (useSpecialchar)
        {
            validChars = validChars.Insert(validChars.Length, special);
            chars.Insert(random.Next(0, chars.Count),
                special[random.Next(0, special.Length)]);
        }

        // Select one random character at a time from the string and create an array of chars

        for (int i = chars.Count; i < length; i++)
        {
            chars.Insert(random.Next(0, chars.Count),
                validChars[random.Next(0, validChars.Length)]);
        }

        return new string(chars.ToArray());
    }

    public static string GeneratePassword(int numberLength)
    {
        var random = new Random();
        var rNum = new StringBuilder();
        for (int i = 0; i < numberLength; i++)
        {
            _ = rNum.Append(random.Next(0, 6).ToString());
        }

        return rNum.ToString();
    }

    public static bool IsBase64String(this string base64String)
    {
        try
        {
            var base64Bytes = Convert.FromBase64String(base64String);
            return base64Bytes.Length > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public static string ToThousandSeparator(this string amount)
    {
        return Convert.ToDecimal(amount).ToString("#,##0.00");
    }

    public static string GetNrcNoOld(this string nrc)
    {
        string numbers = "";
        Match match = Regex.Match(nrc, @"\)(\d+)");
        if (match.Success)
        {
            numbers = match.Groups[1].Value;
        }

        return numbers;
    }

    public static string ReportDecrypt(this string ciphertext)
    {
        try
        {
            string urlDecoded = HttpUtility.UrlDecode(ciphertext);
            byte[] encryptedData = Convert.FromBase64String(urlDecoded);

            using (Aes aes = Aes.Create())
            {
                aes.Key = _key;
                byte[] iv = new byte[aes.IV.Length];
                Buffer.BlockCopy(encryptedData, 0, iv, 0, aes.IV.Length);
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                byte[] decryptedBytes;
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(encryptedData, aes.IV.Length, encryptedData.Length - aes.IV.Length);
                    }

                    decryptedBytes = ms.ToArray();
                }

                string decryptedText = Encoding.UTF8.GetString(decryptedBytes);
                return decryptedText;
            }
        }
        catch
        {
            return ciphertext;
        }
    }

    public static string ReportEncrypt(this string plaintext)
    {
        using Aes aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;
        // aes.GenerateIV();

        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        byte[] encryptedBytes;
        using (var ms = new MemoryStream())
        {
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
                cs.Write(plaintextBytes, 0, plaintextBytes.Length);
            }

            encryptedBytes = ms.ToArray();
        }

        byte[] encryptedData = new byte[aes.IV.Length + encryptedBytes.Length];
        Buffer.BlockCopy(aes.IV, 0, encryptedData, 0, aes.IV.Length);
        Buffer.BlockCopy(encryptedBytes, 0, encryptedData, aes.IV.Length, encryptedBytes.Length);

        string base64Encoded = Convert.ToBase64String(encryptedData);
        string urlEncoded = HttpUtility.UrlEncode(base64Encoded);
        return urlEncoded;
    }
    public static string GenerateReferralCode()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz";
        const string digits = "0123456789";
        const int charLength = 2;
        const int digitLength = 4;

        var sb = new StringBuilder();

        for (int i = 0; i < charLength; i++)
        {
            int index = RandomNumberGenerator.GetInt32(chars.Length);
            _ = sb.Append(chars[index]);
        }

        for (int i = 0; i < digitLength; i++)
        {
            int index = RandomNumberGenerator.GetInt32(digits.Length);
            _ = sb.Append(digits[index]);
        }

        return sb.ToString();
    }

    public static string ToDashFromNullDateTime(this DateTime? data)
    {
        string result;
        if (data == null || string.IsNullOrEmpty(data.ToString()!.Trim()) ||
            string.IsNullOrWhiteSpace(data.ToString()!.Trim()))
        {
            result = "-";
        }
        else
        {
            result = data.ToDateTime().ToString(_Format.DateFormat5);
        }

        return result;
    }
    public static string ToDashFromNullDateTime(this DateTime data)
    {
        string result;
        if (data.IsDefaultDate())
        {
            result = "-";
        }
        else
        {
            result = data.ToDateTime().ToString(_Format.DateFormat5);
        }

        return result;
    }

    public static string ToDashFromNullDateTime24(this DateTime data)
    {
        string result;
        if (data.IsDefaultDate())
        {
            result = "-";
        }
        else
        {
            result = data.ToDateTime().ToString(_Format.DateFormat12);
        }

        return result;
    }

    private const int AesBlockSize = 16;
    public static string DecryptCookieData(this string encryptedStr, string key)
    {
        // Your controller code now passes only the key, as the IV is extracted here.
        // We will ignore the 'iv' parameter from your previous code completely.

        byte[] fullEncryptedBytes = Convert.FromBase64String(encryptedStr);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);

        // 1. Validate total length
        if (fullEncryptedBytes.Length < AesBlockSize)
        {
            throw new ArgumentException("Encrypted string is too short to contain IV and data.", nameof(encryptedStr));
        }

        // 2. Extract the IV (first 16 bytes)
        byte[] ivBytes = new byte[AesBlockSize];
        Buffer.BlockCopy(fullEncryptedBytes, 0, ivBytes, 0, AesBlockSize);

        // 3. Extract the Ciphertext (everything after the IV)
        int ciphertextLength = fullEncryptedBytes.Length - AesBlockSize;
        byte[] ciphertext = new byte[ciphertextLength];
        Buffer.BlockCopy(fullEncryptedBytes, AesBlockSize, ciphertext, 0, ciphertextLength);

        // 4. Decrypt using AES
        using (Aes aesAlg = Aes.Create())
        {
            // Set Key and IV
            aesAlg.Key = keyBytes;
            aesAlg.IV = ivBytes;

            // Standard configuration for AES
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(ciphertext))
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
            {
                return srDecrypt.ReadToEnd();
            }
        }
    }

    public static string EncryptCookieData(this string plainText, string key)
    {
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);

        // 1. Generate a unique, random IV (16 bytes)
        byte[] ivBytes = new byte[AesBlockSize];
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(ivBytes);
        }

        // 2. Encrypt the data
        byte[] encryptedBytes;

        using (Aes aesAlg = Aes.Create())
        {
            // Set Key and IV
            aesAlg.Key = keyBytes;
            aesAlg.IV = ivBytes;

            // Standard configuration
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                    csEncrypt.FlushFinalBlock();
                    encryptedBytes = msEncrypt.ToArray();
                }
            }
        }

        // 3. Combine IV and Ciphertext
        // Create a final array with size IV + Ciphertext
        byte[] resultBytes = new byte[ivBytes.Length + encryptedBytes.Length];

        // Copy IV to the beginning
        Buffer.BlockCopy(ivBytes, 0, resultBytes, 0, ivBytes.Length);

        // Copy Ciphertext immediately after the IV
        Buffer.BlockCopy(encryptedBytes, 0, resultBytes, ivBytes.Length, encryptedBytes.Length);

        // 4. Return the Base64 representation of the combined bytes
        return Convert.ToBase64String(resultBytes);
    }

    public static string PhoneNoMasking(this string phoneNo, char symbol = '*')
    {
        if (string.IsNullOrWhiteSpace(phoneNo))
        {
            return string.Empty;
        }

        phoneNo = phoneNo.Trim();

        if (phoneNo.Length != 9 && phoneNo.Length != 11)
        {
            return phoneNo;
        }

        return "".PadLeft(phoneNo.Length - 4, symbol)
               + phoneNo.Substring(phoneNo.Length - 4, 4);
    }

    public static string EncryptData(this string str, string key)
    {
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(str);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);

        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.GenerateIV(); // Automatically creates a unique 16-byte IV
            byte[] ivBytes = aes.IV;

            // Perform the encryption using your existing Encrypt logic
            byte[] encryptedBytes = Encrypt(plainTextBytes, keyBytes, ivBytes);

            // Combine [IV (16 bytes)] + [Ciphertext]
            byte[] result = new byte[ivBytes.Length + encryptedBytes.Length];
            Buffer.BlockCopy(ivBytes, 0, result, 0, ivBytes.Length);
            Buffer.BlockCopy(encryptedBytes, 0, result, ivBytes.Length, encryptedBytes.Length);

            return Convert.ToBase64String(result);
        }
    }

    public static string DecryptData(this string encryptedStr, string key)
    {
        if (string.IsNullOrWhiteSpace(encryptedStr))
        {
            throw new Exception("Encrypted string is empty.");
        }

        // Fix corrupted Base64 from transport
        encryptedStr = encryptedStr.Trim();
        encryptedStr = encryptedStr.Replace(" ", "+");

        byte[] fullPackage = Convert.FromBase64String(encryptedStr);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);

        // AES IV is always 16 bytes
        byte[] ivBytes = new byte[16];
        byte[] ciphertext = new byte[fullPackage.Length - 16];

        // Extract IV from the first 16 bytes
        Buffer.BlockCopy(fullPackage, 0, ivBytes, 0, 16);

        // Extract the actual encrypted data from the rest
        Buffer.BlockCopy(fullPackage, 16, ciphertext, 0, ciphertext.Length);

        byte[] decryptedBytes = Decrypt(ciphertext, keyBytes, ivBytes);
        return Encoding.UTF8.GetString(decryptedBytes);
    }
}
