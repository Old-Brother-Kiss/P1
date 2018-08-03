using Common.Protobuf;
using Google.Protobuf;
using System;
using System.Collections.Generic;

class Protocol
{
    public static void LogProtocolMap(){
        foreach(KeyValuePair<Type, short> pair in MsgCode.ProtocolMap)
        {
            Log.Debug("Key:" + pair.Key + " Value:" + pair.Value);
        }
    }

    public static short GetMsgCode(IMessage protocol)
    {
        Type type = protocol.GetType();
        if (!MsgCode.ProtocolMap.ContainsKey(type))
        {
            Log.Debug("协议类型没有赋予协议号:" + type.ToString());
            return -1;
        }
        return MsgCode.ProtocolMap[type];
    }

    public static byte[] Encode(IMessage protocol)
    {
        short msg_code = Protocol.GetMsgCode(protocol);
        if(msg_code == -1)
        {
            return null;
        }

        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteShort(msg_code);
        buffer.WriteBytes(protocol.ToByteArray());
        return buffer.ToBytes();
    }

    public static IMessage Decode(byte[] msg_bytes)
    {
        ByteBuffer buffer = new ByteBuffer(msg_bytes);
        short msg_code = buffer.ReadShort();
        byte[] msg = buffer.ReadBytes((int)buffer.RemainingBytes());

        if (!MsgCode.ProtocolParser.ContainsKey(msg_code))
        {
            Log.Debug("未知类型的协议号:" + msg_code);
            return null;
        }
        MessageParser parser = MsgCode.ProtocolParser[msg_code];
        return parser.ParseFrom(msg);
    }
}
