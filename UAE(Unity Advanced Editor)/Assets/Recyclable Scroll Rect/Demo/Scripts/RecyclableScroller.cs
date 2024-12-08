using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;
using AssetsTools.NET.Extra;

/// <summary>
/// Demo controller class for Recyclable Scroll Rect. 
/// A controller class is responsible for providing the scroll rect with datasource. Any class can be a controller class. 
/// The only requirement is to inherit from IRecyclableScrollRectDataSource and implement the interface methods
/// </summary>

//Dummy Data model for demostraion
public struct ContactInfo
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

public class RecyclableScroller : MonoBehaviour, IRecyclableScrollRectDataSource
{
    [SerializeField]
    RecyclableScrollRect _recyclableScrollRect;

    //Dummy data List
    private List<ContactInfo> _contactList = new List<ContactInfo>();

    //Recyclable scroll rect's data source must be assigned in Awake.

    /*private void Awake()
    {
        ContactInfo info = new ContactInfo();
        _contactList.Add(info);
        _recyclableScrollRect.DataSource = this;
    }*/
    public void InitializeData(List<ContactInfo> list)
    {
        _contactList = list;
    }
    public void DisplayData()
    {
        //_recyclableScrollRect.DataSource = this;
        _recyclableScrollRect.Initialize(this);
    }
    public void RemoveData()
    {
        _recyclableScrollRect.DataSource = null;
        _recyclableScrollRect.Initialize(null);
    }

    #region DATA-SOURCE

    /// <summary>
    /// Data source method. return the list length.
    /// </summary>
    public int GetItemCount()
    {
        return _contactList.Count;
    }

    /// <summary>
    /// Data source method. Called for a cell every time it is recycled.
    /// Implement this method to do the necessary cell configuration.
    /// </summary>
    public void SetCell(ICell cell, int index)
    {
        //Casting to the implemented Cell
        var item = cell as TypeFieldCell;
        item.ConfigureCell(_contactList[index], index);
    }

    #endregion
}