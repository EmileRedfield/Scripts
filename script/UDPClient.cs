using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class UDPClient : MonoBehaviour
{
    private UdpClient udpClient;
    private IPEndPoint remoteEndPoint;
    public bool isServerConnected = false;
    public RawImage receivedImageDisplay; // Asignado en el Inspector

    public void StartUDPClient(string ipAddress, int port)
    {
        udpClient = new UdpClient();
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
        udpClient.BeginReceive(ReceiveData, null);
        Debug.Log("Cliente conectado al servidor");
        isServerConnected = true;
        udpClient.Client.ReceiveBufferSize = 500000; // Tamaño máximo permitido
        udpClient.Client.SendBufferSize = 500000;
    }

    private void ReceiveData(IAsyncResult result)
    {
        byte[] receivedBytes = udpClient.EndReceive(result, ref remoteEndPoint);

        // Si los datos son pequeños, se asume que es un mensaje de texto
        if (receivedBytes.Length < 1000)
        {
            string receivedMessage = System.Text.Encoding.UTF8.GetString(receivedBytes);
            Debug.Log("Mensaje recibido: " + receivedMessage);
        }
        else
        {
            // Convertir bytes en textura
            Texture2D receivedTexture = new Texture2D(2, 2);
            receivedTexture.LoadImage(receivedBytes);
            DisplayImage(receivedTexture);
        }

        udpClient.BeginReceive(ReceiveData, null);
    }

    public void SendData(string message)
    {
        byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes(message);
        udpClient.Send(sendBytes, sendBytes.Length, remoteEndPoint);
        Debug.Log("Mensaje enviado: " + message);
    }

    public void SendImage(byte[] imageBytes)
    {
        Debug.Log(imageBytes.Length);
        udpClient.Send(imageBytes, imageBytes.Length, remoteEndPoint);
        Debug.Log("Imagen enviada");
        
    }

    private void DisplayImage(Texture2D texture)
    {
        receivedImageDisplay.texture = texture;
    }
}

