using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class MessageQueue
{
    private Queue<IMessage> queue = new Queue<IMessage>();

    public void AddToQueue(IMessage protocol)
    {
        queue.Enqueue(protocol);
    }

    public IMessage[] GetProtocolList()
    {
        IMessage[] protocol_list = queue.ToArray();
        queue.Clear();
        return protocol_list;
    }
}
