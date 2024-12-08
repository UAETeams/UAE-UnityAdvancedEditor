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

public class JsonExport : MonoBehaviour
{
    public Load load;
    public SelectedFieldInfo selectedFieldInfo;
    public void ExportJson()
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

                                    using MemoryStream ms = new MemoryStream();
                                    StreamWriter sw = new StreamWriter(ms);

                                    AssetImportExport impexp = new AssetImportExport();
                                    impexp.DumpJsonAsset(sw, atvf);

                                    sw.Flush();
                                    ms.Position = 0;

                                    string str = Encoding.UTF8.GetString(ms.ToArray());
                                    StartCoroutine(save(str));
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

                        using MemoryStream ms = new MemoryStream();
                        StreamWriter sw = new StreamWriter(ms);

                        AssetImportExport impexp = new AssetImportExport();
                        impexp.DumpJsonAsset(sw, atvf);

                        sw.Flush();
                        ms.Position = 0;

                        string str = Encoding.UTF8.GetString(ms.ToArray());
                        StartCoroutine(save(str));
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
    IEnumerator save(string bytes)
    {
        yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Save Your Json File", "Save");
        try
        {
            string path = FileBrowser.Result[0] + ".json";
            File.WriteAllText(path, bytes);

            load.ComplainError("Exported Json Successfully.");
        }
        catch (Exception ex)
        {
            load.ComplainError(ex.ToString());
        }
    }
}
