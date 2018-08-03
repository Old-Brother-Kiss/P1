using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

delegate void ClientHandler(IMessage protocol, int connect_id);
delegate void Handler(IMessage protocol, int connect_id, int addition, List<int> user_id_list);

class ProtocolDispatcher
{
    private Dictionary<Type, Handler> handle_map = new Dictionary<Type, Handler>();

    public void AddHandler(Type type, Handler handler)
    {
        this.handle_map[type] = handler;
    }

    public void Dispatch(IMessage protocol, int connect_id, int addition, List<int> user_id_list)
    {
        Type type = protocol.GetType();
        if (!handle_map.ContainsKey(type))
        {
            Log.Debug("协议类型没有注册处理函数:" + type.ToString());
            return;
        }

        Handler handler = handle_map[type];
        handler(protocol, connect_id, addition, user_id_list);
    }
}

