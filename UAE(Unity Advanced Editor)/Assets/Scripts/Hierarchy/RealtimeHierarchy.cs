using System;
using System.Net.Mime;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RealtimeHierarchy : MonoBehaviour
{
    public bool assets = false;
    [SerializeField]
    private GameObject Scene;

    [SerializeField]
    private HierarchyFieldConstructor constructor;

    public GameObject Content;

    public GameObject BaseField;

    private int index;

    private void Initialize(GameObject Object)
    {
        index = 0;
        CalculateIndex(Object,Scene);
        int count = Object.transform.childCount;
        if(count > 0)
        {
            constructor.CreateField(Object.GetComponent<SceneExpand>().isExpanded, Object.name, Object, index);
            if(Object.GetComponent<SceneExpand>().isExpanded == true)
            {
                for(int i = 0; i < count; i++)
                {
                    Initialize(Object.transform.GetChild(i).gameObject);
                }
            }
        }
        else
        {
            constructor.CreateField(Object.name, Object, index);
        }
    }
    private void CleanLayout()
    {
        int count = Content.transform.childCount;
        for(int i = 0; i < count; i++)
        {
            if(Content.transform.GetChild(i).gameObject != BaseField)
                Destroy(Content.transform.GetChild(i).gameObject);
        }
    }
    private void InitializeAsset(GameObject Object)
    {
        index = 0;
        CalculateIndex(Object, Scene);
        if(!Object.GetComponent<SceneExpand>())
        {
            var expand = Object.AddComponent<SceneExpand>();
            expand.isExpanded = false;
        }
        int count = Object.transform.childCount;
        if (count > 0)
        {
            constructor.CreateAssetField(Object.GetComponent<SceneExpand>().isExpanded, Object.name, Object, index, Object.GetComponent<TypeFieldCell>().Type);
            if (Object.GetComponent<SceneExpand>().isExpanded == true)
            {
                for (int i = 0; i < count; i++)
                {
                    InitializeAsset(Object.transform.GetChild(i).gameObject);
                }
            }
        }
        else
        {
            constructor.CreateAssetField(Object.name, Object, index, Object.GetComponent<TypeFieldCell>().Type);
        }
    }
    public void ReInitializeLayout()
    {
        if (assets == false)
        {
            CleanLayout();
            Initialize(Scene);
        }
        else
        {
            CleanLayout();
            InitializeAsset(Scene);
        }
    }
    private void Start()
    {
        if(assets == false)
        {
            CleanLayout();
            Initialize(Scene);
        }
        else
        {
            CleanLayout();
            InitializeAsset(Scene);
        }
    }
    private void CalculateIndex(GameObject ObjectToFind, GameObject refObject)
    {
        Transform tr = ObjectToFind.transform;
        index = 0;
        while(tr != refObject.transform)
        {
            index++;
            tr = tr.parent;
        }
    }
}
