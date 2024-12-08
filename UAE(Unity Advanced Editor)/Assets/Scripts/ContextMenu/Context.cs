using UnityEngine;
using UnityEngine.UI;
using Z.ContextMenu;

public class Context : MonoBehaviour, IContextMenu
{
    public Button Orig;

    public ExportWithConvertManager exportWithConvertManager;
    public ImportWithConvertmanager importWithConvertmanager;

    public ExportRaw exportRaw;
    public ImportRaw importRaw;

    public JsonExport jsonExport;
    public JsonImport jsonImport;

    public Load load;
    public Z.ContextMenu.ContextMenu menu;
    public void BuildContextMenu(PrefabProxy prefabs)
    {
        try
        {
            Orig.onClick.Invoke();

            Button exportWithConvert = prefabs.GetButton("Export format");
            exportWithConvert.onClick?.AddListener(exportWithConvertManager.ExportWithConvert);
            exportWithConvert.onClick?.AddListener(menu.Pop);
            Button importWithConvert = prefabs.GetButton("Import format");
            importWithConvert.onClick?.AddListener(importWithConvertmanager.ImportWithConvert);
            importWithConvert.onClick?.AddListener(menu.Pop);

            Button exportRawB = prefabs.GetButton("Export Raw");
            exportRawB.onClick?.AddListener(exportRaw.ExportRawAsset);
            exportRawB.onClick?.AddListener(menu.Pop);
            Button importRawB = prefabs.GetButton("Import Raw");
            importRawB.onClick?.AddListener(importRaw.importRaw);
            importRawB.onClick?.AddListener(menu.Pop);

            Button ExportJson = prefabs.GetButton("Export Json");
            ExportJson.onClick?.AddListener(jsonExport.ExportJson);
            ExportJson.onClick?.AddListener(menu.Pop);
            Button ImportJson = prefabs.GetButton("Import Json");
            ImportJson.onClick?.AddListener(jsonImport.importJson);
            ImportJson.onClick?.AddListener(menu.Pop);

            Button View3dScene = prefabs.GetButton("View 3dScene");
            View3dScene.onClick?.AddListener(err);
        }
        catch(System.Exception ex)
        {
            load.ComplainError(ex.ToString());
            menu.Pop();
        }
    }
    void err()
    {
        load.ComplainError("Not Yet Implemented");
        menu.Pop();
    }

}
