using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

/// <summary>
///ClientSocket 的摘要说明
/// </summary>
public class ClientSocket
{
	public ClientSocket()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    public string Send( string host, int port)
    {

        IPAddress ip = IPAddress.Parse(host);
        IPEndPoint ipe = new IPEndPoint(ip, port);//把ip和端口转化为IPEndPoint实例

        Socket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建一个Socket
        Console.WriteLine("Conneting...");
        c.Connect(ipe);//连接到服务器
        string sendStr = "hello!This is a socket test";
        byte[] bs = Encoding.ASCII.GetBytes(sendStr);

        Console.WriteLine("Send Message");
        c.Send(bs, bs.Length, 0);//发送测试信息

        string recvStr = "";
        byte[] recvBytes = new byte[1024];
        int bytes;
        bytes = c.Receive(recvBytes, recvBytes.Length, 0);//从服务器端接受返回信息
        recvStr += Encoding.ASCII.GetString(recvBytes, 0, bytes);
        Console.WriteLine("Client Get Message:{0}", recvStr);//显示服务器返回信息
        c.Close();

        return recvStr;

    }
}
