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

public class JsonViewer : MonoBehaviour
{
    public Load load;
    public SelectedFieldInfo selectedFieldInfo;

    public TextMeshProUGUI JsonText;
    public void ViewJson()
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

                                    using MemoryStream ms = new MemoryStream();
                                    StreamWriter sw = new StreamWriter(ms);

                                    AssetImportExport impexp = new AssetImportExport();
                                    impexp.DumpJsonAsset(sw, atvf);

                                    sw.Flush();
                                    ms.Position = 0;

                                    string str = Encoding.UTF8.GetString(ms.ToArray());

                                    JObject jobj = JObject.Parse(str);

                                    JsonText.text = paintKeys(jobj);
                                    Debug.Log(-(JsonText.rectTransform.sizeDelta.y / 2));

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

                        JObject jobj = JObject.Parse(str);

                        JsonText.text = paintKeys(jobj);

                        Debug.Log(-(JsonText.rectTransform.sizeDelta.y / 2));

                    }
                }
            }
            manager.UnloadAll();
        }
    }
    private string paintKeys(JObject jobj)
    {
        JObject obj = new JObject();
        foreach (var kvp in jobj)
        {
            if (kvp.Value is JObject)
            {
                JObject nObj = (JObject)kvp.Value;
                string strn = paintKeys(nObj);

                JObject mObj = JsonConvert.DeserializeObject<JObject>(strn);

                obj["<color=#FF8800>" + kvp.Key + "</color=#FF8800>"] = mObj;
                Debug.Log("it reached here");
            }
            if(kvp.Value is JValue)
            {
                obj.Add("<color=#FF8800>" + kvp.Key + "</color=#FF8800>", kvp.Value);
            }
            if(kvp.Value is JArray)
            {
                JArray array = (JArray)kvp.Value;

                JArray narray = new JArray();

                for(int i = 0; i < array.Count; i++)
                {
                    JToken token = array[i];
                    
                    if(token is JObject)
                    {
                        JObject aobj = (JObject)token;
                        string astr = paintKeys(aobj);
                        JObject naobj = JsonConvert.DeserializeObject<JObject>(astr);
                        narray.Add(naobj);
                        Debug.Log(astr);
                    }
                }
                obj["<color=#FF8800>" + kvp.Key + "</color=#FF8800>"] = narray;
            }
        }
        string str = JsonConvert.SerializeObject(obj, Formatting.Indented);
        return obj.ToString(Formatting.Indented);
    }
}
