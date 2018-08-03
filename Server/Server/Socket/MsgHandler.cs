using Google.Protobuf;
using System;
using System.Collections.Generic;

class MsgHandler
{
    protected ProtocolDispatcher dispatcher = new ProtocolDispatcher();

    public void AddHandler(Type type, Handler handler)
    {
        this.dispatcher.AddHandler(type, handler);
    }

    public void Handle(IMessage protocol, int connect_id)
    {

    }
}
