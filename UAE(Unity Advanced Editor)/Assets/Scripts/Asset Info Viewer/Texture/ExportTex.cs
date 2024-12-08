using AssetsTools.NET;
using AssetsTools.NET.Texture;
using AssetsTools.NET.Extra;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;
using System.Collections;
using SimpleFileBrowser;
public class ExportTex : MonoBehaviour
{
    public Load load;
    public SelectedFieldInfo selectedFieldInfo;

    public bool isCurrentFunction;

    public ResourceHandle resHandle;
    public void exportTex2d()
    {
        if (selectedFieldInfo.cell != null && selectedFieldInfo.cell.TypeID == AssetClassID.Texture2D.ToString())
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

                                    TextureFile tf = TextureFile.ReadTextureFile(atvf);
                                    if (tf.pictureData.Length == null || tf.pictureData.Length == 0)
                                    {
                                        {
                                            TextureFile.StreamingInfo streamInfo = tf.m_StreamData;
                                            if (streamInfo.path != null && streamInfo.path != "")
                                            {
                                                string searchPath = streamInfo.path;
                                                if (searchPath.StartsWith("archive:/"))
                                                    searchPath = searchPath.Substring(9);

                                                searchPath = Path.GetFileName(searchPath);

                                                AssetBundleFile bundle = afileInst.parentBundle.file;

                                                AssetsFileReader reader = bundle.Reader;
                                                AssetBundleDirectoryInfo[] dirInf = bundle.BlockAndDirInfo.DirectoryInfos;
                                                for (int i = 0; i < dirInf.Length; i++)
                                                {
                                                    AssetBundleDirectoryInfo ABDInfo = dirInf[i];
                                                    if (ABDInfo.Name == searchPath)
                                                    {
                                                        reader.Position = bundle.Header.GetFileDataOffset() + ABDInfo.Offset + (long)streamInfo.offset;
                                                        tf.pictureData = reader.ReadBytes((int)streamInfo.size);
                                                        tf.m_StreamData.offset = 0;
                                                        tf.m_StreamData.size = 0;
                                                        tf.m_StreamData.path = "";
                                                    }
                                                }

                                                try
                                                {
                                                    byte[] ARGB32Bytes = tf.DecodeTextureRaw(tf.pictureData);

                                                    UnityEngine.Texture2D tex = new UnityEngine.Texture2D(tf.m_Width, tf.m_Height, UnityEngine.TextureFormat.BGRA32, false);
                                                    tex.LoadRawTextureData(ARGB32Bytes);
                                                    tex.Apply();

                                                    Texture2D simpletex = new Texture2D(tex.width, tex.height);
                                                    simpletex.SetPixels(tex.GetPixels());
                                                    simpletex.Apply();

                                                    byte[] picture = simpletex.EncodeToPNG();

                                                    StartCoroutine(save(picture));
                                                }
                                                catch(Exception ex)
                                                {
                                                    ex.ToString();
                                                }
                                            }
                                            else
                                            {
                                                load.ComplainError("This texture has no texture bytes.");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            byte[] ARGB32Bytes = tf.DecodeTextureRaw(tf.pictureData);

                                            UnityEngine.Texture2D tex = new UnityEngine.Texture2D(tf.m_Width, tf.m_Height, UnityEngine.TextureFormat.BGRA32, false);
                                            tex.LoadRawTextureData(ARGB32Bytes);
                                            tex.Apply();

                                            Texture2D simpletex = new Texture2D(tex.width, tex.height);
                                            simpletex.SetPixels(tex.GetPixels());
                                            simpletex.Apply();

                                            byte[] picture = simpletex.EncodeToPNG();

                                            StartCoroutine(save(picture));
                                        }
                                        catch (Exception ex)
                                        {
                                            ex.ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                manager.UnloadAll();
            }
            else if(info.parentPath != "")
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

                        TextureFile tf = TextureFile.ReadTextureFile(atvf);
                        if (tf.pictureData.Length == null || tf.pictureData.Length == 0)
                        {
                            {
                                TextureFile.StreamingInfo streamInfo = tf.m_StreamData;
                                if (streamInfo.path != null && streamInfo.path != "")
                                {
                                    string searchPath = streamInfo.path;
                                    if (searchPath.StartsWith("archive:/"))
                                        searchPath = searchPath.Substring(9);

                                    searchPath = Path.GetFileName(searchPath);

                                    resHandle.OpenResourceWindow("Name:- " + searchPath);
                                    isCurrentFunction = true;


                                }
                                else
                                {
                                    load.ComplainError("This texture has no texture bytes.");
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                byte[] ARGB32Bytes = tf.DecodeTextureRaw(tf.pictureData);

                                UnityEngine.Texture2D tex = new UnityEngine.Texture2D(tf.m_Width, tf.m_Height, UnityEngine.TextureFormat.BGRA32, false);
                                tex.LoadRawTextureData(ARGB32Bytes);
                                tex.Apply();

                                Texture2D simpletex = new Texture2D(tex.width, tex.height);
                                simpletex.SetPixels(tex.GetPixels());
                                simpletex.Apply();

                                byte[] picture = simpletex.EncodeToPNG();

                                StartCoroutine(save(picture));
                            }
                            catch (Exception ex)
                            {
                                ex.ToString();
                            }
                        }
                    }
                }
            }
            manager.UnloadAll();
        }
        else
        {
            load.ComplainError("Either no asset is selected or the selected asset is not a texture2D");
        }
    }

    IEnumerator save(byte[] bytes)
    {
        yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Save Files and Folders", "Save");
        try
        {
            string path = FileBrowser.Result[0] + ".png";
            File.WriteAllBytes(path, bytes);

            load.ComplainError("Exported Texture Successfully.");
        }
        catch(Exception ex)
        {
            load.ComplainError(ex.ToString());
        }
    }
    public void exportTex2dRest(byte[] resourceBytes)
    {
        if (selectedFieldInfo.cell != null && selectedFieldInfo.cell.TypeID == AssetClassID.Texture2D.ToString())
        {
            ContactInfo info = selectedFieldInfo.cell._contactInfo;
            var manager = new AssetsManager();

            manager.LoadClassPackage(Path.Combine(Application.persistentDataPath, "classdata.tpk"));

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
                        AssetTypeValueField atvf = manager.GetBaseField(AFinst, AFinfo);

                        TextureFile tf = TextureFile.ReadTextureFile(atvf);

                        {
                            {
                                TextureFile.StreamingInfo streamInfo = tf.m_StreamData;

                                AssetsFileReader reader = new AssetsFileReader(new MemoryStream(resourceBytes));

                                reader.Position = (long)streamInfo.offset;
                                tf.pictureData = reader.ReadBytes((int)streamInfo.size);

                                try
                                {
                                    byte[] ARGB32Bytes = tf.DecodeTextureRaw(tf.pictureData);

                                    UnityEngine.Texture2D tex = new UnityEngine.Texture2D(tf.m_Width, tf.m_Height, UnityEngine.TextureFormat.BGRA32, false);
                                    tex.LoadRawTextureData(ARGB32Bytes);
                                    tex.Apply();

                                    Texture2D simpletex = new Texture2D(tex.width, tex.height);
                                    simpletex.SetPixels(tex.GetPixels());
                                    simpletex.Apply();

                                    byte[] picture = simpletex.EncodeToPNG();

                                    StartCoroutine(save(picture));
                                }
                                catch (Exception ex)
                                {
                                    ex.ToString();
                                }

                                isCurrentFunction = false;
                            }
                        }
                    }
                }
            }
            manager.UnloadAll();
        }
        else
        {
            load.ComplainError("Either no asset is selected or the selected asset is not a texture2D");
        }
    }
}
