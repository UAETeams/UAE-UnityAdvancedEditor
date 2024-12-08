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
using System.Linq;
using AssetsTools.NET.Texture;
using SimpleFileBrowser;

public class MatViewer : MonoBehaviour
{
    public Load load;
    public SelectedFieldInfo selectedFieldInfo;

    public Material mat;

    public bool isCurrentFunction;

    public ResourceHandle resHandle;
    public void ViewMat()
    {
        if (selectedFieldInfo.cell != null)
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

                                    var count = atvf["m_SavedProperties.m_TexEnvs.Array"].AsArray.size;

                                    {
                                        for (int i = 0; i < count; i++)
                                        {


                                            ContactInfo newContactInfo = new ContactInfo();
                                            AssetTypeValueField data = atvf["m_SavedProperties.m_TexEnvs.Array"][i];

                                            if (data["first"].AsString.Contains("main")|| data["first"].AsString.Contains("Main"))
                                            {
                                                AssetPPtr pptr = AssetPPtr.FromField(data["second.m_Texture"]);
                                                pptr.SetFilePathFromFile(manager, afileInst);

                                                Debug.Log(pptr.FilePath);

                                                if (load.result.ToList().Contains(info.BundlePath) && pptr.PathId != 0 && pptr.FilePath != "")
                                                {
                                                    var AFinst = manager.LoadAssetsFileFromBundle(bunfileInst, Path.GetFileName(pptr.FilePath));
                                                    var inf = load.FindCorrectAsset(manager, AFinst, pptr.PathId);

                                                    var BaseField = manager.GetBaseField(AFinst, inf);

                                                    TextureFile tf = TextureFile.ReadTextureFile(BaseField);
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
                                                                for (int j = 0; j < dirInf.Length; j++)
                                                                {
                                                                    AssetBundleDirectoryInfo ABDInfo = dirInf[j];
                                                                    if (ABDInfo.Name == searchPath)
                                                                    {
                                                                        reader.Position = bundle.Header.GetFileDataOffset() + ABDInfo.Offset + (long)streamInfo.offset;
                                                                        tf.pictureData = reader.ReadBytes((int)streamInfo.size);
                                                                        tf.m_StreamData.offset = 0;
                                                                        tf.m_StreamData.size = 0;
                                                                        tf.m_StreamData.path = "";
                                                                    }
                                                                }
                                                                UnityEngine.Texture2D tex = new UnityEngine.Texture2D(tf.m_Width, tf.m_Height, (UnityEngine.TextureFormat)tf.m_TextureFormat, false);
                                                                tex.LoadRawTextureData(tf.pictureData);
                                                                tex.Apply();

                                                                mat.mainTexture = tex;
                                                            }
                                                            else
                                                            {
                                                                load.ComplainError("This texture has no texture bytes.");
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        UnityEngine.Texture2D tex = new UnityEngine.Texture2D(tf.m_Width, tf.m_Height, (UnityEngine.TextureFormat)tf.m_TextureFormat, false);
                                                        tex.LoadRawTextureData(tf.pictureData);
                                                        tex.Apply();

                                                        mat.mainTexture = tex;
                                                    }
                                                }
                                            }
                                        }
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


                        var count = atvf["m_SavedProperties.m_TexEnvs.Array"].AsArray.size;

                        ///Material Fields
                        {
                            for (int i = 0; i < count; i++)
                            {

                                AssetTypeValueField data = atvf["m_SavedProperties.m_TexEnvs.Array"][i];
                                if (data["first.name"].IsDummy)
                                {
                                    if (data["first"].AsString.Contains("main") || data["first"].AsString.Contains("Main"))
                                    {
                                        AssetPPtr pptr = AssetPPtr.FromField(data["second.m_Texture"]);
                                        pptr.SetFilePathFromFile(manager, AFinst);

                                        Debug.Log(pptr.FilePath);

                                        if (load.result.ToList().Contains(pptr.FilePath) && pptr.PathId != 0)
                                        {
                                            manager.UnloadAssetsFile(info.parentPath);

                                            var AFinste = manager.LoadAssetsFile(pptr.FilePath, false);
                                            var inf = load.FindCorrectAsset(manager, AFinste, pptr.PathId);

                                            var BaseField = manager.GetBaseField(AFinst, inf);


                                            TextureFile tf = TextureFile.ReadTextureFile(BaseField);
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
                                                UnityEngine.Texture2D tex = new UnityEngine.Texture2D(tf.m_Width, tf.m_Height, (UnityEngine.TextureFormat)tf.m_TextureFormat, false);
                                                tex.LoadRawTextureData(tf.pictureData);
                                                tex.Apply();

                                                mat.mainTexture = tex;
                                            }

                                        }
                                        else if (pptr.PathId != 0)
                                        {
                                            load.ComplainError("You didnt load the referenced Asset file yet.\nThe name of the Asset file is :- " + Path.GetFileName(pptr.FilePath));
                                        }
                                    }
                                }
                                else
                                {
                                    if (data["first.name"].AsString.Contains("main") || data["first.name"].AsString.Contains("Main"))
                                    {
                                        AssetPPtr pptr = AssetPPtr.FromField(data["second.m_Texture"]);
                                        pptr.SetFilePathFromFile(manager, AFinst);

                                        Debug.Log(pptr.FilePath);

                                        if (load.result.ToList().Contains(pptr.FilePath) && pptr.PathId != 0)
                                        {
                                            manager.UnloadAssetsFile(info.parentPath);

                                            var AFinste = manager.LoadAssetsFile(pptr.FilePath, false);
                                            var inf = load.FindCorrectAsset(manager, AFinste, pptr.PathId);

                                            var BaseField = manager.GetBaseField(AFinste, inf);


                                            TextureFile tf = TextureFile.ReadTextureFile(BaseField);
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
                                                UnityEngine.Texture2D tex = new UnityEngine.Texture2D(tf.m_Width, tf.m_Height, (UnityEngine.TextureFormat)tf.m_TextureFormat, false);
                                                tex.LoadRawTextureData(tf.pictureData);
                                                tex.Apply();

                                                mat.mainTexture = tex;
                                            }

                                        }
                                        else if (pptr.PathId != 0)
                                        {
                                            load.ComplainError("You didnt load the referenced Asset file yet.\nThe name of the Asset file is :- " + Path.GetFileName(pptr.FilePath));
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }
            manager.UnloadAll();
        }
    }
    public void viewMatRest(byte[] resourceBytes)
    {
        if (selectedFieldInfo.cell != null && selectedFieldInfo.cell.TypeID == AssetClassID.Material.ToString())
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

                                UnityEngine.Texture2D tex = new UnityEngine.Texture2D(tf.m_Width, tf.m_Height, (UnityEngine.TextureFormat)tf.m_TextureFormat, false);
                                tex.LoadRawTextureData(tf.pictureData);
                                tex.Apply();

                                mat.mainTexture = tex;

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
