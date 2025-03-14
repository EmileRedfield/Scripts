using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class APIClient : MonoBehaviour
{
    private string apiUrl = "http://192.168.42.10:5005/student/servicios/30000080498";  //

    void Start()
    {
        StartCoroutine(GetRequest(apiUrl));
    }

    IEnumerator GetRequest(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            if(webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error en la solicitud: " + webRequest.error);
            }
            else
            {
                Debug.LogError("rta recibida pe: " + webRequest.downloadHandler.text);
            }
        }
    }
    
}