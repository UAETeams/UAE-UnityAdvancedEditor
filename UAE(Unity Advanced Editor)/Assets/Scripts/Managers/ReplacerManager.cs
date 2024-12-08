using NUnit.Framework;
using PolyAndCode.UI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class ReplacerManager : MonoBehaviour
{
    [SerializeField]
    private RecyclableScroller RectManager;

    [SerializeField]
    private SelectedFieldInfo selectedFieldInfo;

    public TMP_InputField typeid;
    
    public TMP_InputField pathid;

    public TMP_InputField parrent;

    public TMP_InputField bundle;

    public List<ContactInfo> info = new List<ContactInfo>();

    public GameObject ReferencedField;

    public GameObject Content;

    public void Delete(int cellIndex)
    {
        info.RemoveAt(cellIndex);

        defameData();

        initializeData();

        displayData();
    }
    public void defameData()
    {
        RectManager.RemoveData();

        for(int i = 0; i < Content.transform.childCount; i++) 
        {
            if(Content.transform.GetChild(i).gameObject != ReferencedField)
            {
                GameObject.Destroy(Content.transform.GetChild(i).gameObject);
            }
        }
    }
    public void initializeData()
    {
        RectManager.InitializeData(info.ToList());
    }
    public void displayData()
    {
        RectManager.DisplayData();
    }

    public void showDetails()
    {
        ContactInfo cInfo = selectedFieldInfo.cell._contactInfo;

        typeid.text = "TypeID: " + cInfo.TypeID.ToString();

        pathid.text = "PathID: " +cInfo.pathID.ToString();

        if(cInfo.BundlePath != "")
        {
            bundle.text = "Bundle: " + Path.GetFileName(cInfo.BundlePath);
            parrent.text = "Parrent: " + cInfo.parentPath;
        }
        else
        {
            bundle.text = "Bundle: Nill";
            parrent.text = "Parrent: " + Path.GetFileName(cInfo.parentPath);
        }
    }
}
