using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class TcpServer : MonoBehaviour
{
    private TcpListener tcpListener;
    private TcpClient connectedClient;
    private NetworkStream networksStream;
    private byte[] receiveBuffer;

    public bool isServerRunning;


    public void StartServer(int port)
    {
        tcpListener = new TcpListener(IPAddress.Any, port);
        tcpListener.Start();
        Debug.Log("Severd Started, waiting for connections...");
        tcpListener.BeginAcceptTcpClient(HandleIncomingConnection, null);
        isServerRunning = true;
    }

    private void HandleIncomingConnection(IAsyncResult result)
    {
        connectedClient = tcpListener.EndAcceptTcpClient(result);
        networksStream = connectedClient.GetStream();
        Debug.Log("Client connected: " + connectedClient);
        receiveBuffer = new byte[connectedClient.ReceiveBufferSize];
        networksStream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, ReceiveData, null);
        tcpListener.BeginAcceptTcpClient(HandleIncomingConnection, null);



    }

    private void ReceiveData(IAsyncResult result)
    {
        int bytesRead = networksStream.EndRead(result);

        if (bytesRead <= 0)
        {
            Debug.Log("Client disconnected: " + connectedClient.Client.RemoteEndPoint);
            connectedClient.Close();
            return;
        }
        byte[] receivedBytes = new byte[bytesRead];
        Array.Copy(receiveBuffer, receivedBytes, bytesRead);
        string receivedMessage = System.Text.Encoding.UTF8.GetString(receivedBytes);
        Debug.Log("Received from client: " + receivedMessage);
        networksStream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, ReceiveData, null);
    }

    public void SendData(string message)
    {
        try
        {
            byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes(message);
            networksStream.Write(sendBytes, 0, sendBytes.Length);
            networksStream.Flush();
            Debug.Log("Sent to client: " + message);
        }
        catch
        {

            Debug.Log("There is no client to send the message:" + message);

        }
    }

    void Start()
    {

    }


    void Update()
    {

    }
}