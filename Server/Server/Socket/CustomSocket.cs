using Fleck;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class CustomSocket
{
    private WebSocketServer mWebSocket;
    private MsgHandler mMsgHandler = new MsgHandler();
    private ByteCache byte_cache = new ByteCache();
    private Dictionary<IWebSocketConnection, ByteCache> cache_map = new Dictionary<IWebSocketConnection, ByteCache>();
    private CustomDictionary<int, IWebSocketConnection> socket_map = new CustomDictionary<int, IWebSocketConnection>();
    private int max_connect_id = 0;

    public void Init()
    {
        this.mWebSocket = new WebSocketServer("ws://127.0.0.1:6324");
        this.mWebSocket.Start(socket => {
            socket.OnOpen = delegate ()
            {
                this.cache_map[socket] = new ByteCache();
                this.socket_map.Add(++this.max_connect_id, socket);
                Log.Debug("用户连接:" + socket.ConnectionInfo.ClientIpAddress);
            };

            socket.OnClose = delegate ()
            {
                this.cache_map.Remove(socket);
                this.socket_map.Remove(socket);
                Log.Debug("用户断线:" + socket.ConnectionInfo.ClientIpAddress);
            };

            socket.OnError = delegate (Exception e)
            {
                Log.Debug(e.ToString());
            };

            socket.OnMessage = delegate (string str)
            {
                Log.Debug("用户发送数据:" + socket.ConnectionInfo.ClientIpAddress + ":" + str);
            };

            socket.OnBinary = delegate (byte[] bytes)
            {
                ByteCache cache = this.cache_map[socket];
                cache.AddBytes(bytes, bytes.Length);

                try
                {
                    while (true)
                    {
                        byte[] msg_bytes = cache.DivideMessage();
                        if (msg_bytes == null)
                        {
                            break;
                        }

                        int connect_id;
                        if (this.socket_map.HaveValue(socket))
                        {
                            connect_id = this.socket_map.GetKey(socket);
                        }
                        else
                        {
                            Exception e = new Exception("Socket对应的连接ID不存在!!!");
                            throw e;
                        }
                        this.UnPackProtocol(msg_bytes, connect_id);
                    }
                }
                catch (Exception e)
                {
                    Log.Debug(e.ToString());
                    socket.Close();
                }
            };
        });
        Log.Debug("服务器WebSocket正在监听 127.0.0.1:6324");
    }

    public void Release()
    {
        if (this.mWebSocket != null) this.mWebSocket.Dispose();
    }

    public void AddHandler(Type type, Handler handler)
    {
        this.mMsgHandler.AddHandler(type, handler);
    }

    private IWebSocketConnection GetSocket(int connect_id)
    {
        IWebSocketConnection socket = null;
        if (this.socket_map.HaveKey(connect_id))
        {
            socket = this.socket_map.GetValue(connect_id);
        }
        return socket;
    }

    private byte[] PackProtocol(IMessage protocol)
    {
        ByteBuffer buffer = new ByteBuffer();

        //写入协议体
        byte[] protocol_bytes = Protocol.Encode(protocol);
        buffer.WriteBytes(protocol_bytes);

        byte[] msg_bytes = buffer.ToBytes();
        buffer.Clear();
        buffer.WriteInt(msg_bytes.Length);
        buffer.WriteBytes(msg_bytes);
        msg_bytes = buffer.ToBytes();
        buffer.Close();

        return msg_bytes;
    }

    private void UnPackProtocol(byte[] msg_bytes, int connect_id)
    {
        ByteBuffer buffer = new ByteBuffer(msg_bytes);

        //读出协议体
        byte[] protocol_bytes = buffer.ReadBytes((int)buffer.RemainingBytes());
        IMessage protocol = Protocol.Decode(protocol_bytes);
        buffer.Close();

        if (protocol != null)
        {
            Log.Debug("服务端接收消息:" + protocol.GetType() + " 数据:" + protocol.ToString());
            this.DispatchProtocol(protocol, connect_id);
        }
    }

    private void DispatchProtocol(IMessage protocol, int connect_id)
    {
        this.mMsgHandler.Handle(protocol, connect_id);
    }

    public void SendMsgToClient(IMessage protocol, int connect_id)
    {
        IWebSocketConnection socket = this.GetSocket(connect_id);
        if (socket == null)
        {
            Log.Debug("用户Socket不存在:" + connect_id);
            return;
        }
        Log.Debug("服务端发送消息:" + protocol.GetType() + " 数据:" + protocol.ToString());
        socket.Send(this.PackProtocol(protocol));
    }
}
