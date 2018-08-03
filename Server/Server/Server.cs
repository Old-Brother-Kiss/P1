using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Server
{
    private static Server Instance;
    private CustomSocket mSocket;

    public static Server GetInstance()
    {
        if (Instance == null)
        {
            Instance = new Server();
        }
        return Instance;
    }

    public void Start()
    {
        this.mSocket = new CustomSocket();
        this.mSocket.Init();
    }

    public CustomSocket GetSocket()
    {
        return this.mSocket;
    }
}
