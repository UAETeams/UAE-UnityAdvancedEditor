using AssetsTools.NET.Extra;
using UnityEngine;

public class ExportWithConvertManager : MonoBehaviour
{
    public ExportTex exportTex;
    public ExportAudio exportAudio;
    public ExportText exportText;
    public SelectedFieldInfo selectedFieldInfo;
    public Load load;

    public void ExportWithConvert()
    {
        if(selectedFieldInfo.cell.TypeID == AssetClassID.Texture2D.ToString())
        {
            exportTex.exportTex2d();
        }
        if (selectedFieldInfo.cell.TypeID == AssetClassID.AudioClip.ToString())
        {
            exportAudio.ExportAudioClip();
        }
        if(selectedFieldInfo.cell.TypeID == AssetClassID.TextAsset.ToString())
        {
            exportText.ExportTextAsset();
        }
        else
        {
            load.ComplainError("This type of asset cannot be exported with conversion. \nTry Exporting another asset....");
        }
    }
}
