using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UdpServerUI : MonoBehaviour
{
    public int serverPort = 5555;
    [SerializeField] private UDPServer _server;
    [SerializeField] private TMP_InputField messageInput;

    public void SendServerMessage()
    {
        if (!_server.isServerRunning)
        {
            Debug.Log("El servidor no está corriendo");
            return;
        }

        if (messageInput.text == "")
        {
            Debug.Log("El campo de mensaje está vacío");
            return;
        }

        string message = messageInput.text;
        _server.SendData(message);
    }

    public void StartServer()
    {
        _server.StartUDPServer(serverPort);
    }
}
