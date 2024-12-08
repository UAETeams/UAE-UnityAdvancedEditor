using System.Collections;
using UnityEngine;

public class NoBorderClick : MonoBehaviour
{
    //public Window scrt;
    public WindowScript script;
    void Start()
    {
        base.StartCoroutine(Dis());
        StartCoroutine(delayedNote());
        base.StartCoroutine(Siz());
    }
    IEnumerator delayedNote()
    {
        yield return new WaitForSeconds(5f);

        NativeWinAlert.Show("Started UAE. \nEnjoy Your Experience....",
    "Notice", "OK");
    }
    
    IEnumerator Dis()
    {
        yield return new WaitForEndOfFrame();
        script.OnNoBorderBtnClick();
    }
    IEnumerator Siz()
    {
        yield return new WaitForSeconds(.5f);
        script.ResetWindowSize();
    }
}
