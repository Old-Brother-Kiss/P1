using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class User
{
    public int ConnectId;
    public int UserId;
    public int State;
    public int ActiveTime;
    public int CurServerId;
    public int CurSceneId;
    public IMessage LoginMsg;
    public int MaxMsgNo;        //已发送最大的协议编号
    public Dictionary<int, IMessage> msg_map = new Dictionary<int, IMessage>();     //用于断线重连

    public User(int connect_id)
    {
        this.ConnectId = connect_id;
    }

    public void SetActive()
    {
        this.ActiveTime = Global.GetCurTime();
    }

    public void ClearMsgCache()
    {
        this.msg_map.Clear();
    }

    public void SetState(short state)
    {
        this.State = state;
    }

    public bool CheckState(short state)
    {
        return this.State == state;
    }

    public void SendMsg(IMessage message)
    {
        Server.GetInstance().GetSocket().SendMsgToClient(message, this.ConnectId);
    }
}