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
using UAE.Text;
using UnityEngine.UIElements;

public class ViewText : MonoBehaviour
{
    public Load load;
    public SelectedFieldInfo selectedFieldInfo;

    public TextMeshProUGUI textTextAsset;
    public void ViewTextAsset()
    {
        if (selectedFieldInfo.cell != null && selectedFieldInfo.cell.TypeID == AssetClassID.TextAsset.ToString())
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

                                    TextAssetLibrary textasset = new TextAssetLibrary();
                                    textasset.ReadTextAsset(atvf);
                                    
                                    this.textTextAsset.text = Encoding.UTF8.GetString(textasset.textassetdat);
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

                        TextAssetLibrary textasset = new TextAssetLibrary();
                        textasset.ReadTextAsset(atvf);

                        this.textTextAsset.text = Encoding.UTF8.GetString(textasset.textassetdat);

                    }
                }
            }
            manager.UnloadAll();
        }
        else
        {
            load.ComplainError("Either no valid asset is selected or no asset is selected or the selected asset is not of type AssetClassID.TextAsset");
        }
    }
}
