using System.Collections;
using UnityEngine;

public class coroutine : MonoBehaviour
{
    Color color = Color.blue;

    private void Start()
    {
        StartCoroutine(function());
    }
    IEnumerator function()
    {
        yield return null;
        for(int i = 0; i < 12;  i++)
        {
            color = Random.ColorHSV();
            this.gameObject.GetComponent<MeshRenderer>().material.color = color;
            Debug.Log(color);
            yield return new WaitForSeconds(1f);
        }    
    }
}
