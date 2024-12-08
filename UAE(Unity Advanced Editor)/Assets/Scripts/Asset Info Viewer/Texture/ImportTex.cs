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
using UnityEngine.XR;
using System.Collections.Generic;
using System.Linq;

public class ImportTex : MonoBehaviour
{
    public Load load;
    public SelectedFieldInfo selectedFieldInfo;

    public TextMeshProUGUI importedTextureFormat;

    public TMP_InputField importedTextureHeight;

    public TMP_InputField importedTextureWidth;

    public byte[] importedTextureBytes;

    public ReplacerManager RManager;
    public void importTex2d()
    {
        StartCoroutine(Load());
    }
    IEnumerator Load()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");

        if (FileBrowser.Success)
        {
            this.importedTextureBytes = File.ReadAllBytes(FileBrowser.Result[0]);

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

                                        AssetTypeValueField m_StreamData = atvf["m_StreamData"];
                                        m_StreamData["offset"].AsInt = 0;
                                        m_StreamData["size"].AsInt = 0;
                                        m_StreamData["path"].AsString = "";

                                        if (!atvf["m_MipCount"].IsDummy)
                                            atvf["m_MipCount"].AsInt = (1);

                                        Enum.TryParse(importedTextureFormat.text, out UnityEngine.TextureFormat UnityFormat);

                                        Enum.TryParse(importedTextureFormat.text, out AssetsTools.NET.Texture.TextureFormat AssetFromat);

                                        try
                                        {
                                            Texture2D loadedtex = new Texture2D(int.Parse(importedTextureWidth.text), int.Parse(importedTextureHeight.text));
                                            loadedtex.LoadImage(importedTextureBytes);
                                            loadedtex.Apply();


                                            Texture2D ARGBtex = new Texture2D(loadedtex.width, loadedtex.height, UnityFormat, false);
                                            ARGBtex.SetPixels(loadedtex.GetPixels());
                                            ARGBtex.Apply();

                                            byte[] newBytes = ARGBtex.GetRawTextureData();

                                            atvf["m_TextureFormat"].AsInt = (int)AssetFromat;
                                            atvf["m_CompleteImageSize"].AsInt = (newBytes.Length);

                                            atvf["m_Width"].AsInt = ARGBtex.width;
                                            atvf["m_Height"].AsInt = ARGBtex.height;

                                            AssetTypeValueField image_data = atvf["image data"];

                                            image_data.Value.ValueType = AssetValueType.ByteArray;
                                            image_data.TemplateField.ValueType = AssetValueType.ByteArray;

                                            atvf["image data"].AsByteArray = (newBytes);

                                            var newGoBytes = atvf.WriteToByteArray();

                                            ContactInfo Cinfo = new ContactInfo();

                                            Cinfo.Replacer = true;
                                            Cinfo.formatter = false;

                                            Cinfo.TypeID = (AssetClassID)AFinfo.TypeId;
                                            Cinfo.pathID = AFinfo.PathId;
                                            Cinfo.parentPath = info.parentPath;
                                            Cinfo.BundlePath = info.BundlePath;
                                            Cinfo.replacedAssetBytes = newGoBytes;

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
                                            goto end1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    end1:
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

                            TextureFile tf = TextureFile.ReadTextureFile(atvf);

                            AssetTypeValueField m_StreamData = atvf["m_StreamData"];
                            m_StreamData["offset"].AsInt = 0;
                            m_StreamData["size"].AsInt = 0;
                            m_StreamData["path"].AsString = "";

                            if (!atvf["m_MipCount"].IsDummy)
                                atvf["m_MipCount"].AsInt = (1);

                            Enum.TryParse(importedTextureFormat.text, out UnityEngine.TextureFormat UnityFormat);

                            Enum.TryParse(importedTextureFormat.text, out AssetsTools.NET.Texture.TextureFormat AssetFromat);

                            try
                            {
                                Texture2D loadedtex = new Texture2D(int.Parse(importedTextureWidth.text), int.Parse(importedTextureHeight.text));
                                loadedtex.LoadImage(importedTextureBytes);
                                loadedtex.Apply();


                                Texture2D ARGBtex = new Texture2D(loadedtex.width, loadedtex.height, UnityFormat, false);
                                ARGBtex.SetPixels(loadedtex.GetPixels());
                                ARGBtex.Apply();

                                byte[] newBytes = ARGBtex.GetRawTextureData();

                                atvf["m_TextureFormat"].AsInt = (int)AssetFromat;
                                atvf["m_CompleteImageSize"].AsInt = (newBytes.Length);

                                atvf["m_Width"].AsInt = ARGBtex.width;
                                atvf["m_Height"].AsInt = ARGBtex.height;

                                AssetTypeValueField image_data = atvf["image data"];

                                image_data.Value.ValueType = AssetValueType.ByteArray;
                                image_data.TemplateField.ValueType = AssetValueType.ByteArray;

                                atvf["image data"].AsByteArray = (newBytes);

                                var newGoBytes = atvf.WriteToByteArray();

                                ContactInfo Cinfo = new ContactInfo();

                                Cinfo.Replacer = true;
                                Cinfo.formatter = false;

                                Cinfo.TypeID = (AssetClassID)AFinfo.TypeId;
                                Cinfo.pathID = AFinfo.PathId;
                                Cinfo.parentPath = info.parentPath;
                                Cinfo.BundlePath = "";
                                Cinfo.replacedAssetBytes = newGoBytes;

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
                                goto end;
                            }
                        }
                    }
                }
                end:
                manager.UnloadAll();
            }
            else
            {
                load.ComplainError("Either no asset is selected or the selected asset is not a texture2D");
            }
        }
    }

}
namespace UAE
{ 
    public class Texture
    {
        public enum TextureFormat
        {
            Alpha8,
            ARGB4444,
            RGB24,
            RGBA32,
            ARGB32,
            ARGBFloat,
            RGB565,
            BGR24,
            R16,
            DXT1,
            DXT3,
            DXT5,
            RGBA4444,
            BGRA32,
            RHalf,
            RGHalf,
            RGBAHalf,
            RFloat,
            RGFloat,
            RGBAFloat,
            YUY2,
            RGB9e5Float,
            RGBFloat,
            BC6H,
            BC7,
            BC4,
            BC5,
            DXT1Crunched,
            DXT5Crunched,
            PVRTC_RGB2,
            PVRTC_RGBA2,
            PVRTC_RGB4,
            PVRTC_RGBA4,
            ETC_RGB4,
            ATC_RGB4,
            ATC_RGBA8,
            BGRA32Old,
            EAC_R,
            EAC_R_SIGNED,
            EAC_RG,
            EAC_RG_SIGNED,
            ETC2_RGB4,
            ETC2_RGBA1,
            ETC2_RGBA8,
            ASTC_RGB_4x4,
            ASTC_RGB_5x5,
            ASTC_RGB_6x6,
            ASTC_RGB_8x8,
            ASTC_RGB_10x10,
            ASTC_RGB_12x12,
            ASTC_RGBA_4x4,
            ASTC_RGBA_5x5,
            ASTC_RGBA_6x6,
            ASTC_RGBA_8x8,
            ASTC_RGBA_10x10,
            ASTC_RGBA_12x12,
            ETC_RGB4_3DS,
            ETC_RGBA8_3DS,
            RG16,
            R8,
            ETC_RGB4Crunched,
            ETC2_RGBA8Crunched,
            ASTC_HDR_4x4,
            ASTC_HDR_5x5,
            ASTC_HDR_6x6,
            ASTC_HDR_8x8,
            ASTC_HDR_10x10,
            ASTC_HDR_12x12,
            RG32,
            RGB48,
            RGBA64
        }
    }
}
