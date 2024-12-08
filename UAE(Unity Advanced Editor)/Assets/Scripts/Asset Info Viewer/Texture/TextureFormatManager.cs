using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextureFormatManager : MonoBehaviour
{
    [SerializeField]
    private RecyclableScroller RectManager;
    [SerializeField]
    private List<ContactInfo> contactInfos = new List<ContactInfo>();

    public SelectedFieldInfo selectedField;

    public TextMeshProUGUI TextureFormat;
    void Start()
    {
        for (int i = 0; i < 71; i++)
        {
            UAE.Texture.TextureFormat Format = (UAE.Texture.TextureFormat)i;

            ContactInfo info = new ContactInfo();
            info.Replacer = false;
            info.formatter = true;

            info.Textureformat = Format.ToString();
            contactInfos.Add(info);
        }

        RectManager.InitializeData(contactInfos);

        RectManager.DisplayData();
    }

    public void SetTextureFormat()
    {
        TextureFormat.text = selectedField.cell._contactInfo.Textureformat;
    }

}
