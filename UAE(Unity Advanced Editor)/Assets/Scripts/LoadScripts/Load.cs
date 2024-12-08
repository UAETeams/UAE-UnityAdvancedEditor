using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SimpleFileBrowser;
using System.IO;
using UAE;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System;
using System.Linq;

public class Load : MonoBehaviour
{
    public Transform UAEFileRoot;

    [SerializeField]
    private GameObject ErrorWindow;

    [SerializeField]
    private TextMeshProUGUI consoleText;

    public string[] result;

    [SerializeField]
    private SelectedFieldInfo selectedFieldInfo;

    public Texture2D[] identifiers;

    public GameObject LoadingBar;
    public TextMeshProUGUI LoadingText;
    public TextMeshProUGUI LoadingAssetText;

    public GameObject inAccessible;

    public RealtimeHierarchy rh;


    public Texture2D SetClassIDIdentifier(AssetClassID ID)
    {
        if (ID == AssetClassID.Animation)
        {
            return identifiers[0];
        }
        else if (ID == AssetClassID.AnimationClip)
        {
            return identifiers[1];
        }
        else if (ID == AssetClassID.Animator)
        {
            return identifiers[2];
        }
        else if (ID == AssetClassID.AnimatorController)
        {
            return identifiers[3];
        }
        else if (ID == AssetClassID.AnimatorOverrideController)
        {
            return identifiers[4];
        }
        else if (ID == AssetClassID.AudioClip)
        {
            return identifiers[5];
        }
        else if (ID == AssetClassID.AudioListener)
        {
            return identifiers[6];
        }
        else if (ID == AssetClassID.AudioMixer)
        {
            return identifiers[7];
        }
        else if (ID == AssetClassID.AudioMixerGroup)
        {
            return identifiers[8];
        }
        else if (ID == AssetClassID.AudioSource)
        {
            return identifiers[9];
        }
        else if (ID == AssetClassID.Avatar)
        {
            return identifiers[10];
        }
        else if (ID == AssetClassID.BillboardAsset)
        {
            return identifiers[11];
        }
        else if (ID == AssetClassID.BillboardRenderer)
        {
            return identifiers[12];
        }
        else if (ID == AssetClassID.BoxCollider)
        {
            return identifiers[13];
        }
        else if (ID == AssetClassID.Camera)
        {
            return identifiers[14];
        }
        else if (ID == AssetClassID.Canvas)
        {
            return identifiers[15];
        }
        else if (ID == AssetClassID.CanvasGroup)
        {
            return identifiers[16];
        }
        else if (ID == AssetClassID.CanvasRenderer)
        {
            return identifiers[17];
        }
        else if (ID == AssetClassID.CapsuleCollider)
        {
            return identifiers[18];
        }
        else if (ID == AssetClassID.ComputeShader)
        {
            return identifiers[19];
        }
        else if (ID == AssetClassID.Cubemap)
        {
            return identifiers[20];
        }
        else if (ID == AssetClassID.Flare)
        {
            return identifiers[21];
        }
        else if (ID == AssetClassID.Font)
        {
            return identifiers[22];
        }
        else if (ID == AssetClassID.GameObject)
        {
            return identifiers[23];
        }
        else if (ID == AssetClassID.Light)
        {
            return identifiers[24];
        }
        else if (ID == AssetClassID.LightmapSettings)
        {
            return identifiers[25];
        }
        else if (ID == AssetClassID.LODGroup)
        {
            return identifiers[26];
        }
        else if (ID == AssetClassID.Material)
        {
            return identifiers[27];
        }
        else if (ID == AssetClassID.Mesh)
        {
            return identifiers[28];
        }
        else if (ID == AssetClassID.MeshCollider)
        {
            return identifiers[29];
        }

        else if (ID == AssetClassID.MeshFilter)
        {
            return identifiers[30];
        }

        else if (ID == AssetClassID.MeshRenderer)
        {
            return identifiers[31];
        }

        else if (ID == AssetClassID.MonoBehaviour)
        {
            return identifiers[32];
        }

        else if (ID == AssetClassID.MonoScript)
        {
            return identifiers[33];
        }

        else if (ID == AssetClassID.NavMeshSettings)
        {
            return identifiers[34];
        }

        else if (ID == AssetClassID.ParticleSystem)
        {
            return identifiers[35];
        }
        else if (ID == AssetClassID.ParticleSystemRenderer)
        {
            return identifiers[36];
        }

        else if (ID == AssetClassID.RectTransform)
        {
            return identifiers[37];
        }
        else if (ID == AssetClassID.ReflectionProbe)
        {
            return identifiers[38];
        }

        else if (ID == AssetClassID.Rigidbody)
        {
            return identifiers[39];
        }

        else if (ID == AssetClassID.Shader)
        {
            return identifiers[40];
        }

        else if (ID == AssetClassID.ShaderVariantCollection)
        {
            return identifiers[41];
        }

        else if (ID == AssetClassID.Sprite)
        {
            return identifiers[42];
        }

        else if (ID == AssetClassID.SpriteRenderer)
        {
            return identifiers[43];
        }

        else if (ID == AssetClassID.Terrain)
        {
            return identifiers[44];
        }

        else if (ID == AssetClassID.TerrainCollider)
        {
            return identifiers[45];
        }

        else if (ID == AssetClassID.TextAsset)
        {
            return identifiers[46];
        }

        else if (ID == AssetClassID.Texture2D)
        {
            return identifiers[47];
        }

        else if (ID == AssetClassID.Transform)
        {
            return identifiers[48];
        }
        else if (ID == AssetClassID.Prefab)
        {
            return identifiers[51];
        }
        else if (ID == AssetClassID.PrefabInstance)
        {
            return identifiers[52];
        }
        else if (ID == AssetClassID.Special)
        {
            return identifiers[53];
        }
        else
        {
            return identifiers[49];
        }
    }

    public void ComplainError(string message)
    {
        //if (UnityEngine.Application.platform == RuntimePlatform.Android)
        {
            ErrorWindow.SetActive(true);
            consoleText.text = message;
        }
        //else
        {
            //inAccessible.SetActive(true);
            //NetConsoleMessage Log = new NetConsoleMessage(message);
            //FindObjectOfType<BaseClient>().SendToServer(Log);
        }
    }

    public void InitializeData()
    {
        rh.ReInitializeLayout();
    }

    public void DefameData()
    {
        GameObject root = UAEFileRoot.gameObject;
        int count = root.transform.childCount;
        for(int i = 0; i < count; i++)
        {
            GameObject.Destroy(root.transform.GetChild(i).gameObject);
        }
        rh.ReInitializeLayout();
    }
    public void GoBackSubdivision()
    {

    }

    public void SubDivideFieldInfo()
    {
        try
        {
            if (selectedFieldInfo.cell != null)
            {
                if (result != null)
                {
                    ContactInfo info = selectedFieldInfo.cell._contactInfo;
                    
                    if (selectedFieldInfo.cell.gameObject.transform.childCount == 0)
                    {
                        if (info.BundlePath != "")
                        {
                            if (result.ToList().Contains(info.BundlePath))
                            {
                                DetectedFileType FileTypes = FileTypeDetector.DetectFileType(info.BundlePath);
                                Debug.Log("reached here");
                                if (FileTypes == DetectedFileType.BundleFile)
                                {
                                    var KeyValuePair = AMLoadAndGetBaseFieldAssets(info.parentPath, info.BundlePath, info.TypeID, info.pathID);

                                    if (KeyValuePair != null)
                                    {
                                        if (info.TypeID == AssetClassID.GameObject)
                                        {
                                            {
                                                Transform GEntity = selectedFieldInfo.cell.gameObject.transform;
                                                Debug.Log(info.Name);
                                                Debug.Log(info.parentPath);
                                                Debug.Log(info.pathID);
                                                Debug.Log(info.TypeID);

                                                var count = KeyValuePair.Basefield["m_Component.Array"].AsArray.size;

                                                for (int i = 0; i < count; i++)
                                                {
                                                    ContactInfo newContactInfo = new ContactInfo();
                                                    AssetTypeValueField data = KeyValuePair.Basefield["m_Component.Array"][i];

                                                    AssetPPtr pptr = AssetPPtr.FromField(data["component"]);

                                                    pptr.SetFilePathFromFile(KeyValuePair.Manager, KeyValuePair.AFinst);
                                                    {
                                                        {
                                                            {
                                                                Debug.Log("it contains");
                                                                var AFinst = KeyValuePair.Manager.LoadAssetsFileFromBundle(KeyValuePair.BFInst, Path.GetFileName(pptr.FilePath));

                                                                var inf = FindCorrectAsset(KeyValuePair.Manager, AFinst, pptr.PathId);

                                                                var BaseField = KeyValuePair.Manager.GetBaseField(AFinst, inf);

                                                                string name = "F";

                                                                if (BaseField["m_Name"].IsDummy == true)
                                                                {
                                                                    AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                                    name = id2.ToString() + " #" + inf.PathId.ToString();
                                                                }
                                                                else
                                                                {
                                                                    name = BaseField["m_Name"].AsString;
                                                                    if (name == null || name == "")
                                                                    {
                                                                        AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                                        name = id2.ToString() + " #" + inf.PathId.ToString();
                                                                    }
                                                                }
                                                                AssetClassID id = (AssetClassID)inf.TypeId;

                                                                Debug.Log(name);
                                                                newContactInfo.Name = name;

                                                                newContactInfo.Type = SetClassIDIdentifier(id);
                                                                newContactInfo.parentPath = Path.GetFileName(pptr.FilePath);

                                                                newContactInfo.BundlePath = info.BundlePath;

                                                                newContactInfo.pathID = inf.PathId;
                                                                newContactInfo.refPath = "/" + newContactInfo.Name;

                                                                newContactInfo.UnknownAsset = false;
                                                                newContactInfo.TypeID = id;

                                                                GameObject assetObject = new GameObject(newContactInfo.Name);
                                                                assetObject.AddComponent<TypeFieldCell>();

                                                                assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(newContactInfo);

                                                                assetObject.transform.SetParent(GEntity);
                                                            }
                                                        }

                                                    }
                                                }
                                            }
                                            InitializeData();
                                        }
                                        else if (info.TypeID == AssetClassID.MeshFilter)
                                        {

                                            Transform GEntity = selectedFieldInfo.cell.gameObject.transform;

                                            ///Mesh Fields
                                            {
                                                {
                                                    ContactInfo newContactInfo = new ContactInfo();
                                                    AssetTypeValueField data = KeyValuePair.Basefield["m_Mesh"];

                                                    AssetPPtr pptr = AssetPPtr.FromField(data);
                                                    pptr.SetFilePathFromFile(KeyValuePair.Manager, KeyValuePair.AFinst);

                                                    if (result.ToList().Contains(info.BundlePath) && pptr.PathId != 0)
                                                    {
                                                        var AFinst = KeyValuePair.Manager.LoadAssetsFileFromBundle(KeyValuePair.BFInst, Path.GetFileName(pptr.FilePath));
                                                        var inf = FindCorrectAsset(KeyValuePair.Manager, AFinst, pptr.PathId);

                                                        var BaseField = KeyValuePair.Manager.GetBaseField(AFinst, inf);

                                                        string name = "F";

                                                        if (BaseField["m_Name"].IsDummy == true)
                                                        {
                                                            AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                            name = id2.ToString() + " #" + inf.PathId.ToString();
                                                        }
                                                        else
                                                        {
                                                            name = BaseField["m_Name"].AsString;
                                                            if (name == null || name == "")
                                                            {
                                                                AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                                name = id2.ToString() + " #" + inf.PathId.ToString();
                                                            }
                                                        }
                                                        AssetClassID id = (AssetClassID)inf.TypeId;

                                                        Debug.Log(name);
                                                        newContactInfo.Name = name;

                                                        newContactInfo.Type = SetClassIDIdentifier(id);
                                                        newContactInfo.parentPath = Path.GetFileName(pptr.FilePath);
                                                        newContactInfo.BundlePath = info.BundlePath;
                                                        newContactInfo.pathID = inf.PathId;
                                                        newContactInfo.refPath = "/" + newContactInfo.Name;

                                                        newContactInfo.UnknownAsset = false;
                                                        newContactInfo.TypeID = id;

                                                        GameObject assetObject = new GameObject(newContactInfo.Name);
                                                        assetObject.AddComponent<TypeFieldCell>();

                                                        assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(newContactInfo);

                                                        assetObject.transform.SetParent(GEntity);

                                                    }
                                                    else if (pptr.PathId != 0)
                                                    {
                                                        ComplainError("You didnt load the referenced Asset file yet.\nThe name of the Asset file is :- " + Path.GetFileName(pptr.FilePath));
                                                    }
                                                    else
                                                    {
                                                        ComplainError("Asset cannot be further subdivided because its reference fields are null");
                                                    }
                                                }
                                            }
                                            InitializeData();
                                        }

                                        else if (info.TypeID == AssetClassID.MonoBehaviour)
                                        {
                                            Transform GEntity = selectedFieldInfo.cell.gameObject.transform;

                                            ///Mesh Fields
                                            {
                                                {
                                                    ContactInfo newContactInfo = new ContactInfo();
                                                    AssetTypeValueField data = KeyValuePair.Basefield["m_Script"];

                                                    AssetPPtr pptr = AssetPPtr.FromField(data);
                                                    pptr.SetFilePathFromFile(KeyValuePair.Manager, KeyValuePair.AFinst);

                                                    if (result.ToList().Contains(info.BundlePath) && pptr.PathId != 0)
                                                    {
                                                        var AFinst = KeyValuePair.Manager.LoadAssetsFileFromBundle(KeyValuePair.BFInst, Path.GetFileName(pptr.FilePath));
                                                        var inf = FindCorrectAsset(KeyValuePair.Manager, AFinst, pptr.PathId);

                                                        var BaseField = KeyValuePair.Manager.GetBaseField(AFinst, inf);

                                                        string name = "F";

                                                        if (BaseField["m_Name"].IsDummy == true)
                                                        {
                                                            AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                            name = id2.ToString() + " #" + inf.PathId.ToString();
                                                        }
                                                        else
                                                        {
                                                            name = BaseField["m_Name"].AsString;
                                                            if (name == null || name == "")
                                                            {
                                                                AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                                name = id2.ToString() + " #" + inf.PathId.ToString();
                                                            }
                                                        }
                                                        AssetClassID id = (AssetClassID)inf.TypeId;

                                                        Debug.Log(name);
                                                        newContactInfo.Name = name + ".cs";

                                                        newContactInfo.Type = SetClassIDIdentifier(id);
                                                        newContactInfo.parentPath = Path.GetFileName(pptr.FilePath);
                                                        newContactInfo.BundlePath = info.BundlePath;
                                                        newContactInfo.pathID = inf.PathId;
                                                        newContactInfo.refPath = "/" + newContactInfo.Name + ".cs";

                                                        newContactInfo.UnknownAsset = false;
                                                        newContactInfo.TypeID = id;
                                                        GameObject assetObject = new GameObject(newContactInfo.Name);
                                                        assetObject.AddComponent<TypeFieldCell>();

                                                        assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(newContactInfo);

                                                        assetObject.transform.SetParent(GEntity);
                                                    }
                                                    else
                                                    {
                                                        ComplainError("You didnt load the referenced Asset file yet.\nThe name of the Asset file is :- " + Path.GetFileName(pptr.FilePath));
                                                    }
                                                }
                                            }
                                            InitializeData();
                                        }

                                        else if (info.TypeID == AssetClassID.MeshRenderer)
                                        {
                                            var count = KeyValuePair.Basefield["m_Materials.Array"].AsArray.size;

                                            Transform GEntity = selectedFieldInfo.cell.gameObject.transform;

                                            for (int i = 0; i < count; i++)
                                            {
                                                ContactInfo newContactInfo = new ContactInfo();
                                                AssetTypeValueField data = KeyValuePair.Basefield["m_Materials.Array"][i];

                                                AssetPPtr pptr = AssetPPtr.FromField(data);
                                                pptr.SetFilePathFromFile(KeyValuePair.Manager, KeyValuePair.AFinst);

                                                if (result.ToList().Contains(info.BundlePath) && pptr.PathId != 0)
                                                {

                                                    var AFinst = KeyValuePair.Manager.LoadAssetsFileFromBundle(KeyValuePair.BFInst, Path.GetFileName(pptr.FilePath));

                                                    var inf = FindCorrectAsset(KeyValuePair.Manager, AFinst, pptr.PathId);

                                                    var BaseField = KeyValuePair.Manager.GetBaseField(AFinst, inf);

                                                    string name = "F";

                                                    if (BaseField["m_Name"].IsDummy == true)
                                                    {
                                                        AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                        name = id2.ToString() + " #" + inf.PathId.ToString();
                                                    }
                                                    else
                                                    {
                                                        name = BaseField["m_Name"].AsString;
                                                        if (name == null || name == "")
                                                        {
                                                            AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                            name = id2.ToString() + " #" + inf.PathId.ToString();
                                                        }
                                                    }
                                                    AssetClassID id = (AssetClassID)inf.TypeId;

                                                    Debug.Log(name);
                                                    newContactInfo.Name = name;

                                                    newContactInfo.Type = SetClassIDIdentifier(id);
                                                    newContactInfo.parentPath = Path.GetFileName(pptr.FilePath);
                                                    newContactInfo.BundlePath = info.BundlePath;
                                                    newContactInfo.pathID = inf.PathId;
                                                    newContactInfo.refPath = "/" + newContactInfo.Name;

                                                    newContactInfo.UnknownAsset = false;
                                                    newContactInfo.TypeID = id;

                                                    GameObject assetObject = new GameObject(newContactInfo.Name);
                                                    assetObject.AddComponent<TypeFieldCell>();

                                                    assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(newContactInfo);

                                                    assetObject.transform.SetParent(GEntity);
                                                }
                                                else
                                                {
                                                    ComplainError("You didnt load the referenced Asset file yet.\nThe name of the Asset file is :- " + Path.GetFileName(pptr.FilePath));
                                                }
                                            }
                                            InitializeData();
                                        }

                                        else if (info.TypeID == AssetClassID.Transform)
                                        {
                                            var count = KeyValuePair.Basefield["m_Children.Array"].AsArray.size;

                                            Transform GEntity = selectedFieldInfo.cell.gameObject.transform;

                                            for (int i = 0; i < count; i++)
                                            {
                                                ContactInfo newContactInfo = new ContactInfo();
                                                AssetTypeValueField data = KeyValuePair.Basefield["m_Children.Array"][i];

                                                AssetPPtr pptr = AssetPPtr.FromField(data);
                                                pptr.SetFilePathFromFile(KeyValuePair.Manager, KeyValuePair.AFinst);

                                                if (result.ToList().Contains(info.BundlePath))
                                                {

                                                    var AFinst = KeyValuePair.Manager.LoadAssetsFileFromBundle(KeyValuePair.BFInst, Path.GetFileName(pptr.FilePath));

                                                    var inf = FindCorrectAsset(KeyValuePair.Manager, AFinst, pptr.PathId);

                                                    var BaseField = KeyValuePair.Manager.GetBaseField(AFinst, inf);

                                                    string name = "F";

                                                    if (BaseField["m_Name"].IsDummy == true)
                                                    {
                                                        AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                        name = id2.ToString() + " #" + inf.PathId.ToString();
                                                    }
                                                    else
                                                    {
                                                        name = BaseField["m_Name"].AsString;
                                                        if (name == null || name == "")
                                                        {
                                                            AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                            name = id2.ToString() + " #" + inf.PathId.ToString();
                                                        }
                                                    }
                                                    AssetClassID id = (AssetClassID)inf.TypeId;

                                                    Debug.Log(name);
                                                    newContactInfo.Name = name;

                                                    newContactInfo.Type = SetClassIDIdentifier(id);
                                                    newContactInfo.parentPath = Path.GetFileName(pptr.FilePath);
                                                    newContactInfo.BundlePath = info.BundlePath;
                                                    newContactInfo.pathID = inf.PathId;
                                                    newContactInfo.refPath = "/" + newContactInfo.Name;

                                                    newContactInfo.UnknownAsset = false;
                                                    newContactInfo.TypeID = id;

                                                    GameObject assetObject = new GameObject(newContactInfo.Name);
                                                    assetObject.AddComponent<TypeFieldCell>();

                                                    assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(newContactInfo);

                                                    assetObject.transform.SetParent(GEntity);
                                                }
                                                else
                                                {
                                                    ComplainError("You didnt load the referenced Asset file yet.\nThe name of the Asset file is :- " + Path.GetFileName(pptr.FilePath));
                                                }
                                            }

                                            {
                                                {
                                                    {
                                                        ContactInfo newContactInfo = new ContactInfo();
                                                        AssetTypeValueField data = KeyValuePair.Basefield["m_GameObject"];

                                                        AssetPPtr pptr = AssetPPtr.FromField(data);
                                                        pptr.SetFilePathFromFile(KeyValuePair.Manager, KeyValuePair.AFinst);

                                                        if (result.ToList().Contains(info.BundlePath) && pptr.PathId != 0)
                                                        {
                                                            var AFinst = KeyValuePair.Manager.LoadAssetsFileFromBundle(KeyValuePair.BFInst, Path.GetFileName(pptr.FilePath));
                                                            var inf = FindCorrectAsset(KeyValuePair.Manager, AFinst, pptr.PathId);

                                                            var BaseField = KeyValuePair.Manager.GetBaseField(AFinst, inf);

                                                            string name = "F";

                                                            if (BaseField["m_Name"].IsDummy == true)
                                                            {
                                                                AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                                name = id2.ToString() + " #" + inf.PathId.ToString();
                                                            }
                                                            else
                                                            {
                                                                name = BaseField["m_Name"].AsString;
                                                                if (name == null || name == "")
                                                                {
                                                                    AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                                    name = id2.ToString() + " #" + inf.PathId.ToString();
                                                                }
                                                            }
                                                            AssetClassID id = (AssetClassID)inf.TypeId;

                                                            Debug.Log(name);
                                                            newContactInfo.Name = name;

                                                            newContactInfo.Type = SetClassIDIdentifier(id);
                                                            newContactInfo.parentPath = Path.GetFileName(pptr.FilePath);
                                                            newContactInfo.BundlePath = info.BundlePath;
                                                            newContactInfo.pathID = inf.PathId;
                                                            newContactInfo.refPath = "/" + newContactInfo.Name;

                                                            newContactInfo.UnknownAsset = false;
                                                            newContactInfo.TypeID = id;

                                                            GameObject assetObject = new GameObject(newContactInfo.Name);
                                                            assetObject.AddComponent<TypeFieldCell>();

                                                            assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(newContactInfo);

                                                            assetObject.transform.SetParent(GEntity);
                                                        }
                                                        else
                                                        {
                                                            ComplainError("You didnt load the referenced Asset file yet.\nThe name of the Asset file is :- " + Path.GetFileName(pptr.FilePath));
                                                        }
                                                    }
                                                }

                                            }
                                            InitializeData();
                                        }
                                        else if (info.TypeID == AssetClassID.RectTransform)
                                        {
                                            var count = KeyValuePair.Basefield["m_Children.Array"].AsArray.size;

                                            Transform GEntity = selectedFieldInfo.cell.gameObject.transform;

                                            for (int i = 0; i < count; i++)
                                            {
                                                ContactInfo newContactInfo = new ContactInfo();
                                                AssetTypeValueField data = KeyValuePair.Basefield["m_Children.Array"][i];

                                                AssetPPtr pptr = AssetPPtr.FromField(data);
                                                pptr.SetFilePathFromFile(KeyValuePair.Manager, KeyValuePair.AFinst);
                                                if (result.ToList().Contains(info.BundlePath) && pptr.PathId != 0)
                                                {
                                                    var AFinst = KeyValuePair.Manager.LoadAssetsFileFromBundle(KeyValuePair.BFInst, Path.GetFileName(pptr.FilePath));

                                                    var inf = FindCorrectAsset(KeyValuePair.Manager, AFinst, pptr.PathId);

                                                    var BaseField = KeyValuePair.Manager.GetBaseField(AFinst, inf);

                                                    string name = "F";

                                                    if (BaseField["m_Name"].IsDummy == true)
                                                    {
                                                        AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                        name = id2.ToString() + " #" + inf.PathId.ToString();
                                                    }
                                                    else
                                                    {
                                                        name = BaseField["m_Name"].AsString;
                                                        if (name == null || name == "")
                                                        {
                                                            AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                            name = id2.ToString() + " #" + inf.PathId.ToString();
                                                        }
                                                    }
                                                    AssetClassID id = (AssetClassID)inf.TypeId;

                                                    Debug.Log(name);
                                                    newContactInfo.Name = name;

                                                    newContactInfo.Type = SetClassIDIdentifier(id);
                                                    newContactInfo.parentPath = Path.GetFileName(pptr.FilePath);
                                                    newContactInfo.BundlePath = info.BundlePath;
                                                    newContactInfo.pathID = inf.PathId;
                                                    newContactInfo.refPath = "/" + newContactInfo.Name;

                                                    newContactInfo.UnknownAsset = false;
                                                    newContactInfo.TypeID = id;

                                                    GameObject assetObject = new GameObject(newContactInfo.Name);
                                                    assetObject.AddComponent<TypeFieldCell>();

                                                    assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(newContactInfo);

                                                    assetObject.transform.SetParent(GEntity);
                                                }
                                                else
                                                {
                                                    ComplainError("You didnt load the referenced Asset file yet.\nThe name of the Asset file is :- " + Path.GetFileName(pptr.FilePath));
                                                }
                                            }

                                            {
                                                {
                                                    {
                                                        ContactInfo newContactInfo = new ContactInfo();
                                                        AssetTypeValueField data = KeyValuePair.Basefield["m_GameObject"];

                                                        AssetPPtr pptr = AssetPPtr.FromField(data);
                                                        pptr.SetFilePathFromFile(KeyValuePair.Manager, KeyValuePair.AFinst);

                                                        if (result.ToList().Contains(info.BundlePath) && pptr.PathId != 0)
                                                        {
                                                            var AFinst = KeyValuePair.Manager.LoadAssetsFileFromBundle(KeyValuePair.BFInst, Path.GetFileName(pptr.FilePath));
                                                            var inf = FindCorrectAsset(KeyValuePair.Manager, AFinst, pptr.PathId);

                                                            var BaseField = KeyValuePair.Manager.GetBaseField(AFinst, inf);

                                                            string name = "F";

                                                            if (BaseField["m_Name"].IsDummy == true)
                                                            {
                                                                AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                                name = id2.ToString() + " #" + inf.PathId.ToString();
                                                            }
                                                            else
                                                            {
                                                                name = BaseField["m_Name"].AsString;
                                                                if (name == null || name == "")
                                                                {
                                                                    AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                                    name = id2.ToString() + " #" + inf.PathId.ToString();
                                                                }
                                                            }
                                                            AssetClassID id = (AssetClassID)inf.TypeId;

                                                            Debug.Log(name);
                                                            newContactInfo.Name = name;

                                                            newContactInfo.Type = SetClassIDIdentifier(id);
                                                            newContactInfo.parentPath = Path.Combine(pptr.FilePath);
                                                            newContactInfo.BundlePath = info.BundlePath;
                                                            newContactInfo.pathID = inf.PathId;
                                                            newContactInfo.refPath = "/" + newContactInfo.Name;

                                                            newContactInfo.UnknownAsset = false;
                                                            newContactInfo.TypeID = id;

                                                            GameObject assetObject = new GameObject(newContactInfo.Name);
                                                            assetObject.AddComponent<TypeFieldCell>();

                                                            assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(newContactInfo);

                                                            assetObject.transform.SetParent(GEntity);
                                                        }
                                                        else
                                                        {
                                                            ComplainError("You didnt load the referenced Asset file yet.\nThe name of the Asset file is :- " + Path.GetFileName(pptr.FilePath));
                                                        }
                                                    }
                                                }

                                            }
                                            InitializeData();
                                        }
                                        else if (info.TypeID == AssetClassID.SkinnedMeshRenderer)
                                        {
                                            var count = KeyValuePair.Basefield["m_Materials.Array"].AsArray.size;

                                            Transform GEntity = selectedFieldInfo.cell.gameObject.transform;

                                            ///Material Fields
                                            {
                                                for (int i = 0; i < count; i++)
                                                {
                                                    ContactInfo newContactInfo = new ContactInfo();
                                                    AssetTypeValueField data = KeyValuePair.Basefield["m_Materials.Array"][i];

                                                    AssetPPtr pptr = AssetPPtr.FromField(data);
                                                    pptr.SetFilePathFromFile(KeyValuePair.Manager, KeyValuePair.AFinst);

                                                    if (result.ToList().Contains(info.BundlePath) && pptr.PathId != 0)
                                                    {
                                                        var AFinst = KeyValuePair.Manager.LoadAssetsFileFromBundle(KeyValuePair.BFInst, Path.GetFileName(pptr.FilePath));
                                                        var inf = FindCorrectAsset(KeyValuePair.Manager, AFinst, pptr.PathId);

                                                        var BaseField = KeyValuePair.Manager.GetBaseField(AFinst, inf);

                                                        string name = "F";

                                                        if (BaseField["m_Name"].IsDummy == true)
                                                        {
                                                            AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                            name = id2.ToString() + " #" + inf.PathId.ToString();
                                                        }
                                                        else
                                                        {
                                                            name = BaseField["m_Name"].AsString;
                                                            if (name == null || name == "")
                                                            {
                                                                AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                                name = id2.ToString() + " #" + inf.PathId.ToString();
                                                            }
                                                        }
                                                        AssetClassID id = (AssetClassID)inf.TypeId;

                                                        Debug.Log(name);
                                                        newContactInfo.Name = name;

                                                        newContactInfo.Type = SetClassIDIdentifier(id);
                                                        newContactInfo.parentPath = Path.GetFileName(pptr.FilePath);
                                                        newContactInfo.BundlePath = info.BundlePath;
                                                        newContactInfo.pathID = inf.PathId;
                                                        newContactInfo.refPath = "/" + newContactInfo.Name;

                                                        newContactInfo.UnknownAsset = false;
                                                        newContactInfo.TypeID = id;

                                                        GameObject assetObject = new GameObject(newContactInfo.Name);
                                                        assetObject.AddComponent<TypeFieldCell>();

                                                        assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(newContactInfo);

                                                        assetObject.transform.SetParent(GEntity);
                                                    }
                                                    else
                                                    {
                                                        ComplainError("You didnt load the referenced Asset file yet.\nThe name of the Asset file is :- " + Path.GetFileName(pptr.FilePath));
                                                    }
                                                }
                                            }

                                            ///Mesh Fields
                                            {
                                                {
                                                    ContactInfo newContactInfo = new ContactInfo();
                                                    AssetTypeValueField data = KeyValuePair.Basefield["m_Mesh"];

                                                    AssetPPtr pptr = AssetPPtr.FromField(data);
                                                    pptr.SetFilePathFromFile(KeyValuePair.Manager, KeyValuePair.AFinst);

                                                    if (result.ToList().Contains(info.BundlePath) && pptr.PathId != 0)
                                                    {
                                                        var AFinst = KeyValuePair.Manager.LoadAssetsFileFromBundle(KeyValuePair.BFInst, Path.GetFileName(pptr.FilePath));
                                                        var inf = FindCorrectAsset(KeyValuePair.Manager, AFinst, pptr.PathId);

                                                        var BaseField = KeyValuePair.Manager.GetBaseField(AFinst, inf);

                                                        string name = "F";

                                                        if (BaseField["m_Name"].IsDummy == true)
                                                        {
                                                            AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                            name = id2.ToString() + " #" + inf.PathId.ToString();
                                                        }
                                                        else
                                                        {
                                                            name = BaseField["m_Name"].AsString;
                                                            if (name == null || name == "")
                                                            {
                                                                AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                                name = id2.ToString() + " #" + inf.PathId.ToString();
                                                            }
                                                        }
                                                        AssetClassID id = (AssetClassID)inf.TypeId;

                                                        Debug.Log(name);
                                                        newContactInfo.Name = name;

                                                        newContactInfo.Type = SetClassIDIdentifier(id);
                                                        newContactInfo.parentPath = Path.Combine(pptr.FilePath);
                                                        newContactInfo.BundlePath = info.BundlePath;
                                                        newContactInfo.pathID = inf.PathId;
                                                        newContactInfo.refPath = "/" + newContactInfo.Name;

                                                        newContactInfo.UnknownAsset = false;
                                                        newContactInfo.TypeID = id;

                                                        GameObject assetObject = new GameObject(newContactInfo.Name);
                                                        assetObject.AddComponent<TypeFieldCell>();

                                                        assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(newContactInfo);

                                                        assetObject.transform.SetParent(GEntity);
                                                    }
                                                    else
                                                    {
                                                        ComplainError("You didnt load the referenced Asset file yet.\nThe name of the Asset file is :- " + Path.GetFileName(pptr.FilePath));
                                                    }
                                                }
                                            }
                                            InitializeData();
                                        }
                                        else if (info.TypeID == AssetClassID.Material)
                                        {
                                            Debug.Log("reached mat");
                                            var count = KeyValuePair.Basefield["m_SavedProperties.m_TexEnvs.Array"].AsArray.size;

                                            Transform GEntity = selectedFieldInfo.cell.gameObject.transform;


                                            ///Material Fields
                                            {
                                                for (int i = 0; i < count; i++)
                                                {


                                                    ContactInfo newContactInfo = new ContactInfo();
                                                    AssetTypeValueField data = KeyValuePair.Basefield["m_SavedProperties.m_TexEnvs.Array"][i];

                                                    AssetPPtr pptr = AssetPPtr.FromField(data["second.m_Texture"]);
                                                    pptr.SetFilePathFromFile(KeyValuePair.Manager, KeyValuePair.AFinst);

                                                    Debug.Log(pptr.FilePath);

                                                    if (result.ToList().Contains(info.BundlePath) && pptr.PathId != 0 && pptr.FilePath != "")
                                                    {
                                                        var AFinst = KeyValuePair.Manager.LoadAssetsFileFromBundle(KeyValuePair.BFInst, Path.GetFileName(pptr.FilePath));
                                                        var inf = FindCorrectAsset(KeyValuePair.Manager, AFinst, pptr.PathId);

                                                        var BaseField = KeyValuePair.Manager.GetBaseField(AFinst, inf);

                                                        string name = "F";

                                                        if (BaseField["m_Name"].IsDummy == true)
                                                        {
                                                            AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                            name = id2.ToString() + " #" + inf.PathId.ToString();
                                                        }
                                                        else
                                                        {
                                                            name = BaseField["m_Name"].AsString;
                                                            if (name == null || name == "")
                                                            {
                                                                AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                                name = id2.ToString() + " #" + inf.PathId.ToString();
                                                            }
                                                        }
                                                        AssetClassID id = (AssetClassID)inf.TypeId;

                                                        Debug.Log(name);
                                                        newContactInfo.Name = name;

                                                        newContactInfo.Type = SetClassIDIdentifier(id);
                                                        newContactInfo.parentPath = Path.GetFileName(pptr.FilePath);
                                                        newContactInfo.BundlePath = info.BundlePath;
                                                        newContactInfo.pathID = inf.PathId;
                                                        newContactInfo.refPath = "/" + newContactInfo.Name;

                                                        newContactInfo.UnknownAsset = false;
                                                        newContactInfo.TypeID = id;

                                                        GameObject assetObject = new GameObject(newContactInfo.Name);
                                                        assetObject.AddComponent<TypeFieldCell>();

                                                        assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(newContactInfo);

                                                        assetObject.transform.SetParent(GEntity);
                                                    }
                                                }
                                            }

                                            ///Mesh Fields
                                            {
                                                {
                                                    ContactInfo newContactInfo = new ContactInfo();
                                                    AssetTypeValueField data = KeyValuePair.Basefield["m_Shader"];

                                                    AssetPPtr pptr = AssetPPtr.FromField(data);
                                                    pptr.SetFilePathFromFile(KeyValuePair.Manager, KeyValuePair.AFinst);

                                                    if (result.ToList().Contains(info.BundlePath) && pptr.PathId != 0 && pptr.FilePath != "")
                                                    {
                                                        var AFinst = KeyValuePair.Manager.LoadAssetsFileFromBundle(KeyValuePair.BFInst, Path.GetFileName(pptr.FilePath));
                                                        Debug.Log(pptr.FilePath);
                                                        var inf = FindCorrectAsset(KeyValuePair.Manager, AFinst, pptr.PathId);

                                                        var BaseField = KeyValuePair.Manager.GetBaseField(AFinst, inf);

                                                        string name = "F";

                                                        if (BaseField["m_Name"].IsDummy == true)
                                                        {
                                                            AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                            name = id2.ToString() + " #" + inf.PathId.ToString();
                                                        }
                                                        else
                                                        {
                                                            name = BaseField["m_Name"].AsString;
                                                            if (name == null || name == "")
                                                            {
                                                                AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                                name = id2.ToString() + " #" + inf.PathId.ToString();
                                                            }
                                                        }
                                                        AssetClassID id = (AssetClassID)inf.TypeId;

                                                        Debug.Log(name);
                                                        newContactInfo.Name = name;

                                                        newContactInfo.Type = SetClassIDIdentifier(id);
                                                        newContactInfo.parentPath = Path.GetFileName(pptr.FilePath);
                                                        newContactInfo.BundlePath = info.BundlePath;
                                                        newContactInfo.pathID = inf.PathId;
                                                        newContactInfo.refPath = "/" + newContactInfo.Name;

                                                        newContactInfo.UnknownAsset = false;
                                                        newContactInfo.TypeID = id;

                                                        GameObject assetObject = new GameObject(newContactInfo.Name);
                                                        assetObject.AddComponent<TypeFieldCell>();

                                                        assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(newContactInfo);

                                                        assetObject.transform.SetParent(GEntity);
                                                    }
                                                }
                                            }
                                            InitializeData();
                                        }
                                        else
                                        {
                                            ComplainError("This asset is not available to be further subdivided.");
                                        }
                                        KeyValuePair.Manager.UnloadAll();
                                    }
                                    else
                                    {
                                        ComplainError("Failed to generate a key value pair.");
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (result.ToList().Contains(info.parentPath))
                            {
                                DetectedFileType FileTypes = FileTypeDetector.DetectFileType(info.parentPath);
                                Debug.Log("reached here");
                                if (FileTypes == DetectedFileType.AssetsFile)
                                {
                                    var KeyValuePair = AMLoadAndGetBaseFieldAssets(info.parentPath, info.BundlePath, info.TypeID, info.pathID);

                                    if (KeyValuePair != null)
                                    {
                                        if (info.TypeID == AssetClassID.GameObject)
                                        {
                                            {
                                                Transform GEntity = selectedFieldInfo.cell.gameObject.transform;
                                                Debug.Log(info.Name);
                                                Debug.Log(info.parentPath);
                                                Debug.Log(info.pathID);
                                                Debug.Log(info.TypeID);

                                                var count = KeyValuePair.Basefield["m_Component.Array"].AsArray.size;


                                                for (int i = 0; i < count; i++)
                                                {
                                                    ContactInfo newContactInfo = new ContactInfo();
                                                    AssetTypeValueField data = KeyValuePair.Basefield["m_Component.Array"][i];

                                                    AssetPPtr pptr = AssetPPtr.FromField(data["component"]);
                                                    pptr.SetFilePathFromFile(KeyValuePair.Manager, KeyValuePair.AFinst);

                                                    if (result.ToList().Contains(pptr.FilePath))
                                                    {
                                                        KeyValuePair.Manager.UnloadAssetsFile(info.parentPath);

                                                        var AFinst = KeyValuePair.Manager.LoadAssetsFile(pptr.FilePath, false);

                                                        var inf = FindCorrectAsset(KeyValuePair.Manager, AFinst, pptr.PathId);

                                                        var BaseField = KeyValuePair.Manager.GetBaseField(AFinst, inf);

                                                        string name = "F";

                                                        if (BaseField["m_Name"].IsDummy == true)
                                                        {
                                                            AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                            name = id2.ToString() + " #" + inf.PathId.ToString();
                                                        }
                                                        else
                                                        {
                                                            name = BaseField["m_Name"].AsString;
                                                            if (name == null || name == "")
                                                            {
                                                                AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                                name = id2.ToString() + " #" + inf.PathId.ToString();
                                                            }
                                                        }
                                                        AssetClassID id = (AssetClassID)inf.TypeId;

                                                        Debug.Log(name);
                                                        newContactInfo.Name = name;

                                                        newContactInfo.Type = SetClassIDIdentifier(id);
                                                        newContactInfo.parentPath = pptr.FilePath;

                                                        newContactInfo.BundlePath = "";

                                                        newContactInfo.pathID = inf.PathId;
                                                        newContactInfo.refPath = "/" + newContactInfo.Name;

                                                        newContactInfo.UnknownAsset = false;
                                                        newContactInfo.TypeID = id;

                                                        GameObject assetObject = new GameObject(newContactInfo.Name);
                                                        assetObject.AddComponent<TypeFieldCell>();

                                                        assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(newContactInfo);

                                                        assetObject.transform.SetParent(GEntity);
                                                    }
                                                    else
                                                    {
                                                        ComplainError("You didnt load the referenced Asset file yet.\nThe name of the Asset file is :- " + Path.GetFileName(pptr.FilePath));
                                                    }
                                                }
                                                InitializeData();
                                            }
                                        }
                                        else if (info.TypeID == AssetClassID.MeshFilter)
                                        {

                                            Transform GEntity = selectedFieldInfo.cell.gameObject.transform;

                                            ///Mesh Fields
                                            {
                                                {
                                                    ContactInfo newContactInfo = new ContactInfo();
                                                    AssetTypeValueField data = KeyValuePair.Basefield["m_Mesh"];

                                                    AssetPPtr pptr = AssetPPtr.FromField(data);
                                                    pptr.SetFilePathFromFile(KeyValuePair.Manager, KeyValuePair.AFinst);

                                                    if (result.ToList().Contains(pptr.FilePath) && pptr.PathId != 0)
                                                    {
                                                        KeyValuePair.Manager.UnloadAssetsFile(info.parentPath);

                                                        var AFinst = KeyValuePair.Manager.LoadAssetsFile(pptr.FilePath, false);
                                                        var inf = FindCorrectAsset(KeyValuePair.Manager, AFinst, pptr.PathId);

                                                        var BaseField = KeyValuePair.Manager.GetBaseField(AFinst, inf);

                                                        string name = "F";

                                                        if (BaseField["m_Name"].IsDummy == true)
                                                        {
                                                            AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                            name = id2.ToString() + " #" + inf.PathId.ToString();
                                                        }
                                                        else
                                                        {
                                                            name = BaseField["m_Name"].AsString;
                                                            if (name == null || name == "")
                                                            {
                                                                AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                                name = id2.ToString() + " #" + inf.PathId.ToString();
                                                            }
                                                        }
                                                        AssetClassID id = (AssetClassID)inf.TypeId;

                                                        Debug.Log(name);
                                                        newContactInfo.Name = name;

                                                        newContactInfo.Type = SetClassIDIdentifier(id);
                                                        newContactInfo.parentPath = pptr.FilePath;
                                                        newContactInfo.BundlePath = "";
                                                        newContactInfo.pathID = inf.PathId;
                                                        newContactInfo.refPath = "/" + newContactInfo.Name;

                                                        newContactInfo.UnknownAsset = false;
                                                        newContactInfo.TypeID = id;

                                                        GameObject assetObject = new GameObject(newContactInfo.Name);
                                                        assetObject.AddComponent<TypeFieldCell>();

                                                        assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(newContactInfo);

                                                        assetObject.transform.SetParent(GEntity);

                                                    }
                                                    else if (pptr.PathId != 0)
                                                    {
                                                        ComplainError("You didnt load the referenced Asset file yet.\nThe name of the Asset file is :- " + Path.GetFileName(pptr.FilePath));
                                                    }
                                                    else
                                                    {
                                                        ComplainError("Asset cannot be further subdivided because its reference fields are null");
                                                    }
                                                }
                                            }
                                            InitializeData();
                                        }

                                        else if (info.TypeID == AssetClassID.MonoBehaviour)
                                        {

                                            Transform GEntity = selectedFieldInfo.cell.gameObject.transform;

                                            ///Mesh Fields
                                            {
                                                {
                                                    ContactInfo newContactInfo = new ContactInfo();
                                                    AssetTypeValueField data = KeyValuePair.Basefield["m_Script"];

                                                    AssetPPtr pptr = AssetPPtr.FromField(data);
                                                    pptr.SetFilePathFromFile(KeyValuePair.Manager, KeyValuePair.AFinst);

                                                    if (result.ToList().Contains(pptr.FilePath))
                                                    {
                                                        KeyValuePair.Manager.UnloadAssetsFile(info.parentPath);

                                                        var AFinst = KeyValuePair.Manager.LoadAssetsFile(pptr.FilePath, false);
                                                        var inf = FindCorrectAsset(KeyValuePair.Manager, AFinst, pptr.PathId);

                                                        var BaseField = KeyValuePair.Manager.GetBaseField(AFinst, inf);

                                                        string name = "F";

                                                        if (BaseField["m_Name"].IsDummy == true)
                                                        {
                                                            AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                            name = id2.ToString() + " #" + inf.PathId.ToString();
                                                        }
                                                        else
                                                        {
                                                            name = BaseField["m_Name"].AsString;
                                                            if (name == null || name == "")
                                                            {
                                                                AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                                name = id2.ToString() + " #" + inf.PathId.ToString();
                                                            }
                                                        }
                                                        AssetClassID id = (AssetClassID)inf.TypeId;

                                                        Debug.Log(name);
                                                        newContactInfo.Name = name + ".cs";

                                                        newContactInfo.Type = SetClassIDIdentifier(id);
                                                        newContactInfo.parentPath = pptr.FilePath;
                                                        newContactInfo.BundlePath = "";
                                                        newContactInfo.pathID = inf.PathId;
                                                        newContactInfo.refPath = "/" + newContactInfo.Name + ".cs";

                                                        newContactInfo.UnknownAsset = false;
                                                        newContactInfo.TypeID = id;



                                                        GameObject assetObject = new GameObject(newContactInfo.Name);
                                                        assetObject.AddComponent<TypeFieldCell>();

                                                        assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(newContactInfo);

                                                        assetObject.transform.SetParent(GEntity);
                                                    }
                                                    else
                                                    {
                                                        ComplainError("You didnt load the referenced Asset file yet.\nThe name of the Asset file is :- " + Path.GetFileName(pptr.FilePath));
                                                    }
                                                }
                                            }
                                            InitializeData();
                                        }

                                        else if (info.TypeID == AssetClassID.MeshRenderer)
                                        {
                                            var count = KeyValuePair.Basefield["m_Materials.Array"].AsArray.size;

                                            Transform GEntity = selectedFieldInfo.cell.gameObject.transform;

                                            for (int i = 0; i < count; i++)
                                            {
                                                ContactInfo newContactInfo = new ContactInfo();
                                                AssetTypeValueField data = KeyValuePair.Basefield["m_Materials.Array"][i];

                                                AssetPPtr pptr = AssetPPtr.FromField(data);
                                                pptr.SetFilePathFromFile(KeyValuePair.Manager, KeyValuePair.AFinst);

                                                if (result.ToList().Contains(pptr.FilePath))
                                                {
                                                    KeyValuePair.Manager.UnloadAssetsFile(info.parentPath);

                                                    var AFinst = KeyValuePair.Manager.LoadAssetsFile(pptr.FilePath, false);

                                                    var inf = FindCorrectAsset(KeyValuePair.Manager, AFinst, pptr.PathId);

                                                    var BaseField = KeyValuePair.Manager.GetBaseField(AFinst, inf);

                                                    string name = "F";

                                                    if (BaseField["m_Name"].IsDummy == true)
                                                    {
                                                        AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                        name = id2.ToString() + " #" + inf.PathId.ToString();
                                                    }
                                                    else
                                                    {
                                                        name = BaseField["m_Name"].AsString;
                                                        if (name == null || name == "")
                                                        {
                                                            AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                            name = id2.ToString() + " #" + inf.PathId.ToString();
                                                        }
                                                    }
                                                    AssetClassID id = (AssetClassID)inf.TypeId;

                                                    Debug.Log(name);
                                                    newContactInfo.Name = name;

                                                    newContactInfo.Type = SetClassIDIdentifier(id);
                                                    newContactInfo.parentPath = pptr.FilePath;
                                                    newContactInfo.BundlePath = "";
                                                    newContactInfo.pathID = inf.PathId;
                                                    newContactInfo.refPath = "/" + newContactInfo.Name;

                                                    newContactInfo.UnknownAsset = false;
                                                    newContactInfo.TypeID = id;


                                                    GameObject assetObject = new GameObject(newContactInfo.Name);
                                                    assetObject.AddComponent<TypeFieldCell>();

                                                    assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(newContactInfo);

                                                    assetObject.transform.SetParent(GEntity);
                                                }
                                                else
                                                {
                                                    ComplainError("You didnt load the referenced Asset file yet.\nThe name of the Asset file is :- " + Path.GetFileName(pptr.FilePath));
                                                }
                                            }
                                            InitializeData();
                                        }

                                        else if (info.TypeID == AssetClassID.Transform)
                                        {
                                            var count = KeyValuePair.Basefield["m_Children.Array"].AsArray.size;

                                            Transform GEntity = selectedFieldInfo.cell.gameObject.transform;

                                            for (int i = 0; i < count; i++)
                                            {
                                                ContactInfo newContactInfo = new ContactInfo();
                                                AssetTypeValueField data = KeyValuePair.Basefield["m_Children.Array"][i];

                                                AssetPPtr pptr = AssetPPtr.FromField(data);
                                                pptr.SetFilePathFromFile(KeyValuePair.Manager, KeyValuePair.AFinst);

                                                if (result.ToList().Contains(pptr.FilePath))
                                                {
                                                    KeyValuePair.Manager.UnloadAssetsFile(info.parentPath);

                                                    var AFinst = KeyValuePair.Manager.LoadAssetsFile(pptr.FilePath, false);

                                                    var inf = FindCorrectAsset(KeyValuePair.Manager, AFinst, pptr.PathId);

                                                    var BaseField = KeyValuePair.Manager.GetBaseField(AFinst, inf);

                                                    string name = "F";

                                                    if (BaseField["m_Name"].IsDummy == true)
                                                    {
                                                        AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                        name = id2.ToString() + " #" + inf.PathId.ToString();
                                                    }
                                                    else
                                                    {
                                                        name = BaseField["m_Name"].AsString;
                                                        if (name == null || name == "")
                                                        {
                                                            AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                            name = id2.ToString() + " #" + inf.PathId.ToString();
                                                        }
                                                    }
                                                    AssetClassID id = (AssetClassID)inf.TypeId;

                                                    Debug.Log(name);
                                                    newContactInfo.Name = name;

                                                    newContactInfo.Type = SetClassIDIdentifier(id);
                                                    newContactInfo.parentPath = pptr.FilePath;
                                                    newContactInfo.BundlePath = "";
                                                    newContactInfo.pathID = inf.PathId;
                                                    newContactInfo.refPath = "/" + newContactInfo.Name;

                                                    newContactInfo.UnknownAsset = false;
                                                    newContactInfo.TypeID = id;

                                                    GameObject assetObject = new GameObject(newContactInfo.Name);
                                                    assetObject.AddComponent<TypeFieldCell>();

                                                    assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(newContactInfo);

                                                    assetObject.transform.SetParent(GEntity);

                                                }
                                                else
                                                {
                                                    ComplainError("You didnt load the referenced Asset file yet.\nThe name of the Asset file is :- " + Path.GetFileName(pptr.FilePath));
                                                }
                                            }

                                            {
                                                {
                                                    {
                                                        ContactInfo newContactInfo = new ContactInfo();
                                                        AssetTypeValueField data = KeyValuePair.Basefield["m_GameObject"];

                                                        AssetPPtr pptr = AssetPPtr.FromField(data);
                                                        pptr.SetFilePathFromFile(KeyValuePair.Manager, KeyValuePair.AFinst);

                                                        if (result.ToList().Contains(pptr.FilePath))
                                                        {
                                                            KeyValuePair.Manager.UnloadAssetsFile(info.parentPath);

                                                            var AFinst = KeyValuePair.Manager.LoadAssetsFile(pptr.FilePath, false);
                                                            var inf = FindCorrectAsset(KeyValuePair.Manager, AFinst, pptr.PathId);

                                                            var BaseField = KeyValuePair.Manager.GetBaseField(AFinst, inf);

                                                            string name = "F";

                                                            if (BaseField["m_Name"].IsDummy == true)
                                                            {
                                                                AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                                name = id2.ToString() + " #" + inf.PathId.ToString();
                                                            }
                                                            else
                                                            {
                                                                name = BaseField["m_Name"].AsString;
                                                                if (name == null || name == "")
                                                                {
                                                                    AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                                    name = id2.ToString() + " #" + inf.PathId.ToString();
                                                                }
                                                            }
                                                            AssetClassID id = (AssetClassID)inf.TypeId;

                                                            Debug.Log(name);
                                                            newContactInfo.Name = name;

                                                            newContactInfo.Type = SetClassIDIdentifier(id);
                                                            newContactInfo.parentPath = pptr.FilePath;
                                                            newContactInfo.BundlePath = "";
                                                            newContactInfo.pathID = inf.PathId;
                                                            newContactInfo.refPath = "/" + newContactInfo.Name;

                                                            newContactInfo.UnknownAsset = false;
                                                            newContactInfo.TypeID = id;

                                                            GameObject assetObject = new GameObject(newContactInfo.Name);
                                                            assetObject.AddComponent<TypeFieldCell>();

                                                            assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(newContactInfo);

                                                            assetObject.transform.SetParent(GEntity);

                                                        }
                                                        else
                                                        {
                                                            ComplainError("You didnt load the referenced Asset file yet.\nThe name of the Asset file is :- " + Path.GetFileName(pptr.FilePath));
                                                        }
                                                    }
                                                }

                                            }
                                            InitializeData();
                                        }
                                        else if (info.TypeID == AssetClassID.RectTransform)
                                        {
                                            var count = KeyValuePair.Basefield["m_Children.Array"].AsArray.size;

                                            Transform GEntity = selectedFieldInfo.cell.gameObject.transform;

                                            for (int i = 0; i < count; i++)
                                            {
                                                ContactInfo newContactInfo = new ContactInfo();
                                                AssetTypeValueField data = KeyValuePair.Basefield["m_Children.Array"][i];

                                                AssetPPtr pptr = AssetPPtr.FromField(data);
                                                pptr.SetFilePathFromFile(KeyValuePair.Manager, KeyValuePair.AFinst);

                                                if (result.ToList().Contains(pptr.FilePath))
                                                {
                                                    KeyValuePair.Manager.UnloadAssetsFile(info.parentPath);

                                                    var AFinst = KeyValuePair.Manager.LoadAssetsFile(pptr.FilePath, false);

                                                    var inf = FindCorrectAsset(KeyValuePair.Manager, AFinst, pptr.PathId);

                                                    var BaseField = KeyValuePair.Manager.GetBaseField(AFinst, inf);

                                                    string name = "F";

                                                    if (BaseField["m_Name"].IsDummy == true)
                                                    {
                                                        AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                        name = id2.ToString() + " #" + inf.PathId.ToString();
                                                    }
                                                    else
                                                    {
                                                        name = BaseField["m_Name"].AsString;
                                                        if (name == null || name == "")
                                                        {
                                                            AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                            name = id2.ToString() + " #" + inf.PathId.ToString();
                                                        }
                                                    }
                                                    AssetClassID id = (AssetClassID)inf.TypeId;

                                                    Debug.Log(name);
                                                    newContactInfo.Name = name;

                                                    newContactInfo.Type = SetClassIDIdentifier(id);
                                                    newContactInfo.parentPath = pptr.FilePath;
                                                    newContactInfo.BundlePath = "";
                                                    newContactInfo.pathID = inf.PathId;
                                                    newContactInfo.refPath = "/" + newContactInfo.Name;

                                                    newContactInfo.UnknownAsset = false;
                                                    newContactInfo.TypeID = id;

                                                    GameObject assetObject = new GameObject(newContactInfo.Name);
                                                    assetObject.AddComponent<TypeFieldCell>();

                                                    assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(newContactInfo);

                                                    assetObject.transform.SetParent(GEntity);

                                                }
                                                else
                                                {
                                                    ComplainError("You didnt load the referenced Asset file yet.\nThe name of the Asset file is :- " + Path.GetFileName(pptr.FilePath));
                                                }
                                            }

                                            {
                                                {
                                                    {
                                                        ContactInfo newContactInfo = new ContactInfo();
                                                        AssetTypeValueField data = KeyValuePair.Basefield["m_GameObject"];

                                                        AssetPPtr pptr = AssetPPtr.FromField(data);
                                                        pptr.SetFilePathFromFile(KeyValuePair.Manager, KeyValuePair.AFinst);

                                                        if (result.ToList().Contains(pptr.FilePath))
                                                        {
                                                            KeyValuePair.Manager.UnloadAssetsFile(info.parentPath);

                                                            var AFinst = KeyValuePair.Manager.LoadAssetsFile(pptr.FilePath, false);
                                                            var inf = FindCorrectAsset(KeyValuePair.Manager, AFinst, pptr.PathId);

                                                            var BaseField = KeyValuePair.Manager.GetBaseField(AFinst, inf);

                                                            string name = "F";

                                                            if (BaseField["m_Name"].IsDummy == true)
                                                            {
                                                                AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                                name = id2.ToString() + " #" + inf.PathId.ToString();
                                                            }
                                                            else
                                                            {
                                                                name = BaseField["m_Name"].AsString;
                                                                if (name == null || name == "")
                                                                {
                                                                    AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                                    name = id2.ToString() + " #" + inf.PathId.ToString();
                                                                }
                                                            }
                                                            AssetClassID id = (AssetClassID)inf.TypeId;

                                                            Debug.Log(name);
                                                            newContactInfo.Name = name;

                                                            newContactInfo.Type = SetClassIDIdentifier(id);
                                                            newContactInfo.parentPath = pptr.FilePath;
                                                            newContactInfo.BundlePath = "";
                                                            newContactInfo.pathID = inf.PathId;
                                                            newContactInfo.refPath = "/" + newContactInfo.Name;

                                                            newContactInfo.UnknownAsset = false;
                                                            newContactInfo.TypeID = id;

                                                            GameObject assetObject = new GameObject(newContactInfo.Name);
                                                            assetObject.AddComponent<TypeFieldCell>();

                                                            assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(newContactInfo);

                                                            assetObject.transform.SetParent(GEntity);

                                                        }
                                                        else
                                                        {
                                                            ComplainError("You didnt load the referenced Asset file yet.\nThe name of the Asset file is :- " + Path.GetFileName(pptr.FilePath));
                                                        }
                                                    }
                                                }

                                            }
                                            InitializeData();
                                        }
                                        else if (info.TypeID == AssetClassID.SkinnedMeshRenderer)
                                        {
                                            var count = KeyValuePair.Basefield["m_Materials.Array"].AsArray.size;

                                            Transform GEntity = selectedFieldInfo.cell.gameObject.transform;

                                            ///Material Fields
                                            {
                                                for (int i = 0; i < count; i++)
                                                {
                                                    ContactInfo newContactInfo = new ContactInfo();
                                                    AssetTypeValueField data = KeyValuePair.Basefield["m_Materials.Array"][i];

                                                    AssetPPtr pptr = AssetPPtr.FromField(data);
                                                    pptr.SetFilePathFromFile(KeyValuePair.Manager, KeyValuePair.AFinst);

                                                    if (result.ToList().Contains(pptr.FilePath))
                                                    {
                                                        KeyValuePair.Manager.UnloadAssetsFile(info.parentPath);

                                                        var AFinst = KeyValuePair.Manager.LoadAssetsFile(pptr.FilePath, false);
                                                        var inf = FindCorrectAsset(KeyValuePair.Manager, AFinst, pptr.PathId);

                                                        var BaseField = KeyValuePair.Manager.GetBaseField(AFinst, inf);

                                                        string name = "F";

                                                        if (BaseField["m_Name"].IsDummy == true)
                                                        {
                                                            AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                            name = id2.ToString() + " #" + inf.PathId.ToString();
                                                        }
                                                        else
                                                        {
                                                            name = BaseField["m_Name"].AsString;
                                                            if (name == null || name == "")
                                                            {
                                                                AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                                name = id2.ToString() + " #" + inf.PathId.ToString();
                                                            }
                                                        }
                                                        AssetClassID id = (AssetClassID)inf.TypeId;

                                                        Debug.Log(name);
                                                        newContactInfo.Name = name;

                                                        newContactInfo.Type = SetClassIDIdentifier(id);
                                                        newContactInfo.parentPath = pptr.FilePath;
                                                        newContactInfo.BundlePath = "";
                                                        newContactInfo.pathID = inf.PathId;
                                                        newContactInfo.refPath = "/" + newContactInfo.Name;

                                                        newContactInfo.UnknownAsset = false;
                                                        newContactInfo.TypeID = id;

                                                        GameObject assetObject = new GameObject(newContactInfo.Name);
                                                        assetObject.AddComponent<TypeFieldCell>();

                                                        assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(newContactInfo);

                                                        assetObject.transform.SetParent(GEntity);

                                                    }
                                                    else
                                                    {
                                                        ComplainError("You didnt load the referenced Asset file yet.\nThe name of the Asset file is :- " + Path.GetFileName(pptr.FilePath));
                                                    }
                                                }
                                            }

                                            ///Mesh Fields
                                            {
                                                {
                                                    ContactInfo newContactInfo = new ContactInfo();
                                                    AssetTypeValueField data = KeyValuePair.Basefield["m_Mesh"];

                                                    AssetPPtr pptr = AssetPPtr.FromField(data);
                                                    pptr.SetFilePathFromFile(KeyValuePair.Manager, KeyValuePair.AFinst);

                                                    if (result.ToList().Contains(pptr.FilePath))
                                                    {
                                                        KeyValuePair.Manager.UnloadAssetsFile(info.parentPath);

                                                        var AFinst = KeyValuePair.Manager.LoadAssetsFile(pptr.FilePath, false);
                                                        var inf = FindCorrectAsset(KeyValuePair.Manager, AFinst, pptr.PathId);

                                                        var BaseField = KeyValuePair.Manager.GetBaseField(AFinst, inf);

                                                        string name = "F";

                                                        if (BaseField["m_Name"].IsDummy == true)
                                                        {
                                                            AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                            name = id2.ToString() + " #" + inf.PathId.ToString();
                                                        }
                                                        else
                                                        {
                                                            name = BaseField["m_Name"].AsString;
                                                            if (name == null || name == "")
                                                            {
                                                                AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                                name = id2.ToString() + " #" + inf.PathId.ToString();
                                                            }
                                                        }
                                                        AssetClassID id = (AssetClassID)inf.TypeId;

                                                        Debug.Log(name);
                                                        newContactInfo.Name = name;

                                                        newContactInfo.Type = SetClassIDIdentifier(id);
                                                        newContactInfo.parentPath = pptr.FilePath;
                                                        newContactInfo.BundlePath = "";
                                                        newContactInfo.pathID = inf.PathId;
                                                        newContactInfo.refPath = "/" + newContactInfo.Name;

                                                        newContactInfo.UnknownAsset = false;
                                                        newContactInfo.TypeID = id;

                                                        GameObject assetObject = new GameObject(newContactInfo.Name);
                                                        assetObject.AddComponent<TypeFieldCell>();

                                                        assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(newContactInfo);

                                                        assetObject.transform.SetParent(GEntity);

                                                    }
                                                    else
                                                    {
                                                        ComplainError("You didnt load the referenced Asset file yet.\nThe name of the Asset file is :- " + Path.GetFileName(pptr.FilePath));
                                                    }
                                                }
                                            }
                                            InitializeData();
                                        }
                                        else if (info.TypeID == AssetClassID.Material)
                                        {
                                            var count = KeyValuePair.Basefield["m_SavedProperties.m_TexEnvs.Array"].AsArray.size;

                                            Transform GEntity = selectedFieldInfo.cell.gameObject.transform;

                                            ///Material Fields
                                            {
                                                for (int i = 0; i < count; i++)
                                                {
                                                    ContactInfo newContactInfo = new ContactInfo();
                                                    AssetTypeValueField data = KeyValuePair.Basefield["m_SavedProperties.m_TexEnvs.Array"][i];
                                                    AssetPPtr pptr = AssetPPtr.FromField(data["second.m_Texture"]);
                                                    pptr.SetFilePathFromFile(KeyValuePair.Manager, KeyValuePair.AFinst);

                                                    Debug.Log(pptr.FilePath);

                                                    if (result.ToList().Contains(pptr.FilePath) && pptr.PathId != 0)
                                                    {
                                                        KeyValuePair.Manager.UnloadAssetsFile(info.parentPath);

                                                        var AFinst = KeyValuePair.Manager.LoadAssetsFile(pptr.FilePath, false);
                                                        var inf = FindCorrectAsset(KeyValuePair.Manager, AFinst, pptr.PathId);

                                                        var BaseField = KeyValuePair.Manager.GetBaseField(AFinst, inf);

                                                        string name = "F";

                                                        if (BaseField["m_Name"].IsDummy == true)
                                                        {
                                                            AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                            name = id2.ToString() + " #" + inf.PathId.ToString();
                                                        }
                                                        else
                                                        {
                                                            name = BaseField["m_Name"].AsString;
                                                            if (name == null || name == "")
                                                            {
                                                                AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                                name = id2.ToString() + " #" + inf.PathId.ToString();
                                                            }
                                                        }
                                                        AssetClassID id = (AssetClassID)inf.TypeId;

                                                        Debug.Log(name);
                                                        newContactInfo.Name = name;

                                                        newContactInfo.Type = SetClassIDIdentifier(id);
                                                        newContactInfo.parentPath = pptr.FilePath;
                                                        newContactInfo.BundlePath = "";
                                                        newContactInfo.pathID = inf.PathId;
                                                        newContactInfo.refPath = "/" + newContactInfo.Name;

                                                        newContactInfo.UnknownAsset = false;
                                                        newContactInfo.TypeID = id;

                                                        GameObject assetObject = new GameObject(newContactInfo.Name);
                                                        assetObject.AddComponent<TypeFieldCell>();

                                                        assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(newContactInfo);

                                                        assetObject.transform.SetParent(GEntity);

                                                    }
                                                    else if (pptr.PathId != 0)
                                                    {
                                                        ComplainError("You didnt load the referenced Asset file yet.\nThe name of the Asset file is :- " + Path.GetFileName(pptr.FilePath));
                                                    }
                                                }
                                            }

                                            ///Mesh Fields
                                            {
                                                {
                                                    ContactInfo newContactInfo = new ContactInfo();
                                                    AssetTypeValueField data = KeyValuePair.Basefield["m_Shader"];

                                                    AssetPPtr pptr = AssetPPtr.FromField(data);
                                                    pptr.SetFilePathFromFile(KeyValuePair.Manager, KeyValuePair.AFinst);

                                                    if (result.ToList().Contains(pptr.FilePath))
                                                    {
                                                        KeyValuePair.Manager.UnloadAssetsFile(info.parentPath);

                                                        var AFinst = KeyValuePair.Manager.LoadAssetsFile(pptr.FilePath, false);
                                                        var inf = FindCorrectAsset(KeyValuePair.Manager, AFinst, pptr.PathId);

                                                        var BaseField = KeyValuePair.Manager.GetBaseField(AFinst, inf);

                                                        string name = "F";

                                                        if (BaseField["m_Name"].IsDummy == true)
                                                        {
                                                            AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                            name = id2.ToString() + " #" + inf.PathId.ToString();
                                                        }
                                                        else
                                                        {
                                                            name = BaseField["m_Name"].AsString;
                                                            if (name == null || name == "")
                                                            {
                                                                AssetClassID id2 = (AssetClassID)inf.TypeId;
                                                                name = id2.ToString() + " #" + inf.PathId.ToString();
                                                            }
                                                        }
                                                        AssetClassID id = (AssetClassID)inf.TypeId;

                                                        Debug.Log(name);
                                                        newContactInfo.Name = name;

                                                        newContactInfo.Type = SetClassIDIdentifier(id);
                                                        newContactInfo.parentPath = pptr.FilePath;
                                                        newContactInfo.BundlePath = "";
                                                        newContactInfo.pathID = inf.PathId;
                                                        newContactInfo.refPath = "/" + newContactInfo.Name;

                                                        newContactInfo.UnknownAsset = false;
                                                        newContactInfo.TypeID = id;

                                                        GameObject assetObject = new GameObject(newContactInfo.Name);
                                                        assetObject.AddComponent<TypeFieldCell>();

                                                        assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(newContactInfo);

                                                        assetObject.transform.SetParent(GEntity);

                                                    }
                                                    else
                                                    {
                                                        ComplainError("You didnt load the referenced Asset file yet.\nThe name of the Asset file is :- " + Path.GetFileName(pptr.FilePath));
                                                    }
                                                }
                                            }
                                            InitializeData();
                                        }
                                        else
                                        {
                                            ComplainError("This asset is not available to be further subdivided.");
                                        }
                                        KeyValuePair.Manager.UnloadAll();
                                    }
                                    else
                                    {
                                        ComplainError("Failed to generate a key value pair.");
                                    }
                                }
                                else if (FileTypes == DetectedFileType.BundleFile)
                                {

                                }
                            }
                        }
                    }
                }
                else
                {
                    ComplainError("No files are loaded yet");
                }
            }
            else
            {
                ComplainError("You have not selected any field yet.");
            }
        }
        catch (Exception ex)
        {
            ComplainError(ex.ToString());
        }
    }
    public AssetFileInfo FindCorrectAsset(AssetsManager AM, AssetsFileInstance Afinst, long pathId)
    {
        var Afile = Afinst.file;
        AM.LoadClassDatabaseFromPackage(Afile.Metadata.UnityVersion);

        foreach (var info in Afile.AssetInfos)
        {
            if (info.PathId == pathId)
            {

                return info;
            }
        }
        return null;
    }
    public AssetFileInfo FindCorrectBundleAsset()
    {
        return null;
    }

    public AFileValuePair AMLoadAndGetBaseFieldAssets(string parentPath, string bundlePath, AssetClassID TypeID, long pathID)
    {
        var manager = new AssetsManager();

        manager.LoadClassPackage(Path.Combine(Application.persistentDataPath, "classdata.tpk"));

        if (bundlePath != "")
        {
            var bunFileInst = manager.LoadBundleFile(bundlePath, true);
            foreach (AssetBundleDirectoryInfo assetBundleDirectoryInfo in bunFileInst.file.BlockAndDirInfo.DirectoryInfos)
            {
                if (assetBundleDirectoryInfo.Name == parentPath)
                {
                    AssetsFileInstance afileInst = manager.LoadAssetsFileFromBundle(bunFileInst, assetBundleDirectoryInfo.Name, false);
                    var afile = afileInst.file;

                    if (StaticSettings.unityMetadata != "")
                    {
                        manager.LoadClassDatabaseFromPackage(StaticSettings.unityMetadata);
                    }
                    else
                    {
                        manager.LoadClassDatabaseFromPackage(afile.Metadata.UnityVersion);
                    }

                    foreach (var Info in afile.AssetInfos)
                    {
                        if (Info.TypeId == (int)TypeID && Info.PathId == pathID)
                        {
                            var BaseField = manager.GetBaseField(afileInst, Info);

                            AFileValuePair pair = new AFileValuePair();
                            pair.Manager = manager;
                            pair.BFInst = bunFileInst;
                            pair.AFinst = afileInst;
                            pair.Basefield = BaseField;
                            return pair;
                        }
                    }
                    return null;
                }
            }
        }
        else
        {
            AssetsFileInstance afileInst = manager.LoadAssetsFile(parentPath, false);
            var afile = afileInst.file;

            if (StaticSettings.unityMetadata != "")
            {
                manager.LoadClassDatabaseFromPackage(StaticSettings.unityMetadata);
            }
            else
            {
                manager.LoadClassDatabaseFromPackage(afile.Metadata.UnityVersion);
            }

            foreach (var Info in afile.AssetInfos)
            {
                if (Info.TypeId == (int)TypeID && Info.PathId == pathID)
                {
                    var BaseField = manager.GetBaseField(afileInst, Info);

                    AFileValuePair pair = new AFileValuePair();
                    pair.Manager = manager;
                    pair.AFinst = afileInst;
                    pair.Basefield = BaseField;
                    return pair;
                }
            }
            return null;

        }
        return null;
    }

    public void LoadDrag(string[] files)
    {
        result = null;

        DefameData();

        StartCoroutine(LoadNETCoroutine(files));
    }

    public void LoadGeneral()
    {
        result = null;

        DefameData();

        StartCoroutine(ShowLoadDialogCoroutine());
    }
    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");

        if (FileBrowser.Success)
        {
            this.result = FileBrowser.Result;

            int count = 0;

            LoadingBar.SetActive(true);
            string Inform = "";

            for (int i = 0; i < result.Length; i++)
            {
                yield return null;
                LoadingText.text = "Loading File:- " + Path.GetFileName(result[i]);
                Debug.Log(Path.GetFileName(result[i]));
                {
                    DetectedFileType FileTypes = FileTypeDetector.DetectFileType(result[i]);
                    if (FileTypes == DetectedFileType.AssetsFile)
                    {
                        if (!UAEFileRoot.Find("AssetsFiles"))
                        {
                            GameObject AssetsFiles = new GameObject("AssetsFiles");
                            AssetsFiles.AddComponent<TypeFieldCell>();

                            ContactInfo Info = new ContactInfo();
                            Info.Name = AssetsFiles.name;
                            Info.Type = identifiers[54];
                            AssetsFiles.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                            AssetsFiles.transform.SetParent(UAEFileRoot);
                        }

                        if (!UAEFileRoot.Find("AssetsFiles").Find("Managed"))
                        {
                            GameObject Managed = new GameObject("Managed");
                            Managed.AddComponent<TypeFieldCell>();

                            ContactInfo Info = new ContactInfo();
                            Info.Name = Managed.name;
                            Info.Type = identifiers[54];
                            Managed.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                            Managed.transform.SetParent(UAEFileRoot.Find("AssetsFiles"));
                        }

                        if (!UAEFileRoot.Find("AssetsFiles").Find("UnManaged"))
                        {
                            GameObject UnManaged = new GameObject("UnManaged");
                            UnManaged.AddComponent<TypeFieldCell>();

                            ContactInfo Info = new ContactInfo();
                            Info.Name = UnManaged.name;
                            Info.Type = identifiers[54];
                            UnManaged.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                            UnManaged.transform.SetParent(UAEFileRoot.Find("AssetsFiles"));
                        }

                        {
                            string path = result[i];
                            {
                                var manager = new AssetsManager();

                                manager.LoadClassPackage(Path.Combine(Application.persistentDataPath, "classdata.tpk"));

                                var afileInst = manager.LoadAssetsFile(path, false);
                                var afile = afileInst.file;

                                afileInst.file.GenerateQuickLookup();

                                if (StaticSettings.unityMetadata != "")
                                {
                                    manager.LoadClassDatabaseFromPackage(StaticSettings.unityMetadata);
                                }
                                else
                                {
                                    manager.LoadClassDatabaseFromPackage(afile.Metadata.UnityVersion);
                                }

                                string assetsFileName = afileInst.name;

                                if (assetsFileName.Contains("level"))
                                {

                                    ///Creating a Scene Folder
                                    if (!UAEFileRoot.Find("AssetsFiles").Find("Scenes"))
                                    {
                                        Transform AssetsFiles = UAEFileRoot.Find("AssetsFiles");
                                        GameObject Scenes = new GameObject("Scenes");
                                        Scenes.AddComponent<TypeFieldCell>();

                                        ContactInfo Info = new ContactInfo();
                                        Info.Name = Scenes.name;
                                        Info.Type = identifiers[54];
                                        Scenes.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                        Scenes.transform.SetParent(AssetsFiles);
                                    }//End;

                                    //Getting the Scene Folder;
                                    Transform ScenesFolder = UAEFileRoot.Find("AssetsFiles").Find("Scenes");


                                    ///Adding Managed to Scene
                                    if (!UAEFileRoot.Find("AssetsFiles").Find("Scenes").Find("Managed"))
                                    {
                                        GameObject Managed = new GameObject("Managed");
                                        Managed.AddComponent<TypeFieldCell>();

                                        ContactInfo Info = new ContactInfo();
                                        Info.Name = Managed.name;
                                        Info.Type = identifiers[54];
                                        Managed.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                        Managed.transform.SetParent(ScenesFolder);
                                    }//End;

                                    ///Adding The Scene
                                    {
                                        GameObject level = new GameObject(assetsFileName + ".unity3d");
                                        level.AddComponent<TypeFieldCell>();

                                        ContactInfo Info = new ContactInfo();
                                        Info.Name = assetsFileName;
                                        Info.Type = identifiers[50];
                                        level.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                        level.transform.SetParent(ScenesFolder);
                                    }//End;
                                }
                                if (assetsFileName.Contains("sharedAssets"))
                                {
                                    ///Creating a Scene Folder
                                    if (!UAEFileRoot.Find("AssetsFiles").Find("Scenes"))
                                    {
                                        Transform AssetsFiles = UAEFileRoot.Find("AssetsFiles");
                                        GameObject Scenes = new GameObject("Scenes");
                                        Scenes.AddComponent<TypeFieldCell>();

                                        ContactInfo Info = new ContactInfo();
                                        Info.Name = Scenes.name;
                                        Info.Type = identifiers[54];
                                        Scenes.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                        Scenes.transform.SetParent(AssetsFiles);
                                    }//End;

                                    //Getting the Scene Folder;
                                    Transform ScenesFolder = UAEFileRoot.Find("AssetsFiles").Find("Scenes");


                                    ///Adding Managed to Scene
                                    if (!UAEFileRoot.Find("AssetsFiles").Find("Scenes").Find("Managed"))
                                    {
                                        GameObject Managed = new GameObject("Managed");
                                        Managed.AddComponent<TypeFieldCell>();

                                        ContactInfo Info = new ContactInfo();
                                        Info.Name = Managed.name;
                                        Info.Type = identifiers[54];
                                        Managed.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                        Managed.transform.SetParent(ScenesFolder);
                                    }//End;

                                    ///Adding the SharedScene
                                    {
                                        GameObject level = new GameObject(assetsFileName + ".unity3d");
                                        level.AddComponent<TypeFieldCell>();

                                        ContactInfo Info = new ContactInfo();
                                        Info.Name = assetsFileName;
                                        Info.Type = identifiers[50];
                                        level.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                        level.transform.SetParent(ScenesFolder);
                                    }//End;
                                }

                                if (!afileInst.name.Contains("level") && !afileInst.name.Contains("sharedassets"))
                                {
                                    foreach (var Info in afile.Metadata.AssetInfos)
                                    {
                                        yield return null;
                                        if (Info.TypeId == (int)AssetClassID.GameObject)
                                        {
                                            if (!UAEFileRoot.Find("AssetsFiles").Find("G'O RX"))
                                            {
                                                GameObject GoRX = new GameObject("G'O RX");
                                                GoRX.AddComponent<TypeFieldCell>();

                                                ContactInfo Infos = new ContactInfo();
                                                Infos.Name = GoRX.name;
                                                Infos.Type = identifiers[54];
                                                GoRX.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Infos);

                                                GoRX.transform.SetParent(UAEFileRoot.Find("AssetsFiles"));
                                            }

                                            Transform GORX = UAEFileRoot.Find("AssetsFiles").Find("G'O RX");

                                            string name = "F";
                                            AssetTypeValueField BaseField = new AssetTypeValueField();
                                            try
                                            {
                                                BaseField = manager.GetBaseField(afileInst, Info);

                                                AssetClassID id = new AssetClassID();
                                                id = (AssetClassID)Info.TypeId;

                                                if (BaseField["m_Name"].IsDummy == true)
                                                {
                                                    name = id.ToString() + " #" + Info.PathId.ToString();
                                                }
                                                else
                                                {
                                                    name = BaseField["m_Name"].AsString;
                                                    if (name == null || name == "")
                                                    {
                                                        name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                }
                                                LoadingAssetText.text = "Loading Asset:- " + name;

                                                // #Transform

                                                AssetTypeValueField data = BaseField["m_Component.Array"][0];
                                                AssetPPtr pptr = AssetPPtr.FromField(data["component"]);


                                                AssetTypeValueField Transform = new AssetTypeValueField();

                                                if (pptr.FileId == 0)
                                                {
                                                    Transform = manager.GetBaseField(afileInst, pptr.PathId);
                                                }
                                                AssetPPtr pptr2 = AssetPPtr.FromField(Transform["m_Father"]);

                                                if (pptr2.FileId == 0 && pptr2.PathId == 0)
                                                {


                                                    ContactInfo contactInfo = new ContactInfo();

                                                    contactInfo.Name = name;

                                                    contactInfo.Type = SetClassIDIdentifier(id);

                                                    contactInfo.parentPath = path;

                                                    contactInfo.BundlePath = "";

                                                    contactInfo.pathID = Info.PathId;

                                                    contactInfo.refPath = "/" + contactInfo.Name;

                                                    contactInfo.UnknownAsset = false;

                                                    contactInfo.TypeID = id;

                                                    GameObject assetObject = new GameObject(contactInfo.Name);
                                                    assetObject.AddComponent<TypeFieldCell>();

                                                    assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(contactInfo);

                                                    assetObject.transform.SetParent(GORX);

                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                if (Inform == "")
                                                {
                                                    Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                                else
                                                {
                                                    Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                            }
                                            yield return null;
                                        }


                                        ///Managed Section
                                        {
                                            AssetClassID ids = new AssetClassID();
                                            ids = (AssetClassID)Info.TypeId;
                                            if (!UAEFileRoot.Find("AssetsFiles").Find("Managed").Find(ids.ToString()))
                                            {
                                                GameObject Entity = new GameObject(ids.ToString());
                                                Entity.AddComponent<TypeFieldCell>();

                                                ContactInfo Infos = new ContactInfo();
                                                Infos.Name = Entity.name;
                                                Infos.Type = identifiers[54];
                                                Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Infos);

                                                Entity.transform.SetParent(UAEFileRoot.Find("AssetsFiles").Find("Managed"));
                                            }

                                            Transform GEntity = UAEFileRoot.Find("AssetsFiles").Find("Managed").Find(ids.ToString());

                                            AssetTypeValueField BaseField = new AssetTypeValueField();

                                            string Name = "F";
                                            try
                                            {
                                                BaseField = manager.GetBaseField(afileInst, Info);


                                                if (BaseField["m_Name"].IsDummy == true)
                                                {
                                                    AssetClassID id = (AssetClassID)Info.TypeId;
                                                    Name = id.ToString() + " #" + Info.PathId.ToString();
                                                }
                                                else
                                                {
                                                    Name = BaseField["m_Name"].AsString;
                                                    if (Name == null || Name == "")
                                                    {
                                                        AssetClassID id = (AssetClassID)Info.TypeId;
                                                        Name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                }
                                                LoadingAssetText.text = "Loading Asset:- " + Name;
                                                Debug.Log(name);


                                                ContactInfo info = new ContactInfo();
                                                if (Info.TypeId == (int)AssetClassID.MonoScript)
                                                {
                                                    info.Name = Name + ".cs";
                                                    info.refPath = "/" + Name + ".cs";
                                                }
                                                else
                                                {
                                                    info.Name = Name;
                                                    info.refPath = "/" + Name;
                                                }
                                                info.parentPath = path;
                                                info.BundlePath = "";
                                                info.pathID = Info.PathId;
                                                info.Type = SetClassIDIdentifier((AssetClassID)Info.TypeId);
                                                info.TypeID = (AssetClassID)Info.TypeId;
                                                info.UnknownAsset = false;


                                                GameObject assetObject = new GameObject(info.Name);
                                                assetObject.AddComponent<TypeFieldCell>();

                                                assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(info);

                                                assetObject.transform.SetParent(GEntity);
                                                //info.FileID = afileInst.file.
                                            }
                                            catch (Exception ex)
                                            {
                                                if (Inform == "")
                                                {
                                                    Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                                else
                                                {
                                                    Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                            }
                                            yield return null;

                                        }//End;

                                        ///UnManaged Section
                                        {
                                            Transform GEntity = UAEFileRoot.Find("AssetsFiles").Find("UnManaged");

                                            AssetTypeValueField BaseField = new AssetTypeValueField();

                                            string Name = "F";
                                            try
                                            {
                                                BaseField = manager.GetBaseField(afileInst, Info);


                                                if (BaseField["m_Name"].IsDummy == true)
                                                {
                                                    AssetClassID id = (AssetClassID)Info.TypeId;
                                                    Name = id.ToString() + " #" + Info.PathId.ToString();
                                                }
                                                else
                                                {
                                                    Name = BaseField["m_Name"].AsString;
                                                    if (Name == null || Name == "")
                                                    {
                                                        AssetClassID id = (AssetClassID)Info.TypeId;
                                                        Name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                }
                                                LoadingAssetText.text = "Loading Asset:- " + Name;
                                                Debug.Log(name);


                                                ContactInfo info = new ContactInfo();
                                                if (Info.TypeId == (int)AssetClassID.MonoScript)
                                                {
                                                    info.Name = Name + ".cs";
                                                    info.refPath = "/" + Name + ".cs";
                                                }
                                                else
                                                {
                                                    info.Name = Name;
                                                    info.refPath = "/" + Name;
                                                }
                                                info.parentPath = path;
                                                info.BundlePath = "";
                                                info.pathID = Info.PathId;
                                                info.Type = SetClassIDIdentifier((AssetClassID)Info.TypeId);
                                                info.TypeID = (AssetClassID)Info.TypeId;
                                                info.UnknownAsset = false;




                                                GameObject assetObject = new GameObject(info.Name);
                                                assetObject.AddComponent<TypeFieldCell>();

                                                assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(info);

                                                assetObject.transform.SetParent(GEntity);
                                                //info.FileID = afileInst.file.
                                            }
                                            catch (Exception ex)
                                            {
                                                if (Inform == "")
                                                {
                                                    Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                                else
                                                {
                                                    Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                            }
                                            yield return null;

                                        }//End;
                                    }
                                }
                                else if (afileInst.name.Contains("level"))
                                {
                                    foreach (var Info in afile.Metadata.AssetInfos)
                                    {
                                        if (Info.TypeId == (int)AssetClassID.GameObject)
                                        {
                                            Transform GEntity = UAEFileRoot.Find("AssetsFiles").Find("Scenes").Find(assetsFileName + ".unity3d");

                                            string name = "F";
                                            AssetTypeValueField BaseField = new AssetTypeValueField();
                                            try
                                            {

                                                BaseField = manager.GetBaseField(afileInst, Info);

                                                AssetClassID id = new AssetClassID();
                                                id = (AssetClassID)Info.TypeId;

                                                if (BaseField["m_Name"].IsDummy == true)
                                                {
                                                    name = id.ToString() + " #" + Info.PathId.ToString();
                                                }
                                                else
                                                {
                                                    name = BaseField["m_Name"].AsString;
                                                    if (name == null || name == "")
                                                    {
                                                        name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                }

                                                LoadingAssetText.text = "Loading Asset:- " + name;
                                                // #Transform

                                                AssetTypeValueField data = BaseField["m_Component.Array"][0];
                                                AssetPPtr pptr = AssetPPtr.FromField(data["component"]);


                                                AssetTypeValueField Transform = new AssetTypeValueField();

                                                if (pptr.FileId == 0)
                                                {
                                                    Transform = manager.GetBaseField(afileInst, pptr.PathId);
                                                }
                                                AssetPPtr pptr2 = AssetPPtr.FromField(Transform["m_Father"]);

                                                if (pptr2.FileId == 0 && pptr2.PathId == 0)
                                                {

                                                    ContactInfo contactInfo = new ContactInfo();

                                                    contactInfo.Name = name;

                                                    contactInfo.Type = SetClassIDIdentifier(AssetClassID.Special);

                                                    contactInfo.parentPath = path;

                                                    contactInfo.BundlePath = "";

                                                    contactInfo.pathID = Info.PathId;

                                                    contactInfo.refPath = "/" + contactInfo.Name;

                                                    contactInfo.UnknownAsset = false;

                                                    contactInfo.TypeID = id;

                                                    GameObject Entity = new GameObject(contactInfo.Name);
                                                    Entity.AddComponent<TypeFieldCell>();


                                                    Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(contactInfo);

                                                    Entity.transform.SetParent(GEntity);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                if (Inform == "")
                                                {
                                                    Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                                else
                                                {
                                                    Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                            }
                                            yield return null;
                                        }

                                        {
                                            AssetClassID ids = new AssetClassID();
                                            ids = (AssetClassID)Info.TypeId;
                                            if (!UAEFileRoot.Find("AssetsFiles").Find("Scenes").Find("Managed").Find(ids.ToString()))
                                            {
                                                GameObject Entity = new GameObject(ids.ToString());
                                                Entity.AddComponent<TypeFieldCell>();

                                                ContactInfo Infos = new ContactInfo();
                                                Infos.Name = Entity.name;
                                                Infos.Type = identifiers[54];
                                                Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Infos);

                                                Entity.transform.SetParent(UAEFileRoot.Find("AssetsFiles").Find("Scenes").Find("Managed"));
                                            }


                                            Transform GEntity = UAEFileRoot.Find("AssetsFiles").Find("Scenes").Find("Managed").Find(ids.ToString());



                                            AssetTypeValueField BaseField = new AssetTypeValueField();

                                            string Name = "F";
                                            try
                                            {
                                                BaseField = manager.GetBaseField(afileInst, Info);

                                                if (BaseField["m_Name"].IsDummy == true)
                                                {
                                                    AssetClassID id = (AssetClassID)Info.TypeId;
                                                    Name = id.ToString() + " #" + Info.PathId.ToString();
                                                }
                                                else
                                                {
                                                    Name = BaseField["m_Name"].AsString;
                                                    if (Name == null || Name == "")
                                                    {
                                                        AssetClassID id = (AssetClassID)Info.TypeId;
                                                        Name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                }
                                                LoadingAssetText.text = "Loading Asset:- " + Name;
                                                Debug.Log(name);

                                                ContactInfo info = new ContactInfo();
                                                if (Info.TypeId == (int)AssetClassID.MonoScript)
                                                {
                                                    info.Name = Name + ".cs";
                                                    info.refPath = "/" + Name + ".cs";
                                                }
                                                else
                                                {
                                                    info.Name = Name;
                                                    info.refPath = "/" + Name;
                                                }
                                                info.parentPath = path;
                                                info.BundlePath = "";
                                                info.pathID = Info.PathId;
                                                info.Type = SetClassIDIdentifier((AssetClassID)Info.TypeId);
                                                info.TypeID = (AssetClassID)Info.TypeId;
                                                info.UnknownAsset = false;
                                                //info.FileID = afileInst.file.


                                                GameObject Entity = new GameObject(info.Name);
                                                Entity.AddComponent<TypeFieldCell>();


                                                Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(info);

                                                Entity.transform.SetParent(GEntity);
                                            }
                                            catch (Exception ex)
                                            {
                                                if (Inform == "")
                                                {
                                                    Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                                else
                                                {
                                                    Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                            }
                                            yield return null;
                                        }
                                    }
                                }
                                else if (afileInst.name.Contains("sharedassets"))
                                {
                                    foreach (var Info in afile.Metadata.AssetInfos)
                                    {
                                        if (Info.TypeId == (int)AssetClassID.GameObject)
                                        {
                                            Transform GEntity = UAEFileRoot.Find("AssetsFiles").Find("Scenes").Find(assetsFileName + ".unity3d");

                                            string name = "F";
                                            AssetTypeValueField BaseField = new AssetTypeValueField();
                                            try
                                            {

                                                BaseField = manager.GetBaseField(afileInst, Info);

                                                AssetClassID id = new AssetClassID();
                                                id = (AssetClassID)Info.TypeId;

                                                if (BaseField["m_Name"].IsDummy == true)
                                                {
                                                    name = id.ToString() + " #" + Info.PathId.ToString();
                                                }
                                                else
                                                {
                                                    name = BaseField["m_Name"].AsString;
                                                    if (name == null || name == "")
                                                    {
                                                        name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                }

                                                LoadingAssetText.text = "Loading Asset:- " + name;
                                                // #Transform

                                                AssetTypeValueField data = BaseField["m_Component.Array"][0];
                                                AssetPPtr pptr = AssetPPtr.FromField(data["component"]);


                                                AssetTypeValueField Transform = new AssetTypeValueField();

                                                if (pptr.FileId == 0)
                                                {
                                                    Transform = manager.GetBaseField(afileInst, pptr.PathId);
                                                }
                                                AssetPPtr pptr2 = AssetPPtr.FromField(Transform["m_Father"]);

                                                if (pptr2.FileId == 0 && pptr2.PathId == 0)
                                                {

                                                    ContactInfo contactInfo = new ContactInfo();

                                                    contactInfo.Name = name;

                                                    contactInfo.Type = SetClassIDIdentifier(AssetClassID.Special);

                                                    contactInfo.parentPath = path;

                                                    contactInfo.BundlePath = "";

                                                    contactInfo.pathID = Info.PathId;

                                                    contactInfo.refPath = "/" + contactInfo.Name;

                                                    contactInfo.UnknownAsset = false;

                                                    contactInfo.TypeID = id;

                                                    GameObject Entity = new GameObject(contactInfo.Name);
                                                    Entity.AddComponent<TypeFieldCell>();


                                                    Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(contactInfo);

                                                    Entity.transform.SetParent(GEntity);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                if (Inform == "")
                                                {
                                                    Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                                else
                                                {
                                                    Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                            }
                                            yield return null;
                                        }

                                        {
                                            AssetClassID ids = new AssetClassID();
                                            ids = (AssetClassID)Info.TypeId;
                                            if (!UAEFileRoot.Find("AssetsFiles").Find("Scenes").Find("Managed").Find(ids.ToString()))
                                            {
                                                GameObject Entity = new GameObject(ids.ToString());
                                                Entity.AddComponent<TypeFieldCell>();

                                                ContactInfo Infos = new ContactInfo();
                                                Infos.Name = Entity.name;
                                                Infos.Type = identifiers[54];
                                                Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Infos);

                                                Entity.transform.SetParent(UAEFileRoot.Find("AssetsFiles").Find("Scenes").Find("Managed"));
                                            }


                                            Transform GEntity = UAEFileRoot.Find("AssetsFiles").Find("Scenes").Find("Managed").Find(ids.ToString());



                                            AssetTypeValueField BaseField = new AssetTypeValueField();

                                            string Name = "F";
                                            try
                                            {
                                                BaseField = manager.GetBaseField(afileInst, Info);

                                                if (BaseField["m_Name"].IsDummy == true)
                                                {
                                                    AssetClassID id = (AssetClassID)Info.TypeId;
                                                    Name = id.ToString() + " #" + Info.PathId.ToString();
                                                }
                                                else
                                                {
                                                    Name = BaseField["m_Name"].AsString;
                                                    if (Name == null || Name == "")
                                                    {
                                                        AssetClassID id = (AssetClassID)Info.TypeId;
                                                        Name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                }
                                                LoadingAssetText.text = "Loading Asset:- " + Name;
                                                Debug.Log(name);

                                                ContactInfo info = new ContactInfo();
                                                if (Info.TypeId == (int)AssetClassID.MonoScript)
                                                {
                                                    info.Name = Name + ".cs";
                                                    info.refPath = "/" + Name + ".cs";
                                                }
                                                else
                                                {
                                                    info.Name = Name;
                                                    info.refPath = "/" + Name;
                                                }
                                                info.parentPath = path;
                                                info.BundlePath = "";
                                                info.pathID = Info.PathId;
                                                info.Type = SetClassIDIdentifier((AssetClassID)Info.TypeId);
                                                info.TypeID = (AssetClassID)Info.TypeId;
                                                info.UnknownAsset = false;
                                                //info.FileID = afileInst.file.


                                                GameObject Entity = new GameObject(info.Name);
                                                Entity.AddComponent<TypeFieldCell>();


                                                Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(info);

                                                Entity.transform.SetParent(GEntity);
                                            }
                                            catch (Exception ex)
                                            {
                                                if (Inform == "")
                                                {
                                                    Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                                else
                                                {
                                                    Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                            }
                                            yield return null;
                                        }
                                    }
                                }
                                manager.UnloadAll();
                            }
                        }
                        count = count + 1;
                    }
                    else if (FileTypes == DetectedFileType.BundleFile)
                    {
                        {
                            if (!UAEFileRoot.Find("Bundles"))
                            {
                                GameObject Bundles = new GameObject("Bundles");
                                Bundles.AddComponent<TypeFieldCell>();

                                ContactInfo Info = new ContactInfo();
                                Info.Name = Bundles.name;
                                Info.Type = identifiers[54];
                                Bundles.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                Bundles.transform.SetParent(UAEFileRoot);
                            }

                            if (!UAEFileRoot.Find("Bundles").Find("Managed"))
                            {
                                GameObject Managed = new GameObject("Managed");
                                Managed.AddComponent<TypeFieldCell>();

                                ContactInfo Info = new ContactInfo();
                                Info.Name = Managed.name;
                                Info.Type = identifiers[54];
                                Managed.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                Managed.transform.SetParent(UAEFileRoot.Find("Bundles"));
                            }

                            if (!UAEFileRoot.Find("Bundles").Find("UnManaged"))
                            {
                                GameObject UnManaged = new GameObject("UnManaged");
                                UnManaged.AddComponent<TypeFieldCell>();

                                ContactInfo Info = new ContactInfo();
                                Info.Name = UnManaged.name;
                                Info.Type = identifiers[54];
                                UnManaged.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);


                                UnManaged.transform.SetParent(UAEFileRoot.Find("Bundles"));
                            }

                            string path = result[i];
                            {
                                var manager = new AssetsManager();
                                manager.LoadClassPackage(Path.Combine(Application.persistentDataPath, "classdata.tpk"));

                                var bunfileInst = manager.LoadBundleFile(path, true);

                                string levelName = "";

                                foreach (AssetBundleDirectoryInfo assetBundleDirectoryInfo in bunfileInst.file.BlockAndDirInfo.DirectoryInfos)
                                {
                                    yield return null;
                                    string assetsFileName = assetBundleDirectoryInfo.Name;

                                    if (assetsFileName.EndsWith(".sharedAssets") && levelName == "")
                                    {
                                        levelName = assetsFileName.Split(".")[0];
                                    }

                                    if (assetsFileName.EndsWith(".sharedAssets"))
                                    {
                                        ///Creating a Scene Folder
                                        if (!UAEFileRoot.Find("Bundles").Find("Scenes"))
                                        {
                                            Transform Bundles = UAEFileRoot.Find("Bundles");
                                            GameObject Scenes = new GameObject("Scenes");
                                            Scenes.AddComponent<TypeFieldCell>();

                                            ContactInfo Info = new ContactInfo();
                                            Info.Name = Bundles.name;
                                            Info.Type = identifiers[54];
                                            Scenes.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                            Scenes.transform.SetParent(Bundles);
                                        }//End;

                                        //Getting the Scene Folder;
                                        Transform ScenesFolder = UAEFileRoot.Find("Bundles").Find("Scenes");


                                        ///Adding Managed to Scene
                                        if (!UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed"))
                                        {
                                            GameObject Managed = new GameObject("Managed");
                                            Managed.AddComponent<TypeFieldCell>();

                                            ContactInfo Info = new ContactInfo();
                                            Info.Name = Managed.name;
                                            Info.Type = identifiers[54];
                                            Managed.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                            Managed.transform.SetParent(ScenesFolder);
                                        }//End;

                                        ///Adding the SharedScene
                                        {
                                            GameObject level = new GameObject(assetsFileName + ".unity3d");
                                            level.AddComponent<TypeFieldCell>();

                                            ContactInfo Info = new ContactInfo();
                                            Info.Name = assetsFileName;
                                            Info.Type = identifiers[50];
                                            level.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                            level.transform.SetParent(ScenesFolder);
                                        }//End;

                                        ///Adding the Shared ref Scene
                                        {
                                            GameObject level = new GameObject(levelName + ".unity3d");
                                            level.AddComponent<TypeFieldCell>();

                                            ContactInfo Info = new ContactInfo();
                                            Info.Name = levelName;
                                            Info.Type = identifiers[50];
                                            level.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                            level.transform.SetParent(ScenesFolder);
                                        }//End;
                                    }
                                    
                                    if (assetsFileName.EndsWith(".assets") && assetsFileName.Contains("sharedassets"))
                                    {
                                        ///Creating a Scene Folder
                                        if (!UAEFileRoot.Find("Bundles").Find("Scenes"))
                                        {
                                            Transform Bundles = UAEFileRoot.Find("Bundles");
                                            GameObject Scenes = new GameObject("Scenes");
                                            Scenes.AddComponent<TypeFieldCell>();

                                            ContactInfo Info = new ContactInfo();
                                            Info.Name = Bundles.name;
                                            Info.Type = identifiers[54];
                                            Scenes.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                            Scenes.transform.SetParent(Bundles);
                                        }//End;

                                        //Getting the Scene Folder;
                                        Transform ScenesFolder = UAEFileRoot.Find("Bundles").Find("Scenes");


                                        ///Adding Managed to Scene
                                        if (!UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed"))
                                        {
                                            GameObject Managed = new GameObject("Managed");
                                            Managed.AddComponent<TypeFieldCell>();

                                            ContactInfo Info = new ContactInfo();
                                            Info.Name = Managed.name;
                                            Info.Type = identifiers[54];
                                            Managed.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                            Managed.transform.SetParent(ScenesFolder);
                                        }//End;

                                        ///Adding the SharedScene
                                        {
                                            GameObject level = new GameObject(assetsFileName + ".unity3d");
                                            level.AddComponent<TypeFieldCell>();

                                            ContactInfo Info = new ContactInfo();
                                            Info.Name = assetsFileName;
                                            Info.Type = identifiers[50];
                                            level.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                            level.transform.SetParent(ScenesFolder);
                                        }//End;
                                    }


                                    if (assetsFileName.Contains("level"))
                                    {

                                        ///Creating a Scene Folder
                                        if (!UAEFileRoot.Find("Bundles").Find("Scenes"))
                                        {
                                            Transform Bundles = UAEFileRoot.Find("Bundles");
                                            GameObject Scenes = new GameObject("Scenes");
                                            Scenes.AddComponent<TypeFieldCell>();

                                            ContactInfo Info = new ContactInfo();
                                            Info.Name = Bundles.name;
                                            Info.Type = identifiers[54];
                                            Scenes.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                            Scenes.transform.SetParent(Bundles);
                                        }//End;

                                        //Getting the Scene Folder;
                                        Transform ScenesFolder = UAEFileRoot.Find("Bundles").Find("Scenes");


                                        ///Adding Managed to Scene
                                        if (!UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed"))
                                        {
                                            GameObject Managed = new GameObject("Managed");
                                            Managed.AddComponent<TypeFieldCell>();

                                            ContactInfo Info = new ContactInfo();
                                            Info.Name = Managed.name;
                                            Info.Type = identifiers[54];
                                            Managed.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                            Managed.transform.SetParent(ScenesFolder);
                                        }//End;

                                        ///Adding The Scene
                                        {
                                            GameObject level = new GameObject(assetsFileName + ".unity3d");
                                            level.AddComponent<TypeFieldCell>();

                                            ContactInfo Info = new ContactInfo();
                                            Info.Name = assetsFileName;
                                            Info.Type = identifiers[50];
                                            level.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                            level.transform.SetParent(ScenesFolder);
                                        }//End;
                                    }



                                    Debug.Log("definite");

                                    if (!assetsFileName.EndsWith(".sharedAssets") && assetsFileName != levelName && !assetsFileName.EndsWith(".resource") && !assetsFileName.EndsWith(".resS") && !assetsFileName.Contains("sharedassets") && !assetsFileName.Contains("level"))
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

                                        Debug.Log("it reached");
                                        foreach (var Info in afile.Metadata.AssetInfos)
                                        {
                                            yield return null;
                                            if (Info.TypeId == (int)AssetClassID.GameObject)
                                            {
                                                if (!UAEFileRoot.Find("Bundles").Find("G'O RX"))
                                                {
                                                    GameObject GoRX = new GameObject("G'O RX");
                                                    GoRX.AddComponent<TypeFieldCell>();

                                                    ContactInfo Infos = new ContactInfo();
                                                    Infos.Name = GoRX.name;
                                                    Infos.Type = identifiers[54];
                                                    GoRX.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Infos);

                                                    GoRX.transform.SetParent(UAEFileRoot.Find("Bundles"));
                                                }

                                                Transform GORX = UAEFileRoot.Find("Bundles").Find("G'O RX");

                                                string name = "F";
                                                AssetTypeValueField BaseField = new AssetTypeValueField();
                                                try
                                                {
                                                    BaseField = manager.GetBaseField(afileInst, Info);

                                                    AssetClassID id = new AssetClassID();
                                                    id = (AssetClassID)Info.TypeId;

                                                    if (BaseField["m_Name"].IsDummy == true)
                                                    {
                                                        name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                    else
                                                    {
                                                        name = BaseField["m_Name"].AsString;
                                                        if (name == null || name == "")
                                                        {
                                                            name = id.ToString() + " #" + Info.PathId.ToString();
                                                        }
                                                    }
                                                    LoadingAssetText.text = "Loading Asset:- " + name;
                                                    // #Transform

                                                    AssetTypeValueField data = BaseField["m_Component.Array"][0];
                                                    AssetPPtr pptr = AssetPPtr.FromField(data["component"]);


                                                    AssetTypeValueField Transform = new AssetTypeValueField();

                                                    if (pptr.FileId == 0)
                                                    {
                                                        Transform = manager.GetBaseField(afileInst, pptr.PathId);
                                                    }
                                                    AssetPPtr pptr2 = AssetPPtr.FromField(Transform["m_Father"]);

                                                    if (pptr2.FileId == 0 && pptr2.PathId == 0)
                                                    {

                                                        ContactInfo contactInfo = new ContactInfo();

                                                        contactInfo.Name = name;

                                                        contactInfo.Type = SetClassIDIdentifier(id);

                                                        contactInfo.parentPath = afileInst.name;

                                                        contactInfo.BundlePath = path;

                                                        contactInfo.pathID = Info.PathId;

                                                        contactInfo.refPath = "/" + contactInfo.Name;

                                                        contactInfo.UnknownAsset = false;

                                                        contactInfo.TypeID = id;

                                                        GameObject Entity = new GameObject(contactInfo.Name);
                                                        Entity.AddComponent<TypeFieldCell>();


                                                        Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(contactInfo);

                                                        Entity.transform.SetParent(GORX);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {

                                                    if (Inform == "")
                                                    {
                                                        Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                    else
                                                    {
                                                        Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                }
                                                yield return null;
                                            }

                                            //Managed
                                            {
                                                AssetClassID ids = new AssetClassID();
                                                ids = (AssetClassID)Info.TypeId;
                                                if (!UAEFileRoot.Find("Bundles").Find("Managed").Find(ids.ToString()))
                                                {
                                                    GameObject Entity = new GameObject(ids.ToString());
                                                    Entity.AddComponent<TypeFieldCell>();

                                                    ContactInfo Infos = new ContactInfo();
                                                    Infos.Name = Entity.name;
                                                    Infos.Type = identifiers[54];
                                                    Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Infos);

                                                    Entity.transform.SetParent(UAEFileRoot.Find("Bundles").Find("Managed"));
                                                }

                                                Transform GEntity = UAEFileRoot.Find("Bundles").Find("Managed").Find(ids.ToString());


                                                AssetTypeValueField BaseField = new AssetTypeValueField();

                                                string Name = "F";
                                                try
                                                {
                                                    BaseField = manager.GetBaseField(afileInst, Info);

                                                    if (BaseField["m_Name"].IsDummy == true)
                                                    {
                                                        AssetClassID id = (AssetClassID)Info.TypeId;
                                                        Name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                    else
                                                    {
                                                        Name = BaseField["m_Name"].AsString;
                                                        if (Name == null || Name == "")
                                                        {
                                                            AssetClassID id = (AssetClassID)Info.TypeId;
                                                            Name = id.ToString() + " #" + Info.PathId.ToString();
                                                        }
                                                    }
                                                    LoadingAssetText.text = "Loading Asset:- " + Name;
                                                    Debug.Log(name);

                                                    ContactInfo info = new ContactInfo();
                                                    if (Info.TypeId == (int)AssetClassID.MonoScript)
                                                    {
                                                        info.Name = Name + ".cs";
                                                        info.refPath = "/" + Name + ".cs";
                                                    }
                                                    else
                                                    {
                                                        info.Name = Name;
                                                        info.refPath = "/" + Name;
                                                    }
                                                    info.parentPath = afileInst.name;
                                                    info.BundlePath = path;
                                                    info.pathID = Info.PathId;
                                                    info.Type = SetClassIDIdentifier((AssetClassID)Info.TypeId);
                                                    info.TypeID = (AssetClassID)Info.TypeId;
                                                    info.UnknownAsset = false;
                                                    //info.FileID = afileInst.file.

                                                    GameObject Entity = new GameObject(info.Name);
                                                    Entity.AddComponent<TypeFieldCell>();


                                                    Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(info);

                                                    Entity.transform.SetParent(GEntity);
                                                }
                                                catch (Exception ex)
                                                {

                                                    if (Inform == "")
                                                    {
                                                        Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                    else
                                                    {
                                                        Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                }
                                                yield return null;
                                            }
                                            //UnManaged
                                            {
                                                Transform GEntity = UAEFileRoot.Find("Bundles").Find("UnManaged");


                                                AssetTypeValueField BaseField = new AssetTypeValueField();

                                                string Name = "F";
                                                try
                                                {
                                                    BaseField = manager.GetBaseField(afileInst, Info);

                                                    if (BaseField["m_Name"].IsDummy == true)
                                                    {
                                                        AssetClassID id = (AssetClassID)Info.TypeId;
                                                        Name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                    else
                                                    {
                                                        Name = BaseField["m_Name"].AsString;
                                                        if (Name == null || Name == "")
                                                        {
                                                            AssetClassID id = (AssetClassID)Info.TypeId;
                                                            Name = id.ToString() + " #" + Info.PathId.ToString();
                                                        }
                                                    }
                                                    LoadingAssetText.text = "Loading Asset:- " + Name;
                                                    Debug.Log(name);

                                                    ContactInfo info = new ContactInfo();
                                                    if (Info.TypeId == (int)AssetClassID.MonoScript)
                                                    {
                                                        info.Name = Name + ".cs";
                                                        info.refPath = "/" + Name + ".cs";
                                                    }
                                                    else
                                                    {
                                                        info.Name = Name;
                                                        info.refPath = "/" + Name;
                                                    }
                                                    info.parentPath = afileInst.name;
                                                    info.BundlePath = path;
                                                    info.pathID = Info.PathId;
                                                    info.Type = SetClassIDIdentifier((AssetClassID)Info.TypeId);
                                                    info.TypeID = (AssetClassID)Info.TypeId;
                                                    info.UnknownAsset = false;
                                                    //info.FileID = afileInst.file.

                                                    GameObject Entity = new GameObject(info.Name);
                                                    Entity.AddComponent<TypeFieldCell>();


                                                    Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(info);

                                                    Entity.transform.SetParent(GEntity);
                                                }
                                                catch (Exception ex)
                                                {

                                                    if (Inform == "")
                                                    {
                                                        Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                    else
                                                    {
                                                        Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                }
                                                yield return null;
                                            }
                                        }
                                    }
                                    if (assetsFileName.Contains("level"))
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

                                        foreach (var Info in afile.Metadata.AssetInfos)
                                        {
                                            yield return null;
                                            if (Info.TypeId == (int)AssetClassID.GameObject)
                                            {
                                                Transform GEntity = UAEFileRoot.Find("Bundles").Find("Scenes").Find(assetsFileName + ".unity3d");

                                                string name = "F";
                                                AssetTypeValueField BaseField = new AssetTypeValueField();
                                                try
                                                {
                                                    BaseField = manager.GetBaseField(afileInst, Info);

                                                    AssetClassID id = new AssetClassID();
                                                    id = (AssetClassID)Info.TypeId;

                                                    if (BaseField["m_Name"].IsDummy == true)
                                                    {
                                                        name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                    else
                                                    {
                                                        name = BaseField["m_Name"].AsString;
                                                        if (name == null || name == "")
                                                        {
                                                            name = id.ToString() + " #" + Info.PathId.ToString();
                                                        }
                                                    }
                                                    LoadingAssetText.text = "Loading Asset:- " + name;
                                                    // #Transform

                                                    AssetTypeValueField data = BaseField["m_Component.Array"][0];
                                                    AssetPPtr pptr = AssetPPtr.FromField(data["component"]);


                                                    AssetTypeValueField Transform = new AssetTypeValueField();

                                                    if (pptr.FileId == 0)
                                                    {
                                                        Transform = manager.GetBaseField(afileInst, pptr.PathId);
                                                    }
                                                    AssetPPtr pptr2 = AssetPPtr.FromField(Transform["m_Father"]);

                                                    if (pptr2.FileId == 0 && pptr2.PathId == 0)
                                                    {

                                                        ContactInfo contactInfo = new ContactInfo();

                                                        contactInfo.Name = name;

                                                        contactInfo.Type = SetClassIDIdentifier(AssetClassID.Special);

                                                        contactInfo.parentPath = assetsFileName;

                                                        contactInfo.BundlePath = path;

                                                        contactInfo.pathID = Info.PathId;

                                                        contactInfo.refPath = "/" + contactInfo.Name;

                                                        contactInfo.UnknownAsset = false;

                                                        contactInfo.TypeID = id;

                                                        GameObject Entity = new GameObject(contactInfo.Name);
                                                        Entity.AddComponent<TypeFieldCell>();


                                                        Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(contactInfo);

                                                        Entity.transform.SetParent(GEntity);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {

                                                    if (Inform == "")
                                                    {
                                                        Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                    else
                                                    {
                                                        Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                }
                                                yield return null;
                                            }

                                            {
                                                AssetClassID ids = new AssetClassID();
                                                ids = (AssetClassID)Info.TypeId;
                                                if (!UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed").Find(ids.ToString()))
                                                {
                                                    GameObject Entity = new GameObject(ids.ToString());
                                                    Entity.AddComponent<TypeFieldCell>();

                                                    ContactInfo Infos = new ContactInfo();
                                                    Infos.Name = Entity.name;
                                                    Infos.Type = identifiers[54];
                                                    Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Infos);

                                                    Entity.transform.SetParent(UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed"));
                                                }


                                                Transform GEntity = UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed").Find(ids.ToString());


                                                AssetTypeValueField BaseField = new AssetTypeValueField();

                                                string Name = "F";
                                                try
                                                {
                                                    BaseField = manager.GetBaseField(afileInst, Info);

                                                    if (BaseField["m_Name"].IsDummy == true)
                                                    {
                                                        AssetClassID id = (AssetClassID)Info.TypeId;
                                                        Name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                    else
                                                    {
                                                        Name = BaseField["m_Name"].AsString;
                                                        if (Name == null || Name == "")
                                                        {
                                                            AssetClassID id = (AssetClassID)Info.TypeId;
                                                            Name = id.ToString() + " #" + Info.PathId.ToString();
                                                        }
                                                    }
                                                    LoadingAssetText.text = "Loading Asset:- " + Name;
                                                    Debug.Log(name);

                                                    ContactInfo info = new ContactInfo();
                                                    if (Info.TypeId == (int)AssetClassID.MonoScript)
                                                    {
                                                        info.Name = Name + ".cs";
                                                        info.refPath = "/" + Name + ".cs";
                                                    }
                                                    else
                                                    {
                                                        info.Name = Name;
                                                        info.refPath = "/" + Name;
                                                    }
                                                    info.parentPath = path;
                                                    info.BundlePath = "";
                                                    info.pathID = Info.PathId;
                                                    info.Type = SetClassIDIdentifier((AssetClassID)Info.TypeId);
                                                    info.TypeID = (AssetClassID)Info.TypeId;
                                                    info.UnknownAsset = false;
                                                    //info.FileID = afileInst.file.

                                                    GameObject Entity = new GameObject(info.Name);
                                                    Entity.AddComponent<TypeFieldCell>();


                                                    Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(info);

                                                    Entity.transform.SetParent(GEntity);
                                                }
                                                catch (Exception ex)
                                                {

                                                    if (Inform == "")
                                                    {
                                                        Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                    else
                                                    {
                                                        Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                }
                                                yield return null;
                                            }
                                        }
                                    }
                                    if (levelName != "")
                                    {
                                        {
                                            {
                                                var afileInst = manager.LoadAssetsFileFromBundle(bunfileInst, levelName, false);

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

                                                foreach (var Info in afile.Metadata.AssetInfos)
                                                {
                                                    yield return null;
                                                    if (Info.TypeId == (int)AssetClassID.GameObject)
                                                    {
                                                        Transform GEntity = UAEFileRoot.Find("Bundles").Find("Scenes").Find(levelName + ".unity3d");


                                                        string name = "F";
                                                        AssetTypeValueField BaseField = new AssetTypeValueField();
                                                        try
                                                        {
                                                            BaseField = manager.GetBaseField(afileInst, Info);

                                                            AssetClassID id = new AssetClassID();
                                                            id = (AssetClassID)Info.TypeId;

                                                            if (BaseField["m_Name"].IsDummy == true)
                                                            {
                                                                name = id.ToString() + " #" + Info.PathId.ToString();
                                                            }
                                                            else
                                                            {
                                                                name = BaseField["m_Name"].AsString;
                                                                if (name == null || name == "")
                                                                {
                                                                    name = id.ToString() + " #" + Info.PathId.ToString();
                                                                }
                                                            }
                                                            LoadingAssetText.text = "Loading Asset:- " + name;
                                                            // #Transform

                                                            AssetTypeValueField data = BaseField["m_Component.Array"][0];
                                                            AssetPPtr pptr = AssetPPtr.FromField(data["component"]);


                                                            AssetTypeValueField Transform = new AssetTypeValueField();

                                                            if (pptr.FileId == 0)
                                                            {
                                                                Transform = manager.GetBaseField(afileInst, pptr.PathId);
                                                            }
                                                            AssetPPtr pptr2 = AssetPPtr.FromField(Transform["m_Father"]);

                                                            if (pptr2.FileId == 0 && pptr2.PathId == 0)
                                                            {

                                                                ContactInfo contactInfo = new ContactInfo();

                                                                contactInfo.Name = name;

                                                                contactInfo.Type = SetClassIDIdentifier(id);

                                                                contactInfo.parentPath = levelName;

                                                                contactInfo.BundlePath = path;

                                                                contactInfo.pathID = Info.PathId;

                                                                contactInfo.refPath = "/" + contactInfo.Name;

                                                                contactInfo.UnknownAsset = false;

                                                                contactInfo.TypeID = id;


                                                                GameObject Entity = new GameObject(contactInfo.Name);
                                                                Entity.AddComponent<TypeFieldCell>();


                                                                Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(contactInfo);

                                                                Entity.transform.SetParent(GEntity);
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {

                                                            if (Inform == "")
                                                            {
                                                                Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                                "\nThe PathID on which it fails:- " + Info.PathId +
                                                                "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                                "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                            }
                                                            else
                                                            {
                                                                Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                                "\nThe PathID on which it fails:- " + Info.PathId +
                                                                "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                                "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                            }
                                                        }
                                                        yield return null;
                                                    }

                                                    {
                                                        AssetClassID ids = new AssetClassID();
                                                        ids = (AssetClassID)Info.TypeId;
                                                        if (!UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed").Find(ids.ToString()))
                                                        {
                                                            GameObject Entity = new GameObject(ids.ToString());
                                                            Entity.AddComponent<TypeFieldCell>();

                                                            ContactInfo Infos = new ContactInfo();
                                                            Infos.Name = Entity.name;
                                                            Infos.Type = identifiers[54];
                                                            Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Infos);

                                                            Entity.transform.SetParent(UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed"));
                                                        }
                                                        Transform GEntity = UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed").Find(ids.ToString());

                                                        AssetTypeValueField BaseField = new AssetTypeValueField();

                                                        string Name = "F";
                                                        try
                                                        {
                                                            BaseField = manager.GetBaseField(afileInst, Info);

                                                            if (BaseField["m_Name"].IsDummy == true)
                                                            {
                                                                AssetClassID id = (AssetClassID)Info.TypeId;
                                                                Name = id.ToString() + " #" + Info.PathId.ToString();
                                                            }
                                                            else
                                                            {
                                                                Name = BaseField["m_Name"].AsString;
                                                                if (Name == null || Name == "")
                                                                {
                                                                    AssetClassID id = (AssetClassID)Info.TypeId;
                                                                    Name = id.ToString() + " #" + Info.PathId.ToString();
                                                                }
                                                            }
                                                            LoadingAssetText.text = "Loading Asset:- " + Name;
                                                            Debug.Log(name);

                                                            ContactInfo info = new ContactInfo();
                                                            if (Info.TypeId == (int)AssetClassID.MonoScript)
                                                            {
                                                                info.Name = Name + ".cs";
                                                                info.refPath = "/" + Name + ".cs";
                                                            }
                                                            else
                                                            {
                                                                info.Name = Name;
                                                                info.refPath = "/" + Name;
                                                            }
                                                            info.parentPath = levelName;
                                                            info.BundlePath = path;
                                                            info.pathID = Info.PathId;
                                                            info.Type = SetClassIDIdentifier((AssetClassID)Info.TypeId);
                                                            info.TypeID = (AssetClassID)Info.TypeId;
                                                            info.UnknownAsset = false;
                                                            //info.FileID = afileInst.file.


                                                            GameObject Entity = new GameObject(info.Name);
                                                            Entity.AddComponent<TypeFieldCell>();


                                                            Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(info);

                                                            Entity.transform.SetParent(GEntity);
                                                        }
                                                        catch (Exception ex)
                                                        {

                                                            if (Inform == "")
                                                            {
                                                                Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                                "\nThe PathID on which it fails:- " + Info.PathId +
                                                                "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                                "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                            }
                                                            else
                                                            {
                                                                Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                                "\nThe PathID on which it fails:- " + Info.PathId +
                                                                "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                                "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                            }
                                                        }
                                                        yield return null;
                                                    }
                                                }
                                            }
                                        }
                                        levelName = "";
                                    }
                                    if (assetsFileName.EndsWith(".sharedAssets"))
                                    {
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

                                            foreach (var Info in afile.Metadata.AssetInfos)
                                            {
                                                yield return null;
                                                if (Info.TypeId == (int)AssetClassID.GameObject)
                                                {
                                                    Transform GEntity = UAEFileRoot.Find("Bundles").Find("Scenes").Find(assetsFileName + ".unity3d");

                                                    string name = "F";
                                                    AssetTypeValueField BaseField = new AssetTypeValueField();
                                                    try
                                                    {
                                                        BaseField = manager.GetBaseField(afileInst, Info);

                                                        AssetClassID id = new AssetClassID();
                                                        id = (AssetClassID)Info.TypeId;

                                                        if (BaseField["m_Name"].IsDummy == true)
                                                        {
                                                            name = id.ToString() + " #" + Info.PathId.ToString();
                                                        }
                                                        else
                                                        {
                                                            name = BaseField["m_Name"].AsString;
                                                            if (name == null || name == "")
                                                            {
                                                                name = id.ToString() + " #" + Info.PathId.ToString();
                                                            }
                                                        }
                                                        LoadingAssetText.text = "Loading Asset:- " + name;
                                                        // #Transform

                                                        AssetTypeValueField data = BaseField["m_Component.Array"][0];
                                                        AssetPPtr pptr = AssetPPtr.FromField(data["component"]);


                                                        AssetTypeValueField Transform = new AssetTypeValueField();

                                                        if (pptr.FileId == 0)
                                                        {
                                                            Transform = manager.GetBaseField(afileInst, pptr.PathId);
                                                        }
                                                        AssetPPtr pptr2 = AssetPPtr.FromField(Transform["m_Father"]);

                                                        if (pptr2.FileId == 0 && pptr2.PathId == 0)
                                                        {

                                                            ContactInfo contactInfo = new ContactInfo();

                                                            contactInfo.Name = name;

                                                            contactInfo.Type = SetClassIDIdentifier(id);

                                                            contactInfo.parentPath = assetsFileName;

                                                            contactInfo.BundlePath = path;

                                                            contactInfo.pathID = Info.PathId;

                                                            contactInfo.refPath = "/" + contactInfo.Name;

                                                            contactInfo.UnknownAsset = false;

                                                            contactInfo.TypeID = id;

                                                            GameObject Entity = new GameObject(contactInfo.Name);
                                                            Entity.AddComponent<TypeFieldCell>();


                                                            Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(contactInfo);

                                                            Entity.transform.SetParent(GEntity);
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                        if (Inform == "")
                                                        {
                                                            Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                            "\nThe PathID on which it fails:- " + Info.PathId +
                                                            "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                            "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                        }
                                                        else
                                                        {
                                                            Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                            "\nThe PathID on which it fails:- " + Info.PathId +
                                                            "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                            "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                        }
                                                    }
                                                    yield return null;
                                                }

                                                //Managed
                                                {
                                                    AssetClassID ids = new AssetClassID();
                                                    ids = (AssetClassID)Info.TypeId;
                                                    if (!UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed").Find(ids.ToString()))
                                                    {
                                                        GameObject Entity = new GameObject(ids.ToString());
                                                        Entity.AddComponent<TypeFieldCell>();

                                                        ContactInfo Infos = new ContactInfo();
                                                        Infos.Name = Entity.name;
                                                        Infos.Type = identifiers[54];
                                                        Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Infos);

                                                        Entity.transform.SetParent(UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed"));
                                                    }
                                                    Transform GEntity = UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed").Find(ids.ToString());


                                                    AssetTypeValueField BaseField = new AssetTypeValueField();

                                                    string Name = "F";
                                                    try
                                                    {
                                                        BaseField = manager.GetBaseField(afileInst, Info);

                                                        if (BaseField["m_Name"].IsDummy == true)
                                                        {
                                                            AssetClassID id = (AssetClassID)Info.TypeId;
                                                            Name = id.ToString() + " #" + Info.PathId.ToString();
                                                        }
                                                        else
                                                        {
                                                            Name = BaseField["m_Name"].AsString;
                                                            if (Name == null || Name == "")
                                                            {
                                                                AssetClassID id = (AssetClassID)Info.TypeId;
                                                                Name = id.ToString() + " #" + Info.PathId.ToString();
                                                            }
                                                        }
                                                        LoadingAssetText.text = "Loading Asset:- " + Name;
                                                        Debug.Log(name);

                                                        ContactInfo info = new ContactInfo();
                                                        if (Info.TypeId == (int)AssetClassID.MonoScript)
                                                        {
                                                            info.Name = Name + ".cs";
                                                            info.refPath = "/" + Name + ".cs";
                                                        }
                                                        else
                                                        {
                                                            info.Name = Name;
                                                            info.refPath = "/" + Name;
                                                        }
                                                        info.parentPath = assetsFileName;
                                                        info.BundlePath = path;
                                                        info.pathID = Info.PathId;
                                                        info.Type = SetClassIDIdentifier((AssetClassID)Info.TypeId);
                                                        info.TypeID = (AssetClassID)Info.TypeId;
                                                        info.UnknownAsset = false;
                                                        //info.FileID = afileInst.file.


                                                        GameObject Entity = new GameObject(info.Name);
                                                        Entity.AddComponent<TypeFieldCell>();


                                                        Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(info);

                                                        Entity.transform.SetParent(GEntity);
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                        if (Inform == "")
                                                        {
                                                            Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                            "\nThe PathID on which it fails:- " + Info.PathId +
                                                            "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                            "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                        }
                                                        else
                                                        {
                                                            Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                            "\nThe PathID on which it fails:- " + Info.PathId +
                                                            "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                            "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                        }
                                                    }
                                                    yield return null;
                                                }
                                            }
                                        }
                                    }
                                    if (assetsFileName.Contains("sharedassets") && assetsFileName.EndsWith(".assets"))
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

                                        foreach (var Info in afile.Metadata.AssetInfos)
                                        {
                                            yield return null;
                                            if (Info.TypeId == (int)AssetClassID.GameObject)
                                            {
                                                Transform GEntity = UAEFileRoot.Find("Bundles").Find("Scenes").Find(assetsFileName + ".unity3d");

                                                string name = "F";
                                                AssetTypeValueField BaseField = new AssetTypeValueField();
                                                try
                                                {
                                                    BaseField = manager.GetBaseField(afileInst, Info);

                                                    AssetClassID id = new AssetClassID();
                                                    id = (AssetClassID)Info.TypeId;

                                                    if (BaseField["m_Name"].IsDummy == true)
                                                    {
                                                        name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                    else
                                                    {
                                                        name = BaseField["m_Name"].AsString;
                                                        if (name == null || name == "")
                                                        {
                                                            name = id.ToString() + " #" + Info.PathId.ToString();
                                                        }
                                                    }
                                                    LoadingAssetText.text = "Loading Asset:- " + name;
                                                    // #Transform

                                                    AssetTypeValueField data = BaseField["m_Component.Array"][0];
                                                    AssetPPtr pptr = AssetPPtr.FromField(data["component"]);


                                                    AssetTypeValueField Transform = new AssetTypeValueField();

                                                    if (pptr.FileId == 0)
                                                    {
                                                        Transform = manager.GetBaseField(afileInst, pptr.PathId);
                                                    }
                                                    AssetPPtr pptr2 = AssetPPtr.FromField(Transform["m_Father"]);

                                                    if (pptr2.FileId == 0 && pptr2.PathId == 0)
                                                    {

                                                        ContactInfo contactInfo = new ContactInfo();

                                                        contactInfo.Name = name;

                                                        contactInfo.Type = SetClassIDIdentifier(id);

                                                        contactInfo.parentPath = assetsFileName;

                                                        contactInfo.BundlePath = path;

                                                        contactInfo.pathID = Info.PathId;

                                                        contactInfo.refPath = "/" + contactInfo.Name;

                                                        contactInfo.UnknownAsset = false;

                                                        contactInfo.TypeID = id;

                                                        GameObject Entity = new GameObject(contactInfo.Name);
                                                        Entity.AddComponent<TypeFieldCell>();


                                                        Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(contactInfo);

                                                        Entity.transform.SetParent(GEntity);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {

                                                    if (Inform == "")
                                                    {
                                                        Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                    else
                                                    {
                                                        Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                }
                                                yield return null;
                                            }

                                            //Managed
                                            {
                                                AssetClassID ids = new AssetClassID();
                                                ids = (AssetClassID)Info.TypeId;
                                                if (!UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed").Find(ids.ToString()))
                                                {
                                                    GameObject Entity = new GameObject(ids.ToString());
                                                    Entity.AddComponent<TypeFieldCell>();

                                                    ContactInfo Infos = new ContactInfo();
                                                    Infos.Name = Entity.name;
                                                    Infos.Type = identifiers[54];
                                                    Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Infos);

                                                    Entity.transform.SetParent(UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed"));
                                                }
                                                Transform GEntity = UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed").Find(ids.ToString());

                                                AssetTypeValueField BaseField = new AssetTypeValueField();

                                                string Name = "F";
                                                try
                                                {
                                                    BaseField = manager.GetBaseField(afileInst, Info);

                                                    if (BaseField["m_Name"].IsDummy == true)
                                                    {
                                                        AssetClassID id = (AssetClassID)Info.TypeId;
                                                        Name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                    else
                                                    {
                                                        Name = BaseField["m_Name"].AsString;
                                                        if (Name == null || Name == "")
                                                        {
                                                            AssetClassID id = (AssetClassID)Info.TypeId;
                                                            Name = id.ToString() + " #" + Info.PathId.ToString();
                                                        }
                                                    }
                                                    LoadingAssetText.text = "Loading Asset:- " + Name;
                                                    Debug.Log(name);

                                                    ContactInfo info = new ContactInfo();
                                                    if (Info.TypeId == (int)AssetClassID.MonoScript)
                                                    {
                                                        info.Name = Name + ".cs";
                                                        info.refPath = "/" + Name + ".cs";
                                                    }
                                                    else
                                                    {
                                                        info.Name = Name;
                                                        info.refPath = "/" + Name;
                                                    }
                                                    info.parentPath = assetsFileName;
                                                    info.BundlePath = path;
                                                    info.pathID = Info.PathId;
                                                    info.Type = SetClassIDIdentifier((AssetClassID)Info.TypeId);
                                                    info.TypeID = (AssetClassID)Info.TypeId;
                                                    info.UnknownAsset = false;
                                                    //info.FileID = afileInst.file.

                                                    GameObject Entity = new GameObject(info.Name);
                                                    Entity.AddComponent<TypeFieldCell>();


                                                    Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(info);

                                                    Entity.transform.SetParent(GEntity);
                                                }
                                                catch (Exception ex)
                                                {

                                                    if (Inform == "")
                                                    {
                                                        Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                    else
                                                    {
                                                        Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                }
                                                yield return null;
                                            }
                                        }
                                    }
                                }
                                levelName = "";
                                manager.UnloadAll();
                            }
                        }
                        count = count + 1;
                    }
                    else
                    {
                        if (result.Length == 1)
                        {
                            DefameData();
                            ComplainError("Selected File is neither a Bundle nor an Asset file.\nPlease make sure you use the correct file.");
                        }
                    }
                }
            }
            if (count == 0)
            {
                DefameData();
                ComplainError("Selected Files and Binaries are neither a Bundle nor an Asset file.\nPlease make sure you use the correct files.");
            }

            if (Inform == "" && count != 0)
            {
                ComplainError("Files Loaded Successfully....");
            }
            else if (count != 0)
            {
                ComplainError(Inform);
            }
            LoadingBar.SetActive(false);
            InitializeData();
        }
    }
    public void LoadNET(string[] paths)
    {
        inAccessible.SetActive(false);
        StartCoroutine(LoadNETCoroutine(paths));
    }
    IEnumerator LoadNETCoroutine(string[] paths)
    {
        this.result = paths;

        if (paths.Length != 0)
        {
            int count = 0;

            LoadingBar.SetActive(true);
            string Inform = "";

            for (int i = 0; i < result.Length; i++)
            {
                yield return null;
                LoadingText.text = "Loading File:- " + Path.GetFileName(result[i]);
                Debug.Log(Path.GetFileName(result[i]));
                {
                    DetectedFileType FileTypes = FileTypeDetector.DetectFileType(result[i]);
                    if (FileTypes == DetectedFileType.AssetsFile)
                    {
                        if (!UAEFileRoot.Find("AssetsFiles"))
                        {
                            GameObject AssetsFiles = new GameObject("AssetsFiles");
                            AssetsFiles.AddComponent<TypeFieldCell>();

                            ContactInfo Info = new ContactInfo();
                            Info.Name = AssetsFiles.name;
                            Info.Type = identifiers[54];
                            AssetsFiles.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                            AssetsFiles.transform.SetParent(UAEFileRoot);
                        }

                        if (!UAEFileRoot.Find("AssetsFiles").Find("Managed"))
                        {
                            GameObject Managed = new GameObject("Managed");
                            Managed.AddComponent<TypeFieldCell>();

                            ContactInfo Info = new ContactInfo();
                            Info.Name = Managed.name;
                            Info.Type = identifiers[54];
                            Managed.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                            Managed.transform.SetParent(UAEFileRoot.Find("AssetsFiles"));
                        }

                        if (!UAEFileRoot.Find("AssetsFiles").Find("UnManaged"))
                        {
                            GameObject UnManaged = new GameObject("UnManaged");
                            UnManaged.AddComponent<TypeFieldCell>();

                            ContactInfo Info = new ContactInfo();
                            Info.Name = UnManaged.name;
                            Info.Type = identifiers[54];
                            UnManaged.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                            UnManaged.transform.SetParent(UAEFileRoot.Find("AssetsFiles"));
                        }

                        {
                            string path = result[i];
                            {
                                var manager = new AssetsManager();

                                manager.LoadClassPackage(Path.Combine(Application.persistentDataPath, "classdata.tpk"));

                                var afileInst = manager.LoadAssetsFile(path, false);
                                var afile = afileInst.file;

                                afileInst.file.GenerateQuickLookup();

                                if (StaticSettings.unityMetadata != "")
                                {
                                    manager.LoadClassDatabaseFromPackage(StaticSettings.unityMetadata);
                                }
                                else
                                {
                                    manager.LoadClassDatabaseFromPackage(afile.Metadata.UnityVersion);
                                }

                                string assetsFileName = afileInst.name;

                                if (assetsFileName.Contains("level"))
                                {

                                    ///Creating a Scene Folder
                                    if (!UAEFileRoot.Find("AssetsFiles").Find("Scenes"))
                                    {
                                        Transform AssetsFiles = UAEFileRoot.Find("AssetsFiles");
                                        GameObject Scenes = new GameObject("Scenes");
                                        Scenes.AddComponent<TypeFieldCell>();

                                        ContactInfo Info = new ContactInfo();
                                        Info.Name = Scenes.name;
                                        Info.Type = identifiers[54];
                                        Scenes.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                        Scenes.transform.SetParent(AssetsFiles);
                                    }//End;

                                    //Getting the Scene Folder;
                                    Transform ScenesFolder = UAEFileRoot.Find("AssetsFiles").Find("Scenes");


                                    ///Adding Managed to Scene
                                    if (!UAEFileRoot.Find("AssetsFiles").Find("Scenes").Find("Managed"))
                                    {
                                        GameObject Managed = new GameObject("Managed");
                                        Managed.AddComponent<TypeFieldCell>();

                                        ContactInfo Info = new ContactInfo();
                                        Info.Name = Managed.name;
                                        Info.Type = identifiers[54];
                                        Managed.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                        Managed.transform.SetParent(ScenesFolder);
                                    }//End;

                                    ///Adding The Scene
                                    {
                                        GameObject level = new GameObject(assetsFileName + ".unity3d");
                                        level.AddComponent<TypeFieldCell>();

                                        ContactInfo Info = new ContactInfo();
                                        Info.Name = assetsFileName;
                                        Info.Type = identifiers[50];
                                        level.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                        level.transform.SetParent(ScenesFolder);
                                    }//End;
                                }
                                if (assetsFileName.Contains("sharedAssets"))
                                {
                                    ///Creating a Scene Folder
                                    if (!UAEFileRoot.Find("AssetsFiles").Find("Scenes"))
                                    {
                                        Transform AssetsFiles = UAEFileRoot.Find("AssetsFiles");
                                        GameObject Scenes = new GameObject("Scenes");
                                        Scenes.AddComponent<TypeFieldCell>();

                                        ContactInfo Info = new ContactInfo();
                                        Info.Name = Scenes.name;
                                        Info.Type = identifiers[54];
                                        Scenes.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                        Scenes.transform.SetParent(AssetsFiles);
                                    }//End;

                                    //Getting the Scene Folder;
                                    Transform ScenesFolder = UAEFileRoot.Find("AssetsFiles").Find("Scenes");


                                    ///Adding Managed to Scene
                                    if (!UAEFileRoot.Find("AssetsFiles").Find("Scenes").Find("Managed"))
                                    {
                                        GameObject Managed = new GameObject("Managed");
                                        Managed.AddComponent<TypeFieldCell>();

                                        ContactInfo Info = new ContactInfo();
                                        Info.Name = Managed.name;
                                        Info.Type = identifiers[54];
                                        Managed.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                        Managed.transform.SetParent(ScenesFolder);
                                    }//End;

                                    ///Adding the SharedScene
                                    {
                                        GameObject level = new GameObject(assetsFileName + ".unity3d");
                                        level.AddComponent<TypeFieldCell>();

                                        ContactInfo Info = new ContactInfo();
                                        Info.Name = assetsFileName;
                                        Info.Type = identifiers[50];
                                        level.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                        level.transform.SetParent(ScenesFolder);
                                    }//End;
                                }

                                if (!afileInst.name.Contains("level") && !afileInst.name.Contains("sharedassets"))
                                {
                                    foreach (var Info in afile.Metadata.AssetInfos)
                                    {
                                        yield return null;
                                        if (Info.TypeId == (int)AssetClassID.GameObject)
                                        {
                                            if (!UAEFileRoot.Find("AssetsFiles").Find("G'O RX"))
                                            {
                                                GameObject GoRX = new GameObject("G'O RX");
                                                GoRX.AddComponent<TypeFieldCell>();

                                                ContactInfo Infos = new ContactInfo();
                                                Infos.Name = GoRX.name;
                                                Infos.Type = identifiers[54];
                                                GoRX.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Infos);

                                                GoRX.transform.SetParent(UAEFileRoot.Find("AssetsFiles"));
                                            }

                                            Transform GORX = UAEFileRoot.Find("AssetsFiles").Find("G'O RX");

                                            string name = "F";
                                            AssetTypeValueField BaseField = new AssetTypeValueField();
                                            try
                                            {
                                                BaseField = manager.GetBaseField(afileInst, Info);

                                                AssetClassID id = new AssetClassID();
                                                id = (AssetClassID)Info.TypeId;

                                                if (BaseField["m_Name"].IsDummy == true)
                                                {
                                                    name = id.ToString() + " #" + Info.PathId.ToString();
                                                }
                                                else
                                                {
                                                    name = BaseField["m_Name"].AsString;
                                                    if (name == null || name == "")
                                                    {
                                                        name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                }
                                                LoadingAssetText.text = "Loading Asset:- " + name;

                                                // #Transform

                                                AssetTypeValueField data = BaseField["m_Component.Array"][0];
                                                AssetPPtr pptr = AssetPPtr.FromField(data["component"]);


                                                AssetTypeValueField Transform = new AssetTypeValueField();

                                                if (pptr.FileId == 0)
                                                {
                                                    Transform = manager.GetBaseField(afileInst, pptr.PathId);
                                                }
                                                AssetPPtr pptr2 = AssetPPtr.FromField(Transform["m_Father"]);

                                                if (pptr2.FileId == 0 && pptr2.PathId == 0)
                                                {


                                                    ContactInfo contactInfo = new ContactInfo();

                                                    contactInfo.Name = name;

                                                    contactInfo.Type = SetClassIDIdentifier(id);

                                                    contactInfo.parentPath = path;

                                                    contactInfo.BundlePath = "";

                                                    contactInfo.pathID = Info.PathId;

                                                    contactInfo.refPath = "/" + contactInfo.Name;

                                                    contactInfo.UnknownAsset = false;

                                                    contactInfo.TypeID = id;

                                                    GameObject assetObject = new GameObject(contactInfo.Name);
                                                    assetObject.AddComponent<TypeFieldCell>();

                                                    assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(contactInfo);

                                                    assetObject.transform.SetParent(GORX);

                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                if (Inform == "")
                                                {
                                                    Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                                else
                                                {
                                                    Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                            }
                                            yield return null;
                                        }


                                        ///Managed Section
                                        {
                                            AssetClassID ids = new AssetClassID();
                                            ids = (AssetClassID)Info.TypeId;
                                            if (!UAEFileRoot.Find("AssetsFiles").Find("Managed").Find(ids.ToString()))
                                            {
                                                GameObject Entity = new GameObject(ids.ToString());
                                                Entity.AddComponent<TypeFieldCell>();

                                                ContactInfo Infos = new ContactInfo();
                                                Infos.Name = Entity.name;
                                                Infos.Type = identifiers[54];
                                                Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Infos);

                                                Entity.transform.SetParent(UAEFileRoot.Find("AssetsFiles").Find("Managed"));
                                            }

                                            Transform GEntity = UAEFileRoot.Find("AssetsFiles").Find("Managed").Find(ids.ToString());

                                            AssetTypeValueField BaseField = new AssetTypeValueField();

                                            string Name = "F";
                                            try
                                            {
                                                BaseField = manager.GetBaseField(afileInst, Info);


                                                if (BaseField["m_Name"].IsDummy == true)
                                                {
                                                    AssetClassID id = (AssetClassID)Info.TypeId;
                                                    Name = id.ToString() + " #" + Info.PathId.ToString();
                                                }
                                                else
                                                {
                                                    Name = BaseField["m_Name"].AsString;
                                                    if (Name == null || Name == "")
                                                    {
                                                        AssetClassID id = (AssetClassID)Info.TypeId;
                                                        Name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                }
                                                LoadingAssetText.text = "Loading Asset:- " + Name;
                                                Debug.Log(name);


                                                ContactInfo info = new ContactInfo();
                                                if (Info.TypeId == (int)AssetClassID.MonoScript)
                                                {
                                                    info.Name = Name + ".cs";
                                                    info.refPath = "/" + Name + ".cs";
                                                }
                                                else
                                                {
                                                    info.Name = Name;
                                                    info.refPath = "/" + Name;
                                                }
                                                info.parentPath = path;
                                                info.BundlePath = "";
                                                info.pathID = Info.PathId;
                                                info.Type = SetClassIDIdentifier((AssetClassID)Info.TypeId);
                                                info.TypeID = (AssetClassID)Info.TypeId;
                                                info.UnknownAsset = false;


                                                GameObject assetObject = new GameObject(info.Name);
                                                assetObject.AddComponent<TypeFieldCell>();

                                                assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(info);

                                                assetObject.transform.SetParent(GEntity);
                                                //info.FileID = afileInst.file.
                                            }
                                            catch (Exception ex)
                                            {
                                                if (Inform == "")
                                                {
                                                    Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                                else
                                                {
                                                    Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                            }
                                            yield return null;

                                        }//End;

                                        ///UnManaged Section
                                        {
                                            Transform GEntity = UAEFileRoot.Find("AssetsFiles").Find("UnManaged");

                                            AssetTypeValueField BaseField = new AssetTypeValueField();

                                            string Name = "F";
                                            try
                                            {
                                                BaseField = manager.GetBaseField(afileInst, Info);


                                                if (BaseField["m_Name"].IsDummy == true)
                                                {
                                                    AssetClassID id = (AssetClassID)Info.TypeId;
                                                    Name = id.ToString() + " #" + Info.PathId.ToString();
                                                }
                                                else
                                                {
                                                    Name = BaseField["m_Name"].AsString;
                                                    if (Name == null || Name == "")
                                                    {
                                                        AssetClassID id = (AssetClassID)Info.TypeId;
                                                        Name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                }
                                                LoadingAssetText.text = "Loading Asset:- " + Name;
                                                Debug.Log(name);


                                                ContactInfo info = new ContactInfo();
                                                if (Info.TypeId == (int)AssetClassID.MonoScript)
                                                {
                                                    info.Name = Name + ".cs";
                                                    info.refPath = "/" + Name + ".cs";
                                                }
                                                else
                                                {
                                                    info.Name = Name;
                                                    info.refPath = "/" + Name;
                                                }
                                                info.parentPath = path;
                                                info.BundlePath = "";
                                                info.pathID = Info.PathId;
                                                info.Type = SetClassIDIdentifier((AssetClassID)Info.TypeId);
                                                info.TypeID = (AssetClassID)Info.TypeId;
                                                info.UnknownAsset = false;




                                                GameObject assetObject = new GameObject(info.Name);
                                                assetObject.AddComponent<TypeFieldCell>();

                                                assetObject.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(info);

                                                assetObject.transform.SetParent(GEntity);
                                                //info.FileID = afileInst.file.
                                            }
                                            catch (Exception ex)
                                            {
                                                if (Inform == "")
                                                {
                                                    Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                                else
                                                {
                                                    Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                            }
                                            yield return null;

                                        }//End;
                                    }
                                }
                                else if (afileInst.name.Contains("level"))
                                {
                                    foreach (var Info in afile.Metadata.AssetInfos)
                                    {
                                        if (Info.TypeId == (int)AssetClassID.GameObject)
                                        {
                                            Transform GEntity = UAEFileRoot.Find("AssetsFiles").Find("Scenes").Find(assetsFileName + ".unity3d");

                                            string name = "F";
                                            AssetTypeValueField BaseField = new AssetTypeValueField();
                                            try
                                            {

                                                BaseField = manager.GetBaseField(afileInst, Info);

                                                AssetClassID id = new AssetClassID();
                                                id = (AssetClassID)Info.TypeId;

                                                if (BaseField["m_Name"].IsDummy == true)
                                                {
                                                    name = id.ToString() + " #" + Info.PathId.ToString();
                                                }
                                                else
                                                {
                                                    name = BaseField["m_Name"].AsString;
                                                    if (name == null || name == "")
                                                    {
                                                        name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                }

                                                LoadingAssetText.text = "Loading Asset:- " + name;
                                                // #Transform

                                                AssetTypeValueField data = BaseField["m_Component.Array"][0];
                                                AssetPPtr pptr = AssetPPtr.FromField(data["component"]);


                                                AssetTypeValueField Transform = new AssetTypeValueField();

                                                if (pptr.FileId == 0)
                                                {
                                                    Transform = manager.GetBaseField(afileInst, pptr.PathId);
                                                }
                                                AssetPPtr pptr2 = AssetPPtr.FromField(Transform["m_Father"]);

                                                if (pptr2.FileId == 0 && pptr2.PathId == 0)
                                                {

                                                    ContactInfo contactInfo = new ContactInfo();

                                                    contactInfo.Name = name;

                                                    contactInfo.Type = SetClassIDIdentifier(AssetClassID.Special);

                                                    contactInfo.parentPath = path;

                                                    contactInfo.BundlePath = "";

                                                    contactInfo.pathID = Info.PathId;

                                                    contactInfo.refPath = "/" + contactInfo.Name;

                                                    contactInfo.UnknownAsset = false;

                                                    contactInfo.TypeID = id;

                                                    GameObject Entity = new GameObject(contactInfo.Name);
                                                    Entity.AddComponent<TypeFieldCell>();


                                                    Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(contactInfo);

                                                    Entity.transform.SetParent(GEntity);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                if (Inform == "")
                                                {
                                                    Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                                else
                                                {
                                                    Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                            }
                                            yield return null;
                                        }

                                        {
                                            AssetClassID ids = new AssetClassID();
                                            ids = (AssetClassID)Info.TypeId;
                                            if (!UAEFileRoot.Find("AssetsFiles").Find("Scenes").Find("Managed").Find(ids.ToString()))
                                            {
                                                GameObject Entity = new GameObject(ids.ToString());
                                                Entity.AddComponent<TypeFieldCell>();

                                                ContactInfo Infos = new ContactInfo();
                                                Infos.Name = Entity.name;
                                                Infos.Type = identifiers[54];
                                                Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Infos);

                                                Entity.transform.SetParent(UAEFileRoot.Find("AssetsFiles").Find("Scenes").Find("Managed"));
                                            }


                                            Transform GEntity = UAEFileRoot.Find("AssetsFiles").Find("Scenes").Find("Managed").Find(ids.ToString());



                                            AssetTypeValueField BaseField = new AssetTypeValueField();

                                            string Name = "F";
                                            try
                                            {
                                                BaseField = manager.GetBaseField(afileInst, Info);

                                                if (BaseField["m_Name"].IsDummy == true)
                                                {
                                                    AssetClassID id = (AssetClassID)Info.TypeId;
                                                    Name = id.ToString() + " #" + Info.PathId.ToString();
                                                }
                                                else
                                                {
                                                    Name = BaseField["m_Name"].AsString;
                                                    if (Name == null || Name == "")
                                                    {
                                                        AssetClassID id = (AssetClassID)Info.TypeId;
                                                        Name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                }
                                                LoadingAssetText.text = "Loading Asset:- " + Name;
                                                Debug.Log(name);

                                                ContactInfo info = new ContactInfo();
                                                if (Info.TypeId == (int)AssetClassID.MonoScript)
                                                {
                                                    info.Name = Name + ".cs";
                                                    info.refPath = "/" + Name + ".cs";
                                                }
                                                else
                                                {
                                                    info.Name = Name;
                                                    info.refPath = "/" + Name;
                                                }
                                                info.parentPath = path;
                                                info.BundlePath = "";
                                                info.pathID = Info.PathId;
                                                info.Type = SetClassIDIdentifier((AssetClassID)Info.TypeId);
                                                info.TypeID = (AssetClassID)Info.TypeId;
                                                info.UnknownAsset = false;
                                                //info.FileID = afileInst.file.


                                                GameObject Entity = new GameObject(info.Name);
                                                Entity.AddComponent<TypeFieldCell>();


                                                Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(info);

                                                Entity.transform.SetParent(GEntity);
                                            }
                                            catch (Exception ex)
                                            {
                                                if (Inform == "")
                                                {
                                                    Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                                else
                                                {
                                                    Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                            }
                                            yield return null;
                                        }
                                    }
                                }
                                else if (afileInst.name.Contains("sharedassets"))
                                {
                                    foreach (var Info in afile.Metadata.AssetInfos)
                                    {
                                        if (Info.TypeId == (int)AssetClassID.GameObject)
                                        {
                                            Transform GEntity = UAEFileRoot.Find("AssetsFiles").Find("Scenes").Find(assetsFileName + ".unity3d");

                                            string name = "F";
                                            AssetTypeValueField BaseField = new AssetTypeValueField();
                                            try
                                            {

                                                BaseField = manager.GetBaseField(afileInst, Info);

                                                AssetClassID id = new AssetClassID();
                                                id = (AssetClassID)Info.TypeId;

                                                if (BaseField["m_Name"].IsDummy == true)
                                                {
                                                    name = id.ToString() + " #" + Info.PathId.ToString();
                                                }
                                                else
                                                {
                                                    name = BaseField["m_Name"].AsString;
                                                    if (name == null || name == "")
                                                    {
                                                        name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                }

                                                LoadingAssetText.text = "Loading Asset:- " + name;
                                                // #Transform

                                                AssetTypeValueField data = BaseField["m_Component.Array"][0];
                                                AssetPPtr pptr = AssetPPtr.FromField(data["component"]);


                                                AssetTypeValueField Transform = new AssetTypeValueField();

                                                if (pptr.FileId == 0)
                                                {
                                                    Transform = manager.GetBaseField(afileInst, pptr.PathId);
                                                }
                                                AssetPPtr pptr2 = AssetPPtr.FromField(Transform["m_Father"]);

                                                if (pptr2.FileId == 0 && pptr2.PathId == 0)
                                                {

                                                    ContactInfo contactInfo = new ContactInfo();

                                                    contactInfo.Name = name;

                                                    contactInfo.Type = SetClassIDIdentifier(AssetClassID.Special);

                                                    contactInfo.parentPath = path;

                                                    contactInfo.BundlePath = "";

                                                    contactInfo.pathID = Info.PathId;

                                                    contactInfo.refPath = "/" + contactInfo.Name;

                                                    contactInfo.UnknownAsset = false;

                                                    contactInfo.TypeID = id;

                                                    GameObject Entity = new GameObject(contactInfo.Name);
                                                    Entity.AddComponent<TypeFieldCell>();


                                                    Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(contactInfo);

                                                    Entity.transform.SetParent(GEntity);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                if (Inform == "")
                                                {
                                                    Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                                else
                                                {
                                                    Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                            }
                                            yield return null;
                                        }

                                        {
                                            AssetClassID ids = new AssetClassID();
                                            ids = (AssetClassID)Info.TypeId;
                                            if (!UAEFileRoot.Find("AssetsFiles").Find("Scenes").Find("Managed").Find(ids.ToString()))
                                            {
                                                GameObject Entity = new GameObject(ids.ToString());
                                                Entity.AddComponent<TypeFieldCell>();

                                                ContactInfo Infos = new ContactInfo();
                                                Infos.Name = Entity.name;
                                                Infos.Type = identifiers[54];
                                                Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Infos);

                                                Entity.transform.SetParent(UAEFileRoot.Find("AssetsFiles").Find("Scenes").Find("Managed"));
                                            }


                                            Transform GEntity = UAEFileRoot.Find("AssetsFiles").Find("Scenes").Find("Managed").Find(ids.ToString());



                                            AssetTypeValueField BaseField = new AssetTypeValueField();

                                            string Name = "F";
                                            try
                                            {
                                                BaseField = manager.GetBaseField(afileInst, Info);

                                                if (BaseField["m_Name"].IsDummy == true)
                                                {
                                                    AssetClassID id = (AssetClassID)Info.TypeId;
                                                    Name = id.ToString() + " #" + Info.PathId.ToString();
                                                }
                                                else
                                                {
                                                    Name = BaseField["m_Name"].AsString;
                                                    if (Name == null || Name == "")
                                                    {
                                                        AssetClassID id = (AssetClassID)Info.TypeId;
                                                        Name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                }
                                                LoadingAssetText.text = "Loading Asset:- " + Name;
                                                Debug.Log(name);

                                                ContactInfo info = new ContactInfo();
                                                if (Info.TypeId == (int)AssetClassID.MonoScript)
                                                {
                                                    info.Name = Name + ".cs";
                                                    info.refPath = "/" + Name + ".cs";
                                                }
                                                else
                                                {
                                                    info.Name = Name;
                                                    info.refPath = "/" + Name;
                                                }
                                                info.parentPath = path;
                                                info.BundlePath = "";
                                                info.pathID = Info.PathId;
                                                info.Type = SetClassIDIdentifier((AssetClassID)Info.TypeId);
                                                info.TypeID = (AssetClassID)Info.TypeId;
                                                info.UnknownAsset = false;
                                                //info.FileID = afileInst.file.


                                                GameObject Entity = new GameObject(info.Name);
                                                Entity.AddComponent<TypeFieldCell>();


                                                Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(info);

                                                Entity.transform.SetParent(GEntity);
                                            }
                                            catch (Exception ex)
                                            {
                                                if (Inform == "")
                                                {
                                                    Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                                else
                                                {
                                                    Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                    "\nThe PathID on which it fails:- " + Info.PathId +
                                                    "\nThe File Loaded is:- " + Path.GetFileName(path));
                                                }
                                            }
                                            yield return null;
                                        }
                                    }
                                }
                                manager.UnloadAll();
                            }
                        }
                        count = count + 1;
                    }
                    else if (FileTypes == DetectedFileType.BundleFile)
                    {
                        {
                            if (!UAEFileRoot.Find("Bundles"))
                            {
                                GameObject Bundles = new GameObject("Bundles");
                                Bundles.AddComponent<TypeFieldCell>();

                                ContactInfo Info = new ContactInfo();
                                Info.Name = Bundles.name;
                                Info.Type = identifiers[54];
                                Bundles.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                Bundles.transform.SetParent(UAEFileRoot);
                            }

                            if (!UAEFileRoot.Find("Bundles").Find("Managed"))
                            {
                                GameObject Managed = new GameObject("Managed");
                                Managed.AddComponent<TypeFieldCell>();

                                ContactInfo Info = new ContactInfo();
                                Info.Name = Managed.name;
                                Info.Type = identifiers[54];
                                Managed.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                Managed.transform.SetParent(UAEFileRoot.Find("Bundles"));
                            }

                            if (!UAEFileRoot.Find("Bundles").Find("UnManaged"))
                            {
                                GameObject UnManaged = new GameObject("UnManaged");
                                UnManaged.AddComponent<TypeFieldCell>();

                                ContactInfo Info = new ContactInfo();
                                Info.Name = UnManaged.name;
                                Info.Type = identifiers[54];
                                UnManaged.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);


                                UnManaged.transform.SetParent(UAEFileRoot.Find("Bundles"));
                            }

                            string path = result[i];
                            {
                                var manager = new AssetsManager();
                                manager.LoadClassPackage(Path.Combine(Application.persistentDataPath, "classdata.tpk"));

                                var bunfileInst = manager.LoadBundleFile(path, true);

                                string levelName = "";

                                foreach (AssetBundleDirectoryInfo assetBundleDirectoryInfo in bunfileInst.file.BlockAndDirInfo.DirectoryInfos)
                                {
                                    yield return null;
                                    string assetsFileName = assetBundleDirectoryInfo.Name;

                                    if (assetsFileName.EndsWith(".sharedAssets") && levelName == "")
                                    {
                                        levelName = assetsFileName.Split(".")[0];
                                    }

                                    if (assetsFileName.EndsWith(".sharedAssets"))
                                    {
                                        ///Creating a Scene Folder
                                        if (!UAEFileRoot.Find("Bundles").Find("Scenes"))
                                        {
                                            Transform Bundles = UAEFileRoot.Find("Bundles");
                                            GameObject Scenes = new GameObject("Scenes");
                                            Scenes.AddComponent<TypeFieldCell>();

                                            ContactInfo Info = new ContactInfo();
                                            Info.Name = Bundles.name;
                                            Info.Type = identifiers[54];
                                            Scenes.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                            Scenes.transform.SetParent(Bundles);
                                        }//End;

                                        //Getting the Scene Folder;
                                        Transform ScenesFolder = UAEFileRoot.Find("Bundles").Find("Scenes");


                                        ///Adding Managed to Scene
                                        if (!UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed"))
                                        {
                                            GameObject Managed = new GameObject("Managed");
                                            Managed.AddComponent<TypeFieldCell>();

                                            ContactInfo Info = new ContactInfo();
                                            Info.Name = Managed.name;
                                            Info.Type = identifiers[54];
                                            Managed.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                            Managed.transform.SetParent(ScenesFolder);
                                        }//End;

                                        ///Adding the SharedScene
                                        {
                                            GameObject level = new GameObject(assetsFileName + ".unity3d");
                                            level.AddComponent<TypeFieldCell>();

                                            ContactInfo Info = new ContactInfo();
                                            Info.Name = assetsFileName;
                                            Info.Type = identifiers[50];
                                            level.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                            level.transform.SetParent(ScenesFolder);
                                        }//End;

                                        ///Adding the Shared ref Scene
                                        {
                                            GameObject level = new GameObject(levelName + ".unity3d");
                                            level.AddComponent<TypeFieldCell>();

                                            ContactInfo Info = new ContactInfo();
                                            Info.Name = levelName;
                                            Info.Type = identifiers[50];
                                            level.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                            level.transform.SetParent(ScenesFolder);
                                        }//End;
                                    }

                                    if (assetsFileName.EndsWith(".assets") && assetsFileName.Contains("sharedassets"))
                                    {
                                        ///Creating a Scene Folder
                                        if (!UAEFileRoot.Find("Bundles").Find("Scenes"))
                                        {
                                            Transform Bundles = UAEFileRoot.Find("Bundles");
                                            GameObject Scenes = new GameObject("Scenes");
                                            Scenes.AddComponent<TypeFieldCell>();

                                            ContactInfo Info = new ContactInfo();
                                            Info.Name = Bundles.name;
                                            Info.Type = identifiers[54];
                                            Scenes.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                            Scenes.transform.SetParent(Bundles);
                                        }//End;

                                        //Getting the Scene Folder;
                                        Transform ScenesFolder = UAEFileRoot.Find("Bundles").Find("Scenes");


                                        ///Adding Managed to Scene
                                        if (!UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed"))
                                        {
                                            GameObject Managed = new GameObject("Managed");
                                            Managed.AddComponent<TypeFieldCell>();

                                            ContactInfo Info = new ContactInfo();
                                            Info.Name = Managed.name;
                                            Info.Type = identifiers[54];
                                            Managed.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                            Managed.transform.SetParent(ScenesFolder);
                                        }//End;

                                        ///Adding the SharedScene
                                        {
                                            GameObject level = new GameObject(assetsFileName + ".unity3d");
                                            level.AddComponent<TypeFieldCell>();

                                            ContactInfo Info = new ContactInfo();
                                            Info.Name = assetsFileName;
                                            Info.Type = identifiers[50];
                                            level.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                            level.transform.SetParent(ScenesFolder);
                                        }//End;
                                    }


                                    if (assetsFileName.Contains("level"))
                                    {

                                        ///Creating a Scene Folder
                                        if (!UAEFileRoot.Find("Bundles").Find("Scenes"))
                                        {
                                            Transform Bundles = UAEFileRoot.Find("Bundles");
                                            GameObject Scenes = new GameObject("Scenes");
                                            Scenes.AddComponent<TypeFieldCell>();

                                            ContactInfo Info = new ContactInfo();
                                            Info.Name = Bundles.name;
                                            Info.Type = identifiers[54];
                                            Scenes.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                            Scenes.transform.SetParent(Bundles);
                                        }//End;

                                        //Getting the Scene Folder;
                                        Transform ScenesFolder = UAEFileRoot.Find("Bundles").Find("Scenes");


                                        ///Adding Managed to Scene
                                        if (!UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed"))
                                        {
                                            GameObject Managed = new GameObject("Managed");
                                            Managed.AddComponent<TypeFieldCell>();

                                            ContactInfo Info = new ContactInfo();
                                            Info.Name = Managed.name;
                                            Info.Type = identifiers[54];
                                            Managed.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                            Managed.transform.SetParent(ScenesFolder);
                                        }//End;

                                        ///Adding The Scene
                                        {
                                            GameObject level = new GameObject(assetsFileName + ".unity3d");
                                            level.AddComponent<TypeFieldCell>();

                                            ContactInfo Info = new ContactInfo();
                                            Info.Name = assetsFileName;
                                            Info.Type = identifiers[50];
                                            level.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Info);

                                            level.transform.SetParent(ScenesFolder);
                                        }//End;
                                    }



                                    Debug.Log("definite");

                                    if (!assetsFileName.EndsWith(".sharedAssets") && assetsFileName != levelName && !assetsFileName.EndsWith(".resource") && !assetsFileName.EndsWith(".resS") && !assetsFileName.Contains("sharedassets") && !assetsFileName.Contains("level"))
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

                                        Debug.Log("it reached");
                                        foreach (var Info in afile.Metadata.AssetInfos)
                                        {
                                            yield return null;
                                            if (Info.TypeId == (int)AssetClassID.GameObject)
                                            {
                                                if (!UAEFileRoot.Find("Bundles").Find("G'O RX"))
                                                {
                                                    GameObject GoRX = new GameObject("G'O RX");
                                                    GoRX.AddComponent<TypeFieldCell>();

                                                    ContactInfo Infos = new ContactInfo();
                                                    Infos.Name = GoRX.name;
                                                    Infos.Type = identifiers[54];
                                                    GoRX.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Infos);

                                                    GoRX.transform.SetParent(UAEFileRoot.Find("Bundles"));
                                                }

                                                Transform GORX = UAEFileRoot.Find("Bundles").Find("G'O RX");

                                                string name = "F";
                                                AssetTypeValueField BaseField = new AssetTypeValueField();
                                                try
                                                {
                                                    BaseField = manager.GetBaseField(afileInst, Info);

                                                    AssetClassID id = new AssetClassID();
                                                    id = (AssetClassID)Info.TypeId;

                                                    if (BaseField["m_Name"].IsDummy == true)
                                                    {
                                                        name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                    else
                                                    {
                                                        name = BaseField["m_Name"].AsString;
                                                        if (name == null || name == "")
                                                        {
                                                            name = id.ToString() + " #" + Info.PathId.ToString();
                                                        }
                                                    }
                                                    LoadingAssetText.text = "Loading Asset:- " + name;
                                                    // #Transform

                                                    AssetTypeValueField data = BaseField["m_Component.Array"][0];
                                                    AssetPPtr pptr = AssetPPtr.FromField(data["component"]);


                                                    AssetTypeValueField Transform = new AssetTypeValueField();

                                                    if (pptr.FileId == 0)
                                                    {
                                                        Transform = manager.GetBaseField(afileInst, pptr.PathId);
                                                    }
                                                    AssetPPtr pptr2 = AssetPPtr.FromField(Transform["m_Father"]);

                                                    if (pptr2.FileId == 0 && pptr2.PathId == 0)
                                                    {

                                                        ContactInfo contactInfo = new ContactInfo();

                                                        contactInfo.Name = name;

                                                        contactInfo.Type = SetClassIDIdentifier(id);

                                                        contactInfo.parentPath = afileInst.name;

                                                        contactInfo.BundlePath = path;

                                                        contactInfo.pathID = Info.PathId;

                                                        contactInfo.refPath = "/" + contactInfo.Name;

                                                        contactInfo.UnknownAsset = false;

                                                        contactInfo.TypeID = id;

                                                        GameObject Entity = new GameObject(contactInfo.Name);
                                                        Entity.AddComponent<TypeFieldCell>();


                                                        Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(contactInfo);

                                                        Entity.transform.SetParent(GORX);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {

                                                    if (Inform == "")
                                                    {
                                                        Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                    else
                                                    {
                                                        Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                }
                                                yield return null;
                                            }

                                            //Managed
                                            {
                                                AssetClassID ids = new AssetClassID();
                                                ids = (AssetClassID)Info.TypeId;
                                                if (!UAEFileRoot.Find("Bundles").Find("Managed").Find(ids.ToString()))
                                                {
                                                    GameObject Entity = new GameObject(ids.ToString());
                                                    Entity.AddComponent<TypeFieldCell>();

                                                    ContactInfo Infos = new ContactInfo();
                                                    Infos.Name = Entity.name;
                                                    Infos.Type = identifiers[54];
                                                    Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Infos);

                                                    Entity.transform.SetParent(UAEFileRoot.Find("Bundles").Find("Managed"));
                                                }

                                                Transform GEntity = UAEFileRoot.Find("Bundles").Find("Managed").Find(ids.ToString());


                                                AssetTypeValueField BaseField = new AssetTypeValueField();

                                                string Name = "F";
                                                try
                                                {
                                                    BaseField = manager.GetBaseField(afileInst, Info);

                                                    if (BaseField["m_Name"].IsDummy == true)
                                                    {
                                                        AssetClassID id = (AssetClassID)Info.TypeId;
                                                        Name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                    else
                                                    {
                                                        Name = BaseField["m_Name"].AsString;
                                                        if (Name == null || Name == "")
                                                        {
                                                            AssetClassID id = (AssetClassID)Info.TypeId;
                                                            Name = id.ToString() + " #" + Info.PathId.ToString();
                                                        }
                                                    }
                                                    LoadingAssetText.text = "Loading Asset:- " + Name;
                                                    Debug.Log(name);

                                                    ContactInfo info = new ContactInfo();
                                                    if (Info.TypeId == (int)AssetClassID.MonoScript)
                                                    {
                                                        info.Name = Name + ".cs";
                                                        info.refPath = "/" + Name + ".cs";
                                                    }
                                                    else
                                                    {
                                                        info.Name = Name;
                                                        info.refPath = "/" + Name;
                                                    }
                                                    info.parentPath = afileInst.name;
                                                    info.BundlePath = path;
                                                    info.pathID = Info.PathId;
                                                    info.Type = SetClassIDIdentifier((AssetClassID)Info.TypeId);
                                                    info.TypeID = (AssetClassID)Info.TypeId;
                                                    info.UnknownAsset = false;
                                                    //info.FileID = afileInst.file.

                                                    GameObject Entity = new GameObject(info.Name);
                                                    Entity.AddComponent<TypeFieldCell>();


                                                    Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(info);

                                                    Entity.transform.SetParent(GEntity);
                                                }
                                                catch (Exception ex)
                                                {

                                                    if (Inform == "")
                                                    {
                                                        Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                    else
                                                    {
                                                        Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                }
                                                yield return null;
                                            }
                                            //UnManaged
                                            {
                                                Transform GEntity = UAEFileRoot.Find("Bundles").Find("UnManaged");


                                                AssetTypeValueField BaseField = new AssetTypeValueField();

                                                string Name = "F";
                                                try
                                                {
                                                    BaseField = manager.GetBaseField(afileInst, Info);

                                                    if (BaseField["m_Name"].IsDummy == true)
                                                    {
                                                        AssetClassID id = (AssetClassID)Info.TypeId;
                                                        Name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                    else
                                                    {
                                                        Name = BaseField["m_Name"].AsString;
                                                        if (Name == null || Name == "")
                                                        {
                                                            AssetClassID id = (AssetClassID)Info.TypeId;
                                                            Name = id.ToString() + " #" + Info.PathId.ToString();
                                                        }
                                                    }
                                                    LoadingAssetText.text = "Loading Asset:- " + Name;
                                                    Debug.Log(name);

                                                    ContactInfo info = new ContactInfo();
                                                    if (Info.TypeId == (int)AssetClassID.MonoScript)
                                                    {
                                                        info.Name = Name + ".cs";
                                                        info.refPath = "/" + Name + ".cs";
                                                    }
                                                    else
                                                    {
                                                        info.Name = Name;
                                                        info.refPath = "/" + Name;
                                                    }
                                                    info.parentPath = afileInst.name;
                                                    info.BundlePath = path;
                                                    info.pathID = Info.PathId;
                                                    info.Type = SetClassIDIdentifier((AssetClassID)Info.TypeId);
                                                    info.TypeID = (AssetClassID)Info.TypeId;
                                                    info.UnknownAsset = false;
                                                    //info.FileID = afileInst.file.

                                                    GameObject Entity = new GameObject(info.Name);
                                                    Entity.AddComponent<TypeFieldCell>();


                                                    Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(info);

                                                    Entity.transform.SetParent(GEntity);
                                                }
                                                catch (Exception ex)
                                                {

                                                    if (Inform == "")
                                                    {
                                                        Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                    else
                                                    {
                                                        Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                }
                                                yield return null;
                                            }
                                        }
                                    }
                                    if (assetsFileName.Contains("level"))
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

                                        foreach (var Info in afile.Metadata.AssetInfos)
                                        {
                                            yield return null;
                                            if (Info.TypeId == (int)AssetClassID.GameObject)
                                            {
                                                Transform GEntity = UAEFileRoot.Find("Bundles").Find("Scenes").Find(assetsFileName + ".unity3d");

                                                string name = "F";
                                                AssetTypeValueField BaseField = new AssetTypeValueField();
                                                try
                                                {
                                                    BaseField = manager.GetBaseField(afileInst, Info);

                                                    AssetClassID id = new AssetClassID();
                                                    id = (AssetClassID)Info.TypeId;

                                                    if (BaseField["m_Name"].IsDummy == true)
                                                    {
                                                        name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                    else
                                                    {
                                                        name = BaseField["m_Name"].AsString;
                                                        if (name == null || name == "")
                                                        {
                                                            name = id.ToString() + " #" + Info.PathId.ToString();
                                                        }
                                                    }
                                                    LoadingAssetText.text = "Loading Asset:- " + name;
                                                    // #Transform

                                                    AssetTypeValueField data = BaseField["m_Component.Array"][0];
                                                    AssetPPtr pptr = AssetPPtr.FromField(data["component"]);


                                                    AssetTypeValueField Transform = new AssetTypeValueField();

                                                    if (pptr.FileId == 0)
                                                    {
                                                        Transform = manager.GetBaseField(afileInst, pptr.PathId);
                                                    }
                                                    AssetPPtr pptr2 = AssetPPtr.FromField(Transform["m_Father"]);

                                                    if (pptr2.FileId == 0 && pptr2.PathId == 0)
                                                    {

                                                        ContactInfo contactInfo = new ContactInfo();

                                                        contactInfo.Name = name;

                                                        contactInfo.Type = SetClassIDIdentifier(AssetClassID.Special);

                                                        contactInfo.parentPath = assetsFileName;

                                                        contactInfo.BundlePath = path;

                                                        contactInfo.pathID = Info.PathId;

                                                        contactInfo.refPath = "/" + contactInfo.Name;

                                                        contactInfo.UnknownAsset = false;

                                                        contactInfo.TypeID = id;

                                                        GameObject Entity = new GameObject(contactInfo.Name);
                                                        Entity.AddComponent<TypeFieldCell>();


                                                        Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(contactInfo);

                                                        Entity.transform.SetParent(GEntity);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {

                                                    if (Inform == "")
                                                    {
                                                        Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                    else
                                                    {
                                                        Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                }
                                                yield return null;
                                            }

                                            {
                                                AssetClassID ids = new AssetClassID();
                                                ids = (AssetClassID)Info.TypeId;
                                                if (!UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed").Find(ids.ToString()))
                                                {
                                                    GameObject Entity = new GameObject(ids.ToString());
                                                    Entity.AddComponent<TypeFieldCell>();

                                                    ContactInfo Infos = new ContactInfo();
                                                    Infos.Name = Entity.name;
                                                    Infos.Type = identifiers[54];
                                                    Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Infos);

                                                    Entity.transform.SetParent(UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed"));
                                                }


                                                Transform GEntity = UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed").Find(ids.ToString());


                                                AssetTypeValueField BaseField = new AssetTypeValueField();

                                                string Name = "F";
                                                try
                                                {
                                                    BaseField = manager.GetBaseField(afileInst, Info);

                                                    if (BaseField["m_Name"].IsDummy == true)
                                                    {
                                                        AssetClassID id = (AssetClassID)Info.TypeId;
                                                        Name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                    else
                                                    {
                                                        Name = BaseField["m_Name"].AsString;
                                                        if (Name == null || Name == "")
                                                        {
                                                            AssetClassID id = (AssetClassID)Info.TypeId;
                                                            Name = id.ToString() + " #" + Info.PathId.ToString();
                                                        }
                                                    }
                                                    LoadingAssetText.text = "Loading Asset:- " + Name;
                                                    Debug.Log(name);

                                                    ContactInfo info = new ContactInfo();
                                                    if (Info.TypeId == (int)AssetClassID.MonoScript)
                                                    {
                                                        info.Name = Name + ".cs";
                                                        info.refPath = "/" + Name + ".cs";
                                                    }
                                                    else
                                                    {
                                                        info.Name = Name;
                                                        info.refPath = "/" + Name;
                                                    }
                                                    info.parentPath = path;
                                                    info.BundlePath = "";
                                                    info.pathID = Info.PathId;
                                                    info.Type = SetClassIDIdentifier((AssetClassID)Info.TypeId);
                                                    info.TypeID = (AssetClassID)Info.TypeId;
                                                    info.UnknownAsset = false;
                                                    //info.FileID = afileInst.file.

                                                    GameObject Entity = new GameObject(info.Name);
                                                    Entity.AddComponent<TypeFieldCell>();


                                                    Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(info);

                                                    Entity.transform.SetParent(GEntity);
                                                }
                                                catch (Exception ex)
                                                {

                                                    if (Inform == "")
                                                    {
                                                        Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                    else
                                                    {
                                                        Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                }
                                                yield return null;
                                            }
                                        }
                                    }
                                    if (levelName != "")
                                    {
                                        {
                                            {
                                                var afileInst = manager.LoadAssetsFileFromBundle(bunfileInst, levelName, false);

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

                                                foreach (var Info in afile.Metadata.AssetInfos)
                                                {
                                                    yield return null;
                                                    if (Info.TypeId == (int)AssetClassID.GameObject)
                                                    {
                                                        Transform GEntity = UAEFileRoot.Find("Bundles").Find("Scenes").Find(levelName + ".unity3d");


                                                        string name = "F";
                                                        AssetTypeValueField BaseField = new AssetTypeValueField();
                                                        try
                                                        {
                                                            BaseField = manager.GetBaseField(afileInst, Info);

                                                            AssetClassID id = new AssetClassID();
                                                            id = (AssetClassID)Info.TypeId;

                                                            if (BaseField["m_Name"].IsDummy == true)
                                                            {
                                                                name = id.ToString() + " #" + Info.PathId.ToString();
                                                            }
                                                            else
                                                            {
                                                                name = BaseField["m_Name"].AsString;
                                                                if (name == null || name == "")
                                                                {
                                                                    name = id.ToString() + " #" + Info.PathId.ToString();
                                                                }
                                                            }
                                                            LoadingAssetText.text = "Loading Asset:- " + name;
                                                            // #Transform

                                                            AssetTypeValueField data = BaseField["m_Component.Array"][0];
                                                            AssetPPtr pptr = AssetPPtr.FromField(data["component"]);


                                                            AssetTypeValueField Transform = new AssetTypeValueField();

                                                            if (pptr.FileId == 0)
                                                            {
                                                                Transform = manager.GetBaseField(afileInst, pptr.PathId);
                                                            }
                                                            AssetPPtr pptr2 = AssetPPtr.FromField(Transform["m_Father"]);

                                                            if (pptr2.FileId == 0 && pptr2.PathId == 0)
                                                            {

                                                                ContactInfo contactInfo = new ContactInfo();

                                                                contactInfo.Name = name;

                                                                contactInfo.Type = SetClassIDIdentifier(id);

                                                                contactInfo.parentPath = levelName;

                                                                contactInfo.BundlePath = path;

                                                                contactInfo.pathID = Info.PathId;

                                                                contactInfo.refPath = "/" + contactInfo.Name;

                                                                contactInfo.UnknownAsset = false;

                                                                contactInfo.TypeID = id;


                                                                GameObject Entity = new GameObject(contactInfo.Name);
                                                                Entity.AddComponent<TypeFieldCell>();


                                                                Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(contactInfo);

                                                                Entity.transform.SetParent(GEntity);
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {

                                                            if (Inform == "")
                                                            {
                                                                Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                                "\nThe PathID on which it fails:- " + Info.PathId +
                                                                "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                                "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                            }
                                                            else
                                                            {
                                                                Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                                "\nThe PathID on which it fails:- " + Info.PathId +
                                                                "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                                "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                            }
                                                        }
                                                        yield return null;
                                                    }

                                                    {
                                                        AssetClassID ids = new AssetClassID();
                                                        ids = (AssetClassID)Info.TypeId;
                                                        if (!UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed").Find(ids.ToString()))
                                                        {
                                                            GameObject Entity = new GameObject(ids.ToString());
                                                            Entity.AddComponent<TypeFieldCell>();

                                                            ContactInfo Infos = new ContactInfo();
                                                            Infos.Name = Entity.name;
                                                            Infos.Type = identifiers[54];
                                                            Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Infos);

                                                            Entity.transform.SetParent(UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed"));
                                                        }
                                                        Transform GEntity = UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed").Find(ids.ToString());

                                                        AssetTypeValueField BaseField = new AssetTypeValueField();

                                                        string Name = "F";
                                                        try
                                                        {
                                                            BaseField = manager.GetBaseField(afileInst, Info);

                                                            if (BaseField["m_Name"].IsDummy == true)
                                                            {
                                                                AssetClassID id = (AssetClassID)Info.TypeId;
                                                                Name = id.ToString() + " #" + Info.PathId.ToString();
                                                            }
                                                            else
                                                            {
                                                                Name = BaseField["m_Name"].AsString;
                                                                if (Name == null || Name == "")
                                                                {
                                                                    AssetClassID id = (AssetClassID)Info.TypeId;
                                                                    Name = id.ToString() + " #" + Info.PathId.ToString();
                                                                }
                                                            }
                                                            LoadingAssetText.text = "Loading Asset:- " + Name;
                                                            Debug.Log(name);

                                                            ContactInfo info = new ContactInfo();
                                                            if (Info.TypeId == (int)AssetClassID.MonoScript)
                                                            {
                                                                info.Name = Name + ".cs";
                                                                info.refPath = "/" + Name + ".cs";
                                                            }
                                                            else
                                                            {
                                                                info.Name = Name;
                                                                info.refPath = "/" + Name;
                                                            }
                                                            info.parentPath = levelName;
                                                            info.BundlePath = path;
                                                            info.pathID = Info.PathId;
                                                            info.Type = SetClassIDIdentifier((AssetClassID)Info.TypeId);
                                                            info.TypeID = (AssetClassID)Info.TypeId;
                                                            info.UnknownAsset = false;
                                                            //info.FileID = afileInst.file.


                                                            GameObject Entity = new GameObject(info.Name);
                                                            Entity.AddComponent<TypeFieldCell>();


                                                            Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(info);

                                                            Entity.transform.SetParent(GEntity);
                                                        }
                                                        catch (Exception ex)
                                                        {

                                                            if (Inform == "")
                                                            {
                                                                Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                                "\nThe PathID on which it fails:- " + Info.PathId +
                                                                "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                                "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                            }
                                                            else
                                                            {
                                                                Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                                "\nThe PathID on which it fails:- " + Info.PathId +
                                                                "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                                "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                            }
                                                        }
                                                        yield return null;
                                                    }
                                                }
                                            }
                                        }
                                        levelName = "";
                                    }
                                    if (assetsFileName.EndsWith(".sharedAssets"))
                                    {
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

                                            foreach (var Info in afile.Metadata.AssetInfos)
                                            {
                                                yield return null;
                                                if (Info.TypeId == (int)AssetClassID.GameObject)
                                                {
                                                    Transform GEntity = UAEFileRoot.Find("Bundles").Find("Scenes").Find(assetsFileName + ".unity3d");

                                                    string name = "F";
                                                    AssetTypeValueField BaseField = new AssetTypeValueField();
                                                    try
                                                    {
                                                        BaseField = manager.GetBaseField(afileInst, Info);

                                                        AssetClassID id = new AssetClassID();
                                                        id = (AssetClassID)Info.TypeId;

                                                        if (BaseField["m_Name"].IsDummy == true)
                                                        {
                                                            name = id.ToString() + " #" + Info.PathId.ToString();
                                                        }
                                                        else
                                                        {
                                                            name = BaseField["m_Name"].AsString;
                                                            if (name == null || name == "")
                                                            {
                                                                name = id.ToString() + " #" + Info.PathId.ToString();
                                                            }
                                                        }
                                                        LoadingAssetText.text = "Loading Asset:- " + name;
                                                        // #Transform

                                                        AssetTypeValueField data = BaseField["m_Component.Array"][0];
                                                        AssetPPtr pptr = AssetPPtr.FromField(data["component"]);


                                                        AssetTypeValueField Transform = new AssetTypeValueField();

                                                        if (pptr.FileId == 0)
                                                        {
                                                            Transform = manager.GetBaseField(afileInst, pptr.PathId);
                                                        }
                                                        AssetPPtr pptr2 = AssetPPtr.FromField(Transform["m_Father"]);

                                                        if (pptr2.FileId == 0 && pptr2.PathId == 0)
                                                        {

                                                            ContactInfo contactInfo = new ContactInfo();

                                                            contactInfo.Name = name;

                                                            contactInfo.Type = SetClassIDIdentifier(id);

                                                            contactInfo.parentPath = assetsFileName;

                                                            contactInfo.BundlePath = path;

                                                            contactInfo.pathID = Info.PathId;

                                                            contactInfo.refPath = "/" + contactInfo.Name;

                                                            contactInfo.UnknownAsset = false;

                                                            contactInfo.TypeID = id;

                                                            GameObject Entity = new GameObject(contactInfo.Name);
                                                            Entity.AddComponent<TypeFieldCell>();


                                                            Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(contactInfo);

                                                            Entity.transform.SetParent(GEntity);
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                        if (Inform == "")
                                                        {
                                                            Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                            "\nThe PathID on which it fails:- " + Info.PathId +
                                                            "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                            "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                        }
                                                        else
                                                        {
                                                            Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                            "\nThe PathID on which it fails:- " + Info.PathId +
                                                            "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                            "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                        }
                                                    }
                                                    yield return null;
                                                }

                                                //Managed
                                                {
                                                    AssetClassID ids = new AssetClassID();
                                                    ids = (AssetClassID)Info.TypeId;
                                                    if (!UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed").Find(ids.ToString()))
                                                    {
                                                        GameObject Entity = new GameObject(ids.ToString());
                                                        Entity.AddComponent<TypeFieldCell>();

                                                        ContactInfo Infos = new ContactInfo();
                                                        Infos.Name = Entity.name;
                                                        Infos.Type = identifiers[54];
                                                        Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Infos);

                                                        Entity.transform.SetParent(UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed"));
                                                    }
                                                    Transform GEntity = UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed").Find(ids.ToString());


                                                    AssetTypeValueField BaseField = new AssetTypeValueField();

                                                    string Name = "F";
                                                    try
                                                    {
                                                        BaseField = manager.GetBaseField(afileInst, Info);

                                                        if (BaseField["m_Name"].IsDummy == true)
                                                        {
                                                            AssetClassID id = (AssetClassID)Info.TypeId;
                                                            Name = id.ToString() + " #" + Info.PathId.ToString();
                                                        }
                                                        else
                                                        {
                                                            Name = BaseField["m_Name"].AsString;
                                                            if (Name == null || Name == "")
                                                            {
                                                                AssetClassID id = (AssetClassID)Info.TypeId;
                                                                Name = id.ToString() + " #" + Info.PathId.ToString();
                                                            }
                                                        }
                                                        LoadingAssetText.text = "Loading Asset:- " + Name;
                                                        Debug.Log(name);

                                                        ContactInfo info = new ContactInfo();
                                                        if (Info.TypeId == (int)AssetClassID.MonoScript)
                                                        {
                                                            info.Name = Name + ".cs";
                                                            info.refPath = "/" + Name + ".cs";
                                                        }
                                                        else
                                                        {
                                                            info.Name = Name;
                                                            info.refPath = "/" + Name;
                                                        }
                                                        info.parentPath = assetsFileName;
                                                        info.BundlePath = path;
                                                        info.pathID = Info.PathId;
                                                        info.Type = SetClassIDIdentifier((AssetClassID)Info.TypeId);
                                                        info.TypeID = (AssetClassID)Info.TypeId;
                                                        info.UnknownAsset = false;
                                                        //info.FileID = afileInst.file.


                                                        GameObject Entity = new GameObject(info.Name);
                                                        Entity.AddComponent<TypeFieldCell>();


                                                        Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(info);

                                                        Entity.transform.SetParent(GEntity);
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                        if (Inform == "")
                                                        {
                                                            Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                            "\nThe PathID on which it fails:- " + Info.PathId +
                                                            "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                            "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                        }
                                                        else
                                                        {
                                                            Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                            "\nThe PathID on which it fails:- " + Info.PathId +
                                                            "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                            "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                        }
                                                    }
                                                    yield return null;
                                                }
                                            }
                                        }
                                    }
                                    if (assetsFileName.Contains("sharedassets") && assetsFileName.EndsWith(".assets"))
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

                                        foreach (var Info in afile.Metadata.AssetInfos)
                                        {
                                            yield return null;
                                            if (Info.TypeId == (int)AssetClassID.GameObject)
                                            {
                                                Transform GEntity = UAEFileRoot.Find("Bundles").Find("Scenes").Find(assetsFileName + ".unity3d");

                                                string name = "F";
                                                AssetTypeValueField BaseField = new AssetTypeValueField();
                                                try
                                                {
                                                    BaseField = manager.GetBaseField(afileInst, Info);

                                                    AssetClassID id = new AssetClassID();
                                                    id = (AssetClassID)Info.TypeId;

                                                    if (BaseField["m_Name"].IsDummy == true)
                                                    {
                                                        name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                    else
                                                    {
                                                        name = BaseField["m_Name"].AsString;
                                                        if (name == null || name == "")
                                                        {
                                                            name = id.ToString() + " #" + Info.PathId.ToString();
                                                        }
                                                    }
                                                    LoadingAssetText.text = "Loading Asset:- " + name;
                                                    // #Transform

                                                    AssetTypeValueField data = BaseField["m_Component.Array"][0];
                                                    AssetPPtr pptr = AssetPPtr.FromField(data["component"]);


                                                    AssetTypeValueField Transform = new AssetTypeValueField();

                                                    if (pptr.FileId == 0)
                                                    {
                                                        Transform = manager.GetBaseField(afileInst, pptr.PathId);
                                                    }
                                                    AssetPPtr pptr2 = AssetPPtr.FromField(Transform["m_Father"]);

                                                    if (pptr2.FileId == 0 && pptr2.PathId == 0)
                                                    {

                                                        ContactInfo contactInfo = new ContactInfo();

                                                        contactInfo.Name = name;

                                                        contactInfo.Type = SetClassIDIdentifier(id);

                                                        contactInfo.parentPath = assetsFileName;

                                                        contactInfo.BundlePath = path;

                                                        contactInfo.pathID = Info.PathId;

                                                        contactInfo.refPath = "/" + contactInfo.Name;

                                                        contactInfo.UnknownAsset = false;

                                                        contactInfo.TypeID = id;

                                                        GameObject Entity = new GameObject(contactInfo.Name);
                                                        Entity.AddComponent<TypeFieldCell>();


                                                        Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(contactInfo);

                                                        Entity.transform.SetParent(GEntity);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {

                                                    if (Inform == "")
                                                    {
                                                        Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                    else
                                                    {
                                                        Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                }
                                                yield return null;
                                            }

                                            //Managed
                                            {
                                                AssetClassID ids = new AssetClassID();
                                                ids = (AssetClassID)Info.TypeId;
                                                if (!UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed").Find(ids.ToString()))
                                                {
                                                    GameObject Entity = new GameObject(ids.ToString());
                                                    Entity.AddComponent<TypeFieldCell>();

                                                    ContactInfo Infos = new ContactInfo();
                                                    Infos.Name = Entity.name;
                                                    Infos.Type = identifiers[54];
                                                    Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(Infos);

                                                    Entity.transform.SetParent(UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed"));
                                                }
                                                Transform GEntity = UAEFileRoot.Find("Bundles").Find("Scenes").Find("Managed").Find(ids.ToString());

                                                AssetTypeValueField BaseField = new AssetTypeValueField();

                                                string Name = "F";
                                                try
                                                {
                                                    BaseField = manager.GetBaseField(afileInst, Info);

                                                    if (BaseField["m_Name"].IsDummy == true)
                                                    {
                                                        AssetClassID id = (AssetClassID)Info.TypeId;
                                                        Name = id.ToString() + " #" + Info.PathId.ToString();
                                                    }
                                                    else
                                                    {
                                                        Name = BaseField["m_Name"].AsString;
                                                        if (Name == null || Name == "")
                                                        {
                                                            AssetClassID id = (AssetClassID)Info.TypeId;
                                                            Name = id.ToString() + " #" + Info.PathId.ToString();
                                                        }
                                                    }
                                                    LoadingAssetText.text = "Loading Asset:- " + Name;
                                                    Debug.Log(name);

                                                    ContactInfo info = new ContactInfo();
                                                    if (Info.TypeId == (int)AssetClassID.MonoScript)
                                                    {
                                                        info.Name = Name + ".cs";
                                                        info.refPath = "/" + Name + ".cs";
                                                    }
                                                    else
                                                    {
                                                        info.Name = Name;
                                                        info.refPath = "/" + Name;
                                                    }
                                                    info.parentPath = assetsFileName;
                                                    info.BundlePath = path;
                                                    info.pathID = Info.PathId;
                                                    info.Type = SetClassIDIdentifier((AssetClassID)Info.TypeId);
                                                    info.TypeID = (AssetClassID)Info.TypeId;
                                                    info.UnknownAsset = false;
                                                    //info.FileID = afileInst.file.

                                                    GameObject Entity = new GameObject(info.Name);
                                                    Entity.AddComponent<TypeFieldCell>();


                                                    Entity.GetComponent<TypeFieldCell>().ConfigueHierarchyCell(info);

                                                    Entity.transform.SetParent(GEntity);
                                                }
                                                catch (Exception ex)
                                                {

                                                    if (Inform == "")
                                                    {
                                                        Inform = ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                    else
                                                    {
                                                        Inform = Inform + "\n \n \n \n" + ("The TypeID on which it fails is:- " + Info.TypeId.ToString() +
                                                        "\nThe PathID on which it fails:- " + Info.PathId +
                                                        "\nThe File Loaded is:- " + Path.GetFileName(path) +
                                                        "\nThe Current failed assetfile in case of bundle is:- " + assetsFileName);
                                                    }
                                                }
                                                yield return null;
                                            }
                                        }
                                    }
                                }
                                levelName = "";
                                manager.UnloadAll();
                            }
                        }
                        count = count + 1;
                    }
                    else
                    {
                        if (result.Length == 1)
                        {
                            DefameData();
                            ComplainError("Selected File is neither a Bundle nor an Asset file.\nPlease make sure you use the correct file.");
                        }
                    }
                }
            }
            if (count == 0)
            {
                DefameData();
                ComplainError("Selected Files and Binaries are neither a Bundle nor an Asset file.\nPlease make sure you use the correct files.");
            }

            if (Inform == "" && count != 0)
            {
                ComplainError("Files Loaded Successfully....");
            }
            else if (count != 0)
            {
                ComplainError(Inform);
            }
            LoadingBar.SetActive(false);
            InitializeData();
        }
    }
}
public class AFileValuePair
{
    public AssetsManager Manager;
    public BundleFileInstance BFInst;
    public AssetsFileInstance AFinst;
    public AssetTypeValueField Basefield;
}


/*if (Info.TypeId == (int)AssetClassID.GameObject)
{
    var count =BaseField["m_Component.Array"].AsArray.size;



    for (int i = 0; i < count; i++)
    {
        AssetTypeValueField data = BaseField["m_Component.Array"][i];

        AssetPPtr pptr = AssetPPtr.FromField(data["component"]);
        pptr.SetFilePathFromFile(manager, afileInst);
        Debug.Log(pptr.FilePath);
    }
}*/