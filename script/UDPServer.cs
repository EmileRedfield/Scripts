using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class UDPServer : MonoBehaviour
{
    private UdpClient udpServer;
    private IPEndPoint remoteEndPoint;
    public bool isServerRunning = false;
    public RawImage receivedImageDisplay; // Asignado en el Inspector

    public void StartUDPServer(int port)
    {
        try
        {
            udpServer = new UdpClient(port);
            remoteEndPoint = new IPEndPoint(IPAddress.Any, port);
            udpServer.BeginReceive(ReceiveData, null);
            Debug.Log("Servidor iniciado en el puerto " + port);
            isServerRunning = true;
        }
        catch (Exception e)
        {
            Debug.LogError("Error al iniciar el servidor: " + e.Message);
        }
    }

    private void ReceiveData(IAsyncResult result)
    {
        byte[] receivedBytes = udpServer.EndReceive(result, ref remoteEndPoint);

        // Si los datos son pequeños, se asume que es un mensaje de texto
        if (receivedBytes.Length < 1000)
        {
            string receivedMessage = System.Text.Encoding.UTF8.GetString(receivedBytes);
            Debug.Log("Mensaje recibido del cliente: " + receivedMessage);
        }
        else
        {
            // Convertir bytes en textura
            Texture2D receivedTexture = new Texture2D(2, 2);
            receivedTexture.LoadImage(receivedBytes);
            DisplayImage(receivedTexture);
        }

        udpServer.BeginReceive(ReceiveData, null);
    }

    public void SendData(string message)
    {
        byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes(message);
        udpServer.Send(sendBytes, sendBytes.Length, remoteEndPoint);
        Debug.Log("Mensaje enviado al cliente: " + message);
    }

    public void SendImage(byte[] imageBytes)
    {
        udpServer.Send(imageBytes, imageBytes.Length, remoteEndPoint);
        Debug.Log("Imagen enviada al cliente");
    }

    private void DisplayImage(Texture2D texture)
    {
        receivedImageDisplay.texture = texture;
    }
}
