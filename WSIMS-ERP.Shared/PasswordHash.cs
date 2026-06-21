namespace WSIMS_ERP.Shared;

public static class PasswordHash
{
    #region SHA256 Password HashString
    public static string ToHex(byte[] bytes, bool upperCase)
    {
        StringBuilder result = new StringBuilder(bytes.Length * 2);
        for (int i = 0; i < bytes.Length; i++)
        {
            _ = result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));
        }

        return result.ToString();
    }

    public static string SHA256HexHashString(this string password, string userName)
    {
        password = password.Trim();
        userName = userName.Trim();

        string saltedCode = EncodedbySalted(userName);//username
        string hashString;
        using (var sha256 = SHA256.Create())
        {
            var hash = sha256.ComputeHash(Encoding.Default.GetBytes(password + saltedCode));
            hashString = ToHex(hash, false);
        }

        return hashString;
    }

    public static string EncodedbySalted(string decodestring)
    {
        decodestring = decodestring.ToLower()
            .Replace("a", "@")
            .Replace("i", "!")
            .Replace("l", "1")
            .Replace("e", "3")
            .Replace("o", "0")
            .Replace("s", "$")
            .Replace("n", "&");
        return decodestring;
    }

    #endregion
}

