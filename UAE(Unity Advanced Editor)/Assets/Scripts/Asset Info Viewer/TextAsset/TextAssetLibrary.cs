using AssetsTools.NET;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UAE.Text
{
    public class TextAssetLibrary
    {
        public string m_Name;
        public byte[] textassetdat;

        public TextAssetLibrary ReadTextAsset(AssetTypeValueField baseField)
        {
            TextAssetLibrary textasset = new TextAssetLibrary();
            m_Name = baseField["m_Name"].AsString;
            textassetdat = baseField["m_Script"].AsByteArray;
            return textasset;
        }
    }
}
