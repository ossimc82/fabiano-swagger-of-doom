#region

using System;
using System.IO;
using System.Net;
using System.Text;

#endregion

public class NWriter : BinaryWriter
{
    public NWriter(Stream s)
        : base(s, Encoding.UTF8)
    {
    }

    public override void Write(short value)
    {
        base.Write(IPAddress.HostToNetworkOrder(value));
    }

    public override void Write(int value)
    {
        base.Write(IPAddress.HostToNetworkOrder(value));
    }

    public override void Write(long value)
    {
        base.Write(IPAddress.HostToNetworkOrder(value));
    }

    public override void Write(ushort value)
    {
        base.Write((ushort) IPAddress.HostToNetworkOrder((short) value));
    }

    public override void Write(uint value)
    {
        base.Write((uint) IPAddress.HostToNetworkOrder((int) value));
    }

    public override void Write(ulong value)
    {
        base.Write((ulong) IPAddress.HostToNetworkOrder((long) value));
    }

    public override void Write(float value)
    {
        byte[] b = BitConverter.GetBytes(value);
        Array.Reverse(b);
        base.Write(b);
    }

    public override void Write(double value)
    {
        byte[] b = BitConverter.GetBytes(value);
        Array.Reverse(b);
        base.Write(b);
    }

    public void WriteNullTerminatedString(string str)
    {
        Write(Encoding.UTF8.GetBytes(str));
        Write((byte) 0);
    }

    public void WriteUTF(string str)
    {
        if (str == null)
            Write((short) 0);
        else
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            Write((short)bytes.Length);
            Write(bytes);
        }
    }

    public void Write32UTF(string str)
    {
        var bytes = Encoding.UTF8.GetBytes(str);
        Write(bytes.Length);
        Write(bytes);
    }
}