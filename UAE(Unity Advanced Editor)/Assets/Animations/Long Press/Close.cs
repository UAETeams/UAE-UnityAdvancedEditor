using System.Collections;
using UnityEngine;

public class Close : MonoBehaviour
{
    public AnimationClip anim;
    public GameObject longpress;
    public void close()
    {
        StartCoroutine(closecoro());
    }
    IEnumerator closecoro()
    {
        yield return new WaitForSeconds(anim.length);
        longpress.SetActive(false);
    }
}
