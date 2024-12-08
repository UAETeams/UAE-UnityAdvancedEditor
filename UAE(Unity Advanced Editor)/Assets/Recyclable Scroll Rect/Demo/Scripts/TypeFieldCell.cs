using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;
using TMPro;

//Cell class for demo. A cell in Recyclable Scroll Rect must have a cell class inheriting from ICell.
//The class is required to configure the cell(updating UI elements etc) according to the data during recycling of cells.
//The configuration of a cell is done through the DataSource SetCellData method.
//Check RecyclableScrollerDemo class
public class TypeFieldCell : MonoBehaviour, ICell
{
    //UI
    public TextMeshProUGUI Text;
    public RawImage Image;

    //Model
    public ContactInfo _contactInfo;
    public int _cellIndex;

    public string Name;
    public Texture2D Type;
    public long pathID;
    public string parentPath;
    public string BundlePath;
    public string refPath;
    public string TypeID;
    public bool UnknownAsset;

    public TextMeshProUGUI replacerTextIndex;

    //This is called from the SetCell method in DataSource
    public void ConfigureCell(ContactInfo contactInfo,int cellIndex)
    {
        this._cellIndex = cellIndex;
        this._contactInfo = contactInfo;

        if (contactInfo.Replacer == false && contactInfo.formatter == false)
        {
            {
                Text.text = contactInfo.Name;
                Image.texture = contactInfo.Type;
            }


            Name = contactInfo.Name;
            Type = contactInfo.Type;
            pathID = contactInfo.pathID;
            parentPath = contactInfo.parentPath;
            BundlePath = contactInfo.BundlePath;
            refPath = contactInfo.refPath;
            TypeID = contactInfo.TypeID.ToString();
            UnknownAsset = contactInfo.UnknownAsset;
        }
        else if(contactInfo.formatter == false && contactInfo.Replacer == true)
        {
            replacerTextIndex.text = "Reference no: " + cellIndex.ToString();
        }
        else if(contactInfo.formatter == true && contactInfo.Replacer == false)
        {
            replacerTextIndex.text = contactInfo.Textureformat;
        }
    }
    public void ConfigueHierarchyCell(ContactInfo contactInfo)
    {
        this._cellIndex = 0;
        this._contactInfo = contactInfo;
        {
            Name = contactInfo.Name;
            Type = contactInfo.Type;
            pathID = contactInfo.pathID;
            parentPath = contactInfo.parentPath;
            BundlePath = contactInfo.BundlePath;
            refPath = contactInfo.refPath;
            TypeID = contactInfo.TypeID.ToString();
            UnknownAsset = contactInfo.UnknownAsset;
        }
    }
}
