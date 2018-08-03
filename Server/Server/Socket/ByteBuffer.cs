using System.IO;
using System.Text;
using System;

class ByteBuffer
{
    MemoryStream stream;
    BinaryWriter writer;
    BinaryReader reader;

    public ByteBuffer()
    {
        stream = new MemoryStream();
        writer = new BinaryWriter(stream);
        reader = new BinaryReader(stream);
    }

    public ByteBuffer(byte[] data)
    {
        if (data != null)
        {
            stream = new MemoryStream(data);
            writer = new BinaryWriter(stream);
            reader = new BinaryReader(stream);
        }
        else
        {
            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
            reader = new BinaryReader(stream);
        }
    }

    public void Close()
    {
        if (writer != null) writer.Close();
        if (reader != null) reader.Close();
        stream.Close();
    }

    public void WriteByte(byte v)
    {
        writer.Write(v);
    }

    public void WriteInt(int v)
    {
        writer.Write(System.Net.IPAddress.HostToNetworkOrder(v));
    }

    public void WriteShort(short v)
    {
        writer.Write(System.Net.IPAddress.HostToNetworkOrder(v));
    }

    public void WriteLong(long v)
    {
        writer.Write(System.Net.IPAddress.HostToNetworkOrder(v));
    }

    public void WriteFloat(float v)
    {
        byte[] temp = BitConverter.GetBytes(v);
        Array.Reverse(temp);
        writer.Write(BitConverter.ToSingle(temp, 0));
    }

    public void WriteDouble(double v)
    {
        byte[] temp = BitConverter.GetBytes(v);
        Array.Reverse(temp);
        writer.Write(BitConverter.ToDouble(temp, 0));
    }

    public void WriteString(string v)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(v);
        writer.Write((ushort)bytes.Length);
        writer.Write(bytes);
    }

    public void WriteBytes(byte[] v)
    {
        writer.Write(v);
    }

    public void WriteBytes(byte[] v, int len)
    {
        writer.Write(v, 0, len);
    }

    public byte ReadByte()
    {
        return reader.ReadByte();
    }

    public int ReadInt()
    {
        return System.Net.IPAddress.NetworkToHostOrder(reader.ReadInt32());
    }

    public short ReadShort()
    {
        return System.Net.IPAddress.NetworkToHostOrder(reader.ReadInt16());
    }

    public long ReadLong()
    {
        return System.Net.IPAddress.NetworkToHostOrder(reader.ReadInt64());
    }

    public float ReadFloat()
    {
        byte[] temp = BitConverter.GetBytes(reader.ReadSingle());
        Array.Reverse(temp);
        return BitConverter.ToSingle(temp, 0);
    }

    public double ReadDouble()
    {
        byte[] temp = BitConverter.GetBytes(reader.ReadDouble());
        Array.Reverse(temp);
        return BitConverter.ToDouble(temp, 0);
    }

    public string ReadString()
    {
        short len = ReadShort();
        byte[] buffer = new byte[len];
        buffer = reader.ReadBytes(len);
        return Encoding.UTF8.GetString(buffer);
    }

    public byte[] ReadBytes(int len)
    {
        return reader.ReadBytes(len);
    }

    public byte[] ToBytes()
    {
        writer.Flush();
        return stream.ToArray();
    }

    public void Seek(int offset, SeekOrigin seek_origin)
    {
        stream.Seek(offset, seek_origin);
    }

    public void Flush()
    {
        writer.Flush();
    }

    public void Clear()
    {
        stream.SetLength(0);
    }

    public Stream GetStream()
    {
        return this.stream;
    }

    public long RemainingBytes()
    {
        return stream.Length  - stream.Position;
    }
}