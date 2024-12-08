using SimpleFileBrowser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ResourceLoader : MonoBehaviour
{
    public GameObject resourceWindow;

    public GameObject InAccessible;
    public Load loader;

    public TexViewer texViewer;
    public ViewAudio audioViewer;
    public ExportTex texExport;
    public ExportAudio audioExport;

    public MatViewer matViewer;

    public void load()
    {
        StartCoroutine(LoadResource());
    }
    public void close()
    {
        resourceWindow.SetActive(false);
    }
    public void closeBeforeHand()
    {
        resourceWindow.SetActive(false);
        if (texViewer.isCurrentFunction == true)
        {
            texViewer.isCurrentFunction = false;
        }
        else if (texExport.isCurrentFunction == true)
        {
            texExport.isCurrentFunction = false;
        }
        else if (audioViewer.isCurrentFunction == true)
        {
            audioViewer.isCurrentFunction = false;
        }
        else if (audioExport.isCurrentFunction == true)
        {
            audioExport.isCurrentFunction = false;
        }
        else if (matViewer.isCurrentFunction == true)
        {
            matViewer.isCurrentFunction = false;
        }
    }
    IEnumerator LoadResource()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, name, "Load Files and Folders", "Load");
        {
            try
            {
                close();
                byte[] resourcesbytes = File.ReadAllBytes(FileBrowser.Result[0]);
                if (texViewer.isCurrentFunction == true)
                {
                    texViewer.vewTex2dRest(resourcesbytes);
                }
                else if (texExport.isCurrentFunction == true)
                {
                    texExport.exportTex2dRest(resourcesbytes);
                }
                else if (audioViewer.isCurrentFunction == true)
                {
                    audioViewer.viewAudioRest(resourcesbytes);
                }
                else if(audioExport.isCurrentFunction == true)
                {
                    audioExport.ExportAudioRest(resourcesbytes);
                }
                else if(matViewer.isCurrentFunction == true)
                {
                    matViewer.viewMatRest(resourcesbytes);
                }
            }
            catch (Exception ex)
            {
                loader.ComplainError(ex.ToString());
            }
        }
    }
    public void NetLoadResource(string path)
    {
        try
        {
            InAccessible.SetActive(false);
            byte[] resourcesbytes = File.ReadAllBytes(path);
            if (texViewer.isCurrentFunction == true)
            {
                texViewer.vewTex2dRest(resourcesbytes);
            }
            else if (texExport.isCurrentFunction == true)
            {
                texExport.exportTex2dRest(resourcesbytes);
            }
            else if (audioViewer.isCurrentFunction == true)
            {
                audioViewer.viewAudioRest(resourcesbytes);
            }
            else if (audioExport.isCurrentFunction == true)
            {
                audioExport.ExportAudioRest(resourcesbytes);
            }
            else if (matViewer.isCurrentFunction == true)
            {
                matViewer.viewMatRest(resourcesbytes);
            }
        }
        catch (Exception ex)
        {
            loader.ComplainError(ex.ToString());
        }
    }
}
