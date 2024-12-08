using System.Collections;
using UnityEngine;

public class SceneClose : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject mainCanvas;
    public GameObject ViewPort;
    public GameObject HDRPAsset;

    public void Close()
    {
        StartCoroutine(OpenCanvas());

        StartCoroutine(OpenCamera());
    }
    IEnumerator OpenCamera()
    {
        yield return null;
        mainCamera.SetActive(true);
    }
    IEnumerator OpenCanvas()
    {
        yield return null;
        mainCanvas.SetActive(true);
    }
}
