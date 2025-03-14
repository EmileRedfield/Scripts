using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ImageSelector : MonoBehaviour
{
    public UDPClient udpClient;  // Asignar en el Inspector
    public RawImage selectedImageDisplay;  // Asignar en el Inspector

    public void SelectImage()
    {
        // Verifica si udpClient está asignado
        if (udpClient == null)
        {
            Debug.LogError("udpClient no está asignado en el Inspector.");
            return;
        }

        // Verifica si selectedImageDisplay está asignado
        if (selectedImageDisplay == null)
        {
            Debug.LogError("selectedImageDisplay no está asignado en el Inspector.");
            return;
        }

        // Usar UnityEditor solo funciona en el Editor, no en compilaciones
        #if UNITY_EDITOR
        string path = UnityEditor.EditorUtility.OpenFilePanel("Seleccionar imagen", "", "png,jpg,jpeg");

        if (!string.IsNullOrEmpty(path))
        {
            byte[] imageBytes = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);
            selectedImageDisplay.texture = texture;

            // Enviar imagen al servidor
            udpClient.SendImage(imageBytes);
        }
        else
        {
            Debug.Log("No se seleccionó ninguna imagen.");
        }
        #else
        Debug.LogError("La selección de imágenes solo funciona en el Editor de Unity.");
        #endif
    }
}
