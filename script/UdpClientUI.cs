using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UdpClientUI : MonoBehaviour
{
    public int serverPort = 5555;
    public string serverAddress = "127.0.0.1";
    [SerializeField] private UDPClient _client;
    [SerializeField] private TMP_InputField messageInput;
    [SerializeField] private Button sendImageButton;

    public void SendClientMessage()
    {
        if (!_client.isServerConnected)
        {
            Debug.Log("El cliente no está conectado");
            return;
        }

        if (messageInput.text == "")
        {
            Debug.Log("El campo de mensaje está vacío");
            return;
        }

        string message = messageInput.text;
        _client.SendData(message);
    }

    public void ConnectClient()
    {
        _client.StartUDPClient(serverAddress, serverPort);
    }
}
