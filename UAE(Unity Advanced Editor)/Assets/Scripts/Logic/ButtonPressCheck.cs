using UnityEngine;
using UnityEngine.UIElements;

public class ButtonPressCheck : MonoBehaviour
{
    public UnityEngine.UI.Toggle toggle;
    public GameObject Object;
    void Update()
    {
        if(toggle.isOn)
        {
            Object.SetActive(true);
        }
        else
        {
            Object.SetActive(false);
        }
    }
}
