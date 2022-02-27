using System;
using System.Text;

namespace NetDevPack.Security.PasswordHasher.Core.Utilities;

internal static class StringUtils
{
    public static byte[] FromBase64(this string str)
    {
        return Convert.FromBase64String(str);
    }

    /// <summary>
    /// Equivalent to xxd -p
    /// see: https://linux.die.net/man/1/xxd
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    internal static string ToPlainHexDumpStyle(this byte[] data)
    {
        return BitConverter.ToString(data).Replace("-", "").ToLower();
    }

    internal static byte[] FromPlainHexDumpStyleToByteArray(this string hex)
    {
        if (hex.Length % 2 == 1)
            throw new Exception("The binary key cannot have an odd number of digits");

        byte[] arr = new byte[hex.Length >> 1];

        for (int i = 0; i < hex.Length >> 1; ++i)
        {
            arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
        }

        return arr;
    }

    private static int GetHexVal(char hex)
    {
        int val = (int)hex;
        //For uppercase A-F letters:
        //return val - (val < 58 ? 48 : 55);
        //For lowercase a-f letters:
        //return val - (val < 58 ? 48 : 87);
        //Or the two combined, but a bit slower:
        return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
    }

    internal static string ToBase64(this string str, Encoding enc = null)
    {
        return ToBase64((enc ?? Encoding.Default).GetBytes(str));
    }

    internal static string ToBase64(this byte[] data, Encoding enc = null)
    {
        return Convert.ToBase64String(data);
    }
}