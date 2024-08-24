using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Summer;
using System.Net;
using System.Net.Sockets;
using Summer.Network;
using System;
using Google.Protobuf;

/// <summary>
/// 网络客户端
/// </summary>
public class NetClient
{
    private static Connection conn = null;

    public static void Send(IMessage message)
    {
        if(conn != null)
        {
            conn.Send(message);
        }
    }


    /// <summary>
    /// 连接到服务器
    /// </summary>
    /// <param name="host"></param>
    /// <param name="port"></param>
    public static void ConnectToServer(string host, int port)
    {
        //服务器终端
        IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(host), port);
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(ipe);
        Debug.Log("连接到服务端");
        conn = new Connection(socket);
        conn.OnDisconnected += OnDisconnected;
        //启动消息分发器
        MessageRouter.Instance.Start(4);
    }

    //连接断开
    private static void OnDisconnected(Connection sender)
    {
        Debug.Log("与服务器断开");
    }

    /// <summary>
    /// 关闭网络客户端
    /// </summary>
    public static void Close()
    {
        try
        {
            conn?.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
