using AssetsTools.NET;
using AssetsTools.NET.Texture;
using AssetsTools.NET.Extra;
using UnityEngine;
using System.IO;
using System;
using System.Collections;
using UAE.IO;
using SimpleFileBrowser;
using System.Text;

public class ImportText : MonoBehaviour
{
    public Load load;

    public SelectedFieldInfo selectedFieldInfo;

    public byte[] importedText;

    public ReplacerManager RManager;

    public GameObject settings;

    public void importText()
    {
        StartCoroutine(Load());
    }
    IEnumerator Load()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Your TextAsset bytes", "Load");

        if (FileBrowser.Success)
        {
            this.importedText = File.ReadAllBytes(FileBrowser.Result[0]);

            if (selectedFieldInfo.cell != null && selectedFieldInfo.cell.TypeID != AssetClassID.Object.ToString() || selectedFieldInfo.cell.TypeID != "")
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
                                        try
                                        {
                                            atvf["m_Script"].AsByteArray = importedText;

                                            var newGoBytes = atvf.WriteToByteArray();

                                            ContactInfo Cinfo = new ContactInfo();

                                            Cinfo.Replacer = true;
                                            Cinfo.formatter = false;

                                            Cinfo.TypeID = (AssetClassID)AFinfo.TypeId;
                                            Cinfo.pathID = AFinfo.PathId;
                                            Cinfo.parentPath = info.parentPath;
                                            Cinfo.BundlePath = info.BundlePath;
                                            Cinfo.replacedAssetBytes = newGoBytes;

                                            settings.SetActive(true);
                                            RManager.info.Add(Cinfo);

                                            {
                                                RManager.defameData();
                                                RManager.initializeData();
                                                RManager.displayData();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            load.ComplainError(ex.ToString());
                                        }
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
                            try
                            {
                                atvf["m_Script"].AsByteArray = importedText;

                                var newGoBytes = atvf.WriteToByteArray();

                                ContactInfo Cinfo = new ContactInfo();

                                Cinfo.Replacer = true;
                                Cinfo.formatter = false;

                                Cinfo.TypeID = (AssetClassID)AFinfo.TypeId;
                                Cinfo.pathID = AFinfo.PathId;
                                Cinfo.parentPath = info.parentPath;
                                Cinfo.BundlePath = "";
                                Cinfo.replacedAssetBytes = newGoBytes;

                                settings.SetActive(true);
                                RManager.info.Add(Cinfo);

                                {
                                    RManager.defameData();
                                    RManager.initializeData();
                                    RManager.displayData();
                                }
                            }
                            catch (Exception ex)
                            {
                                load.ComplainError(ex.ToString());
                            }
                        }
                    }
                }
                manager.UnloadAll();
            }
            else
            {
                load.ComplainError("Either no asset is selected or the selected asset is not a valid Asset or not a TextAsset");
            }
        }
    }

}
