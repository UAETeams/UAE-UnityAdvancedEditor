using AssetsTools.NET.Extra;
using AssetsTools.NET;
using System.IO;
using UAE.IO;
using UnityEngine;
using TMPro;
using System.Text;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SimpleFileBrowser;
using System.Collections;

public class ExportRaw : MonoBehaviour
{
    public Load load;
    public SelectedFieldInfo selectedFieldInfo;
    public void ExportRawAsset()
    {
        if (selectedFieldInfo.cell != null && selectedFieldInfo.cell.TypeID != AssetClassID.Object.ToString())
        {
            ContactInfo info = selectedFieldInfo.cell._contactInfo;
            var manager = new AssetsManager();

            manager.LoadClassPackage(Path.Combine(Application.persistentDataPath, "classdata.tpk"));

            if (info.BundlePath != "")
            {
                manager.LoadClassPackage(Path.Combine(Application.persistentDataPath, "classdata.tpk"));

                var bunfileInst = manager.LoadBundleFile(info.BundlePath, true);


                foreach (AssetBundleDirectoryInfo assetBundleDirectoryInfo in bunfileInst.file.BlockAndDirInfo.DirectoryInfos)
                {
                    string assetsFileName = assetBundleDirectoryInfo.Name;
                    if (!assetsFileName.EndsWith(".resource") && !assetsFileName.EndsWith(".resS"))
                    {
                        var afileInst = manager.LoadAssetsFileFromBundle(bunfileInst, assetBundleDirectoryInfo.Name, false);

                        var afile = afileInst.file;

                        afile.GenerateQuickLookup();

                        if (StaticSettings.unityMetadata != "")
                        {
                            manager.LoadClassDatabaseFromPackage(StaticSettings.unityMetadata);
                        }
                        else
                        {
                            manager.LoadClassDatabaseFromPackage(afile.Metadata.UnityVersion);
                        }

                        if (afileInst.name == info.parentPath)
                        {
                            foreach (AssetFileInfo AFinfo in afileInst.file.AssetInfos)
                            {
                                if (AFinfo.PathId == info.pathID && (AssetClassID)AFinfo.TypeId == info.TypeID)
                                {
                                    AssetTypeValueField atvf = new AssetTypeValueField();
                                    atvf = manager.GetBaseField(afileInst, AFinfo);

                                    byte[] str = atvf.WriteToByteArray();
                                    StartCoroutine(save(str, info.Name));
                                }
                            }
                        }
                    }
                }
                manager.UnloadAll();
            }
            else if (info.parentPath != "")
            {
                AssetsFileInstance AFinst = manager.LoadAssetsFile(info.parentPath);
                AFinst.file.GenerateQuickLookup();

                if (StaticSettings.unityMetadata != "")
                {
                    manager.LoadClassDatabaseFromPackage(StaticSettings.unityMetadata);
                }
                else
                {
                    manager.LoadClassDatabaseFromPackage(AFinst.file.Metadata.UnityVersion);
                }

                foreach (AssetFileInfo AFinfo in AFinst.file.AssetInfos)
                {
                    if (AFinfo.PathId == info.pathID && (AssetClassID)AFinfo.TypeId == info.TypeID)
                    {
                        AssetTypeValueField atvf = new AssetTypeValueField();
                        atvf = manager.GetBaseField(AFinst, AFinfo);
                        byte[] str = atvf.WriteToByteArray();
                        StartCoroutine(save(str, info.Name));
                    }
                }
            }
            manager.UnloadAll();
        }
        if(selectedFieldInfo.cell.TypeID == AssetClassID.Object.ToString())
        {
            load.ComplainError("This is not a valid Asset");
        }
    }
    IEnumerator save(byte[] bytes, string name)
    {
        yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.FilesAndFolders, true, null, name, "Save Your Json File", "Save");
        try
        {
            string path = FileBrowser.Result[0] + ".dat";
            File.WriteAllBytes(path, bytes);

            load.ComplainError("Exported Json Successfully.");
        }
        catch (Exception ex)
        {
            load.ComplainError(ex.ToString());
        }
    }
}
