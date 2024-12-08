using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ClassDataTpk : MonoBehaviour
{
    public TextAsset bytes;
    public bool ride;
    void Start()
    {
        if (!File.Exists(Path.Combine(Application.persistentDataPath, "classdata.tpk")) || ride == true)
        {
            File.WriteAllBytes(Path.Combine(Application.persistentDataPath, "classdata.tpk"), bytes.bytes);
        }
    }
}
