using AssetsTools.NET.Extra;
using UnityEngine;
using UnityEngine.UI;

public class Viewer : MonoBehaviour
{
    public TexViewer viewer;
    public Button TexWindow;
    public GameObject TextureViewportLayout;
    public ViewAudio Aviewer;
    public Button AudioWindow;
    public GameObject AudioViewportLayout;
    public JsonViewer jsonViewer;
    public Button jsonWindow;
    public GameObject JsonViewportLayout;
    public ViewText textViewer;
    public Button textAssetWindow;
    public GameObject TextViewportLayout;
    public MatViewer matViewer;
    public GameObject MatViewportLayout;
    public Button matWindow;

    public GameObject InfoViewportLayout;
    public Button infoWindow;
    public FieldInspector inspector;
    public SelectedFieldInfo info;
    public void UpdateField()
    {
        if (info != null)
        {
            inspector.ShowInspectorValues();

            if (info.cell.TypeID == AssetClassID.Texture2D.ToString())
            {
                setNullLayout();
                TextureViewportLayout.SetActive(true);
                TexWindow.onClick.Invoke();
                {
                    viewer.viewTex2d();
                    jsonViewer.ViewJson();
                }
            }
            else if (info.cell.TypeID == AssetClassID.AudioClip.ToString())
            {
                setNullLayout();
                AudioViewportLayout.SetActive(true);
                AudioWindow.onClick.Invoke();
                {
                    Aviewer.viewAudioClip();
                    jsonViewer.ViewJson();
                }
            }
            else if(info.cell.TypeID == AssetClassID.TextAsset.ToString())
            {
                setNullLayout();
                TextViewportLayout.SetActive(true);
                textAssetWindow.onClick.Invoke();
                {
                    textViewer.ViewTextAsset();
                    jsonViewer.ViewJson();
                }
            }
            else if(info.cell.TypeID == AssetClassID.Material.ToString())
            {
                setNullLayout();
                MatViewportLayout.SetActive(true);
                matWindow.onClick.Invoke();
                {
                    matViewer.ViewMat();
                    jsonViewer.ViewJson();
                }
            }
            else if(info.cell.TypeID == "" || info.cell.TypeID == AssetClassID.Object.ToString())
            {
                setNullLayout();
                InfoViewportLayout.SetActive(true);
                infoWindow.onClick.Invoke();
            }
            else
            {
                setNullLayout();
                JsonViewportLayout.SetActive(true);
                jsonWindow.onClick.Invoke();
                {
                    jsonViewer.ViewJson();
                }
            }

        }
    }
    private void setNullLayout()
    {
        TextureViewportLayout.SetActive(false);
        AudioViewportLayout.SetActive(false);
        InfoViewportLayout.SetActive(false);
        JsonViewportLayout.SetActive(false);
        TextViewportLayout.SetActive(false);
        MatViewportLayout.SetActive(false);
    }
}
