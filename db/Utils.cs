#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

#endregion

public static class Utils
{
    public static class ConsoleCloseEventHandler
    {
        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);
        public delegate bool EventHandler(CtrlType sig);

        public enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }
    }

    public static int FromString(string x)
    {
        if (x.StartsWith("0x")) return int.Parse(x.Substring(2), NumberStyles.HexNumber);
        return int.Parse(x);
    }

    /// <summary>
    ///     Indicates whether a specified string is null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="value">value: The string to test.</param>
    /// <returns>
    ///     true if the value parameter is null or System.String.Empty, or if value consists exclusively of white-space
    ///     characters.
    /// </returns>
    public static bool IsNullOrWhiteSpace(this string value)
    {
        if (value == null)
        {
            return true;
        }
        int index = 0;
        while (index < value.Length)
        {
            if (char.IsWhiteSpace(value[index]))
            {
                index++;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public static string To4Hex(short x)
    {
        return "0x" + x.ToString("x4");
    }

    public static string To2Hex(short x)
    {
        return "0x" + x.ToString("x2");
    }

    public static string GetCommaSepString<T>(T[] arr)
    {
        StringBuilder ret = new StringBuilder();
        for (int i = 0; i < arr.Length; i++)
        {
            if (i != 0) ret.Append(", ");
            ret.Append(arr[i]);
        }
        return ret.ToString();
    }

    public static List<int> StringListToIntList(List<string> strList)
    {
        List<int> ret = new List<int>();
        foreach (string i in strList)
        {
            try
            {
                ret.Add(Convert.ToInt32(i));
            }
            catch
            {
            }
        }
        return ret;
    }

    public static int[] FromCommaSepString32(string x)
    {
        if (IsNullOrWhiteSpace(x)) return new int[0];
        return x.Split(',').Select(_ => FromString(_.Trim())).ToArray();
    }

    public static short[] FromCommaSepString16(string x)
    {
        if (IsNullOrWhiteSpace(x)) return new short[0];
        return x.Split(',').Select(_ => (short) FromString(_.Trim())).ToArray();
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        if (list == null) return;
        int n = list.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box); while (!(box[0] < n*(uint.MaxValue/n)));
            int k = (box[0]%n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static string ToSafeText(this string str)
    {
        return Encoding.ASCII.GetString(
            Encoding.Convert(
                Encoding.UTF8,
                Encoding.GetEncoding(
                    Encoding.ASCII.EncodingName,
                    new EncoderReplacementFallback(string.Empty),
                    new DecoderExceptionFallback()
                    ),
                Encoding.UTF8.GetBytes(str)
                )
            );
    }

    public static short[] PackFromEquips(this Char chr)
    {
        List<short> bpItems = FromCommaSepString16(chr._Equipment).ToList();
        bpItems.RemoveRange(0, 4);
        return bpItems.ToArray();
    }

    public static short[] EquipSlots(this Char chr)
    {
        List<short> eqpSlots = FromCommaSepString16(chr._Equipment).ToList();
        //eqpSlots.RemoveRange(4, 8);
        return eqpSlots.ToArray();
    }

    public static T GetEnumByName<T>(string value)
    {
        return (T) Enum.Parse(typeof (T), value, true);
    }

    public static string GetEnumName<T>(object value)
    {
        return Enum.GetName(typeof (T), value);
    }

    public static byte[] RandomBytes(int len)
    {
        var arr = new byte[len];
        var r = new Random();
        r.NextBytes(arr);
        return arr;
    }

    public static void ExecuteSync(this Task task)
    {
        task.Wait();
    }

    public static T ExecuteSync<T>(this Task<T> task)
    {
        task.Wait();
        return task.Result;
    }
}