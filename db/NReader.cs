#region

using System;
using System.IO;
using System.Net;
using System.Text;

#endregion

public class NReader : BinaryReader
{
    public NReader(Stream s) : base(s, Encoding.UTF8)
    {
    }

    public override short ReadInt16()
    {
        return IPAddress.NetworkToHostOrder(base.ReadInt16());
    }

    public override int ReadInt32()
    {
        return IPAddress.NetworkToHostOrder(base.ReadInt32());
    }

    public override long ReadInt64()
    {
        return IPAddress.NetworkToHostOrder(base.ReadInt64());
    }

    public override ushort ReadUInt16()
    {
        return (ushort) IPAddress.NetworkToHostOrder((short) base.ReadUInt16());
    }

    public override uint ReadUInt32()
    {
        return (uint) IPAddress.NetworkToHostOrder((int) base.ReadUInt32());
    }

    public override ulong ReadUInt64()
    {
        return (ulong) IPAddress.NetworkToHostOrder((long) base.ReadUInt64());
    }

    public override float ReadSingle()
    {
        byte[] arr = base.ReadBytes(4);
        Array.Reverse(arr);
        return BitConverter.ToSingle(arr, 0);
    }

    public override double ReadDouble()
    {
        byte[] arr = base.ReadBytes(8);
        Array.Reverse(arr);
        return BitConverter.ToDouble(arr, 0);
    }

    public string ReadNullTerminatedString()
    {
        StringBuilder ret = new StringBuilder();
        byte b = ReadByte();
        while (b != 0)
        {
            ret.Append((char) b);
            b = ReadByte();
        }
        return ret.ToString();
    }

    public string ReadUTF()
    {
        return Encoding.UTF8.GetString(ReadBytes(ReadInt16()));
    }

    public string Read32UTF()
    {
        return Encoding.UTF8.GetString(ReadBytes(ReadInt32()));
    }
}