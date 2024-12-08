using SimpleFileBrowser;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using AssetsTools.NET;
using AssetsTools;
using System.IO;
using AssetsTools.NET.Extra;
using System.Collections.Generic;


public class SaveManager : MonoBehaviour
{
    public ReplacerManager Rmanager;
    public GameObject replacerWindow;
    public ReplacerRemover remover;
    public Load load;

    public void Save()
    {
        replacerWindow.SetActive(true);
        int count = 0;

        foreach (ContactInfo inf in Rmanager.info)
        {
            count++;
            AssetsManager manager = new AssetsManager();

            manager.LoadClassPackage(Path.Combine(Application.persistentDataPath, "classdata.tpk"));

            if (inf.BundlePath != "")
            {
                var bunfileInst = manager.LoadBundleFile(inf.BundlePath, true);

                foreach (AssetBundleDirectoryInfo assetBundleDirectoryInfo in bunfileInst.file.BlockAndDirInfo.DirectoryInfos)
                {
                    if(assetBundleDirectoryInfo.Name == inf.parentPath)
                    {
                        var AFinst = manager.LoadAssetsFileFromBundle(bunfileInst, assetBundleDirectoryInfo.Name, false);

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
                            if (AFinfo.PathId == inf.pathID && (AssetClassID)AFinfo.TypeId == inf.TypeID)
                            {
                                var repl = new AssetsReplacerFromMemory(AFinst.file, AFinfo, inf.replacedAssetBytes);

                                //write changes to memory
                                byte[] newAssetData;
                                using (var stream = new MemoryStream())
                                using (var writer = new AssetsFileWriter(stream))
                                {
                                    AFinst.file.Write(writer, 0, new List<AssetsReplacer>() { repl });
                                    newAssetData = stream.ToArray();
                                    stream.Close();
                                }

                                var bunRepl = new BundleReplacerFromMemory(AFinst.name, AFinst.name, true, newAssetData, -1);
                                MemoryStream ms = new MemoryStream();
                                var bunWriter = new AssetsFileWriter(ms);
                                bunfileInst.file.Write(bunWriter, new List<BundleReplacer>() { bunRepl });

                                byte[] bundleBytes = ms.ToArray();
                                ms.Close();

                                manager.UnloadAll();
                                StartCoroutine(save(bundleBytes, Path.GetFullPath(inf.BundlePath), Path.GetFileName(inf.BundlePath), count));
                            }
                        }
                    }
                }
            }
            else if (inf.parentPath != "")
            {
                AssetsFileInstance AFinst = manager.LoadAssetsFile(inf.parentPath);
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
                    if (AFinfo.PathId == inf.pathID && (AssetClassID)AFinfo.TypeId == inf.TypeID)
                    {
                        var repl = new AssetsReplacerFromMemory(AFinst.file, AFinfo, inf.replacedAssetBytes);

                        //write changes to memory
                        byte[] newAssetData;
                        using (var stream = new MemoryStream())
                        using (var writer = new AssetsFileWriter(stream))
                        {
                            AFinst.file.Write(writer, 0, new List<AssetsReplacer>() { repl });
                            newAssetData = stream.ToArray();
                            stream.Close();
                        }
                        manager.UnloadAll();
                        StartCoroutine(save(newAssetData, Path.GetFullPath(inf.parentPath), Path.GetFileName(inf.parentPath),count));
                    }
                }
            }
        }
        //remover.RemoveAll();
    }
    IEnumerator save(byte[] bytes, string ipath, string filename, int index)
    {
        yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.FilesAndFolders, false, ipath, filename, "Save Files and Folders", "Save");
        try
        {
            string path = FileBrowser.Result[0];
            File.WriteAllBytes(path, bytes);
            remover.RemoveAt(index - 1);

            load.ComplainError("Exported File Successfully.");
        }
        catch (Exception ex)
        {
            load.ComplainError(ex.ToString());
        }
    }

}
