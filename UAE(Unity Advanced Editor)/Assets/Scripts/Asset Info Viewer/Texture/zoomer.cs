using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class zoomer : MonoBehaviour
{
    public RawImage texture2d;
    public GameObject Object;

    public void indec()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            texture2d.rectTransform.sizeDelta = new Vector2(texture2d.rectTransform.sizeDelta.x * 1.1f, texture2d.rectTransform.sizeDelta.y * 1.1f);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            texture2d.rectTransform.sizeDelta = new Vector2(texture2d.rectTransform.sizeDelta.x * 0.9f, texture2d.rectTransform.sizeDelta.y * 0.9f);
        }
    }
    public void indecGO()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            Object.transform.localScale = new Vector3(Object.transform.localScale.x * 1.1f, Object.transform.localScale.y * 1.1f, Object.transform.localScale.z * 1.1f);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            Object.transform.localScale = new Vector3(Object.transform.localScale.x * 0.9f, Object.transform.localScale.y * 0.9f, Object.transform.localScale.z * 0.9f);
        }
    }
    public void retGo()
    {
        if(Object != null)
        {
            Object.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    public void retTex()
    {
        if(texture2d != null)
        {
            texture2d.rectTransform.sizeDelta = new Vector2(200, 200);
        }
    }
}
