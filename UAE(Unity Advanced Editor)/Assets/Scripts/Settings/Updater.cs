using UnityEngine;
using TMPro;
using static StaticSettings;

public class Updater : MonoBehaviour
{
    public TMPro.TMP_InputField inputField;

    private void Update()
    {
        StaticSettings.unityMetadata = inputField.text;
    }
}
