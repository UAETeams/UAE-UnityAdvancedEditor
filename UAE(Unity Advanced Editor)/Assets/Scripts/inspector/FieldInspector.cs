using AssetsTools.NET.Extra;
using System.Security.Policy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FieldInspector : MonoBehaviour
{
    public SelectedFieldInfo selectedFieldInfo;


    public TMP_InputField cellIndex;
    public TMP_InputField Name;
    public TMP_InputField PathId;
    public TMP_InputField ParentPath;
    public TMP_InputField BundlePath;
    public TMP_InputField ReferencePath;
    public TMP_InputField AssetClassId;

    public RawImage icon;

    public Texture2D basicTex;

    public Load load;

    public void ShowInspectorValues()
    {
        if (selectedFieldInfo.cell != null)
        {
            ContactInfo info = selectedFieldInfo.cell._contactInfo;

            cellIndex.text = selectedFieldInfo.cell._cellIndex.ToString();

            Name.text = info.Name;
            PathId.text = info.pathID.ToString();
            ParentPath.text = info.parentPath;
            BundlePath.text = info.BundlePath;
            ReferencePath.text = info.refPath;
            AssetClassID id = (AssetClassID)info.TypeID;

            icon.texture = info.Type;

            AssetClassId.text = id.ToString();

        }
        else
        {

            cellIndex.text = "0";

            Name.text = "NULL";
            PathId.text = "0";
            ParentPath.text = "NULL";
            BundlePath.text = "NULL";
            ReferencePath.text = "NULL";

            icon.texture = basicTex;

            AssetClassId.text = "NULL";

        }
    }
}
