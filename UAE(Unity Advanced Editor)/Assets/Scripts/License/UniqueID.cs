using TMPro;
using UnityEngine;

public class UniqueID : MonoBehaviour
{
    public TMP_InputField text;
    void Start()
    {
        text.text = SystemInfo.deviceUniqueIdentifier;
    }
}
