using AssetsTools.NET.Extra;
using UnityEngine;

public class CInfo : MonoBehaviour
{
    public UAEFieldInfo Info;
}
public struct UAEFieldInfo
{
    public string Name;
    public Texture2D Type;
    public long pathID;
    public string parentPath;
    public string BundlePath;
    public string refPath;
    public AssetClassID TypeID;
    public bool UnknownAsset;

    public byte[] replacedAssetBytes;
    public bool Replacer;
    public bool formatter;
    public string Textureformat;
}
