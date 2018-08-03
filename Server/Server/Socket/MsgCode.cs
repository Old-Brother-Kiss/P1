using Common.Protobuf;
using Google.Protobuf;
using System;
using System.Collections.Generic;

class MsgCode
{
	public const short CS_Login = 10001;
	public const short SC_Login = 10002;
	public const short SC_Notice = 10003;

	public static Dictionary<short, MessageParser> ProtocolParser = new Dictionary<short, MessageParser>() {
		{MsgCode.CS_Login, CSLogin.Parser},
		{MsgCode.SC_Login, SCLogin.Parser},
	};

	public static Dictionary<Type, short> ProtocolMap = new Dictionary<Type, short>(){
		{typeof(CSLogin), MsgCode.CS_Login},
		{typeof(SCLogin), MsgCode.SC_Login},
	};
};
