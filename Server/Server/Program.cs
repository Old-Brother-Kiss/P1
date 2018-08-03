using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        Server.GetInstance().Start();
        Thread.CurrentThread.Suspend();
    }
}
