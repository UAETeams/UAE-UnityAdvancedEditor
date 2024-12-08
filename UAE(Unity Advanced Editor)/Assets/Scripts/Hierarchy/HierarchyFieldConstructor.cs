using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class HierarchyFieldConstructor : MonoBehaviour
{
    public GameObject Field;
    public GameObject Content;

    public void CreateField(bool isExpanded, string Name, GameObject refObject, int index)
    {
        //HierarchyField
        GameObject Object = Instantiate(Field, Content.transform);

        Object.GetComponent<RectTransform>().sizeDelta = new Vector2(Object.GetComponent<RectTransform>().sizeDelta.x + (20f * index), Object.GetComponent<RectTransform>().sizeDelta.y);

        Object.SetActive(true);

        //FieldContent
        int count = Object.transform.GetChild(0).childCount;
        if (isExpanded == true)
        {
            for(int i = 0; i < count; i++)
            {
                if(Object.transform.GetChild(0).GetChild(i).gameObject.name == "Expanded")
                {
                    Object.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
                }
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                if (Object.transform.GetChild(0).GetChild(i).gameObject.name == "Expand")
                {
                    Object.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
                }
            }
        }

        for (int i = 0; i < count; i++)
        {
            if (Object.transform.GetChild(0).GetChild(i).gameObject.name == "FieldInfoText")
            {
                Object.transform.GetChild(0).GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().text = Name;
            }
        }
        Object.GetComponent<HierarchyField>().refGameObject = refObject;
    }
    public void CreateField(string Name, GameObject refObject, int index)
    {
        GameObject Object = Instantiate(Field,Content.transform);

        Object.GetComponent<RectTransform>().sizeDelta = new Vector2(Object.GetComponent<RectTransform>().sizeDelta.x + (20f * index), Object.GetComponent<RectTransform>().sizeDelta.y);

        Object.SetActive(true);
        int count = Object.transform.GetChild(0).childCount;

        for (int i = 0; i < count; i++)
        {
            if (Object.transform.GetChild(0).GetChild(i).gameObject.name == "FieldInfoText")
            {
                Object.transform.GetChild(0).GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().text = Name;
            }
        }
        Object.GetComponent<HierarchyField>().refGameObject = refObject;
    }
    public void CreateAssetField(bool isExpanded, string Name, GameObject refObject, int index, Texture2D identifier)
    {
        //HierarchyField
        GameObject Object = Instantiate(Field, Content.transform);

        Object.transform.Find("Field").Find("None").gameObject.GetComponent<RawImage>().texture = identifier;

        Object.GetComponent<RectTransform>().sizeDelta = new Vector2(Object.GetComponent<RectTransform>().sizeDelta.x + (20f * index), Object.GetComponent<RectTransform>().sizeDelta.y);

        Object.SetActive(true);

        //FieldContent
        int count = Object.transform.GetChild(0).childCount;
        if (isExpanded == true)
        {
            for (int i = 0; i < count; i++)
            {
                if (Object.transform.GetChild(0).GetChild(i).gameObject.name == "Expanded")
                {
                    Object.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
                }
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                if (Object.transform.GetChild(0).GetChild(i).gameObject.name == "Expand")
                {
                    Object.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
                }
            }
        }

        for (int i = 0; i < count; i++)
        {
            if (Object.transform.GetChild(0).GetChild(i).gameObject.name == "FieldInfoText")
            {
                Object.transform.GetChild(0).GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().text = Name;
            }
        }
        Object.GetComponent<HierarchyField>().refGameObject = refObject;
    }
    public void CreateAssetField(string Name, GameObject refObject, int index, Texture2D identifier)
    {
        GameObject Object = Instantiate(Field, Content.transform);

        Object.transform.Find("Field").Find("None").gameObject.GetComponent<RawImage>().texture = identifier;

        Object.GetComponent<RectTransform>().sizeDelta = new Vector2(Object.GetComponent<RectTransform>().sizeDelta.x + (20f * index), Object.GetComponent<RectTransform>().sizeDelta.y);

        Object.SetActive(true);
        int count = Object.transform.GetChild(0).childCount;

        for (int i = 0; i < count; i++)
        {
            if (Object.transform.GetChild(0).GetChild(i).gameObject.name == "FieldInfoText")
            {
                Object.transform.GetChild(0).GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().text = Name;
            }
        }
        Object.GetComponent<HierarchyField>().refGameObject = refObject;
    }
}
