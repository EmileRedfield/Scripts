using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class ServerUI : MonoBehaviour
{

    public int serverPort = 5555;
    [SerializeField] private TcpServer _server;
    [SerializeField] private TMP_InputField messageInput;


    public void SendServerMessage()
    {
        if (!_server.isServerRunning)
        {
            Debug.Log("The server is not running");
            return;
        }

        if (messageInput.text == "")
        {
            Debug.Log("The chat entry is empty");
            return;
        }

        string message = messageInput.text;
        _server.SendData(message);
    }

    public void StartServer()
    {
        _server.StartServer(serverPort);
    }

}