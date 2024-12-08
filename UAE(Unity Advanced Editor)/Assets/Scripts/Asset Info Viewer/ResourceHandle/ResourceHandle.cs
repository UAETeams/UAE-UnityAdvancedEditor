using TMPro;
using UnityEngine;

public class ResourceHandle : MonoBehaviour
{
    public GameObject ResourceWindow;
    public TextMeshProUGUI requiredResText;
    public GameObject InAccessible;

    public void OpenResourceWindow(string text)
    {
        //if (UnityEngine.Application.platform == RuntimePlatform.Android)
        {
            ResourceWindow.SetActive(true);
            requiredResText.text = text;
        }
        /*else
        {
            InAccessible.SetActive(true);
            NetResourceLoad netResourceLoad = new NetResourceLoad(text);
            BaseClient client = FindObjectOfType<BaseClient>();
            client.SendToServer(netResourceLoad);
        }*/
    }
}
