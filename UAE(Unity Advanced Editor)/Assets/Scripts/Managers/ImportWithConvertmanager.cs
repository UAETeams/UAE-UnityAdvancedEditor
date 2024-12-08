using AssetsTools.NET.Extra;
using UnityEngine;

public class ImportWithConvertmanager : MonoBehaviour
{
    public GameObject ImportTexWindow;
    public ImportText importText;
    public SelectedFieldInfo selectedFieldInfo;
    public Load load;
    public void ImportWithConvert()
    {
        if (selectedFieldInfo.cell.TypeID == AssetClassID.Texture2D.ToString())
        {
            ImportTexWindow.SetActive(true);
        }
        else if (selectedFieldInfo.cell.TypeID == AssetClassID.TextAsset.ToString())
        {
            importText.importText();
        }
        else
        {
            load.ComplainError("This type of asset cannot be imported with conversion. \nTry importing another asset....");
        }
    }
}
